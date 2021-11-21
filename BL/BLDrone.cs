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

        public void UpdateDroneModelBL(int droneId, string newModel)
        {
            IDAL.DO.Drone drone = new();

            drone = dalObject.FindDroneById(droneId);
            drone.Model = newModel;
        }


        public Drone FindDroneByIdBL(int droneId)
        {
            //var drone = from Item in droneToLists 
            //            where Item.Id == droneId 
            //            select Item;

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
        internal ParcelInDelivery SetParcelInDelivery(int parcelId)
        {
            ParcelInDelivery parcelInDalivery = new();
            IDAL.DO.Parcel parcel = dalObject.FindParcelById(parcelId);

            parcelInDalivery.Id = parcel.Id;
            parcelInDalivery.WeightCategory = (WeightCategories)parcel.Weight;
            parcelInDalivery.Priority = (Priorities)parcel.Priority;

            IDAL.DO.Customer sender = dalObject.FindCustomerById(parcel.SenderId);
            IDAL.DO.Customer target = dalObject.FindCustomerById(parcel.TargetId);
            parcelInDalivery.DeliveryDistance = dalObject.Distance(sender.Lattitude, target.Lattitude, sender.Longitude, target.Longitude);

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
    }
}
