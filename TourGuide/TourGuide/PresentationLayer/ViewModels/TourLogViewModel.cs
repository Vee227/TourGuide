using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Models;
using TourGuide.DataLayer.Repositories;
using TourGuide.DataLayer;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class TourLogViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TourLog> TourLogs { get; set; } = new();

        private TourLog _selectedTourLog;
        public TourLog SelectedTourLog
        {
            get => _selectedTourLog;
            set
            {
                _selectedTourLog = value;
                OnPropertyChanged(nameof(SelectedTourLog));
            }
        }

        public ICommand AddTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand ModifyTourLogCommand { get; }

        private int _currentTourId;

        public TourLogViewModel()
        {
            AddTourLogCommand = new RelayCommand(_ => OpenAddTourLogWindow());
            DeleteTourLogCommand = new RelayCommand(_ => DeleteTourLog(), _ => SelectedTourLog != null);
            ModifyTourLogCommand = new RelayCommand(_ => OpenModifyTourLogWindow(), _ => SelectedTourLog != null);
        }

        public async void LoadTourLogs(int tourId)
        {
            _currentTourId = tourId;

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repo = new TourLogRepository(context);

                var logs = await repo.GetLogsByTourIdAsync(tourId);
                TourLogs = new ObservableCollection<TourLog>(logs);
                OnPropertyChanged(nameof(TourLogs));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der TourLogs: {ex.Message}");
            }
        }

        public async void DeleteTourLog()
        {
            if (SelectedTourLog == null) return;

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repo = new TourLogRepository(context);

                await repo.DeleteTourLogAsync(SelectedTourLog.Id);
                TourLogs.Remove(SelectedTourLog);
                SelectedTourLog = null;
                OnPropertyChanged(nameof(TourLogs));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Löschen des TourLogs: {ex.Message}");
            }
        }

        public void ModifyTourLog()
        {
            OnPropertyChanged(nameof(TourLogs));
        }

        private void OpenModifyTourLogWindow()
        {
            var modifyWindow = new Views.ModifyTourLogView(this);
            modifyWindow.ShowDialog();
        }

        private void OpenAddTourLogWindow()
        {
            var addTourLogWindow = new Views.AddTourLogView(this, _currentTourId);
            addTourLogWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}