using System;
using System.Collections.Generic;
using BO;

namespace ConsoleBl
{
    class Program
    {
        internal static BlApi.IBL BLObject;

        static void Main(string[] args)
        {
            try
            {
                BLObject = BlApi.BlFactory.GetBl();
            }
            catch (DalApi.DalConfigException e)
            {

                Console.WriteLine(e.Message);
            }
            Menu();
        }

        internal static void Menu()
        {
            int choice;
            int subChoice;

            do
            {
                //MAIN MENU
                Console.WriteLine("Enter a choice:");
                Console.WriteLine("1. Add options");
                Console.WriteLine("2. Update options");
                Console.WriteLine("3. View options");
                Console.WriteLine("4. View lists options");
                Console.WriteLine("5. Exit");

                int.TryParse(Console.ReadLine(), out choice);

                //SUB MENU
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
                        Console.WriteLine("\nEnter your choice:");
                        Console.WriteLine("1. Base-stations view");
                        Console.WriteLine("2. Drones view");
                        Console.WriteLine("3. Customers view");
                        Console.WriteLine("4. Parcels view");
                        Console.WriteLine("5. Non-associate parcels view");
                        Console.WriteLine("6. Base-stations with available charging slots");

                        int.TryParse(Console.ReadLine(), out subChoice);
                        ViewListMenu(subChoice);
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
                    Console.WriteLine("Invalid choice!\n");
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
                    CollectParcelByDrone();
                    break;
                case 8:
                    DeliveredParcelToCustomer();
                    break;
                default:
                    Console.WriteLine("Invalid choice!\n");
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
                    try
                    {
                        ViewDrone();
                    }
                    catch (System.FormatException)
                    {
                        Console.WriteLine("Invalid Input Of Drone Id Exception, Please Try Again.");
                    }
                    catch (System.ArgumentNullException)
                    {
                        Console.WriteLine("please enter valid Id ");
                    }
                    break;

                case 3:
                    ViewCustomer();
                    break;
                case 4:
                    ViewParcel();
                    break;
                default:
                    Console.WriteLine("Invalid choice!\n");
                    Menu();
                    break;
            }
        }
        internal static void ViewListMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    ViewBaseStationsList();
                    break;
                case 2:
                    ViewDronesList();
                    break;
                case 3:
                    ViewCustomersList();
                    break;
                case 4:
                    ViewParcelsList();
                    break;
                case 5:
                    ViewNonAssociateParcelsList();
                    break;
                case 6:
                    ViewStationsWithAvailableChargingSlots();
                    break;
                default:
                    Console.WriteLine("Invalid choice!\n");
                    Menu();
                    break;
            }
        }

        //-------------------- ADD MENU FUNCTOINS ---------------------//

        public static void AddNewBaseStation()
        {
            int intTemp;
            double doubleTemp;
            Station station = new();

            Console.WriteLine("Enter base-station Id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            station.Id = intTemp;

            Console.WriteLine("Enter station name:");
            station.Name = Console.ReadLine();

            Console.WriteLine("Enter the longitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            station.Location.Longitude = doubleTemp;


            Console.WriteLine("Enter the latitude:");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            station.Location.Latitude = doubleTemp;

            Console.WriteLine("Enter number of charging station:");
            int.TryParse(Console.ReadLine(), out intTemp);
            station.AvailableChargeSlots = intTemp;

            station.DroneChargesList = new();

            try
            {
                BLObject.AddNewStationBL(station);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectAlreadyExistException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void AddNewDrone()
        {
            int intTemp;
            Drone drone = new();

            Console.WriteLine("Enter Id:");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.Id = intTemp;

            Console.WriteLine("Enter model of drone:");
            drone.Model = Console.ReadLine();

            Console.WriteLine("Enter max weight: (0 - Heavy,  1 - Intermediate,  2 - Light)");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.MaxWeight = (WeightCategories)intTemp;

            Console.WriteLine("Enter base staion Id to put the drone for first charge");
            int.TryParse(Console.ReadLine(), out intTemp);

            try
            {
                BLObject.AddNewDroneBL(drone, intTemp);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectAlreadyExistException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void AddNewCostumer()
        {
            int intTemp;
            double doubleTemp;
            Customer customer = new();

            Console.WriteLine("Enter Id: ");
            int.TryParse(Console.ReadLine(), out intTemp);
            customer.Id = intTemp;

            Console.WriteLine("Enter Customer name: ");
            customer.Name = Console.ReadLine();

            Console.WriteLine("Enter phone number: ");
            customer.Phone = Console.ReadLine();

            Console.WriteLine("Enter location: ");
            Console.WriteLine("longtitude: ");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.Location.Longitude = doubleTemp;

            Console.WriteLine("Lattitude: ");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.Location.Latitude = doubleTemp;

            try
            {
                BLObject.AddNewCustomerBL(customer);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectAlreadyExistException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void AddNewParcel()
        {
            int intTemp;
            Parcel parcel = new();

            Console.WriteLine("Enter sender id: ");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.senderCustomer.Id = intTemp;

            Console.WriteLine("Enter reciver id: ");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.receiverCustomer.Id = intTemp;
            Console.WriteLine("Enter parcel weight category: (0 - Heavy,  1 - Intermediate,  2 - Light)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Weight = (WeightCategories)intTemp;

            Console.WriteLine("Enter parcel priority:  (0 -  Regular,  1 - Fast,  2 - Emergency)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Priority = (Priorities)intTemp;

            parcel.Drone = null;

            try
            {
                BLObject.AddNewParcelBL(parcel);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //------------------- UPDATE FANCTIONS ------------------//

        public static void UpdateDroneModelByID()
        {
            Console.WriteLine("Enter drone Id: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            Console.WriteLine("Enter new name: ");
            string newModel = Console.ReadLine();

            try
            {
                BLObject.UpdateDroneModelBL(droneId, newModel);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
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

            try
            {
                BLObject.UpdateBaseStationDetailsBL(baseStationId, baseStationNewName, baseStationChargeSlots);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void UpdateCustomerDetailes()
        {
            Console.WriteLine("Enter customer Id: ");
            int.TryParse(Console.ReadLine(), out int coustomerId);

            Console.WriteLine("Enter detailes to change: \n (if you don't want to change leave empty)\n");
            Console.WriteLine("Enter new name: ");
            string newCustomerName = Console.ReadLine();

            Console.WriteLine("Enter new Phone number: ");
            string newCustomerPhoneNumber = Console.ReadLine();

            try
            {
                BLObject.UpdateCustomerDetailesBL(coustomerId, newCustomerName, newCustomerPhoneNumber);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void UpdateDroneToCharging()
        {
            Console.WriteLine("Enter drone Id: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                BLObject.UpdateDroneToChargingBL(droneId);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (OutOfBatteryException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void UpdateDroneFromCharging()
        {
            Console.WriteLine("Enter drone Id: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                BLObject.UpdateDroneFromChargingBL(droneId);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (NotValidRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void AssociateParcelToDrone()
        {

            Console.WriteLine("Enter Drone Id:");
            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                BLObject.AssociateDroneTofParcelBL(droneId);
                Console.WriteLine("Drone associated successfully");
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void CollectParcelByDrone()
        {
            Console.WriteLine("Enter Drone Id: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                BLObject.UpdatePickedUpParcelByDroneIdBL(droneId);

            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (NotValidRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void DeliveredParcelToCustomer()
        {
            Console.WriteLine("Enter Drone Id: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                BLObject.UpdateDeliveredParcelByDroneIdBL(droneId);
            }
            catch (NotValidRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //--------------------- VIEW MENU FUNCTION ----------------------//

        /// <summary>
        /// Print BaseStation details.
        /// </summary>
        public static void ViewBaseStation()
        {
            Console.WriteLine("Enter Base-Station Id: ");
            int.TryParse(Console.ReadLine(), out int baseStatinId);

            try
            {
                Station myBaseStation = BLObject.GetStationByIdBL(baseStatinId);
                Console.WriteLine(myBaseStation.ToString());
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Print Drone details.
        /// </summary>
        public static void ViewDrone()
        {
            Console.WriteLine("Enter drone Id: ");

            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                Drone blDrone = BLObject.GetDroneByIdBL(droneId);
                Console.WriteLine(blDrone.ToString());
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Print Customer details.
        /// </summary>
        public static void ViewCustomer()
        {
            Console.WriteLine("Enter customer Id: ");
            int.TryParse(Console.ReadLine(), out int customerId);

            try
            {
                Customer myCusromer = BLObject.GetCustomerByIdBL(customerId);
                Console.WriteLine(myCusromer.ToString());
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Print Parcel details.
        /// </summary>
        public static void ViewParcel()
        {
            Console.WriteLine("Enter parcel Id:");
            int.TryParse(Console.ReadLine(), out int parcelId);

            try
            {
                Parcel blparcel = BLObject.GetParcelByIdBL(parcelId);
                Console.WriteLine(blparcel.ToString());
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //--------------------- LIST VIEW MENU FUNCTION ----------------------//

        public static void ViewBaseStationsList()
        {
            Console.WriteLine("The stations are:");
            try
            {
                foreach (var station in BLObject.GetAllBaseStationsToList())
                {
                    Console.WriteLine(station.ToString());
                    Console.WriteLine();
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void ViewDronesList()
        {
            Console.WriteLine("The drones are:");

            foreach (var drone in BLObject.GetAllDroneToList())
            {
                Console.WriteLine(drone.ToString());
                Console.WriteLine();
            }
        }
        public static void ViewCustomersList()
        {
            Console.WriteLine("The customers are:");

            foreach (var customer in BLObject.GetAllCustomerToList())
            {
                Console.WriteLine(customer.ToString());
                Console.WriteLine();
            }
        }
        public static void ViewParcelsList()
        {
            Console.WriteLine("The parcels are:");

            foreach (var parcel in BLObject.GetAllParcelToList())
            {
                Console.WriteLine(parcel.ToString());
                Console.WriteLine();
            }
        }
        public static void ViewNonAssociateParcelsList()
        {
            Console.WriteLine("The non-associate parcels are:");

            foreach (var nonAssociateParcel in BLObject.GetNonAssociateParcelsListBL())
            {
                Console.WriteLine(nonAssociateParcel.ToString());
                Console.WriteLine();
            }
        }
        public static void ViewStationsWithAvailableChargingSlots()
        {
            Console.WriteLine("The stations with available charging slots are:");

            foreach (var station in BLObject.GetStationsWithAvailableChargingSlotstBL())
            {
                Console.WriteLine(station.ToString());
                Console.WriteLine();
            }
        }
    }
}