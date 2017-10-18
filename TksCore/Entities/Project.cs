using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{
    public class Project
        : EntityBase
    {
        public Project() : base() { }
        public Project(int id) : base(id) { }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ClientId { get; set; }

        public int ResponsibleUserId { get; set; }

        public int CategoryId { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
