using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.Comands;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class AddTourLogViewModel : INotifyPropertyChanged
    {
        public string Date { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int? Difficulty { get; set; }
        public int? TotalTime { get; set; }
        public int? Rating { get; set; }

        private readonly TourLogViewModel _tourLogViewModel;
        private readonly string _selectedTourName;

        public ICommand SaveCommand { get; }
        public Action CloseWindow { get; set; }
        
        public List<int> DifficultyOptions { get; } = new() { 1, 2, 3, 4, 5 };
        public List<int> RatingOptions { get; } = new() { 1, 2, 3, 4, 5 };

        public AddTourLogViewModel(TourLogViewModel tourLogViewModel, string selectedTourName)
        {
            _tourLogViewModel = tourLogViewModel;
            _selectedTourName = selectedTourName;
            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Date) || !DateTime.TryParse(Date, out _))
            {
                MessageBox.Show("Please enter a valid date (YYYY-MM-DD).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Comment))
            {
                MessageBox.Show("Comment cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Difficulty is null || Difficulty < 1 || Difficulty > 5)
            {
                MessageBox.Show("Please select a valid difficulty (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (TotalTime is null || TotalTime <= 0)
            {
                MessageBox.Show("Total time must be a positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Rating is null || Rating < 1 || Rating > 5)
            {
                MessageBox.Show("Please select a valid rating (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newTourLog = new TourLog
            {
                TourName = _selectedTourName,
                Date = Date,
                Comment = Comment,
                Difficulty = Difficulty.Value,
                TotalTime = TotalTime.Value,
                Rating = Rating.Value
            };

            _tourLogViewModel.AddTourLog(newTourLog);
            CloseWindow?.Invoke();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}