using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Represenr droneCharge card which connect between the station and the charging drone in it
        /// </summary>
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }

            /// <summary>
            /// Return describe of DroneCharge struct string
            /// </summary>
            /// <returns>describe of DroneCharge struct string</returns>
            public override string ToString()
            {
                return $"DroneCharge: " +
                       $"Id: {DroneId}, " +
                       $"StationId: {StationId}, ";
            }
        }
    }
}