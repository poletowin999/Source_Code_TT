using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{
    public class ActivityComment
        : EntityBase
    {
        public ActivityComment() : base() { }
        public ActivityComment(int id) : base(id) { }

        public int ActivityId { get; set; }

        public int ActivityStatusId { get; set; }

        public string Comment { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
