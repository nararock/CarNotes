using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CarNotes.Enums;

namespace CarNotes.Classes
{
    public class RefuelHelper
    {
        public List<RefuelModel> GetList(int vehicleId)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Include(v => v.RefuelEvents.Select(r => r.Station))
                .FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.RefuelEvents.Select(x => new RefuelModel {Id = x.ID, Date = x.Date, Mileage = x.Mileage, Fuel = x.Fuel.ToString(), Station = x.Station.Name,
                Volume = x.Volume, PricePerOneLiter = x.PricePerOneLiter, FullTank = x.FullTank,
                ForgotRecordPreviousGasStation = x.ForgotRecordPreviousGasStation }).ToList();
            return list;
        }
        public void SaveToDataBase(RefuelModel rm, int vehicleId)
        {
            var database = new CnDbContext();
            var refuelEvent = new RefuelEvent();
            refuelEvent.Date = rm.Date;
            FuelType fuelanswer;
            Enum.TryParse(rm.Fuel, out fuelanswer);
            refuelEvent.Fuel = fuelanswer;
            refuelEvent.FullTank = rm.FullTank;
            refuelEvent.Mileage = rm.Mileage;
            refuelEvent.PricePerOneLiter = rm.PricePerOneLiter;
            refuelEvent.VehicleId = vehicleId;
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