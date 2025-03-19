using System;
using System.IO;
using System.Linq;
using Xunit;
using TourGuide.PresentationLayer.ViewModels;
using TourGuide.DataLayer.Models;

namespace TourGuide.Tests
{
    public class TourGuideTests
    {
        private const string TestToursFile = "tours.json";
        private const string TestLogsFile = "tourlogs.json";

        public TourGuideTests()
        {
            ResetTestFiles();
        }

        private void ResetTestFiles()
        {
            if (File.Exists(TestToursFile)) File.Delete(TestToursFile);
            if (File.Exists(TestLogsFile)) File.Delete(TestLogsFile);
        }

        [Fact]
        public void AddTour_ValidTour_ShouldBeAdded()
        {
            // Arrange
            var tourListVM = new TourListViewModel();
            var newTour = new Tour
            {
                name = "Mountain Trip",
                description = "A beautiful mountain tour",
                startLocation = "Base Camp",
                endLocation = "Summit",
                transporttype = "Hike",
                distance = 20.0,
                estimatedTime = 300
            };

            // Act
            tourListVM.AddTour(newTour);
            tourListVM.LoadTours();  // Reload to verify persistence

            // Assert
            Assert.Contains(tourListVM.Tours, t => t.name == "Mountain Trip");
        }

        [Fact]
        public void AddTour_InvalidData_ShouldThrowException()
        {
            var tourListVM = new TourListViewModel();
            var invalidTour = new Tour
            {
                name = "", // Invalid name
                startLocation = "",
                endLocation = "",
                transporttype = "Plane", // Not allowed
                distance = -10, // Invalid
                estimatedTime = -1 // Invalid
            };

            Assert.Throws<ArgumentException>(() => tourListVM.AddTour(invalidTour));
        }

        [Fact]
        public void LoadTours_ValidFile_ShouldLoadCorrectly()
        {
            // Arrange
            ResetTestFiles();  // Start fresh
            string json = "[{\"name\":\"Beach Walk\",\"description\":\"Relaxing\",\"startLocation\":\"Hotel\",\"endLocation\":\"Shore\",\"transporttype\":\"Walk\",\"distance\":5,\"estimatedTime\":60}]";
            File.WriteAllText(TestToursFile, json);

            var tourListVM = new TourListViewModel();

            // Act
            tourListVM.LoadTours();

            // Assert
            Assert.Single(tourListVM.Tours);  // Ensures exactly one tour is loaded
            Assert.Equal("Mountain Trip", tourListVM.Tours[0].name);
        }

        [Fact]
        public void DeleteTour_ExistingTour_ShouldRemoveFromFile()
        {
            // Arrange
            var tourListVM = new TourListViewModel();
            var testTour = new Tour
            {
                name = "To Be Deleted",
                description = "This will be deleted",
                startLocation = "A",
                endLocation = "B",
                transporttype = "Bike",
                distance = 10,
                estimatedTime = 100
            };
            tourListVM.AddTour(testTour);

            // Act
            tourListVM.DeleteTour(testTour);
            tourListVM.LoadTours();  // Reload to check if it's gone

            // Assert
            Assert.DoesNotContain(tourListVM.Tours, t => t.name == "To Be Deleted");
        }

        [Fact]
        public void AddTourLog_ValidLog_ShouldBeStored()
        {
            var tourLogVM = new TourLogViewModel();
            var newLog = new TourLog
            {
                TourName = "Mountain Trip",
                Date = "2024-06-01",
                Comment = "Challenging but fun",
                Difficulty = 4,
                TotalTime = 180,
                Rating = 5
            };

            // Act
            tourLogVM.AddTourLog(newLog);
            tourLogVM.LoadTourLogs("Mountain Trip"); // Reload to verify persistence

            // Assert
            Assert.Contains(tourLogVM.TourLogs, log => log.TourName == "Mountain Trip");
        }

        [Fact]
        public void DeleteTourLog_NonExistent_ShouldNotCrash()
        {
            var tourLogVM = new TourLogViewModel();

            // Act & Assert
            Exception ex = Record.Exception(() => tourLogVM.DeleteTourLog());
            Assert.Null(ex); // Should not throw an exception
        }
    }
}
