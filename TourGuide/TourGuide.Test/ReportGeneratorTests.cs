using Xunit;
using TourGuide.Logs;
using TourGuide.DataLayer.Models;
using System.Collections.Generic;
using System.IO;
using System;

namespace TourGuide.Test
{
    public class ReportGeneratorTests
    {
        [Fact]
        public void GenerateSingleTourReport_CreatesPdfFile()
        {
            var tour = new Tour
            {
                name = "TestTour",
                startLocation = "A",
                endLocation = "B",
                transporttype = "Bike",
                distance = 12.5,
                estimatedTime = 80
            };

            var logs = new List<TourLog>(); 
            string logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            
            ReportGenerator.GenerateSingleTourReport(tour, logs);
            
            var files = Directory.GetFiles(logsDir, $"SingleTourReport_{tour.name}_*.pdf");
            Assert.NotEmpty(files);
            
            foreach (var f in files)
                File.Delete(f);
        }
        
        [Fact]
        public void GenerateSingleTourReport_WithNullTour_ThrowsException()
        {
            Tour tour = null;
            var logs = new List<TourLog>();
            
            Assert.Throws<ArgumentNullException>(() =>
                ReportGenerator.GenerateSingleTourReport(tour, logs));
        }
        
        [Fact]
        public void GenerateSingleTourReport_WithMissingMapImage_StillCreatesPdf()
        {
            var tour = new Tour
            {
                name = "TourWithMissingImage",
                startLocation = "A",
                endLocation = "B",
                transporttype = "Car",
                distance = 40,
                estimatedTime = 100
            };

            var logs = new List<TourLog>();
            string fakeImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "doesnotexist.jpg");
            string logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            
            ReportGenerator.GenerateSingleTourReport(tour, logs, fakeImagePath);

            var files = Directory.GetFiles(logsDir, $"SingleTourReport_{tour.name}_*.pdf");
            Assert.NotEmpty(files);

            foreach (var f in files)
                File.Delete(f);
        }
        
        [Fact]
        public void ReportsFolder_ShouldBeWritable()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string testFile = Path.Combine(path, "test.txt");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(testFile, "test");
            Assert.True(File.Exists(testFile));

            File.Delete(testFile);
        }

    }
}

