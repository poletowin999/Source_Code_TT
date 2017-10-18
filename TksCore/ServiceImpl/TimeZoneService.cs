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
    internal sealed class TimeZoneService
        : ITimeZoneService
    {
        #region Class variable
        SqlConnection mDbConnection = null;
        IAppManager _appManager = null;

        #endregion

        public Entities.TimeZone Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Entities.TimeZone> TimeZones = this.Retrieve(ids);
                //return the TimeZone Data
                return (TimeZones.Count > 0 ? TimeZones[0] : null);
            }
            catch { throw; }


        }

        public List<Entities.TimeZone> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                StringBuilder xml = new StringBuilder();
                xml.Append("<TimeZones>");
                foreach (int id in ids)
                {
                    xml.Append(String.Format("<TimeZone><Id>{0}</Id></TimeZone>", id));

                }
                xml.Append("</TimeZones>");

                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveTimeZones";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@data", SqlDbType.Xml).Value = xml.ToString();
                adapter = new SqlDataAdapter(command);
                DataTable TimeZoneDataTable = new DataTable("TimeZone");
                adapter.Fill(TimeZoneDataTable);

                //create a List 
                List<Entities.TimeZone> TimeZones = new List<Entities.TimeZone>();

                // Iterate each row.
                foreach (DataRow row in TimeZoneDataTable.Rows)
                {
                    // Create an instance of TimeZones.

                    Entities.TimeZone timezone = new Entities.TimeZone(Int32.Parse(row["TimeZoneId"].ToString()));
                    timezone.Name = row["Name"].ToString();
                    timezone.ShortName = row["ShortName"].ToString();
                    timezone.Description = row["Description"].ToString();
                    timezone.Reason = row["Reason"].ToString();
                    timezone.IsActive = bool.Parse(row["IsActive"].ToString());
                    if (timezone.LastUpdateUserId >= 0)
                        timezone.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    if (timezone.LastUpdateDate != null)
                        timezone.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    timezone.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    // Add to the List
                    TimeZones.Add(timezone);
                }

                // Return the list.
                return TimeZones;

            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }


        }

        public void Update(Entities.TimeZone entity)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;

            try
            {
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateTimeZones";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = entity.GetXml();
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
                        StringBuilder message = new StringBuilder();
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format("{0}", row["Value"].ToString()));
                        }
                        exception.Data.Add("IsExists", message);
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

                if (adapter != null) adapter.Dispose();
                if (transaction != null) adapter.Dispose();
                if (command != null) command.Dispose();


            }
        }

        public List<Entities.TimeZone> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                //create a instance of sqlcommand
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchTimeZones";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();
                //create a instanse of SqlAdapter 
                adapter = new SqlDataAdapter(command);
                //create a instance of Datatable
                DataTable TimeZoneDatatable = new DataTable();
                adapter.Fill(TimeZoneDatatable);

                //create a List 
                List<Entities.TimeZone> TimeZones = new List<Entities.TimeZone>();

                // Iterate each row.
                foreach (DataRow row in TimeZoneDatatable.Rows)
                {
                    // Create an instance of Role.
                    Entities.TimeZone timezone = new Entities.TimeZone(Int32.Parse(row["TimeZoneId"].ToString()));
                    timezone.Name = row["Name"].ToString();
                    timezone.ShortName = row["ShortName"].ToString();
                    timezone.Description = row["Description"].ToString();
                    timezone.Reason = row["Reason"].ToString();
                    timezone.IsActive = bool.Parse(row["IsActive"].ToString());
                    if (timezone.LastUpdateUserId >= 0)
                        timezone.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    if (timezone.LastUpdateDate != null)
                        timezone.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    timezone.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    // Add to the List
                    TimeZones.Add(timezone);


                }
                // Return the list.
                return TimeZones;




            }
            catch { throw; }
            finally
            {
                // Dispose.

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


        public List<Entities.TimeZone> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAllTimeZones";
                command.CommandType = CommandType.StoredProcedure;
                adapter = new SqlDataAdapter(command);
                DataTable TimeZoneDataTable = new DataTable("TimeZone");
                adapter.Fill(TimeZoneDataTable);

                //create a List 
                List<Entities.TimeZone> TimeZones = null;
                if (TimeZoneDataTable.Rows.Count > 0)
                    TimeZones = new List<Entities.TimeZone>();

                // Iterate each row.
                foreach (DataRow row in TimeZoneDataTable.Rows)
                {
                    // Create an instance of TimeZones.

                    Entities.TimeZone timezone = new Entities.TimeZone(Int32.Parse(row["TimeZoneId"].ToString()));
                    timezone.Name = row["Name"].ToString();
                    timezone.ShortName = row["ShortName"].ToString();
                    timezone.Description = row["Description"].ToString();
                    timezone.Reason = row["Reason"].ToString();
                    timezone.IsActive = bool.Parse(row["IsActive"].ToString());
                    timezone.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    timezone.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    
                    
                    // Add to the List
                    TimeZones.Add(timezone);
                }

                // Return the list.
                return TimeZones;

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
