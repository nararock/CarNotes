using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RepairModel
    {
        public RepairModel()
        {
            Parts = new List<CarPartModel>();
            Date = DateTime.Now.ToString("dd.MM.yyyy");
        }
        public int Id { get; set; }
        public string Date { get; set; }
        public double Mileage { get; set; }
        public string Repair { get; set; }
        public List<CarPartModel> Parts { get; set; }
        public string CarService { get; set; }
        public decimal RepairCost { get; set; }
        public string Comments { get; set; }
    }
}