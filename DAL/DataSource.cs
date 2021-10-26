using System;
using System.Collections.Generic;
using IDAL.DO;

namespace DalObject
{
    /// <summary>
    /// Contain the data source
    /// </summary>
    public class DataSource
    {
        internal static Drone[] Drones = new Drone[10];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static Parcel[] Parcels = new Parcel[1000];
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        /// <summary>
        /// Inner class.
        /// Contain all the configuring values 
        /// </summary>
        internal class Config
        {
            internal static int DroneIndex = 0;
            internal static int StationIndex = 0;
            internal static int CustomerIndex = 0;
            internal static int ParcelIndex = 0;
            internal static int SerialNumber = 0;

            internal static readonly Random randomNumber = new();
            internal static readonly DateTime currentDate = DateTime.Now;
        }

        /// <summary>
        /// Initialize the arryas.
        /// </summary>
        internal static void Initialize()
        {
            for (int i = 0; i < 2; i++)
            {
                Stations[i].Id = Config.randomNumber.Next(1000, 10000);
                Stations[i].Name = "Station" + i;
                Stations[i].Longitude = 35 + Config.randomNumber.NextDouble();
                Stations[i].Lattitude = 31 + Config.randomNumber.NextDouble();
                Stations[i].ChargeSlots = Config.randomNumber.Next(0, 2);
                Config.StationIndex++;
            }

            for (int i = 0; i < 5; i++)
            {
                Drones[i].Id = Config.randomNumber.Next(1000, 10000);
                Drones[i].Model = "V" + i;
                Drones[i].Battery = Config.randomNumber.Next(0, 1001) / Config.randomNumber.Next(1, 11);
                Drones[i].MaxWeight = (WeightCategiries)Config.randomNumber.Next(2);
                Config.DroneIndex++;
            }

            Drones[0].Status = DroneStatuses.Available;
            Drones[1].Status = DroneStatuses.Maintenance;
            Drones[2].Status = DroneStatuses.Shipment;
            Drones[3].Status = DroneStatuses.Available;
            Drones[4].Status = DroneStatuses.Maintenance;

            for (int i = 0; i <= 10; i++)
            {
                Customers[i].Id = Config.randomNumber.Next(1000, 10000);
                Customers[i].Name = "Name" + i;
                Customers[i].Phone = "050123456" + i;
                Customers[i].Longitude = 35 + Config.randomNumber.NextDouble();
                Customers[i].Lattitude = 31 + Config.randomNumber.NextDouble();
                Config.CustomerIndex++;
            }

            for (int i = 0; i < 10; i++)
            {
                Parcels[i].Id = 1 + i;
                Parcels[i].SenderId = Customers[i].Id;
                Parcels[i].TargetId = Customers[i + 1].Id;
                Parcels[i].Weight = (WeightCategiries)Config.randomNumber.Next(2);
                Parcels[i].Priority = (Priorities)Config.randomNumber.Next(2);
                Parcels[i].Requested = Config.currentDate;
                Config.SerialNumber++;
                Config.ParcelIndex++;
            }

            Parcels[0].Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(2, 10));
            Parcels[0].PickedUp = Config.currentDate.AddMinutes(Config.randomNumber.Next(15, 30));
            Parcels[0].Delivered = Config.currentDate.AddMinutes(Config.randomNumber.Next(20, 40));
            Parcels[0].DroneId = Drones[0].Id;

            Parcels[1].Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(2, 5));
            Parcels[1].PickedUp = Config.currentDate.AddMinutes(Config.randomNumber.Next(15, 20));
            Parcels[1].Delivered = Config.currentDate.AddMinutes(Config.randomNumber.Next(10, 28));
            Parcels[1].DroneId = Drones[3].Id;

            Parcels[2].Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(5, 7));
            Parcels[2].DroneId = Drones[2].Id;
        }
    }
}
