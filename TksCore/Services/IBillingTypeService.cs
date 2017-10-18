using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IBillingTypeService
        : IEntityService
    {
        BillingType Retrieve(int id);
        List<BillingType> Retrieve(int[] ids);
        List<BillingType> RetrieveAll();
        void Update(BillingType entity);
        List<BillingType> Search(SearchCriteria criteria);
    }
}
