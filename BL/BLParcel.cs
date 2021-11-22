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
        public void AddNewParcelBL(Parcel parcel)
        {
            IDAL.DO.Parcel newParcel = new();

            newParcel.SenderId = parcel.senderCustomer.Id;
            newParcel.TargetId = parcel.receiverCustomer.Id;
            newParcel.Weight = (IDAL.DO.WeightCategories)parcel.Weight;
            newParcel.Priority = (IDAL.DO.Priorities)parcel.Priority;

            newParcel.Requested = DateTime.Now;
            newParcel.Scheduled = DateTime.MinValue;
            newParcel.PickedUp = DateTime.MinValue;
            newParcel.Delivered = DateTime.MinValue;

            dalObject.SetNewParcel(newParcel);
        }

        public Parcel FindParcelByIdBL(int parcelId)
        {
            IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(parcelId);
            Parcel blParcel = new();
           
            IDAL.DO.Customer blCustomer = new();
            Drone blDrone = new();

            blParcel.Id = dalParcel.Id;

            blParcel.senderCustomer.Id = dalParcel.SenderId;
            blCustomer = dalObject.FindCustomerById(blParcel.senderCustomer.Id);
            blParcel.senderCustomer.Name = blCustomer.Name;

            blParcel.receiverCustomer.Id = dalParcel.TargetId;
            blCustomer = dalObject.FindCustomerById(blParcel.receiverCustomer.Id);
            blParcel.receiverCustomer.Name = blCustomer.Name;

            blParcel.Weight = (WeightCategories)dalParcel.Weight;
            blParcel.Priority = (Priorities)dalParcel.Priority;

            blParcel.Drone.Id = dalParcel.DroneId;
            blDrone = FindDroneByIdBL(blParcel.Drone.Id);
            blParcel.Drone.BatteryStatus = blDrone.BatteryStatus;
            blParcel.Drone.CurrentLocation.Lattitude = blDrone.CurrentLocation.Lattitude;
            blParcel.Drone.CurrentLocation.Longitude = blDrone.CurrentLocation.Longitude;

            blParcel.Requested = dalParcel.Requested;
            blParcel.Scheduled = dalParcel.Scheduled;
            blParcel.PickedUp = dalParcel.PickedUp;
            blParcel.Delivered = dalParcel.Delivered;

            return blParcel;
        }
    }
}
