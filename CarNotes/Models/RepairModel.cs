using CarNotes.CnDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RepairModel
    {
        public DateTime Date { get; set; }
        public double Mileage { get; set; }
        public string Repair { get; set; }
        public List<CarPart> Parts { get; set; }
        public string CarService { get; set; }
        public decimal RepairCost { get; set; }
        public string Comments { get; set; }
    }
}