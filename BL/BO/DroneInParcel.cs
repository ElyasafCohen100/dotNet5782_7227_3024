using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public double BatteryStatus { get; set; }
        public Location CurrentLocation { get; set; } = new();

        /// <summary>
        /// Return describe of Drone class string.
        /// </summary>
        /// <returns> Describe of Drone class string </returns>
        public override string ToString()
        {
            return $"Drone In Parcel:\n" +
                    $"Id: {Id}\n" +
                    $"Battery status: {BatteryStatus}\n" +
                    $"Current location: {CurrentLocation}\n";
        }
    }
}
