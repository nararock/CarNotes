using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace CarNotes.Classes
{
    public class ExpenseHelper
    {
        public List<ExpenseModel> GetList(int vehicleId) 
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Include(v => v.Expenses.Select(r => r.Type)).FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.Expenses.Select(x => new 
            {
                Id = x.Id,
                Type = x.Type.Name,
                Date = x.Date,
                Mileage = x.Mileage,
                Sum = x.Sum,
                Description = x.Description,
                Comment = x.Comment,
                WrongMileage = x.WrongMileage
            }).OrderByDescending(x=>x.Date).ThenByDescending(x=>x.Mileage).ToList();
            var expenseModel = new List<ExpenseModel>();
            expenseModel.AddRange(list.Select(x => new ExpenseModel
            {
                Id = x.Id,
                Type = x.Type,
                Date = x.Date.ToString("dd.MM.yyyy"),
                Mileage = x.Mileage,
                Sum = x.Sum,
                Description = x.Description,
                Comment = x.Comment,
                WrongMileage = x.WrongMileage
            }));
            return expenseModel;
        }

        public List<TypeExpenseModel> GetTypeExpenseList()
        {
            var db = new CnDbContext();
            var list = db.ExpenseTypes.Select(x=>new TypeExpenseModel { Id = x.Id, Name = x.Name}).ToList();
            return list;
        }
        public ExpenseEditModel GetExpenseEditList(int id)
        {
            var db = new CnDbContext();
            var editExpense = db.Expenses
                .FirstOrDefault(x => x.Id == id);
            var expenseEditModel = new ExpenseEditModel();
            expenseEditModel.Id = editExpense.Id;
            expenseEditModel.Date = editExpense.Date.ToString("dd.MM.yyyy");
            expenseEditModel.Mileage = editExpense.Mileage;
            expenseEditModel.TypeId = editExpense.TypeId;
            expenseEditModel.Description = editExpense.Description;
            expenseEditModel.Sum = editExpense.Sum;
            expenseEditModel.Comment = editExpense.Comment;
            return expenseEditModel;
        }

        public void ChangeData(ExpenseEditModel em, int vehicleId, HttpContextBase hc)
        {
            var db = new CnDbContext();
            var checkUser = new CommonHelper().GetAccessToVehicle(hc.User.Identity.GetUserId(), vehicleId, db);
            if (!checkUser) { return; }
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
            expenseEvent.Mileage = em.Mileage;
            if ((em.Mileage != null && new CommonHelper().CheckMileage(em.Date, Convert.ToString(em.Mileage), vehicleId)) || em.Mileage == null)
            {
                expenseEvent.WrongMileage = false;
            }
            else { expenseEvent.WrongMileage = true; }
            expenseEvent.TypeId = em.TypeId;
            expenseEvent.Sum = em.Sum;
            expenseEvent.Sum = em.Sum;
            expenseEvent.Description = em.Description;
            expenseEvent.Comment = em.Comment;
            db.SaveChanges();
        }

        public void Delete(int id, HttpContextBase hc)
        {
            var db = new CnDbContext();
            var expense = db.Expenses.Include(z => z.Type).FirstOrDefault(x => x.Id == id);
            if (expense?.Vehicle?.UserId == hc.User.Identity.GetUserId())
            {
                db.Expenses.Remove(expense);
                db.SaveChanges();
            }
        }
    }
}