using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TourGuide.PresentationLayer.ViewModels;

namespace TourGuide.PresentationLayer.Views
{
    public partial class AddTourLogView : Window
    {
        public AddTourLogView(TourLogViewModel tourLogViewModel, int selectedTourId)
        {
            InitializeComponent();
            var viewModel = new AddTourLogViewModel(tourLogViewModel, selectedTourId);
            viewModel.CloseWindow = () => this.DialogResult = true;
            DataContext = viewModel;
        }
    }
}
