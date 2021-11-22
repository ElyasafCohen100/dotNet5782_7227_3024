using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location location = new Location();

            public List<ParcelInCustomer> ParcelsToSendList;
            public List<ParcelInCustomer> ParcelsToTakeList;

            /// <summary>
            /// Return describe of Customer struct string
            /// </summary>
            /// <returns>describe of Customer struct string</returns>
            public override string ToString()
            {
                return $"Customer name: {Name}\n" +
                       $"Id: {Id}\n" +
                       $"Phone: {Phone}\n" +
                       location.ToString();
            }
        }
    }
}