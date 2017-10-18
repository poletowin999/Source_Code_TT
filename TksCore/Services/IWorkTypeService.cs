using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IWorkTypeService
        : IEntityService
    {
        WorkType Retrieve(int id);
        List<WorkType> Retrieve(int[] ids);
        List<WorkType> RetrieveAll();
        void Update(WorkType entity);
        List<WorkType> Search(SearchCriteria criteria);
    }
}
