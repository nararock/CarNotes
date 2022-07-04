using CarNotes.Enums;
using System;

namespace CarNotes.Models
{
    public class CommonModel
    {
        public int Id { get; set; }
        public RecordType Record { get; set; }
        public DateTime Date { get; set; }
        public double? Mileage { get; set; }
        public double Cost { get; set; }
        public bool WrongMileage { get; set; }
    }
}