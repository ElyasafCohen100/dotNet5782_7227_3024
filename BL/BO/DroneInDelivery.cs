using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInDelivery
    {
        public int Id { get; set; }
        public double BatteryStatus { get; set; }
        public Location CurrentLocation { get; set; } = new();

        /// <summary>
        /// Return describe of DroneInDelivery class string.
        /// </summary>
        /// <returns> Describe of DroneInDelivery class string </returns>
        public override string ToString()
        {
            return $"Drone In Delivery:\n" +
                   $"Id: {Id}\n" +
                   $"Battery Status: {BatteryStatus}\n" +
                   CurrentLocation.ToString();
        }
    }
}
