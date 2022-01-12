using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {
        #region Get
        /// <summary>
        /// Finds Drone by specific Id.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        /// <returns> Drone object </returns>
        /// <exception cref="ObjectNotFoundException">Throw if drone with such id has not found</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDroneById(int droneId)
        {


            Drone drone = DataSource.Drones.Find(x => x.Id == droneId);
            return drone.Id != droneId ? throw new ObjectNotFoundException("drone") : drone;

        }


        /// <summary>
        /// Finds DroneCharge by specific Id.
        /// </summary>
        /// <param name="droneId">Id of Drone </param>
        /// <returns> DroneCharge object </returns>
        /// <exception cref="ObjectNotFoundException">Throw if drone-charge with such drone id has not found</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneChargeByDroneId(int droneId)
        {
            DroneCharge droneCharge = DataSource.DroneCharges.Find(x => x.DroneId == droneId);
            return droneCharge.DroneId != droneId ? throw new ObjectNotFoundException("droneCharge") : droneCharge;
        }


        /// <summary>
        /// Return list of Drones.
        /// </summary>
        /// <returns> List of Drones </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDroneList()
        {
            return from drone in DataSource.Drones where drone.IsActive select drone;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate)
        {
            return DataSource.Drones.FindAll(predicate).FindAll(x => x.IsActive);
        }


        /// <summary>
        /// get the drone charge list by given staionID
        /// </summary>
        /// <param name="stationId"> Id of station </param>
        /// <returns> drone charge list  </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate)
        {
            return DataSource.DroneCharges.FindAll(predicate);
        }
        #endregion


        #region Add
        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewDrone(Drone drone)
        {
            drone.IsActive = true;
            DataSource.Drones.Add(drone);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(int droneId, int stationId)
        {
            DroneCharge droneCharge = new();
            droneCharge.DroneId = droneId;
            droneCharge.StationId = stationId;
            droneCharge.ChargeTime = DateTime.Now;
            DataSource.DroneCharges.Add(droneCharge);
        }
        #endregion


        #region Update
        /// <summary>
        /// Decrese the number of charge slots in the Base-Station,
        /// and the buttery to 100%.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param>
        /// <param name="stationId"> Id of Station </param>
        /// <exception cref="ArgumentOutOfRangeException">Throw if there no available charging-slots 
        ///                                                 left in the receiving base-station</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneFromCharging(int droneId)
        {
            DroneCharge droneCharge = GetDroneChargeByDroneId(droneId);

            int index = DataSource.Stations.FindIndex(x => x.Id == droneCharge.StationId);
            if (index == -1) throw new ObjectNotFoundException("station");

            Station station = DataSource.Stations[index];
            station.ChargeSlots++;
            DataSource.Stations[index] = station;

            DataSource.DroneCharges.Remove(droneCharge);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneModel(int droneId, string newModel)
        {
            int index = DataSource.Drones.FindIndex(x => x.Id == droneId);
            if (index == -1) throw new ObjectNotFoundException("drone");

            Drone drone = DataSource.Drones[index];
            drone.Model = newModel;
            DataSource.Drones[index] = drone;
        }
        #endregion
       
        
        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            int index = DataSource.Drones.FindIndex(x => x.Id == droneId);
            if (index == -1) throw new ObjectNotFoundException("drone");
            Drone drone = DataSource.Drones[index];
            drone.IsActive = false;
            DataSource.Drones[index] = drone;
            if (IsDroneChargeExist(droneId))
                DeleteDroneCharge(droneId);
        }


        private bool IsDroneChargeExist(int droneId)
        {
            try
            {
                lock (DalObj)
                {
                    GetDroneChargeByDroneId(droneId);
                }
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
            return true;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int droneId)
        {
            DroneCharge droneCharge = GetDroneChargeByDroneId(droneId);
            DataSource.DroneCharges.Remove(droneCharge);
        }
        #endregion
    }
}