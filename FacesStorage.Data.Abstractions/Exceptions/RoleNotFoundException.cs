using System;

namespace FacesStorage.Data.Abstractions.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public RoleNotFoundException() { }
        public RoleNotFoundException(string message) : base(message) { }
    }
}
