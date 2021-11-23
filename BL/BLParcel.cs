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
        /// add new BL parcel to the DATA SOURSCE by DAL
        /// </summary>
        /// <param name="parcel"> the new parcel</param>
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

        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// get the parcel from DAL by parcel ID 
        /// </summary>
        /// <param name="parcelId">id of parcel </param>
        /// <returns> BL parcel </returns>
        public Parcel FindParcelByIdBL(int parcelId)
        {
            IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(parcelId);
            Parcel Parcel = new();
         
            IDAL.DO.Customer myCustomer = new();
            Drone Drone = new();

            Parcel.Id = dalParcel.Id;

            Parcel.senderCustomer.Id = dalParcel.SenderId;
            myCustomer = dalObject.FindCustomerById(Parcel.senderCustomer.Id);
            Parcel.senderCustomer.Name = myCustomer.Name;

            Parcel.receiverCustomer.Id = dalParcel.TargetId;
            myCustomer = dalObject.FindCustomerById(Parcel.receiverCustomer.Id);
            Parcel.receiverCustomer.Name = myCustomer.Name;

            Parcel.Weight = (WeightCategories)dalParcel.Weight;
            Parcel.Priority = (Priorities)dalParcel.Priority;

            if (dalParcel.DroneId != 0)
            {
                Parcel.Drone.Id = dalParcel.DroneId;
                Drone = FindDroneByIdBL(Parcel.Drone.Id);
                Parcel.Drone.BatteryStatus = Drone.BatteryStatus;
                Parcel.Drone.CurrentLocation.Lattitude = Drone.CurrentLocation.Lattitude;
                Parcel.Drone.CurrentLocation.Longitude = Drone.CurrentLocation.Longitude;
            }

            Parcel.Requested = dalParcel.Requested;
            Parcel.Scheduled = dalParcel.Scheduled;
            Parcel.PickedUp = dalParcel.PickedUp;
            Parcel.Delivered = dalParcel.Delivered;

            return Parcel;
        }

        //---------------------------------- VIEW FUNCTIONS ---------------------------------------//

        /// <summary>
        /// view list of parcelTOLIst detailes
        /// </summary>
        /// <returns>list of parcelTOLIst detailes</returns>
        public IEnumerable<ParcelToList> ViewParcelToList()
        {
            List<ParcelToList> ParcelList = new();

            foreach (var parcel in dalObject.GetParcelList())
            {
                IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(parcel.Id);
                ParcelToList Parcel = new();
                IDAL.DO.Customer dalCustomer = new();

                Parcel.Id = dalParcel.Id;

                dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
                Parcel.SenderName = dalCustomer.Name;

                dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
                Parcel.ReceiverName = dalCustomer.Name;

                Parcel.WeightCategory = (WeightCategories)dalParcel.Weight;
                Parcel.Prioritie = (Priorities)dalParcel.Priority;

                if (dalParcel.Delivered != DateTime.MinValue)
                {
                    Parcel.ParcelStatus = ParcelStatus.Delivered;
                }
                else if (dalParcel.PickedUp != DateTime.MinValue)
                {
                    Parcel.ParcelStatus = ParcelStatus.PickedUp;
                }
                else if (dalParcel.Scheduled != DateTime.MinValue)
                {
                    Parcel.ParcelStatus = ParcelStatus.Scheduled;
                }
                else
                {
                    Parcel.ParcelStatus = ParcelStatus.Requested;
                }

                ParcelList.Add(Parcel);
            }
            return ParcelList;
        }

        /// <summary>
        /// view list of nonassociate ParcelsList 
        /// </summary>
        /// <returns> list of nonassociate ParcelsList </returns>
        public IEnumerable<ParcelToList> ViewNonAssociateParcelsListBL()
        {
            List<ParcelToList> ParcelToList = new();

            foreach (var parcel in dalObject.GetNonAssociateParcelList())
            {
                IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(parcel.Id);
                ParcelToList myParcel = new();
                IDAL.DO.Customer dalCustomer = new();

                myParcel.Id = dalParcel.Id;

                dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
                myParcel.SenderName = dalCustomer.Name;

                dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
                myParcel.ReceiverName = dalCustomer.Name;

                myParcel.WeightCategory = (WeightCategories)dalParcel.Weight;
                myParcel.Prioritie = (Priorities)dalParcel.Priority;

                myParcel.ParcelStatus = ParcelStatus.Requested;
            }

            return ParcelToList;
        }
    }
}
