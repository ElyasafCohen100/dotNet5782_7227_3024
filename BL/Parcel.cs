using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Parcel
        {
            public int Id { get; set; }

            public CustomerInParcel receiverCustomer;
            public CustomerInParcel senderCustomer;
            public DroneInParcel Drone { get; set; }

            public Priorities Priority;
            public WeightCategories Weight;

            public DateTime Requested;
            public DateTime Scheduled;
            public DateTime PickedUp;
            public DateTime Delivered;

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
                       $"Droneld: {DroneId}\n" +
                       $"Scheduled: {Scheduled}\n" +
                       $"PickedUp: {PickedUp}\n" +
                       $"Delivered: {Delivered}\n";
            }
        }
    }
}

