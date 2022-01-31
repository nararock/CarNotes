using CarNotes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RefuelEventCreateModel
    {
        public double? LastMileage { get; set; }
        public FuelType LastFuel { get; set; }
    }
}