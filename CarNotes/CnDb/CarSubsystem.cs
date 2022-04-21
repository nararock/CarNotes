using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarNotes.CnDb
{
    public class CarSubsystem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarsystemId { get; set; }
        public virtual CarSystem CarSystem { get; set; }
        public List<CarPart> CarParts { get; set; }
    }
}