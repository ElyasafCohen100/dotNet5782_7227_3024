using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace IBL :)
{
    partial class BL : IBL
    {
        public BL()
        {
            //Create instance of dalObject for reference to DAL
            IDAL.IDal dalObject = new DalObject.DalObject();

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
                BO.DroneToList newDrone = new();
                //Take all the information from the drone entity in DAL
                newDrone.Id = drone.Id;
                newDrone.Model = drone.Model;
                newDrone.MaxWeight = (WeightCategories)drone.MaxWeight;

                // Get the rest of the information and fill the other fileds in the new DroneToList entity
                foreach (var parcel in dalObject.GetParcelList())
                {
                    if (parcel.DroneId == newDrone.Id && parcel.Delivered == DateTime.MinValue)
                    {
                        newDrone.DroneStatus = DroneStatuses.Shipment;

                        if (parcel.PickedUp == DateTime.MinValue)
                        {
                            int baseStationId = FindNearestBaseStationByCustomerId(parcel.SenderId);

                            double senderLatitude = dalObject.FindStationById(baseStationId).Lattitude;
                            double senderLongitude = dalObject.FindStationById(baseStationId).Longitude;

                            // Update the location coordinates of the droneToList to the closest base station to the sender location
                            newDrone.CurrentLocation.Lattitude = senderLatitude;
                            newDrone.CurrentLocation.Longitude = senderLongitude;
                        }
                        else if (parcel.Delivered == DateTime.MinValue)
                        {
                            // Get the Sender location coordinates
                            double senderLatitude = dalObject.FindCustomerById(parcel.SenderId).Lattitude;
                            double senderLongitude = dalObject.FindCustomerById(parcel.SenderId).Longitude;

                            newDrone.CurrentLocation.Lattitude = senderLatitude;
                            newDrone.CurrentLocation.Longitude = senderLongitude;
                        }
                        double minimumOfBattery = FindMinPowerSuply(newDrone, parcel.TargetId);


                        newDrone.BatteryStatus = r.Next((int)minimumOfBattery, 101);
                    }
                    else  //if the drone isn't in shipment status and the parcel may be delivered.
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

                    newDrone.BatteryStatus = FindMinPowerSuply(newDrone);
                }

                droneToLists.Add(newDrone);

            }
        }

        /// <summary>
        /// Find the nearest base-station by cousomer id
        /// </summary>
        /// <param name="customerId">The id of the costumer</param>
        /// <returns>The id of the nearest base-station to the reciever coustomer as parameter</returns>
        public int FindNearestBaseStationByCustomerId(int customerId)
        {
            IDAL.IDal dalObject = new DalObject.DalObject();
            double minDistance = double.MaxValue;
            int closetBaseStationId = 0;
            foreach (var baseStation in dalObject.GetBaseStationList())
            {
                // Get the Sender location coordinates
                double senderLatitude = dalObject.FindCustomerById(customerId).Lattitude;
                double senderLongitude = dalObject.FindCustomerById(customerId).Longitude;

                // calculate the distance between the sender and the current base 
                double currentDistance = dalObject.Distance(baseStation.Lattitude, senderLatitude, baseStation.Longitude, senderLongitude);

                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    closetBaseStationId = baseStation.Id;
                }
            }
            return closetBaseStationId;
        }

        /// <summary>
        /// Get the nearest base-station with available charging-slot by id
        /// </summary>
        /// <param name="sourceLatitude">The latitude of the source (from where we want to the distance)</param>
        /// <param name="sourceLongitude">The longitude of the source (from where we want to the distance)</param>
        /// <returns>The id of the nearest base-station with at least one available charge-slot</returns>
        public int GetNearestBaseStationWithAvailableChargingSlotsById(double sourceLatitude, double sourceLongitude)
        {
            IDAL.IDal dalObject = new DalObject.DalObject();
            //find the closest base stasion with at least one available charge slot
            double minDistance2 = double.MaxValue;
            int closestBaseStationID = 0;
            foreach (var myBaseStation in dalObject.GetStationsWithAvailableChargingSlots())
            {
                double currentDistance2 = dalObject.Distance(myBaseStation.Lattitude, sourceLatitude, myBaseStation.Longitude, sourceLongitude);

                if (currentDistance2 < minDistance2)
                {
                    closestBaseStationID = myBaseStation.Id;
                    minDistance2 = currentDistance2;
                }
            }
            return closestBaseStationID;
        }

        /// <summary>
        /// Find the minimal needed power suply for a given drone to go to the destination and go to nearest base-station
        /// for charging
        /// </summary>
        /// <param name="drone">The drone to calculate his minimum power suply</param>
        /// <param name="targetCustomerId">The destination to calculate the needed power suply to get there</param>
        /// <returns>The minimum needed power suply to go to the destination</returns>
        public double FindMinPowerSuply(BO.DroneToList drone, int targetCustomerId)
        {
            IDAL.IDal dalObject = new DalObject.DalObject();
            //Step 1: Find the distance between the drone current location and the destination location
            double destinationLatitude = dalObject.FindCustomerById(targetCustomerId).Lattitude;
            double destinationLongitude = dalObject.FindCustomerById(targetCustomerId).Longitude;
            double distance1 = dalObject.Distance(drone.CurrentLocation.Lattitude, destinationLatitude, drone.CurrentLocation.Longitude, destinationLongitude);

            //Step 2: Find the minimal needed power suply to go to the destination
            double suply1 = 0;
            switch (drone.MaxWeight)
            {
                case BO.WeightCategories.Heavy:
                    //Available-0, Light-1, Intermediate-2, Heavy-3, DroneChargingRate-4
                    suply1 = distance1 / dalObject.ElectricityUseRequest()[3];
                    break;
                case BO.WeightCategories.Intermediate:
                    suply1 = distance1 / dalObject.ElectricityUseRequest()[2];
                    break;
                case BO.WeightCategories.Light:
                    suply1 = distance1 / dalObject.ElectricityUseRequest()[1];
                    break;
            }

            //Step 3: Find the nearest base-station with available charge-slot and calcuolate the needed power suply
            int closestBaseStationID = GetNearestBaseStationWithAvailableChargingSlotsById(destinationLatitude, destinationLongitude);

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Lattitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
            double distance2 = dalObject.Distance(destinationLatitude, nearestBaseStationLatitude, destinationLongitude, nearestBaseStationLongitude);

            //Step 4: Find the minimal needed power suply to go to the destination
            double suply2 = distance2 / dalObject.ElectricityUseRequest()[0];

            //Step 5: Calculate the final needed power suply
            double minBatteryValue = suply1 + suply2;

            return minBatteryValue;
        }

        public double FindMinPowerSuply(BO.DroneToList drone)
        {
            IDAL.IDal dalObject = new DalObject.DalObject();

            //Step 3: Find the nearest base-station with available charge-slot and calcuolate the needed power suply
            int closestBaseStationID = GetNearestBaseStationWithAvailableChargingSlotsById(drone.CurrentLocation.Lattitude, drone.CurrentLocation.Longitude);

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Lattitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
            double distance = dalObject.Distance(drone.CurrentLocation.Lattitude, nearestBaseStationLatitude, drone.CurrentLocation.Longitude, nearestBaseStationLongitude);

            //Step 4: Find the minimal needed power suply to go to the destination
            double minBatteryValue = distance / dalObject.ElectricityUseRequest()[0];

            return minBatteryValue;
        }
    }
}
