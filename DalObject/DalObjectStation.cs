using System;
using System.Collections.Generic;
using DO;

namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        #region FIND
        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Finds Station by specific Id.
        /// </summary>
        /// <param name="stationId"> Station Id </param>
        /// <returns> Station object </returns>
        public Station FindStationById(int stationId)
        {
            Station station = DataSource.Stations.Find(x => x.Id == stationId);
            return station.Id != stationId ? throw new ObjectNotFoundException(station.GetType().ToString()) : station;
        }

        public void UpdateBaseStationDetails(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == baseStationId);
            if (index == -1) throw new ObjectNotFoundException("base station");

            Station station = DataSource.Stations[index];
            station.Name = baseStationNewName;
            station.ChargeSlots = baseStationChargeSlots;
            DataSource.Stations[index] = station;
        }
        #endregion

        #region SET
        //------------------------- SETTERS --------------------------//

        /// <summary>
        /// Set new Station.
        /// </summary>
        /// <param name="station"> Station object </param>
        public void SetNewStation(Station station)
        {
            DataSource.Stations.Add(station);
        }
        #endregion

        #region GET

        //-------------------------- GETTERS -------------------------//
        /// <summary>
        /// Return list of Stations.
        /// </summary>
        /// <returns> List of Stations </returns>
        public IEnumerable<Station> GetBaseStationList()
        {
            return DataSource.Stations;
        }
        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            return DataSource.Stations.FindAll(predicate);
        }

        #endregion
    }
}