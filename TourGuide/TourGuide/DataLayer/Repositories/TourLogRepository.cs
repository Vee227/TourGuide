using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourGuide.DataLayer.Models;
using TourGuide.Logs;
using log4net;

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
            return await _context.TourLogs
                .FromSqlInterpolated($@"SELECT * FROM ""TourLogs"" WHERE ""TourId"" = {tourId}")
                .ToListAsync();
        }

        public async Task AddTourLogAsync(TourLog log)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO ""TourLogs"" 
                    (""TourId"", ""Date"", ""Comment"", ""Difficulty"", ""TotalTime"", ""Rating"") 
                VALUES 
                    ({log.TourId}, {log.Date}, {log.Comment}, {log.Difficulty}, {log.TotalTime}, {log.Rating});
            ");
            LoggerHelper.Info($"Added TourLog for Tour ID {log.TourId} on {log.Date}.");
        }

        public async Task DeleteTourLogAsync(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM ""TourLogs"" WHERE ""Id"" = {id};
            ");
            LoggerHelper.Info($"Deleted TourLog ID {id}.");
        }

        public async Task UpdateTourLogAsync(TourLog log)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""TourLogs"" SET
                    ""Date"" = {log.Date},
                    ""Comment"" = {log.Comment},
                    ""Difficulty"" = {log.Difficulty},
                    ""TotalTime"" = {log.TotalTime},
                    ""Rating"" = {log.Rating}
                WHERE ""Id"" = {log.Id};
            ");
            LoggerHelper.Info($"Updated TourLog ID {log.Id} for Tour ID {log.TourId}.");
        }
        
        public async Task<Dictionary<int, List<TourLog>>> GetAllLogsGroupedByTourAsync()
        {
            var logs = await _context.TourLogs.FromSqlInterpolated($@"SELECT * FROM ""TourLogs""").ToListAsync();
            return logs.GroupBy(l => l.TourId).ToDictionary(g => g.Key, g => g.ToList());
        }

    }
}
