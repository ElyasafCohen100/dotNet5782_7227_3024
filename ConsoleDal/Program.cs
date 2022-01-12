using System;
using System.Linq;
using System.Text;
using DO;
using System.Collections.Generic;

namespace ConsoleUI
{
    /// <summary>
    /// Program class.
    /// Contains the main fuction
    /// </summary>
    class Program
    {
        internal static DalApi.IDal dalObject;

        /// <summary>
        /// The main fantion
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                dalObject = DalApi.DalFactory.GetDal();
            }
            catch (DalApi.DalConfigException e)
            {
                Console.WriteLine(e.Message);
            }

            Menu();
        }

        /// <summary>
        /// Menu of choice.
        /// </summary>
        internal static void Menu()
        {
            int choice;
            int subChoice;

            do
            {
                //MAIN MENU
                Console.WriteLine("Enter option number:");
                Console.WriteLine("0. Calculate Distance between two points");
                Console.WriteLine("1. Add options");
                Console.WriteLine("2. Update options");
                Console.WriteLine("3. View options");
                Console.WriteLine("4. View lists options");
                Console.WriteLine("5. Exit");

                int.TryParse(Console.ReadLine(), out choice);

                //SUB MENU
                switch (choice)
                {
                    case 0:
                        DistanceBetweenTwoPointsOnEarth();
                        break;
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
                        // Exit from the program.
                        break;
                    default:
                        Console.WriteLine("Bad choice, please Enter new choice:");
                        break;
                }

            } while (choice != 5);
        }

        //---------------------- SUB MENU FUNCTOINS -----------------------//

        /// <summary>
        /// Menu of add functions.
        /// </summary>
        /// <param name="subChoice"> cohice </param>
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

        /// <summary>
        /// Menu of update functions.
        /// </summary>
        /// <param name="subChoice">choice</param>
        internal static void UpdateMenu(int subChoice)
        {
            switch (subChoice)
            {
                case 1:
                    AssociateParcelToDrone();
                    break;
                case 2:
                    ParcelCollectByDrone();
                    break;
                case 3:
                    ParcelDeliveredToCustomer();
                    break;
                case 4:
                    SendDroneForCharging();
                    break;
                case 5:
                    ReleaseDroneFromChargingSlot();
                    break;
            }
        }

        /// <summary>
        /// Menu of view functions.
        /// </summary>
        /// <param name="subChoice">xhoice</param>
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

        /// <summary>
        /// Menu of view List.
        /// </summary>
        /// <param name="subChoice"> choice </param>
        public static void ListsViewMenu(int subChoice)
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
            }
        }

        //------------------- ADD MENU FUNCTOINS -------------------//

        /// <summary>
        /// Add new Base-Station.
        /// </summary>
        public static void AddNewBaseStation()
        {
            int intTemp;
            double doubleTemp;
            Station station = new();

            Console.WriteLine("Enter ID:");
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
            station.Latitude = doubleTemp;

            dalObject.AddNewStation(station);

            Console.WriteLine("New base station added");
        }

        /// <summary>
        /// Add new Drone.
        /// </summary>
        public static void AddNewDrone()
        {
            int intTemp;
            Drone drone = new();

            Console.WriteLine("Enter ID:");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.Id = intTemp;

            Console.WriteLine("Enter model of drone:");
            drone.Model = Console.ReadLine();

            Console.WriteLine("Enter max weight: (1 - Heavy,  2 - Intermediate,  3 - Light)");
            int.TryParse(Console.ReadLine(), out intTemp);
            drone.MaxWeight = (WeightCategories)intTemp;

            dalObject.AddNewDrone(drone);

            Console.WriteLine("New drone added");
        }

        /// <summary>
        /// Add new Customer.
        /// </summary>
        public static void AddNewCustomr()
        {
            int intTemp;
            double doubleTemp;
            Customer customer = new();

            Console.WriteLine("Enter ID:");
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
            customer.Latitude = doubleTemp;

            dalObject.AddNewCustomer(customer);

            Console.WriteLine("New customer added");
        }

        /// <summary>
        /// Add new Parcel.
        /// </summary>
        public static void AddNewParcel()
        {
            int intTemp;

            int choice;
            Parcel parcel = new();
            DateTime currentDate = DateTime.Now;

            Console.WriteLine("Enter ID:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Id = intTemp;

            Console.WriteLine("Enter sender ID:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.SenderId = intTemp;

            Console.WriteLine("Enter target ID:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.TargetId = intTemp;

            Console.WriteLine("Enter drone ID:");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.DroneId = intTemp;

            Console.WriteLine("Enter parcel weight: (1 - Heavy, 2 - Intermediate, 3 - Light)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Weight = (WeightCategories)intTemp;

            Console.WriteLine("Enter parcel priority: (1 - Regular, 2 - Fast, 3 - Emergency)");
            int.TryParse(Console.ReadLine(), out intTemp);
            parcel.Priority = (Priorities)intTemp;

            Console.WriteLine("Enter parcel status: (1 - Requested" +
                ", 2 - Scheduled, 3 - PickedUp, 4 - Delivered)");
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
            dalObject.AddNewParcel(parcel);

            Console.WriteLine("A new parcel has been added");
        }

        //------------------- UPDATE FANCTIONS -------------------//

        /// <summary>
        /// Update Parcel status by the recived Id.
        /// </summary>
        public static void AssociateParcelToDrone()
        {
            int parcelId;
            int droneId;

            Console.WriteLine("Enter parcel ID: ");
            int.TryParse(Console.ReadLine(), out parcelId);

            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out droneId);

            dalObject.UpdateDroneIdOfParcel(parcelId, droneId);

            Console.WriteLine("The drone has been successfully associated");
        }

        /// <summary>
        /// Update Parcel status by the recived Id.
        /// </summary>
        public static void ParcelCollectByDrone()
        {
            int parcelId;

            Console.WriteLine("Enter parcel ID: ");
            int.TryParse(Console.ReadLine(), out parcelId);


            dalObject.UpdatePickedUpParcelById(parcelId);

            Console.WriteLine("Picked up time updated successfully");
        }

        /// <summary>
        /// Update Parcel  by the recived Id.
        /// </summary>
        public static void ParcelDeliveredToCustomer()
        {
            int ParcelId;

            Console.WriteLine("Enter your parcel ID: ");
            int.TryParse(Console.ReadLine(), out ParcelId);


            dalObject.UpdateDeliveredParcelById(ParcelId);

            Console.WriteLine("Delivery time updated successfully");
        }

        /// <summary>
        /// Update Drone status by the recived Id.
        /// </summary>
        public static void SendDroneForCharging()
        {
            int droneId;
            int stationId;

            Console.WriteLine("Enter Drone ID: ");
            int.TryParse(Console.ReadLine(), out droneId);

            Console.WriteLine("Enter station ID: ");
            ViewStationsWithAvailableChargingSlots();
            int.TryParse(Console.ReadLine(), out stationId);

            dalObject.UpdateDroneToCharging(droneId, stationId);

            Console.WriteLine("Drone sent to charging slot");
        }

        /// <summary>
        /// Update Drone status by the recived Id.
        /// </summary>
        public static void ReleaseDroneFromChargingSlot()
        {
            int droneId;

            Console.WriteLine("Enter Drone ID: ");
            int.TryParse(Console.ReadLine(), out droneId);

            dalObject.UpdateDroneFromCharging(droneId);
            Console.WriteLine("The drone released from charging succesfully");
        }


        //------------------- VIEW MENU FUNCTION -------------------//

        /// <summary>
        /// Print base station details
        /// </summary>
        public static void ViewBaseStation()
        {
            Console.WriteLine("Enter Base-Station ID: ");
            int.TryParse(Console.ReadLine(), out int baseStatinId);


            Station myBaseStation = dalObject.GetStationById(baseStatinId);
            Console.WriteLine(myBaseStation.ToString());

        }

        /// <summary>
        /// Print Drone details.
        /// </summary>
        public static void ViewDrone()
        {
            Console.WriteLine("Enter drone ID: ");
            int.TryParse(Console.ReadLine(), out int droneId);

            Drone myDrone = dalObject.GetDroneById(droneId);
            Console.WriteLine(myDrone.ToString());

        }

        /// <summary>
        /// Print Customer details.
        /// </summary>
        public static void ViewCustomer()
        {
            Console.WriteLine("Enter customer ID: ");
            int.TryParse(Console.ReadLine(), out int customerId);

            Customer myCusromer = dalObject.GetCustomerById(customerId);
            Console.WriteLine(myCusromer.ToString());
        }

        /// <summary>
        /// Print Parcel details.
        /// </summary>
        public static void ViewParcel()
        {
            Console.WriteLine("Enter parcel ID: ");
            int.TryParse(Console.ReadLine(), out int parcelId);

            Parcel myParcel = dalObject.GetParcelById(parcelId);
            Console.WriteLine(myParcel.ToString());
        }


        //------------------- LISTS VIEW MENU FUNCTION -------------------//

        /// <summary>
        /// Print List of Base-Stations.
        /// </summary>
        public static void ViewBaseStationsList()
        {
            Console.WriteLine("The stations are:");
            IEnumerable<Station> stations = dalObject.GetBaseStationList();


            foreach (var station in stations)
            {
                Console.WriteLine(station.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print List of Drones.
        /// </summary>
        public static void ViewDronesList()
        {
            Console.WriteLine("The drones are:");
            IEnumerable<Drone> myDrones = dalObject.GetDroneList();

            foreach (var drone in myDrones)
            {
                Console.WriteLine(drone.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print list of Customers.
        /// </summary>
        public static void ViewCustomersList()
        {
            Console.WriteLine("The customers are:");
            IEnumerable<Customer> myCustomers = dalObject.GetCustomerList();

            foreach (var customer in myCustomers)
            {
                Console.WriteLine(customer.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print list of Parcels.
        /// </summary>
        public static void ViewParcelsList()
        {
            Console.WriteLine("The parcels are:");
            IEnumerable<Parcel> myParcels = dalObject.GetParcelList();

            foreach (var parcel in myParcels)
            {
                Console.WriteLine(parcel.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print list of non associate Parcels.
        /// </summary>
        public static void ViewNonAssociateParcelsList()
        {
            Console.WriteLine("The non-associate parcels are:");
            IEnumerable<Parcel> myNonAssociateParcels = dalObject.GetParcels(x => x.DroneId == 0);

            foreach (var nonAssociateParcel in myNonAssociateParcels)
            {
                Console.WriteLine(nonAssociateParcel.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print Stations with available charging slots.
        /// </summary>
        public static void ViewStationsWithAvailableChargingSlots()
        {
            IEnumerable<Station> Stations = new List<Station>();
            Stations = dalObject.GetStations(x => x.ChargeSlots > 0);
            foreach (var station in Stations)
            {
                Console.WriteLine(station.ToString());
                Console.WriteLine();
            }
        }

        /// <summary>
        ///  Calculate distance between two points on earth.
        /// </summary>
        public static void DistanceBetweenTwoPointsOnEarth()
        {
            Console.WriteLine("Enter the first point coordinats: ");
            Console.Write("longitude: ");
            double.TryParse(Console.ReadLine(), out double longitudeA);
            Console.Write("lattitude: ");
            double.TryParse(Console.ReadLine(), out double lattitudeA);

            Console.WriteLine("Enter the second point coordinats: ");
            Console.Write("longitude: ");
            double.TryParse(Console.ReadLine(), out double longitudeB);
            Console.Write("lattitude: ");
            double.TryParse(Console.ReadLine(), out double lattitudeB);

            Console.WriteLine(dalObject.Distance(lattitudeA, lattitudeB, longitudeA, longitudeB));
        }
    }
}