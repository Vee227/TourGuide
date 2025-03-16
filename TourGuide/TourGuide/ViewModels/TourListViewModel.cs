using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using TourGuide.ViewModels;
using TourGuide.Models;
using TourGuide.Controls;

namespace TourGuide.ViewModels
{
    public class TourListViewModel
    {
        public ObservableCollection<TourCardViewModel> TourCards { get; set; }

        public TourListViewModel()
        {
            TourCards = new ObservableCollection<TourCardViewModel>
            {
                new TourCardViewModel(new Tour { name = "Mountain Trail", description = "A scenic mountain route", transporttype = "Bike", distance = 120 }),
                new TourCardViewModel(new Tour { name = "Lake Loop", description = "A peaceful loop around the lake", transporttype = "Walking", distance = 45 })
            };
        }
    }
}
