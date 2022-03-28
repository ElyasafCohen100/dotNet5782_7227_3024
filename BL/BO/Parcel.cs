using System;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }

        public CustomerInParcel senderCustomer { get; set; } = new();
        public CustomerInParcel receiverCustomer { get; set; } = new();
        public DroneInParcel Drone { get; set; } = new();

        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }

        public DateTime? Requested { get; set; } = null;
        public DateTime? Scheduled { get; set; }  = null;
        public DateTime? PickedUp { get; set; }  = null;
        public DateTime? Delivered { get; set; } = null;

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

