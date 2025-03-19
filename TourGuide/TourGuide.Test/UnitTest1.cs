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

            
            tourListVM.AddTour(newTour);
            tourListVM.LoadTours(); 

            
            Assert.Contains(tourListVM.Tours, t => t.name == "Mountain Trip");
        }

        [Fact]
        public void AddTour_InvalidData_ShouldThrowException()
        {
            var tourListVM = new TourListViewModel();
            var invalidTour = new Tour
            {
                name = "", 
                startLocation = "",
                endLocation = "",
                transporttype = "Plane", 
                distance = -10, 
                estimatedTime = -1 
            };

            Assert.Throws<ArgumentException>(() => tourListVM.AddTour(invalidTour));
        }

        [Fact]
        public void LoadTours_ValidFile_ShouldLoadCorrectly()
        {
            
            ResetTestFiles(); 
            string json = "[{\"name\":\"Beach Walk\",\"description\":\"Relaxing\",\"startLocation\":\"Hotel\",\"endLocation\":\"Shore\",\"transporttype\":\"Walk\",\"distance\":5,\"estimatedTime\":60}]";
            File.WriteAllText(TestToursFile, json);

            var tourListVM = new TourListViewModel();

            
            tourListVM.LoadTours();

            
            Assert.Single(tourListVM.Tours); 
            Assert.Equal("Mountain Trip", tourListVM.Tours[0].name);
        }

        [Fact]
        public void DeleteTour_ExistingTour_ShouldRemoveFromFile()
        {
            
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

            
            tourListVM.DeleteTour(testTour);
            tourListVM.LoadTours();  

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

            
            tourLogVM.AddTourLog(newLog);
            tourLogVM.LoadTourLogs("Mountain Trip"); 

            
            Assert.Contains(tourLogVM.TourLogs, log => log.TourName == "Mountain Trip");
        }

        [Fact]
        public void DeleteTourLog_NonExistent_ShouldNotCrash()
        {
            var tourLogVM = new TourLogViewModel();

            
            Exception ex = Record.Exception(() => tourLogVM.DeleteTourLog());
            Assert.Null(ex); 
        }
    }
}
