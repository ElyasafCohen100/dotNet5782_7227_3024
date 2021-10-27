using IDAL.DO;
using System;
using System.Collections.Generic;

namespace DalObject
{
    /// <summary>
    /// Repersents Data Accsess Layer. 
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// C-tor.Initialize the DataSource components.
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }


        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Finds Drone by specific Id.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        /// <returns> Drone object </returns>
        public static Drone FindDroneById(int droneId)
        {
            int droneIndex = 0;
            while (DataSource.Drones[droneIndex].Id != droneId)
            {
                droneIndex++;
            }
            return DataSource.Drones[droneIndex];
        }

        /// <summary>
        /// Finds Station by specific Id.
        /// </summary>
        /// <param name="stationId"> Id of Station </param>
        /// <returns>Station object</returns>
        public static Station FindStationById(int stationId)
        {
            int stationIndex = 0;

            while (DataSource.Stations[stationIndex].Id != stationId)
            {
                stationIndex++;
            }

            return DataSource.Stations[stationIndex];
        }

        /// <summary>
        /// Finds DroneCharge by specific Id.
        /// </summary>
        /// <param name="droneId">Id of Drone </param>
        /// <returns> DroneCharge object </returns>
        public static DroneCharge FindDroneChargeByDroneId(int droneId)
        {
            DroneCharge myDroneCharge = new DroneCharge();

            foreach (var droneCharge in DataSource.DroneCharges)
            {
                if (droneCharge.DroneId == droneId)
                    myDroneCharge = droneCharge;
            }

            return myDroneCharge;
        }

        /// <summary>
        /// Finds Customer by specific Id.
        /// </summary>
        /// <param name="customerId"> Id of customer </param>
        /// <returns> Customer object </returns>
        public static Customer FindCustomerById(int customerId)
        {
            Customer myCustomer = new Customer();

            foreach (var customer in DataSource.Customers)
            {
                if (customer.Id == customerId)
                    myCustomer = customer;
            }

            return myCustomer;
        }

        /// <summary>
        /// Finds Parcel by specific Id.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <returns> Parcel </returns>
        public static Parcel FindParcelById(int parcelId)
        {
            Parcel myParcel = new Parcel();

            foreach (var parcel in DataSource.Parcels)
            {
                if (parcel.Id == parcelId)
                    myParcel = parcel;
            }

            return myParcel;
        }


        //----------------------- SETTERS -----------------------//

        /// <summary>
        /// Set new Drone.
        /// </summary>
        /// <param name="drone">Drone object</param>
        public static void SetNewDrone(Drone drone)
        {
            DataSource.Drones[DataSource.Config.DroneIndex] = drone;
            DataSource.Config.DroneIndex++;
        }

        /// <summary>
        /// Set new Station.
        /// </summary>
        /// <param name="station">Station object</param>
        public static void SetNewStation(Station station)
        {
            DataSource.Stations[DataSource.Config.StationIndex] = station;
            DataSource.Config.StationIndex++;
        }

        /// <summary>
        /// Set new Customer.
        /// </summary>
        /// <param name="customer"> Customer object </param>
        public static void SetNewCustomer(Customer customer)
        {
            DataSource.Customers[DataSource.Config.CustomerIndex] = customer;
            DataSource.Config.CustomerIndex++;
        }

        /// <summary>
        /// Set new Parcel.
        /// </summary>
        /// <param name="parcel"> Parcel object </param>
        public static void SetNewParcel(Parcel parcel)
        {
            DataSource.Parcels[DataSource.Config.ParcelIndex] = parcel;
            DataSource.Config.ParcelIndex++;
        }


        //----------------------- UPDATE FUNCTIONS -----------------------//

        /// <summary>
        /// Update Drone Id of Parcel.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        /// <param name="droneId"> Id of Drone </param>
        public static void UpdateDroneIdOfParcel(int parcelId, int droneId)
        {
            Parcel myParcel = FindParcelById(parcelId);
            myParcel.DroneId = droneId;

            Drone myDrone = FindDroneById(droneId);
            myDrone.Status = DroneStatuses.Shipment;
        }

        /// <summary>
        /// Update Parcel status to picked up.
        /// </summary>
        /// <param name="parcelId"> Id of Parcel </param>
        public static void UpdatePickedUpParcelById(int parcelId)
        {
            DateTime currentDate = DateTime.Now;
            Parcel myParcel = FindParcelById(parcelId);

            myParcel.PickedUp = currentDate;
        }

        /// <summary>
        /// Update Parcel status to Delivered. 
        /// </summary>
        /// <param name="parcelId">Id of Parcel</param>
        public static void UpdateDeliveredParcelById(int parcelId)
        {
            DateTime currentDate = DateTime.Now;
            Parcel myParcel = FindParcelById(parcelId);

            myParcel.Delivered = currentDate;
        }

        /// <summary>
        /// Update Drone status to maintenance,
        /// and decrese the number of charge slots in the Base-Station.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param>
        /// <param name="stationId"> Id of Station </param>
        public static void UpdateDroneToCharging(int droneId, int stationId)
        {

            Drone myDrone = FindDroneById(droneId);
            myDrone.Status = DroneStatuses.Maintenance;

            Station myStation = FindStationById(stationId);
            myStation.ChargeSlots--;

            DroneCharge droneCharge = new DroneCharge();
            droneCharge.DroneId = droneId;
            droneCharge.StationId = stationId;
            DataSource.DroneCharges.Add(droneCharge);
        }

        /// <summary>
        /// Update Drone status to Available from maintenance,
        /// and increse the number of charge slots in the Base-Station.
        /// </summary>
        /// <param name="droneId"> Id of Drone </param> 
        public static void UpdateDroneFromCharging(int droneId)
        {
            Drone myDrone = FindDroneById(droneId);
            myDrone.Status = DroneStatuses.Available;
            myDrone.Battery = 100;

            DroneCharge myDroneCharge = FindDroneChargeByDroneId(droneId);

            Station myStation = FindStationById(myDroneCharge.StationId);
            myStation.ChargeSlots++;

            DataSource.DroneCharges.Remove(myDroneCharge);
        }


        //--------------------------- GETTERS ---------------------------//

        /// <summary>
        /// Return list of Stations.
        /// </summary>
        /// <returns>List of Stations</returns>
        public static List<Station> GetBaseStationList()
        {
            List<Station> MyStations = new List<Station>(DataSource.Stations);

            return MyStations;
        }

        /// <summary>
        /// Return list of Drones.
        /// </summary>
        /// <returns> List of Drones </returns>
        public static List<Drone> GetDroneList()
        {
            List<Drone> MyDrones = new List<Drone>(DataSource.Drones);

            return MyDrones;
        }

        /// <summary>
        /// Return List of Customers.
        /// </summary>
        /// <returns> List of Customers </returns>
        public static List<Customer> GetCustomerList()
        {
            List<Customer> MyCustomers = new List<Customer>(DataSource.Customers);

            return MyCustomers;
        }

        /// <summary>
        /// Return List of Parcels.
        /// </summary>
        /// <returns>List of Parcels </returns>
        public static List<Parcel> GetParcelList()
        {
            List<Parcel> MyParcels = new List<Parcel>(DataSource.Parcels);

            return MyParcels;
        }

        /// <summary>
        /// Return List of non associate Parcels.
        /// </summary>
        /// <returns> List of non associate Parcels </returns>
        public static List<Parcel> GetNonAssociateParcelList()
        {
            List<Parcel> MyParcels = new List<Parcel>();

            foreach (var parcel in DataSource.Parcels)
            {
                if (parcel.DroneId == 0)
                    MyParcels.Add(parcel);
            }
            return MyParcels;
        }

        /// <summary>
        /// Return List of Stations with available charging slot.
        /// </summary>
        /// <returns> List of Stations with available charging slot </returns>
        public static List<Station> GetStationsWithAvailableChargingSlots()
        {
            List<Station> MyStations = new List<Station>();
            int stationIndex = 0;
            while (stationIndex < DataSource.Config.StationIndex)
            {
                if (DataSource.Stations[stationIndex].ChargeSlots > 0)
                {
                    MyStations.Add(DataSource.Stations[stationIndex]);
                }
                stationIndex++;
            }
            return MyStations;
        }


        //------------------------ BONUS FUNCTIONS ---------------------------//

        /// <summary>
        /// Return string of sexagesimal presentation.
        /// </summary>
        /// <param name="decimalNumber"></param>
        /// <returns> String of sexagesimal presentation </returns>
        public static string SexagesimalPresentation(double decimalNumber)
        {
            int degrees, minutes1, seconds;
            double minutes2;

            degrees = (int)decimalNumber;

            minutes2 = (decimalNumber - degrees) * 60;
            minutes1 = (int)minutes2;

            seconds = (int)((minutes2 - minutes1) * 60);

            return $"{degrees}°{minutes1}'{seconds}\"";
        }

        /// <summary>
        /// Convert the receive angle to radian degree
        /// </summary>
        /// <param name="angleIn10thofDegree"></param>
        /// <returns> Radian degree (double)</returns>
        static double ToRadians(double angleIn10thofDegree)
        {
            //Angle in 10th of a degree
            return (angleIn10thofDegree * Math.PI) / 180;
        } 

        /// <summary>
        /// Calcute and return the distance between two points on earth (using longitude and lattitude).
        /// </summary>
        /// <param name="lattitude1">Lattitude point A</param>
        /// <param name="lattitude2">Lattitude point B</param>
        /// <param name="longitude1">Longitude point A</param>
        /// <param name="longitude2">Longitude point B</param>
        /// <returns> Distance between the points </returns>
        public static double Distance(double lattitude1, double lattitude2, double longitude1, double longitude2)
        {
            //Convert longitude and lattitude values to radians.
            longitude1 = ToRadians(longitude1);
            longitude2 = ToRadians(longitude2);
            lattitude1 = ToRadians(lattitude1);
            lattitude2 = ToRadians(lattitude2);

            //Haversine formula.
            double distanceLongitude = longitude2 - longitude1;
            double distanceLattitude = lattitude2 - lattitude1;
            //a = (sin(distanceLattitude / 2)) ^ 2 + (cos(lattitude1) * cos(lattitude2)) * (sin(distanceLongitude / 2)) ^ 2.
            double a = Math.Pow(Math.Sin(distanceLattitude / 2), 2) +
                       Math.Cos(lattitude1) * Math.Cos(lattitude2) *
                       Math.Pow(Math.Sin(distanceLongitude / 2), 2);

            //c = 2\arcsin(sqrt(a)).
            double c = 2 * Math.Asin(Math.Sqrt(a));

            //Radius of earth in kilometers.
            double r = 6371;

            //Calculate the result.
            return (c * r);
        }
    }
}




