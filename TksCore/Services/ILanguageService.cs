using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface ILanguageService
        : IEntityService
    {
        Language Retrieve(int id);
        List<Language> Retrieve(int[] ids);
        void Update(Language entity);
        List<Language> Search(SearchCriteria criteria);
    }
}
