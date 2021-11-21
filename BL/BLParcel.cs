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
            Parcel myParcel = new();
            IDAL.DO.Customer myCustomer = new();
            Drone myDrone = new();

            myParcel.Id = dalParcel.Id;

            myParcel.senderCustomer.Id = dalParcel.SenderId;
            myCustomer = dalObject.FindCustomerById(myParcel.senderCustomer.Id);
            myParcel.senderCustomer.Name = myCustomer.Name;

            myParcel.receiverCustomer.Id = dalParcel.TargetId;
            myCustomer = dalObject.FindCustomerById(myParcel.receiverCustomer.Id);
            myParcel.receiverCustomer.Name = myCustomer.Name;

            myParcel.Weight = (WeightCategories)dalParcel.Weight;
            myParcel.Priority = (Priorities)dalParcel.Priority;

            myParcel.Drone.Id = dalParcel.DroneId;
            myDrone = FindDroneByIdBL(myParcel.Drone.Id);
            myParcel.Drone.BatteryStatus = myDrone.BatteryStatus;
            myParcel.Drone.CurrentLocation.Lattitude = myDrone.CurrentLocation.Lattitude;
            myParcel.Drone.CurrentLocation.Longitude = myDrone.CurrentLocation.Longitude;

            myParcel.Requested = dalParcel.Requested;
            myParcel.Scheduled = dalParcel.Scheduled;
            myParcel.PickedUp = dalParcel.PickedUp;
            myParcel.Delivered = dalParcel.Delivered;

            return myParcel;
        }
    }
}
