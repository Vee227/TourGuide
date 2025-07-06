using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using TourGuide.DataLayer.Models;

namespace TourGuide.Logs
{
    public static class ReportGenerator
    {
        public static void GenerateSingleTourReport(Tour tour, List<TourLog> logs, string mapImagePath = null)
        {
            try
            {
                LoggerHelper.Info("Starting report generation...");

                // Null check for tour
                if (tour == null)
                {
                    LoggerHelper.Error("Tour is null. Cannot generate report.");
                    throw new ArgumentNullException(nameof(tour));
                }

                // Create Logs directory if missing
                string logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                    LoggerHelper.Info("Created Logs directory at: " + logsDir);
                }

                string filename = $"SingleTourReport_{tour.name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string fullPath = Path.Combine(logsDir, filename);

                LoggerHelper.Info($"Saving PDF to: {fullPath}");

                using PdfWriter writer = new(fullPath);
                using PdfDocument pdf = new(writer);
                Document document = new(pdf);

                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                document.Add(new Paragraph($"🗺️ Tour Report: {tour.name ?? "Unnamed"}")
                    .SetFontSize(18)
                    .SetFont(boldFont)
                    .SetTextAlignment(TextAlignment.CENTER));

                document.Add(
                    new Paragraph($"Start: {tour.startLocation ?? "N/A"}\nDestination: {tour.endLocation ?? "N/A"}")
                        .SetFontSize(12));

                document.Add(new Paragraph(
                    $"Transport: {tour.transporttype}\nDistance: {tour.distance} km\nEstimated Time: {tour.estimatedTime} mins"));

                // Map image (optional)
                if (!string.IsNullOrWhiteSpace(mapImagePath))
                {
                    if (File.Exists(mapImagePath))
                    {
                        ImageData? imgData = ImageDataFactory.Create(mapImagePath);
                        Image? img = new Image(imgData).ScaleToFit(400, 400);
                        document.Add(img);
                        LoggerHelper.Info("Map image included in report.");
                    }
                    else
                    {
                        LoggerHelper.Warn("Map image path provided but file not found: " + mapImagePath);
                    }
                }

                // Tour Logs
                document.Add(new Paragraph("\nTour Logs")
                    .SetFontSize(14)
                    .SetFont(boldFont));

                if (logs != null && logs.Any())
                {
                    foreach (TourLog log in logs)
                    {
                        document.Add(new Paragraph(
                            $"📅 {log.Date} | 🕓 {log.TotalTime}min | ⭐ {log.Rating}\n" +
                            $"Comment: {log.Comment}\nDifficulty: {log.Difficulty}"
                        ).SetMarginBottom(10));
                    }
                }
                else
                {
                    document.Add(new Paragraph("No logs available for this tour."));
                    LoggerHelper.Info("No logs found for this tour.");
                }

                document.Close();
                LoggerHelper.Info("Report generation completed successfully.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("PDF report generation failed.", ex);
                throw;
            }
        }
        
        public static void GenerateSummaryReport(List<Tour> tours, Dictionary<int, List<TourLog>> logsPerTour)
        {
            try
            {
                LoggerHelper.Info("Generating tour summary report...");

                string logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);

                string filename = $"TourSummary_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string fullPath = Path.Combine(logsDir, filename);

                using PdfWriter writer = new(fullPath);
                using PdfDocument pdf = new(writer);
                Document document = new(pdf);

                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                document.Add(new Paragraph("🗂️ Tour Summary Report").SetFontSize(18).SetFont(boldFont).SetTextAlignment(TextAlignment.CENTER));

                foreach (var tour in tours)
                {
                    document.Add(new Paragraph($"\n📌 {tour.name ?? "Unnamed"}").SetFontSize(14).SetFont(boldFont));

                    if (logsPerTour.TryGetValue(tour.Id, out var logs) && logs.Any())
                    {
                        double avgTime = logs.Average(l => l.TotalTime);
                        double avgRating = logs.Average(l => l.Rating);
                        double distance = tour.distance;

                        document.Add(new Paragraph($"📏 Tour Distance: {distance} km"));
                        document.Add(new Paragraph($"⏱️ Avg. Total Time (from logs): {avgTime:F1} min"));
                        document.Add(new Paragraph($"⭐ Avg. Rating (from logs): {avgRating:F1}/5"));
                    }
                    else
                    {
                        document.Add(new Paragraph("No logs for this tour."));
                    }
                }

                document.Close();
                LoggerHelper.Info("Summary report created.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Failed to generate summary report.", ex);
                throw;
            }
        }

    }
}