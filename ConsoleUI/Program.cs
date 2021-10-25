using System;
using DalObject;
using System.Linq;
using System.Text;
using IDAL.DO;
using System.Collections.Generic;

namespace ConsoleUI
{
    class Program
    {


        static void Main(string[] args)
        {
            Menu();
            Console.Read();
        }


        internal static void Menu()
        {
            int choice;
            int subChoice;

            Console.WriteLine("Enter option number:");
            Console.WriteLine("1. Add options");
            Console.WriteLine("2. Update options");
            Console.WriteLine("3. View options");
            Console.WriteLine("4. View lists options");
            Console.WriteLine("5. Exit");

            do
            {
                int.TryParse(Console.ReadLine(), out choice);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\nEnter option number:");
                        Console.WriteLine("1. Add a new base-station");
                        Console.WriteLine("2. Add a new drone");
                        Console.WriteLine("3. Add new customer");
                        Console.WriteLine("4. Add a new parcel");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        AddMenu(subChoice);
                        break;
                    case 2:
                        Console.WriteLine("\nEnter option number:");
                        Console.WriteLine("1. Associate parcel to drone");
                        Console.WriteLine("2. Collect parcel by drone");
                        Console.WriteLine("3. Delivered parcel to customer");
                        Console.WriteLine("4. Send drone to base-station for charging");
                        Console.WriteLine("5. Release drone from charging");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        UpdateMenu(subChoice);
                        break;
                    case 3:
                        Console.WriteLine("\nEnter option number:");
                        Console.WriteLine("1. Base-station view");
                        Console.WriteLine("2. Drone view");
                        Console.WriteLine("3. Customer view");
                        Console.WriteLine("4. Parcel view");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        ViewMenu(subChoice);
                        break;
                    case 4:
                        Console.WriteLine("\nEnter option number:");
                        Console.WriteLine("1. Base-stations view");
                        Console.WriteLine("2. Drones view");
                        Console.WriteLine("3. Customers view");
                        Console.WriteLine("4. Associate parcels view");
                        Console.WriteLine("5. Non-associate parcels view");
                        Console.WriteLine("6. Base-stations with available charging slots");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        ListsViewMenu(subChoice);
                        break;
                    case 5:
                        /* Exit from the program. Do nothing */
                        break;
                    default:
                        Console.WriteLine("Bad choice, please Enter new choice:");
                        break;
                }

            } while (choice < 0 || choice > 5);
        }


