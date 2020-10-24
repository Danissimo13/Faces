using System;

namespace FacesStorage.Data.Abstractions.Exceptions
{
    public class RequestNotFoundException : Exception 
    {
        public RequestNotFoundException() { }
        public RequestNotFoundException(string message) : base(message) { }
    }
}
