using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IProjectService
        : IEntityService
    {
        Project Retrieve(int id);
        List<Project> Retrieve(int[] ids);
        void Update(Project entity, List<Location> locations, List<Platform> platforms, List<Test> tests);
        List<Project> Search(SearchCriteria criteria);

        // Used in activity entry process.
        List<Project> RetrieveByClient(int clientId, int userid);


    }
}
