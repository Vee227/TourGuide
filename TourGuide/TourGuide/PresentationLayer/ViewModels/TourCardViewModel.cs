using System;
using System.Windows.Input;
using TourGuide.PresentationLayer.Controls;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.ViewModels;
using TourGuide.PresentationLayer.Comands;
using System.ComponentModel;


namespace TourGuide.PresentationLayer.ViewModels
{
    public class TourCardViewModel : INotifyPropertyChanged
    {
        public Tour Tour { get; }

        public string TourName => Tour.name;
        public string Description => Tour.description;
        public string TransportType => Tour.transporttype;
        public double Distance => Tour.distance;

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public TourCardViewModel(Tour tour, ICommand editCommand, ICommand deleteCommand)
        {
            Tour = tour;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;
            
            Tour.PropertyChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(TourName));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(TransportType));
                OnPropertyChanged(nameof(Distance));
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
