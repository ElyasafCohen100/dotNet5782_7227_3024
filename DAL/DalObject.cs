using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }



        //-------- SETTERS -------//
        public static void SetNewDrone(Drone drone)
        {
            DataSource.Drones[DataSource.Config.DroneIndex] = drone;
            DataSource.Config.DroneIndex++;
        }

        public static void SetNewStation(Station station)
        {
            DataSource.Stations[DataSource.Config.StationIndex] = station;
            DataSource.Config.StationIndex++;
        }

        public static void SetNewCustomer(Customer customer)
        {
            DataSource.Customers[DataSource.Config.CustomerIndex] = customer;
            DataSource.Config.CustomerIndex++;
        }

        public static void SetNewParcel(Parcel parcel)
        {
            DataSource.Parcels[DataSource.Config.ParcelIndex] = parcel;
            DataSource.Config.ParcelIndex++;
        }




        //--------------- UPDATE FUNCTIONS --------------//

        public static void UpdateDronIdOfParcel(int parcelId, int droneId)
        {
            int parcelIndex = 0;
            int droneIndex = 0;

            while (DataSource.Parcels[parcelIndex].Id != parcelId)
            {
                parcelIndex++;
            }

            DataSource.Parcels[parcelIndex].DroneId = droneId;

            while (DataSource.Drones[droneIndex].Id != droneId)
            {
                droneIndex++;
            }

            DataSource.Drones[droneIndex].Status = DroneStatuses.shipment;
        }



        //----------------------------------------------------------------------//



        public static void UpdatePickedUpParcelById(int parcelId)
        {
            int parcelIndex = 0;
            DateTime currentDate = DateTime.Now;

            while (DataSource.Parcels[parcelIndex].Id != parcelId)
            {
                parcelIndex++;
            }

            DataSource.Parcels[parcelIndex].PickedUp = currentDate;
        }



        //----------------------------------------------------------------------//



        public static void UpdateDeliveredParcelById(int parcelId)
        {
            int parcelIndex = 0;
            DateTime currentDate = DateTime.Now;


            while (DataSource.Parcels[parcelIndex].Id != parcelId)
            {
                parcelIndex++;
            }

            DataSource.Parcels[parcelIndex].Delivered = currentDate;
        }





        public static void UpdateDroneToCharging(int droneId, int stationId)
        {
            int droneIndex = 0;
            int stationIndex = 0;

            while (DataSource.Drones[droneIndex].Id != droneId)
            {
                droneIndex++;
            }
            DataSource.Drones[droneIndex].Status = DroneStatuses.maintenance;

            while (DataSource.Drones[stationIndex].Id != stationId)
            {
                stationIndex++;
            }
            DataSource.Stations[stationIndex].ChargeSlots -= 1;
        }




        public static List<Station> GetStationsWithAvailableChargingSlots()
        {
            List<Station> list = new List<Station>();
            int StationIndex = 0;

            while (StationIndex < DataSource.Config.StationIndex)
            {
                if (DataSource.Stations[StationIndex].ChargeSlots > 0)
                {
                    list.Add(DataSource.Stations[StationIndex]);
                }
                StationIndex++;
            }
            return list;
        }
    }
}
