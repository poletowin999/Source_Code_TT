using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Tks.Model;
using Tks.Entities;
using System.Data;


namespace Tks.Services
{
    public interface IActivityService
        :IEntityService
    {
        Activity Retrieve(int id);
        List<Activity> Retrieve(int[] ids);
        List<Activity> Search(SearchCriteria criteria);
        void Update(Activity entity);
        void CopyActivities(DateTime CopyActivityDate, DateTime ActivityDate, int LanguageId);
        void DeleteActivity(int ActivityId);

        // Used in activity approval process.
        List<UserActivitySummary> RetrieveActivitySummaryByUser(int userId);
        List<Activity> Retrieve(int userId, DateTime activityDate);
        void Approve(List<Activity> entities);
        void Reject(List<Activity> entities);
        void SendRejectMail(MailMessage message);
        List<ActivityComment> RetrieveComments(int activityId);

        // Used in reset approved activity process.
        List<CustomEntity> RetrieveActivitySummary(int userId,  DateTime activityFromDate, DateTime activityToDate);
        void ResetApproval(List<Activity> entities, string comment);
        void ResetApproval(int userId, DateTime[] activityDates, string comment);
        DataTable RetrieveAppovalusers(int userId);
    }
}
