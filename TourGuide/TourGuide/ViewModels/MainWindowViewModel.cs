using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TourGuide.Comands;
using TourGuide.ViewModels;
using TourGuide.Views;
using TourGuide.Comands;

namespace TourGuide.ViewModels
{
    class MainWindowViewModel
    {
        public object TourListView { get; set; }

        public MainWindowViewModel()
        {
            TourListView = new TourListView();
            CurrentView = new TourListView();
            ShowTourListCommand = new RelayCommand(_ => CurrentView = new TourListView());
        }

        public object CurrentView { get; set; }
        public ICommand ShowTourListCommand { get; }

    }
}
