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
using log4net;
using TourGuide.Logs;
using TourGuide.BusinessLayer;


namespace TourGuide.PresentationLayer.ViewModels
{
     public class AddTourViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Tour NewTour { get; set; } = new Tour();
        public List<string> TransportTypes { get; } = new List<string> { "Bike", "Hike", "Run", "Car" };
        public Action CloseWindow { get; set; }

        public ICommand SaveTourCommand { get; }

        private readonly TourListViewModel _tourListVM;

        public AddTourViewModel(TourListViewModel tourListViewModel)
        {
            _tourListVM = tourListViewModel;
            SaveTourCommand = new RelayCommand(_ => SaveTour());
        }
        
        public async void SaveTour()
        {
            if (string.IsNullOrWhiteSpace(NewTour.name) ||
                string.IsNullOrWhiteSpace(NewTour.startLocation) ||
                string.IsNullOrWhiteSpace(NewTour.endLocation))
            {
                MessageBox.Show("Please fill all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LoggerHelper.Warn("Tour validation failed. Tour not saved.");
                return;
            }

            try
            {
                var routeService = new RouteService();
                string moveType = (NewTour.transporttype ?? "").ToLower() switch
                {
                    "bike" => "cycling-regular",
                    "hike" => "foot-hiking",
                    "run" => "foot-walking",
                    "car" => "driving-car",
                    _ => "foot-walking"
                };
                
                double startLat = await GeoCoder.GetLat(NewTour.startLocation);
                double startLng = await GeoCoder.GetLng(NewTour.startLocation);
                double endLat = await GeoCoder.GetLat(NewTour.endLocation);
                double endLng = await GeoCoder.GetLng(NewTour.endLocation);
              
                

                var result = await routeService.GetRouteAsync(startLat, startLng, endLat, endLng, moveType);

                if (result == null)
                {
                    MessageBox.Show("Could not get route data.", "Error");
                    return;
                }
                
                await routeService.SaveCoordinatesAsJsAsync(result.Coordinates);
                
                NewTour.distance = result.DistanceKm;
                NewTour.estimatedTime = (int)Math.Round(result.DurationMinutes);
                
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repository = new TourRepository(context);
                await repository.AddTourAsync(NewTour); 
                
                string safeName = string.Concat(NewTour.name.Split(System.IO.Path.GetInvalidFileNameChars()));
                string filename = $"{safeName}_{NewTour.Id}.png";
                
                await routeService.SaveMapScreenshotAsync(filename);
                
                string relativePath = System.IO.Path.Combine("Assets", "Maps", filename);
                NewTour.mapImagePath = relativePath;
                
                await repository.UpdateTourAsync(NewTour);

                _tourListVM.LoadTours();

                LoggerHelper.Info($"Tour '{NewTour.name}' created (Start: {NewTour.startLocation}, Destination: {NewTour.endLocation}).");
                MessageBox.Show("Tour added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindow?.Invoke();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Error while saving new tour.", ex);
                MessageBox.Show("Error saving the tour: " + ex.Message);
            }
        }


    }

}
