using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.Comands;
using TourGuide.Models;
using TourGuide.Views;

namespace TourGuide.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TourGuide", "tours.json");

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

        public ObservableCollection<Tour> Tours { get; set; }

        public ICommand AddTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand ModifyTourCommand { get; }

        public MainWindowViewModel()
        {
            _tourListViewModel = new TourListViewModel();
            Tours = _tourListViewModel.Tours;

            AddTourCommand = new RelayCommand(_ => OpenAddTourWindow());
            DeleteTourCommand = new RelayCommand(_ => DeleteTour(), _ => SelectedTour != null);
            ModifyTourCommand = new RelayCommand(_ => ModifyTour(), _ => SelectedTour != null);
        }

        private void OpenAddTourWindow()
        {
            var addTourWindow = new AddTourView(TourListViewModel);
            addTourWindow.ShowDialog();
        }

        private void DeleteTour()
        {
            if (SelectedTour != null)
            {
                _tourListViewModel.DeleteTour(SelectedTour);
                SelectedTour = null;
                OnPropertyChanged(nameof(Tours));
            }
        }

        private void ModifyTour()
        {
            if (SelectedTour != null)
            {
                _tourListViewModel.ModifySelectedTour();
                OnPropertyChanged(nameof(Tours));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}