using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Tks.Model;

namespace Tks.Entities
{
    /// <summary>
    /// Represents search criteria for client entity.
    /// </summary>
    /// 
    [XmlRoot("SearchCriteria")]
    public class ClientSearchCriteria
        : SearchCriteria
    {
        public ClientSearchCriteria() : base() 
        {
            this.Status = "Active";
        }

        public string Name { get; set; }
        public string Status { get; set; }
    }
}
