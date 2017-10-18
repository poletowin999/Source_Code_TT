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
    internal sealed class RoleService
        : IRoleService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        public Role Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Role> roles = this.Retrieve(ids);

                return (roles.Count > 0) ? roles[0] : null;
            }
            catch { throw; }
        }

        public List<Role> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Roles>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Role><Id>{0}</Id></Role>", element));
                }
                xml.Append("</Roles>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveRoles";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable roleDataTable = new DataTable("Roles");
                adapter.Fill(roleDataTable);

                // Create a list.
                List<Role> roles = null;
                if (roleDataTable.Rows.Count > 0)
                    roles = new List<Role>();

                // Iterate each row.
                foreach (DataRow row in roleDataTable.Rows)
                {
                    // Create an instance of Role.
                    Role role = new Role(Int32.Parse(row["RoleId"].ToString()));
                    role.Name = row["Name"].ToString();
                    role.Description = row["Description"].ToString();
                    role.Reason = row["Reason"].ToString();
                    role.IsActive = bool.Parse(row["IsActive"].ToString());
                    role.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    role.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    role.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    roles.Add(role);
                }

                // Return the list.
                return roles;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(Role entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                adapter = new SqlDataAdapter();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateRoles";
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

        
        public List<Role> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchRoles";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable roleDataTable = new DataTable("Roles");
                adapter.Fill(roleDataTable);

                // Create a list.
                List<Role> roles = null;
                if (roleDataTable.Rows.Count > 0)
                    roles = new List<Role>();

                // Iterate each row.
                foreach (DataRow row in roleDataTable.Rows)
                {
                    // Create an instance of Role.
                    Role role = new Role(Int32.Parse(row["RoleId"].ToString()));
                    role.Name = row["Name"].ToString();
                    role.Description = row["Description"].ToString();
                    role.Reason = row["Reason"].ToString();
                    role.IsActive = bool.Parse(row["IsActive"].ToString());
                    role.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    role.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    role.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    roles.Add(role);
                }

                // Return the list.
                return roles;
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
    }
}
