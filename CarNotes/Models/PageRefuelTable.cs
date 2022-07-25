using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class PageRefuelTable : PageContainer
    {
        public PageRefuelTable(int count, int pageNumber, int pageSize) : base(count, pageNumber, pageSize) { }
        public List<RefuelModelOutput> PageList { get; set; }
    }
}