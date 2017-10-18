using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{
    public class TimeZone
        : EntityBase
    {

        public TimeZone() : base() { }
        public TimeZone(int id) : base(id) { }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Description { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
