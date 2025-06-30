using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourGuide.DataLayer.Models;

namespace TourGuide.DataLayer.Repositories
{
    public class TourLogRepository : ITourLogRepository
    {
        private readonly TourPlannerContext _context;

        public TourLogRepository(TourPlannerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourLog>> GetLogsByTourIdAsync(int tourId)
        {
            return await _context.TourLogs.Where(l => l.TourId == tourId).ToListAsync();
        }

        public async Task AddTourLogAsync(TourLog log)
        {
            await _context.TourLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTourLogAsync(int id)
        {
            var log = await _context.TourLogs.FindAsync(id);
            if (log != null)
            {
                _context.TourLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}
