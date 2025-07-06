using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourGuide.DataLayer.Models;

namespace TourGuide.DataLayer.Repositories
{
    public interface ITourLogRepository
    {
        Task<IEnumerable<TourLog>> GetLogsByTourIdAsync(int tourId);
        Task AddTourLogAsync(TourLog log);
        Task DeleteTourLogAsync(int id);
        Task UpdateTourLogAsync(TourLog log);
    }
}
