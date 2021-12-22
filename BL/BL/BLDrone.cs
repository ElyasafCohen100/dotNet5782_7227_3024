using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        #region ADD
        //----------------------- ADD FUNCTIONS -----------------------//

        /// <summary>
        /// Add new BL drone to the list using DAL.
        /// </summary>
        /// <param name="drone"> Drone object </param>
        /// <param name="baseStationID"> Station Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone details is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if no base-station found </exception>
        public void AddNewDroneBL(Drone drone, int baseStationID)
        {

            if (drone.Id < 1000 || drone.Id >= 10000) throw new InvalidInputException("Id (Drone)");
            IfExistDrone(drone.Id);
            if (drone.Model == null) throw new InvalidInputException("Model");
            if ((int)drone.MaxWeight < 0 || (int)drone.MaxWeight > 2) throw new InvalidInputException("Max weight");
            if (baseStationID < 1000 || baseStationID >= 10000) throw new InvalidInputException("Id (base-station)");

            Random r = new();
            DO.Drone newDrone = new();

            newDrone.Id = drone.Id;
            newDrone.Model = drone.Model;
            newDrone.MaxWeight = (DO.WeightCategories)drone.MaxWeight;

            drone.DroneStatus = DroneStatuses.Maintenance;
            drone.BatteryStatus = r.Next(20, 41);

            try
            {
                DO.Station myStaion = dalObject.FindStationById(baseStationID);

                drone.CurrentLocation.Latitude = myStaion.Latitude;
                drone.CurrentLocation.Longitude = myStaion.Longitude;
            }
            catch (DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("base station");
            }

            dalObject.SetNewDrone(newDrone);
            UpdateDroneToListsList(drone);
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

        #region IfExistDron
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

        #endregion

        #region FIND
        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Find BL drone by drone Id.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <returns> BL drone object </returns>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if no drone with such id found </exception>
        public Drone FindDroneByIdBL(int droneId)
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
                blDrone.ParcelInDelivery = SetParcelInDelivery(drone.DeliveryParcelId);
            }

            DO.Parcel parcel = dalObject.FindParcelById(drone.DeliveryParcelId);
            if (parcel.PickedUp != null && parcel.Delivered == null)
                blDrone.ParcelInDelivery.ParcelStatus = true;
            return blDrone;
        }

        #endregion

        #region SET
        //----------------------- SET FUNCTIONS -----------------------//

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
                parcel = dalObject.FindParcelById(parcelId);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }

            parcelInDalivery.Id = parcel.Id;
            parcelInDalivery.Weight = (WeightCategories)parcel.Weight;
            parcelInDalivery.Priority = (Priorities)parcel.Priority;

            DO.Customer sender = dalObject.FindCustomerById(parcel.SenderId);
            DO.Customer target = dalObject.FindCustomerById(parcel.TargetId);
            parcelInDalivery.DistanceDelivery = dalObject.Distance(sender.Lattitude, target.Lattitude, sender.Longitude, target.Longitude);

            parcelInDalivery.receiverCustomer.Id = target.Id;
            parcelInDalivery.receiverCustomer.Name = target.Name;
            parcelInDalivery.TargetLocation.Latitude = target.Lattitude;
            parcelInDalivery.TargetLocation.Longitude = target.Longitude;

            parcelInDalivery.senderCustomer.Id = sender.Id;
            parcelInDalivery.senderCustomer.Name = sender.Name;
            parcelInDalivery.SourceLocation.Latitude = sender.Lattitude;
            parcelInDalivery.SourceLocation.Longitude = sender.Longitude;

            return parcelInDalivery;
        }

        #endregion

        #region UPDATE
        //----------------------- UPDATE FUNCTIONS -----------------------//

        /// <summary>
        /// Update Id and the model of the BL drone by using DAL.
        /// </summary>
        /// <param name="droneId"> Drone Id</param>
        /// <param name="newModel"> New model of drone </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is not valid </exception>
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
            droneToLists.Find(x => x.Id == droneId).Model = newModel;
        }

        /// <summary>
        /// Update the status of BL drone to available and decrease the battery of drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if drone with such id not found </exception>
        /// <exception cref="OutOfBatteryException"> Thrown if there is not enough battery </exception>
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
                myStation = FindStationByIdBL(nearestBaseStationId);

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
            }
        }

        /// <summary>
        /// Update the status of drone to available and increase the battery of drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <param name="chargeTime"> Charge time </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id or charging time is invalid </exception>
        /// <exception cref="NotValidRequestException"> Thrown if the drone is not in 'maintenance' status </exception>
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
        }

        private double TimeIntervalInMinutes(DateTime time1, DateTime time2)
        {
            TimeSpan interval = time2 - time1;
            double daysInMinutes = (interval.Days / 24) / 60;
            double hoursInMinutes = interval.Hours / 60;
            return daysInMinutes + hoursInMinutes + interval.Minutes;

        }

        /// <summary>
        /// Update drone Id of parcel.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        public void UpdateDroneIdOfParcelBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);

            if (droneToList.DroneStatus == DroneStatuses.Available)
            {
                IEnumerable<DO.Parcel> parcels = dalObject.GetParcels(x => x.DroneId == 0);
                if (parcels.Any())
                {
                    //Initiate some valuse for comparation
                    DO.Parcel dalParcel = new();
                    dalParcel.Priority = DO.Priorities.Regular;
                    dalParcel.Weight = DO.WeightCategories.Heavy;

                    double minDistance = double.MaxValue;

                    Customer sender = new();
                    Customer target = new();

                    bool haveParcelToAssociate = false;
                    foreach (var parcel in parcels)
                    {

                        if ((int)droneToList.MaxWeight <= (int)parcel.Weight)
                        {
                            sender = FindCustomerByIdBL(parcel.SenderId);
                            target = FindCustomerByIdBL(parcel.TargetId);

                            //Distance between drone and sender.
                            double distance = dalObject.Distance(droneToList.CurrentLocation.Latitude,
                                                                      sender.Location.Latitude,
                                                                      droneToList.CurrentLocation.Longitude,
                                                                      sender.Location.Longitude);

                            //Distance between sender and target.
                            distance += dalObject.Distance(sender.Location.Latitude,
                                                                      target.Location.Latitude,
                                                                     sender.Location.Longitude,
                                                                      target.Location.Longitude);

                            //Find the nearest base-station to the target.
                            int stationId = FindNearestBaseStationWithAvailableChargingSlots(new Location
                            {
                                Latitude = target.Location.Latitude,
                                Longitude = target.Location.Longitude
                            });

                            Station station = FindStationByIdBL(stationId);

                            //Distance between target and base-station.
                            distance += dalObject.Distance(target.Location.Latitude, station.Location.Latitude,
                                                        target.Location.Longitude, station.Location.Longitude);

                            if (droneToList.BatteryStatus > FindMinSuplyForAllPath(droneToList.Id, parcel.SenderId, parcel.TargetId))
                            {
                                if ((int)parcel.Priority > (int)dalParcel.Priority)
                                {
                                    dalParcel = parcel;
                                }
                                else if (((int)parcel.Weight < (int)dalParcel.Weight) && ((int)parcel.Priority == (int)dalParcel.Priority))
                                {
                                    dalParcel = parcel;
                                }
                                else if ((distance < minDistance) && ((int)parcel.Weight == (int)dalParcel.Weight) && ((int)parcel.Priority == (int)dalParcel.Priority))
                                {
                                    dalParcel = parcel;
                                }
                                minDistance = distance;
                                haveParcelToAssociate = true;
                            }
                        }
                    }
                    if (haveParcelToAssociate)
                    {
                        droneToList.DroneStatus = DroneStatuses.Shipment;
                        droneToList.DeliveryParcelId = dalParcel.Id;

                        try
                        {
                            dalObject.UpdateDroneIdOfParcel(dalParcel.Id, droneToList.Id);
                        }
                        catch (DO.ObjectNotFoundException e)
                        {
                            throw new ObjectNotFoundException(e.Message);
                        }
                    }
                }
                else
                {
                    throw new ObjectNotFoundException("parcel");
                }
            }
            else
            {
                throw new InvalidOperationException("Could not send this drone");
            }
        }

        /// <summary>
        /// Update the parcel status to picked up by the drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        /// <exception cref="NotValidRequestException"> Thrown if the drone has already picked up the parcel </exception>
        public void UpdatePickedUpParcelByDroneIdBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);
            if (droneToList == null) throw new ObjectNotFoundException("drone");
            DO.Parcel dalParcel = dalObject.FindParcelById(droneToList.DeliveryParcelId);

            if (dalParcel.PickedUp == null && droneToList.DroneStatus == DroneStatuses.Shipment)
            {
                droneToList.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(droneToList.Id, dalParcel.SenderId);

                Customer costomer = FindCustomerByIdBL(dalParcel.SenderId);
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
        public void UpdateDeliveredParcelByDroneIdBL(int droneId)
        {
            Drone blDrone = FindDroneByIdBL(droneId);
            if (blDrone.ParcelInDelivery.Id == 0) throw new NotValidRequestException("No parcel was found in this drone");

            Parcel blParcel = FindParcelByIdBL(blDrone.ParcelInDelivery.Id);
            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);

            if ((blParcel.PickedUp != null) && (blParcel.Delivered == null))
            {
                Customer blCostomer = FindCustomerByIdBL(blDrone.ParcelInDelivery.receiverCustomer.Id);
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

        #region VIEW
        //----------------------- VIEW FUNCTIONS -----------------------//

        /// <summary>
        /// View list of droneToList.
        /// </summary>
        /// <returns> List of droneToList </returns>
        public IEnumerable<DroneToList> ViewDroneToList()
        {
            return droneToLists;
        }
        public IEnumerable<DroneToList> ViewDronesToList(Predicate<DroneToList> predicate)
        {
            return droneToLists.FindAll(predicate);
        }
        #endregion
    }
}