using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList
        {
            public int Id { set; get; }
            public string ReceiverName { get; set; }
            public string SenderName { get; set; }
            
            public WeightCategories WeightCategory;
            public Priorities Prioritie;
            public ParcelStatus ParcelStatus;

            public override string ToString()
            {
                return $"Parcel to list:\n" +
                    $"Id: {Id}\n" +
                    $"Receiver: {ReceiverName}\n" +
                    $"Sender: {SenderName}\n" +
                    $"Weight category: {WeightCategory}\n" +
                    $"Priority: {Prioritie}\n" +
                    $"Status: {ParcelStatus}\n";
            }
        }
    }
}
