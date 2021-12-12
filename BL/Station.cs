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
            public Location Location = new();
            public int AvailableChargeSlots { get; set; }
            public List<DroneCharge> DroneChargesList = new();

            /// <summary>
            /// Return describe of Station class string.
            /// </summary>
            /// <returns> Describe of Station class string </returns>
            public override string ToString()
            {
                string stringDroneChargeList = "";

                foreach (var droneCharge in DroneChargesList)
                {
                    stringDroneChargeList += droneCharge.ToString();
                }

                return $"Station:\n " +
                       $"Id: {Id}\n" +
                       $"Name: {Name}\n " +
                       $"Location: {Location}\n " +
                       $"Available charge slots: {AvailableChargeSlots}\n";
            }
        }
    }
}