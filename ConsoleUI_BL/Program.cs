using System;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
        internal static IBL.IBL BLObject = new IBL.BL();
        static void Main(string[] args)
        {
            Menu();
            Console.WriteLine("Have a nice day!");
        }


        internal static void Menu()
        {
            int choice;
            int subChoice;

            do
            {
                // MAIN MENU
                Console.WriteLine("Please enter your choice:");
                Console.WriteLine("1. Add options");
                Console.WriteLine("2. Update options");
                Console.WriteLine("3. View options");
                Console.WriteLine("4. View lists options");
                Console.WriteLine("5. Exit");

                int.TryParse(Console.ReadLine(), out choice);

                // SUB MENU
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please enter your choice: ");
                        Console.WriteLine("1. Add new base-station");
                        Console.WriteLine("2. Add new Drone");
                        Console.WriteLine("3. Add new customer");
                        Console.WriteLine("4. Add new parcel");
                        int.TryParse(Console.ReadLine(), out subChoice);
                        break;
                    case 2:
                        Console.WriteLine("\nPlease enter your choice: ");
                        Console.WriteLine("1. Update drone name (by ID)");
                        Console.WriteLine("2. Update base-station detailes");
                        Console.WriteLine("3. Send drone to charge");
                        Console.WriteLine("4. Release drone from charging");
                        Console.WriteLine("5. Associate parcel to drone");
                        Console.WriteLine("6. Pick-up parcel by drone");
                        Console.WriteLine("7. Deliverd parcel by drone");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        break;
                    case 3:
                        Console.WriteLine("\nPlease enter your choice:");
                        Console.WriteLine("1. Base-station view");
                        Console.WriteLine("2. Drone view");
                        Console.WriteLine("3. Customer view");
                        Console.WriteLine("4. Parcel view");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        break;
                    case 4:
                        Console.WriteLine("\nPlease enter your choice:");
                        Console.WriteLine("1. Base-stations view");
                        Console.WriteLine("2. Drones view");
                        Console.WriteLine("3. Customers view");
                        Console.WriteLine("4. Parcels view");
                        Console.WriteLine("5. Non-associate parcels view");
                        Console.WriteLine("6. Base-stations with available charging slots");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        break;
                    default:
                        Console.WriteLine("Bad choice, please Enter new choice:");
                        break;
                }
            } while (choice != 5);
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

                    break;
                case 4:

                    break;
                default:
                    break;
            }
        }
        internal static void AddNewBaseStation()
        {
            int intTemp;
            double doubleTemp;
            Station station = new();

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            station.Id = intTemp;

            Console.WriteLine("Enter station name:");
            station.Name = Console.ReadLine();

            Console.WriteLine("Enter the longitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            station.Location.Longitude = doubleTemp;

            Console.WriteLine("Enter the latitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            station.Location.Lattitude = doubleTemp;

            Console.WriteLine("Enter number of charging station:");
            int.TryParse(Console.ReadLine(), out intTemp);
            station.AvailableChargeSlots = intTemp;

            station.DroneChargesList = new();

            BLObject.SetNewStationBL(station);
        }

        internal static void AddNewDrone()
        {
            int intTemp;
            Drone drone = new();

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.Id = intTemp;

            Console.WriteLine("Enter model of drone:");
            drone.Model = Console.ReadLine();

            Console.WriteLine("Enter max weight: (1 - Light,  2- average,  3- Heavy)");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.MaxWeight = (WeightCategories)intTemp;

            Console.WriteLine("Enter base staion ID to put the drone for first charge");
            int.TryParse(Console.ReadLine(), out intTemp);

            BLObject.SetNewDroneBL(drone, intTemp);
        }

    }
}