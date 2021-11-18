using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        public void AddNewDroneBL(Drone drone, int baseStationID)
        {
            Random r = new();
            IDAL.DO.Drone newDrone = new();

            newDrone.Id = drone.Id;
            newDrone.Model = drone.Model;
            newDrone.MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight;

            drone.DroneStatus = DroneStatuses.Maintenance;
            drone.BatteryStatus = r.Next(20, 41);

            IDAL.DO.Station myStaion = dalObject.FindStationById(baseStationID);
            drone.CurrentLocation.Lattitude = myStaion.Lattitude;
            drone.CurrentLocation.Longitude = myStaion.Longitude;

            dalObject.SetNewDrone(newDrone);
        }

        public void UpdateDroneModelBL(int droneId, string newModel)
        {
           IDAL.DO.Drone drone = new();

           drone =  dalObject.FindDroneById(droneId);
           drone.Model = newModel;
        }
    }
}
