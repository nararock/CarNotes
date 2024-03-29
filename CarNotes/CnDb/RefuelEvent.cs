﻿using CarNotes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CarNotes.CnDb
{
    public class RefuelEvent
    {
        [Key]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double Mileage { get; set; }
        public FuelType Fuel { get; set; }
        public GasStation Station { get; set; }
        public string CustomStation { get; set; }
        public int Station_ID { get; set; }
        public double Volume { get; set; }
        public double PricePerOneLiter { get; set; }
        public bool FullTank { get; set; }
        public bool ForgotRecordPreviousGasStation { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
        public bool WrongMileage { get; set; }
    }
}