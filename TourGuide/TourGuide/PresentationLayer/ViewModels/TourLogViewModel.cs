using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using TourGuide.PresentationLayer.Comands;
using TourGuide.DataLayer.Models;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class TourLogViewModel : INotifyPropertyChanged
    {
        private static readonly string LogFilePath = Path.Combine(
            Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName,
            "tourlogs.json"
        );

        public ObservableCollection<TourLog> TourLogs { get; set; } = new ObservableCollection<TourLog>();

        private TourLog _selectedTourLog;
        public TourLog SelectedTourLog
        {
            get => _selectedTourLog;
            set
            {
                _selectedTourLog = value;
                OnPropertyChanged(nameof(SelectedTourLog));
            }
        }

        public ICommand AddTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand ModifyTourLogCommand { get; }

        public TourLogViewModel()
        {
            AddTourLogCommand = new RelayCommand(_ => OpenAddTourLogWindow());
            DeleteTourLogCommand = new RelayCommand(_ => DeleteTourLog(), _ => SelectedTourLog != null);
            ModifyTourLogCommand = new RelayCommand(_ => OpenModifyTourLogWindow(), _ => SelectedTourLog != null);
        }

        public void LoadTourLogs(string? tourName)
        {
            try
            {
                if (File.Exists(LogFilePath))
                {
                    var json = File.ReadAllText(LogFilePath);
                    var loadedLogs = JsonSerializer.Deserialize<ObservableCollection<TourLog>>(json);

                    if (loadedLogs != null)
                    {
                        TourLogs = new ObservableCollection<TourLog>(
                            loadedLogs.Where(log => log.TourName == tourName)
                        );
                        OnPropertyChanged(nameof(TourLogs));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der TourLogs: {ex.Message}");
            }
        }

        public void AddTourLog(TourLog newLog)
        {
            TourLogs.Add(newLog);
            SaveTourLogs();
            OnPropertyChanged(nameof(TourLogs));
        }

        public void DeleteTourLog()
        {
            if (SelectedTourLog != null)
            {
                TourLogs.Remove(SelectedTourLog);
                SaveTourLogs();
                SelectedTourLog = null;
                OnPropertyChanged(nameof(TourLogs));
            }
        }

        public void ModifyTourLog()
        {
            SaveTourLogs();
            OnPropertyChanged(nameof(TourLogs));
        }

        private void OpenModifyTourLogWindow()
        {
            var modifyWindow = new Views.ModifyTourLogView(this);
            modifyWindow.ShowDialog();
        }

        private void SaveTourLogs()
        {
            try
            {
                var json = JsonSerializer.Serialize(TourLogs, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LogFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der TourLogs: {ex.Message}");
            }
        }

        private void OpenAddTourLogWindow()
        {
            var addTourLogWindow = new Views.AddTourLogView(this, "Selected Tour");
            addTourLogWindow.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}