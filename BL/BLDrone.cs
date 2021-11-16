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
        public void SetNewDroneBL(Drone drone, int baseStationID)
        {

            //TODO: change the function refer to the exercise requirments
            Random r = new();
            IDAL.DO.Drone newDrone = new();
            IDAL.IDal dalObject = new DalObject.DalObject();



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
    }
}
