using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    /// <summary> Тип расхода </summary>
    public class ExpenseType
    {
        /// <summary> Ид типа </summary>
        [Key]
        public int Id { get; set; }

        /// <summary> Название типа расхода </summary>
        public string Name { get; set; }
    }
}