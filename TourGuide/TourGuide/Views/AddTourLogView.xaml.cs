using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TourGuide.Models;
using TourGuide.ViewModels;

namespace TourGuide.Views
{
    public partial class AddTourLogView : Window
    {
        private readonly TourLogViewModel _tourLogViewModel;
        private readonly string _selectedTourName;

        public AddTourLogView(TourLogViewModel tourLogViewModel, string selectedTourName)
        {
            InitializeComponent();
            _tourLogViewModel = tourLogViewModel;
            _selectedTourName = selectedTourName;
        }

        private void SaveTourLog_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DateTextBox.Text) || !DateTime.TryParse(DateTextBox.Text, out _))
            {
                MessageBox.Show("Please enter a valid date (YYYY-MM-DD).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(CommentTextBox.Text))
            {
                MessageBox.Show("Comment cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DifficultyComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a difficulty (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(TotalTimeTextBox.Text, out int totalTime) || totalTime <= 0)
            {
                MessageBox.Show("Total time must be a positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RatingComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a rating (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                TourLog newTourLog = new TourLog
                {
                    TourName = _selectedTourName,
                    Date = DateTextBox.Text,
                    Comment = CommentTextBox.Text,
                    Difficulty = int.Parse(((System.Windows.Controls.ComboBoxItem)DifficultyComboBox.SelectedItem).Content.ToString()),
                    TotalTime = totalTime,
                    Rating = int.Parse(((System.Windows.Controls.ComboBoxItem)RatingComboBox.SelectedItem).Content.ToString())
                };

                _tourLogViewModel.AddTourLog(newTourLog);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
