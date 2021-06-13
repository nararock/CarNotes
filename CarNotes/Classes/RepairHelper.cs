using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class RepairHelper
    {
        public void SaveToDataBase(RepairModel rm)
        {
            var database = new CnDbContext();
            var repairEvent = new RepairEvent();
            repairEvent.CarService = rm.CarService;
            repairEvent.Comments = rm.Comments;
            repairEvent.Date = rm.Date;
            repairEvent.Mileage = rm.Mileage;
            repairEvent.Repair = rm.Repair;
            repairEvent.RepairCost = rm.RepairCost;
            database.RepairEvents.Add(repairEvent);
            database.SaveChanges();
        }
    }
}