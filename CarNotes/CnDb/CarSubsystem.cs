using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CarSubsystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarsystemId { get; set; }
        public virtual CarSystem CarSystem { get; set; }
        public List<CarPart> CarParts { get; set; }
    }
}