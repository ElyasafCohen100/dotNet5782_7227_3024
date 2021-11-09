using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class ParcelInDelivery
        {
            public int Id { set; get; }
            public double DeliveryDistance { get; set; }
            
            public bool ParcelStatus;


            public WeightCategories WeightCategory;
            public Priorities Priority;

            public CustomerInParcel receiverCustomer;
            public CustomerInParcel senderCustomer;

            public Location TargetLocation;
            public Location SourceLocation;

            public override string ToString()
            {
                return $"Id: {Id}\n" +
                    $"PickedUp: {ParcelStatus}\n" +
                    $"Weight: {WeightCategory}\n" +
                    $"Priority: {Priority}\n" +
                    $"receiver: {receiverCustomer.ToString()}" +
                    $"sender: {senderCustomer.ToString()}" +
                    $"Target location: {TargetLocation}\n" +
                    $"Soutce location: {SourceLocation}\n" +
                    $"Delivery distance: {DeliveryDistance}\n";
            }
        }
    }
}
