using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class ExpenseEditModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public DateTime Date { get; set; }
        public double? Mileage { get; set; }
        public decimal Sum { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
    }
}