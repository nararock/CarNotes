using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ReleaseYear { get; set; }
        public string Color { get; set; }
        public string Body { get; set; }
    }
}