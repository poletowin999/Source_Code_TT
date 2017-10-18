using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tks.Model
{
    public sealed class CustomEntity
        :EntityBase
    {
        public CustomEntity() : base() { }
        public CustomEntity(int id) : base(id) { }
    }
}
