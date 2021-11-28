using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DalObject;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        //---------------------------------- ADD FUNCTIONS ----------------------------------------//

        /// <summary>
        /// add new BL station by using DAL 
        /// </summary>
        /// <param name="station"> the station </param>
        /// <exception cref="InvalidInputException">Thrown if station id or number of charge-slots are invalid</exception>
        public void AddNewStationBL(Station station)
        {
            if (station.Id < 1000 || station.Id >= 10000) throw new InvalidInputException("id");
            IfExistBaseStation(station);
            if (station.AvailableChargeSlots < 0) throw new InvalidInputException("number of charge slots");

            IDAL.DO.Station dalStation = new();

            dalStation.Id = station.Id;
            dalStation.Name = station.Name;
            dalStation.Latitude = station.Location.Latitude;
            dalStation.Longitude = station.Location.Longitude;
            dalStation.ChargeSlots = station.AvailableChargeSlots;

            dalObject.SetNewStation(dalStation);
        }

        /// <summary>
        /// Check if the base-station has already exist
        /// </summary>
        /// <param name="station">The base-station</param>
        /// <exception cref="ObjectAlreadyExistException">If the id or the name has already exist</exception>
        static void IfExistBaseStation(Station station)
        {
            foreach (var myStation in dalObject.GetBaseStationList())
            {
                if (myStation.Id == station.Id) throw new ObjectAlreadyExistException("base-station Id");
                if (myStation.Name == station.Name) throw new ObjectAlreadyExistException("base-station Name");
            }
        }
        //--------------------------------- FIND FUNCTIONS ---------------------------------------//

        /// <summary>
        /// find BL station by station ID
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns> the station </returns>
        /// <exception cref="InvalidInputException">Thrown if station id is invalid</exception>
        public Station FindStationByIdBL(int stationId)
        {
            if (stationId < 1000 || stationId >= 10000) throw new InvalidInputException("id");

            IDAL.DO.Station dalStation = dalObject.FindStationById(stationId);
            Station Station = new();
            int numOfUnAvailableChargeSlots = 0;

            Station.Id = dalStation.Id;
            Station.Name = dalStation.Name;
            Station.Location.Longitude = dalStation.Longitude;
            Station.Location.Longitude = dalStation.Latitude;


            foreach (var droneCharge in dalObject.GetDroneChargeListByStationId(stationId))
            {
                DroneCharge DroneCharge = new();
                Drone Drone = new();

                DroneCharge.DroneId = droneCharge.DroneId;

                Drone = FindDroneByIdBL(DroneCharge.DroneId);
                DroneCharge.BatteryStatus = Drone.BatteryStatus;

                Station.DroneChargesList.Add(DroneCharge);

                ++numOfUnAvailableChargeSlots;
            }
            Station.AvailableChargeSlots = dalStation.ChargeSlots - numOfUnAvailableChargeSlots;

            return Station;
        }

        //--------------------------------- UPDATE FUNCTIONS --------------------------------------//

        /// <summary>
        /// update the dateils of base station
        /// </summary>
        /// <param name="baseStationId"> the ID of the base station </param>
        /// <param name="baseStationNewName"> the new name of the base station </param>
        /// <param name="baseStationChargeSlots"> how many chargen slots the base station has </param>
        /// <exception cref="InvalidInputException">Thrown if station name or number of charge slots are invalid</exception>
        public void UpdateBaseStationDetails(int baseStationId, string baseStationNewName, int baseStationChargeSlots)
        {
            if (baseStationNewName == "") throw new InvalidInputException("name"); 
            if (baseStationChargeSlots < 0) throw new InvalidInputException("number of charge slots"); 

            IDAL.DO.Station dalBaseStation = dalObject.FindStationById(baseStationId);
            dalBaseStation.Name = baseStationNewName;
            dalBaseStation.ChargeSlots = baseStationChargeSlots;
        }

        //---------------------------------- VIEW FUNCTIONS ---------------------------------------//

        /// <summary>
        /// view list of BL "StationToList"
        /// </summary>
        /// <returns> list of BL "StationToList" </returns>
        public IEnumerable<StationToList> ViewBaseStationsToList()
        {
            List<StationToList> stationToList = new();

            foreach (var baseStation in dalObject.GetBaseStationList())
            {
                StationToList station = new();

                station.Id = baseStation.Id;
                station.Name = baseStation.Name;

                station.NotAvailableChargeSlots = dalObject.GetDroneChargeListByStationId(baseStation.Id).Count();
                station.AvailableChargeSlots = baseStation.ChargeSlots - station.NotAvailableChargeSlots;

                stationToList.Add(station);
            }

            return stationToList;
        }

        /// <summary>
        /// view stations with available charging slots 
        /// </summary>
        /// <returns> list of stations with available charging slots </returns>
        public IEnumerable<StationToList> ViewStationsWithAvailableChargingSlotstBL()
        {
            List<StationToList> stationList = ViewBaseStationsToList().ToList();
            List<StationToList> stationWithAvailableChargingSlotstList = new();

            foreach (var baseStation in dalObject.GetStationsWithAvailableChargingSlots())
            {
                StationToList stationToList = stationList.Find(x => x.Id == baseStation.Id);

                if (stationToList != null)
                {
                    stationWithAvailableChargingSlotstList.Add(stationToList);
                }
            }

            return stationWithAvailableChargingSlotstList;
        }
    }
}