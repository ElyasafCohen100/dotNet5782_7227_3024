using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Finds Drone by specific Id.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        /// <returns> Drone object </returns>
        public static Drone FindDroneById(int droneId)
        {
            foreach (var myDrone in DataSource.Drones)
            {
                if (myDrone.Id == droneId)
                    return myDrone;
            }

            return new Drone();
        }

        /// <summary>
        /// Finds DroneCharge by specific Id.
        /// </summary>
        /// <param name="droneId">Id of Drone </param>
        /// <returns> DroneCharge object </returns>
        public static DroneCharge FindDroneChargeByDroneId(int droneId)
        {
            foreach (var droneCharge in DataSource.DroneCharges)
            {
                if (droneCharge.DroneId == droneId)
                    return droneCharge;
            }

            return new DroneCharge();
        }


        //-------------------------- SETTERS ---------------------------//

        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        public static void SetNewDrone(Drone drone)
        {
            DataSource.Drones.Add(drone);
        }


        //----------------------- UPDATE FUNCTIONS ----------------------//

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        public static void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            Parcel myParcel = FindParcelById(parcelId);
            myParcel.DroneId = droneId;
        }

        /// <summary>
        /// decrese the number of charge slots in the Base-Station.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param>
        /// <param name="stationId"> Id of Station </param>
        public static void UpdateDroneToCharging(int droneId, int stationId)
        {
            Station myStation = FindStationById(stationId);
            myStation.ChargeSlots--;

            DroneCharge droneCharge = new DroneCharge();
            droneCharge.DroneId = droneId;
            droneCharge.StationId = stationId;
            DataSource.DroneCharges.Add(droneCharge);
        }

        /// <summary>,
        /// increse the number of charge slots in the Base-Station.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        public static void UpdateDroneFromCharging(int droneId)
        {
            DroneCharge myDroneCharge = FindDroneChargeByDroneId(droneId);

            Station myStation = FindStationById(myDroneCharge.StationId);
            myStation.ChargeSlots++;

            DataSource.DroneCharges.Remove(myDroneCharge);
        }

        //--------------------------- GETTERS ---------------------------//

        /// <summary>
        /// Return list of Drones.
        /// </summary>
        /// <returns> List of Drones </returns>
        public static IEnumerable<Drone> GetDroneList()
        {
            return DataSource.Drones;
        }
    }
}
