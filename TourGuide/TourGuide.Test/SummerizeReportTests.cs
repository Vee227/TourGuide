using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourGuide.DataLayer.Models;
using TourGuide.Logs;
using Xunit;

namespace TourGuide.Test
{
    public class SummarizeReportTests
    {
        private readonly string _logsDir;

        public SummarizeReportTests()
        {
            _logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(_logsDir))
                Directory.CreateDirectory(_logsDir);
        }

        [Fact]
        public void GenerateSummaryReport_CreatesPdfFile()
        {
            var tour1 = new Tour
            {
                Id = 1,
                name = "Test Tour",
                description = "Desc",
                startLocation = "A",
                endLocation = "B",
                transporttype = "Walk",
                distance = 10,
                estimatedTime = 60
            };

            var logs = new List<TourLog>
            {
                new TourLog { TourId = 1, Date = "2024-06-01", Comment = "Ok", Difficulty = 2, TotalTime = 60, Rating = 4 },
                new TourLog { TourId = 1, Date = "2024-06-02", Comment = "Good", Difficulty = 3, TotalTime = 55, Rating = 5 }
            };

            var logsPerTour = new Dictionary<int, List<TourLog>>
            {
                { 1, logs }
            };

            ReportGenerator.GenerateSummaryReport(new List<Tour> { tour1 }, logsPerTour);

            var files = Directory.GetFiles(_logsDir, "TourSummary_*.pdf");
            Assert.NotEmpty(files);

            foreach (var file in files)
                File.Delete(file);
        }

        [Fact]
        public void GenerateSummaryReport_NoLogs_CreatesPdfWithFallbackText()
        {
            var tour = new Tour
            {
                Id = 2,
                name = "Empty Tour",
                description = "Test",
                startLocation = "Start",
                endLocation = "End",
                transporttype = "Car",
                distance = 20,
                estimatedTime = 90
            };

            var logsPerTour = new Dictionary<int, List<TourLog>>(); // Keine Logs

            // Act
            ReportGenerator.GenerateSummaryReport(new List<Tour> { tour }, logsPerTour);

            // Assert
            var files = Directory.GetFiles(_logsDir, "TourSummary_*.pdf");
            Assert.NotEmpty(files);

            foreach (var file in files)
                File.Delete(file);
        }

    }
}
