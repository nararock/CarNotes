using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Models
{
    public class GasStationModel : IComparable<GasStationModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public int CompareTo(GasStationModel gsm)
        {
            if (gsm is null) throw new ArgumentException("Некорректное значение параметра");
            if (this.Id == 1 || gsm.Id == 1) return this.Order - gsm.Order;
            int resultCompare = -1 * this.Order.CompareTo(gsm.Order);
            if (resultCompare == 0) return this.Name.CompareTo(gsm.Name);
            else return resultCompare; 
        }
    }
}