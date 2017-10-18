using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tks.Model
{
    public sealed class AuthenticationException
         : System.ApplicationException
    {

        public AuthenticationException() : base("Authentication failure.") { }

        public AuthenticationException(string message) : base(message) { }

        public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }

    }
}
