using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IPlatformService
        : IEntityService
    {
        Platform Retrieve(int id);
        List<Platform> Retrieve(int[] ids);
        void Update(Platform entity);
        List<Platform> Search(SearchCriteria criteria);

        // Used in project master while adding platform.
        List<Platform> RetrieveAll();

        // Used in project master, activity entry screens.
        List<Platform> RetrieveByProject(int projectId);
    }
}
