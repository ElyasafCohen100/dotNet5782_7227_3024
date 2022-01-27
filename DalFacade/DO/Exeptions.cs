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
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
}
