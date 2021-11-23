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
        public void AddNewDroneBL(Drone drone, int baseStationID)
        {
            Random r = new();
            IDAL.DO.Drone newDrone = new();

            newDrone.Id = drone.Id;
            newDrone.Model = drone.Model;
            newDrone.MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight;

            drone.DroneStatus = DroneStatuses.Maintenance;
            drone.BatteryStatus = r.Next(20, 41);

            IDAL.DO.Station myStaion = dalObject.FindStationById(baseStationID);
            drone.CurrentLocation.Lattitude = myStaion.Lattitude;
            drone.CurrentLocation.Longitude = myStaion.Longitude;

            dalObject.SetNewDrone(newDrone);
        }

        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// find BL drone by droneID
        /// </summary>
        /// <param name="droneId">the ID of drone</param>
        /// <returns> BL drone </returns>
        public Drone FindDroneByIdBL(int droneId)
        {
           
            DroneToList drone = droneToLists.Find(x => x.Id == droneId);

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
        internal ParcelInDelivery SetParcelInDelivery(int parcelId)
        {
            ParcelInDelivery parcelInDalivery = new();
            IDAL.DO.Parcel parcel = dalObject.FindParcelById(parcelId);

            parcelInDalivery.Id = parcel.Id;
            parcelInDalivery.Weight = (WeightCategories)parcel.Weight;
            parcelInDalivery.Priority = (Priorities)parcel.Priority;

            IDAL.DO.Customer sender = dalObject.FindCustomerById(parcel.SenderId);
            IDAL.DO.Customer target = dalObject.FindCustomerById(parcel.TargetId);
            parcelInDalivery.DistanceDelivery = dalObject.Distance(sender.Lattitude, target.Lattitude, sender.Longitude, target.Longitude);

            parcelInDalivery.receiverCustomer.Id = target.Id;
            parcelInDalivery.receiverCustomer.Name = target.Name;

            parcelInDalivery.senderCustomer.Id = sender.Id;
            parcelInDalivery.senderCustomer.Name = sender.Name;

            parcelInDalivery.SourceLocation.Lattitude = sender.Lattitude;
            parcelInDalivery.SourceLocation.Longitude = sender.Longitude;

            parcelInDalivery.TargetLocation.Lattitude = target.Lattitude;
            parcelInDalivery.TargetLocation.Longitude = target.Longitude;

            return parcelInDalivery;
        }

        //--------------------------------- UPDATE FUNCTIONS --------------------------------------//

        /// <summary>
        /// update the status of BL drone to available and decrease the battery of drone
        /// </summary>
        /// <param name="droneId"></param>
        public void UpdateDroneToChargingBL(int droneId)
        {
            var drone = droneToLists.Find(x => x.Id == droneId && x.DroneStatus == DroneStatuses.Available);
            double minBatteryForCharging = FindMinPowerSuplyForCharging(drone);

            if (drone.BatteryStatus < minBatteryForCharging)
            {
                // TODO: create an exeption
            }
            else
            {
                //-----------Drone --------//
                int nearestBaseStationID;
                Station myStation = new();

                drone.BatteryStatus = drone.BatteryStatus - minBatteryForCharging;

                nearestBaseStationID = FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
                myStation = FindStationByIdBL(nearestBaseStationID);

                drone.CurrentLocation = myStation.Location;
                drone.DroneStatus = DroneStatuses.Maintenance;

                //---------Station + DroneCharge-------//
                dalObject.UpdateDroneToCharging(drone.Id, myStation.Id);
            }
        }

        /// <summary>
        /// update the ID and the model of the BL drone by using DAL
        /// </summary>
        /// <param name="droneId"> ID of drone </param>
        /// <param name="newModel"> The model we are changing to </param>
        public void UpdateDroneModelBL(int droneId, string newModel)
        {
            IDAL.DO.Drone drone = new();

            drone = dalObject.FindDroneById(droneId);
            drone.Model = newModel;
        }

        /// <summary>
        /// update the status of drone to available and increase the battery of drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargeTime"></param>
        public void UpdateDroneFromChargingBL(int droneId, double chargeTime)
        {
            Drone myDrone = FindDroneByIdBL(droneId);
            if (myDrone.DroneStatus == DroneStatuses.Maintenance)
            {
                dalObject.UpdateDroneFromCharging(droneId);
                myDrone.DroneStatus = DroneStatuses.Available;
                myDrone.BatteryStatus += dalObject.ElectricityUseRequest()[4] * chargeTime;
            }
            else
            {
                //TODO throw an exeption 
            }

        }

        /// <summary>
        /// update drone ID of parcel
        /// </summary>
        /// <param name="droneId"> the ID of drone </param>
        public void UpdateDroneIdOfParcelBL(int droneId)
        {
            Drone myDrone = FindDroneByIdBL(droneId);

            if (myDrone.DroneStatus == DroneStatuses.Available)
            {
                IDAL.DO.Parcel myParcel = new();
                myParcel.Priority = IDAL.DO.Priorities.Regular;
                myParcel.Weight = IDAL.DO.WeightCategories.Light;

                double disFromMyParcelSenderToMyDrone = double.MaxValue;

                Customer customerOfParcel = new();

                bool flag = false;

                foreach (var parcel in dalObject.GetParcelList())
                {

                    if ((int)myDrone.MaxWeight <= (int)parcel.Weight)
                    {
                        customerOfParcel = FindCustomerByIdBL(parcel.SenderId);
                        double disFromParcelSenderToMyDrone = dalObject.Distance(myDrone.CurrentLocation.Lattitude,
                        customerOfParcel.Location.Lattitude,
                        myDrone.CurrentLocation.Longitude,
                        customerOfParcel.Location.Longitude);

                        if (myDrone.BatteryStatus >= FindMinSuplyForAllPath(myDrone.Id, customerOfParcel.Id))
                        {
                            if ((int)parcel.Priority > (int)myParcel.Priority)
                            {
                                myParcel = parcel;
                                flag = true;
                            }
                            else if (((int)parcel.Weight < (int)myParcel.Weight) && ((int)parcel.Priority == (int)myParcel.Priority))
                            {
                                myParcel = parcel;
                                flag = true;
                            }
                            else if ((disFromParcelSenderToMyDrone < disFromMyParcelSenderToMyDrone) && ((int)parcel.Weight == (int)myParcel.Weight) && ((int)parcel.Priority == (int)myParcel.Priority))
                            {
                                myParcel = parcel;
                                flag = true;
                            }

                            if (flag)
                            {
                                myDrone.DroneStatus = DroneStatuses.Shipment;
                                myParcel.DroneId = myDrone.Id;
                                myParcel.Scheduled = DateTime.Now;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// update the parcel status to picked up by the drone
        /// </summary>
        /// <param name="droneId"> the ID of the drone </param>
        public void UpdatePickedUpParcelByDroneIDBL(int droneId)
        {
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
                //TODO throw an exepion.
            }
        }

        /// <summary>
        /// update the parcel status to delivered by the drone 
        /// </summary>
        /// <param name="droneId">the ID of drone </param>
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
                //TODO throw an exepion.
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
