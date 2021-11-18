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

        /// <summary>
        /// c-tor of BL.
        /// Initiate DroneToList list.
        /// </summary>
        public BL()
        {

            // Import from DAL the 4 weight categiries and the charging rate in seperate varibales
            double[] tempArray = dalObject.ElectricityUseRequest();
            double dorneChargingRate = tempArray[4];
            double[] electricityUse = new double[4];
            Random r = new();

            for (int i = 0; i < tempArray.Length - 1; i++)
            {
                electricityUse[i] = tempArray[i];
            }

            List<DroneToList> droneToLists = new();

            // Initialize all the drones
            foreach (var drone in dalObject.GetDroneList())
            {
                DroneToList newDrone = new();
                //Take all the information from the drone entity in DAL and set it in the DroneToList object (newDrone)
                newDrone.Id = drone.Id;
                newDrone.Model = drone.Model;
                newDrone.MaxWeight = (WeightCategories)drone.MaxWeight;

                // Get the rest of the information and fill the other fileds in the new DroneToList object
                //Set DroneStatus, DeliveryParcelId and CurrentLocation (and BatteryStatus - if needed) fileds
                foreach (var parcel in dalObject.GetParcelList())
                {
                    if (parcel.DroneId == newDrone.Id && parcel.Delivered == DateTime.MinValue)
                    {
                        newDrone.DroneStatus = DroneStatuses.Shipment;
                        newDrone.DeliveryParcelId = parcel.Id;

                        if (parcel.PickedUp == DateTime.MinValue)
                        {
                            int baseStationId = FindNearestBaseStationByCustomerId(parcel.SenderId);

                            double baseStationLatitude = dalObject.FindStationById(baseStationId).Lattitude;
                            double baseStationLongitude = dalObject.FindStationById(baseStationId).Longitude;

                            // Update the location coordinates of the droneToList same as the closest base station to the sender location
                            newDrone.CurrentLocation.Lattitude = baseStationLatitude;
                            newDrone.CurrentLocation.Longitude = baseStationLongitude;
                        }
                        else
                        {
                            double senderLatitude = dalObject.FindCustomerById(parcel.SenderId).Lattitude;
                            double senderLongitude = dalObject.FindCustomerById(parcel.SenderId).Longitude;

                            // Update the location coordinates of the droneToList same as the sender location
                            newDrone.CurrentLocation.Lattitude = senderLatitude;
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

                    newDrone.CurrentLocation.Lattitude = List[index].Lattitude;
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
                    int index = r.Next(0, size);
                    int targetId = CustomerIdList[index];
                    IDAL.DO.Customer target = dalObject.FindCustomerById(targetId);
                    newDrone.CurrentLocation.Lattitude = target.Lattitude;
                    newDrone.CurrentLocation.Longitude = target.Longitude;

                    newDrone.BatteryStatus = FindMinPowerSuplyForCharging(newDrone);
                }
                droneToLists.Add(newDrone);
            }
        }

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
                double distance = dalObject.Distance(baseStation.Lattitude, customerLatitude, baseStation.Longitude, customerLongitude);

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
                double distance = dalObject.Distance(myBaseStation.Lattitude, location.Lattitude, myBaseStation.Longitude, location.Longitude);

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
            IDAL.IDal dalObject = new DalObject.DalObject();
            //Step 1: Find the distance between the drone current location and the destination location
            Location location = new();
            location.Lattitude = dalObject.FindCustomerById(targetId).Lattitude;
            location.Longitude = dalObject.FindCustomerById(targetId).Longitude;
            double distance1 = dalObject.Distance(drone.CurrentLocation.Lattitude, location.Lattitude, drone.CurrentLocation.Longitude, location.Longitude);

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

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Lattitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
            double distance2 = dalObject.Distance(location.Lattitude, nearestBaseStationLatitude, location.Longitude, nearestBaseStationLongitude);

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
            IDAL.IDal dalObject = new DalObject.DalObject();

            //Step 1: Find the nearest base-station with available charge-slot and calcuolate the needed power suply
            int closestBaseStationID = FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Lattitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
            double distance = dalObject.Distance(drone.CurrentLocation.Lattitude, nearestBaseStationLatitude, drone.CurrentLocation.Longitude, nearestBaseStationLongitude);

            //Step 2: Find the minimal needed power suply to go to the destination
            double minBatteryValue = distance / dalObject.ElectricityUseRequest()[0];

            return minBatteryValue;
        }
    }
}
