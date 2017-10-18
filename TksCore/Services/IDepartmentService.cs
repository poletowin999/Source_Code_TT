using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IDepartmentService
        : IEntityService
    {
        Department Retrieve(int id);
        List<Department> Retrieve(int[] ids);
        void Update(Department entity);
        List<Department> Searchdpt(SearchCriteria criteria);

        // Used in project master while adding Department.
        List<Department> RetrieveAll();

        // Used in project master, activity entry screens.
        List<Department> RetrieveByProject(int projectId);
    }
}
