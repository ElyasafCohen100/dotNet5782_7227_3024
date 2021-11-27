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
        public class ObjectNotFoundException : Exception
        {
            public ObjectNotFoundException() { }
            public ObjectNotFoundException(string message) : base($"The {message} hasn't been found") { }
            public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
        }
    }
   
}
