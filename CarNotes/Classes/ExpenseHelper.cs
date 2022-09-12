using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace CarNotes.Classes
{
    public class ExpenseHelper
    {
        public List<ExpenseModel> GetList(int vehicleId, int pageNumder, int pageSize)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Find(vehicleId);
            if (vehicle == null) return null;
            SqlParameter paramId = new SqlParameter("@Id", vehicleId);
            SqlParameter paramAmountOffset = new SqlParameter("@amountOffset", (pageNumder - 1) * pageSize);
            SqlParameter paramAmountGet = new SqlParameter("@amountGet", pageSize);
            var expenseModel = db.Database.SqlQuery<ExpenseModel>(@"select top(@amountGet) * from(select Id, Name as Type, Date, Sum, ISNULL(Mileage, 0) as Mileage, Description, Comment, WrongMileage from Expenses as re
                                                                    left outer join (select Id as Idtype, Name from ExpenseTypes) as et on re.TypeId = et.Idtype
                                                                    Where VehicleId = @Id
                                                                    order by Date desc, Mileage desc
                                                                    offset @amountOffset rows) as data", paramId, paramAmountOffset, paramAmountGet).ToList();
            return expenseModel;
        }


        public List<TypeExpenseModel> GetTypeExpenseList()
        {
            var db = new CnDbContext();
            var list = db.ExpenseTypes.Select(x=>new TypeExpenseModel { Id = x.Id, Name = x.Name, Order = x.Order}).OrderBy(x=>x.Order).ThenBy(x=>x.Id).ToList();
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
            expenseEvent.Date = DateTime.Parse(em.Date);
            expenseEvent.Mileage = em.Mileage;
            if ((em.Mileage != null && new CommonHelper().CheckMileage(DateTime.Parse(em.Date), Convert.ToString(em.Mileage), vehicleId)) || em.Mileage == null)
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