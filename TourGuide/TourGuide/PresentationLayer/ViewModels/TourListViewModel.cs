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
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class TourListViewModel : INotifyPropertyChanged
    {
        private static readonly string FilePath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "tours.json");

        public ObservableCollection<Tour> Tours { get; private set; } = new();
        public ObservableCollection<TourCardViewModel> TourCards { get; private set; } = new();

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

        public void AddTour(Tour newTour)
        {
            if (string.IsNullOrWhiteSpace(newTour.name) ||
                string.IsNullOrWhiteSpace(newTour.startLocation) ||
                string.IsNullOrWhiteSpace(newTour.endLocation) ||
                newTour.distance <= 0 || newTour.estimatedTime <= 0)
            {
                throw new ArgumentException("Ungültige Tourdaten.");
            }

            Tours.Add(newTour);
            TourCards.Add(CreateCardViewModel(newTour));
            SaveTours();
            OnPropertyChanged(nameof(Tours));
            OnPropertyChanged(nameof(TourCards));
        }

        public void DeleteTour(Tour tour)
        {
            if (tour != null)
            {
                Tours.Remove(tour);

                var cardToRemove = TourCards.FirstOrDefault(c => c.Tour == tour);
                if (cardToRemove != null)
                    TourCards.Remove(cardToRemove);

                SaveTours();
                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(TourCards));
            }
        }

        public void ModifySelectedTour()
        {
            if (SelectedTour == null) return;

            var existingTour = Tours.FirstOrDefault(t => t.name == SelectedTour.name);
            if (existingTour != null)
            {
                existingTour.name = SelectedTour.name;
                existingTour.description = SelectedTour.description;
                existingTour.startLocation = SelectedTour.startLocation;
                existingTour.endLocation = SelectedTour.endLocation;
                existingTour.transporttype = SelectedTour.transporttype;
                existingTour.distance = SelectedTour.distance;
                existingTour.estimatedTime = SelectedTour.estimatedTime;

                SaveTours();
                
                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(TourCards));
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        private TourCardViewModel CreateCardViewModel(Tour tour)
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

        public void SaveTours()
        {
            try
            {
                var json = JsonSerializer.Serialize(Tours, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
                });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Touren: {ex.Message}");
            }
        }

        public void LoadTours()
        {
            try
            {
                Tours.Clear();
                TourCards.Clear();

                if (!File.Exists(FilePath))
                {
                    Tours = new ObservableCollection<Tour>();
                    return;
                }

                var json = File.ReadAllText(FilePath);
                var loadedTours = JsonSerializer.Deserialize<ObservableCollection<Tour>>(json) ?? new();

                Tours = loadedTours;

                foreach (var tour in Tours)
                {
                    TourCards.Add(CreateCardViewModel(tour));
                }

                OnPropertyChanged(nameof(Tours));
                OnPropertyChanged(nameof(TourCards));
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: Ungültiges JSON Format.");
                Tours = new ObservableCollection<Tour>();
                TourCards = new ObservableCollection<TourCardViewModel>();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}