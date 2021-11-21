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
            IDAL.DO.Station newStation = new();

            newStation.Id = station.Id;
            newStation.Name = station.Name;
            newStation.Lattitude = station.Location.Lattitude;
            newStation.Longitude = station.Location.Longitude;
            newStation.ChargeSlots = station.AvailableChargeSlots;

            dalObject.SetNewStation(newStation);
        }

        public void UpdateBaseStationDetailes(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            IDAL.DO.Station baseStation = dalObject.FindStationById(baseStationId);

            if (baseStationNewName != "")
            {
                baseStation.Name = baseStationNewName;
            }

            if (baseStationChargeSlots != 0)
            {
                baseStation.ChargeSlots = baseStationChargeSlots;
            }
        }

        public Station FindStationByIdBL(int stationId)
        {
            IDAL.DO.Station dalStation = dalObject.FindStationById(stationId);
            Station myStation = new();
            int numOfUnAvailableChargeSlots = 0;

            myStation.Id = dalStation.Id;
            myStation.Name = dalStation.Name;
            myStation.Location.Longitude = dalStation.Longitude;
            myStation.Location.Longitude = dalStation.Lattitude;


            foreach (var droneCharge in dalObject.GetDroneChargeListByStationId(stationId))
            {
                DroneCharge myDroneCharge = new();
                Drone myDrone = new();

                myDroneCharge.DroneId = droneCharge.DroneId;

                myDrone = FindDroneByIdBL(myDroneCharge.DroneId);
                myDroneCharge.BatteryStatus = myDrone.BatteryStatus;

                myStation.DroneChargesList.Add(myDroneCharge);

                ++numOfUnAvailableChargeSlots;
            }
            myStation.AvailableChargeSlots = dalStation.ChargeSlots - numOfUnAvailableChargeSlots;

            return myStation;
        }
    }
}
