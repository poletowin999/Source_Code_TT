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
    /// Represents search criteria for role.
    /// </summary>
    [Obsolete("Use MasterEntitySearchCriteria class, this is for future purpose only.")]
    [XmlRoot("SearchCriteria")]
    public class RoleSearchCriteria
        : SearchCriteria
    {

        public RoleSearchCriteria() : base() { this.Status = "Active"; }

        public string Name { get; set; }
        public string Status { get; set; }

    }
}
