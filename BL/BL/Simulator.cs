using System;
using System.Linq;
using System.Threading;
using BO;

namespace BL
{
    class Simulator
    {
        #region Const Value
        public const double DRONE_VELOCITY = 10;
        public const int DELAY = 500;
        #endregion


        #region Simulator Constructor
        public Simulator(BL BLObject, int droneId, Action action, Func<bool> checkStopFunc)
        {
            DroneToList drone;

            while (!checkStopFunc())
            {
                drone = BLObject.GetDronesToList(x => x.Id == droneId).FirstOrDefault();
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


                            int stationId = BLObject.FindNearestBaseStationWithAvailableChargingSlots(drone.CurrentLocation);
                            Station station = BLObject.GetStationByIdBL(stationId);
                            drone.CurrentLocation = station.Location;

                            lock (BLObject)
                            {
                                BLObject.UpdateDroneToChargingBL(droneId);
                            }
                        }
                    }
                }
                else if (drone.DroneStatus == DroneStatuses.Shipment)
                {
                    Thread.Sleep(DELAY);
                    lock (BLObject)
                    {
                        Parcel parcel = BLObject.GetParcelByIdBL(drone.DeliveryParcelId);
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

                        double batteryStatus;
                        lock (BLObject)
                        {
                            DroneCharge droneCharge = BLObject.FindDroneChargeByDroneIdBL(drone.Id);
                            batteryStatus = BLObject.BatteryCalc(drone, droneCharge);
                        }

                        if (batteryStatus == 100)
                        {
                            lock (BLObject)
                            {
                                BLObject.UpdateDroneFromChargingBL(droneId);
                            }
                        }
                    }
                    catch (OutOfBatteryException)
                    {
                        //Do nothing, stay in charging.
                    }
                }
                action();
            }
        }
        #endregion
    }
}