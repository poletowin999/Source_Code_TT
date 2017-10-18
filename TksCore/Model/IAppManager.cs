using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Entities;

namespace Tks.Model
{
    public interface IAppManager
    {
        User LoginUser { get; }
        DbConnectionProvider DbConnectionProvider { get; }
    }
}
