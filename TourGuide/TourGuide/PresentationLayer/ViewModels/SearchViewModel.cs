using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using TourGuide.DataLayer.Repositories;
using TourGuide.DataLayer.Models;
using System.Collections.ObjectModel;
using TourGuide.Logs;
using log4net;
using TourGuide.BusinessLayer;

namespace TourGuide.PresentationLayer.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
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
                    PerformSearch();
                }
            }
        }

        private readonly TourListViewModel _tourListViewModel;

        public SearchViewModel(TourListViewModel tourListViewModel)
        {
            _tourListViewModel = tourListViewModel;
        }

        private void PerformSearch()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchInput))
                {
                    _tourListViewModel.Tours.Clear();
                    foreach (var tour in _tourListViewModel.AllTours)
                        _tourListViewModel.Tours.Add(tour);

                    LoggerHelper.Info("Search input empty → Resetting to full tour list.");
                    return;
                }

                var repo = new SearchService();
                var filtered = repo.FilterTours(_tourListViewModel.AllTours, SearchInput);

                _tourListViewModel.Tours.Clear();
                foreach (var tour in filtered)
                    _tourListViewModel.Tours.Add(tour);

                LoggerHelper.Info($"Search triggered: \"{SearchInput}\" → {filtered.Count()} of {_tourListViewModel.AllTours.Count} tours matched.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Error while performing search.", ex);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
