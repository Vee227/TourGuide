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

        public TourDetailViewModel(Tour selectedTour)
        {
            SelectedTour = selectedTour;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}