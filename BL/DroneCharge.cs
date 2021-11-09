using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneCharge
        {
            public int DroneId { get; set; }
            public double BatteryStatus { get; set; }

            /// <summary>
            /// Return describe of DroneCharge struct string
            /// </summary>
            /// <returns>describe of DroneCharge struct string</returns>
            public override string ToString()
            {
                return $"DroneCharge:\n " +
                       $"Id: {DroneId}\n" +
                       $"Battery status: {BatteryStatus}\n";
            }
        }
    }
}
