using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class StatisticHelper
    {
        public List<PieChartModel> GetDataForPieAllExpense(int vehicleId)
        {
            var db = new CnDbContext();
            var result = new List<PieChartModel>();
            var costRefuel = db.RefuelEvents.Where(e => e.VehicleId == vehicleId)
                .Select(e => new {e.Volume, e.PricePerOneLiter})
                .ToList()
                .Sum(e=>e.Volume * e.PricePerOneLiter);
            if (costRefuel != 0)
            {
                var refuel = new PieChartModel();
                refuel.Cost = (int)costRefuel;
                refuel.Color = "#a333c8";
                refuel.Name = "Заправки";
                result.Add(refuel);
            }
            var costRepair = db.RepairEvents.Where(e => e.VehicleId == vehicleId)
                .Select(e => new {e.RepairCost, parts = e.Parts.Select(p=>p.Price)})
                .ToList().Sum(e=>(double)e.RepairCost + e.parts.Sum(p=>p));
            if (costRepair != 0)
            {
                var refuel = new PieChartModel();
                refuel.Cost = (int)costRepair;
                refuel.Color = "#e03997";
                refuel.Name = "Ремонты";
                result.Add(refuel);
            }
            var costExpense = db.Expenses.Where(e => e.VehicleId == vehicleId).Select(c=>c.Sum).ToList().Sum();
            if (costExpense != 0)
            {
                var refuel = new PieChartModel();
                refuel.Cost = (int)costExpense;
                refuel.Color = "#6185d0";
                refuel.Name = "Расходы";
                result.Add(refuel);
            }
            return result;
        }
    }
}