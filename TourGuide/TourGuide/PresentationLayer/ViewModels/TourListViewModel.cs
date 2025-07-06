using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TourGuide.PresentationLayer.ViewModels;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.Controls;
using System.ComponentModel;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer;
using TourGuide.DataLayer.Repositories;


namespace TourGuide.PresentationLayer.ViewModels
{
     public class TourListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; private set; } = new();
        public ObservableCollection<TourCardViewModel> TourCards { get; private set; } = new();
        
        public List<Tour> AllTours { get; private set; } = new();

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

        public ICommand ModifyTourCommand { get; }

        public TourListViewModel()
        {
            LoadTours();
            ModifyTourCommand = new RelayCommand(_ => ModifySelectedTour(), _ => SelectedTour != null);
        }

        public async void DeleteTour(Tour tour)
        {
            if (tour == null) return;

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repository = new TourRepository(context);

                await repository.DeleteTourAsync(tour.Id);

                Tours.Remove(tour);
                AllTours.Remove(tour);

                var cardToRemove = TourCards.FirstOrDefault(c => c.Tour == tour);
                if (cardToRemove != null)
                    TourCards.Remove(cardToRemove);

                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(TourCards));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting the tour: {ex.Message}");
            }
        }

        public async void ModifySelectedTour()
        {
            if (SelectedTour == null) return;

            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repository = new TourRepository(context);

                await repository.UpdateTourAsync(SelectedTour);
                Console.WriteLine("Tour saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Saving the tour failed: {ex.Message}");
            }

            OnPropertyChanged(nameof(Tours));
            OnPropertyChanged(nameof(TourCards));
        }

        public TourCardViewModel CreateCardViewModel(Tour tour)
        {
            return new TourCardViewModel(
                tour,
                new RelayCommand(_ => EditTour(tour)),
                new RelayCommand(_ => DeleteTour(tour))
            );
        }

        private void EditTour(Tour tour)
        {
            SelectedTour = tour;
            ModifySelectedTour();
        }

        public async void LoadTours()
        {
            try
            {
                var factory = new TourPlannerContextFactory();
                using var context = factory.CreateDbContext(null);
                var repository = new TourRepository(context);

                var loadedTours = await repository.GetAllToursAsync();
                
                AllTours = loadedTours.ToList();
                
                Tours.Clear();
                foreach (var tour in AllTours)
                    Tours.Add(tour);
                
                TourCards.Clear();
                foreach (var tour in Tours)
                    TourCards.Add(CreateCardViewModel(tour));

                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(TourCards));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load tours: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}