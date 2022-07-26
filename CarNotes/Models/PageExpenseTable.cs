using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class PageExpenseTable: PageContainer
    {
        public PageExpenseTable(int count, int pageNumber, int pageSize) : base(count, pageNumber, pageSize) { }
        public List<ExpenseModel> PageList { get; set; }
    }
}