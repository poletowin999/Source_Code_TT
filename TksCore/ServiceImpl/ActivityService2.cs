using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Net.Mail;

using Tks.Model;
using Tks.Entities;
using Tks.Services;

namespace Tks.ServiceImpl
{
    internal sealed partial class ActivityService
        : IActivityService
    {



        public List<UserActivitySummary> RetrieveActivitySummaryByUser(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveActivitySummaryByUser";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Parentuserid", SqlDbType.Int).Value = userId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtActivitySummary = new DataTable();
                adapter.Fill(DtActivitySummary);

                // Create a list.
                List<UserActivitySummary> ActivitySummarylst = null;
                if (DtActivitySummary.Rows.Count > 0)
                    ActivitySummarylst = new List<UserActivitySummary>();

                // Iterate each row.
                foreach (DataRow row in DtActivitySummary.Rows)
                {

                    UserActivitySummary usr = new UserActivitySummary();
                    usr.UserName = row["username"].ToString();
                    usr.UserId = Convert.ToInt32(row["userId"].ToString());
                    usr.ActivityDate = Convert.ToDateTime(row["Activitydate"].ToString());
                    usr.ActivityCount = Convert.ToInt32(row["ActivityCount"].ToString());
                    usr.CustomData.Add("Duration", (string.IsNullOrEmpty(row["Duration"].ToString())) ? null : row["Duration"].ToString());
                    //Commented by saravanan on 07312012 release later
                    //usr.CustomData.Add("ActiveStatus",row["ActiveStatus"].ToString());
                    // Add to list.
                    ActivitySummarylst.Add(usr);
                }

                // Return the list.
                return ActivitySummarylst;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<Activity> Retrieve(int userId, DateTime activityDate)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveActivitySummary";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@ActivityDate", SqlDbType.DateTime).Value = activityDate;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtActivity = new DataTable();
                adapter.Fill(DtActivity);

                // Create a list.
                List<Activity> Activitylst = null;
                if (DtActivity.Rows.Count > 0)
                    Activitylst = new List<Activity>();

                // Iterate each row.
                foreach (DataRow row in DtActivity.Rows)
                {
                    Activity activity = new Activity(Int32.Parse(row["ActivityId"].ToString()));
                    activity.Date = DateTime.Parse(row["ActivityDate"].ToString());
                    activity.TypeId = (string.IsNullOrEmpty(row["Type"].ToString())) ? 0 : Int32.Parse(row["Type"].ToString());

                    activity.ClientId = (string.IsNullOrEmpty(row["ClientId"].ToString())) ? 0 : Int32.Parse(row["ClientId"].ToString());
                    activity.ProjectId = (string.IsNullOrEmpty(row["ProjectId"].ToString())) ? 0 : Int32.Parse(row["ProjectId"].ToString());
                    activity.LocationId = (string.IsNullOrEmpty(row["LocationId"].ToString())) ? 0 : Int32.Parse(row["LocationId"].ToString());
                    activity.PlatformId = (string.IsNullOrEmpty(row["PlatformId"].ToString())) ? 0 : Int32.Parse(row["PlatformId"].ToString());
                    activity.TestId = (string.IsNullOrEmpty(row["TestId"].ToString())) ? 0 : Int32.Parse(row["TestId"].ToString());
                    activity.BillingTypeId = (string.IsNullOrEmpty(row["BillingTypeId"].ToString())) ? 0 : Int32.Parse(row["BillingTypeId"].ToString());
                    activity.LanguageId = (string.IsNullOrEmpty(row["LanguageId"].ToString())) ? 0 : Int32.Parse(row["LanguageId"].ToString());

                    activity.WorkTypeId = Int32.Parse(row["WorkTypeId"].ToString());
                    activity.TimeZoneId = Int32.Parse(row["TimeZoneId"].ToString());
                    activity.StartDateTime = DateTime.Parse(row["StartDateTime"].ToString());
                    activity.EndDateTime = DateTime.Parse(row["EndDateTime"].ToString());
                    activity.Duration = Int32.Parse(row["Duration"].ToString());
                    activity.StatusId = Int32.Parse(row["StatusId"].ToString());

                    activity.CreateUserId = Int32.Parse(row["CreateUserId"].ToString());
                    activity.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                    activity.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    activity.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());

