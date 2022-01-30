using System;
using System.Collections.Generic;
using DO;


namespace Dal
{
    /// <summary>
    /// Contain the data source.
    /// </summary>
    public class DataSource
    {
        #region Lists
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<Admin> Admins = new List<Admin>();

        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        #endregion


        #region Config
        /// <summary>
        /// Inner class.
        /// Contain all the configuring values.
        /// </summary>
        internal class Config
        {
            internal static double Light = 3.4;
            internal static double Intermediate = 3.7;
            internal static double Heavy = 4.3;
            internal static double Available = 3.2;
            internal static double DroneChargingRate = 2;

            internal static int SerialNumber = 1;

            internal static Random r = new Random();
            internal static DateTime currentDate = DateTime.Now;

        }
        #endregion


        #region Initialize
        /// <summary>
        /// Initialize the arryas.
        /// </summary>
        internal static void Initialize()
        {
            Admins.Add(new Admin
            {
                UserName = "admin",
                Password = "admin"
            });

            Admins.Add(new Admin
            {
                UserName = "Elyasaf",
                Password = "Elyasaf"
            });

            for (int i = 0; i < 20; i++)
            {
                Stations.Add(new Station()
                {
                    Id = Config.r.Next(1000, 10000),
                    Name = "Station" + i,
                    Latitude = 31.7 + Config.r.NextDouble() / 5,
                    Longitude = 35.1 + Config.r.NextDouble() / 5,
                    ChargeSlots = Config.r.Next(5, 11),
                    IsActive = true
                });
            }
            for (int i = 0; i < 15; i++)
            {
                Drones.Add(new Drone()
                {
                    Id = Config.r.Next(1000, 10000),
                    Model = "A" + i,
                    MaxWeight = (WeightCategories)Config.r.Next(2),
                    IsActive = true
                });
            }

            for (int i = 0; i < 25; i++)
            {
                Customers.Add(new Customer()
                {
                    Id = Config.r.Next(100000000, 400000000),
                    Name = "Customer" + i,
                    Phone = "05" + Config.r.Next(10000000, 100000000).ToString(),
                    Latitude = 31.7 + Config.r.NextDouble() / 5,
                    Longitude = 35.1 + Config.r.NextDouble() / 5,
                    IsActive = true,
                    UserName = "Customer" + i,
                    Password = "Customer" + i
                });
            }

            for (int i = 0; i < 50; i++)
            {
                Parcel parcel = new Parcel();
                parcel.Id = Config.SerialNumber;
                parcel.SenderId = Customers[i % 25].Id;
                parcel.TargetId = Customers[(i + 1) % 25].Id;
                parcel.Weight = (WeightCategories)Config.r.Next(2);
                parcel.Priority = (Priorities)Config.r.Next(2);
                parcel.Requested = Config.currentDate;
                parcel.IsActive = true;
                switch (i)
                {
                    case 0:
                        parcel.Scheduled = Config.currentDate.AddMinutes(Config.r.Next(5, 15));
                        parcel.PickedUp = Config.currentDate.AddMinutes(Config.r.Next(30, 45));
                        parcel.Delivered = Config.currentDate.AddMinutes(Config.r.Next(50, 100));
                        parcel.DroneId = Drones[0].Id;
                        break;
                    case 1:
                        parcel.Scheduled = Config.currentDate.AddMinutes(Config.r.Next(5, 15));
                        parcel.PickedUp = Config.currentDate.AddMinutes(Config.r.Next(30, 45));
                        parcel.Delivered = Config.currentDate.AddMinutes(Config.r.Next(50, 100));
                        parcel.DroneId = Drones[3].Id;
                        break;
                    case 2:
                        parcel.Scheduled = Config.currentDate.AddMinutes(Config.r.Next(5, 15));
                        parcel.DroneId = Drones[2].Id;
                        break;
                }
                Parcels.Add(parcel);

                Config.SerialNumber++;
            }
        }
        #endregion
    }
}