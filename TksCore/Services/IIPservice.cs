using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Tks.Model;
using Tks.Entities;

namespace Tks.Services
{
    public interface IIPservice : IEntityService
    {
        DataTable Search(string IPAddress, string Location, string Linktype, string status);
        void Update(string IPAddress, string Location, string Linktype, string status, string Remarks, int ipid);
    }
}
