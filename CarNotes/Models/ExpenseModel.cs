using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class ExpenseModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Mileage { get; set; }
        public string Sum { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
    }
}