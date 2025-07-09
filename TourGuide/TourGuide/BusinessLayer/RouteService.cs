using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.Globalization;
using System.IO;
using PuppeteerSharp;




namespace TourGuide.BusinessLayer
{
    public class RouteResult
    {
        public double DistanceKm { get; set; }
        public double DurationMinutes { get; set; }
        public List<(double lat, double lng)> Coordinates { get; set; } = new();
    }

    public class RouteService
    {
        private readonly string _apiKey;
        private readonly HttpClient _http;

        public RouteService()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _apiKey = config["ORS:ApiKey"] ?? throw new Exception("API key missing in config");
            _http = new HttpClient();
        }
        public async Task<RouteResult?> GetRouteAsync(double startLat, double startLng, double endLat, double endLng, string moveType)
        { 

            var url = $"https://api.openrouteservice.org/v2/directions/{moveType}" +
                      $"?start={startLng.ToString(CultureInfo.InvariantCulture)},{startLat.ToString(CultureInfo.InvariantCulture)}" +
                      $"&end={endLng.ToString(CultureInfo.InvariantCulture)},{endLat.ToString(CultureInfo.InvariantCulture)}";


            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.TryAddWithoutValidation("Authorization", _apiKey);

            var response = await _http.SendAsync(request);
            
            var responseText = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                string errContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Request failed: {(int)response.StatusCode}\n{errContent}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var summary = doc.RootElement
                .GetProperty("features")[0]
                .GetProperty("properties")
                .GetProperty("summary");

            var distance = summary.GetProperty("distance").GetDouble() / 1000.0;
            var duration = summary.GetProperty("duration").GetDouble() / 60.0;

            var coords = doc.RootElement
                .GetProperty("features")[0]
                .GetProperty("geometry")
                .GetProperty("coordinates");

            var points = new List<(double lat, double lng)>();
            foreach (var coord in coords.EnumerateArray())
            {
                var lng = coord[0].GetDouble();
                var lat = coord[1].GetDouble();
                points.Add((lat, lng));
            }

            return new RouteResult
            {
                DistanceKm = Math.Round(distance, 2),
                DurationMinutes = Math.Round(duration, 1),
                Coordinates = points
            };
        }
        
        
        public async Task SaveCoordinatesAsJsAsync(List<(double lat, double lng)> coordinates)
        { 
            MessageBox.Show("Save coordinates in directions.js...");

            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory!;
                MessageBox.Show($"BaseDirectory :\n{baseDir}");
                string folder = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\Assets\leaflet"));

                string filePath = Path.Combine(folder, "directions.js");

                var jsContent = "const routeCoordinates = [\n";

                foreach (var (lat, lng) in coordinates)
                {
                    jsContent += $"    [{lat.ToString(CultureInfo.InvariantCulture)}, {lng.ToString(CultureInfo.InvariantCulture)}],\n";
                }

                jsContent = jsContent.TrimEnd(',', '\n') + "\n];\n";

                await File.WriteAllTextAsync(filePath, jsContent);
                
                MessageBox.Show($"directions.js saved in:\n{filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing the coordinates:\n{ex.Message}");
            }

        }
        
        
        public async Task SaveMapScreenshotAsync(string outputFileName)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string htmlPath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\Assets\map.html"));
                string outputPath = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\Assets\Maps", outputFileName));

                MessageBox.Show($"Try Screenshot with:\nHTML:\n{htmlPath}\nOUT:\n{outputPath}");

                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox" } 
                };

                using var browser = await Puppeteer.LaunchAsync(launchOptions);
                using var page = await browser.NewPageAsync();

                await page.GoToAsync(new Uri(htmlPath).AbsoluteUri);
                await Task.Delay(1500); 

                await page.ScreenshotAsync(outputPath, new ScreenshotOptions { FullPage = true });

                MessageBox.Show($"Screenshot saved: {outputPath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Screenshot:\n{ex}");
            }
        }
    }
}
