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
            internal static int SerialNumber = 0;

            private static Random randomNumber = new Random();
            private static DateTime currentDate = DateTime.Now;


            internal static void Initialize()
            {
                int Number_Of_Drones_Available_For_Delivery = 0;
                int[] AvailableDronesIdArray = new int[5];
                int AvailableDronesIdIndex = 0;
                int[] ShipmentDronesIdArray = new int[5];
                int ShipmentDronesIdIndex = 0;


                for (int i = 0; i < 2; i++)
                {
                    Stations[i].Id = randomNumber.Next(1000, 10000);
                    Stations[i].Name = "Station" + i;
                    Stations[i].Longitude = 35 + randomNumber.NextDouble();
                    Stations[i].Lattitude = 31 + randomNumber.NextDouble();
                    Stations[i].ChargeSlots = randomNumber.Next(0, 2);
                }


                for (int i = 0; i < 5; i++)
                {
                    Drones[i].Id = randomNumber.Next(1000, 10000);
                    Drones[i].Model = "V" + i;
                    Drones[i].Battery = randomNumber.Next(0, 1001) / randomNumber.Next(1, 11);
                    Drones[i].MaxWeight = (WeightCategiries)randomNumber.Next(2);
                    Drones[i].Status = (DroneStatuses)(i % 3);
                    if (Drones[i].Status != DroneStatuses.maintenance)
                    {
                        Number_Of_Drones_Available_For_Delivery++;
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
                }


                for (int i = 0; i < 10; i++)
                {
                    Customers[i].Id = randomNumber.Next(1000, 10000);
                    Customers[i].Name = "Name" + i;
                    Customers[i].Phone = "050123456" + i;
                    Customers[i].Longitude = 35 + randomNumber.NextDouble();
                    Customers[i].Lattitude = 31 + randomNumber.NextDouble();
                }


                for (int i = 1; i < 10; i++)
                {
                    Parcels[i].Id = randomNumber.Next(1000, 10000);
                    Parcels[i].SenderId = randomNumber.Next(1000, 10000);
                    Parcels[i].TargetId = randomNumber.Next(1000, 10000);
                    Parcels[i].Weight = (WeightCategiries)randomNumber.Next(2);
                    Parcels[i].Priority = (Priorities)randomNumber.Next(2);

                    if (i < Number_Of_Drones_Available_For_Delivery)
                    {

                        switch (randomNumber.Next(1, 4))
                        {
                            case 1:

                                Parcels[i].Scheduled = currentDate;
                                Parcels[i].DroneId = ShipmentDronesIdArray[--ShipmentDronesIdIndex];
                                break;

                            case 2:
                                Parcels[i].PickedUp = currentDate;
                                Parcels[i].DroneId = ShipmentDronesIdArray[--ShipmentDronesIdIndex];
                                break;

                            case 3:
                            //falling-through - because it's do the same thing 
                            default:
                                Parcels[i].Delivered = currentDate;
                                Parcels[i].DroneId = AvailableDronesIdArray[--AvailableDronesIdIndex];
                                break;
                        }
                    }
                    else
                    {
                        Parcels[i].Requested = currentDate;
                    }


                    SerialNumber++;
                }
            }
        }
    }
}
