using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Tks.Model;
using Tks.Entities;
using Tks.Services;
using System.Web;

namespace Tks.ServiceImpl
{
    internal sealed partial class ActivityService
        : IActivityService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        #region Internal members

        private void ValidateActivity(Activity activity)
        {
            try
            {
                ValidationException exception = new ValidationException("Validation error.");

                if (activity.TypeId != 1 && activity.TypeId != 2)
                    exception.Data.Add("TYPE", "Activity type is invalid.");

                // Activity is misc activity.
                if (activity.TypeId == 2)
                {
                    if (activity.TimeZoneId == 0)
                        exception.Data.Add("TIME_ZONE", "Time zone should not be empty.");
                    if (activity.WorkTypeId == 0)
                        exception.Data.Add("WORK_TYPE", "Work type should not be empty.");
                    if (!activity.StartDateTime.HasValue)
                        exception.Data.Add("START_DATE", "Start date should not be empty.");
                    if (!activity.EndDateTime.HasValue)
                        exception.Data.Add("END_DATE", "End date should not be empty.");
                    if (activity.LocationId == 0)
                        exception.Data.Add("Location", "Location should not be empty.");

                    //if (activity.StartDateTime.HasValue && activity.EndDateTime.HasValue)
                    //    if (activity.EndDateTime.Value <= activity.StartDateTime.Value)
                    //        exception.Data.Add("END_DATE", "End date should not be equal or earlier than start date.");
                }
                // Activity is project activity.
                else
                {
                    if (activity.ClientId == 0)
                        exception.Data.Add("CLIENT", "Client should not be empty.");
                    if (activity.ProjectId == 0)
                        exception.Data.Add("PROJECT", "Project should not be empty.");
                    if (activity.LocationId == 0)
                        exception.Data.Add("LOCATION", "Location should not be empty.");
                    if (activity.TimeZoneId == 0)
                        exception.Data.Add("TIME_ZONE", "Time zone should not be empty.");
                    if (activity.PlatformId == 0)
                        exception.Data.Add("PLATFORM", "Platform should not be empty.");
                    if (activity.TestId == 0)
                        exception.Data.Add("TEST", "Test should not be empty.");
                    if (activity.LanguageId == 0)
                        exception.Data.Add("LANGUAGE", "Language should not be empty.");
                    if (activity.WorkTypeId == 0)
                        exception.Data.Add("WORK_TYPE", "Work type should not be empty.");
                    if (activity.BillingTypeId == 0)
                        exception.Data.Add("BILLING_TYPE", "Billing type should not be empty.");
                    if (!activity.StartDateTime.HasValue)
                        exception.Data.Add("START_DATE", "Start date should not be empty.");
                    if (!activity.EndDateTime.HasValue)
                        exception.Data.Add("END_DATE", "End date should not be empty.");
                    //if (activity.StartDateTime.HasValue && activity.EndDateTime.HasValue)
                    //    if (activity.EndDateTime.Value <= activity.StartDateTime.Value)
                    //        exception.Data.Add("END_DATE", "End date should not be equal or earlier than start date.");
                }

                // Throw the exception
                if (exception.Data.Count > 0)
                    throw exception;
            }
            catch { throw; }
        }

        #endregion

        public Activity Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Activity> activities = this.Retrieve(ids);

                return (activities.Count > 0) ? activities[0] : null;
            }
            catch { throw; }
        }

        public List<Activity> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Activities>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Activity><Id>{0}</Id></Activity>", element));
                }
                xml.Append("</Activities>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveActivity";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable activityDataTable = new DataTable("Activities");
                adapter.Fill(activityDataTable);

                // Create a list.
                List<Activity> activities = null;
                if (activityDataTable.Rows.Count > 0)
                    activities = new List<Activity>();

                // Iterate each row.
                foreach (DataRow row in activityDataTable.Rows)
                {
                    // Create an instance of Activity.
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
                    activity.CustomData.Add("Comment", (string.IsNullOrEmpty(row["Comment"].ToString())) ? null : row["Comment"].ToString());
                    // Add to list.
                    activities.Add(activity);
                }

                // Return the list.
                return activities;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<Activity> Search(SearchCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public void Update(Activity entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Validate the entity.
                this.ValidateActivity(entity);

                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateActivities_new";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = entity.GetXml();
                command.Parameters.Add("@LanguageId", SqlDbType.Int).Value = HttpContext.Current.Session["SesLanguageId"];
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //command.ExecuteNonQuery();

                // execute
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // fill error datatable
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // Get output parameters.
                if (bool.Parse(command.Parameters["@HasError"].Value.ToString()))
                {
                    // Rollback the transaction.
                    transaction.Rollback();

                    // Create exception instance.
                    ValidationException exception = new ValidationException("Validation error.");

                    foreach (DataRow row in errorDataTable.Rows)
                    {
                        exception.Data.Add(row["Key"].ToString(), row["Value"].ToString());
                    }

                    // Throw exception.
                    throw exception;
                }

                // Commit the transaction.
                transaction.Commit();
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
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void CopyActivities(DateTime CopyActivityDate, DateTime ActivityDate, int LanguageId)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "CopyActivities";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ActivityDate", SqlDbType.DateTime).Value = ActivityDate;
                command.Parameters.Add("@CopyActivityDate", SqlDbType.DateTime).Value = CopyActivityDate;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;
                command.Parameters.Add("@LanguageId", SqlDbType.Int).Value = LanguageId;
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //command.ExecuteNonQuery();

                // execute
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // fill error datatable
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // Get output parameters.
                if (bool.Parse(command.Parameters["@HasError"].Value.ToString()))
                {
                    // Rollback the transaction.
                    transaction.Rollback();

                    // Create exception instance.
                    ValidationException exception = new ValidationException("Validation error.");

                    foreach (DataRow row in errorDataTable.Rows)
                    {
                        exception.Data.Add(row["Key"].ToString(), row["Value"].ToString());
                    }

                    // Throw exception.
                    throw exception;
                }

                // Commit the transaction.
                transaction.Commit();
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
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void DeleteActivity(int ActivityId)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "DeleteActivity";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ActivityId", SqlDbType.Int).Value = ActivityId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //command.ExecuteNonQuery();

                // execute
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // fill error datatable
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // Get output parameters.
                if (bool.Parse(command.Parameters["@HasError"].Value.ToString()))
                {
                    // Rollback the transaction.
                    transaction.Rollback();

                    // Create exception instance.
                    ValidationException exception = new ValidationException("Validation error.");

                    foreach (DataRow row in errorDataTable.Rows)
                    {
                        exception.Data.Add(row["Key"].ToString(), row["Value"].ToString());
                    }

                    // Throw exception.
                    throw exception;
                }

                // Commit the transaction.
                transaction.Commit();
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
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }


        public IAppManager AppManager
        {
            get
            {
                return this._appManager;
            }
            set
            {
                this._appManager = value;
                if (value != null)
                    mDbConnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
            }
        }

        public void Dispose()
        {
            try
            {
                // Dispose.
                if (mDbConnection != null)
                {
                    if (mDbConnection.State != ConnectionState.Closed)
                        mDbConnection.Close();
                    mDbConnection.Dispose();

                    // Clear pool.
                    SqlConnection.ClearPool(mDbConnection);
                    SqlConnection.ClearAllPools();
                }
            }
            catch { throw; }
        }

       
    }
}
