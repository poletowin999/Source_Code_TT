using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Tks.Model;


namespace Tks.Entities
{
    [XmlRoot("SearchCriteria")]
    public class ProjectSearchCriteria
        : SearchCriteria
    {
        public ProjectSearchCriteria() : base() 
        {
            this.Status = "Active";
        }

        public string Name { get; set; }

        public string ClientName { get; set; }
        public string Status { get; set; }
        public string CategoryId { get; set; }
        
        public string PlatformName { get; set; }

        public string TestName { get; set; }

        public string LocationName { get; set; }
    }
}
