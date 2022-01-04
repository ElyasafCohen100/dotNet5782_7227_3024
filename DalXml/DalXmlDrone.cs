using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string dalDronePath = @"Drones.xml";
            XElement dalDronesRoot = XElement.Load(dalDronePath);

            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);
            Drone drone = droneList.Find(x => x.Id == droneId);

            dalDronesRoot.Save(dalDronePath);
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
            string dalDroneChargePath = @"DroneCharges.xml";
            XElement dalDroneChargesRoot = XElement.Load(dalDroneChargePath);

            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);
            DroneCharge droneCharge = droneChargeList.Find(x => x.DroneId == droneId);

            dalDroneChargesRoot.Save(dalDroneChargePath);
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
            string dalDronePath = @"Drones.xml";
            XElement dalDronesRoot = XElement.Load(dalDronePath);

            drone.IsActive = true;
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);
            droneList.Add(drone);
            XMLTools.SaveListToXMLSerializer(droneList, dalDronePath);
        }
        #endregion

        #region ADD
        public void AddDroneCharge(int droneId, int stationId)
        {
            string dalDroneChargePath = @"DroneCharges.xml";
            XElement dalDroneChargesRoot = XElement.Load(dalDroneChargePath);

            DroneCharge droneCharge = new();
            droneCharge.DroneId = droneId;
            droneCharge.StationId = stationId;
            droneCharge.ChargeTime = DateTime.Now;
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dalDroneChargePath);
            droneChargeList.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer(droneChargeList, dalDroneChargePath);
        }
        #endregion

        #region Delete
        public void DeleteDrone(int droneId)
        {
            string dalDronePath = @"Drones.xml";
            XElement dalDronesRoot = XElement.Load(dalDronePath);
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dalDronePath);

            int index = droneList.FindIndex(x => x.Id == droneId);
            if (index == -1) throw new ObjectNotFoundException("drone");
            Drone drone = droneList[index];
            drone.IsActive = false;
            droneList[index] = drone;
            XMLTools.SaveListToXMLSerializer(droneList, dalDronePath);
        }
        #endregion 
    }
}
