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
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public int DroneId { get; set; }

            public Priorities Priority;
            public WeightCategiries Weight;

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
                       $"Senderld: {SenderId}\n " +
                       $"Targetld: {TargetId}\n" +
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

