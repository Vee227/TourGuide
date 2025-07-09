using TourGuide.DataLayer.Models;
using System.Windows;
using System.IO;

namespace TourGuide.BusinessLayer;

public class TourService
{
    public int CalculatePopularity(Tour tour)
    {
        return tour.TourLogs?.Count ?? 0;
    }

    public bool IsChildFriendly(Tour tour)
    {
        if (tour.TourLogs == null || !tour.TourLogs.Any())
            return false;

        return tour.TourLogs.All(log =>
            log.Difficulty <= 2 &&
            log.TotalTime <= 120 &&
            log.Distance <= 10);
    }
}