using System;
using System.ComponentModel;
using System.Windows.Input;
using TourGuide.Comands;
using TourGuide.Models;
using TourGuide.Views;

namespace TourGuide.ViewModels
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

                    // Re-evaluate the CanExecute status of the ShowTourDetailCommand
                    //(ShowTourDetailCommand as RelayCommand)?.RaiseCanExecuteChanged();

                    // Show the details of the selected tour
                    ShowTourDetails();
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

        public ICommand ShowTourListCommand { get; }
        public ICommand ShowTourDetailCommand { get; }
        public ICommand OpenAddTourViewCommand { get; }

        public MainWindowViewModel()
        {
            _tourListViewModel = new TourListViewModel();
            CurrentView = new TourListView { DataContext = _tourListViewModel };

            ShowTourListCommand = new RelayCommand(_ => CurrentView = new TourListView { DataContext = _tourListViewModel });
            ShowTourDetailCommand = new RelayCommand(_ => ShowTourDetails(), _ => SelectedTour != null);
            OpenAddTourViewCommand = new RelayCommand(_ => OpenAddTourView());
        }

        private void ShowTourDetails()
        {
            if (SelectedTour != null)
            {
                CurrentView = new TourDetailView { DataContext = new TourDetailViewModel(SelectedTour, _tourListViewModel) };
            }
        }

        private void OpenAddTourView()
        {
            AddTourView addTourView = new AddTourView(_tourListViewModel);
            bool? result = addTourView.ShowDialog();

            // If the dialog was closed with a successful result, refresh the tour list
            if (result == true)
            {
                // Notify the UI that the Tours collection has been updated
                OnPropertyChanged(nameof(TourListViewModel));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}