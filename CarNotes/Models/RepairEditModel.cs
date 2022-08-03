using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class RepairEditModel
    {
        public RepairEditModel()
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
        public double RepairCost { get; set; }
        public string Comments { get; set; }
        public bool WrongMileage { get; set; }
    }
}