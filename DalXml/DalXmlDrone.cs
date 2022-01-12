using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
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
            try
            {


                List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);
                Drone drone = droneList.Find(x => x.Id == droneId);
                return drone.Id != droneId ? throw new ObjectNotFoundException("drone") : drone;

            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
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
            try
            {

                List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);
                DroneCharge droneCharge = droneChargeList.Find(x => x.DroneId == droneId);

                return droneCharge.DroneId != droneId ? throw new ObjectNotFoundException("droneCharge") : droneCharge;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDroneList()
        {
            try
            {
                List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);

                return from drone in droneList where drone.IsActive select drone;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }


        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);

            return droneList.FindAll(predicate).FindAll(x => x.IsActive);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);

            return droneChargeList.FindAll(predicate);
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
            try
            {
                drone.IsActive = true;
                List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);
                droneList.Add(drone);
                XMLTools.SaveListToXMLSerializer(droneList, dalDronePath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(int droneId, int stationId)
        {
            try
            {
                DroneCharge droneCharge = new();
                droneCharge.DroneId = droneId;
                droneCharge.StationId = stationId;
                droneCharge.ChargeTime = DateTime.Now;
                List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);
                droneChargeList.Add(droneCharge);
                XMLTools.SaveListToXMLSerializer(droneChargeList, dalDroneChargePath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        #endregion


        #region Update
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToCharging(int droneId, int stationId)
        {
            try
            {
                List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

                int index = stationList.FindIndex(x => x.Id == stationId);
                if (index == -1) throw new ObjectNotFoundException("station");

                Station station = stationList[index];

                if (station.ChargeSlots == 0) throw new ArgumentOutOfRangeException();
                if (!station.IsActive) throw new ObjectIsNotActiveException("station");

                station.ChargeSlots--;
                stationList[index] = station;

                List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);

                DroneCharge droneCharge = new DroneCharge();
                droneCharge.DroneId = droneId;
                droneCharge.ChargeTime = DateTime.Now;
                droneCharge.StationId = stationId;
                droneChargeList.Add(droneCharge);

                XMLTools.SaveListToXMLSerializer(droneChargeList, dalDroneChargePath);
                XMLTools.SaveListToXMLSerializer(stationList, dalStationPath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneFromCharging(int droneId)
        {
            try
            {
                List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

                lock (DalObj)
                {
                    DroneCharge droneCharge = GetDroneChargeByDroneId(droneId);

                    int index = stationList.FindIndex(x => x.Id == droneCharge.StationId);
                    if (index == -1) throw new ObjectNotFoundException("station");

                    Station station = stationList[index];
                    station.ChargeSlots++;
                    stationList[index] = station;

                    List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);

                    droneChargeList.Remove(droneCharge);

                    XMLTools.SaveListToXMLSerializer(droneChargeList, dalDroneChargePath);
                    XMLTools.SaveListToXMLSerializer(stationList, dalStationPath);
                }
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneModel(int droneId, string newModel)
        {
            try
            {
                List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);

                int index = droneList.FindIndex(x => x.Id == droneId);
                if (index == -1) throw new ObjectNotFoundException("drone");

                Drone drone = droneList[index];
                drone.Model = newModel;
                droneList[index] = drone;
                XMLTools.SaveListToXMLSerializer(droneList, dalDronePath);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        #endregion


        #region Delete
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
        public void DeleteDrone(int droneId)
        {
            try
            {
                List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);

                int index = droneList.FindIndex(x => x.Id == droneId);
                if (index == -1) throw new ObjectNotFoundException("drone");

                Drone drone = droneList[index];
                drone.IsActive = false;
                droneList[index] = drone;
                XMLTools.SaveListToXMLSerializer(droneList, dalDronePath);

                if (IsDroneChargeExist(droneId))
                {
                    DeleteDroneCharge(droneId);
                }
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int droneId)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);

            DroneCharge droneCharge = GetDroneChargeByDroneId(droneId);
            droneCharges.Remove(droneCharge);

            XMLTools.SaveListToXMLSerializer(droneCharges, dalDroneChargePath);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleseDroneCharges()
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);
            foreach (var droneCharge in droneChargeList)
            {
                UpdateDroneFromCharging(droneCharge.DroneId);
            }
            droneChargeList.Clear();
            XMLTools.SaveListToXMLSerializer(droneChargeList, dalDroneChargePath);
        }
        #endregion
    }
}
