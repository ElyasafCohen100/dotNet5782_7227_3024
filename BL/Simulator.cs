using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BO;
using BL;

namespace BL
{
    class Simulator
    {
        public const double DRONE_VELOCITY = 10;
        public const int DELAY_STEP_TIMER = 500;

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
                        lock (BL.dalObject)
                        {
                            BLObject.AssociateDroneTofParcelBL(droneId);
                        }
                    }
                    catch (OutOfBatteryException)
                    {
                        try
                        {
                            lock (BL.dalObject)
                            {
                                BLObject.UpdateDroneToChargingBL(droneId);
                            }
                        }
                        catch (OutOfBatteryException)
                        {
                            //Demonstrate drone collect by technition.
                            Thread.Sleep(3000);

                            lock (BLObject) lock (BL.dalObject)
                                {
                                    int stationId = BLObject.FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
                                    Station station = BLObject.GetStationByIdBL(stationId);
                                    drone.CurrentLocation = station.Location;
                                    BLObject.UpdateDroneToChargingBL(droneId);
                                    drone.DroneStatus = DroneStatuses.Maintenance;

                                    //Update Station and DroneCharge detailes.

                                    try
                                    {
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
                        BLObject.AssociateDroneTofParcelBL(droneId);
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
