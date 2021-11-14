using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace IBL
{
    partial class BL : IBL
    {
        public BL()
        {
            IDAL.IDal dalObject = new DalObject.DalObject();
            double[] tempArray = dalObject.ElectricityUseRequest();
            double dorneChargingRate = tempArray[4];
            double[] electricityUse = new double[4];
            for (int i = 0; i < tempArray.Length - 1; i++)
            {
                electricityUse[i] = tempArray[i];
            }

            List<BO.DroneToList> droneToLists = new();

            foreach (var drone in dalObject.GetDroneList())
            {
                BO.DroneToList newDrone = new();
                //Take all the information from the drone entity in DAL
                newDrone.Id = drone.Id;
                newDrone.Model = drone.Model;
                newDrone.MaxWeight = (WeightCategories)drone.MaxWeight;

                //Get the rest of the information and fill the other fileds in the new DroneToList entity
                foreach (var parcel in dalObject.GetParcelList())
                {
                    if (parcel.DroneId == drone.Id && parcel.Delivered != DateTime.MinValue)
                    {
                        newDrone.DroneStatus = DroneStatuses.Shipment;

                        if (parcel.PickedUp == DateTime.MinValue)
                        {
                            double minDistance = double.MaxValue;
                            foreach (var baseStation in dalObject.GetBaseStationList())
                            {
                                // Get the Sender location coordinates
                                double senderLatitude = dalObject.FindCustomerById(parcel.SenderId).Lattitude;
                                double senderLongitude = dalObject.FindCustomerById(parcel.SenderId).Longitude;

                                // calculate the distance between the sender and the current base station
                                double currentDistance = dalObject.Distance(baseStation.Lattitude, senderLatitude, baseStation.Longitude, senderLongitude);

                                if (currentDistance < minDistance)
                                {
                                    minDistance = currentDistance;

                                    // update the location coordinates of the droneToList to the closest base station to the sender location
                                    newDrone.CurrentLocation.Lattitude = baseStation.Lattitude;
                                    newDrone.CurrentLocation.Longitude = baseStation.Longitude;
                                }
                            }
                        }
                        else if (parcel.Delivered == DateTime.MinValue)
                        {
                            // Get the Sender location coordinates
                            double senderLatitude = dalObject.FindCustomerById(parcel.SenderId).Lattitude;
                            double senderLongitude = dalObject.FindCustomerById(parcel.SenderId).Longitude;

                            newDrone.CurrentLocation.Lattitude = senderLatitude;
                            newDrone.CurrentLocation.Longitude = senderLongitude;

                        }
                        //Step 1: find the distance between the current location of the drone and the receiver location
                        double targetLatitude = dalObject.FindCustomerById(parcel.TargetId).Lattitude;
                        double targetLongitude = dalObject.FindCustomerById(parcel.TargetId).Longitude;
                        double distance1 = dalObject.Distance(newDrone.CurrentLocation.Lattitude, targetLatitude, newDrone.CurrentLocation.Longitude, targetLongitude);

                        //Step 2: find the minimal needed power suply
                        double suply;
                        switch (parcel.Weight)
                        {
                            case IDAL.DO.WeightCategiries.Heavy:
                                //Available-0, Light-1, Intermediate-2, Heavy-3, DroneChargingRate-4
                                suply = distance1 / electricityUse[3];
                                break;
                            case IDAL.DO.WeightCategiries.Average:
                                suply = distance1 / electricityUse[2];
                                break;
                            case IDAL.DO.WeightCategiries.Light:
                                suply = distance1 / electricityUse[1];
                                break;
                        }
                        //TODO: calculate the distance between the target and the closest base station with at least one available chargSlot
                        //double distance2 = dalObject.Distance(targetLatitude, )
                    }
                }


                droneToLists.Add(newDrone);
            }
        }
    }
}
