using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using TourGuide.DataLayer.Repositories;
using TourGuide.PresentationLayer.Comands;

namespace TourGuide.PresentationLayer.ViewModels
{
    //Ich habs nur mal auskommentiert, weil die searchbar mit DB ZUgriff noch nicht funktioniert aber
    //es gehört einkommentiert wieder - wir brauchen das!!!
    /*public class SearchViewModel : INotifyPropertyChanged
    {
        private string _searchinput = string.Empty;
        public string SearchInput
        {
            get => _searchinput;
            set
            {
                if (_searchinput != value)
                {
                    _searchinput = value;
                    OnPropertyChanged(nameof(SearchInput));
                }
            }
        }

        public ICommand SearchCommand { get; }

        public SearchViewModel()
        {
            SearchCommand = new RelayCommand(_ => PerformSearch(), _ => !string.IsNullOrWhiteSpace(SearchInput));
        }

        private void PerformSearch()
        {
            var search = new SearchRepository();
            var searchQuery = search.BuildSearchQuery(SearchInput);

            //Das ist nur mal zum debuggen könn ma aber später weggeben wenn alles funkt
            System.Diagnostics.Debug.WriteLine(searchQuery);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }*/
}
