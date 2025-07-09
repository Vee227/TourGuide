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
using log4net;
using TourGuide.Logs;
using TourGuide.BusinessLayer;

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
                
                LoggerHelper.Info($"Tour '{tour.name}' deleted (ID: {tour.Id}).");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Failed to delete tour '{tour?.name}'", ex);
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
                LoggerHelper.Info($"Tour '{SelectedTour.name}' updated (ID: {SelectedTour.Id}).");
                Console.WriteLine("Tour saved");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"Failed to update tour '{SelectedTour?.name}'", ex);
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

        
        private readonly ITourRepository _tourRepository = new TourRepository(new TourPlannerContextFactory().CreateDbContext(null));
        public async Task LoadTours()
        {
            var loadedTours = await _tourRepository.GetAllToursAsync();

            var logRepo = new TourLogRepository(new TourPlannerContextFactory().CreateDbContext(null));
            var allLogsGrouped = await logRepo.GetAllLogsGroupedByTourAsync();

            foreach (var tour in loadedTours)
            {
                if (allLogsGrouped.TryGetValue(tour.Id, out var logs))
                    tour.TourLogs = logs;
                else
                    tour.TourLogs = new List<TourLog>();
            }

            Tours.Clear();
            foreach (var tour in loadedTours)
                Tours.Add(tour);
            
            AllTours = loadedTours.ToList();

        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}