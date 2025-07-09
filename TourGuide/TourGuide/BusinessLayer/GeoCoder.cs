using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace TourGuide.BusinessLayer
{
    public static class GeoCoder
    {
        private static readonly HttpClient _http = new();

        static GeoCoder()
        {
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("TourPlannerApp/1.0 (https://github.com/Vee227/TourGuide)");
        }

        public static async Task<double> GetLat(string location)
        {
            var coords = await GetCoordinates(location);
            return coords.lat;
        }

        public static async Task<double> GetLng(string location)
        {
            var coords = await GetCoordinates(location);
            return coords.lng;
        }

        private static async Task<(double lat, double lng)> GetCoordinates(string location)
        {
            var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(location)}";
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var first = doc.RootElement[0];
            
            string latStr = first.GetProperty("lat").GetString();
            string lngStr = first.GetProperty("lon").GetString();
            
            var lat = double.Parse(latStr, CultureInfo.InvariantCulture);
            var lng = double.Parse(lngStr, CultureInfo.InvariantCulture);


            return (lat, lng);
        }
    }
}