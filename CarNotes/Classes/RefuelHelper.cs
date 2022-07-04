using CarNotes.CnDb;
using CarNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CarNotes.Enums;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

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
            var list = vehicle.RefuelEvents.Select(x => new { Id = x.ID, Date = x.Date, Mileage = x.Mileage, Fuel = x.Fuel.ToString(),
                Station = (x.Station_ID == 1) ? x.CustomStation : x.Station.Name,
                Volume = x.Volume, PricePerOneLiter = x.PricePerOneLiter, Cost = Math.Round(x.Volume * x.PricePerOneLiter),  FullTank = x.FullTank,
                ForgotRecordPreviousGasStation = x.ForgotRecordPreviousGasStation, WrongMileage = x.WrongMileage }).OrderByDescending(x => x.Date).ThenByDescending(x => x.Mileage).ToList();
            var refuelModelOutput = new List<RefuelModelOutput>();
            refuelModelOutput.AddRange(list.Select(x => new RefuelModelOutput
            {
                Id = x.Id,
                Date = x.Date.ToString("dd.MM.yyyy"),
                Mileage = x.Mileage,
                Fuel = x.Fuel,
                Station = x.Station,
                Volume = x.Volume,
                PricePerOneLiter = x.PricePerOneLiter,
                Cost = x.Cost,
                FullTank = x.FullTank,
                ForgotRecordPreviousGasStation = x.ForgotRecordPreviousGasStation,
                WrongMileage = x.WrongMileage 
            }));
            return refuelModelOutput;
        }
        public void SaveToDataBase(RefuelModel rm, int vehicleId)
        {
            var database = new CnDbContext();
            var refuelEvent = new RefuelEvent();
            refuelEvent.Date = DateTime.ParseExact(rm.Date, "dd.MM.yyyy", null);
            refuelEvent.Fuel = rm.Fuel;
            refuelEvent.FullTank = rm.FullTank;
            refuelEvent.Mileage = double.Parse(rm.Mileage);
            if (new CommonHelper().CheckMileage(rm.Date, rm.Mileage, vehicleId))
            {
                refuelEvent.WrongMileage = false;
            } else { refuelEvent.WrongMileage = true; }
            refuelEvent.PricePerOneLiter = rm.PricePerOneLiter;
            refuelEvent.VehicleId = vehicleId;
            if (rm.Station == 1)
            {
                refuelEvent.CustomStation = rm.CustomStation;
            }
            else
            {
                var gasStation = database.GasStations.Where(x => x.ID == rm.Station).ToList();
                gasStation[0].Order++;                
            }
            refuelEvent.Station_ID = rm.Station;
            refuelEvent.Volume = double.Parse(rm.Volume.Replace('.', ','));
            refuelEvent.ForgotRecordPreviousGasStation = rm.ForgotRecordPreviousGasStation;
            database.RefuelEvents.Add(refuelEvent);
            database.SaveChanges();
        }

        public void Delete(int id, HttpContextBase hc)
        {
            var data = new CnDbContext();
            var refuel = data.RefuelEvents.FirstOrDefault(x => x.ID == id);
            if ((refuel?.Vehicle?.UserId) != hc.User.Identity.GetUserId())
            {
                return;
            }
            data.RefuelEvents.Remove(refuel);
            data.SaveChanges();
        }

        public RefuelModel GetDataEdit(int id)
        {
            var db = new CnDbContext();
            var editRefuel = db.RefuelEvents.Include(x => x.Station).FirstOrDefault(x => x.ID == id);
            if (editRefuel == null)
            {
                return new RefuelModel();
            }
            var editRefuelModel = new RefuelModel();
            editRefuelModel.Date = editRefuel.Date.ToString("dd.MM.yyyy");
            editRefuelModel.Fuel = editRefuel.Fuel;
            editRefuelModel.FullTank = editRefuel.FullTank;
            editRefuelModel.Mileage = editRefuel.Mileage.ToString();
            editRefuelModel.PricePerOneLiter = editRefuel.PricePerOneLiter;
            editRefuelModel.Station = editRefuel.Station.ID;
            if (editRefuel.CustomStation != null)
            {
                editRefuelModel.CustomStation = editRefuel.CustomStation;
            }
            editRefuelModel.Volume = editRefuel.Volume.ToString();
            editRefuelModel.ForgotRecordPreviousGasStation = editRefuel.ForgotRecordPreviousGasStation;
            editRefuelModel.Id = editRefuel.ID;
            return editRefuelModel;
        }

        public void ChangeData(RefuelModel rm, HttpContextBase hc)
        {
            var data = new CnDbContext();
            var refuelEvent = data.RefuelEvents.Where(x => x.ID == rm.Id).Include(y => y.Station).FirstOrDefault();
            if (refuelEvent == null)
            {
                return;
            }
            if ((refuelEvent?.Vehicle?.UserId) != hc.User.Identity.GetUserId())
            {
                return;
            }
            refuelEvent.Date = DateTime.ParseExact(rm.Date, "dd.MM.yyyy", null);
            refuelEvent.ForgotRecordPreviousGasStation = rm.ForgotRecordPreviousGasStation;
            refuelEvent.Fuel = rm.Fuel;
            refuelEvent.FullTank = rm.FullTank;
            refuelEvent.ID = rm.Id;
            refuelEvent.Mileage = double.Parse(rm.Mileage);
            if (new CommonHelper().CheckMileage(rm.Date, rm.Mileage, rm.Id))
            {
                refuelEvent.WrongMileage = false;
            }
            else { refuelEvent.WrongMileage = true; }
            refuelEvent.PricePerOneLiter = rm.PricePerOneLiter;
            refuelEvent.Station_ID = rm.Station;
            refuelEvent.Volume = double.Parse(rm.Volume.Replace('.', ','));
            data.SaveChanges();
        }

        public List<GasStationModel> GetGasStationsList()
        {
            var db = new CnDbContext();
            var gasStation = db.GasStations.Select(x => new GasStationModel {Id = x.ID, Name = x.Name, Order = x.Order }).ToList();
            gasStation.Sort();
            return gasStation;
        }

        public RefuelEventCreateModel GetDataForCreateEvent(int vehicleId)
        {
            var lastMileage = new CommonHelper().GetLastMileage(vehicleId);
            var lastFuel = GetLastFuelType(vehicleId);
            var createModel = new RefuelEventCreateModel();
            createModel.LastMileage = lastMileage;
            createModel.LastFuel = lastFuel;
            return createModel;
        }

        public FuelType GetLastFuelType(int vehicleId)
        {
            var db = new CnDbContext();
            db.Database.Log += s => System.Diagnostics.Debug.WriteLine(s);
            SqlParameter param = new SqlParameter("@Id", vehicleId);
            var lastFuel = db.Database.SqlQuery<FuelType>(@"Select top(1) Fuel as LastFuel from RefuelEvents where VehicleId = @Id order by Date desc, Mileage desc", param).ToList();
            var answer = lastFuel.Count == 0 ? (FuelType)1 : lastFuel[0];
            return answer;
        }
    }
}