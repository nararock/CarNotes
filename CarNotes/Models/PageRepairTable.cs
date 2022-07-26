using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class PageRepairTable : PageContainer
    {
        public PageRepairTable(int count, int pageNumber, int pageSize) : base(count, pageNumber, pageSize) { }
        public List<RepairModel> PageList { get; set; }
    }
}