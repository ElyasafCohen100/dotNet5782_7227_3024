using System;
using System.Collections.Generic;
using System.Linq;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {

        #region Find
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
        #endregion

        #region Update
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

        #region Setters

        /// <summary>
        /// Set new Station.
        /// </summary>
        /// <param name="station"> Station object </param>
        public void SetNewStation(Station station)
        {
            station.IsActive = true;
            DataSource.Stations.Add(station);
        }
        #endregion

        #region Getters
        /// <summary>
        /// Return list of Stations.
        /// </summary>
        /// <returns> List of Stations </returns>
        public IEnumerable<Station> GetBaseStationList()
        {
            return from station in DataSource.Stations where station.IsActive select station;
        }
        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            return DataSource.Stations.FindAll(predicate).FindAll(x => x.IsActive);
        }
        #endregion

        public void DeleteStation(int stationId)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == stationId);
            if (index == -1) throw new ObjectNotFoundException("station");
            Station station = DataSource.Stations[index];
            station.IsActive = false;
            DataSource.Stations[index] = station;
        }
    }
}