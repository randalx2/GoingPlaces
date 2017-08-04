using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoingPlaces.Models
{
    public class Location
    {
        //Primary Key
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //Each location has many images ==> 1 to many relationship
        //List of courses under this author
        //Shows a 1 to many relationship between authors and courses
        public IList<Images> Images { get; set; }
    }
}