                    activity.IsReviewed = bool.Parse(row["IsReviewed"].ToString());
                    activity.ReviewUserId = (string.IsNullOrEmpty(row["ReviewUserId"].ToString())) ? 0 : Int32.Parse(row["ReviewUserId"].ToString());
                    if (activity.IsReviewed == true)
                        activity.ReviewDate = DateTime.Parse(row["ReviewDate"].ToString());

                    activity.IsReset = bool.Parse(row["IsReset"].ToString());
                    activity.ResetId = (row["ResetId"] == DBNull.Value) ? (int?)null : Int32.Parse(row["ResetId"].ToString());

                    activity.CustomData.Add("TypeName", (string.IsNullOrEmpty(row["TypeName"].ToString())) ? null : row["TypeName"].ToString());
                    activity.CustomData.Add("ClientName", (string.IsNullOrEmpty(row["ClientName"].ToString())) ? null : row["ClientName"].ToString());
                    activity.CustomData.Add("ProjectName", (string.IsNullOrEmpty(row["ProjectName"].ToString())) ? null : row["ProjectName"].ToString());
                    activity.CustomData.Add("City", (string.IsNullOrEmpty(row["City"].ToString())) ? null : row["City"].ToString());
                    activity.CustomData.Add("State", (string.IsNullOrEmpty(row["State"].ToString())) ? null : row["State"].ToString());
                    activity.CustomData.Add("Country", (string.IsNullOrEmpty(row["Country"].ToString())) ? null : row["Country"].ToString());
                    activity.CustomData.Add("TimeZoneName", (string.IsNullOrEmpty(row["TimeZoneName"].ToString())) ? null : row["TimeZoneName"].ToString());
                    activity.CustomData.Add("TimeZoneShortName", (string.IsNullOrEmpty(row["TimeZoneShortName"].ToString())) ? null : row["TimeZoneShortName"].ToString());
                    activity.CustomData.Add("PlatformName", (string.IsNullOrEmpty(row["PlatformName"].ToString())) ? null : row["PlatformName"].ToString());
                    activity.CustomData.Add("TestName", (string.IsNullOrEmpty(row["TestName"].ToString())) ? null : row["TestName"].ToString());
                    activity.CustomData.Add("LanguageName", (string.IsNullOrEmpty(row["LanguageName"].ToString())) ? null : row["LanguageName"].ToString());
                    activity.CustomData.Add("WorkTypeName", (string.IsNullOrEmpty(row["WorkTypeName"].ToString())) ? null : row["WorkTypeName"].ToString());
                    activity.CustomData.Add("BillingTypeName", (string.IsNullOrEmpty(row["BillingTypeName"].ToString())) ? null : row["BillingTypeName"].ToString());
                    activity.CustomData.Add("StatusName", (string.IsNullOrEmpty(row["StatusName"].ToString())) ? null : row["StatusName"].ToString());
                    activity.CustomData.Add("CreateUserName", (string.IsNullOrEmpty(row["CreateUserName"].ToString())) ? null : row["CreateUserName"].ToString());
                    activity.CustomData.Add("ReviewUserName", (string.IsNullOrEmpty(row["ReviewUserName"].ToString())) ? null : row["ReviewUserName"].ToString());
                    activity.CustomData.Add("ResetUserName", (string.IsNullOrEmpty(row["ResetUserName"].ToString())) ? null : row["ResetUserName"].ToString());
                    activity.CustomData.Add("LastUpdateUserName", (string.IsNullOrEmpty(row["LastUpdateUserName"].ToString())) ? null : row["LastUpdateUserName"].ToString());
                    activity.CustomData.Add("Comment", (string.IsNullOrEmpty(row["Comment"].ToString())) ? "" : row["Comment"].ToString());
                    activity.CustomData.Add("FormattedDuration", (string.IsNullOrEmpty(row["FormattedDuration"].ToString())) ? null : row["FormattedDuration"].ToString());

