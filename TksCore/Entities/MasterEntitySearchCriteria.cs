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
    /// Represents search criteria for master entity.
    /// </summary>
    [XmlRoot("SearchCriteria")]
    public class MasterEntitySearchCriteria
        : SearchCriteria
    {
        public MasterEntitySearchCriteria() : base()
        { this.Status = "Active"; }

        public string Name { get; set; }
        public string Status { get; set; }
    }
}
