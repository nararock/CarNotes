using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class PageCommonTable: PageContainer
    {
        public PageCommonTable(int count, int pageNumber, int pageSize): base(count, pageNumber, pageSize) { }
        public List<CommonModel> PageList { get; set; }
    }
}