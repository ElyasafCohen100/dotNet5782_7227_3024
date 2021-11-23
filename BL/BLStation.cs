using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddNewStationBL(Station station)
        {
            IDAL.DO.Station dalStation = new();

            dalStation.Id = station.Id;
            dalStation.Name = station.Name;
            dalStation.Lattitude = station.Location.Lattitude;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.ChargeSlots = station.AvailableChargeSlots;

            dalObject.SetNewStation(dalStation);
        }

        public void UpdateBaseStationDetailes(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            IDAL.DO.Station dalBaseStation = dalObject.FindStationById(baseStationId);

            if (baseStationNewName != "")
            {
                dalBaseStation.Name = baseStationNewName;
            }

            if (baseStationChargeSlots != 0)
            {
                dalBaseStation.ChargeSlots = baseStationChargeSlots;
            }
        }

        public Station FindStationByIdBL(int stationId)
        {
            IDAL.DO.Station dalStation = dalObject.FindStationById(stationId);
            Station Station = new();
            int numOfUnAvailableChargeSlots = 0;

            Station.Id = dalStation.Id;
            Station.Name = dalStation.Name;
            Station.Location.Longitude = dalStation.Longitude;
            Station.Location.Longitude = dalStation.Lattitude;


            foreach (var droneCharge in dalObject.GetDroneChargeListByStationId(stationId))
            {
                DroneCharge DroneCharge = new();
                Drone Drone = new();

                DroneCharge.DroneId = droneCharge.DroneId;

                Drone = FindDroneByIdBL(DroneCharge.DroneId);
                DroneCharge.BatteryStatus = Drone.BatteryStatus;

                Station.DroneChargesList.Add(DroneCharge);

                ++numOfUnAvailableChargeSlots;
            }
            Station.AvailableChargeSlots = dalStation.ChargeSlots - numOfUnAvailableChargeSlots;

            return Station;
        }

        public IEnumerable<StationToList> ViewBaseStationsToList()
        {
            List<StationToList> stationToList = new();

            foreach (var baseStation in dalObject.GetBaseStationList())
            {
                StationToList station = new();

                station.Id = baseStation.Id;
                station.Name = baseStation.Name;

                station.NotAvailableChargeSlots = dalObject.GetDroneChargeListByStationId(baseStation.Id).Count();
                station.AvailableChargeSlots = baseStation.ChargeSlots - station.NotAvailableChargeSlots;

                stationToList.Add(station);
            }

            return stationToList;
        }

        public IEnumerable<StationToList> ViewStationsWithAvailableChargingSlotstBL()
        {
            List<StationToList> stationList = ViewBaseStationsToList().ToList();
            List<StationToList> stationWithAvailableChargingSlotstList = new();

            foreach (var baseStation in dalObject.GetStationsWithAvailableChargingSlots())
            {
                StationToList stationToList = stationList.Find(x => x.Id == baseStation.Id);

                if (stationToList != null)
                {
                    stationWithAvailableChargingSlotstList.Add(stationToList);
                }
            }

            return stationWithAvailableChargingSlotstList;
        }
    }
}
