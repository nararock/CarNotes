using CarNotes.Classes;
using CarNotes.Enums;
using System;

namespace CarNotes.Models
{
    public class CommonModel
    {
        public int Id { get; set; }
        public RecordType Record { get; set; }
        public DateTime Date { get; set; }
        public double? Mileage { get; set; }
        public double Cost { get; set; }
        public bool WrongMileage { get; set; }
        
        /// <summary>
        /// получение названия типа события для общей таблицы 
        /// </summary>
        /// <returns></returns>
        public string getRecordName()
        {
            if (Record == RecordType.Refuel) return "Заправка";
            else if (Record == RecordType.Repair) return "Ремонт";
            else if (Record == RecordType.Expense) return "Расход";
            else return "";
        }

        public string getColor()
        {
            if (Record == RecordType.Refuel) return new LinkMenuHelper().getColor("Заправки");
            else if (Record == RecordType.Repair) return new LinkMenuHelper().getColor("Ремонты");
            else if (Record == RecordType.Expense) return new LinkMenuHelper().getColor("Расходы");
            return "white";
        }
    }
}