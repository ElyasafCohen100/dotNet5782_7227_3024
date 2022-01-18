using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        #region Get
        /// <summary>
        /// View list of droneToList.
        /// </summary>
        /// <returns> List of droneToList </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetAllDroneToList()
        {
            return droneToLists;
        }
     
        
        /// <summary>
        /// get the droneToList's list
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>the dronToList's list</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesToList(Predicate<DroneToList> predicate)
        {
            return droneToLists.FindAll(predicate);
        }


        /// <summary>
        /// get BL drone by drone Id.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <returns> BL drone object </returns>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if no drone with such id found </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDroneByIdBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            DroneToList drone = droneToLists.Find(x => x.Id == droneId);
            if (drone == null) throw new ObjectNotFoundException("drone");

            Drone blDrone = new();
            blDrone.Id = drone.Id;
            blDrone.MaxWeight = drone.MaxWeight;
            blDrone.Model = drone.Model;
            blDrone.DroneStatus = drone.DroneStatus;
            blDrone.BatteryStatus = drone.BatteryStatus;
            blDrone.CurrentLocation = drone.CurrentLocation;

            if (drone.DroneStatus == DroneStatuses.Shipment)
            {
                blDrone.ParcelInDelivery = AddParcelInDelivery(drone.DeliveryParcelId);
                DO.Parcel parcel = dalObject.GetParcelById(drone.DeliveryParcelId);
                if (parcel.PickedUp != null && parcel.Delivered == null)
                {
                    blDrone.ParcelInDelivery.ParcelStatus = true;
                }
            }
            return blDrone;
        }
        #endregion
      
        
        #region Add
        /// <summary>
        /// Check if the drone is already exist.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="ObjectAlreadyExistException"> Thrown if drone id is already exist </exception>
        static void IfExistDrone(int droneId)
        {
            foreach (var drone in dalObject.GetDroneList())
            {
                if (drone.Id == droneId) throw new ObjectAlreadyExistException("drone");
            }
        }


        /// <summary>
        /// Add new BL drone to the list using DAL.
        /// </summary>
        /// <param name="drone"> Drone object </param>
        /// <param name="baseStationId"> Station Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone details is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if no base-station found </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewDroneBL(Drone drone, int baseStationId)
        {

            if (drone.Id < 1000 || drone.Id >= 10000) throw new InvalidInputException("Id (Drone)");
            IfExistDrone(drone.Id);
            if (drone.Model == null) throw new InvalidInputException("Model");
            if ((int)drone.MaxWeight < 0 || (int)drone.MaxWeight > 2) throw new InvalidInputException("Max weight");
            if (baseStationId < 1000 || baseStationId >= 10000) throw new InvalidInputException("Id (base-station)");

            Random r = new();
            DO.Drone newDrone = new();

            newDrone.Id = drone.Id;
            newDrone.Model = drone.Model;
            newDrone.MaxWeight = (DO.WeightCategories)drone.MaxWeight;

            drone.DroneStatus = DroneStatuses.Maintenance;
            drone.BatteryStatus = r.Next(20, 41);

            try
            {
                DO.Station myStaion = dalObject.GetStationById(baseStationId);

                drone.CurrentLocation.Latitude = myStaion.Latitude;
                drone.CurrentLocation.Longitude = myStaion.Longitude;
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("base station");
            }
            try
            {
                dalObject.AddNewDrone(newDrone);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
            UpdateDroneToListsList(drone);
        }


        /// <summary>
        /// Set the detailes of the fild ParcelInDelivery of drone.
        /// </summary>
        /// <param name="parcelId"> the ID of parcel </param>
        /// <returns>ParcelInDelivery object </returns>
        /// <exception cref="InvalidInputException"> Thrown if parcel id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if parcel with such id not found </exception>
        internal ParcelInDelivery AddParcelInDelivery(int parcelId)
        {
            if (parcelId < 0) throw new InvalidInputException("Id");

            ParcelInDelivery parcelInDalivery = new();

            DO.Parcel parcel = new();
            try
            {
                parcel = dalObject.GetParcelById(parcelId);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }

            parcelInDalivery.Id = parcel.Id;
            parcelInDalivery.Weight = (WeightCategories)parcel.Weight;
            parcelInDalivery.Priority = (Priorities)parcel.Priority;

            DO.Customer sender = dalObject.GetCustomerById(parcel.SenderId);
            DO.Customer target = dalObject.GetCustomerById(parcel.TargetId);
            parcelInDalivery.DistanceDelivery = dalObject.Distance(sender.Latitude, target.Latitude, sender.Longitude, target.Longitude);

            parcelInDalivery.receiverCustomer.Id = target.Id;
            parcelInDalivery.receiverCustomer.Name = target.Name;
            parcelInDalivery.TargetLocation.Latitude = target.Latitude;
            parcelInDalivery.TargetLocation.Longitude = target.Longitude;

            parcelInDalivery.senderCustomer.Id = sender.Id;
            parcelInDalivery.senderCustomer.Name = sender.Name;
            parcelInDalivery.SourceLocation.Latitude = sender.Latitude;
            parcelInDalivery.SourceLocation.Longitude = sender.Longitude;

            return parcelInDalivery;
        }
        #endregion


        #region Update
        /// <summary>
        /// Update Id and the model of the BL drone by using DAL.
        /// </summary>
        /// <param name="droneId"> Drone Id</param>
        /// <param name="newModel"> New model of drone </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is not valid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneModelBL(int droneId, string newModel)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");
            if (newModel == string.Empty) throw new InvalidInputException("Model");

            try
            {
                dalObject.UpdateDroneModel(droneId, newModel);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new(e.Message);
            }
            catch (XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
            droneToLists.Find(x => x.Id == droneId).Model = newModel;
        }


        /// <summary>
        /// update the droneToList's list
        /// </summary>
        /// <param name="drone"></param>
        private void UpdateDroneToListsList(Drone drone)
        {
            DroneToList newDrone = new();
            newDrone.Id = drone.Id;
            newDrone.Model = drone.Model;
            newDrone.MaxWeight = drone.MaxWeight;
            newDrone.BatteryStatus = drone.BatteryStatus;
            newDrone.DroneStatus = drone.DroneStatus;
            newDrone.CurrentLocation = drone.CurrentLocation;
           
            droneToLists.Add(newDrone);
        }


        /// <summary>
        /// Update the status of BL drone to available and decrease the battery of drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if drone with such id not found </exception>
        /// <exception cref="OutOfBatteryException"> Thrown if there is not enough battery </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToChargingBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            var drone = droneToLists.Find(x => x.Id == droneId && x.DroneStatus == DroneStatuses.Available);
            if (drone == null) throw new ObjectNotFoundException("drone");


            double minBatteryForCharging = FindMinPowerSuplyForCharging(drone);

            if (drone.BatteryStatus < minBatteryForCharging)
            {
                throw new OutOfBatteryException(droneId.ToString());
            }
            else
            {
                //Update drone detailes.
                int nearestBaseStationId;
                Station myStation = new();

                drone.BatteryStatus -= minBatteryForCharging;

                nearestBaseStationId = FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
                myStation = GetStationByIdBL(nearestBaseStationId);

                drone.CurrentLocation.Latitude = myStation.Location.Latitude;
                drone.CurrentLocation.Longitude = myStation.Location.Longitude;
                drone.DroneStatus = DroneStatuses.Maintenance;
                //Update Station and DroneCharge detailes.
                try
                {
                    dalObject.UpdateDroneToCharging(drone.Id, myStation.Id);
                }
                catch (DO.ObjectNotFoundException e)
                {
                    throw new ObjectNotFoundException(e.Message);
                }
                catch (DO.ObjectIsNotActiveException e)
                {
                    throw new ObjectIsNotActiveException(e.Message);
                }
                catch (DO.XMLFileLoadCreateException e)
                {
                    throw new XMLFileLoadCreateException(e.Message);
                }
            }
        }


        /// <summary>
        /// Update the status of drone to available and increase the battery of drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <param name="chargeTime"> Charge time </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id or charging time is invalid </exception>
        /// <exception cref="NotValidRequestException"> Thrown if the drone is not in 'maintenance' status </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneFromChargingBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");
            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId && x.DroneStatus == DroneStatuses.Maintenance);
            if (droneToList == null) throw new NotValidRequestException("The drone has not in maintenance status");

            DroneCharge droneCharge = FindDroneChargeByDroneIdBL(droneId);
            if (droneCharge == null) throw new ObjectNotFoundException("droneCharge");
            double chargingTime = TimeIntervalInMinutes(droneCharge.ChargeTime, DateTime.Now);
            droneToList.DroneStatus = DroneStatuses.Available;
            droneToList.BatteryStatus += dalObject.ElectricityUseRequest()[4] * chargingTime;
            try
            {
                dalObject.UpdateDroneFromCharging(droneId);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
            catch (DO.XMLFileLoadCreateException e)
            {
                throw new XMLFileLoadCreateException(e.Message);
            }
        }
    
        
        private double TimeIntervalInMinutes(DateTime time1, DateTime time2)
        {
            TimeSpan interval = time2 - time1;
            double daysInMinutes = (interval.Days / 24) / 60;
            double hoursInMinutes = interval.Hours / 60;
            return daysInMinutes + hoursInMinutes + interval.Minutes;
        }


        /// <summary>
        /// Update the parcel status to picked up by the drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        /// <exception cref="NotValidRequestException"> Thrown if the drone has already picked up the parcel </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePickedUpParcelByDroneIdBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);
            if (droneToList == null) throw new ObjectNotFoundException("drone");
            DO.Parcel dalParcel = dalObject.GetParcelById(droneToList.DeliveryParcelId);

            if (dalParcel.PickedUp == null && droneToList.DroneStatus == DroneStatuses.Shipment)
            {
                droneToList.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(droneToList.Id, dalParcel.SenderId);

                Customer costomer = GetCustomerByIdBL(dalParcel.SenderId);
                droneToList.CurrentLocation = costomer.Location;


                dalObject.UpdatePickedUpParcelById(dalParcel.Id);
            }
            else
            {
                throw new NotValidRequestException("The drone not in shipment status or alredy picked up the parcel");
            }
        }


        /// <summary>
        /// Update the parcel status to delivered by the drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="NotValidRequestException"> Thrown if the drone has already delivered the parcel </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDeliveredParcelByDroneIdBL(int droneId)
        {
            Drone blDrone = GetDroneByIdBL(droneId);
            if (blDrone.ParcelInDelivery.Id == 0) throw new NotValidRequestException("No parcel was found in this drone");

            Parcel blParcel = GetParcelByIdBL(blDrone.ParcelInDelivery.Id);
            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);

            if ((blParcel.PickedUp != null) && (blParcel.Delivered == null))
            {
                Customer blCostomer = GetCustomerByIdBL(blDrone.ParcelInDelivery.receiverCustomer.Id);
                droneToList.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(blDrone.Id, blCostomer.Id);
                droneToList.CurrentLocation = blCostomer.Location;

                droneToList.DroneStatus = DroneStatuses.Available;
                droneToList.DeliveryParcelId = 0;

                int index = droneToLists.FindIndex(x => x.Id == droneId);
                droneToLists[index] = droneToList;

                dalObject.UpdateDeliveredParcelById(blParcel.Id);
            }
            else
            {
                throw new NotValidRequestException("Could not update parcel status");
            }
        }
        #endregion


        #region Delete
        /// <summary>
        /// delete the drone from the list
        /// </summary>
        /// <param name="droneId"></param>
        /// <exception cref="ObjectIsNotActiveException">throw if the object is not active</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            try
            {
                dalObject.DeleteDrone(droneId);
                DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);
                droneToLists.Remove(droneToList);
            }
            catch (DO.ObjectIsNotActiveException e)
            {
                throw new ObjectIsNotActiveException(e.Message);
            }
        }
        #endregion


        #region Simulator
        /// <summary>
        /// statrting the simulator
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="UpdateAction"></param>
        /// <param name="checkStopFunc"></param>
        public void StartSimulator(int droneId, Action UpdateAction, Func<bool> checkStopFunc)
        {
            new Simulator(this, droneId, UpdateAction, checkStopFunc);
        }
        #endregion
    }
}
