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
    public class TourDetailViewModel : INotifyPropertyChanged
    {
        public Tour SelectedTour { get; }

        public List<string> TransportTypes { get; } = new()
        {
            "Bike",
            "Hike",
            "Run",
            "Car"
        };

        public TourDetailViewModel(Tour selectedTour)
        {
            SelectedTour = selectedTour;
            OnPropertyChanged(nameof(SelectedTour));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}