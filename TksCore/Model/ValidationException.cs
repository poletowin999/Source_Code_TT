using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tks.Model
{
    public class ValidationException
        : System.ApplicationException
    {

        public ValidationException() : base() { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }

        protected ValidationException(System.Runtime.Serialization.SerializationInfo info
            , System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
