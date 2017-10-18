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
    internal sealed class DepartmentService
        : IDepartmentService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        public Department Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Department> Departments = this.Retrieve(ids);

                return (Departments.Count > 0) ? Departments[0] : null;
            }
            catch { throw; }
        }

        public List<Department> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Departments>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Department><Id>{0}</Id></Department>", element));
                }
                xml.Append("</Departments>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveDepartments_TT";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DepartmentDataTable = new DataTable("Departments");
                adapter.Fill(DepartmentDataTable);

                // Create a list.
                List<Department> Departments = null;
                if (DepartmentDataTable.Rows.Count > 0)
                    Departments = new List<Department>();

                // Iterate each row.
                foreach (DataRow row in DepartmentDataTable.Rows)
                {
                    // Create an instance of Department.
                    Department Department = new Department(Int32.Parse(row["DepartmentId"].ToString()));
                    Department.Name = row["Name"].ToString();
                    Department.Description = row["Description"].ToString();
                    Department.Reason = row["Reason"].ToString();
                    Department.IsActive = bool.Parse(row["IsActive"].ToString());
                    Department.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    Department.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    Department.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    Departments.Add(Department);
                }

                // Return the list.
                return Departments;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(Department entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateDepartments";
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

        public List<Department> Searchdpt(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchDepartments";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DepartmentDataTable = new DataTable("Departments");
                adapter.Fill(DepartmentDataTable);

                // Create a list.
                List<Department> Departments = null;
                if (DepartmentDataTable.Rows.Count > 0)
                    Departments = new List<Department>();

                // Iterate each row.
                foreach (DataRow row in DepartmentDataTable.Rows)
                {
                    // Create an instance of Department.
                    Department Department = new Department(Int32.Parse(row["DepartmentId"].ToString()));
                    Department.Name = row["Name"].ToString();
                    Department.Description = row["Description"].ToString();
                    Department.Reason = row["Reason"].ToString();
                    Department.IsActive = bool.Parse(row["IsActive"].ToString());
                    Department.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    Department.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    Department.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    Departments.Add(Department);
                }

                // Return the list.
                return Departments;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<Department> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchDepartments";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DepartmentDataTable = new DataTable("Departments");
                adapter.Fill(DepartmentDataTable);

                // Create a list.
                List<Department> Departments = null;
                if (DepartmentDataTable.Rows.Count > 0)
                    Departments = new List<Department>();

                // Iterate each row.
                foreach (DataRow row in DepartmentDataTable.Rows)
                {
                    // Create an instance of Department.
                    Department Department = new Department(Int32.Parse(row["DepartmentId"].ToString()));
                    Department.Name = row["Name"].ToString();
                    Department.Description = row["Description"].ToString();
                    Department.Reason = row["Reason"].ToString();
                    Department.IsActive = bool.Parse(row["IsActive"].ToString());
                    Department.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    Department.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    Department.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    Departments.Add(Department);
                }

                // Return the list.
                return Departments;
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
        public List<Department> RetrieveByProject(int projectId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveDepartmentsByProject";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ProjectId", SqlDbType.Int).Value = projectId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DepartmentDataTable = new DataTable("Departments");
                adapter.Fill(DepartmentDataTable);

                // Create a list.
                List<Department> Departments = null;
                if (DepartmentDataTable.Rows.Count > 0)
                    Departments = new List<Department>();

                // Iterate each row.
                foreach (DataRow row in DepartmentDataTable.Rows)
                {
                    // Create an instance of Department.
                    Department Department = new Department(Int32.Parse(row["DepartmentId"].ToString()));
                    Department.Name = row["Name"].ToString();
                    Department.Description = row["Description"].ToString();
                    Department.Reason = row["Reason"].ToString();
                    Department.IsActive = bool.Parse(row["IsActive"].ToString());
                    Department.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    Department.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());

                    Department.CustomData.Add("ProjectId", row["ProjectId"].ToString());
                    Department.CustomData.Add("IsActive", row["PIsActive"].ToString());
                    Department.CustomData.Add("CreateUserId", row["CreateUserId"].ToString());
                    Department.CustomData.Add("CreateUserName", row["CreateUserName"].ToString());
                    Department.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    Departments.Add(Department);
                }

                // Return the list.
                return Departments;
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


        public List<Department> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAllDepartments";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DepartmentDataTable = new DataTable("Departments");
                adapter.Fill(DepartmentDataTable);

                // Create a list.
                List<Department> Departments = null;
                if (DepartmentDataTable.Rows.Count > 0)
                    Departments = new List<Department>();

                // Iterate each row.
                foreach (DataRow row in DepartmentDataTable.Rows)
                {
                    // Create an instance of Department.
                    Department Department = new Department(Int32.Parse(row["DepartmentId"].ToString()));
                    Department.Name = row["Name"].ToString();
                    Department.Description = row["Description"].ToString();
                    Department.Reason = row["Reason"].ToString();
                    Department.IsActive = bool.Parse(row["IsActive"].ToString());
                    Department.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    Department.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    Department.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    Department.CustomData.Add("IsActive", row["PIsActive"].ToString());

                    // Add to list.
                    Departments.Add(Department);
                }

                // Return the list.
                return Departments;
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
