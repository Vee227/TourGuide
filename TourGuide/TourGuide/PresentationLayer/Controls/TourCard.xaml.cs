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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourGuide.DataLayer.Models;

namespace TourGuide.PresentationLayer.Controls
{
    public partial class TourCard : UserControl
    {
        public static readonly DependencyProperty TourProperty =
            DependencyProperty.Register("Tour", typeof(Tour), typeof(TourCard), new PropertyMetadata(null));

        public Tour Tour
        {
            get => (Tour)GetValue(TourProperty);
            set => SetValue(TourProperty, value);
        }

        public TourCard()
        {
            InitializeComponent();
        }
    }
}
