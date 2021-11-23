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
            public RequiredObjectIsNotFoundException(string message) : base(message) { }
            public RequiredObjectIsNotFoundException(string message, Exception inner) : base(message, inner) { }

            public override string ToString()
            {
                return "Required object is not found ";
            }
        }
    }
}
