using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TourGuide.DataLayer.Models;

namespace TourGuide.DataLayer.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly TourPlannerContext _context;

        public TourRepository(TourPlannerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _context.Tours
                .FromSqlInterpolated($"SELECT * FROM \"Tours\"")
                .ToListAsync();
        }

        public async Task<Tour> GetTourByIdAsync(int id)
        {
            return await _context.Tours
                .FromSqlInterpolated($"SELECT * FROM \"Tours\" WHERE \"Id\" = {id}")
                .FirstOrDefaultAsync();
        }

        public async Task AddTourAsync(Tour tour)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO ""Tours""
                    (""name"", ""description"", ""startLocation"", ""endLocation"", ""transporttype"", ""distance"", ""estimatedTime"")
                VALUES
                    ({tour.name}, {tour.description}, {tour.startLocation}, {tour.endLocation},
                     {tour.transporttype}, {tour.distance}, {tour.estimatedTime});
            ");
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE ""Tours"" SET
                    ""name"" = {tour.name},
                    ""description"" = {tour.description},
                    ""startLocation"" = {tour.startLocation},
                    ""endLocation"" = {tour.endLocation},
                    ""transporttype"" = {tour.transporttype},
                    ""distance"" = {tour.distance},
                    ""estimatedTime"" = {tour.estimatedTime}
                WHERE ""Id"" = {tour.Id};
            ");
        }

        public async Task DeleteTourAsync(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM ""Tours"" WHERE ""Id"" = {id};
            ");
        }
    }
}
