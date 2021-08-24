using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CarNotes.Enums;
using Microsoft.AspNet.Identity;

namespace CarNotes.Classes
{
    public class RefuelHelper
    {
        public List<RefuelModelOutput> GetList(int vehicleId)
        {
            var db = new CnDbContext();
            var vehicle = db.Vehicles.Include(v => v.RefuelEvents.Select(r => r.Station))
                .FirstOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) return null;
            var list = vehicle.RefuelEvents.Select(x => new RefuelModelOutput {Id = x.ID, Date = x.Date.ToString("dd.MM.yyyy"), Mileage = x.Mileage, Fuel = x.Fuel.ToString(), Station = x.Station.Name,
                Volume = x.Volume, PricePerOneLiter = x.PricePerOneLiter, FullTank = x.FullTank,
                ForgotRecordPreviousGasStation = x.ForgotRecordPreviousGasStation }).ToList();
            return list;
        }
        public void SaveToDataBase(RefuelModel rm, int vehicleId)
        {
            var database = new CnDbContext();
            var refuelEvent = new RefuelEvent();
            refuelEvent.Date = DateTime.ParseExact(rm.Date, "dd.MM.yyyy", null);
            FuelType fuelanswer;
            Enum.TryParse(rm.Fuel, out fuelanswer);
            refuelEvent.Fuel = fuelanswer;
            refuelEvent.FullTank = rm.FullTank;
            refuelEvent.Mileage = rm.Mileage;
            refuelEvent.PricePerOneLiter = rm.PricePerOneLiter;
            refuelEvent.VehicleId = vehicleId;
            refuelEvent.Station_ID = rm.Station;
            refuelEvent.Volume = rm.Volume;
            refuelEvent.ForgotRecordPreviousGasStation = rm.ForgotRecordPreviousGasStation;
            database.RefuelEvents.Add(refuelEvent);
            database.SaveChanges();
        }

        public void Delete(int id, HttpContextBase hc)
        {
            var data = new CnDbContext();
            var refuel = data.RefuelEvents.Include(x => x.Station).FirstOrDefault(x => x.ID == id);
            if (refuel?.Vehicle?.UserId == hc.User.Identity.GetUserId())
            {
                data.GasStations.Remove(refuel.Station);
                data.RefuelEvents.Remove(refuel);
                data.SaveChanges();
            }
        }

        public RefuelModelOutput GetDataEdit(int id)
        {
            var db = new CnDbContext();
            var editRefuel = db.RefuelEvents.Include(x => x.Station).FirstOrDefault(x => x.ID == id);
            if (editRefuel == null)
            {
                return new RefuelModelOutput();
            }
            var editRefuelModel = new RefuelModelOutput();
            editRefuelModel.Date = editRefuel.Date.ToString("dd.MM.yyyy");
            editRefuelModel.Fuel = editRefuel.Fuel.ToString();
            editRefuelModel.FullTank = editRefuel.FullTank;
            editRefuelModel.Mileage = editRefuel.Mileage;
            editRefuelModel.PricePerOneLiter = editRefuel.PricePerOneLiter;
            editRefuelModel.Station = editRefuel.Station.Name;
            editRefuelModel.Volume = editRefuel.Volume;
            editRefuelModel.ForgotRecordPreviousGasStation = editRefuel.ForgotRecordPreviousGasStation;
            editRefuelModel.Id = editRefuel.ID;
            return editRefuelModel;
        }

        public void ChangeData(RefuelModel rm)
        {
            var data = new CnDbContext();
            var refuelEvent = data.RefuelEvents.Where(x => x.ID == rm.Id).Include(y=>y.Station).FirstOrDefault();
            if (refuelEvent != null)
            {
                refuelEvent.Date = DateTime.ParseExact(rm.Date, "dd.MM.yyyy", null);
                refuelEvent.ForgotRecordPreviousGasStation = rm.ForgotRecordPreviousGasStation;
                Enum.TryParse(rm.Fuel, out FuelType l);
                refuelEvent.Fuel = l;
                refuelEvent.FullTank = rm.FullTank;
                refuelEvent.ID = rm.Id;
                refuelEvent.Mileage = rm.Mileage;
                refuelEvent.PricePerOneLiter = rm.PricePerOneLiter;
                refuelEvent.Station_ID = rm.Station;
                refuelEvent.Volume = rm.Volume;
                data.SaveChanges();
            }
        }

        public List<GasStationModel> GetGasStationsList()
        {
            var db = new CnDbContext();
            var gasStation = db.GasStations.Select(x => new GasStationModel {Id = x.ID, Name = x.Name }).ToList();         
            return gasStation;
        }
    }
}