using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface ITimeZoneService
        :IEntityService
    {
        Tks.Entities.TimeZone Retrieve(int id);
        List<Tks.Entities.TimeZone> Retrieve(int[] ids);
        void Update(Tks.Entities.TimeZone entity);
        List<Tks.Entities.TimeZone> Search(SearchCriteria criteria);

        List<Tks.Entities.TimeZone> RetrieveAll();
    }
}
