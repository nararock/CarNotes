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

        public List<TypeExpenseModel> GetTypeExpenseList()
        {
            var db = new CnDbContext();
            var list = db.ExpenseTypes.Select(x=>new TypeExpenseModel { Id = x.Id, Name = x.Name}).ToList();
            return list;
        }
        public void ChangeData(ExpenseEditModel em, int vehicleId)
        {
            var db = new CnDbContext();
            var expenseEvent = db.Expenses
                .Include(z=>z.Type)
                .Where(x => x.Id == em.Id)
                .FirstOrDefault();
            if (expenseEvent == null)
            {
                expenseEvent = new Expense();
                db.Expenses.Add(expenseEvent);
                expenseEvent.VehicleId = vehicleId;
            }
            expenseEvent.Date = DateTime.ParseExact(em.Date, "dd.MM.yyyy", null);
            expenseEvent.TypeId = em.TypeId;
            if (decimal.TryParse(em.Sum, out var value))
            {
                expenseEvent.Sum = value;
            }
            expenseEvent.Sum = int.Parse(em.Sum);
            expenseEvent.Description = em.Description;
            expenseEvent.Comment = em.Comment;
            db.SaveChanges();
        }
    }
}