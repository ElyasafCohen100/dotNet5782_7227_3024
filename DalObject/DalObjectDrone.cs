using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        #region Find

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

        #region Setters

        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        public void SetNewDrone(Drone drone)
        {
            drone.IsActive = true;
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

        public void DeleteDrone(int droneId)
        {
            int index = DataSource.Drones.FindIndex(x => x.Id == droneId);
            if (index == -1) throw new ObjectNotFoundException("drone");
            Drone drone = DataSource.Drones[index];
            drone.IsActive = false;
            DataSource.Drones[index] = drone;
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
            if (!station.IsActive) throw new ObjectIsNotActiveException("station");

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

        #region Getters

        /// <summary>
        /// Return list of Drones.
        /// </summary>
        /// <returns> List of Drones </returns>
        public IEnumerable<Drone> GetDroneList()
        {
            return from drone in DataSource.Drones where drone.IsActive select drone;
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate)
        {
            return DataSource.Drones.FindAll(predicate).FindAll(x => x.IsActive);
        }

        /// <summary>
        /// get the drone charge list by given staionID
        /// </summary>
        /// <param name="stationId"> Id of station </param>
        /// <returns> drone charge list  </returns>
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate)
        {
            return DataSource.DroneCharges.FindAll(predicate);
        }
        #endregion
    }
}