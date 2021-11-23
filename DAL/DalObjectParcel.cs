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
        /// <param name="parcelId"> Id of Parcel </param>
        /// <returns> Parcel </returns>
        public Parcel FindParcelById(int parcelId)
        {
            Parcel parcel = DataSource.Parcels.Find(x => x.Id == parcelId);
            if (parcel.Id != parcelId) throw new RequiredObjectIsNotFoundException();
            return parcel;
        }

        //------------------------- SETTERS ---------------------------//

        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        public void SetNewParcel(Parcel Parcel)
        {
            DataSource.Parcels.Add(Parcel);
        }


        //---------------------- UPDATE FUNCTIONS ----------------------//

        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        public void UpdatePickedUpParcelById(int parcelId)
        {
            Parcel myParcel = FindParcelById(parcelId);
            myParcel.PickedUp = DateTime.Now;
        }

        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void UpdateDeliveredParcelById(int parcelId)
        {
            Parcel myParcel = FindParcelById(parcelId);
            myParcel.Delivered = DateTime.Now;
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
        public IEnumerable<Parcel> GetNonAssociateParcelList()
        {
           return DataSource.Parcels.FindAll(x => x.DroneId == 0);
        }

        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        public IEnumerable<Station> GetStationsWithAvailableChargingSlots()
        {
            return DataSource.Stations.FindAll(x => x.ChargeSlots > 0);
        }
    }
}