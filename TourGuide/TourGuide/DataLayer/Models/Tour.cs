using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


namespace TourGuide.DataLayer.Models
{
    public class Tour : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _name;
        public string name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private string _description;
        public string description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        private string _startLocation;
        public string startLocation
        {
            get => _startLocation;
            set => SetField(ref _startLocation, value);
        }

        private string _endLocation;
        public string endLocation
        {
            get => _endLocation;
            set => SetField(ref _endLocation, value);
        }

        private string _transporttype;
        public string transporttype
        {
            get => _transporttype;
            set => SetField(ref _transporttype, value);
        }

        private double _distance;
        public double distance
        {
            get => _distance;
            set => SetField(ref _distance, value);
        }

        private double _estimatedTime;
        public double estimatedTime
        {
            get => _estimatedTime;
            set => SetField(ref _estimatedTime, value);
        }

        public virtual ICollection<TourLog> TourLogs { get; set; } = new List<TourLog>();
        
        public override string ToString()
        {
            return $"{name}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
