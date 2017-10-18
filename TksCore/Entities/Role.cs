using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Tks.Model;

namespace Tks.Entities
{
    /// <summary>
    /// Represents the role of the user.
    /// </summary>
    public sealed class Role
        : MasterEntity
    {
        public Role() : base() { }

        public Role(int id) : base(id) {  }

    }
}
