using CarNotes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class CommonModel
    {
        public RecordType Record { get; set; }
        public DateTime Date { get; set; }
        public double Mileage { get; set; }
        public double Cost { get; set; }
    }
}