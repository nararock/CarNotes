﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RefuelModelOutput
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public double Mileage { get; set; }
        public string Fuel { get; set; }
        public string Station { get; set; }
        public double Volume { get; set; }
        public double PricePerOneLiter { get; set; }
        public bool FullTank { get; set; }
        public bool ForgotRecordPreviousGasStation { get; set; }
    }
}