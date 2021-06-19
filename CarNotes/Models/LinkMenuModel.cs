using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class LinkMenuModel
    {
        public string NameLink { get; set; }
        public string MethodLink { get; set; }
        public string Color { get; set; }
        public List<SubMenuModel> Buttons { get; set; }
    }
}