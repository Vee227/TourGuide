using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer;
using TourGuide.DataLayer.Repositories;
using System.Globalization;
using TourGuide.BusinessLayer;
using log4net;
using TourGuide.Logs;

namespace TourGuide.PresentationLayer.ViewModels
{
   public class AddTourLogViewModel : INotifyPropertyChanged
    {
        public string Date { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int? Difficulty { get; set; }
        public int? TotalTime { get; set; }
        public double? Distance { get; set; }
        public int? Rating { get; set; }
        

        private readonly TourLogViewModel _tourLogViewModel;
        private readonly int _tourId;

        public ICommand SaveCommand { get; }
        public Action CloseWindow { get; set; }

        public List<int> DifficultyOptions { get; } = new() { 1, 2, 3, 4, 5 };
        public List<int> RatingOptions { get; } = new() { 1, 2, 3, 4, 5 };

        public AddTourLogViewModel(TourLogViewModel tourLogViewModel, int tourId)
        {
            _tourLogViewModel = tourLogViewModel;
            _tourId = tourId;
            SaveCommand = new RelayCommand(_ => Save());
        }

        private async void Save()
        {
            if (!DateTime.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                LoggerHelper.Warn("Invalid date format in AddTourLog.");
                MessageBox.Show("Please enter a valid date in format YYYY-MM-DD.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Comment))
            {
                LoggerHelper.Warn("Empty comment in AddTourLog.");
                MessageBox.Show("Comment cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Difficulty is null or < 1 or > 5)
            {
                LoggerHelper.Warn("Invalid difficulty in AddTourLog.");
                MessageBox.Show("Please select a valid difficulty (1 (easy) –5 (hard)).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TotalTime is null or <= 0)
            {
                LoggerHelper.Warn("Invalid total time in AddTourLog.");
                MessageBox.Show("Total time must be a positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (Distance is null or <= 0)
            {
                LoggerHelper.Warn("Invalid distance in AddTourLog.");
                MessageBox.Show("Distance must be a positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (Rating is null or < 1 or > 5)
            {
                LoggerHelper.Warn("Invalid rating in AddTourLog.");
                MessageBox.Show("Please select a valid rating (1 (bad) –5 (good)).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newLog = new TourLog
            {
                TourId = _tourId,
                Date = Date,
                Comment = Comment,
                Difficulty = Difficulty.Value,
                TotalTime = TotalTime.Value,
                Distance = Distance.Value,
                Rating = Rating.Value
            };

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repo = new TourLogRepository(context);
                await repo.AddTourLogAsync(newLog);
                
                LoggerHelper.Info($"TourLog added: Date={Date}, Rating={Rating}, TourId={_tourId}");

                _tourLogViewModel.LoadTourLogs(_tourId);
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Error saving TourLog in AddTourLogViewModel.", ex);
                MessageBox.Show($"Error saving tour log: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}