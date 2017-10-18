using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    /// <summary>
    /// Provides common methods to manipulate client entity.
    /// </summary>
    public interface IClientService
        : IEntityService
    {
        Client Retrieve(int id);
        List<Client> Retrieve(int[] ids);
        void Update(Client entity);
        List<Client> Search(SearchCriteria criteria, int UserId);

 
    }
}
