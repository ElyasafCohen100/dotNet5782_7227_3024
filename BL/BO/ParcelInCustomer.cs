using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInCustomer
    {
        public int Id { set; get; }
        
        public WeightCategories WeightCategory { set; get; }
        public Priorities Priority { set; get; }
        public ParcelStatus ParcelStatus { set; get; }
       
        public CustomerInParcel Customer { get; set; } = new();

        /// <summary>
        /// Return describe of ParcelInCustomer class string.
        /// </summary>
        /// <returns> Describe of ParcelInCustomer class string </returns>
        public override string ToString()
        {
            return $"Parcel In Customer:\n" +
                   $"Id: {Id}\n" +
                   $"Weight: {WeightCategory}\n" +
                   $"Priority: {Priority}\n" +
                   $"Status: {ParcelStatus}\n" +
                   Customer.ToString();
        }
    }
}
