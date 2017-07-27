using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoingPlaces.Models
{
    public class Images
    {
        //Primary Key

        public int Id { get; set; }

        //Foreign Key
        public int LocationId { get; set; }

        //Navigation Property => helps keep the relation non circular
        public Location Location { get; set; }

        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }

        //Main Image == Try to give 3 images per landmark
        //Save images as binary data ==> byte arrays
        public byte[] Image1 { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }
    }
}