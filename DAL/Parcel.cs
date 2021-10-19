using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public Priorities Priority;

            public WeightCategiries Weight;
            public DateTime Requested;
            public int DroneId { get; set; }
            public DateTime Scheduled;
            public DateTime PickedUp;
            public DateTime Delivered;

            public override string ToString()
            {
                return $"Parcel:\n " +
                    $"Id: {Id}:\n " +
                    $"Senderld: {SenderId}\n " +
                    $"Targetld: {TargetId}\n" +
                    $"Weight: {Weight}\n" +
                    $"Priority: {Priority}\n" +
                    $"Requested: {Requested}\n" +
                    $"Droneld: {DroneId}\n" +
                    $"Scheduled: {Scheduled}\n" +
                    $"PickedUp: {PickedUp}\n" +
                    $"Delivered: {Delivered}\n";
            }
        }
    }
}