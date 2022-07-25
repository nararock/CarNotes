using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RefuelModelOutput
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Mileage { get; set; }
        public int Fuel { get; set; }
        public string Station { get; set; }
        public double Volume { get; set; }
        public double PricePerOneLiter { get; set; }
        public double Cost { get; set; }
        public bool FullTank { get; set; }
        public bool ForgotRecordPreviousGasStation { get; set; }
        public bool WrongMileage { get; set; }
    }
}