using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BO;

namespace BL
{
    class Simulator
    {
        public const double DRONE_VELOCITY = 10;
        public const int DELAY = 500;

        public Simulator(BL BLObject, int droneId, Action action, Func<bool> checkStopFunc)
        {
            Drone drone;
            lock (BL.dalObject)
            {
                drone = BLObject.GetDroneByIdBL(droneId);
            }

            while (checkStopFunc.Equals(true))
            {
                if (drone.DroneStatus == DroneStatuses.Available)
                {
                    try
                    {
                        Thread.Sleep(DELAY);
                        lock (BL.dalObject)
                        {
                            BLObject.AssociateDroneTofParcelBL(droneId);
                        }
                    }
                    catch (OutOfBatteryException)
                    {
                        try
                        {
                            Thread.Sleep(DELAY);
                            lock (BL.dalObject)
                            {
                                BLObject.UpdateDroneToChargingBL(droneId);
                            }
                        }
                        catch (OutOfBatteryException)
                        {
                            //Demonstrate drone collect by technition.
                            Thread.Sleep(3000);

                            Thread.Sleep(DELAY);
                            lock (BLObject)
                                {
                                    int stationId = BLObject.FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
                                    Station station = BLObject.GetStationByIdBL(stationId);
                                    drone.CurrentLocation = station.Location;
                                    BLObject.UpdateDroneToChargingBL(droneId);
                                    drone.DroneStatus = DroneStatuses.Maintenance;

                                    //Update Station and DroneCharge detailes.

                                    try
                                    {
                                        Thread.Sleep(DELAY);
                                        BL.dalObject.UpdateDroneToCharging(drone.Id, station.Id);
                                    }
                                    catch (DO.ObjectNotFoundException e)
                                    {
                                        throw new ObjectNotFoundException(e.Message);
                                    }
                                    catch (DO.ObjectIsNotActiveException e)
                                    {
                                        throw new ObjectIsNotActiveException(e.Message);
                                    }
                                    catch (DO.XMLFileLoadCreateException e)
                                    {
                                        throw new XMLFileLoadCreateException(e.Message);
                                    }
                                }
                        }
                    }
                }
                else if (drone.DroneStatus == DroneStatuses.Shipment)
                {
                    Thread.Sleep(DELAY);
                    lock (BLObject)
                    {
                        Parcel parcel = BLObject.GetParcelByIdBL(drone.ParcelInDelivery.Id);
                        if (parcel.PickedUp == null)
                            BLObject.UpdatePickedUpParcelByDroneIdBL(droneId);
                        else if (parcel.Delivered == null)
                            BLObject.UpdateDeliveredParcelByDroneIdBL(droneId);
                    }
                }
                else if (drone.DroneStatus == DroneStatuses.Maintenance)
                {
                    try
                    {
                        Thread.Sleep(DELAY);
                        lock (BLObject) lock (BL.dalObject)
                            {
                                BLObject.AssociateDroneTofParcelBL(droneId);
                            }
                    }
                    catch (OutOfBatteryException)
                    {
                        //Do nothing, stay in charging.
                    }
                }
            }
        }
    }
}
