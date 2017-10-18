using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tks.Model;
using Tks.Entities;

namespace Tks.Services
{
    public interface ILblLanguage : IEntityService
    {
        List<LblLanguage> RetrieveLabel(int userId, string Pagename);
    }
}
