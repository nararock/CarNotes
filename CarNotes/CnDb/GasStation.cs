﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CarNotes.CnDb
{
    public class GasStation
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}