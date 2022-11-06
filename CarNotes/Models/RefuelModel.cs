using CarNotes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RefuelModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Mileage { get; set; }
        public FuelType Fuel { get; set; }
        public string FuelName { get; set; }
        public int Station { get; set; }
        public string StationName { get; set; }
        public string CustomStation { get; set; }
        public string Volume { get; set; }
        public double PricePerOneLiter { get; set; }
        public double Cost { get; set; } 
        public bool FullTank { get; set; }
        public bool ForgotRecordPreviousGasStation { get; set; }
    }
}