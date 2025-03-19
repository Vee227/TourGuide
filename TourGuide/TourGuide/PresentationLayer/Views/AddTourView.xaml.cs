using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using TourGuide.DataLayer.Models;
using TourGuide.PresentationLayer.ViewModels;

namespace TourGuide.PresentationLayer.Views
{
    public partial class AddTourView : Window
    {
        public AddTourView(TourListViewModel tourListViewModel)
        {
            InitializeComponent();
            var viewModel = new AddTourViewModel(tourListViewModel);
            viewModel.CloseWindow = () => this.DialogResult = true;
            DataContext = viewModel;
        }
    }
}
