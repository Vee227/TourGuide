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

        private void SaveTour()
        {
            MessageBox.Show($"Debug Info:\n" +
                    $"Name: {NewTour.name}\n" +
                    $"Description: {NewTour.description}\n" +
                    $"Start: {NewTour.startLocation}\n" +
                    $"End: {NewTour.endLocation}\n" +
                    $"Transport: {NewTour.transporttype}\n" +
                    $"Distance: {NewTour.distance}\n" +
                    $"Time: {NewTour.estimatedTime}",
                    "Debugging", MessageBoxButton.OK, MessageBoxImage.Information);

            if (string.IsNullOrWhiteSpace(NewTour.name) ||
                string.IsNullOrWhiteSpace(NewTour.description) ||
                string.IsNullOrWhiteSpace(NewTour.startLocation) ||
                string.IsNullOrWhiteSpace(NewTour.endLocation) ||
                string.IsNullOrWhiteSpace(NewTour.transporttype) || 
                NewTour.distance <= 0 ||
                NewTour.estimatedTime <= 0)
            {
                MessageBox.Show("Please fill all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _tourListVM.AddTour(NewTour);
            MessageBox.Show("Tour added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            CloseWindow?.Invoke();
        }

    }
}
