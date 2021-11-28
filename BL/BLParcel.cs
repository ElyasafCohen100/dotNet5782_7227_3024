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
        /// <exception cref="InvalidInputException">Thrown if parcel details are invalid</exception>
        public void AddNewParcelBL(Parcel parcel)
        {
            if (parcel.senderCustomer.Id < 100000000 || parcel.senderCustomer.Id >= 1000000000) 
                throw new InvalidInputException("sender Id");
            if (parcel.receiverCustomer.Id < 100000000 || parcel.receiverCustomer.Id >= 1000000000) 
                throw new InvalidInputException("receiver Id");
            if((int)parcel.Weight < 0 || (int)parcel.Weight > 2) throw new InvalidInputException("Weight");
            if((int)parcel.Priority < 0 || (int)parcel.Priority > 2) throw new InvalidInputException("priority");
            

            IDAL.DO.Parcel dalParcel = new();

            dalParcel.SenderId = parcel.senderCustomer.Id;
            dalParcel.TargetId = parcel.receiverCustomer.Id;
            dalParcel.Weight = (IDAL.DO.WeightCategories)parcel.Weight;
            dalParcel.Priority = (IDAL.DO.Priorities)parcel.Priority;

            dalParcel.Requested = DateTime.Now;
            dalParcel.Scheduled = DateTime.MinValue;
            dalParcel.PickedUp = DateTime.MinValue;
            dalParcel.Delivered = DateTime.MinValue;

            dalObject.SetNewParcel(dalParcel);
        }

        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// get the parcel from DAL by parcel ID 
        /// </summary>
        /// <param name="parcelId">id of parcel </param>
        /// <returns> BL parcel </returns>
        /// <exception cref="InvalidInputException">Thrown if drone id is invalid</exception>
        public Parcel FindParcelByIdBL(int parcelId)
        {
            if (parcelId <= 0) throw new InvalidInputException("Id");

            IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(parcelId);
            Parcel Parcel = new();
         

            Parcel.Id = dalParcel.Id;

            IDAL.DO.Customer dalCustomer = new();
            dalCustomer = dalObject.FindCustomerById(Parcel.senderCustomer.Id);
            Parcel.senderCustomer.Id = dalParcel.SenderId;
            Parcel.senderCustomer.Name = dalCustomer.Name;

            dalCustomer = dalObject.FindCustomerById(Parcel.receiverCustomer.Id);
            Parcel.receiverCustomer.Id = dalParcel.TargetId;
            Parcel.receiverCustomer.Name = dalCustomer.Name;

            Parcel.Weight = (WeightCategories)dalParcel.Weight;
            Parcel.Priority = (Priorities)dalParcel.Priority;

            if (dalParcel.DroneId != 0)
            {
                Drone blDrone = new();
                Parcel.Drone.Id = dalParcel.DroneId;
                blDrone = FindDroneByIdBL(Parcel.Drone.Id);
                Parcel.Drone.BatteryStatus = blDrone.BatteryStatus;
                Parcel.Drone.CurrentLocation.Latitude = blDrone.CurrentLocation.Latitude;
                Parcel.Drone.CurrentLocation.Longitude = blDrone.CurrentLocation.Longitude;
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
