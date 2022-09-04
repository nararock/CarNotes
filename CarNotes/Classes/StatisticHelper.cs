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
        /// <summary>
        /// Метод для получения данных для построения круговой диаграммы с общими расходами.
        /// </summary>
        /// <param name="vehicleId"></param> Id автомобиля
        /// <returns></returns>
        public List<PieChartModel> GetDataForCommonStatistic(int vehicleId)
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

        public List<BarChartModel> GetDataForFuelFlowStatistic(int vehicleId, DateTime dateFrom, DateTime dateTo)
        {
            var db = new CnDbContext();
            var refuelCostFromDb = db.RefuelEvents.Where(e => e.VehicleId == vehicleId)
                .Select(e => new { e.Date, e.PricePerOneLiter, e.Volume })
                .Where(e=>e.Date >= dateFrom && e.Date <= dateTo)
                .ToList();
            var refuelCostDictionary = new Dictionary<DateTime, int>();
            foreach(var e in refuelCostFromDb)
            {
                var dateRefuel = new DateTime(e.Date.Year, e.Date.Month, e.Date.Day);
                if (!refuelCostDictionary.ContainsKey(dateRefuel))
                {
                    refuelCostDictionary.Add(dateRefuel, (int)(e.PricePerOneLiter * e.Volume));
                }
                else
                {
                    refuelCostDictionary[dateRefuel] = refuelCostDictionary[dateRefuel] + (int)(e.PricePerOneLiter * (e.Volume));
                }
            }
            var result = refuelCostDictionary
                .OrderBy(e=>e.Key)
                .Select(e=>new BarChartModel{ Date = e.Key.ToString("dd.MM.yyyy"), Cost = e.Value } )
                .ToList();
            return result;
        }
    }
}