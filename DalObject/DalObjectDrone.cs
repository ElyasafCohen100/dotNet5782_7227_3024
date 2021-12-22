using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        #region FIND
        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Finds Drone by specific Id.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        /// <returns> Drone object </returns>
        /// <exception cref="ObjectNotFoundException">Throw if drone with such id has not found</exception>
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
        /// <exception cref="ObjectNotFoundException">Throw if drone-charge with such drone id has not found</exception>
        public DroneCharge FindDroneChargeByDroneId(int droneId)
        {
            DroneCharge droneCharge = DataSource.DroneCharges.Find(x => x.DroneId == droneId);
            return droneCharge.DroneId != droneId ? throw new ObjectNotFoundException(droneCharge.GetType().ToString()) : droneCharge;
        }

        #endregion

        #region SET

        //-------------------------- SETTERS ---------------------------//

        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        public void SetNewDrone(Drone drone)
        {
            DataSource.Drones.Add(drone);
        }
        public void AddDroneCharge(int droneId, int stationId)
        {
            DroneCharge droneCharge = new();
            droneCharge.DroneId = droneId;
            droneCharge.StationId = stationId;
            droneCharge.ChargeTime = DateTime.Now;
            DataSource.DroneCharges.Add(droneCharge);
        }
        #endregion

        #region UPDATE
        //----------------------- UPDATE FUNCTIONS ----------------------//

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        public void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");
            Parcel parcel = DataSource.Parcels[index];
            parcel.DroneId = droneId;
            parcel.Scheduled = DateTime.Now;
            DataSource.Parcels[index] = parcel;
        }

        /// <summary>
        /// Decrese the number of charge slots in the Base-Station,
        /// and the buttery to 100%.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param>
        /// <param name="stationId"> Id of Station </param>
        /// <exception cref="ArgumentOutOfRangeException">Throw if there no available charging-slots 
        ///                                                 left in the receiving base-station</exception>
        public void UpdateDroneToCharging(int droneId, int stationId)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == stationId);
            if (index == -1) throw new ObjectNotFoundException("station");

            Station station = DataSource.Stations[index];

            if (station.ChargeSlots == 0) throw new ArgumentOutOfRangeException();

            station.ChargeSlots--;
            DataSource.Stations[index] = station;

            DroneCharge droneCharge = new DroneCharge();
            droneCharge.DroneId = droneId;
            droneCharge.ChargeTime = DateTime.Now;
            droneCharge.StationId = stationId;
            DataSource.DroneCharges.Add(droneCharge);
        }

        /// <summary>
        /// Increse the number of charge slots in the Base-Station.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        public void UpdateDroneFromCharging(int droneId)
        {
            DroneCharge droneCharge = FindDroneChargeByDroneId(droneId);

            int index = DataSource.Stations.FindIndex(x => x.Id == droneCharge.StationId);
            if (index == -1) throw new ObjectNotFoundException("station");

            Station station = DataSource.Stations[index];
            station.ChargeSlots++;
            DataSource.Stations[index] = station;

            DataSource.DroneCharges.Remove(droneCharge);
        }

        public void UpdateDroneModel(int droneId, string newModel)
        {
            int index = DataSource.Drones.FindIndex(x => x.Id == droneId);
            if (index == -1) throw new ObjectNotFoundException("drone");

            Drone drone = DataSource.Drones[index];
            drone.Model = newModel;
            DataSource.Drones[index] = drone;
        }

        #endregion

        #region GET
        //--------------------------- GETTERS ---------------------------//

        /// <summary>
        /// Return list of Drones.
        /// </summary>
        /// <returns> List of Drones </returns>
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.Drones;
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate)
        {
            return DataSource.Drones.FindAll(predicate);
        }

        /// <summary>
        /// get the drone charge list by given staionID
        /// </summary>
        /// <param name="stationId"> ID of station </param>
        /// <returns> drone charge list  </returns>
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate)
        {
            return DataSource.DroneCharges.FindAll(predicate);
        }
        #endregion
    }
}