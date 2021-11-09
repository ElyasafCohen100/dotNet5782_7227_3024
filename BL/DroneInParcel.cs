using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneInParcel
        {
            public int Id { get; set; }
            public double BatteryStatus { get; set; }
            public Location CurrentLocation;

            /// <summary>
            /// Return describe of Drone struct string
            /// </summary>
            /// <returns>describe of Drone struct string</returns>
            public override string ToString()
            {
                return $"Drone in parcel:\n " +
                       $"Id: {Id}\n " +
                       $"Battery status: {BatteryStatus}\n" +
                       $"Current location: {CurrentLocation}\n";
            }
        }
    }
}
