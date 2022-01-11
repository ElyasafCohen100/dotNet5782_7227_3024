using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL : BlApi.IBL
    {

        #region Add

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


            DO.Parcel dalParcel = new();

            dalParcel.SenderId = parcel.senderCustomer.Id;
            dalParcel.TargetId = parcel.receiverCustomer.Id;
            dalParcel.Weight = (DO.WeightCategories)parcel.Weight;
            dalParcel.Priority = (DO.Priorities)parcel.Priority;

            dalParcel.Requested = DateTime.Now;
            dalParcel.Scheduled = null;
            dalParcel.PickedUp = null;
            dalParcel.Delivered = null;

            dalObject.SetNewParcel(dalParcel);
        }
        #endregion

        #region Find

        /// <summary>
        /// Find BL parcel by Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> BL parcel object </returns>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        public Parcel FindParcelByIdBL(int parcelId)
        {
            if (parcelId <= 0) throw new InvalidInputException("Id");
            DO.Parcel dalParcel;
            try
            {
                dalParcel = dalObject.FindParcelById(parcelId);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
            Parcel Parcel = new();

            Parcel.Id = dalParcel.Id;

            DO.Customer dalCustomer = new();
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
                try
                {
                    Drone blDrone = new();
                    blDrone = FindDroneByIdBL(dalParcel.DroneId);
                    Parcel.Drone.Id = blDrone.Id;
                    Parcel.Drone.BatteryStatus = blDrone.BatteryStatus;
                    Parcel.Drone.CurrentLocation.Latitude = blDrone.CurrentLocation.Latitude;
                    Parcel.Drone.CurrentLocation.Longitude = blDrone.CurrentLocation.Longitude;
                }
                catch (ObjectNotFoundException)
                {

                }
                catch (ObjectIsNotActiveException)
                {
                    Parcel.Drone.Id = 0;
                }

            }

            Parcel.Requested = dalParcel.Requested;
            Parcel.Scheduled = dalParcel.Scheduled;
            Parcel.PickedUp = dalParcel.PickedUp;
            Parcel.Delivered = dalParcel.Delivered;

            return Parcel;
        }
        #endregion

        #region View

        /// <summary>
        /// View list of parcelToList detailes.
        /// </summary>
        /// <returns> List of parcelToList detailes </returns>
        public IEnumerable<ParcelToList> ViewParcelToList()
        {
            List<ParcelToList> ParcelList = new();

            foreach (var parcel in dalObject.GetParcelList())
            {
                DO.Parcel dalParcel = dalObject.FindParcelById(parcel.Id);
                ParcelToList Parcel = new();
                DO.Customer dalCustomer = new();

                Parcel.Id = dalParcel.Id;

                dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
                Parcel.SenderName = dalCustomer.Name;

                dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
                Parcel.ReceiverName = dalCustomer.Name;

                Parcel.WeightCategory = (WeightCategories)dalParcel.Weight;
                Parcel.Priority = (Priorities)dalParcel.Priority;

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
                DO.Parcel dalParcel = dalObject.FindParcelById(parcel.Id);
                ParcelToList blParcel = new();
                DO.Customer dalCustomer = new();

                blParcel.Id = dalParcel.Id;

                dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
                blParcel.SenderName = dalCustomer.Name;

                dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
                blParcel.ReceiverName = dalCustomer.Name;

                blParcel.WeightCategory = (WeightCategories)dalParcel.Weight;
                blParcel.Priority = (Priorities)dalParcel.Priority;

                blParcel.ParcelStatus = ParcelStatus.Requested;

                ParcelToList.Add(blParcel);
            }

            return ParcelToList;
        }
        public IEnumerable<Parcel> ViewParcelsList()
        {
            var parcelList = from parcel in ViewParcelToList() select FindParcelByIdBL(parcel.Id);
            return parcelList;
        }

        public ParcelToList FindParcelToList(int parcelId)
        {
            DO.Parcel dalParcel = dalObject.FindParcelById(parcelId);
            ParcelToList parcel = new();
            DO.Customer dalCustomer = new();

            parcel.Id = dalParcel.Id;

            dalCustomer = dalObject.FindCustomerById(dalParcel.SenderId);
            parcel.SenderName = dalCustomer.Name;

            dalCustomer = dalObject.FindCustomerById(dalParcel.TargetId);
            parcel.ReceiverName = dalCustomer.Name;

            parcel.WeightCategory = (WeightCategories)dalParcel.Weight;
            parcel.Priority = (Priorities)dalParcel.Priority;

            if (dalParcel.Delivered != null)
            {
                parcel.ParcelStatus = ParcelStatus.Delivered;
            }
            else if (dalParcel.PickedUp != null)
            {
                parcel.ParcelStatus = ParcelStatus.PickedUp;
            }
            else if (dalParcel.Scheduled != null)
            {
                parcel.ParcelStatus = ParcelStatus.Scheduled;
            }
            else
            {
                parcel.ParcelStatus = ParcelStatus.Requested;
            }
            return parcel;
        }

        public void DeleteParcel(int parcelId)
        {
            try
            {
                DO.Parcel parcel = dalObject.FindParcelById(parcelId);
                if (parcel.Scheduled == null)
                {
                    dalObject.DeleteParcel(parcelId);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            } catch(DO.ObjectNotFoundException e)
            {
                throw new ObjectIsNotActiveException(e.Message);
            }
        }
        #endregion
    }
}
