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
    public class TourRepository : ITourRepository
    {
        private readonly TourPlannerContext _context;

        public TourRepository(TourPlannerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            LoggerHelper.Info("Fetching all tours from database.");
            return await _context.Tours
                .FromSqlInterpolated($"SELECT * FROM \"Tours\"")
                .ToListAsync();
        }

        public async Task<Tour> GetTourByIdAsync(int id)
        {
            LoggerHelper.Info($"Fetching tour with ID {id}.");
            return await _context.Tours
                .FromSqlInterpolated($"SELECT * FROM \"Tours\" WHERE \"Id\" = {id}")
                .FirstOrDefaultAsync();
        }

        public async Task AddTourAsync(Tour tour)
        { 
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO ""Tours""
                    (""name"", ""description"", ""startLocation"", ""endLocation"", ""transporttype"", ""distance"", ""estimatedTime"")
                 VALUES
                    ({tour.name}, {tour.description}, {tour.startLocation}, {tour.endLocation},
                     {tour.transporttype}, {tour.distance}, {tour.estimatedTime});
                SELECT lastval();
                    ");
                
                var insertedTour = await _context.Tours
                    .FromSqlInterpolated($@"SELECT * FROM ""Tours"" WHERE ""name"" = {tour.name} ORDER BY ""Id"" DESC LIMIT 1")
                    .FirstOrDefaultAsync();

                if (insertedTour != null)
                {
                    tour.Id = insertedTour.Id;
                }

                LoggerHelper.Info($"Tour '{tour.name}' added to database.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Failed to add tour '{tour.name}' to database.", ex);
            }
        }

        public async Task UpdateTourAsync(Tour tour)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Tours"" SET
                ""name"" = {tour.name},
                ""description"" = {tour.description},
                ""startLocation"" = {tour.startLocation},
                ""endLocation"" = {tour.endLocation},
                ""transporttype"" = {tour.transporttype},
                ""mapImagePath"" = {tour.mapImagePath}
            WHERE ""Id"" = {tour.Id};
            ");
                LoggerHelper.Info($"Tour '{tour.name}' (ID: {tour.Id}) updated including mapImagePath.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Failed to update tour ID {tour.Id}.", ex);
            }
        }



        public async Task DeleteTourAsync(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    DELETE FROM ""Tours"" WHERE ""Id"" = {id};
                ");
                LoggerHelper.Info($"Tour with ID {id} deleted from database.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Failed to delete tour ID {id}.", ex);
            }
        }

    }
}
