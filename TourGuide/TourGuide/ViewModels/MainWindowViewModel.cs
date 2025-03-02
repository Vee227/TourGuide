using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TourGuide.Comands;

namespace TourGuide.ViewModels
{
    class MainWindowViewModel
    {
        private void DeleteItem()
        {
            if (SelectedItem != null)
                Items.Remove(SelectedItem);
        }

        public RelayCommand 
    }
}
