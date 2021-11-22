using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Location Location;
            public int AvailableChargeSlots { get; set; }
            public List<DroneCharge> DroneChargesList;

            /// <summary>
            /// Return describe of Station struct string
            /// </summary>
            /// <returns>describe of Station struct string</returns>
            public override string ToString()
            {
                return $"Station:\n " +
                       $"Id: {Id}\n" +
                       $"Name: {Name}\n " +
                       $"Location: {Location}\n " +
                       $"Available charge slots: {AvailableChargeSlots}\n";
            }
        }
    }
}

