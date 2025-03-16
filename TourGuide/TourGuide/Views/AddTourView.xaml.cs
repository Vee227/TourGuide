using System;
using System.Windows;
using TourGuide.Models;
using TourGuide.ViewModels;

namespace TourGuide.Views
{
    public partial class AddTourView : Window
    {
        private readonly TourListViewModel _tourListVM;

        public AddTourView(TourListViewModel tourListViewModel)
        {
            InitializeComponent();
            _tourListVM = tourListViewModel;
        }

        private void SaveTour_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TourNameTextBox.Text))
            {
                MessageBox.Show("Tour Name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(StartLocationTextBox.Text))
            {
                MessageBox.Show("Start Location cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(EndLocationTextBox.Text))
            {
                MessageBox.Show("End Location cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(DistanceTextBox.Text, out int distance) || distance <= 0)
            {
                MessageBox.Show("Distance must be a valid positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Tour newTour = new Tour
                {
                    name = TourNameTextBox.Text,
                    description = DescriptionTextBox.Text,
                    startLocation = StartLocationTextBox.Text,
                    endLocation = EndLocationTextBox.Text,
                    transporttype = TransportTypeComboBox.Text,
                    distance = distance,
                };

                _tourListVM.AddTour(newTour);
                DialogResult = true; // Set dialog result to true
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
