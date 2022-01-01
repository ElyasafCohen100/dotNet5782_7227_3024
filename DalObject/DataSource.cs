using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DO;


namespace Dal
{
    /// <summary>
    /// Contain the data source.
    /// </summary>
    public class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<Admin> Admins = new List<Admin>();


        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

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
            internal static double DroneChargingRate = 10;

            internal static int SerialNum = 1;

            internal static Random r = new Random();
            internal static DateTime currentDate = DateTime.Now;

        }

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


            for (int i = 0; i < 50; i++)
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
            for (int i = 0; i < 5; i++)
            {
                Drones.Add(new Drone()
                {
                    Id = Config.r.Next(1000, 10000),
                    Model = "A" + i,
                    MaxWeight = (WeightCategories)Config.r.Next(2),
                    IsActive = true
                });
            }

            for (int i = 0; i < 11; i++)
            {
                Customers.Add(new Customer()
                {
                    Id = Config.r.Next(100000000, 1000000000),
                    Name = "Customer" + i,
                    Phone = Config.r.Next(100000000, 1000000000).ToString(),
                    Latitude = 31.7 + Config.r.NextDouble() / 5,
                    Longitude = 35.1 + Config.r.NextDouble() / 5,
                    IsActive = true,
                    UserName = "Customer" + i,
                    Password = "Customer" + i
                });
            }

            for (int i = 0; i < 10; i++)
            {
                Parcel myParcel = new Parcel();
                myParcel.Id = Config.SerialNum;
                myParcel.SenderId = Customers[i].Id;
                myParcel.TargetId = Customers[i + 1].Id;
                myParcel.Weight = (WeightCategories)Config.r.Next(2);
                myParcel.Priority = (Priorities)Config.r.Next(2);
                myParcel.Requested = Config.currentDate;
                myParcel.IsActive = true;

                switch (i)
                {
                    case 0:
                        myParcel.Scheduled = Config.currentDate.AddMinutes(Config.r.Next(5, 15));
                        myParcel.PickedUp = Config.currentDate.AddMinutes(Config.r.Next(30, 45));
                        myParcel.Delivered = Config.currentDate.AddMinutes(Config.r.Next(10, 30));
                        myParcel.DroneId = Drones[0].Id;
                        break;
                    case 1:
                        myParcel.Scheduled = Config.currentDate.AddMinutes(Config.r.Next(5, 15));
                        myParcel.PickedUp = Config.currentDate.AddMinutes(Config.r.Next(30, 45));
                        myParcel.Delivered = Config.currentDate.AddMinutes(Config.r.Next(10, 30));
                        myParcel.DroneId = Drones[3].Id;
                        break;
                    case 2:
                        myParcel.Scheduled = Config.currentDate.AddMinutes(Config.r.Next(5, 15));
                        myParcel.DroneId = Drones[2].Id;
                        break;
                }
                Parcels.Add(myParcel);

                Config.SerialNum++;
            }
        }
    }
}