using System;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
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
            internal static int SerialNumber = 0;

            internal static readonly Random randomNumber = new();
            internal static readonly DateTime currentDate = DateTime.Now;
        }

        internal static void Initialize()
        {
            
            int numberOfDronesAvailableForDelivery = 0;
            int[] AvailableDronesIdArray = new int[5];
            int AvailableDronesIdIndex = 0;
            int[] ShipmentDronesIdArray = new int[5];
            int ShipmentDronesIdIndex = 0;

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
                Drones[i].Status = (DroneStatuses)(i % 3);
                if (Drones[i].Status != DroneStatuses.maintenance)
                {

                    numberOfDronesAvailableForDelivery++;
                    if (Drones[i].Status == DroneStatuses.available)
                    {
                        AvailableDronesIdArray[AvailableDronesIdIndex] = Drones[i].Id;
                        AvailableDronesIdIndex++;
                    }
                    else
                    {
                        ShipmentDronesIdArray[ShipmentDronesIdIndex] = Drones[i].Id;
                        ShipmentDronesIdIndex++;
                    }
                }
                Config.DroneIndex++;
            }

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
                Parcels[i].TargetId = Customers[i+1].Id;
                Parcels[i].Weight = (WeightCategiries)Config.randomNumber.Next(2);
                Parcels[i].Priority = (Priorities)Config.randomNumber.Next(2);

                if (i < numberOfDronesAvailableForDelivery)
                {
                    switch (Config.randomNumber.Next(1, 4))
                    {
                        case 1:
                            Parcels[i].Scheduled = Config.currentDate;
                            Parcels[i].DroneId = ShipmentDronesIdArray[--ShipmentDronesIdIndex];
                            break;
                        case 2:
                            Parcels[i].PickedUp = Config.currentDate;
                            Parcels[i].DroneId = ShipmentDronesIdArray[--ShipmentDronesIdIndex];
                            break;
                        case 3:
                        //falling-through - because it's do the same thing 
                        default:
                            Parcels[i].Delivered = Config.currentDate;
                            Parcels[i].DroneId = AvailableDronesIdArray[--AvailableDronesIdIndex];
                            break;
                    }
                    if (ShipmentDronesIdIndex == -1) ShipmentDronesIdIndex++;
                    if (AvailableDronesIdIndex == -1) AvailableDronesIdIndex++;
                }
                else
                {
                    Parcels[i].Requested = Config.currentDate;
                }
                Config.SerialNumber++;
            }
            Config.ParcelIndex++;
        }
    }
}
