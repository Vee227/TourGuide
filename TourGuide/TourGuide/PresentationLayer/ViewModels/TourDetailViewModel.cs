using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourGuide.DataLayer.Models;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;
using System.ComponentModel;

namespace TourGuide.PresentationLayer.ViewModels
{
    class TourDetailViewModel : INotifyPropertyChanged
    {
        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        public ICommand ModifyCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly TourListViewModel _tourListViewModel;

        public TourDetailViewModel(Tour tour, TourListViewModel tourListViewModel)
        {
            SelectedTour = tour;
            _tourListViewModel = tourListViewModel;

            ModifyCommand = new RelayCommand(_ => ModifyTour());
            DeleteCommand = new RelayCommand(_ => DeleteTour());
        }

        private void ModifyTour()
        {
            // Implement modify logic (e.g., open a dialog to edit the tour)
            Console.WriteLine($"Modifying {SelectedTour.name}");
        }

        private void DeleteTour()
        {
            // Implement delete logic (e.g., remove the tour from the list)
            _tourListViewModel.DeleteTour(SelectedTour);
            Console.WriteLine($"Deleting {SelectedTour.name}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}