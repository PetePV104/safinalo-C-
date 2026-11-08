using System;

namespace Library.Domain.Exceptions
{
    internal class UserNotFoundException : Exception
    {
        public string Username { get; }

        public UserNotFoundException(string username)
            : base("User named '" + username + "' not found.")
        {
            Username = username;
        }
    }
}