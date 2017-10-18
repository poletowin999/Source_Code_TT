using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tks.Model
{
    class UserHierarchyException
        : System.ApplicationException
    {

        public UserHierarchyException() : base() { }

        public UserHierarchyException(string message) : base(message) { }

        public UserHierarchyException(string message, Exception innerException) : base(message, innerException) { }

        protected UserHierarchyException(System.Runtime.Serialization.SerializationInfo info
            , System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
