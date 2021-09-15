using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class CarSubsystemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarSubsystemId { get; set; }
    }
}