using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationToList
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int AvailableChargeSlots { get; set; }
            public int NotAvailableChargeSlots { get; set; }

            /// <summary>
            /// Return describe of Station struct string
            /// </summary>
            /// <returns>describe of Station struct string</returns>
            public override string ToString()
            {
                return $"Station to list:\n " +
                       $"Id: {Id}\n" +
                       $"Name: {Name}\n " +
                       $"Available charge slots: {AvailableChargeSlots}\n"+
                       $"Not available charge slots: {NotAvailableChargeSlots}\n";
            }
        }
    }
}
