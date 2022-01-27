using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {

        #region Add
        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewParcel(Parcel parcel)
        {
            parcel.Id = DataSource.Config.SerialNum;
            parcel.IsActive = true;
            DataSource.Parcels.Add(parcel);
            ++DataSource.Config.SerialNum;
        }
        #endregion


        #region Update

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");
            Parcel parcel = DataSource.Parcels[index];
            parcel.DroneId = droneId;
            parcel.Scheduled = DateTime.Now;
            DataSource.Parcels[index] = parcel;
        }

        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePickedUpParcelById(int parcelId)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");

            Parcel parcel = DataSource.Parcels[index];
            parcel.PickedUp = DateTime.Now;
            DataSource.Parcels[index] = parcel;
        }

        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId"> Parcel Id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDeliveredParcelById(int parcelId)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");

            Parcel parcel = DataSource.Parcels[index];
            parcel.Delivered = DateTime.Now;
            DataSource.Parcels[index] = parcel;
        }
        #endregion


        #region Getters

        /// <summary>
        /// Finds Parcel by specific Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> Parcel object </returns>
        /// <exception cref="ObjectDisposedException"></exception
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcelById(int parcelId)
        {
            Parcel parcel = DataSource.Parcels.Find(x => x.Id == parcelId);
            return parcel.Id != parcelId && parcel.IsActive == false ? throw new ObjectNotFoundException("parcel") : parcel;
        }
        /// <summary>
        /// Return List of Parcels.
        /// </summary>
        /// <returns>List of Parcels </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelList()
        {
            return from parcel in DataSource.Parcels where parcel.IsActive select parcel;
        }

        /// <summary>
        /// Return List of non associate Parcels.
        /// </summary>
        /// <returns> List of non associate Parcels </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            return DataSource.Parcels.FindAll(predicate).FindAll(x => x.IsActive);
        }
        #endregion


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int parcelId)
        {
            int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
            if (index == -1) throw new ObjectNotFoundException("parcel");
            Parcel parcel = DataSource.Parcels[index];
            parcel.IsActive = false;
            DataSource.Parcels[index] = parcel;
        }
        #endregion
    }
}