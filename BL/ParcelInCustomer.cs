using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInCustomer
        {
            public int Id { set; get; }
            public WeightCategories WeightCategory;
            public Priorities Priority;
            public ParcelStatus ParcelStatus;
            public CustomerInParcel Customer =new();

            public override string ToString()
            {
                return $"Parcel in customer:\n" +
                       $"Id: {Id}\n" +
                       $"Weight: {WeightCategory}\n" +
                       $"Priority: {Priority}\n" +
                       $"Status: {ParcelStatus}\n" +
                       Customer.ToString();
            }
        }
    }
}
