using System;
using System.Windows.Input;
using TourGuide.Controls;
using TourGuide.Models;
using TourGuide.ViewModels;
using TourGuide.Comands;

namespace TourGuide.ViewModels
{
    public class TourCardViewModel
    {
        public Tour Tour { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public TourCardViewModel(Tour tour)
        {
            Tour = tour;
            EditCommand = new RelayCommand(EditTour);
            DeleteCommand = new RelayCommand(DeleteTour);
        }

        private void EditTour(object obj)
        {
            // Implement edit logic (open tour detail view)
            Console.WriteLine($"Editing {Tour.name}");
        }

        private void DeleteTour(object obj)
        {
            // Implement delete logic
            Console.WriteLine($"Deleting {Tour.name}");
        }
    }
}
