using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class RepairEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Mileage { get; set; }
        public string Repair { get; set; }
        public List<CarPart> Parts { get; set; }
        public string CarService { get; set; }
        public decimal RepairCost { get; set; }
        public string Comments { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public int VehicleId { get; set; }
    }
}