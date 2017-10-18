using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{
    public class ActivitySearchCriteria
        :SearchCriteria
    {
        public ActivitySearchCriteria() : base() { }

        public DateTime? ActivityDate { get; set; }

        public string ClientName { get; set; }

        public string ProjectName { get; set; }
    }
}
