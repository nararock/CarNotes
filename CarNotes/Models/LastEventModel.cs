using CarNotes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class LastEventModel
    {
        public int Id { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public RecordType Record { get; set; }
        public DateTime Date { get; set; }
        public int Cost { get; set; }
    }
}