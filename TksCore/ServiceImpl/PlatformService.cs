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
    internal sealed class PlatformService
        : IPlatformService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        public Platform Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Platform> platforms = this.Retrieve(ids);

                return (platforms.Count > 0) ? platforms[0] : null;
            }
            catch { throw; }
        }

        public List<Platform> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Platforms>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Platform><Id>{0}</Id></Platform>", element));
                }
                xml.Append("</Platforms>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrievePlatforms";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable platformDataTable = new DataTable("Platforms");
                adapter.Fill(platformDataTable);

                // Create a list.
                List<Platform> platforms = null;
                if (platformDataTable.Rows.Count > 0)
                    platforms = new List<Platform>();

                // Iterate each row.
                foreach (DataRow row in platformDataTable.Rows)
                {
                    // Create an instance of Platform.
                    Platform platform = new Platform(Int32.Parse(row["PlatformId"].ToString()));
                    platform.Name = row["Name"].ToString();
                    platform.Description = row["Description"].ToString();
                    platform.Reason = row["Reason"].ToString();
                    platform.IsActive = bool.Parse(row["IsActive"].ToString());
                    platform.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    platform.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    platform.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    platforms.Add(platform);
                }

                // Return the list.
                return platforms;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(Platform entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdatePlatforms";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = entity.GetXml();
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //command.ExecuteNonQuery();

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
                    ValidationException exception = new ValidationException(string.Empty);

                    if (errorDataTable != null)
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format("{1}", row["Name"].ToString(), row["Value"].ToString()));
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
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }
        }


        public List<Platform> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchPlatforms";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable platformDataTable = new DataTable("Platforms");
                adapter.Fill(platformDataTable);

                // Create a list.
                List<Platform> platforms = null;
                if (platformDataTable.Rows.Count > 0)
                    platforms = new List<Platform>();

                // Iterate each row.
                foreach (DataRow row in platformDataTable.Rows)
                {
                    // Create an instance of Platform.
                    Platform platform = new Platform(Int32.Parse(row["PlatformId"].ToString()));
                    platform.Name = row["Name"].ToString();
                    platform.Description = row["Description"].ToString();
                    platform.Reason = row["Reason"].ToString();
                    platform.IsActive = bool.Parse(row["IsActive"].ToString());
                    platform.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    platform.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    platform.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    platforms.Add(platform);
                }

                // Return the list.
                return platforms;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        // Used in project master.
        public List<Platform> RetrieveByProject(int projectId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrievePlatformsByProject";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ProjectId", SqlDbType.Int).Value = projectId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable platformDataTable = new DataTable("Platforms");
                adapter.Fill(platformDataTable);

                // Create a list.
                List<Platform> platforms = null;
                if (platformDataTable.Rows.Count > 0)
                    platforms = new List<Platform>();

                // Iterate each row.
                foreach (DataRow row in platformDataTable.Rows)
                {
                    // Create an instance of Platform.
                    Platform platform = new Platform(Int32.Parse(row["PlatformId"].ToString()));
                    platform.Name = row["Name"].ToString();
                    platform.Description = row["Description"].ToString();
                    platform.Reason = row["Reason"].ToString();
                    platform.IsActive = bool.Parse(row["IsActive"].ToString());
                    platform.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    platform.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());

                    platform.CustomData.Add("ProjectId", row["ProjectId"].ToString());
                    platform.CustomData.Add("IsActive", row["PIsActive"].ToString());
                    platform.CustomData.Add("CreateUserId", row["CreateUserId"].ToString());
                    platform.CustomData.Add("CreateUserName", row["CreateUserName"].ToString());
                    platform.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    platforms.Add(platform);
                }

                // Return the list.
                return platforms;
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


        public List<Platform> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAllPlatforms";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable platformDataTable = new DataTable("Platforms");
                adapter.Fill(platformDataTable);

                // Create a list.
                List<Platform> platforms = null;
                if (platformDataTable.Rows.Count > 0)
                    platforms = new List<Platform>();

                // Iterate each row.
                foreach (DataRow row in platformDataTable.Rows)
                {
                    // Create an instance of Platform.
                    Platform platform = new Platform(Int32.Parse(row["PlatformId"].ToString()));
                    platform.Name = row["Name"].ToString();
                    platform.Description = row["Description"].ToString();
                    platform.Reason = row["Reason"].ToString();
                    platform.IsActive = bool.Parse(row["IsActive"].ToString());
                    platform.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    platform.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    platform.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    platform.CustomData.Add("IsActive", row["PIsActive"].ToString());

                    // Add to list.
                    platforms.Add(platform);
                }

                // Return the list.
                return platforms;
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
