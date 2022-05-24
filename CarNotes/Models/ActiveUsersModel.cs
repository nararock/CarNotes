using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class ActiveUsersModel
    {
        public string Name { get; set; }
        public int VehicleId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Events { get; set; }
        public int refuelEvents { get; set; }
        public int repairEvents { get; set; }
    }
}