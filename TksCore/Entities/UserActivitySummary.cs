using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Tks.Entities
{
    public class UserActivitySummary
    {
        Dictionary<string, object> _customData;

        public UserActivitySummary()
        {
            this._customData = new Dictionary<string, object>();
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTime ActivityDate { get; set; }

        public int ActivityCount { get; set; }

        public Dictionary<string, object> CustomData
        {
            get { return this._customData; }

        }
    }
}
