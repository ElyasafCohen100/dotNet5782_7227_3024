using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Represent parcel with links to customer seller and drone
        /// </summary>
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public int DroneId { get; set; }

            public Priorities Priority;
            public WeightCategories Weight;

            public DateTime? Requested;
            public DateTime? Scheduled;
            public DateTime? PickedUp;
            public DateTime? Delivered;

            /// <summary>
            /// Return describe of Parcel struct string
            /// </summary>
            /// <returns>describe of Parcel struct string</returns>
            public override string ToString()
            {
                return $"Parcel:\n" +
                       $"Id: {Id}:\n" +
                       $"Senderld: {SenderId}\n" +
                       $"Targetld: {TargetId}\n" +
                       $"Weight: {Weight}\n" +
                       $"Priority: {Priority}\n" +
                       $"Droneld: {DroneId}\n" +
                       $"Requested: {Requested}\n" +
                       $"Scheduled: {Scheduled}\n" +
                       $"PickedUp: {PickedUp}\n" +
                       $"Delivered: {Delivered}\n";
            }
        }
    }
}