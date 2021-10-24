using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public enum Priorities
        {
            Regular,
            fast,
            emergency
        }

        public enum WeightCategiries
        {
            Light,
            average,
            Heavy
        }
        public enum DroneStatuses
        {
            available,
            maintenance,
            shipment
        }
    }
}
