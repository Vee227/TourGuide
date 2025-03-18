using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourGuide.Models
{
   public class Tour
    {
        public string name { get; set; } = string.Empty;        
        public string description { get; set; } = string.Empty; 
        public string startLocation { get; set; } = string.Empty;
        public string endLocation { get; set; } = string.Empty;
        public string transporttype { get; set; } = string.Empty;
        public double distance { get; set; }
        public int estimatedTime { get; set; }
 
        public Tour() { }

        public Tour(string name, string description, string startLocation, string endLocation, string transportType, double distance, int estimatedTime)
        {
            this.name = name;
            this.description = description;
            this.startLocation = startLocation;
            this.endLocation = endLocation;
            this.transporttype = transportType;
            this.distance = distance;
            this.estimatedTime = estimatedTime;
        }

        public override string ToString()
        {
            return name;
        }

    }


}
