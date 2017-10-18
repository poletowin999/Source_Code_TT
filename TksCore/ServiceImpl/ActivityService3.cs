using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Tks.Model;
using Tks.Entities;
using Tks.Services;

namespace Tks.ServiceImpl
{
    internal sealed partial class ActivityService
        : IActivityService
    {
        #region Class variables

       

        #endregion


        public List<CustomEntity> RetrieveActivitySummary(int userId,  DateTime activityFromDate, DateTime activityToDate)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Define The Command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "[RetrieveActivitySummaryForReset]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = userId;
                command.Parameters.Add("@ActivityFromDate", SqlDbType.DateTime).Value = activityFromDate;
                command.Parameters.Add("@ActivityToDate", SqlDbType.DateTime).Value = activityToDate;
                //Execute The Command
                adapter = new SqlDataAdapter(command);
                DataTable dtResetActivity = new DataTable("ActivitySummary");
                adapter.Fill(dtResetActivity);

                // Create List
                List<CustomEntity> summaryList = null;
                if (dtResetActivity.Rows.Count > 0)
                    summaryList = new List<CustomEntity>();

                // Iterate each row.
                foreach (DataRow row in dtResetActivity.Rows)
                {
                    // Create an instance.
                    CustomEntity entity = new CustomEntity();
                    entity.CustomData.Add("ActivityDate", row["ActivityDate"]);
                    entity.CustomData.Add("UserId", row["UserId"]);
                    entity.CustomData.Add("UserName", row["UserName"]);
                    entity.CustomData.Add("ActivityCount", row["ActivityCount"]);

                    // Add to list.
                    summaryList.Add(entity);
                }

                // Return the list.
                return summaryList;
            }

            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }

        }
        

        public void ResetApproval(List<Activity> entities, string comment)
        {
            SqlCommand command = null;
          
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Activities>");
                foreach (Activity element in entities)
                {
                    xml.Append(string.Format("<Activity><Id>{0}</Id></Activity>", element.Id));
                }
                xml.Append("</Activities>");
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateResetActivities";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@XmlData", SqlDbType.Xml).Value = xml.ToString();
                command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = comment;
                command.Parameters.Add("@ResetUserId", SqlDbType.Int).Value = _appManager.LoginUser.Id ;
                command.ExecuteNonQuery();
                     

            }
            catch { throw; }
            finally
            {
                // Dispose.


                if (command != null) command.Dispose();
            }
        }


        public void ResetApproval(int userId, DateTime[] activityDates, string comment)
        {
            SqlCommand command = null;
            try
            {
                StringBuilder xml = new StringBuilder();
                xml.Append("<Reset>");
               
                foreach (DateTime element in activityDates)
                {
                    xml.Append(string.Format("<ActivityDates><Date>{0}</Date></ActivityDates>", element.Date));
                }
         
                xml.Append("</Reset>");
                command = mDbConnection.CreateCommand();
                command.CommandText = "ResetApprovedActivity";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = xml.ToString();
                command.Parameters.Add("@CreateUserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@Comment", SqlDbType.VarChar).Value = comment;
                command.Parameters.Add("@ResetUserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;
                command.ExecuteNonQuery();
            }
            catch { throw; }
            finally
            {
                // Dispose.
                
                if (command != null) command.Dispose();
            }

          
        }
    }
}
