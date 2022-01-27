using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
using Dal;

namespace BL
{
    public partial class BL : BlApi.IBL
    {
        #region Get
        /// <summary>
        /// Find BL station by station Id.
        /// </summary>
        /// <param name="stationId"> Station Id </param>
        /// <returns> Station object </returns>
        /// <exception cref="InvalidInputException"> Thrown if station id is invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStationByIdBL(int stationId)
        {
            if (stationId < 1000 || stationId >= 10000) throw new InvalidInputException("id");

            DO.Station dalStation;
            try
            {
                dalStation = dalObject.GetStationById(stationId);
            }
            catch (DO.ObjectNotFoundException e)
            {
                throw new ObjectNotFoundException(e.Message);
            }

            Station Station = new();
            int numOfUnAvailableChargeSlots = 0;

            Station.Id = dalStation.Id;
            Station.Name = dalStation.Name;
            Station.Location.Longitude = dalStation.Longitude;
            Station.Location.Latitude = dalStation.Latitude;


            foreach (var droneCharge in dalObject.GetDroneChargeList(x => x.StationId == stationId))
            {
                DroneCharge DroneCharge = new();
                Drone Drone = new();

                DroneCharge.DroneId = droneCharge.DroneId;
                DroneCharge.ChargeTime = droneCharge.ChargeTime;

                Drone = GetDroneByIdBL(DroneCharge.DroneId);
                DroneCharge.BatteryStatus = Drone.BatteryStatus;

                Station.DroneChargesList.Add(DroneCharge);

                ++numOfUnAvailableChargeSlots;
            }
            Station.AvailableChargeSlots = dalStation.ChargeSlots - numOfUnAvailableChargeSlots;

            return Station;
        }


        /// <summary>
        /// View list of BL StationToList.
        /// </summary>
        /// <returns> List of BL StationToList </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAllBaseStationsToList()
        {
            List<StationToList> stationToList = new();
            foreach (var baseStation in dalObject.GetBaseStationList())
            {
                StationToList station = new();

                station.Id = baseStation.Id;
                station.Name = baseStation.Name;

                station.NotAvailableChargeSlots = dalObject.GetDroneChargeList(x => x.StationId == baseStation.Id).Count();

                station.AvailableChargeSlots = baseStation.ChargeSlots - station.NotAvailableChargeSlots;

                stationToList.Add(station);
            }
            return stationToList;
        }


        /// <summary>
        /// View stations with available charging slots. 
        /// </summary>
        /// <returns> List of stations with available charging slots </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStationsWithAvailableChargingSlotstBL()
        {
            List<StationToList> stationList = GetAllBaseStationsToList().ToList();
            List<StationToList> stationWithAvailableChargingSlotstList = new();

            foreach (var baseStation in dalObject.GetStations(x => x.ChargeSlots > 0))
            {
                StationToList stationToList = stationList.Find(x => x.Id == baseStation.Id);

                if (stationToList != null)
                {
                    stationWithAvailableChargingSlotstList.Add(stationToList);
                }
            }
            return stationWithAvailableChargingSlotstList;
        }
        #endregion


        #region Add
        /// <summary>
        /// Add new BL station by using DAL.
        /// </summary>
        /// <param name="station"> Station object </param>
        /// <exception cref="InvalidInputException"> Thrown if station id or number of charge-slots are invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewStationBL(Station station)
        {
            if (station.Id < 1000 || station.Id >= 10000) throw new InvalidInputException("id");
            IfExistBaseStation(station); //Checks if the customer already exists.
            if (station.AvailableChargeSlots < 0) throw new InvalidInputException("number of charge slots");

            DO.Station dalStation = new();

            dalStation.Id = station.Id;
            dalStation.Name = station.Name;
            dalStation.Latitude = station.Location.Latitude;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.ChargeSlots = station.AvailableChargeSlots;
            lock (dalObject)
            {
                dalObject.AddNewStation(dalStation);
            }
        }


        /// <summary>
        /// Check if the base-station has already exist.
        /// </summary>
        /// <param name="station"> Station object </param>
        /// <exception cref="ObjectAlreadyExistException"> If the id or the name has already exist </exception>
        static void IfExistBaseStation(Station station)
        {
            foreach (var myStation in dalObject.GetBaseStationList())
            {
                if (myStation.Id == station.Id) throw new ObjectAlreadyExistException("base-station Id");
                if (myStation.Name == station.Name) throw new ObjectAlreadyExistException("base-station Name");
            }
        }
        #endregion


        #region Update
        /// <summary>
        /// Update the dateils of base station.
        /// </summary>
        /// <param name="baseStationId"> Station Id </param>
        /// <param name="baseStationNewName"> New name of the base station </param>
        /// <param name="baseStationChargeSlots"> Number of charge slots that the base station has </param>
        /// <exception cref="InvalidInputException"> Thrown if station name or number of charge slots are invalid </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStationDetailsBL(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            if (baseStationNewName == "") throw new InvalidInputException("name");
            if (baseStationChargeSlots < 0 || baseStationChargeSlots < dalObject.GetDroneChargeList(x => x.StationId == baseStationId).Count())
                throw new InvalidInputException("number of charge slots");

            try
            {
                lock (dalObject)
                {
                    dalObject.UpdateBaseStationDetails(baseStationId, baseStationNewName, baseStationChargeSlots);
                }
            }
            catch (ObjectNotFoundException)
            {
                throw new ObjectNotFoundException("station");
            }
        }
        #endregion


        #region Delete
        /// <summary>
        /// delete the station
        /// </summary>
        /// <param name="stationId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            try
            {
                lock (dalObject)
                {
                    dalObject.DeleteStation(stationId);
                }
            }
            catch (DO.ObjectIsNotActiveException e)
            {
                throw new ObjectIsNotActiveException(e.Message);
            }
        }
        #endregion
    }
}