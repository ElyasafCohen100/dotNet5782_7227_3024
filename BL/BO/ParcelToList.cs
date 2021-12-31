using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList
    {
        public int Id { set; get; }
        public string ReceiverName { get; set; }
        public string SenderName { get; set; }

        public WeightCategories WeightCategory { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus ParcelStatus { get; set; }

        /// <summary>
        /// Return describe of ParcelToList class string.
        /// </summary>
        /// <returns> Describe of ParcelToList class string </returns>
        public override string ToString()
        {
            return $"Parcel To List:\n" +
                   $"Id: {Id}\n" +
                   $"Receiver: {ReceiverName}\n" +
                   $"Sender: {SenderName}\n" +
                   $"Weight category: {WeightCategory}\n" +
                   $"Priority: {Priority}\n" +
                   $"Status: {ParcelStatus}\n";
        }
    }
}
