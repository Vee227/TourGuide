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
using TourGuide.ViewModels;

namespace TourGuide.Views
{
    public partial class ModifyTourLogView : Window
    {
        private readonly TourLogViewModel _tourLogViewModel;

        public ModifyTourLogView(TourLogViewModel tourLogViewModel)
        {
            InitializeComponent();
            _tourLogViewModel = tourLogViewModel;
            DataContext = _tourLogViewModel;
        }

        private void SaveTourLog_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_tourLogViewModel.SelectedTourLog.Date) ||
                !DateTime.TryParse(_tourLogViewModel.SelectedTourLog.Date, out _))
            {
                MessageBox.Show("Please enter a valid date (YYYY-MM-DD).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_tourLogViewModel.SelectedTourLog.Comment))
            {
                MessageBox.Show("Comment cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DifficultyComboBox.SelectedItem == null ||
                !int.TryParse(((ComboBoxItem)DifficultyComboBox.SelectedItem).Content.ToString(), out int difficulty) ||
                difficulty < 1 || difficulty > 5)
            {
                MessageBox.Show("Please select a valid difficulty (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _tourLogViewModel.SelectedTourLog.Difficulty = difficulty;

            if (!int.TryParse(TotalTimeTextBox.Text, out int totalTime) || totalTime <= 0)
            {
                MessageBox.Show("Total time must be a positive number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _tourLogViewModel.SelectedTourLog.TotalTime = totalTime;

            if (RatingComboBox.SelectedItem == null ||
                !int.TryParse(((ComboBoxItem)RatingComboBox.SelectedItem).Content.ToString(), out int rating) ||
                rating < 1 || rating > 5)
            {
                MessageBox.Show("Please select a valid rating (1-5).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _tourLogViewModel.SelectedTourLog.Rating = rating;

            _tourLogViewModel.ModifyTourLog();
            DialogResult = true;
            Close();
        }
    }
}

