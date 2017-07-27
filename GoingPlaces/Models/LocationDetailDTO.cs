using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoingPlaces.Models
{
    public class LocationDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public byte[] Image1;
        public string ImageDescription1 { get; set; }

        public byte[] Image2;
        public string ImageDescription2 { get; set; }

        public byte[] Image3;
        public string ImageDescription3 { get; set; }
    }
}