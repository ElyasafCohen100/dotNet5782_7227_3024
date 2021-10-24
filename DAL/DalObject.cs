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
    }
}
