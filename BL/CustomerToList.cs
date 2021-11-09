using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int SendAndDeliveredParcels { get; set; }
            public int SendAndNotDeliveredParcels { get; set; }
            public int DeliveredParcels { get; set; }
            public int PickedUpParcels { get; set; }

            /// <summary>
            /// Return describe of CustomerToList class string.
            /// </summary>
            /// <returns> Describe of CustomerToList class string </returns>
            public override string ToString()
            {
                return $"Customer to list: \n" +
                    $" Id: {Id}\n" +
                    $"Name: {Name}\n" +
                    $"Phone: {Phone}\n" +
                    $"Send And Delivered Parcels: { SendAndDeliveredParcels}\n" +
                    $"Delivered Parcels: {DeliveredParcels}\n" +
                    $"PickedUp Parcels: {PickedUpParcels}\n";
            }
        }
    }
}