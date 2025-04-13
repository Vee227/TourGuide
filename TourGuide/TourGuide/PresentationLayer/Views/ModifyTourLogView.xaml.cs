using System.Windows;
using TourGuide.PresentationLayer.ViewModels;

namespace TourGuide.PresentationLayer.Views
{
    public partial class ModifyTourLogView : Window
    {
        public ModifyTourLogView(TourLogViewModel tourLogViewModel)
        {
            InitializeComponent();
            var viewModel = new ModifyTourLogViewModel(tourLogViewModel);
            viewModel.CloseWindow = () => this.DialogResult = true;
            DataContext = viewModel;
        }
    }
}