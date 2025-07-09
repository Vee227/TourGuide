using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Models;
using TourGuide.DataLayer;
using TourGuide.DataLayer.Repositories;
using TourGuide.PresentationLayer.Views;
using System.Windows;
using log4net;
using TourGuide.Logs;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindowViewModel));

        public ICommand GenerateSingleTourReportCommand { get; }
        public ICommand GenerateSummaryReportCommand { get; }

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                    OnPropertyChanged(nameof(TourDetailViewModel)); 
                    if (_selectedTour != null)
                    {
                        LoggerHelper.Info($"Selected tour changed: {_selectedTour.name} (ID: {_selectedTour.Id})");
                        TourLogViewModel.LoadTourLogs(_selectedTour.Id);
                    }
                    OnPropertyChanged(nameof(TourLogViewModel.TourLogs));
                    OnPropertyChanged(nameof(MapImageFullPath));
                }
            }
        }

        private TourListViewModel _tourListViewModel;
        public TourListViewModel TourListViewModel
        {
            get => _tourListViewModel;
            set
            {
                _tourListViewModel = value;
                OnPropertyChanged(nameof(TourListViewModel));
            }
        }

        public TourLogViewModel TourLogViewModel { get; set; } = new TourLogViewModel();
        
        public SearchViewModel SearchViewModel { get; set; }

        public ICommand AddTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand ModifyTourCommand { get; }
        public ICommand AddTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand ModifyTourLogCommand { get; }
        
        public TourDetailViewModel TourDetailViewModel =>
            SelectedTour != null ? new TourDetailViewModel(SelectedTour) : null;

        public MainWindowViewModel()
        {
            _tourListViewModel = new TourListViewModel();
            
            SearchViewModel = new SearchViewModel(TourListViewModel);
            
            AddTourCommand = new RelayCommand(_ => OpenAddTourWindow());
            DeleteTourCommand = new RelayCommand(_ => DeleteTour(), _ => SelectedTour != null);
            ModifyTourCommand = new RelayCommand(_ => ModifyTour(), _ => SelectedTour != null);
            AddTourLogCommand = new RelayCommand(_ => OpenAddTourLogWindow(), _ => SelectedTour != null);
            DeleteTourLogCommand = new RelayCommand(_ => DeleteTourLog(), _ => TourLogViewModel.SelectedTourLog != null);
            ModifyTourLogCommand = new RelayCommand(_ => OpenModifyTourLogWindow(), _ => TourLogViewModel.SelectedTourLog != null);
            GenerateSingleTourReportCommand = new RelayCommand(_ => GenerateSingleTourReport(), _ => SelectedTour != null);
            GenerateSummaryReportCommand = new RelayCommand(_ => GenerateSummaryReport());
            LoadTopTours();
            LoggerHelper.Info("Main window opened and ViewModel initialized.");

        }

        private void OpenAddTourWindow()
        {
            var addTourWindow = new AddTourView(TourListViewModel);
            addTourWindow.ShowDialog();
        }

        private void OpenAddTourLogWindow()
        {
            var addTourLogWindow = new AddTourLogView(TourLogViewModel, SelectedTour.Id);
            addTourLogWindow.ShowDialog();
        }

        private void DeleteTour()
        {
            if (SelectedTour != null)
            {
                _tourListViewModel.DeleteTour(SelectedTour);
                SelectedTour = null;
            }
        }

        private void DeleteTourLog()
        {
            TourLogViewModel.DeleteTourLog();
        }

        private void ModifyTour()
        {
            if (SelectedTour == null) return;

            var tourInList = TourListViewModel.Tours.FirstOrDefault(t => t.Id == SelectedTour.Id);
            if (tourInList != null)
            {
                TourListViewModel.SelectedTour = tourInList;
                TourListViewModel.ModifySelectedTour();
            }

            OnPropertyChanged(nameof(TourListViewModel.Tours));
        }

        private void OpenModifyTourLogWindow()
        {
            if (TourLogViewModel.SelectedTourLog != null)
            {
                var modifyTourLogWindow = new ModifyTourLogView(TourLogViewModel);
                modifyTourLogWindow.ShowDialog();
            }
        }
        private void GenerateSingleTourReport()
        {
            try
            {
                var logs = TourLogViewModel.TourLogs?.ToList() ?? new List<TourLog>();
                ReportGenerator.GenerateSingleTourReport(SelectedTour, logs);
                LoggerHelper.Info($"Generated report for tour '{SelectedTour.name}' (ID: {SelectedTour.Id})");
                MessageBox.Show("Report created successfully!", "PDF Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Failed to generate tour report.", ex);
                MessageBox.Show("Failed to create report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async void GenerateSummaryReport()
        {
            try
            {
                var tourRepo = new TourRepository(new TourPlannerContextFactory().CreateDbContext(null));
                var logRepo = new TourLogRepository(new TourPlannerContextFactory().CreateDbContext(null));

                var allTours = (await tourRepo.GetAllToursAsync()).ToList();
                var logsPerTour = await logRepo.GetAllLogsGroupedByTourAsync();

                ReportGenerator.GenerateSummaryReport(allTours, logsPerTour);
                LoggerHelper.Info("Generated summary report.");
                MessageBox.Show("Summary report created successfully!", "PDF Report", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Failed to generate summary report.", ex);
                MessageBox.Show("Failed to create summary report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ObservableCollection<Tour> TopTours { get; set; } = new();

        private async void LoadTopTours()
        {
            var repo = new TourLogRepository(new TourPlannerContextFactory().CreateDbContext(null));
            var groupedLogs = await repo.GetAllLogsGroupedByTourAsync();

            var tourRepo = new TourRepository(new TourPlannerContextFactory().CreateDbContext(null));
            var allTours = await tourRepo.GetAllToursAsync();

            var top = groupedLogs
                .OrderByDescending(g => g.Value.Count)
                .Take(3)
                .Select(g => {
                    var tour = allTours.FirstOrDefault(t => t.Id == g.Key);
                    if (tour != null)
                    {
                        tour.TourLogs = g.Value;
                        return tour;
                    }
                    return null;
                })
                .Where(t => t != null)
                .ToList();

            TopTours.Clear();
            foreach (var tour in top)
                TopTours.Add(tour);
        }
        
        public string? MapImageFullPath
        {
            get
            {
                if (SelectedTour != null && !string.IsNullOrWhiteSpace(SelectedTour.mapImagePath))
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory!;
                    string folder = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
                    string fullPath = Path.Combine(folder, SelectedTour.mapImagePath);

                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }

                return null;
            }
        }

        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}