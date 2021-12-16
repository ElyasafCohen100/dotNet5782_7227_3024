using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDAL.IDal
    {
        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Finds Parcel by specific Id.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        /// <returns> Parcel object </returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public Parcel FindParcelById(int parcelId)
        {
            Parcel parcel = DataSource.Parcels.Find(x => x.Id == parcelId);
            return parcel.Id != parcelId ? throw new ObjectNotFoundException("parcel") : parcel;
        }

        //------------------------- SETTERS ---------------------------//

        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        public void SetNewParcel(Parcel Parcel)
        {
            Parcel.Id = DataSource.Config.SerialNum;
            DataSource.Parcels.Add(Parcel);
            ++DataSource.Config.SerialNum;
        }


        //---------------------- UPDATE FUNCTIONS ----------------------//

        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Parcel Id </param>
        public void UpdatePickedUpParcelById(int parcelId)
        {
            try
            {
                int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
                if (index == -1) throw new ObjectNotFoundException("parcel");

                Parcel parcel = DataSource.Parcels[index];
                parcel.PickedUp = DateTime.Now;
                DataSource.Parcels[index] = parcel;

            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId"> Parcel Id</param>
        public void UpdateDeliveredParcelById(int parcelId)
        {
            try
            {
                int index = DataSource.Parcels.FindIndex(x => x.Id == parcelId);
                if (index == -1) throw new ObjectNotFoundException("parcel");

                Parcel parcel = DataSource.Parcels[index];
                parcel.Delivered = DateTime.Now;
                DataSource.Parcels[index] = parcel;
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
        }


        //--------------------------- GETTERS ---------------------------//

        /// <summary>
        /// Return List of Parcels.
        /// </summary>
        /// <returns>List of Parcels </returns>
        public IEnumerable<Parcel> GetParcelList()
        {
            return DataSource.Parcels;
        }

        /// <summary>
        /// Return List of non associate Parcels.
        /// </summary>
        /// <returns> List of non associate Parcels </returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> predicate)
        {
            return DataSource.Parcels.FindAll(predicate);
        }
    }
}