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
        public class UnvalidInputException : Exception
        {
            public UnvalidInputException()
            {
            }

            public UnvalidInputException(string message) : base(message)
            {
            }

            public UnvalidInputException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UnvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
            { 
            }
            public override string ToString()
            {
                return "ERROR - Unvalid Input please try again ";
            } 
        }
    }
}
