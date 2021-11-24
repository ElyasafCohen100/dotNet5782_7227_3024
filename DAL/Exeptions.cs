using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {

        [Serializable]
        public class RequiredObjectIsNotFoundException : Exception
        {
            public RequiredObjectIsNotFoundException() { }
            public RequiredObjectIsNotFoundException(string message) : base($"The {message} was not found") { }
            public RequiredObjectIsNotFoundException(string message, Exception inner) : base(message, inner) { }
        }
    }
   
}
