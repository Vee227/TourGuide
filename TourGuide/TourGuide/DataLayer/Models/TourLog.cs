using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourGuide.DataLayer.Models
{
    public class TourLog
    {
        public int Id { get; set; }

        public int TourId { get; set; }           // foreign key
        public Tour Tour { get; set; }            // navigation property
        public string TourName { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public int Difficulty { get; set; } 
        public int TotalTime { get; set; } 
        public int Rating { get; set; } 
    }
}
