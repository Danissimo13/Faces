using System;

namespace FacesStorage.Data.Abstractions.Exceptions
{
    public class NewsNotFoundException : Exception
    {
        public NewsNotFoundException() { }
        public NewsNotFoundException(string message) : base(message) { }
    }
}
