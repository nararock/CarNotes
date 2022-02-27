using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    /// <summary> Расходы </summary>
    public class Expense
    {
        /// <summary> Ид расхода </summary>
        [Key]
        public int Id { get; set; }
        public int TypeId { get; set; }
        /// <summary> Дата расхода </summary>
        public DateTime Date { get; set; }
        /// <summary> Сумма расхода </summary>
        public decimal Sum { get; set; }
        /// <summary> Описание расхода </summary>
        public string Description { get; set; }
        /// <summary> Комментарий </summary>
        public string Comment { get; set; }
        
        /// <summary> Тип расхода </summary>
        public virtual ExpenseType Type { get; set; }
    }
}