using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IMenuService
        : IEntityService
    {
        Menu Retrieve(int id);
        List<Menu> Retrieve(int[] ids);
        List<Menu> RetrieveByUser(int userId);
        List<Menu> RetrieveByUser(int userId, int languageid);
        List<Menu> RetrieveChildren(int menuId);
    }
}
