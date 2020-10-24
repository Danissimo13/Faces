using System;

namespace FacesStorage.Data.Abstractions.Exceptions
{
    public class ResponseNotFoundException : Exception
    {
        public ResponseNotFoundException() { }
        public ResponseNotFoundException(string message) : base(message) { }
    }
}
