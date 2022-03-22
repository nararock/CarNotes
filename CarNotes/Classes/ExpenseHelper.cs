using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CarNotes.Classes
{
    public class ExpenseHelper
    {
        public List<ExpenseModel> GetList(int vehicleId) 
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Include(v => v.Expenses.Select(r => r.Type)).FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.Expenses.Select(x => new ExpenseModel
            {
                Id = x.Id,
                Type = x.Type.Name,
                Date = x.Date.ToString("dd.MM.yyyy"),
                Sum = x.Sum.ToString("#"),
                Description = x.Description,
                Comment = x.Comment
            }).ToList();
            return list;
        }
    }
}