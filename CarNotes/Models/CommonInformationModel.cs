using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class CommonInformationModel
    {
        public CommonTimeOnSite TotalTime { get; set; }
        public int CommonMileage { get; set; }
        public int RefuelAmount { get; set; }
        public int RefuelCost { get; set; }
        public int RepairAmount { get; set; }
        public int RepairCost { get; set; }
        public int ExpenseAmount { get; set; }
        public int ExpenseCost { get; set; }
        public int AverageFuelPrice { get; set;}
    }
}