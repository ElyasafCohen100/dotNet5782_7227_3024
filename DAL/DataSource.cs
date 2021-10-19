using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;



namespace DalObject
{
    class DataSource
    {
        internal static Drone[] Drones = new Drone[10];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static Parcel[] Parcels = new Parcel[1000];


        internal class Config
        {
            internal static int DroneIndex = 0;
            internal static int StationIndex = 0;
            internal static int CustomerIndex = 0;
            internal static int ParcelIndex = 0;

            internal static int SerialNumber;

            static Random r = new Random();
            static DateTime currentDate = DateTime.Now;

            internal static void Initialize()
            {

                for (int i = 0; i < 2; i++)
                {
                    Stations[i].Id = r.Next();
                    Stations[i].Name = r.Next();
                    Stations[i].Longitude = r.Next() / r.Next();
                    Stations[i].Lattitude = r.Next() / r.Next();
                }



                for (int i = 0; i < 5; i++)
                {
                    Drones[i].Id = r.Next();
                    Drones[i].Model = "V" + i;
                    Drones[0].MaxWeight = WeightCategiries.average;
                    Drones[0].Status = DroneStatuses.available;
                    Drones[i].Battery = r.Next();
                }

                for (int i = 0; i < 10; i++)
                {
                    Customers[i].Id = r.Next();
                    Customers[i].Name = "Name" + i;
                    Customers[i].Phone = "050123456" + i;
                    Customers[i].Longitude = r.Next() / r.Next();
                    Customers[i].Lattitude = r.Next() / r.Next();
                }


                for (int i = 0; i < 10; i++)
                {
                    Parcels[i].Id = r.Next();
                    Parcels[i].SenderId = r.Next();
                    Parcels[i].TargetId = r.Next();
                    Parcels[i].Weight = WeightCategiries.average;
                    Parcels[i].Priority = Priorities.Regular;
                    Parcels[i].Requested = currentDate;
                    Parcels[i].DroneId = r.Next();
                    Parcels[i].Scheduled = currentDate;
                    Parcels[i].PickedUp = currentDate;
                    Parcels[i].Delivered = currentDate;
                }

                SerialNumber = 100;
            }
        }
    }
}
