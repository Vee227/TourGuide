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
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.Comands;

namespace TourGuide.ViewModels
{
    public class TourListViewModel : INotifyPropertyChanged
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TourGuide", "tours.json");

        public ObservableCollection<Tour> Tours { get; set; }

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
            Tours = new ObservableCollection<Tour>();
            LoadTours();
            ModifyTourCommand = new RelayCommand(_ => ModifySelectedTour(), _ => SelectedTour != null);
        }

        public void AddTour(Tour newTour)
        {
            Tours.Add(newTour);
            SaveTours();
            OnPropertyChanged(nameof(Tours));
        }

        public void DeleteTour(Tour tour)
        {
            if (tour != null)
            {
                Tours.Remove(tour);
                SaveTours();
                OnPropertyChanged(nameof(Tours));
            }
        }

        public void ModifySelectedTour()
        {
            if (SelectedTour == null) return;

            var existingTour = Tours.FirstOrDefault(t => t.name == SelectedTour.name);
            if (existingTour != null)
            {
                existingTour.description = SelectedTour.description;
                existingTour.startLocation = SelectedTour.startLocation;
                existingTour.endLocation = SelectedTour.endLocation;
                existingTour.transporttype = SelectedTour.transporttype;
                existingTour.distance = SelectedTour.distance;

                SaveTours();
                OnPropertyChanged(nameof(Tours));
            }
        }

        private void SaveTours()
        {
            try
            {
                var directory = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(Tours, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Touren: {ex.Message}");
            }
        }

        private void LoadTours()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    var loadedTours = JsonSerializer.Deserialize<ObservableCollection<Tour>>(json);
                    if (loadedTours != null)
                    {
                        foreach (var tour in loadedTours)
                            Tours.Add(tour);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Touren: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

