using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() { }
        public ObjectNotFoundException(string message) : base($"The {message} hasn't been found") { }
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
    }


    [Serializable]
    public class ObjectIsNotActiveException : Exception
    {
        public ObjectIsNotActiveException() { }
        public ObjectIsNotActiveException(string message) : base($"The {message} is not active") { }
        public ObjectIsNotActiveException(string message, Exception inner) : base(message, inner) { }
    }
}