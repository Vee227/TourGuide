using TourGuide.DataLayer.Models;
using System.Collections.Generic;
using TourGuide.BusinessLayer;

namespace TourGuide.BusinessLayer
{
    public class SearchService
    {
        private readonly TourService _tourService = new();

        public IEnumerable<Tour> FilterTours(IEnumerable<Tour> allTours, string searchInput)
        {
            if (string.IsNullOrWhiteSpace(searchInput))
                return allTours;

            searchInput = searchInput.ToLower();

            return allTours.Where(tour =>
                (tour.name?.ToLower().Contains(searchInput) ?? false) ||
                (tour.description?.ToLower().Contains(searchInput) ?? false) ||
                (tour.startLocation?.ToLower().Contains(searchInput) ?? false) ||
                (tour.endLocation?.ToLower().Contains(searchInput) ?? false) ||
                (tour.transporttype?.ToLower().Contains(searchInput) ?? false) ||
                tour.distance.ToString().Contains(searchInput) ||
                tour.estimatedTime.ToString().Contains(searchInput) ||
                tour.TourLogs.Any(log =>
                    (log.Comment?.ToLower().Contains(searchInput) ?? false) ||
                    (log.Date?.ToLower().Contains(searchInput) ?? false) ||
                    log.Difficulty.ToString().Contains(searchInput) ||
                    log.TotalTime.ToString().Contains(searchInput) ||
                    log.Rating.ToString().Contains(searchInput)
                ) ||
                ((searchInput.Contains("child") || searchInput.Contains("childfriendly") || searchInput.Contains("easy"))
                 && _tourService.IsChildFriendly(tour)) ||

                ((searchInput.Contains("popular") || searchInput.Contains("popularity") || searchInput.Contains("cool"))
                 && _tourService.CalculatePopularity(tour) >= 3)
            );
        }
    }
}
