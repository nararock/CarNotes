using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CarPart
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string CarManufacturer { get; set; }
        public string Article { get; set; }
        public double Price { get; set; }
        public int CarSubsystemId { get; set; }
        public CarSubsystem CarSubsystem { get; set; }
        [Column("RepairEvent_Id")]
        public int RepairEventId { get; set; }
    }
}