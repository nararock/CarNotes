using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ReleaseYear { get; set;}
        public string Body { get; set; }
        public string Color { get; set; }
        public virtual User User { get; set; }
    }
}