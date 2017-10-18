using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;

namespace Tks.Entities
{
    public class UserSwipe
        :EntityBase
    {

        public UserSwipe() : base() { }
        public UserSwipe(int id) : base(id) { }

        public int UserId { get; set; }

        public DateTime WorkDate { get; set; }

        public int TimeZoneId { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public int Shift { get; set; }

        public int UserworktypeId { get; set; }

        public string Reason { get; set; }

    }
}
