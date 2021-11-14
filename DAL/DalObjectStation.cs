﻿using System;
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
        /// Finds Station by specific Id.
        /// </summary>
        /// <param name="stationId"> Id of Station </param>
        /// <returns>Station object</returns>
        public Station FindStationById(int stationId)
        {
            foreach (var myStation in DataSource.Stations)
            {
                if (myStation.Id == stationId)
                    return myStation;
            }

            return new Station();
        }

        //------------------------- SETTERS --------------------------//

        /// <summary>
        /// Set new Station.
        /// </summary>
        /// <param name="station">Station object</param>
        public void SetNewStation(Station station)
        {
            DataSource.Stations.Add(station);
        }

        //-------------------------- GETTERS -------------------------//
        /// <summary>
        /// Return list of Stations.
        /// </summary>
        /// <returns>List of Stations</returns>
        public IEnumerable<Station> GetBaseStationList()
        {
            return DataSource.Stations;
        }
    }
}