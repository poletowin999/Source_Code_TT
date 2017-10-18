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
    internal sealed class WorkTypeService
        : IWorkTypeService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        public WorkType Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<WorkType> workTypes = this.Retrieve(ids);

                return (workTypes.Count > 0) ? workTypes[0] : null;
            }
            catch { throw; }
        }

        public List<WorkType> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<WorkTypes>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<WorkType><Id>{0}</Id></WorkType>", element));
                }
                xml.Append("</WorkTypes>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveWorkTypes";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable workTypeDataTable = new DataTable("WorkTypes");
                adapter.Fill(workTypeDataTable);

                // Create a list.
                List<WorkType> workTypes = null;
                if (workTypeDataTable.Rows.Count > 0)
                    workTypes = new List<WorkType>();

                // Iterate each row.
                foreach (DataRow row in workTypeDataTable.Rows)
                {
                    // Create an instance of WorkType.
                    WorkType workType = new WorkType(Int32.Parse(row["WorkTypeId"].ToString()));
                    workType.Name = row["Name"].ToString();
                    workType.Description = row["Description"].ToString();
                    workType.Reason = row["Reason"].ToString();
                    workType.IsActive = bool.Parse(row["IsActive"].ToString());
                    workType.ConsiderForReport = bool.Parse(row["ConsiderForReport"].ToString());
                    workType.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    workType.ActivityTypeId = Int32.Parse(row["ActivityTypeId"].ToString());
                    workType.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    workType.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    workType.CustomData.Add("ActivityName", row["ActivityName"].ToString());

                    // Add to list.
                    workTypes.Add(workType);
                }

                // Return the list.
                return workTypes;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(WorkType entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateWorkTypes";
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


        public List<WorkType> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchWorkTypes";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable workTypeDataTable = new DataTable("WorkTypes");
                adapter.Fill(workTypeDataTable);

                // Create a list.
                List<WorkType> workTypes = null;
                if (workTypeDataTable.Rows.Count > 0)
                    workTypes = new List<WorkType>();

                // Iterate each row.
                foreach (DataRow row in workTypeDataTable.Rows)
                {
                    // Create an instance of WorkType.
                    WorkType workType = new WorkType(Int32.Parse(row["WorkTypeId"].ToString()));
                    workType.Name = row["Name"].ToString();
                    workType.Description = row["Description"].ToString();
                    workType.Reason = row["Reason"].ToString();
                    workType.IsActive = bool.Parse(row["IsActive"].ToString());
                    workType.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    workType.ActivityTypeId = Int32.Parse(row["ActivityTypeId"].ToString());
                    workType.ConsiderForReport = bool.Parse(row["ConsiderForReport"].ToString());
                    workType.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    workType.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    workType.CustomData.Add("ActivityName", row["ActivityName"].ToString());

                    // Add to list.
                    workTypes.Add(workType);
                }

                // Return the list.
                return workTypes;
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


        public List<WorkType> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAllWorkTypes";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable workTypeDataTable = new DataTable("WorkTypes");
                adapter.Fill(workTypeDataTable);

                // Create a list.
                List<WorkType> workTypes = null;
                if (workTypeDataTable.Rows.Count > 0)
                    workTypes = new List<WorkType>();

                // Iterate each row.
                foreach (DataRow row in workTypeDataTable.Rows)
                {
                    // Create an instance of WorkType.
                    WorkType workType = new WorkType(Int32.Parse(row["WorkTypeId"].ToString()));
                    workType.Name = row["Name"].ToString();
                    workType.Description = row["Description"].ToString();
                    workType.Reason = row["Reason"].ToString();
                    workType.IsActive = bool.Parse(row["IsActive"].ToString());
                    workType.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    workType.ActivityTypeId = Int32.Parse(row["ActivityTypeId"].ToString());
                    workType.ConsiderForReport = bool.Parse(row["ConsiderForReport"].ToString());
                    workType.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    workType.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    workType.CustomData.Add("ActivityName", row["ActivityName"].ToString());
                    workType.CustomData.Add("DefaultHours", row["DefaultHours"].ToString());
                    // Add to list.
                    workTypes.Add(workType);
                }

                // Return the list.
                return workTypes;
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
