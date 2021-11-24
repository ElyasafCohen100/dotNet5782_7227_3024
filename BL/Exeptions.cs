using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        [Serializable]
        public class NoBaseStationToAssociateDroneToException : Exception
        {
            public NoBaseStationToAssociateDroneToException() { }
            public NoBaseStationToAssociateDroneToException(string message) : base(message) { }
            public NoBaseStationToAssociateDroneToException(string message, Exception inner) : base(message, inner) { }
            public override string ToString()
            {
                return "ERROR - there is no base station to associate the drone ";
            }
        }
    }
}
