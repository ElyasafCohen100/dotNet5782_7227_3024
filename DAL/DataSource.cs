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
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        /// <summary>
        /// Inner class.
        /// Contain all the configuring values 
        /// </summary>
        internal class Config
        {
            internal static double Light;
            internal static double Average;
            internal static double Heavy;
            internal static double Available;
            internal static double DroneChargingRate;

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
                Station myStation = new();

                myStation.Id = Config.randomNumber.Next(1000, 10000);
                myStation.Name = "Station" + i;
                myStation.Longitude = 35 + Config.randomNumber.NextDouble();
                myStation.Lattitude = 31 + Config.randomNumber.NextDouble();
                myStation.ChargeSlots = Config.randomNumber.Next(0, 2);

                Stations.Add(myStation);
            }

            for (int i = 0; i < 5; i++)
            {
                Drone myDrone = new();

                myDrone.Id = Config.randomNumber.Next(1000, 10000);
                myDrone.Model = "V" + i;
                myDrone.MaxWeight = (WeightCategiries)Config.randomNumber.Next(2);

                Drones.Add(myDrone);
            }

            for (int i = 0; i <= 10; i++)
            {
                Customer myCustumer = new();

                myCustumer.Id = Config.randomNumber.Next(1000, 10000);
                myCustumer.Name = "Name" + i;
                myCustumer.Phone = "050123456" + i;
                myCustumer.Longitude = 35 + Config.randomNumber.NextDouble();
                myCustumer.Lattitude = 31 + Config.randomNumber.NextDouble();

                Customers.Add(myCustumer);
            }

            for (int i = 0; i < 10; i++)
            {
                Parcel myParcel = new Parcel();

                myParcel.Id = 1 + i;
                myParcel.SenderId = Customers[i].Id;
                myParcel.TargetId = Customers[i + 1].Id;
                myParcel.Weight = (WeightCategiries)Config.randomNumber.Next(2);
                myParcel.Priority = (Priorities)Config.randomNumber.Next(2);
                myParcel.Requested = Config.currentDate;

                Parcels.Add(myParcel);

                Config.SerialNumber++;

                /**
                 * TODO: to figure out if those line nedded
                 * 
                 * Parcels[0].Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(2, 10));
                 * Parcels[0].PickedUp = Config.currentDate.AddMinutes(Config.randomNumber.Next(15, 30));
                 * Parcels[0].Delivered = Config.currentDate.AddMinutes(Config.randomNumber.Next(20, 40));
                 * Parcels[0].DroneId = Drones[0].Id;
                 * 
                 * Parcels[1].Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(2, 5));
                 * Parcels[1].PickedUp = Config.currentDate.AddMinutes(Config.randomNumber.Next(15, 20));
                 * Parcels[1].Delivered = Config.currentDate.AddMinutes(Config.randomNumber.Next(10, 28));
                 * Parcels[1].DroneId = Drones[3].Id;
                 * 
                 * Parcels[2].Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(5, 7));
                 * Parcels[2].DroneId = Drones[2].Id;
                 * */
            }
        }
    }
}


