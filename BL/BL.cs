using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    /// <summary>
    /// The Bissnes Layer wich responsible for the logic in the project refer to the Layer Model.
    /// </summary>
    public partial class BL : IBL.IBL
    {
        //Create instance of dalObject for reference to DAL.
        internal static IDAL.IDal dalObject = new DalObject.DalObject();

        List<DroneToList> droneToLists = new();

        /// <summary>
        /// C-tor of BL.
        /// Initiate DroneToList list.
        /// </summary>
        /// <exception cref="NoBaseStationToAssociateDroneToException"> Thrown if there are no stations 
        ///                                                             with available charge-slots </exception>
        public BL()
        {
            Random r = new();

            //Import from DAL the 4 weight categories and the charging rate in seperate varibales.
            double[] tempArray = dalObject.ElectricityUseRequest();
            double dorneChargingRate = tempArray[4];
            double[] electricityUse = new double[4];

            for (int i = 0; i < tempArray.Length - 1; i++)
            {
                electricityUse[i] = tempArray[i];
            }

            //Initialize all the drones (if any).
            IEnumerable<IDAL.DO.Drone> dalDronesList = dalObject.GetDroneList();
            if (dalDronesList.Count() > 0)
            {
                foreach (var drone in dalDronesList)
                {
                    DroneToList newDrone = new();
                    //Take all the information from the drone entity in DAL and set it in the DroneToList object (newDrone).
                    newDrone.Id = drone.Id;
                    newDrone.Model = drone.Model;
                    newDrone.MaxWeight = (WeightCategories)drone.MaxWeight;

                    //Get the rest of the information and fill the other fileds in the new DroneToList object.
                    //Set DroneStatus, DeliveryParcelId and CurrentLocation (and BatteryStatus - if needed) fileds.
                    bool isInShipment = false;
                    foreach (var parcel in dalObject.GetParcelList())
                    {
                        if (parcel.DroneId == newDrone.Id && parcel.Delivered == null)
                        {
                            newDrone.DroneStatus = DroneStatuses.Shipment;
                            newDrone.DeliveryParcelId = parcel.Id;

                            if (parcel.PickedUp == null)
                            {
                                try
                                {
                                    int baseStationId = FindNearestBaseStationByCustomerId(parcel.SenderId);

                                    double baseStationLatitude = dalObject.FindStationById(baseStationId).Latitude;
                                    double baseStationLongitude = dalObject.FindStationById(baseStationId).Longitude;

                                    //Update the location coordinates of the droneToList same as the closest base station to the sender location.
                                    newDrone.CurrentLocation.Latitude = baseStationLatitude;
                                    newDrone.CurrentLocation.Longitude = baseStationLongitude;
                                }
                                catch (ObjectNotFoundException)
                                {
                                    throw new NoBaseStationToAssociateDroneToException();
                                }
                            }
                            else
                            {
                                double senderLatitude = dalObject.FindCustomerById(parcel.SenderId).Lattitude;
                                double senderLongitude = dalObject.FindCustomerById(parcel.SenderId).Longitude;

                                //Update the location coordinates of the droneToList same as the sender location.
                                newDrone.CurrentLocation.Latitude = senderLatitude;
                                newDrone.CurrentLocation.Longitude = senderLongitude;
                            }

                            double minimumOfBattery = FindMinPowerSuply(newDrone, parcel.TargetId);
                            newDrone.BatteryStatus = r.Next((int)minimumOfBattery, 101);
                            isInShipment = true;
                        }
                    }
                    if (!isInShipment)
                    {
                        newDrone.DroneStatus = (DroneStatuses)r.Next(2);
                    }

                    if (newDrone.DroneStatus == DroneStatuses.Maintenance)
                    {
                        List<IDAL.DO.Station> stationsList = (List<IDAL.DO.Station>)dalObject.GetBaseStationList();
                        int size = stationsList.Count();
                        int index = r.Next(0, size);
                        IDAL.DO.Station station = stationsList[index];

                        newDrone.CurrentLocation.Latitude = station.Latitude;
                        newDrone.CurrentLocation.Longitude = station.Longitude;
                        newDrone.BatteryStatus = r.Next(0, 21);
                        dalObject.AddDroneCharge(newDrone.Id, station.Id);
                    }
                    else if (newDrone.DroneStatus == DroneStatuses.Available)
                    {
                        List<int> CustomerIdList = new();

                        foreach (var parcel in dalObject.GetParcelList())
                        {
                            if (parcel.Delivered != null)
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

        //----------------------- FIND FUNCTIONS -----------------------//

        /// <summary>
        /// Find the nearest base-station to the customer by customer id.
        /// </summary>
        /// <param name="customerId">Customer Id </param>
        /// <returns> Id of the nearest base-station to the recieven coustomer </returns>
        /// <exception cref="InvalidInputException">Thrown if the receiven id is invalid</exception>
        int FindNearestBaseStationByCustomerId(int customerId)
        {
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("Id");

            double minDistance = double.MaxValue;
            int nearestBaseStationId = 0;

            //Get the Sender location coordinates.
            double customerLatitude = dalObject.FindCustomerById(customerId).Lattitude;
            double customerLongitude = dalObject.FindCustomerById(customerId).Longitude;
            if (dalObject.GetBaseStationList().Count() > 0)
            {
                foreach (var baseStation in dalObject.GetBaseStationList())
                {
                    //Calculate the distance between the sender and the current base station.
                    double distance = dalObject.Distance(baseStation.Latitude, customerLatitude, baseStation.Longitude, customerLongitude);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestBaseStationId = baseStation.Id;
                    }
                }
            }
            else
            {
                throw new ObjectNotFoundException();
            }
            return nearestBaseStationId;
        }

        /// <summary>
        /// Find the nearest base-station with at least one available charging-slot by id.
        /// </summary>
        /// <param name="location"> Location information to calculate the distance </param>
        /// <returns> Id of the nearest base-station with at least one available charge-slot if found </returns>
        /// <excption cref="ObjectNotFoundException"> Thrown if there are no stations with available charging slots </excption>
        int FindNearestBaseStationWithAvailableChargingSlots(Location location)
        {
            double minDistance = double.MaxValue;
            int nearestBaseStationID = 0;

            IEnumerable<IDAL.DO.Station> stations = dalObject.GetStations(x => x.ChargeSlots > 0);
            if (stations.Count() == 0) throw new ObjectNotFoundException("Stations with available charging slots");

            foreach (var myBaseStation in stations)
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
        /// and go to the nearest base-station for charging.
        /// </summary>
        /// <param name="drone"> Drone object to calculate his needed minimum power suply</param>
        /// <param name="customerId"> Target id to calculate the needed power suply for the drone to get there</param>
        /// <returns>The minimum needed power suply to go to the target and to base station for charging if found,
        ///              otherwise retun 0 </returns>
        /// <exception cref="InvalidInputException"> Thrown if drone id or customer id is invalid </exception>
        double FindMinPowerSuply(DroneToList drone, int customerId)
        {
            if (drone.Id < 1000 || drone.Id > 10000) throw new InvalidInputException("drone");
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("customer Id");

            //Step 1: Find the distance between the drone current location and the destination location.
            Location location = new();
            location.Latitude = dalObject.FindCustomerById(customerId).Lattitude;
            location.Longitude = dalObject.FindCustomerById(customerId).Longitude;
            double distance1 = dalObject.Distance(drone.CurrentLocation.Latitude, location.Latitude, drone.CurrentLocation.Longitude, location.Longitude);

            //Step 2: Find the minimal needed power suply to go to the destination.
            double suply1 = 0;
            switch (drone.MaxWeight)
            {
                case WeightCategories.Heavy:
                    //Available-0, Light-1, Intermediate-2, Heavy-3, DroneChargingRate-4.
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
            //        and calculate the distance between the cutomer and the base-station.
            int closestBaseStationID = FindNearestBaseStationWithAvailableChargingSlots(location);
            if (closestBaseStationID != 0)
            {
                double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationID).Latitude;
                double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationID).Longitude;
                double distance2 = dalObject.Distance(location.Latitude, nearestBaseStationLatitude, location.Longitude, nearestBaseStationLongitude);

                //Step 4: Find the minimal needed power suply to go to the destination.
                double suply2 = distance2 / dalObject.ElectricityUseRequest()[0];

                //Step 5: Calculate the final needed power suply.
                double minBatteryValue = suply1 + suply2;

                return minBatteryValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Find the minimum needed power suply to go to the nearest base-station 
        ///          (with at least one available charging slot) for charging.
        /// </summary>
        /// <param name="drone"> Drone object to caclculate the minimum needed power suply </param>
        /// <returns> Minimum needed power suply </returns>
        /// <exception cref="InvalidInputException">Thrown if drone id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if there are no station with such id </exception>
        double FindMinPowerSuplyForCharging(DroneToList drone)
        {
            if (drone.Id < 1000 || drone.Id > 10000) throw new InvalidInputException("drone");

            //Step 1: Find the nearest base-station with available charge-slot and calcuolate the needed power suply.
            int closestBaseStationId = FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
            if (closestBaseStationId == 0) throw new ObjectNotFoundException("base-Station");

            double nearestBaseStationLatitude = dalObject.FindStationById(closestBaseStationId).Latitude;
            double nearestBaseStationLongitude = dalObject.FindStationById(closestBaseStationId).Longitude;
            double distance = dalObject.Distance(drone.CurrentLocation.Latitude, nearestBaseStationLatitude, drone.CurrentLocation.Longitude, nearestBaseStationLongitude);

            //Step 2: Find the minimal needed power suply to go to the destination.
            double minBatteryValue = distance / dalObject.ElectricityUseRequest()[0];
            return minBatteryValue;
        }

        /// <summary>
        /// Get the minimun power of battery for distance between the drone and the destination.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Minimun power of battery for distance between the drone and the dastination </returns>
        /// /// <exception cref="InvalidInputException"> Thrown if drone id or customer id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if there are no drone with such id </exception>
        double FindMinPowerSuplyForDistanceBetweenDroneToTarget(int droneId, int customerId)
        {
            if (droneId < 1000 || droneId > 10000) throw new InvalidInputException("drone id");
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("customer Id");


            DroneToList myDrone = droneToLists.Find(x => x.Id == droneId);
            if (myDrone.Id != droneId) throw new ObjectNotFoundException("drone");

            Customer myCustomer = FindCustomerByIdBL(customerId);

            double myDistantce = dalObject.Distance(myDrone.CurrentLocation.Latitude, myCustomer.Location.Latitude,
                myDrone.CurrentLocation.Longitude, myCustomer.Location.Longitude);

            double mySuply = 0;

            switch (myDrone.MaxWeight)
            {
                case WeightCategories.Heavy:
                    //Available-0, Light-1, Intermediate-2, Heavy-3, DroneChargingRate-4.
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
        /// Get the minimun power of battery for all the jurney of the drone.
        /// </summary>
        /// <param name="droneId"> Drone Id </param>
        /// <param name="customerId"> Customer Id </param>
        /// <returns> Minimun power of battery for all the jurney of the drone </returns>
        /// /// <exception cref="InvalidInputException"> Thrown if drone id or customer id is invalid </exception>
        /// <exception cref="ObjectNotFoundException"> Thrown if there are no drone with such id </exception>
        double FindMinSuplyForAllPath(int droneId, int customerId)
        {
            if (droneId < 1000 || droneId > 10000) throw new InvalidInputException("drone id");
            if (customerId < 100000000 || customerId >= 1000000000) throw new InvalidInputException("customer Id");

            DroneToList myDrone = droneToLists.Find(x => x.Id == droneId);
            if (myDrone.Id != droneId) throw new ObjectNotFoundException("drone");

            double minSuply1 = FindMinPowerSuplyForDistanceBetweenDroneToTarget(myDrone.Id, customerId);

            Customer myTarget = FindCustomerByIdBL(customerId);
            myDrone.CurrentLocation = myTarget.Location;

            double minSuply2 = FindMinPowerSuply(myDrone, customerId);
            return minSuply1 + minSuply2;
        }
        private DroneCharge FindDroneChargeByDroneIdBL(int droneId)
        {
            IDAL.DO.DroneCharge droneCharge = dalObject.FindDroneChargeByDroneId(droneId);
            DroneCharge newDroneCharge = new();
            newDroneCharge.DroneId = droneCharge.DroneId;
            newDroneCharge.ChargeTime = droneCharge.ChargeTime;
            return newDroneCharge;
        }
    }
}
