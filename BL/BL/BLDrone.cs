using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
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
            DO.Station myStaion;


            try
            {
                myStaion = dalObject.GetStationById(baseStationId);

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
            lock (dalObject)
            {
                dalObject.AddDroneCharge(drone.Id, myStaion.Id);

                UpdateDroneToListsList(drone);
            }
        }

        /// <summary>
        /// Set the detailes of the fild ParcelInDelivery of drone.
        /// </summary>
        /// <param name="parcelId"> the ID of parcel </param>
        /// <returns>ParcelInDelivery object </returns>
        /// <exception cref="InvalidInputException"> Thrown if parcel id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if parcel with such id not found </exception>
        internal ParcelInDelivery SetParcelInDelivery(int parcelId)
        {
            if (parcelId < 0) throw new InvalidInputException("Id");

            ParcelInDelivery parcelInDalivery = new();

            DO.Parcel parcel = new();
            try
            {
                parcel = dalObject.GetParcelById(parcelId);
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("parcel");
            }

            parcelInDalivery.Id = parcel.Id;
            parcelInDalivery.Weight = (WeightCategories)parcel.Weight;
            parcelInDalivery.Priority = (Priorities)parcel.Priority;

            DO.Customer sender = dalObject.GetCustomerById(parcel.SenderId);
            DO.Customer target = dalObject.GetCustomerById(parcel.TargetId);
            lock (dalObject)
            {
                parcelInDalivery.DistanceDelivery = dalObject.Distance(sender.Latitude, target.Latitude, sender.Longitude, target.Longitude);
            }

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

        public void ReleseDroneCharges()
        {
            lock (dalObject)
            {
                dalObject.ReleseDroneCharges();
            }
        }
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

        #endregion


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            lock (dalObject)
            {
                try
                {
                    DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);
                    if (droneToList.DroneStatus == DroneStatuses.Shipment) throw new InvalidOperationException();
                    dalObject.DeleteDrone(droneId);
                    droneToLists.Remove(droneToList);
                }
                catch (DO.ObjectIsNotActiveException e)
                {
                    throw new ObjectIsNotActiveException(e.Message);
                }
            }
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

                lock (dalObject)
                {
                    dalObject.UpdateDroneModel(droneId, newModel);
                }
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
                    lock (dalObject)
                    {
                        dalObject.UpdateDroneToCharging(drone.Id, myStation.Id);
                    }
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
            droneToList.DroneStatus = DroneStatuses.Available;
            droneToList.BatteryStatus = BatteryCalac(droneToList, droneCharge);

            try
            {
                lock (dalObject)
                {
                    dalObject.UpdateDroneFromCharging(droneId);
                }
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

        private double TimeIntervalInSeconds(DateTime time1, DateTime time2)
        {
            TimeSpan interval = time2 - time1;
            double daysInMinutes = (double)interval.Days * 24 * 60 * 60;
            double hoursInSeconds = (double)interval.Hours * 60 * 60;
            double seconds = interval.Seconds;
            return daysInMinutes + hoursInSeconds + interval.Minutes * 60 + seconds;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double BatteryCalac(DroneToList droneToList, DroneCharge droneCharge)
        {
            double chargingTime = TimeIntervalInSeconds(droneCharge.ChargeTime, DateTime.Now);
            double battery = droneToList.BatteryStatus + dalObject.ElectricityUseRequest()[4] * chargingTime;

            if (battery > 100)
                battery = 100;

            return battery;
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

            try
            {
                DO.Parcel dalParcel = dalObject.GetParcelById(droneToList.DeliveryParcelId);
                if (dalParcel.PickedUp == null && droneToList.DroneStatus == DroneStatuses.Shipment)
                {
                    droneToList.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(droneToList.Id, dalParcel.SenderId);
                    if (droneToList.BatteryStatus < 0)
                        droneToList.BatteryStatus = 0;

                    Customer costomer = GetCustomerByIdBL(dalParcel.SenderId);
                    droneToList.CurrentLocation = costomer.Location;

                    lock (dalObject)
                    {
                        dalObject.UpdatePickedUpParcelById(dalParcel.Id);
                    }
                }
                else
                {
                    throw new NotValidRequestException("The drone not in shipment status or alredy picked up the parcel");
                }
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("parcel");
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
                if (droneToList.BatteryStatus < 0)
                    droneToList.BatteryStatus = 0;
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


        #region Getters

        /// <summary>
        /// Find BL drone by drone Id.
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

            if (!dalObject.GetDroneById(droneId).IsActive) throw new ObjectIsNotActiveException();


            Drone blDrone = new();
            blDrone.Id = drone.Id;
            blDrone.MaxWeight = drone.MaxWeight;
            blDrone.Model = drone.Model;
            blDrone.DroneStatus = drone.DroneStatus;
            blDrone.CurrentLocation = drone.CurrentLocation;
            blDrone.BatteryStatus = drone.BatteryStatus;

            if (drone.DroneStatus == DroneStatuses.Shipment)
            {
                try
                {
                    blDrone.ParcelInDelivery = SetParcelInDelivery(drone.DeliveryParcelId);
                }
                catch (ObjectNotFoundException)
                {
                    throw new ObjectNotFoundException("parcel");
                }
                lock (dalObject)
                {
                    DO.Parcel parcel = dalObject.GetParcelById(drone.DeliveryParcelId);
                    if (parcel.PickedUp != null && parcel.Delivered == null)
                        blDrone.ParcelInDelivery.ParcelStatus = true;
                }
            }
            return blDrone;
        }
        /// <summary>
        /// View list of droneToList.
        /// </summary>
        /// <returns> List of droneToList </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetAllDroneToList()
        {
            return from droneToList in droneToLists select droneToList;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesToList(Predicate<DroneToList> predicate)
        {
            return droneToLists.FindAll(predicate);
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