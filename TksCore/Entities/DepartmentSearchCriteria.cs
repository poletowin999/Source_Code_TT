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
    public class DepartmentSearchCriteria
        :SearchCriteria
    {

        public DepartmentSearchCriteria() : base()
        {
            this.Status = "Active";
        }

        public string City { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public int TimeZoneId { get; set; }
    }
}
