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

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Description cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            string[] validTransportTypes = { "Bike", "Hike", "Run", "Rollerskates" };

            if (!validTransportTypes.Contains(TransportTypeComboBox.Text))
            {
                MessageBox.Show("Invalid transport type selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(DistanceTextBox.Text, out double distance) || distance <= 0)
            {
                MessageBox.Show("Distance must be a valid positive number (e.g., 10.5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(TimeTextBox.Text, out int estimatedTime) || estimatedTime <= 0)
            {
                MessageBox.Show("Time must be a valid positive number (e.g., 10).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    estimatedTime = estimatedTime,
                };

                _tourListVM.AddTour(newTour);
                DialogResult = true; // Setzt den Dialog als erfolgreich geschlossen
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
