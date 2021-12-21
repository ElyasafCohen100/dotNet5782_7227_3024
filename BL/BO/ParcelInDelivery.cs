using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class ParcelInDelivery
    {
        public int Id { get; set; }
        public bool ParcelStatus { get; set; }

        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }

        public CustomerInParcel receiverCustomer = new();
        public CustomerInParcel senderCustomer = new();

        public Location SourceLocation = new();
        public Location TargetLocation = new();
        public double DistanceDelivery { get; set; }

        /// <summary>
        /// Return describe of ParcelInDelivery class string.
        /// </summary>
        /// <returns> Describe of ParcelInDelivery class string </returns>
        public override string ToString()
        {
            return $"Parcel In Delivery:\n" +
                   $"Id: {Id}\n" +
                   $"Parcel Status: {ParcelStatus}\n" +
                   $"Priority: {Priority}\n" +
                   $"Weight: {Weight}\n" +
                   receiverCustomer.ToString() +
                   senderCustomer.ToString() +
                   $"Source Locaion:" + SourceLocation.ToString() + "\n" +
                   $"Target Location:" + TargetLocation.ToString() + "\n" +
                   $"Distance Delivery: {DistanceDelivery}\n";
        }
    }
}
