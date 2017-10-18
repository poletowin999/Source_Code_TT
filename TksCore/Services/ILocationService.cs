using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface ILocationService
        : IEntityService
    {
        Location Retrieve(int id);
        List<Location> Retrieve(int[] ids);
        void Update(Location entity);        
        List<Location> Search(SearchCriteria criteria, int userId);
        List<Location> ReatrieveAll();

        // Used in project master, activity entry.
        List<Location> RetrieveByProject(int projectId);
    }
}
