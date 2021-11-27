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
        public Drone FindDroneById(int droneId)
        {
            Drone drone = DataSource.Drones.Find(x => x.Id == droneId);
            return drone.Id != droneId ? throw new ObjectNotFoundException(drone.GetType().ToString()) : drone;
        }

        /// <summary>
        /// Finds DroneCharge by specific Id.
        /// </summary>
        /// <param name="droneId">Id of Drone </param>
        /// <returns> DroneCharge object </returns>
        public DroneCharge FindDroneChargeByDroneId(int droneId)
        {
            DroneCharge droneCharge = DataSource.DroneCharges.Find(x => x.DroneId == droneId);
            return droneCharge.DroneId != droneId ? throw new ObjectNotFoundException(droneCharge.GetType().ToString()) : droneCharge;
        }


        //-------------------------- SETTERS ---------------------------//
        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        public void SetNewDrone(Drone drone)
        {
            DataSource.Drones.Add(drone);
        }


        //----------------------- UPDATE FUNCTIONS ----------------------//

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        public void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            try
            {
                Parcel myParcel = FindParcelById(parcelId);
                myParcel.DroneId = droneId;
            }
            catch(ObjectNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Decrese the number of charge slots in the Base-Station,
        /// and the buttery to 100%.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param>
        /// <param name="stationId"> Id of Station </param>
        public void UpdateDroneToCharging(int droneId, int stationId)
        {
            Station myStation = new();
            try
            {
                myStation = FindStationById(stationId);
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }

            if (myStation.ChargeSlots == 0) throw new ArgumentOutOfRangeException();
            myStation.ChargeSlots--;

            DroneCharge droneCharge = new DroneCharge();
            droneCharge.DroneId = droneId;
            droneCharge.StationId = stationId;
            DataSource.DroneCharges.Add(droneCharge);
        }

        /// <summary>
        /// Increse the number of charge slots in the Base-Station.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        public void UpdateDroneFromCharging(int droneId)
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
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.Drones;
        }

        /// <summary>
        /// get the drone charge list by given staionID
        /// </summary>
        /// <param name="stationId"> ID of station </param>
        /// <returns> drone charge list  </returns>
        public IEnumerable<DroneCharge> GetDroneChargeListByStationId(int stationId)
        {
            return DataSource.DroneCharges.FindAll(x => x.StationId == stationId);
        }
    }
}