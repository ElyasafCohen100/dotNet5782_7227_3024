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
        /// Add new BL parcel to the list by using DAL.
        /// </summary>
        /// <param name="parcel"> Parcel object /param>
        /// <exception cref="InvalidInputException"> Thrown if parcel details are invalid </exception>
        public void AddNewParcelBL(Parcel parcel)
        {
            if (parcel.senderCustomer.Id < 100000000 || parcel.senderCustomer.Id >= 1000000000)
                throw new InvalidInputException("sender Id");
            if (parcel.receiverCustomer.Id < 100000000 || parcel.receiverCustomer.Id >= 1000000000)
                throw new InvalidInputException("receiver Id");
            if ((int)parcel.Weight < 0 || (int)parcel.Weight > 2) throw new InvalidInputException("Weight");
            if ((int)parcel.Priority < 0 || (int)parcel.Priority > 2) throw new InvalidInputException("priority");


            IDAL.DO.Parcel dalParcel = new();

            dalParcel.SenderId = parcel.senderCustomer.Id;
            dalParcel.TargetId = parcel.receiverCustomer.Id;
            dalParcel.Weight = (IDAL.DO.WeightCategories)parcel.Weight;
            dalParcel.Priority = (IDAL.DO.Priorities)parcel.Priority;

            dalParcel.Requested = DateTime.Now;
            dalParcel.Scheduled = null;
            dalParcel.PickedUp = null;
            dalParcel.Delivered = null;

            dalObject.SetNewParcel(dalParcel);
        }

        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Find BL parcel by Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> BL parcel object </returns>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        public Parcel FindParcelByIdBL(int parcelId)
        {
            if (parcelId <= 0) throw new InvalidInputException("Id");
            IDAL.DO.Parcel dalParcel;
            try
            {
                dalParcel = dalObject.FindParcelById(parcelId);
            }
            catch (IDAL.DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
            Parcel Parcel = new();

            Parcel.Id = dalParcel.Id;

            IDAL.DO.Customer dalCustomer = new();
            dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
            Parcel.senderCustomer.Id = dalParcel.SenderId;
            Parcel.senderCustomer.Name = dalCustomer.Name;

            dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
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

        //----------------------- VIEW FUNCTIONS -----------------------//

        /// <summary>
        /// View list of parcelToList detailes.
        /// </summary>
        /// <returns> List of parcelToList detailes </returns>
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

                if (dalParcel.Delivered != null)
                {
                    Parcel.ParcelStatus = ParcelStatus.Delivered;
                }
                else if (dalParcel.PickedUp != null)
                {
                    Parcel.ParcelStatus = ParcelStatus.PickedUp;
                }
                else if (dalParcel.Scheduled != null)
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
        /// View list of non associate ParcelsList. 
        /// </summary>
        /// <returns> List of non associate ParcelsList </returns>
        public IEnumerable<ParcelToList> ViewNonAssociateParcelsListBL()
        {
            List<ParcelToList> ParcelToList = new();

            foreach (var parcel in dalObject.GetParcels(x => x.DroneId == 0))
            {
                IDAL.DO.Parcel dalParcel = dalObject.FindParcelById(parcel.Id);
                ParcelToList blParcel = new();
                IDAL.DO.Customer dalCustomer = new();

                blParcel.Id = dalParcel.Id;

                dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
                blParcel.SenderName = dalCustomer.Name;

                dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
                blParcel.ReceiverName = dalCustomer.Name;

                blParcel.WeightCategory = (WeightCategories)dalParcel.Weight;
                blParcel.Prioritie = (Priorities)dalParcel.Priority;

                blParcel.ParcelStatus = ParcelStatus.Requested;

                ParcelToList.Add(blParcel);
            }

            return ParcelToList;
        }
    }
}
