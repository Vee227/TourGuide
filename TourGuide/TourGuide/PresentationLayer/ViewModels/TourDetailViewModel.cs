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
    //Das Tourdetail hier ist einfach nur mal zum Anzeigen der Daten (sind editierbar) - die
    //Logik zum modifyen ist in TourListViewModel (wir updaten ja indem wir die Felder
    //editieren und dann auf den Modify Button klicken is vlt eh ein etwas komischer
    //Weg aber es funktioniert und wir müssen zuerst das andere fertig bekommen
    public class TourDetailViewModel : INotifyPropertyChanged
    {
        public Tour SelectedTour { get; }

        public List<string> TransportTypes { get; } = new()
        {
            "Bike",
            "Hike",
            "Run",
            "Rollerskates"
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