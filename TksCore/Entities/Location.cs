using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{
    public class Location
        : EntityBase
    {
        public Location()
            : base()
        {
        }

        public Location(int id)
            : base(id)
        {

        }

        public string City { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public int TimeZoneId { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public override string ToString()
        {
            return string.Join("-", new string[] { this.City, this.State, this.Country });
        }
    }
}


