using System;
using DalObject;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {

      
        static void Main(string[] args)
        {
            Manu();
        }

    
        internal static void Manu()
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
                    /* call to the fourth function */
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

        public static void AddNewBaseStation()
        {
            Station station = new();

            Console.WriteLine("Enter id:");
            station.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter station name:");
            station.Name = Console.ReadLine();

            Console.WriteLine("Enter number of charging station:");
            station.ChargeSlots = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the longitude:");
            station.Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter the latitude:");
            station.Lattitude = double.Parse(Console.ReadLine());

            DalObject.DalObject.SetNewStation(station);
        }

        public static void AddNewDrone()
        {
            Drone drone = new();
            
            Console.WriteLine("Enter id:");
            drone.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter model of drone:");
            drone.Model = Console.ReadLine();

            Console.WriteLine("Enter max weight: (1 - Light,  2- average,  3- Heavy)");
            drone.MaxWeight = (WeightCategiries)int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the battery status: (0% - 100%)");
            drone.Battery = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter drone status: (1 - available, 2 - maintenance,  3- shipment)");
            drone.Status = (DroneStatuses)int.Parse(Console.ReadLine());

            DalObject.DalObject.SetNewDrone(drone);          
        }

        public static void AddNewCustomr()
        {
            Customer customer = new();

            Console.WriteLine("Enter id:");
            customer.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter phone number:");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("Enter name:");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Enter longitude:");
            customer.Longitude = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter latitude:");
            customer.Latitude = double.Parse(Console.ReadLine());

            DalObject.DalObject.SetNewCustomer(customer);
        }

        public static void AddNewParcel()
        {
            int choice;
            Parcel parcel = new();
            Random randomNumber = new();
            DateTime currentDate = DateTime.Now;

        Console.WriteLine("Enter id:");
            parcel.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter sender id:");
            parcel.SenderId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter target id:");
            parcel.TargetId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter drone id:");
            parcel.DroneId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter parcel weight: (1 - Light, 2 - average, 3 - Heavy)");
            parcel.Weight = (WeightCategiries)int.Parse(Console.ReadLine());

            Console.WriteLine("Enter parcel priority: (1 - Regular, 2 - fast, 3 - emergency)");
            parcel.Priority = (Priorities)int.Parse(Console.ReadLine());

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
        }

    }
}