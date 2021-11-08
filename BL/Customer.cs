using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location location = new Location();

            //TODO: create parcel from Coustomer
            //TODO: create parcel to Coustomer

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