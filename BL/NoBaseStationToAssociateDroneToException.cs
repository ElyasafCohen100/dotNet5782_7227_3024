using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class NoBaseStationToAssociateDroneToException : Exception
        {
            public NoBaseStationToAssociateDroneToException()
            {
            }

            public NoBaseStationToAssociateDroneToException(string message) : base(message)
            {
            }

            public NoBaseStationToAssociateDroneToException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NoBaseStationToAssociateDroneToException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
            public override string ToString()
            {
                return "ERROR - there is no base station to associate the drone ";
            }
        }
    }
}
