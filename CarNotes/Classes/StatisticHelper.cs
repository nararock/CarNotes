using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
        /// <summary>
        /// получение общей информации о пользователе для вывода в таблицу 
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public CommonInformationModel GetDataForCommonInformation(int vehicleId)
        {
            var database = new CnDbContext();
            var commonInformation = new CommonInformationModel();
            var commonMileageList = database.RefuelEvents.Where(e => e.VehicleId == vehicleId).Select(e => e.Mileage)
                .Union(database.RepairEvents.Where(e => e.VehicleId == vehicleId).Select(e => e.Mileage))
                .Union(database.Expenses.Where(e => e.Mileage != null && e.VehicleId == vehicleId).Select(e => (double)e.Mileage))
                .GroupBy(e => true, (e, records) => new
                {
                    maximum = records.Max(),
                    minimum = records.Min()
                })
                .ToList();
            commonInformation.CommonMileage = (int)(commonMileageList[0].maximum - commonMileageList[0].minimum);
            var oldestDate = database.RefuelEvents.Where(e => e.VehicleId == vehicleId).Select(e => e.Date)
                .Union(database.RepairEvents.Where(e => e.VehicleId == vehicleId).Select(e => e.Date))
                .Union(database.Expenses.Where(e => e.VehicleId == vehicleId).Select(e => e.Date))
                .Min(e => e);
            commonInformation.TotalTime = GetTimeSpentOnSite(oldestDate);
            commonInformation.RefuelAmount = database.RefuelEvents.Where(e => e.VehicleId == vehicleId).Count();
            commonInformation.RepairAmount = database.RepairEvents.Where(e => e.VehicleId == vehicleId).Count();
            commonInformation.ExpenseAmount = database.Expenses.Where(e => e.VehicleId == vehicleId).Count();
            commonInformation.RefuelCost = (int)database.RefuelEvents
                .Where(e => e.VehicleId == vehicleId)
                .Select(e => e.Volume * e.PricePerOneLiter)
                .Sum(e => e);
            commonInformation.RepairCost = (int)database.RepairEvents
                .Include(x => x.Parts)
                .Where(e => e.VehicleId == vehicleId)
                .Select(e => (double)e.RepairCost + e.Parts.Sum(p => p.Price))
                .Sum();
            commonInformation.ExpenseCost = (int)database.Expenses
                .Where(e => e.VehicleId == vehicleId)
                .Select(e => (double)e.Sum)
                .Sum(e => e);
            commonInformation.AverageFuelPrice = (int)database.RefuelEvents.Where(e=>e.VehicleId == vehicleId)
                .Average(e=>e.PricePerOneLiter);            
            return commonInformation;
        }
        /// <summary>
        /// вычисление количества лет, месяцев и дней на сайте относительно текущего момента
        /// </summary>
        /// <param name="timeStart"></param>дата записи первого события на сайте
        public CommonTimeOnSite GetTimeSpentOnSite(DateTime timeStart)
        { 
            var timeSpent = new CommonTimeOnSite();
            timeSpent.Year = DateTime.Now.Year - timeStart.Year;
            timeSpent.Month = DateTime.Now.Month - timeStart.Month;
            timeSpent.Day = DateTime.Now.Day - timeStart.Day;
            timeSpent.formYear = GetWordForm(timeSpent.Year, "year");
            timeSpent.formMonth = GetWordForm(timeSpent.Month, "month");
            timeSpent.formDay = GetWordForm(timeSpent.Day, "day");
            return timeSpent;
        }
        /// <summary>
        /// получение правильной формы слова для обозначения временного участка (год, месяц, день) в зависимости от численного значения этого участка
        /// </summary>
        /// <param name="amount"></param> количество временных участков (год, месяц, день)
        /// <param name="timeForm"></param> название передаваемого временного участка (year, month, day)
        /// <returns></returns>
        public string GetWordForm(int amount, string timeForm)
        {
            if ((11 <= amount % 100 && amount % 100 <= 14) || amount % 10 >= 5)
            {
                switch (timeForm)
                {
                    case "year": return "лет";
                    case "month": return "месяцев";
                    case "day": return "дней";
                }
            }
            else if (amount % 10 == 1)
            {
                switch (timeForm)
                {
                    case "year": return "год";
                    case "month": return "месяц";
                    case "day": return "день";
                }
            }
            else if (2 <= amount % 10 && amount % 10 <= 4)
            {
                switch (timeForm)
                {
                    case "year": return "годa";
                    case "month": return "месяца";
                    case "day": return "дня";
                }
            }
            return "";
        }
    }
}