using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using DO;

namespace Dal
{
    partial class DalObject : DalApi.IDal
    {

        #region Update
        [MethodImpl(MethodImplOptions.Synchronized)]
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


        #region Add

        /// <summary>
        /// Set new Station.
        /// </summary>
        /// <param name="station"> Station object </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewStation(Station station)
        {
            station.IsActive = true;
            DataSource.Stations.Add(station);
        }
        #endregion


        #region Delete
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            int index = DataSource.Stations.FindIndex(x => x.Id == stationId);
            if (index == -1) throw new ObjectNotFoundException("station");
            Station station = DataSource.Stations[index];
            station.IsActive = false;
            DataSource.Stations[index] = station;
        }
        #endregion


        #region Getters

        /// <summary>
        /// Finds Station by specific Id.
        /// </summary>
        /// <param name="stationId"> Station Id </param>
        /// <returns> Station object </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStationById(int stationId)
        {
            Station station = DataSource.Stations.Find(x => x.Id == stationId);
            return station.Id != stationId ? throw new ObjectNotFoundException("station") : station;
        }

        /// <summary>
        /// Return list of Stations.
        /// </summary>
        /// <returns> List of Stations </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetBaseStationList()
        {
            return from station in DataSource.Stations where station.IsActive select station;
        }

        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            return DataSource.Stations.FindAll(predicate).FindAll(x => x.IsActive);
        }

        #endregion
    }
}