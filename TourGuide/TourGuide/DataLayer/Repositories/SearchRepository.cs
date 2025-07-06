using TourGuide.DataLayer.Models;
using System.Collections.Generic;

namespace TourGuide.DataLayer.Repositories
{
    public class SearchRepository
    {
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
                    (searchInput.Contains("child") && tour.IsChildFriendly) ||
                    (searchInput.Contains("popular") && tour.Popularity >= 3)
            );
        }
    }
}