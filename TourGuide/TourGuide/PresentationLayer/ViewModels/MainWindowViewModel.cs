using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.Views;

namespace TourGuide.PresentationLayer.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
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
                    TourLogViewModel.LoadTourLogs(_selectedTour?.name);
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

        public ICommand AddTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand ModifyTourCommand { get; }
        public ICommand AddTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand ModifyTourLogCommand { get; }

        public MainWindowViewModel()
        {
            _tourListViewModel = new TourListViewModel();
            TourLogViewModel = new TourLogViewModel();

            AddTourCommand = new RelayCommand(_ => OpenAddTourWindow());
            DeleteTourCommand = new RelayCommand(_ => DeleteTour(), _ => SelectedTour != null);
            ModifyTourCommand = new RelayCommand(_ => ModifyTour(), _ => SelectedTour != null);
            AddTourLogCommand = new RelayCommand(_ => OpenAddTourLogWindow(), _ => SelectedTour != null);
            DeleteTourLogCommand = new RelayCommand(_ => DeleteTourLog(), _ => TourLogViewModel.SelectedTourLog != null);
            ModifyTourLogCommand = new RelayCommand(_ => OpenModifyTourLogWindow(), _ => TourLogViewModel.SelectedTourLog != null);
        }

        private void OpenAddTourWindow()
        {
            var addTourWindow = new AddTourView(TourListViewModel);
            addTourWindow.ShowDialog();
        }

        private void OpenAddTourLogWindow()
        {
            var addTourLogWindow = new AddTourLogView(TourLogViewModel, SelectedTour.name);
            addTourLogWindow.ShowDialog();
        }

        private void DeleteTour()
        {
            if (SelectedTour != null)
            {
                _tourListViewModel.DeleteTour(SelectedTour);
                SelectedTour = null;
                OnPropertyChanged(nameof(TourListViewModel.Tours));
            }
        }

        private void DeleteTourLog()
        {
            TourLogViewModel.DeleteTourLog();
        }

        private void ModifyTour()
        {
            if (SelectedTour != null)
            {
                _tourListViewModel.ModifySelectedTour();
                _tourListViewModel.SaveTours();
                OnPropertyChanged(nameof(TourListViewModel.Tours));
            }
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
