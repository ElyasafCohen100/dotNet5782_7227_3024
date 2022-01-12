using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        #region Get
        /// <summary>
        /// Finds Station by specific Id.
        /// </summary>
        /// <param name="stationId"> Station Id </param>
        /// <returns> Station object </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStationById(int stationId)
        {
            List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

            Station station = stationList.Find(x => x.Id == stationId);
            return station.Id != stationId ? throw new ObjectNotFoundException(station.GetType().ToString()) : station;
        }


        /// <summary>
        /// Return list of Stations.
        /// </summary>
        /// <returns> List of Stations </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetBaseStationList()
        {
            List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

            return from station in stationList where station.IsActive select station;
        }


        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> predicate)
        {
            List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

            return stationList.FindAll(predicate).FindAll(x => x.IsActive);
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
            List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

            station.IsActive = true;
            stationList.Add(station);
            XMLTools.SaveListToXMLSerializer(stationList, dalStationPath);
        }
        #endregion


        #region Update
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStationDetails(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

            int index = stationList.FindIndex(x => x.Id == baseStationId);
            if (index == -1) throw new ObjectNotFoundException("base station");

            Station station = stationList[index];
            station.Name = baseStationNewName;
            station.ChargeSlots = baseStationChargeSlots;
            stationList[index] = station;
            XMLTools.SaveListToXMLSerializer(stationList, dalStationPath);
        }
        #endregion


        #region Delete 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            List<Station> stationList = XMLTools.LoadListFromXMLSerializer<Station>(dalStationPath);

            int index = stationList.FindIndex(x => x.Id == stationId);
            if (index == -1) throw new ObjectNotFoundException("station");
            Station station = stationList[index];
            station.IsActive = false;
            stationList[index] = station;
            XMLTools.SaveListToXMLSerializer(stationList, dalStationPath);
        }
        #endregion
    }
}
