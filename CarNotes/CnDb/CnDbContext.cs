using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CnDbContext: DbContext
    {
        public CnDbContext() : base("name=CnDb") { }
    }
}