using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.IBL
    {
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
            IDAL.DO.Drone newDrone = new();

            newDrone.Id = drone.Id;
            newDrone.Model = drone.Model;
            newDrone.MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight;

            drone.DroneStatus = DroneStatuses.Maintenance;
            drone.BatteryStatus = r.Next(20, 41);

            try
            {
                IDAL.DO.Station myStaion = dalObject.FindStationById(baseStationID);

                drone.CurrentLocation.Latitude = myStaion.Latitude;
                drone.CurrentLocation.Longitude = myStaion.Longitude;
            }
            catch (IDAL.DO.ObjectNotFoundException)
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


        /// <summary>
        /// Check if the drone is already exist.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="ObjectAlreadyExistException"> Thrown if drone id is already exist </exception>
        static void IfExistDrone(int droneId)
        {
            foreach (var myDrone in dalObject.GetDroneList())
            {
                if (myDrone.Id == droneId) throw new ObjectAlreadyExistException("drone");
            }
        }

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

            Drone myDrone = new();
            myDrone.Id = drone.Id;
            myDrone.MaxWeight = drone.MaxWeight;
            myDrone.Model = drone.Model;
            myDrone.DroneStatus = drone.DroneStatus;
            myDrone.BatteryStatus = drone.BatteryStatus;
            myDrone.CurrentLocation = drone.CurrentLocation;

            if (drone.DroneStatus == DroneStatuses.Shipment)
            {
                myDrone.ParcelInDelivery = SetParcelInDelivery(drone.DeliveryParcelId);
            }
            if (dalObject.FindParcelById(drone.DeliveryParcelId).PickedUp != null)
                myDrone.ParcelInDelivery.ParcelStatus = true;

            return myDrone;
        }

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

            IDAL.DO.Parcel parcel = new();
            try
            {
                parcel = dalObject.FindParcelById(parcelId);
            }
            catch (IDAL.DO.ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("Parcel");
            }

            parcelInDalivery.Id = parcel.Id;
            parcelInDalivery.Weight = (WeightCategories)parcel.Weight;
            parcelInDalivery.Priority = (Priorities)parcel.Priority;

            IDAL.DO.Customer sender = dalObject.FindCustomerById(parcel.SenderId);
            IDAL.DO.Customer target = dalObject.FindCustomerById(parcel.TargetId);
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

            dalObject.UpdateDroneModel(droneId, newModel);

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
                dalObject.UpdateDroneToCharging(drone.Id, myStation.Id);
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
            dalObject.UpdateDroneFromCharging(droneId);
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
                IEnumerable<IDAL.DO.Parcel> parcels = dalObject.GetParcelList();
                if (parcels.Any())
                {

                    IDAL.DO.Parcel dalParcel = new();
                    dalParcel.Priority = IDAL.DO.Priorities.Regular;
                    dalParcel.Weight = IDAL.DO.WeightCategories.Light;

                    double minDistance = double.MaxValue;

                    Customer customerOfParcel = new();

                    bool flag = false;
                    foreach (var parcel in parcels)
                    {

                        if ((int)droneToList.MaxWeight <= (int)parcel.Weight)
                        {
                            customerOfParcel = FindCustomerByIdBL(parcel.SenderId);
                            double distance = dalObject.Distance(droneToList.CurrentLocation.Latitude,
                                                                      customerOfParcel.Location.Latitude,
                                                                      droneToList.CurrentLocation.Longitude,
                                                                      customerOfParcel.Location.Longitude);

                            if (droneToList.BatteryStatus >= FindMinSuplyForAllPath(droneToList.Id, customerOfParcel.Id))
                            {
                                if ((int)parcel.Priority > (int)dalParcel.Priority)
                                {
                                    dalParcel = parcel;
                                    flag = true;
                                }
                                else if (((int)parcel.Weight < (int)dalParcel.Weight) && ((int)parcel.Priority == (int)dalParcel.Priority))
                                {
                                    dalParcel = parcel;
                                    flag = true;
                                }
                                else if ((distance < minDistance) && ((int)parcel.Weight == (int)dalParcel.Weight) && ((int)parcel.Priority == (int)dalParcel.Priority))
                                {
                                    dalParcel = parcel;
                                    flag = true;
                                }

                                if (flag)
                                {
                                    droneToList.DroneStatus = DroneStatuses.Shipment;
                                    droneToList.DeliveryParcelId = dalParcel.Id;

                                    dalParcel.DroneId = droneToList.Id;
                                    dalParcel.Scheduled = DateTime.Now;
                                    flag = false;
                                }
                            }
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
                throw new InvalidOperationException("cannot send this drone");
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
            IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(droneToList.DeliveryParcelId);

            if (dalParcel.PickedUp == null && droneToList.DroneStatus == DroneStatuses.Shipment)
            {
                droneToList.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(droneToList.Id, dalParcel.SenderId);

                Customer myCostomer = FindCustomerByIdBL(dalParcel.SenderId);
                droneToList.CurrentLocation = myCostomer.Location;

                dalObject.UpdatePickedUpParcelById(dalParcel.Id);
                Console.WriteLine(dalObject.FindParcelById(dalParcel.Id).ToString());
            }
            else
            {
                throw new NotValidRequestException("The drone not in Shipment status or alredy picked up the parcel");
            }
        }

        /// <summary>
        /// Update the parcel status to delivered by the drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="NotValidRequestException"> Thrown if the drone has already delivered the parcel </exception>
        public void UpdateDeliveredParcelByDroneIdBL(int droneId)
        {
            Drone myDrone = FindDroneByIdBL(droneId);
            Parcel myParcel = FindParcelByIdBL(myDrone.ParcelInDelivery.Id);

            if ((myParcel.PickedUp != null) && (myParcel.Delivered == null))
            {
                myDrone.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(myDrone.Id, myParcel.Id);

                Customer myCostomer = FindCustomerByIdBL(myDrone.ParcelInDelivery.senderCustomer.Id);
                myDrone.CurrentLocation = myCostomer.Location;

                myDrone.DroneStatus = DroneStatuses.Available;

                myParcel.Delivered = DateTime.Now;
            }
            else
            {
                throw new NotValidRequestException("The drone has already delivered the parcel");
            }
        }

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
    }
}