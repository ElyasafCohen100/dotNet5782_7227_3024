using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }

            public CustomerInParcel senderCustomer = new();
            public CustomerInParcel receiverCustomer = new();
            public DroneInParcel Drone { get; set; }

            public Priorities Priority;
            public WeightCategories Weight;

            public DateTime? Requested = null;
            public DateTime? Scheduled = null;
            public DateTime? PickedUp = null;
            public DateTime? Delivered = null;

            /// <summary>
            /// Return describe of Parcel struct string
            /// </summary>
            /// <returns>describe of Parcel struct string</returns>
            public override string ToString()
            {
                return $"Parcel:\n " +
                       $"Id: {Id}:\n " +
                       senderCustomer.ToString() +
                       receiverCustomer.ToString() +
                       $"Weight: {Weight}\n" +
                       $"Requested: {Requested}\n" +
                       $"Priority: {Priority}\n" +
                       Drone.ToString() +
                       $"Scheduled: {Scheduled}\n" +
                       $"PickedUp: {PickedUp}\n" +
                       $"Delivered: {Delivered}\n";
            }
        }
    }
}

