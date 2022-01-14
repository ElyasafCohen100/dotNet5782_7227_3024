using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        #region Get
        /// <summary>
        /// Find BL parcel by Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> BL parcel object </returns>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcelByIdBL(int parcelId)
        {
            if (parcelId <= 0) throw new InvalidInputException("Id");
            DO.Parcel dalParcel;
            lock (dalObject)
            {
                try
                {
                    dalParcel = dalObject.GetParcelById(parcelId);
                }
                catch (DO.ObjectNotFoundException e)
                {
                    throw new ObjectNotFoundException(e.Message);
                }
                Parcel Parcel = new();

                Parcel.Id = dalParcel.Id;

                DO.Customer dalCustomer = new();
                lock (dalObject)
                {
                    dalCustomer = dalObject.GetCustomerById(dalParcel.SenderId);
                    Parcel.senderCustomer.Id = dalParcel.SenderId;
                    Parcel.senderCustomer.Name = dalCustomer.Name;

                    dalCustomer = dalObject.GetCustomerById(dalParcel.TargetId);
                    Parcel.receiverCustomer.Id = dalParcel.TargetId;
                    Parcel.receiverCustomer.Name = dalCustomer.Name;

                    Parcel.Weight = (WeightCategories)dalParcel.Weight;
                    Parcel.Priority = (Priorities)dalParcel.Priority;

                    if (dalParcel.DroneId != 0)
                    {
                        try
                        {
                            Drone blDrone = new();
                            blDrone = GetDroneByIdBL(dalParcel.DroneId);
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
                }

                Parcel.Requested = dalParcel.Requested;
                Parcel.Scheduled = dalParcel.Scheduled;
                Parcel.PickedUp = dalParcel.PickedUp;
                Parcel.Delivered = dalParcel.Delivered;

                return Parcel;
            }
        }


        /// <summary>
        /// View list of parcelToList detailes.
        /// </summary>
        /// <returns> List of parcelToList detailes </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetAllParcelToList()
        {
            List<ParcelToList> ParcelList = new();

            foreach (var parcel in dalObject.GetParcelList())
            {
                lock (dalObject)
                {
                    DO.Parcel dalParcel = dalObject.GetParcelById(parcel.Id);
                    ParcelToList Parcel = new();
                    DO.Customer dalCustomer = new();

                    Parcel.Id = dalParcel.Id;
                    dalCustomer = dalObject.GetCustomerById(dalParcel.SenderId);
                    Parcel.SenderName = dalCustomer.Name;

                    dalCustomer = dalObject.GetCustomerById(dalParcel.TargetId);
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

            }
            return ParcelList;
        }


        /// <summary>
        /// View list of non associate ParcelsList. 
        /// </summary>
        /// <returns> List of non associate ParcelsList </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetNonAssociateParcelsListBL()
        {
            List<ParcelToList> ParcelToList = new();
            lock (dalObject)
            {
                foreach (var parcel in dalObject.GetParcels(x => x.DroneId == 0))
                {
                    DO.Parcel dalParcel = dalObject.GetParcelById(parcel.Id);
                    ParcelToList blParcel = new();
                    DO.Customer dalCustomer = new();

                    blParcel.Id = dalParcel.Id;

                    dalCustomer = dalObject.GetCustomerById(dalParcel.SenderId);
                    blParcel.SenderName = dalCustomer.Name;

                    dalCustomer = dalObject.GetCustomerById(dalParcel.TargetId);
                    blParcel.ReceiverName = dalCustomer.Name;

                    blParcel.WeightCategory = (WeightCategories)dalParcel.Weight;
                    blParcel.Priority = (Priorities)dalParcel.Priority;

                    blParcel.ParcelStatus = ParcelStatus.Requested;

                    ParcelToList.Add(blParcel);
                }
            }
            return ParcelToList;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetAllParcels()
        {
            var parcelList = from parcel in GetAllParcelToList() select GetParcelByIdBL(parcel.Id);
            return parcelList;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelToList GetParcelToList(int parcelId)
        {
            lock (dalObject)
            {
                DO.Parcel dalParcel = dalObject.GetParcelById(parcelId);
                ParcelToList parcel = new();
                DO.Customer dalCustomer = new();

                parcel.Id = dalParcel.Id;

                dalCustomer = dalObject.GetCustomerById(dalParcel.SenderId);
                parcel.SenderName = dalCustomer.Name;

                dalCustomer = dalObject.GetCustomerById(dalParcel.TargetId);
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
        }
        #endregion
       
        
        #region Add
        /// <summary>
        /// Add new BL parcel to the list by using DAL.
        /// </summary>
        /// <param name="parcel"> Parcel object /param>
        /// <exception cref="InvalidInputException"> Thrown if parcel details are invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
            lock (dalObject)
            {
                dalObject.AddNewParcel(dalParcel);
            }
        }
        #endregion


        #region Update
        /// <summary>
        /// Update drone Id of parcel.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <exception cref="InvalidInputException"> Thrown if drone id is invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssociateDroneTofParcelBL(int droneId)
        {
            if (droneId < 1000 || droneId >= 10000) throw new InvalidInputException("Id");

            DroneToList droneToList = droneToLists.Find(x => x.Id == droneId);

            if (droneToList.DroneStatus == DroneStatuses.Available)
            {
                lock (dalObject)
                {
                    IEnumerable<DO.Parcel> parcels = dalObject.GetParcels(x => x.DroneId == 0);
                    if (parcels.Any())
                    {
                        //Initiate some valuse for comparation
                        DO.Parcel dalParcel = new();
                        dalParcel.Priority = DO.Priorities.Regular;
                        dalParcel.Weight = DO.WeightCategories.Heavy;

                        double minDistance = double.MaxValue;

                        Customer sender = new();
                        Customer target = new();

                        bool haveParcelToAssociate = false;
                        foreach (var parcel in parcels)
                        {

                            if ((int)droneToList.MaxWeight <= (int)parcel.Weight)
                            {
                                sender = GetCustomerByIdBL(parcel.SenderId);
                                target = GetCustomerByIdBL(parcel.TargetId);

                                //Distance between drone and sender.
                                double distance = dalObject.Distance(droneToList.CurrentLocation.Latitude,
                                                                          sender.Location.Latitude,
                                                                          droneToList.CurrentLocation.Longitude,
                                                                          sender.Location.Longitude);

                                //Distance between sender and target.
                                distance += dalObject.Distance(sender.Location.Latitude,
                                                                          target.Location.Latitude,
                                                                         sender.Location.Longitude,
                                                                          target.Location.Longitude);

                                //Find the nearest base-station to the target.
                                int stationId = FindNearestBaseStationWithAvailableChargingSlots(new Location
                                {
                                    Latitude = target.Location.Latitude,
                                    Longitude = target.Location.Longitude
                                });

                                Station station = GetStationByIdBL(stationId);

                                //Distance between target and base-station.
                                distance += dalObject.Distance(target.Location.Latitude, station.Location.Latitude,
                                                            target.Location.Longitude, station.Location.Longitude);

                                if (droneToList.BatteryStatus > FindMinSuplyForAllPath(droneToList.Id, parcel.SenderId, parcel.TargetId))
                                {
                                    if ((int)parcel.Priority > (int)dalParcel.Priority)
                                    {
                                        dalParcel = parcel;
                                    }
                                    else if (((int)parcel.Weight < (int)dalParcel.Weight) && ((int)parcel.Priority == (int)dalParcel.Priority))
                                    {
                                        dalParcel = parcel;
                                    }
                                    else if ((distance < minDistance) && ((int)parcel.Weight == (int)dalParcel.Weight) && ((int)parcel.Priority == (int)dalParcel.Priority))
                                    {
                                        dalParcel = parcel;
                                    }
                                    minDistance = distance;
                                    haveParcelToAssociate = true;
                                }
                            }
                        }
                        if (haveParcelToAssociate)
                        {
                            droneToList.DroneStatus = DroneStatuses.Shipment;
                            droneToList.DeliveryParcelId = dalParcel.Id;

                            try
                            {
                                dalObject.UpdateDroneIdOfParcel(dalParcel.Id, droneToList.Id);
                            }
                            catch (DO.ObjectNotFoundException e)
                            {
                                throw new ObjectNotFoundException(e.Message);
                            }
                        }
                    }
                    else
                    {
                        throw new ObjectNotFoundException("parcel");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Could not send this drone");
            }
        }
        #endregion


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int parcelId)
        {
            try
            {
                lock (dalObject)
                {
                    DO.Parcel parcel = dalObject.GetParcelById(parcelId);
                    if (parcel.Scheduled == null)
                    {
                        dalObject.DeleteParcel(parcelId);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }
        }
        #endregion
    }
}
