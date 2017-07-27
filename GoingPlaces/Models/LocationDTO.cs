using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoingPlaces.Models
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}