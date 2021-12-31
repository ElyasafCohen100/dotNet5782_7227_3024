using System;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }

        public CustomerInParcel senderCustomer = new();
        public CustomerInParcel receiverCustomer = new();
        public DroneInParcel Drone = new();

        public Priorities Priority;
        public WeightCategories Weight;

        public DateTime? Requested = null;
        public DateTime? Scheduled = null;
        public DateTime? PickedUp = null;
        public DateTime? Delivered = null;

        /// <summary>
        /// Return describe of Parcel class string.
        /// </summary>
        /// <returns> Describe of Parcel class string </returns>
        public override string ToString()
        {
            return $"Parcel:\n" +
                    $"Id: {Id}:\n" +
                    senderCustomer.ToString() +
                    receiverCustomer.ToString() +
                    $"Weight: {Weight}\n" +
                    $"Priority: {Priority}\n" +
                    Drone.ToString() +
                    $"Requested: {Requested}\n" +
                    $"Scheduled: {Scheduled}\n" +
                    $"PickedUp: {PickedUp}\n" +
                    $"Delivered: {Delivered}\n";
        }
    }
}

