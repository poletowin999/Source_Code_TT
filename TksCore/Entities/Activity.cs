using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{
    public class Activity
        : EntityBase
    {

        public Activity() : base() { }
        public Activity(int id) : base(id) { }

        public DateTime Date { get; set; }

        public int TypeId { get; set; }

        public int? ClientId { get; set; }

        public int? ProjectId { get; set; }

        public int? LocationId { get; set; }

        public int TimeZoneId { get; set; }

        public int? PlatformId { get; set; }

        public int? TestId { get; set; }

        public int? BillingTypeId { get; set; }

        public int? LanguageId { get; set; }

        public int WorkTypeId { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public int Duration { get; set; }

        public int StatusId { get; set; }

        /// <summary>
        /// Gets the last comment of the activity or sets the comment to update while save as new activity.
        /// </summary>
        public string Comment { get; set; }

        public int CreateUserId { get; set; }

        public DateTime CreateDate { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public bool IsReviewed { get; set; }

        public int? ReviewUserId { get; set; }

        public DateTime? ReviewDate { get; set; }

        public bool IsReset { get; set; }

        public int? ResetId { get; set; }
    }
}
