using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Models;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}