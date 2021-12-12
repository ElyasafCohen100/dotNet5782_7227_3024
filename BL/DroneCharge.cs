using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneCharge
        {
            public int DroneId { get; set; }
            public double BatteryStatus { get; set; }

            public DateTime chargeTime;

            /// <summary>
            /// Return describe of DroneCharge class string.
            /// </summary>
            /// <returns> Describe of DroneCharge class string </returns>
            public override string ToString()
            {
                return $"DroneCharge:\n " +
                       $"Id: {DroneId}\n" +
                       $"Battery status: {BatteryStatus}\n";
            }
        }
    }
}
