using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tks.Model
{
    public interface IEntityService
    {
        IAppManager AppManager { get; set; }
        void Dispose();
    }
}
