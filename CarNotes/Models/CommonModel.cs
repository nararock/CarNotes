using CarNotes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class CommonModel
    {
        public int Id { get; set; }
        public RecordType Record { get; set; }
        public string Date { get; set; }
        public double Mileage { get; set; }
        public double Cost { get; set; }
    }
}