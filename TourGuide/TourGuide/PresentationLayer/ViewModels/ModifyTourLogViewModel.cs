using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Repositories;
using TourGuide.DataLayer;

namespace TourGuide.PresentationLayer.ViewModels
{
     public class ModifyTourLogViewModel : INotifyPropertyChanged
    {
        private readonly TourLogViewModel _tourLogViewModel;
        public TourLog SelectedTourLog => _tourLogViewModel.SelectedTourLog;

        public List<int> DifficultyOptions { get; } = new() { 1, 2, 3, 4, 5 };
        public List<int> RatingOptions { get; } = new() { 1, 2, 3, 4, 5 };
        public ICommand SaveCommand { get; }
        public Action CloseWindow { get; set; }

        public ModifyTourLogViewModel(TourLogViewModel tourLogViewModel)
        {
            _tourLogViewModel = tourLogViewModel;
            SaveCommand = new RelayCommand(_ => Save());
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedTourLog.Date) || !DateTime.TryParse(SelectedTourLog.Date, out _))
            {
                MessageBox.Show("Please enter a valid date (YYYY-MM-DD).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(SelectedTourLog.Comment))
            {
                MessageBox.Show("Comment cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedTourLog.Difficulty < 1 || SelectedTourLog.Difficulty > 5)
            {
                MessageBox.Show("Please select a valid difficulty (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedTourLog.TotalTime <= 0)
            {
                MessageBox.Show("Total time must be a positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedTourLog.Rating < 1 || SelectedTourLog.Rating > 5)
            {
                MessageBox.Show("Please select a valid rating (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repo = new TourLogRepository(context);

                await repo.UpdateTourLogAsync(SelectedTourLog);
                _tourLogViewModel.ModifyTourLog(); // UI-Refresh
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating tour log: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
