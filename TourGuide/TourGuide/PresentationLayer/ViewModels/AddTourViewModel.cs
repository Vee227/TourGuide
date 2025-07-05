using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Repositories;
using TourGuide.DataLayer;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class AddTourViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Tour NewTour { get; set; } = new Tour();
        public List<string> TransportTypes { get; } = new List<string> { "Bike", "Hike", "Run", "Rollerskates" };
        public Action CloseWindow { get; set; }

        public ICommand SaveTourCommand { get; }

        private readonly TourListViewModel _tourListVM;

        public AddTourViewModel(TourListViewModel tourListViewModel)
        {
            _tourListVM = tourListViewModel;
            SaveTourCommand = new RelayCommand(_ => SaveTour());
        }

        //Damit fügen wir eine neue Tour ein - validiert - stellt Verbindung mit DB her und 
        //fügt das mit Repository ein - updatet UI und schließt das Fenster wieder.
        public async void SaveTour()
        {
            if (string.IsNullOrWhiteSpace(NewTour.name) ||
                string.IsNullOrWhiteSpace(NewTour.startLocation) ||
                string.IsNullOrWhiteSpace(NewTour.endLocation) ||
                NewTour.distance <= 0 || NewTour.estimatedTime <= 0)
            {
                MessageBox.Show("Please fill all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repository = new TourRepository(context);
                await repository.AddTourAsync(NewTour);
                
                _tourListVM.LoadTours();

                MessageBox.Show("Tour added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving the tour: " + ex.Message);
            }
        }
    }
}
