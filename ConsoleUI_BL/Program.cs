using System;
using System.Collections.Generic;
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
                    CollectParcelByDrone();
                    break;
                case 8:
                    DeliveredParcelToCustomer();
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
                    try
                    {
                      ViewDrone();
                    }
                    catch(System.FormatException)
                    {
                        Console.WriteLine("invalid Input Of Drone Id Exception, Please Try Again.");
                    }
                    catch (System.ArgumentNullException)
                    {
                        Console.WriteLine("please enter valid ID ");
                    }
                    break;

                case 3:
                    ViewCustomer();
                    break;
                case 4:
                    ViewParcel();
                    break;
                default:
                    Console.WriteLine("Not valid choice!\n");
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
                    Console.WriteLine("Not valid choice!\n");
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
            station.Location.Latitude = doubleTemp;

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
            customer.Location.Longitude = doubleTemp;

            Console.WriteLine("Lattitude: ");
            double.TryParse(Console.ReadLine(), out doubleTemp);
            customer.Location.Latitude = doubleTemp;

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

            Console.WriteLine("Enter parcel weight category: (0 - Light,  1 - Average,  2 - Heavy)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Weight = (WeightCategories)intTemp;

            Console.WriteLine("Enter parcel priority:  (0 - Fast,  1 - Regular,  2 - Slow)");
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

            BLObject.UpdateBaseStationDetails(baseStationId, baseStationNewName, baseStationChargeSlots);
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

            Console.WriteLine("Enter the charging time in minutes: ");
            double.TryParse(Console.ReadLine(), out double chargeTime);

            BLObject.UpdateDroneFromChargingBL(droneId, chargeTime);
        }

        public static void AssociateParcelToDrone()
        {

            Console.WriteLine("Enter Drone ID:");
            int.TryParse(Console.ReadLine(), out int droneId);

            try
            {
                BLObject.UpdateDroneIdOfParcelBL(droneId);
                Console.WriteLine("Drone associated successfully");
            }
            catch (ObjectNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidInputException)
            {
                Console.WriteLine("invalid drone id");
            }

        }

        public static void CollectParcelByDrone()
        {

            Console.WriteLine("Enter Drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            BLObject.UpdatePickedUpParcelByDroneIDBL(droneId);

            Console.WriteLine("Picked up time updated successfully");
        }

        public static void DeliveredParcelToCustomer()
        {
            Console.WriteLine("Enter Drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            BLObject.UpdateDeliveredParcelByDroneIdBL(droneId);

            Console.WriteLine("Delivery time updated successfully");
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
            try
            {
                int.TryParse(Console.ReadLine(), out int droneId);
                Drone myDrone = BLObject.FindDroneByIdBL(droneId);
                Console.WriteLine(myDrone.ToString());
            }
            catch (System.FormatException)
            {
                Console.WriteLine("please enter a valid integer for drone ID ");
            }
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

        public static void ViewBaseStationsList()
        {
            Console.WriteLine("The stations are:");

            foreach (var station in BLObject.ViewBaseStationsToList())
            {
                Console.WriteLine(station.ToString());
                Console.WriteLine();
            }
        }

        public static void ViewDronesList()
        {
            Console.WriteLine("The drones are:");

            foreach (var drone in BLObject.ViewDroneToList())
            {
                Console.WriteLine(drone.ToString());
                Console.WriteLine();
            }
        }

        public static void ViewCustomersList()
        {
            Console.WriteLine("The customers are:");

            foreach (var customer in BLObject.ViewCustomerToList())
            {
                Console.WriteLine(customer.ToString());
                Console.WriteLine();
            }
        }

        public static void ViewParcelsList()
        {
            Console.WriteLine("The parcels are:");

            foreach (var parcel in BLObject.ViewParcelToList())
            {
                Console.WriteLine(parcel.ToString());
                Console.WriteLine();
            }
        }

        public static void ViewNonAssociateParcelsList()
        {
            Console.WriteLine("The non-associate parcels are:");

            foreach (var nonAssociateParcel in BLObject.ViewNonAssociateParcelsListBL())
            {
                Console.WriteLine(nonAssociateParcel.ToString());
                Console.WriteLine();
            }
        }

        public static void ViewStationsWithAvailableChargingSlots()
        {
            Console.WriteLine("The stations with available charging slots are:");

            foreach (var station in BLObject.ViewStationsWithAvailableChargingSlotstBL())
            {
                Console.WriteLine(station.ToString());
                Console.WriteLine();
            }
        }
    }
}