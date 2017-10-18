using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface ITestService
        : IEntityService
    {
        Test Retrieve(int id);
        List<Test> Retrieve(int[] ids);
        void Update(Test entity);
        List<Test> Search(SearchCriteria criteria);

        // Used in project master while adding platform.
        List<Test> RetrieveAll();

        // Used in project master, activity entry.
        List<Test> RetrieveByProject(int projectId, int platformId);
        List<Test> RetrieveByProject(int projectId);

    }
}
