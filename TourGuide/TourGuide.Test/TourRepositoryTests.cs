using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;
using TourGuide.DataLayer;
using TourGuide.DataLayer.Models;
using TourGuide.DataLayer.Repositories;

namespace TourGuide.Test
{
    public class TourRepositoryTests : IDisposable
    {
        private readonly TourPlannerContext _context;
        private readonly TourRepository _repository;
        private readonly IDbContextTransaction _transaction;

        public TourRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TourPlannerContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=tourdb;Username=postgres;Password=postgres")
                .Options;

            _context = new TourPlannerContext(options);
            _context.Database.EnsureCreated();

            _transaction = _context.Database.BeginTransaction();
            _repository = new TourRepository(_context);
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }

        [Fact]
        public async Task AddTourAsync_ShouldAddTourTemporarily()
        {
            var tour = new Tour
            {
                name = "Temporary Tour",
                description = "This tour will be rolled back",
                startLocation = "Start",
                endLocation = "End",
                transporttype = "Bike",
                distance = 20,
                estimatedTime = 90
            };

            await _repository.AddTourAsync(tour);

            var allTours = await _repository.GetAllToursAsync();
            Assert.Contains(allTours, t => t.name == "Temporary Tour");
        }

        [Fact]
        public async Task DeleteTourAsync_ShouldRemoveTour()
        {
            var tour = new Tour
            {
                name = "Delete Me",
                description = "To be deleted",
                startLocation = "A",
                endLocation = "B",
                transporttype = "Run",
                distance = 8,
                estimatedTime = 50
            };

            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            await _repository.DeleteTourAsync(tour.Id);

            var result = await _repository.GetTourByIdAsync(tour.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllToursAsync_ShouldReturnAtLeastTwo()
        {
            var t1 = new Tour { name = "Tour A", description = "", startLocation = "S", endLocation = "E", transporttype = "Walk", distance = 3, estimatedTime = 30 };
            var t2 = new Tour { name = "Tour B", description = "", startLocation = "X", endLocation = "Y", transporttype = "Hike", distance = 5, estimatedTime = 45 };

            _context.Tours.AddRange(t1, t2);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllToursAsync();
            Assert.True(result.Count() >= 2);
        }

        [Fact]
        public async Task GetTourByIdAsync_ShouldReturnCorrectTour()
        {
            var tour = new Tour
            {
                name = "TourGetById",
                description = "Check ID fetch",
                startLocation = "Loc1",
                endLocation = "Loc2",
                transporttype = "Bus",
                distance = 12,
                estimatedTime = 60
            };

            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            var fetched = await _repository.GetTourByIdAsync(tour.Id);
            Assert.NotNull(fetched);
            Assert.Equal("TourGetById", fetched.name);
        }

        [Fact]
        public async Task UpdateTourAsync_ShouldChangeValues()
        {
            var tour = new Tour
            {
                name = "UpdateBefore",
                description = "Initial",
                startLocation = "Start",
                endLocation = "End",
                transporttype = "Car",
                distance = 50,
                estimatedTime = 120
            };

            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            tour.name = "UpdateAfter";
            tour.distance = 55;

            await _repository.UpdateTourAsync(tour);

            var updated = await _repository.GetTourByIdAsync(tour.Id);
            Assert.Equal("UpdateAfter", updated.name);
            Assert.Equal(55, updated.distance);
        }
        
        [Fact]
        public void CanConnectToDatabase()
        {
            using var context = new TourPlannerContext(
                new DbContextOptionsBuilder<TourPlannerContext>()
                    .UseNpgsql("Host=localhost;Port=5432;Database=tourdb;Username=postgres;Password=postgres")
                    .Options);

            bool canConnect = context.Database.CanConnect();
            Assert.True(canConnect, "Database connection failed.");
        }
        
        [Fact]
        public async Task OpenRouteService_ShouldReturnValidResponse()
        {
            var httpClient = new HttpClient();
            string apiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6IjBlN2MxZjk0M2I3MmM3MjhhYmUzZThhODMxZjQ3YTJhODU3NjgyYjYxNTFhODZjZmJiMzI1MDQ4IiwiaCI6Im11cm11cjY0In0=";
            string url = $"https://api.openrouteservice.org/v2/directions/foot-walking?api_key={apiKey}&start=8.681495,49.41461&end=8.687872,49.420318";

            var response = await httpClient.GetAsync(url);
    
            Assert.True(response.IsSuccessStatusCode, $"API call failed with status code: {response.StatusCode}");
        }
        
        [Fact]
        public void Popularity_ShouldReflectNumberOfLogs()
        {
            var tour = new Tour
            {
                TourLogs = new List<TourLog>
                {
                    new TourLog(), new TourLog(), new TourLog()
                }
            };

            Assert.Equal(3, tour.Popularity);
        }
        
        [Fact]
        public async Task UpdateTourAsync_ShouldNotThrowIfTourDoesNotExist()
        {
            var fakeTour = new Tour
            {
                Id = 999999, // Use a high int value unlikely to exist
                name = "NonExistent",
                description = "This tour doesn't exist in DB",
                startLocation = "Nowhere",
                endLocation = "Somewhere",
                transporttype = "Car",
                distance = 0,
                estimatedTime = 0
            };

            var exception = await Record.ExceptionAsync(() => _repository.UpdateTourAsync(fakeTour));
            Assert.Null(exception); // Should not throw, maybe silently does nothing
        }
        
        [Fact]
        public async Task DeleteTourAsync_ShouldNotThrowWhenTourDoesNotExist()
        {
            var invalidId = 999999;
            var exception = await Record.ExceptionAsync(() => _repository.DeleteTourAsync(invalidId));
            Assert.Null(exception); // Should silently do nothing or log
        }
        
        [Fact]
        public void FilterTours_ShouldReturnMatchingName()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { name = "Vienna City Walk" },
                new Tour { name = "Mountain Hike" },
                new Tour { name = "Beach Tour" }
            };

            var repo = new SearchRepository();

            // Act
            var result = repo.FilterTours(tours, "vienna");

            // Assert
            Assert.Single(result);
            Assert.Contains(result, t => t.name == "Vienna City Walk");
        }
        
        [Fact]
        public void FilterTours_ShouldReturnEmptyList_WhenNoMatch()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { name = "Castle Tour" },
                new Tour { name = "City Walk" }
            };

            var repo = new SearchRepository();

            // Act
            var result = repo.FilterTours(tours, "spaceship");

            // Assert
            Assert.Empty(result);
        }



    }
}
