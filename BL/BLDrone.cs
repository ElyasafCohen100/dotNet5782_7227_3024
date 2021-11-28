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
        //---------------------------------- ADD FUNCTIONS ----------------------------------------//

        /// <summary>
        /// add new BLdrone to the list in the DATA SOURCE
        /// </summary>
        /// <param name="drone"> drone </param>
        /// <param name="baseStationID"> ID of bace station </param>
        /// <exception cref="InvalidInputException">Thrown if drone details is invalid</exception>
        /// <exception cref="ObjectNotFoundException">Thrown if no base-station found</exception>
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
        }

        /// <summary>
        /// check if the drone is already exist
        /// </summary>
        /// <param name="droneId">drone id</param>
        /// <exception cref="ObjectAlreadyExistException">Thrown if drone id is already exist</exception>
        static void IfExistDrone(int droneId)
        {
            foreach (var myDrone in dalObject.GetDroneList())
            {
                if (myDrone.Id == droneId) throw new ObjectAlreadyExistException("drone");
            }
        }

        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// find BL drone by droneID
        /// </summary>
        /// <param name="droneId">the ID of drone</param>
        /// <returns> BL drone </returns>
        /// <exception cref="InvalidInputException">Thrown if drone id is invalid</exception>
        /// <exception cref="ObjectNotFoundException">Thrown if no drone with such id found</exception>
        public Drone FindDroneByIdBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            DroneToList drone = droneToLists.Find(x => x.Id == droneId);
            if (drone.Id == 0) throw new ObjectNotFoundException("drone");

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

                return myDrone;
            }
            else
            {
                return myDrone;
            }
        }

        //---------------------------------- SET FUNCTIONS ---------------------------------------//

        /// <summary>
        /// set the detailes of the fild "parcel in dalivety" of drone
        /// </summary>
        /// <param name="parcelId"> the ID of parcel </param>
        /// <returns> the parcel type of "parcel in delivery" </returns>
        /// <exception cref="InvalidInputException">Thrown if parcel id is invalid</exception>
        /// <exception cref="ObjectNotFoundException">Thrown if parcel with such id not found</exception>
        internal ParcelInDelivery SetParcelInDelivery(int parcelId)
        {
            if (parcelId <= 0) throw new InvalidInputException("Id");

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

        //--------------------------------- UPDATE FUNCTIONS --------------------------------------//

        /// <summary>
        /// update the status of BL drone to available and decrease the battery of drone
        /// </summary>
        /// <param name="droneId"> the ID of the drone </param>
        /// <exception cref="InvalidInputException">Thrown if drone id is invalid</exception>
        /// <exception cref="ObjectNotFoundException">Thrown if drone with such id not found</exception>
        /// <exception cref="OutOfBatteryException">Thrown if there is not enough battery</exception>
        public void UpdateDroneToChargingBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            var drone = droneToLists.Find(x => x.Id == droneId && x.DroneStatus == DroneStatuses.Available);
            if (drone.Id != droneId) throw new ObjectNotFoundException("drone");

            double minBatteryForCharging = FindMinPowerSuplyForCharging(drone);

            if (drone.BatteryStatus < minBatteryForCharging)
            {
                throw new OutOfBatteryException(droneId.ToString());
            }
            else
            {
                // Update drone detailes
                int nearestBaseStationID;
                Station myStation = new();

                drone.BatteryStatus = drone.BatteryStatus - minBatteryForCharging;

                nearestBaseStationID = FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
                myStation = FindStationByIdBL(nearestBaseStationID);

                drone.CurrentLocation = myStation.Location;
                drone.DroneStatus = DroneStatuses.Maintenance;

                //Update Station and DroneCharge detailes
                dalObject.UpdateDroneToCharging(drone.Id, myStation.Id);
            }
        }

        /// <summary>
        /// update the ID and the model of the BL drone by using DAL
        /// </summary>
        /// <param name="droneId"> ID of drone </param>
        /// <param name="newModel"> The model we are changing to </param>
        /// <exception cref="InvalidInputException">Thrown if drone id is not valid</exception>
        public void UpdateDroneModelBL(int droneId, string newModel)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            IDAL.DO.Drone drone = new();

            drone = dalObject.FindDroneById(droneId);
            drone.Model = newModel;
        }

        /// <summary>
        /// update the status of drone to available and increase the battery of drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargeTime"></param>
        /// <exception cref="InvalidInputException">Thrown if drone id or charging time is invalid</exception>
        /// <exception cref="NotValidRequestException">Thrown if the drone is not in 'maintenance' status</exception>
        public void UpdateDroneFromChargingBL(int droneId, double chargeTime)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");
            if (chargeTime < 0) throw new InvalidInputException("charging time");

            Drone myDrone = FindDroneByIdBL(droneId);
            if (myDrone.DroneStatus == DroneStatuses.Maintenance)
            {
                dalObject.UpdateDroneFromCharging(droneId);
                myDrone.DroneStatus = DroneStatuses.Available;
                myDrone.BatteryStatus += dalObject.ElectricityUseRequest()[4] * chargeTime;
            }
            else
            {
                throw new NotValidRequestException("The drone has not in maintenance status");
            }
        }

        /// <summary>
        /// update drone ID of parcel
        /// </summary>
        /// <param name="droneId"> the ID of drone </param>
        /// <exception cref="InvalidInputException">Thrown if drone id is invalid</exception>
        public void UpdateDroneIdOfParcelBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            Drone blDrone = FindDroneByIdBL(droneId);

            if (blDrone.DroneStatus == DroneStatuses.Available)
            {
                IDAL.DO.Parcel dalParcel = new();
                dalParcel.Priority = IDAL.DO.Priorities.Regular;
                dalParcel.Weight = IDAL.DO.WeightCategories.Light;

                double minDistance = double.MaxValue;

                Customer customerOfParcel = new();

                bool flag = false;

                IEnumerable<IDAL.DO.Parcel> parcels = dalObject.GetParcelList();
                if (parcels.Count() > 0)
                {
                    foreach (var parcel in parcels)
                    {

                        if ((int)blDrone.MaxWeight <= (int)parcel.Weight)
                        {
                            customerOfParcel = FindCustomerByIdBL(parcel.SenderId);
                            double distance = dalObject.Distance(blDrone.CurrentLocation.Latitude,
                            customerOfParcel.Location.Latitude,
                            blDrone.CurrentLocation.Longitude,
                            customerOfParcel.Location.Longitude);

                            if (blDrone.BatteryStatus >= FindMinSuplyForAllPath(blDrone.Id, customerOfParcel.Id))
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
                                    blDrone.DroneStatus = DroneStatuses.Shipment;
                                    dalParcel.DroneId = blDrone.Id;
                                    dalParcel.Scheduled = DateTime.Now;
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
        }

        /// <summary>
        /// update the parcel status to picked up by the drone
        /// </summary>
        /// <param name="droneId"> the ID of the drone </param>
        /// <exception cref="InvalidInputException">Thrown if drone id is invalid</exception>
        /// <exception cref="NotValidRequestException">Thrown if the drone has already picked up the parcel </exception>
        public void UpdatePickedUpParcelByDroneIDBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            Drone myDrone = FindDroneByIdBL(droneId);
            Parcel myParcel = FindParcelByIdBL(myDrone.ParcelInDelivery.Id);

            if (myParcel.PickedUp == DateTime.MinValue)
            {
                myDrone.BatteryStatus -= FindMinPowerSuplyForDistanceBetweenDroneToTarget(myDrone.Id, myParcel.Id);

                Customer myCostomer = FindCustomerByIdBL(myDrone.ParcelInDelivery.senderCustomer.Id);
                myDrone.CurrentLocation = myCostomer.Location;

                myParcel.PickedUp = DateTime.Now;
            }
            else
            {
                throw new NotValidRequestException("The drone has already picked up the parcel");
            }
        }

        /// <summary>
        /// update the parcel status to delivered by the drone 
        /// </summary>
        /// <param name="droneId">the ID of drone </param>
        /// <exception cref="NotValidRequestException">Thrown if the drone has already delivered the parcel </exception>
        public void UpdateDeliveredParcelByDroneIdBL(int droneId)
        {
            Drone myDrone = FindDroneByIdBL(droneId);
            Parcel myParcel = FindParcelByIdBL(myDrone.ParcelInDelivery.Id);

            if ((myParcel.PickedUp != DateTime.MinValue) && (myParcel.Delivered == DateTime.MinValue))
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

        //---------------------------------- VIEW FUNCTIONS ---------------------------------------//
        
        /// <summary>
        /// view list of droneToList
        /// </summary>
        /// <returns> the list of droneToList </returns>
        public IEnumerable<DroneToList> ViewDroneToList()
        {
            return droneToLists;
        }
    }
}
