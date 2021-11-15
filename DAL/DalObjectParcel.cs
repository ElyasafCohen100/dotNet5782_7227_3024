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
            foreach (var parcel in DataSource.Parcels)
            {
                if (parcelId == parcel.Id)
                    return parcel;
            }

            return new Parcel();
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
            DateTime currentDate = DateTime.Now;
            Parcel myParcel = FindParcelById(parcelId);

            myParcel.PickedUp = currentDate;
        }

        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public void UpdateDeliveredParcelById(int parcelId)
        {
            DateTime currentDate = DateTime.Now;
            Parcel myParcel = FindParcelById(parcelId);

            myParcel.Delivered = currentDate;
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
            List<Parcel> MyParcels = new List<Parcel>();

            foreach (var parcel in DataSource.Parcels)
            {
                if (parcel.DroneId == 0)
                    MyParcels.Add(parcel);
            }
            return MyParcels;
        }

        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        public IEnumerable<Station> GetStationsWithAvailableChargingSlots()
        {
            List<Station> MyStations = new List<Station>();
            foreach (var station in DataSource.Stations)
            {
                if (station.ChargeSlots > 0)
                    MyStations.Add(station);
            }
            return MyStations;
        }
    }
}