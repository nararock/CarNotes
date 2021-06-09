using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarNotes.Classes
{
    public class RefuelHelper
    {
        public void SaveToDataBase(RefuelModel rm)
        {
            var database = new CnDbContext();
            var refuelEvent = new RefuelEvent();
            refuelEvent.Date = rm.Date;
            //refuelEvent.Fuel = rm.Fuel;
            refuelEvent.FullTank = rm.FullTank;
            refuelEvent.Mileage = rm.Mileage;
            refuelEvent.PricePerOneLiter = rm.PricePerOneLiter;
            var gs = database.GasStations.FirstOrDefault(x => x.Name == rm.Station);
            if (gs == null)
            {
                gs = new GasStation()
                {
                    Name = rm.Station
                };
                database.GasStations.Add(gs);
            }
            refuelEvent.Station = gs;
            refuelEvent.Volume = rm.Volume;
            refuelEvent.ForgotRecordPreviousGasStation = rm.ForgotRecordPreviousGasStation;
            database.RefuelEvents.Add(refuelEvent);
            database.SaveChanges();
        }
    }
}