using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
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
            try
            {
                XElement dalDronesRoot = XElement.Load(dalDronePath);

                List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);
                Drone drone = droneList.Find(x => x.Id == droneId);

                dalDronesRoot.Save(dalDronePath);
                return drone.Id != droneId ? throw new ObjectNotFoundException(drone.GetType().ToString()) : drone;
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
        public DroneCharge FindDroneChargeByDroneId(int droneId)
        {
            try
            {
                XElement dalDroneChargesRoot = XElement.Load(dalDroneChargePath);

                List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);
                DroneCharge droneCharge = droneChargeList.Find(x => x.DroneId == droneId);

                dalDroneChargesRoot.Save(dalDroneChargePath);
                return droneCharge.DroneId != droneId ? throw new ObjectNotFoundException(droneCharge.GetType().ToString()) : droneCharge;
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        #endregion

        #region Setters

        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        public void SetNewDrone(Drone drone)
        {
            try
            {
                XElement dalDronesRoot = XElement.Load(dalDronePath);

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
        public void AddDroneCharge(int droneId, int stationId)
        {
            try
            {
                XElement dalDroneChargesRoot = XElement.Load(dalDroneChargePath);

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

        #region Delete
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
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
        #endregion

        #region Update
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

                string dalDroneChargePath = @"DroneCharges.xml";
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

        public void UpdateDroneFromCharging(int droneId)
        {
            try
            {
                List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

                DroneCharge droneCharge = FindDroneChargeByDroneId(droneId);

                int index = stationList.FindIndex(x => x.Id == droneCharge.StationId);
                if (index == -1) throw new ObjectNotFoundException("station");

                Station station = stationList[index];
                station.ChargeSlots++;
                stationList[index] = station;

                string dalDroneChargePath = @"DroneCharges.xml";
                List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);

                droneChargeList.Remove(droneCharge);

                XMLTools.SaveListToXMLSerializer(droneChargeList, dalDroneChargePath);
                XMLTools.SaveListToXMLSerializer(stationList, dalStationPath);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }

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

        #region Getters
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

        public IEnumerable<DroneCharge> GetDroneChargeList(Predicate<DroneCharge> predicate)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);

            return droneChargeList.FindAll(predicate);
        }
        #endregion
    }
}
