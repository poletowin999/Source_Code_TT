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
    public class TimeZoneSearchCriteria
        : SearchCriteria
    {
        public TimeZoneSearchCriteria() : base() { this.Status = "Active"; }

        public string Name { get; set; }

        public string ShortName { get; set; }
        public string Status { get; set; }
    }
}
