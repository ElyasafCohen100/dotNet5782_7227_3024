using System;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
        internal static IBL.IBL BLObject = new BL.BL();
        static void Main(string[] args)
        {
            Menu();
        }

        internal static void Menu()
        {
            int choice;
            int subChoice;

            do
            {
                // MAIN MENU
                Console.WriteLine("Enter a choice:");
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
                        Console.WriteLine("Enter a choice:");
                        Console.WriteLine("1. Add new base-station");
                        Console.WriteLine("2. Add new Drone");
                        Console.WriteLine("3. Add new customer");
                        Console.WriteLine("4. Add new parcel");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        AddMenu(subChoice);
                        break;
                    case 2:
                        Console.WriteLine("\nEnter a choice:");
                        Console.WriteLine("1. Update drone model");
                        Console.WriteLine("2. Update base-station detailes");
                        Console.WriteLine("3. Update customer detailes");
                        Console.WriteLine("4. Send drone to charge");
                        Console.WriteLine("5. Release drone from charging");
                        Console.WriteLine("6. Associate parcel to drone");
                        Console.WriteLine("7. Pick-up parcel by drone");
                        Console.WriteLine("8. Deliverd parcel by drone");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        UpdateMenu(subChoice);
                        break;
                    case 3:
                        Console.WriteLine("\nEnter a choice:");
                        Console.WriteLine("1. Base-station view");
                        Console.WriteLine("2. Drone view");
                        Console.WriteLine("3. Customer view");
                        Console.WriteLine("4. Parcel view");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        ViewMenu(subChoice);
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

        //---------------------- SUB MENU FUNCTOINS -----------------------//

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
                    AddNewCostumer();
                    break;
                case 4:
                    AddNewParcel();
                    break;
                default:
                    Console.WriteLine("Not valid choice!\n");
                    Menu();
                    break;
            }
        }

        internal static void UpdateMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    UpdateDroneModelByID();
                    break;
                case 2:
                    UpdateBaseStationdetailes();
                    break;
                case 3:
                    UpdateCustomerDetailes();
                    break;
                case 4:
                    UpdateDroneToCharging();
                    break;
                case 5:
                    UpdateDroneFromCharging();
                    break;
                case 6:
                    AssociateParcelToDrone();
                    break;
                case 7:

                    break;
                case 8:

                    break;
                default:
                    Console.WriteLine("Not valid choice!\n");
                    Menu();
                    break;
            }
        }

        internal static void ViewMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    ViewBaseStation();
                    break;
                case 2:
                    ViewDrone();
                    break;
                case 3:
                    ViewCustomer();
                    break;
                case 4:
                    ViewParcel();
                    break;
            }
        }

        //-------------------- ADD MENU FUNCTOINS ---------------------//

        public static void AddNewBaseStation()
        {
            int intTemp;
            double doubleTemp;
            Station station = new();

            Console.WriteLine("Enter base-station ID:");
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

            BLObject.AddNewStationBL(station);
        }

        public static void AddNewDrone()
        {
            int intTemp;
            Drone drone = new();

            Console.WriteLine("Enter id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.Id = intTemp;

            Console.WriteLine("Enter model of drone:");
            drone.Model = Console.ReadLine();

            Console.WriteLine("Enter max weight: (1 - Light,  2 - Intermediate,  3 - Heavy)");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.MaxWeight = (WeightCategories)intTemp;

            Console.WriteLine("Enter base staion ID to put the drone for first charge");
            int.TryParse(Console.ReadLine(), out intTemp);

            BLObject.AddNewDroneBL(drone, intTemp);
        }

        public static void AddNewCostumer()
        {
            int intTemp;
            double doubleTemp;
            Customer customer = new();

            Console.WriteLine("Enter ID: ");
            int.TryParse(Console.ReadLine(), out intTemp);
            customer.Id = intTemp;

            Console.WriteLine("Enter Customer name: ");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Enter phone number: ");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("Enter location: ");
            Console.WriteLine("longtitude: ");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.location.Longitude = doubleTemp;

            Console.WriteLine("Lattitude: ");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.location.Lattitude = doubleTemp;

            BLObject.AddNewCustomerBL(customer);
        }

        public static void AddNewParcel()
        {
            int intTemp;
            Parcel parcel = new();

            Console.WriteLine("Enter sender ID: ");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.senderCustomer.Id = intTemp;

            Console.WriteLine("Enter reciver id: ");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.receiverCustomer.Id = intTemp;

            Console.WriteLine("Enter parcel weight category: (1 - Light,  2 - Intermediate,  3 - Heavy)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Weight = (WeightCategories)intTemp;

            Console.WriteLine("Enter parcel priority:  (1 - Fast,  2 - Regular,  3 - Slow)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Priority = (Priorities)intTemp;

            parcel.Drone = null;

            BLObject.AddNewParcelBL(parcel);
        }

        //------------------- UPDATE FANCTIONS ------------------//

        public static void UpdateDroneModelByID()
        {
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            Console.WriteLine("Enter new name: ");
            string newModel = Console.ReadLine();

            BLObject.UpdateDroneModelBL(droneId, newModel);
        }

        public static void UpdateBaseStationdetailes()
        {
            Console.WriteLine("Enter base-station number: ");
            int.TryParse(Console.ReadLine(), out int baseStationId);

            Console.WriteLine("Enter detailes to change: \n (if you don't want to change leave empty)\n");
            Console.WriteLine("Enter base-station name: ");
            string baseStationNewName = Console.ReadLine();

            Console.WriteLine("Enter number of base-station charge slots: ");
            int.TryParse(Console.ReadLine(), out int baseStationChargeSlots);

            BLObject.UpdateBaseStationDetailes(baseStationId, baseStationNewName, baseStationChargeSlots);
        }

        public static void UpdateCustomerDetailes()
        {
            Console.WriteLine("Enter customer ID: ");
            int.TryParse(Console.ReadLine(), out int coustomerId);

            Console.WriteLine("Enter detailes to change: \n (if you don't want to change leave empty)\n");
            Console.WriteLine("Enter new name: ");
            string newCustomerName = Console.ReadLine();

            Console.WriteLine("Enter new Phone number: ");
            string newCustomerPhoneNumber = Console.ReadLine();

            BLObject.UpdateCustomerDetailes(coustomerId, newCustomerName, newCustomerPhoneNumber);
        }

        public static void UpdateDroneToCharging()
        {
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            BLObject.UpdateDroneToChargingBL(droneId);
        }

        public static void UpdateDroneFromCharging()
        {
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            Console.WriteLine("Enter time for charging in minutes: ");
            double.TryParse(Console.ReadLine(), out double chargeTime);

            BLObject.UpdateDroneFromChargingBL(droneId, chargeTime);
        }

        public static void AssociateParcelToDrone()
        {

            Console.WriteLine("Enter Drone ID:");
            int.TryParse(Console.ReadLine(), out int droneId);

            BLObject.UpdateDroneIdOfParcelBL(droneId);

            Console.WriteLine("Drone associated successfully");
        }

        //--------------------- VIEW MENU FUNCTION ----------------------//

        /// <summary>
        /// Print BaseStation details.
        /// </summary>
        public static void ViewBaseStation()
        {
            Console.WriteLine("Enter Base-Station ID: ");
            int.TryParse(Console.ReadLine(), out int baseStatinId);

            Station myBaseStation = BLObject.FindStationByIdBL(baseStatinId);
            Console.WriteLine(myBaseStation.ToString());
        }

        /// <summary>
        /// Print Drone details.
        /// </summary>
        public static void ViewDrone()
        {
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            Drone myDrone = BLObject.FindDroneByIdBL(droneId);
            Console.WriteLine(myDrone.ToString());
        }

        /// <summary>
        /// Print Customer details.
        /// </summary>
        public static void ViewCustomer()
        {
            Console.WriteLine("Enter customer ID: ");
            int.TryParse(Console.ReadLine(), out int customerId);

            Customer myCusromer = BLObject.FindCustomerByIdBL(customerId);
            Console.WriteLine(myCusromer.ToString());
        }

        /// <summary>
        /// Print Parcel details.
        /// </summary>
        public static void ViewParcel()
        {
            Console.WriteLine("Enter parcel ID: ");
            int.TryParse(Console.ReadLine(), out int parcelId);

            Parcel myParcel = BLObject.FindParcelByIdBL(parcelId);
            Console.WriteLine(myParcel.ToString());
        }

        //--------------------- LIST VIEW MENU FUNCTION ----------------------//

    }

}