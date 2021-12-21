using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// Parcel priorety categiries 
    /// </summary>
    public enum Priorities
    {
        Regular,
        Fast,
        Emergency
    }

    /// <summary>
    /// Parcel Weight categiries
    /// </summary>
    public enum WeightCategories
    {
        Light,
        Average,
        Heavy
    }

    /// <summary>
    /// Drone Statuses
    /// </summary>
    public enum DroneStatuses
    {
        Available,
        Maintenance,
        Shipment
    }

    public enum ParcelStatus
    {
        Requested,
        Scheduled,
        PickedUp,
        Delivered
    }
}
