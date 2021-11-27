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
            internal static double Light = 1;
            internal static double Average= 1.5;
            internal static double Heavy = 2;
            internal static double Available = 0.5;
            internal static double DroneChargingRate = 5;

            internal static int SerialNumber = 1;
            internal static readonly Random randomNumber = new();
            internal static readonly DateTime currentDate = DateTime.Now;
        }

        /// <summary>
        /// Initialize the arryas.
        /// </summary>
        internal static void Initialize()
        {

            //Initiate stations
            for (int i = 0; i < 2; i++)
            {
                Station myStation = new();

                myStation.Id = Config.randomNumber.Next(1000, 10000);
                myStation.Name = "Station" + i;
                myStation.Longitude = 35 + Config.randomNumber.NextDouble();
                myStation.Latitude = 31 + Config.randomNumber.NextDouble();
                myStation.ChargeSlots = Config.randomNumber.Next(5, 11);

                Stations.Add(myStation);
            }

            // Initiate drones
            for (int i = 0; i < 5; i++)
            {
                Drone myDrone = new();

                myDrone.Id = Config.randomNumber.Next(1000, 10000);
                myDrone.Model = "V" + i;
                myDrone.MaxWeight = (WeightCategories)Config.randomNumber.Next(2);

                Drones.Add(myDrone);
            }

            // Initiate customers
            for (int i = 0; i <= 10; i++)
            {
                Customer myCustumer = new();

                myCustumer.Id = Config.randomNumber.Next(100000000, 1000000000);
                myCustumer.Name = "Name" + i;
                myCustumer.Phone = "0501234" + Config.randomNumber.Next(111, 1000);
                myCustumer.Longitude = 35 + Config.randomNumber.NextDouble();
                myCustumer.Lattitude = 31 + Config.randomNumber.NextDouble();

                Customers.Add(myCustumer);
            }

            // Initiate parcels
            for (int i = 0; i < 10; i++)
            {
                Parcel myParcel = new Parcel();

                myParcel.Id = Config.SerialNumber;
                myParcel.SenderId = Customers[i].Id;
                myParcel.TargetId = Customers[i + 1].Id;
                myParcel.Weight = (WeightCategories)Config.randomNumber.Next(2);
                myParcel.Priority = (Priorities)Config.randomNumber.Next(2);
                myParcel.Requested = Config.currentDate;


                // Make sure that there have at least one parcel in each statusS
                switch (i)
                {
                    case 0:
                        myParcel.Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(2, 10));
                        myParcel.PickedUp = Config.currentDate.AddMinutes(Config.randomNumber.Next(15, 30));
                        myParcel.Delivered = Config.currentDate.AddMinutes(Config.randomNumber.Next(20, 40));
                        myParcel.DroneId = Drones[0].Id;
                        break;
                    case 1:
                        myParcel.Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(2, 5));
                        myParcel.PickedUp = Config.currentDate.AddMinutes(Config.randomNumber.Next(15, 20));
                        myParcel.Delivered = Config.currentDate.AddMinutes(Config.randomNumber.Next(10, 28));
                        myParcel.DroneId = Drones[3].Id;
                        break;
                    case 2:
                        myParcel.Scheduled = Config.currentDate.AddMinutes(Config.randomNumber.Next(5, 7));
                        myParcel.DroneId = Drones[2].Id;
                        break;
                }

                Parcels.Add(myParcel);

                Config.SerialNumber++;
            }
        }
    }
}