                    // Add to list.
                    Activitylst.Add(activity);
                }

                // Return the list.
                return Activitylst;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Approve(List<Activity> entities)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Activities>");
                foreach (Activity element in entities)
                {
                    xml.Append(string.Format("<Activity><Id>{0}</Id><IsReviewed>{1}</IsReviewed><Comment>{2}</Comment><StatusId>{3}</StatusId></Activity>", element.Id, element.IsReviewed, element.Comment, element.StatusId));
                }
                xml.Append("</Activities>");

                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "ApproveActivities";
                // Assign the parameters.
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = xml.ToString();
                command.Parameters.Add("@ApproveuserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;

                // execute
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // fill error datatable
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // commit
                transaction.Commit();

                // Get output parameters.
                bool hasError = bool.Parse(command.Parameters["@hasError"].Value.ToString());

                if (hasError)
                {
                    // Create exception instance.
                    //ValidationException exception = new ValidationException("Validation error(s) occurred.");
                    ValidationException exception = new ValidationException("");

                    if (errorDataTable != null)
                    {
                        StringBuilder message = new StringBuilder(string.Format("<ul>"));
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format("<li>{0}</li>", row["Value"].ToString()));
                        }
                        message.Append("</ul>");
                        exception.Data.Add("ActivityExists", message);
                    }

                    throw exception;
                }
            }
            catch (ValidationException ve)
            {
                throw ve;
            }
            catch
            {
                if (transaction != null)
                    if (transaction.Connection != null)
                        transaction.Rollback();

                throw;
            }
            finally
            {
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Reject(List<Activity> entities)
        {
            Approve(entities);
        }

        public List<ActivityComment> RetrieveComments(int activityId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveComments";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = activityId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtActivityComments = new DataTable();
                adapter.Fill(DtActivityComments);

                // Create a list.
                List<ActivityComment> ActivityCommentlst = null;
                if (DtActivityComments.Rows.Count > 0)
                    ActivityCommentlst = new List<ActivityComment>();

                // Iterate each row.
                foreach (DataRow row in DtActivityComments.Rows)
                {

                    ActivityComment Ac = new ActivityComment();
                    Ac.ActivityId = Convert.ToInt32(row["ActivityId"].ToString());
                    Ac.ActivityStatusId = Convert.ToInt32(row["ActivityStatusId"].ToString());
                    Ac.Comment = row["Comment"].ToString();
                    Ac.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                    Ac.CreateUserId = Convert.ToInt32(row["CreateUserId"].ToString());
                    Ac.CustomData.Add("Username", (string.IsNullOrEmpty(row["Username"].ToString())) ? null : row["Username"].ToString());
                    //Commented by saravanan on 07312012 release later
                    // Ac.CustomData.Add("ActivityStatus", (string.IsNullOrEmpty(row["ActivityStatus"].ToString())) ? null : row["ActivityStatus"].ToString());
                    // Add to list.
                    ActivityCommentlst.Add(Ac);
                }

                // Return the list.
                return ActivityCommentlst;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }


        public void SendRejectMail(System.Net.Mail.MailMessage message)
        {

            try
            {
                // Fetch values from Web.Config file.
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

                // Create to smtp client.
                SmtpClient client = new SmtpClient();
                if (mailSettings != null)
                {
                    client.Host = mailSettings.Smtp.Network.Host;
                    client.Port = mailSettings.Smtp.Network.Port;
                    client.Credentials = new NetworkCredential(Utility.ConvertASCII2String(mailSettings.Smtp.Network.UserName), Utility.ConvertASCII2String(mailSettings.Smtp.Network.Password));
                    client.EnableSsl = true;
                }
                client.Send(message);
            }
            catch (SmtpFailedRecipientException ex)
            {
                throw new Exception("Mail Delivered Failed", ex);
            }
            // TODO: Handler mail failures.
            catch { throw; }
        }

        public DataTable RetrieveAppovalusers(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAppovalusers";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@userid", SqlDbType.Int).Value = userId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable activityDataTable = new DataTable("Appovalusers");
                adapter.Fill(activityDataTable);
                return activityDataTable;

            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }
    }
}
