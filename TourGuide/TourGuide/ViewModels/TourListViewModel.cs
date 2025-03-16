using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TourGuide.ViewModels;
using TourGuide.Models;
using TourGuide.Controls;
using System.ComponentModel;

namespace TourGuide.ViewModels
{
    public class TourListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; set; }

        public TourListViewModel()
        {
            Tours = new ObservableCollection<Tour>();
        }

        public void AddTour(Tour newTour)
        {
            Tours.Add(newTour);
            OnPropertyChanged(nameof(Tours));  // Notify UI of the change
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DeleteTour(Tour tour)
        {
            if (tour != null)
            {
                Tours.Remove(tour);
                OnPropertyChanged(nameof(Tours));  // Notify UI of the change
            }
        }
    }
}

