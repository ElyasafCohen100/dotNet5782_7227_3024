using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    /// <summary>
    /// The Bissnes Layer wich responsible for the logic in the project refer to the Layer Model
    /// </summary>
    public partial class BL : IBL.IBL
    {
        //Create instance of dalObject for reference to DAL
        internal static IDAL.IDal dalObject = new DalObject.DalObject();

        List<DroneToList> droneToLists = new();

        /// <summary>
        /// c-tor of BL.
        /// Initiate DroneToList list.
        /// </summary>
        public BL()
        {
            Random r = new();

            // Import from DAL the 4 weight categories and the charging rate in seperate varibales
            double[] tempArray = dalObject.ElectricityUseRequest();
            double dorneChargingRate = tempArray[4];
            double[] electricityUse = new double[4];

            for (int i = 0; i < tempArray.Length - 1; i++)
            {
                electricityUse[i] = tempArray[i];
            }

            // Initialize all the drones (if any)
            IEnumerable<IDAL.DO.Drone> dalDronesList = dalObject.GetDroneList();
            if (dalDronesList.Count() > 0)
            {
                foreach (var drone in dalDronesList)
                {
                    DroneToList newDrone = new();
                    //Take all the information from the drone entity in DAL and set it in the DroneToList object (newDrone)
                    newDrone.Id = drone.Id;
                    newDrone.Model = drone.Model;
                    newDrone.MaxWeight = (WeightCategories)drone.MaxWeight;

                    // Get the rest of the information and fill the other fileds in the new DroneToList object
                    // Set DroneStatus, DeliveryParcelId and CurrentLocation (and BatteryStatus - if needed) fileds
                    foreach (var parcel in dalObject.GetParcelList())
                    {
                        if (parcel.DroneId == newDrone.Id && parcel.Delivered == DateTime.MinValue)
                        {
                            newDrone.DroneStatus = DroneStatuses.Shipment;
                            newDrone.DeliveryParcelId = parcel.Id;

                            if (parcel.PickedUp == DateTime.MinValue)
                            {
                                int baseStationId = FindNearestBaseStationByCustomerId(parcel.SenderId);

                                double baseStationLatitude = dalObject.FindStationById(baseStationId).Latitude;
                                double baseStationLongitude = dalObject.FindStationById(baseStationId).Longitude;

                                // Update the location coordinates of the droneToList same as the closest base station to the sender location
                                newDrone.CurrentLocation.Latitude = baseStationLatitude;
                                newDrone.CurrentLocation.Longitude = baseStationLongitude;
                            }
                            else
                            {
                                double senderLatitude = dalObject.FindCustomerById(parcel.SenderId).Lattitude;
                                double senderLongitude = dalObject.FindCustomerById(parcel.SenderId).Longitude;

                                // Update the location coordinates of the droneToList same as the sender location
                                newDrone.CurrentLocation.Latitude = senderLatitude;
                                newDrone.CurrentLocation.Longitude = senderLongitude;
                            }

                            double minimumOfBattery = FindMinPowerSuply(newDrone, parcel.TargetId);
                            newDrone.BatteryStatus = r.Next((int)minimumOfBattery, 101);
                        }
                        else
                        {
                            newDrone.DroneStatus = (DroneStatuses)r.Next(2);
                        }
                    }

                    if (newDrone.DroneStatus == DroneStatuses.Maintenance)
                    {
                        List<IDAL.DO.Station> List = (List<IDAL.DO.Station>)dalObject.GetBaseStationList();
                        int size = List.Count();
                        int index = r.Next(0, size);

                        newDrone.CurrentLocation = new();
                        newDrone.CurrentLocation.Latitude = List[index].Latitude;
                        newDrone.CurrentLocation.Longitude = List[index].Longitude;

                        newDrone.BatteryStatus = r.Next(0, 21);
                    }
                    else if (newDrone.DroneStatus == DroneStatuses.Available)
                    {
                        List<int> CustomerIdList = new();

                        foreach (var parcel in dalObject.GetParcelList())
                        {
                            if (parcel.Delivered != DateTime.MinValue)
                            {
                                CustomerIdList.Add(parcel.TargetId);
                            }
                        }

                        int size = CustomerIdList.Count();
                        if (size > 0)
                        {
                            int index = r.Next(0, size);
                            int targetId = CustomerIdList[index];
                            IDAL.DO.Customer target = dalObject.FindCustomerById(targetId);
                            newDrone.CurrentLocation.Latitude = target.Lattitude;
                            newDrone.CurrentLocation.Longitude = target.Longitude;

                            newDrone.BatteryStatus = FindMinPowerSuplyForCharging(newDrone);
                        }
                        else
                        {
                            try
                            {
                                StationToList baseStation = ViewStationsWithAvailableChargingSlotstBL().First();
                                IDAL.DO.Station newStation = dalObject.FindStationById(baseStation.Id);

                                newDrone.CurrentLocation.Longitude = newStation.Longitude;
                                newDrone.CurrentLocation.Latitude = newStation.Latitude;
                                newDrone.BatteryStatus = 100;

                                baseStation.AvailableChargeSlots--;
                                baseStation.NotAvailableChargeSlots++;

                                dalObject.UpdateDroneToCharging(newDrone.Id, baseStation.Id);
                            }
                            catch (InvalidOperationException)
                            {
                                throw new NoBaseStationToAssociateDroneToException();
                            }
                        }
                    }

                    droneToLists.Add(newDrone);
                }
            }
        }

        //----------------------------------- FIND FUNCTIONS --------------------------------------------//

        /// <summary>
        /// Find the nearest base-station to the customer by customer id
        /// </summary>
        /// <param name="customerId">The id of the costumer</param>
        /// <returns>The id of the nearest base-station to the recieven coustomer as parameter</returns>
        int FindNearestBaseStationByCustomerId(int customerId)
        {
            IDAL.IDal dalObject = new DalObject.DalObject();
            double minDistance = double.MaxValue;
            int nearestBaseStationId = 0;

            // Get the Sender location coordinates
            double customerLatitude = dalObject.FindCustomerById(customerId).Lattitude;
            double customerLongitude = dalObject.FindCustomerById(customerId).Longitude;

            foreach (var baseStation in dalObject.GetBaseStationList())
            {
                // calculate the distance between the sender and the current base 
                double distance = dalObject.Distance(baseStation.Latitude, customerLatitude, baseStation.Longitude, customerLongitude);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestBaseStationId = baseStation.Id;
                }
            }
            return nearestBaseStationId;
        }

        /// <summary>
        /// Find the nearest base-station with at least one available charging-slot by id
        /// </summary>
        /// <param name="location">The location information to calculate the distance</param>
        /// <returns>The id of the nearest base-station with at least one available charge-slot</returns>
        int FindNearestBaseStationWithAvailableChargingSlots(Location location)
        {
            IDAL.IDal dalObject = new DalObject.DalObject();
            double minDistance = double.MaxValue;
            int nearestBaseStationID = 0;

            foreach (var myBaseStation in dalObject.GetStationsWithAvailableChargingSlots())
            {
                double distance = dalObject.Distance(myBaseStation.Latitude, location.Latitude, myBaseStation.Longitude, location.Longitude);

                if (distance < minDistance)
                {
                    nearestBaseStationID = myBaseStation.Id;
                    minDistance = distance;
                }
            }
            return nearestBaseStationID;
        }

        /// <summary>
        /// Find the minimal needed power suply for a given drone to deliver the parcel 
        /// and go to the nearest base-station for charging
        /// </summary>
        /// <param name="drone">The drone to calculate his needed minimum power suply</param>
        /// <param name="targetId">The target id to calculate the needed power suply for the drone to get there</param>
        /// <returns>The minimum needed power suply to go to the target</returns>
        double FindMinPowerSuply(DroneToList drone, int targetId)
        {
            //Step 1: Find the distance between the drone current location and the destination location
            Location location = new();
            location.Latitude = dalObject.FindCustomerById(targetId).Lattitude;
            location.Longitude = dalObject.FindCustomerById(targetId).Longitude;
            double distance1 = dalObject.Distance(drone.CurrentLocation.Latitude, location.Latitude, drone.CurrentLocation.Longitude, location.Longitude);

            //Step 2: Find the minimal needed power suply to go to the destination
            double suply1 = 0;
            switch (drone.MaxWeight)
            {
                case WeightCategories.Heavy:
                    //Available-0, Light-1, Intermediate-2, Heavy-3, DroneChargingRate-4
                    suply1 = distance1 / dalObject.ElectricityUseRequest()[3];
                    break;
                case WeightCategories.Average:
                    suply1 = distance1 / dalObject.ElectricityUseRequest()[2];
                    break;
                case WeightCategories.Light:
                    suply1 = distance1 / dalObject.ElectricityUseRequest()[1];
                    break;
            }

            //Step 3: Find the nearest base-station with available charge-slot and calcuolate the needed power suply 
            //        and calculate the distance between the cutomer and the base-station
            int closestBaseStationID = FindNearestBaseStationWithAvailableChargingSlots(location);

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Latitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
            double distance2 = dalObject.Distance(location.Latitude, nearestBaseStationLatitude, location.Longitude, nearestBaseStationLongitude);

            //Step 4: Find the minimal needed power suply to go to the destination
            double suply2 = distance2 / dalObject.ElectricityUseRequest()[0];

            //Step 5: Calculate the final needed power suply
            double minBatteryValue = suply1 + suply2;

            return minBatteryValue;
        }

        /// <summary>
        /// Find the minimum needed power suply to go to the nearest base-station (with at least one available charging slot) for charging
        /// </summary>
        /// <param name="drone">The drone to caclculate the minimum needed power suply</param>
        /// <returns>The minimum needed power suply</returns>
        double FindMinPowerSuplyForCharging(DroneToList drone)
        {
            //Step 1: Find the nearest base-station with available charge-slot and calcuolate the needed power suply
            int closestBaseStationID = FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Latitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
            double distance = dalObject.Distance(drone.CurrentLocation.Latitude, nearestBaseStationLatitude, drone.CurrentLocation.Longitude, nearestBaseStationLongitude);

            //Step 2: Find the minimal needed power suply to go to the destination
            double minBatteryValue = distance / dalObject.ElectricityUseRequest()[0];

            return minBatteryValue;
        }

        /// <summary>
        /// get the minimun power of battery for distance between the drone and the dastination
        /// </summary>
        /// <param name="droneId"> ID of drone </param>
        /// <param name="targetId"> ID of the client </param>
        /// <returns> return the minimun power of battery for distance between the drone and the dastination </returns>
        double FindMinPowerSuplyForDistanceBetweenDroneToTarget(int droneId, int targetId)
        {
            DroneToList myDrone = droneToLists.Find(x => x.Id == droneId);
            Customer myCustomer = FindCustomerByIdBL(targetId);

            double myDistantce = dalObject.Distance(myDrone.CurrentLocation.Latitude, myCustomer.Location.Latitude,
                myDrone.CurrentLocation.Longitude, myCustomer.Location.Longitude);

            double mySuply = 0;

            switch (myDrone.MaxWeight)
            {
                case WeightCategories.Heavy:
                    //Available-0, Light-1, Intermediate-2, Heavy-3, DroneChargingRate-4
                    mySuply = myDistantce / dalObject.ElectricityUseRequest()[3];
                    break;
                case WeightCategories.Average:
                    mySuply = myDistantce / dalObject.ElectricityUseRequest()[2];
                    break;
                case WeightCategories.Light:
                    mySuply = myDistantce / dalObject.ElectricityUseRequest()[1];
                    break;
            }

            return mySuply;
        }

        /// <summary>
        /// get the minimun power of battery for all the jurney of the drone
        /// </summary>
        /// <param name="droneId"> ID of drone </param>
        /// <param name="targetId"> ID of the client </param>
        /// <returns> return the minimun power of battery for all the jurney of the drone </returns>
        double FindMinSuplyForAllPath(int droneId, int targetId)
        {
            DroneToList myDrone = droneToLists.Find(x => x.Id == droneId);

            double minSuply1 = FindMinPowerSuplyForDistanceBetweenDroneToTarget(myDrone.Id, targetId);

            Customer myTarget = FindCustomerByIdBL(targetId);
            myDrone.CurrentLocation = myTarget.Location;

            double minSuply2 = FindMinPowerSuply(myDrone, targetId);
            return minSuply1 + minSuply2;
        }
    }
}
