using System;

namespace MLApplications.Core.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid Username and/or Password. Please try again.")
        { }
        public InvalidCredentialsException(string message) : base(message) { }
        public InvalidCredentialsException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
