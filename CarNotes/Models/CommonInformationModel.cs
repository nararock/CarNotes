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
        public double RefuelCost { get; set; }
        public int RepairAmount { get; set; }
        public double RepairCost { get; set; }
        public int ExpenseAmount { get; set; }
        public double ExpenseCost { get; set; }
        public double AverageFuelPrice { get; set;}
    }
}