        internal static void AddMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    AddNewBaseStation();
                    break;
                case 2:
                    AddNewDrone();
                    break;
                case 3:
                    AddNewCustomr();
                    break;
                case 4:
                    AddNewParcel();
                    break;
            }
        }


        internal static void UpdateMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    /* call to the first function */
                    break;
                case 2:
                    /* call to the secound function */
                    break;
                case 3:
                    /* call to the third function */
                    break;
                case 4:
                    SendingDroneForCharging();
                    break;
                case 5:
                    /* call to the fourth function */
                    break;
            }
        }


        internal static void ViewMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    /* call to the first function */
                    break;
                case 2:
                    /* call to the secound function */
                    break;
                case 3:
                    /* call to the third function */
                    break;
                case 4:
                    /* call to the fourth function */
                    break;
            }
        }


        internal static void ListsViewMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    /* call to the first function */
                    break;
                case 2:
                    /* call to the secound function */
                    break;
                case 3:
                    /* call to the third function */
                    break;
                case 4:
                    /* call to the fourth function */
                    break;
                case 5:
                    /* call to the fourth function */
                    break;
                case 6:
                    /* call to the fourth function */
                    break;
            }
        }

        //-------------- ADD MENU FUNCTOINS ---------------//
        public static void AddNewBaseStation()
        {
            int intTemp;
            double doubleTemp;
            Station station = new();

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            station.Id = intTemp;

            Console.WriteLine("Enter station name:");
            station.Name = Console.ReadLine();

            Console.WriteLine("Enter number of charging station:");
            int.TryParse(Console.ReadLine(), out intTemp);
            station.ChargeSlots = intTemp;

            Console.WriteLine("Enter the longitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            station.Longitude = doubleTemp;

            Console.WriteLine("Enter the latitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            station.Lattitude = doubleTemp;

            DalObject.DalObject.SetNewStation(station);

            Console.WriteLine("A new base station has been added");
        }


        public static void AddNewDrone()
        {
            int intTemp;
            double doubleTemp;
            Drone drone = new();

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.Id = intTemp;

            Console.WriteLine("Enter model of drone:");
            drone.Model = Console.ReadLine();

            Console.WriteLine("Enter max weight: (1 - Light,  2- average,  3- Heavy)");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.MaxWeight = (WeightCategiries)intTemp;

            Console.WriteLine("Enter the battery status: (0% - 100%)");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            drone.Battery = doubleTemp;

            Console.WriteLine("Enter drone status: (1 - available, 2 - maintenance,  3- shipment)");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.Status = (DroneStatuses)intTemp;

            DalObject.DalObject.SetNewDrone(drone);

            Console.WriteLine("New drone added");
        }


        public static void AddNewCustomr()
        {
            int intTemp;
            double doubleTemp;
            Customer customer = new();

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            customer.Id = intTemp;

            Console.WriteLine("Enter phone number:");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("Enter name:");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Enter longitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.Longitude = doubleTemp;

            Console.WriteLine("Enter latitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.Lattitude = doubleTemp;

            DalObject.DalObject.SetNewCustomer(customer);

            Console.WriteLine("New customer added");
        }


        public static void AddNewParcel()
        {
            int intTemp;

            int choice;
            Parcel parcel = new();
            DateTime currentDate = DateTime.Now;

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Id = intTemp;

            Console.WriteLine("Enter sender id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.SenderId = intTemp;

            Console.WriteLine("Enter target id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.TargetId = intTemp;

            Console.WriteLine("Enter drone id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.DroneId = intTemp;

            Console.WriteLine("Enter parcel weight: (1 - Light, 2 - average, 3 - Heavy)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Weight = (WeightCategiries)intTemp;

            Console.WriteLine("Enter parcel priority: (1 - Regular, 2 - fast, 3 - emergency)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Priority = (Priorities)intTemp;

            Console.WriteLine("Enter parcel status: (1 - Requested, 2 - Scheduled, 3 - PickedUp, 4 - Delivered)");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    parcel.Requested = currentDate;
                    break;
                case 2:
                    parcel.Scheduled = currentDate;
                    break;
                case 3:
                    parcel.PickedUp = currentDate;
                    break;
                case 4:
                    parcel.Delivered = currentDate;
                    break;
                default:
                    parcel.Requested = currentDate;
                    break;
            }
            DalObject.DalObject.SetNewParcel(parcel);

            Console.WriteLine("A new parcel has been added");
        }

        //----------------------- UPDATE FANCTIONS ---------------------------//


        public static void AssociateParcelToDrone()
        {
            int parcelId;
            int droneId;

            Console.WriteLine("Enter parcel ID: ");
            int.TryParse(Console.ReadLine(), out parcelId);

            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out droneId);

            DalObject.DalObject.UpdateDronIdOfParcel(parcelId, droneId);

            Console.WriteLine("The drone has been successfully associated");
        }

        //------------------------------------------------------------------------//

        public static void CollectingPackageByDrone()
        {
            int parcelId;

            Console.WriteLine("Please enter your parcel id: ");
            int.TryParse(Console.ReadLine(), out parcelId);


            DalObject.DalObject.UpdatePickedUpParcelById(parcelId);

            Console.WriteLine("Picked up time updated successfully");
        }


        //------------------------------------------------------------------------//

        public static void DeliveryParcelToCustomer()
        {
            int ParcelId;

            Console.WriteLine("Please enter your parcel id: ");
            int.TryParse(Console.ReadLine(), out ParcelId);


            DalObject.DalObject.UpdateDeliveredParcelById(ParcelId);

            Console.WriteLine("Delivery time updated successfully");
        }



        public static void SendingDroneForCharging()
        {
            int droneId;
            int stationId;

            Console.WriteLine("Enter Drone id: ");
            int.TryParse(Console.ReadLine(), out droneId);

            Console.WriteLine("Enter station id: ");
            ViewStationsWithAvailableChargingSlots();
            int.TryParse(Console.ReadLine(), out stationId);

            DalObject.DalObject.UpdateDroneToCharging(droneId, stationId);

            Console.WriteLine("Drone sent to charging slot");
        }



        public static void ViewStationsWithAvailableChargingSlots()
        {
            List<Station> Stations = new List<Station>();
            Stations = DalObject.DalObject.GetStationsWithAvailableChargingSlots();

            foreach (var station in Stations)
            {
                Console.WriteLine(station.Id);
            }
        }
    }
}