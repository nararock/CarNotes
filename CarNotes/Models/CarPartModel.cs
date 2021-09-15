using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class CarPartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CarManufacturer { get; set; }
        public string Article { get; set; }
        public double Price { get; set; }
        public int CarSubsystem { get; set; }
        public CarSubsystemModel CarSubsystemModel { get; set; }
        public bool? IsDeleted { get; set; }
    }
}