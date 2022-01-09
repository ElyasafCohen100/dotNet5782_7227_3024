using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{

    [Serializable]
    public class OutOfBatteryException : Exception
    {
        public OutOfBatteryException() { }
        public OutOfBatteryException(string message) : base($"Could not send the drone {message} to charging " +
                                                            $"because it hasn't enough battery")
        { }
        public OutOfBatteryException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class InvalidInputException : Exception
    {
        public InvalidInputException() { }

        public InvalidInputException(string message) : base($"Invalid input for {message} field") { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    }


    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() { }
        public ObjectNotFoundException(string message) : base(message + "has not been found") { }
        public ObjectNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class NoBaseStationToAssociateDroneToException : Exception
    {
        public NoBaseStationToAssociateDroneToException() { }
        public NoBaseStationToAssociateDroneToException(string message) : base(message) { }
        public NoBaseStationToAssociateDroneToException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            return "There is no base station to associate the drone ";
        }
    }

    [Serializable]
    public class NotValidRequestException : Exception
    {
        public NotValidRequestException() { }
        public NotValidRequestException(string message) : base(message) { }
        public NotValidRequestException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ObjectAlreadyExistException : Exception
    {
        public ObjectAlreadyExistException() { }
        public ObjectAlreadyExistException(string message) : base($"The {message} already exist") { }
        public ObjectAlreadyExistException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ObjectIsNotActiveException : Exception
    {
        public ObjectIsNotActiveException() { }
        public ObjectIsNotActiveException(string message) : base(message) { }
        public ObjectIsNotActiveException(string message, Exception inner) : base(message, inner) { }

    }
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) : base(message, innerException) { }
    }
}