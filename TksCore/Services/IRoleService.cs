using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;

namespace Tks.Services
{
    public interface IRoleService
        :IEntityService
    {
        Role Retrieve(int id);
        List<Role> Retrieve(int[] ids);
        void Update(Role entity);
        List<Role> Search(SearchCriteria criteria);
    }
}
