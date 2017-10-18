using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;
using Tks.Entities;
using Tks.Model;
using Tks.Services;
using System.Web;
namespace Tks.ServiceImpl
{

    internal sealed class UsersProfile : IUsersProfile
    {

        #region Class Variables

        IAppManager mAppManager;
        SqlConnection mDbConnection;

        #endregion

        public IAppManager AppManager
        {
            get
            {
                return this.mAppManager;
            }
            set
            {
                mAppManager = value;
                if (value != null)
                    mDbConnection = this.mAppManager.DbConnectionProvider.GetDefaultDbConnectionInstance();

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

        public DataTable RetrieveLocations()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtLocations = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUsersProfileLocations";
                command.CommandType = CommandType.StoredProcedure;                

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtLocations = new DataTable("Locations");
                adapter.Fill(dtLocations);

                // Return locations.
                if (dtLocations.Rows.Count > 0)
                    return dtLocations;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveDepartments()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtDepts = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUsersProfileDepartments";
                command.CommandType = CommandType.StoredProcedure;                

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtDepts = new DataTable("Departments");
                adapter.Fill(dtDepts);

                // Return departments.
                if (dtDepts.Rows.Count > 0)
                    return dtDepts;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveClients()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUsersProfileClients";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Clients");
                adapter.Fill(dtClients);

                // Return departments.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveReports(int LanguageId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtReprots = null;
           
            try
            {              
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUsersProfileReports";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_LanguageId", SqlDbType.Int).Value = LanguageId;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtReprots = new DataTable("Reports");
                adapter.Fill(dtReprots);

                // Return reports.
                if (dtReprots.Rows.Count > 0)
                    return dtReprots;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUserPermissionById(int PermissionId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUserPermissionById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_PermissionId", SqlDbType.Int).Value = PermissionId;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("UserPermission");
                adapter.Fill(dt);

                // Return reports.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }


        public DataTable RetrieveUserPermissionAccess(int PermissionId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUserPermissionAccess";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_PermissionId", SqlDbType.Int).Value = PermissionId;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("PermissionAccess");
                adapter.Fill(dt);

                // Return reports.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUserClientAccess(int PermissionId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUserClientAccess";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_PermissionId", SqlDbType.Int).Value = PermissionId;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("ClientAccess");
                adapter.Fill(dt);

                // Return reports.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUserReportAccess(int LanguageId,int PermissionId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveUserReportAccess";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@p_LanguageId", SqlDbType.Int).Value = LanguageId;
                command.Parameters.Add("@p_PermissionId", SqlDbType.Int).Value = PermissionId;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("ReportAccess");
                adapter.Fill(dt);

                // Return reports.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }


        public List<UsersPermission> SearchPermissions(string PermissionNo, string Status)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<UsersPermission> UsersPermissionList = null;

            UsersPermission _objUsersPermission = null;
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchUserPermissions";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@PermissionNo", SqlDbType.NVarChar).Value = PermissionNo;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                adapter = new SqlDataAdapter(command);
                //Define the DataTable.
                DataTable UsersPermissionDt = new DataTable();
                adapter.Fill(UsersPermissionDt);
                if (UsersPermissionDt.Rows.Count > 0)
                {
                    UsersPermissionList = new List<UsersPermission>(UsersPermissionDt.Rows.Count);
                    foreach (DataRow row in UsersPermissionDt.Rows)
                    {
                        //create a  instances for Project.
                        _objUsersPermission = new UsersPermission(Convert.ToInt32(row["PermissionId"]));
                        _objUsersPermission.PermissionId = Convert.ToInt32(row["PermissionId"]);
                        _objUsersPermission.PermissionNo = Convert.ToString(row["PermissionNo"]);
                        _objUsersPermission.PermissionName = Convert.ToString(row["PermissionName"]);
                        _objUsersPermission.CustomData.Add("LocationNames", Convert.ToString(row["LocationNames"]));
                        _objUsersPermission.IsActive = bool.Parse(Convert.ToString(row["IsActive"]));
                        _objUsersPermission.CustomData.Add("LastUpdateUserName", Convert.ToString(row["LastUpdateUserName"]));
                        _objUsersPermission.LastUpdateDate = DateTime.Parse(Convert.ToString(row["LastUpdateDate"]));                       

                        //Add to the List
                        UsersPermissionList.Add(_objUsersPermission);
                    }

                }

                return UsersPermissionList;
            }


            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }


            }
        }

        public List<AccessLevel> SearchAccessLevel(string AccessLevelName, string Status)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<AccessLevel> UsersAccessLevelList = null;

            AccessLevel _objAccessLevel = null;
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchAccessLevels";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AccessLevel", SqlDbType.NVarChar).Value = AccessLevelName;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = Status;
                adapter = new SqlDataAdapter(command);
                //Define the DataTable.
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    UsersAccessLevelList = new List<AccessLevel>(dt.Rows.Count);
                    foreach (DataRow row in dt.Rows)
                    {
                        //create a  instances for Project.
                        _objAccessLevel = new AccessLevel(Convert.ToInt32(row["AccessLevelId"]));                        
                        _objAccessLevel.AccessLevelName = Convert.ToString(row["AccessLevelName"]);
                        _objAccessLevel.AccessLevelDescription = Convert.ToString(row["AccessLevelDescription"]);
                        _objAccessLevel.CustomData.Add("PermissionIds", Convert.ToString(row["PermissionIds"]));
                        _objAccessLevel.IsActive = bool.Parse(Convert.ToString(row["IsActive"]));
                        _objAccessLevel.CustomData.Add("LastUpdateUserName", Convert.ToString(row["LastUpdateUserName"]));
                        _objAccessLevel.LastUpdateDate = DateTime.Parse(Convert.ToString(row["LastUpdateDate"]));

                        //Add to the List
                        UsersAccessLevelList.Add(_objAccessLevel);
                    }

                }

                return UsersAccessLevelList;
            }


            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }


            }
        }


        public DataTable RetrieveAllPermissions()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveAllPermissions";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("Permissions");
                adapter.Fill(dt);

                // Return departments.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }


        public DataTable RetrieveAllAccessLevel()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveAllAccessLevel";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("AccessLevels");
                adapter.Fill(dt);

                // Return departments.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveAccessLevelDetailsById(int AccessLevelId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dt = null;

            try
            {
                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveAccessLevelDetailsById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AccessLevelId", SqlDbType.Int).Value = AccessLevelId;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dt = new DataTable("AccessLevelDetails");
                adapter.Fill(dt);

                // Return reports.
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public void AddEditUserPermissions(UsersPermission Permission, List<PermissionAccess> PermissionAccessList, List<ClientAccess> ClientAccessList, List<ReportAccess> ReportAccessList)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            StringBuilder process = null;

            try
            {
                process = new StringBuilder();
                // Build xml criteria.

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Proc_AddEditUserPermissions";

                command.Parameters.Add("@DataPermission", SqlDbType.Xml).Value = Permission.GetXml();                
                command.Parameters.Add("@DataPermissionAccess", SqlDbType.Xml).Value = PermissionAccess.GetXml(PermissionAccessList);
                command.Parameters.Add("@DataClientAccess", SqlDbType.Xml).Value = ClientAccess.GetXml(ClientAccessList);
                command.Parameters.Add("@DataReportAccess", SqlDbType.Xml).Value = ReportAccess.GetXml(ReportAccessList);                              
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                command.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                try
                {
                    // Execute command within transaction.
                    transaction = mDbConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    if (transaction != null)
                        if (transaction.Connection != null)
                            transaction.Rollback();

                    throw;
                }

                // Get output parameters.
                bool IshasError = bool.Parse(command.Parameters["@hasError"].Value.ToString());
                string errorMessage = command.Parameters["@ErrorMessage"].Value.ToString();

                if (IshasError)
                {
                    // Create exception instance.
                    ValidationException exception = new ValidationException("Validation error occurred.");
                    exception.Data.Add("PERMISSION_Error", errorMessage);

                    throw exception;
                }

            }
            catch { throw; }
            finally
            {
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }

        }

        public void AddEditAccessLevelDetails(List<AccessLevelDetails> AccessLevelDetailsList)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            StringBuilder process = null;

            try
            {
                process = new StringBuilder();
                // Build xml criteria.

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Proc_AddEditUserAccessLevelDetails";

                command.Parameters.Add("@DataAccessLevelDetails", SqlDbType.Xml).Value = PermissionAccess.GetXml(AccessLevelDetailsList);
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                command.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                try
                {
                    // Execute command within transaction.
                    transaction = mDbConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    if (transaction != null)
                        if (transaction.Connection != null)
                            transaction.Rollback();

                    throw;
                }

                // Get output parameters.
                bool IshasError = bool.Parse(command.Parameters["@hasError"].Value.ToString());
                string errorMessage = command.Parameters["@ErrorMessage"].Value.ToString();

                if (IshasError)
                {
                    // Create exception instance.
                    ValidationException exception = new ValidationException("Validation error occurred.");
                    exception.Data.Add("AccessLevel_Error", errorMessage);

                    throw exception;
                }
              

            }
            catch { throw; }
            finally
            {
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }

        }
     }
}
