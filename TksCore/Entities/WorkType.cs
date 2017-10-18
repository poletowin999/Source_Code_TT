using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;

namespace Tks.Entities
{
    public class WorkType
        :EntityBase
    {
        public WorkType() : base() { }
        public WorkType(int id) : base(id) { }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ActivityTypeId { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; }
        public bool ConsiderForReport { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
