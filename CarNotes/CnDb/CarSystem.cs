using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CarSystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CarSubsystem> Subsystems { get; set; }
    }
}