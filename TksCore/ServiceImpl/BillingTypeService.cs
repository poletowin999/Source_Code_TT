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
    internal sealed class BillingTypeService
        : IBillingTypeService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        public BillingType Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<BillingType> billingTypes = this.Retrieve(ids);

                return (billingTypes.Count > 0) ? billingTypes[0] : null;
            }
            catch { throw; }
        }

        public List<BillingType> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<BillingTypes>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<BillingType><Id>{0}</Id></BillingType>", element));
                }
                xml.Append("</BillingTypes>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveBillingTypes";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable billingTypeDataTable = new DataTable("BillingTypes");
                adapter.Fill(billingTypeDataTable);

                // Create a list.
                List<BillingType> billingTypes = null;
                if (billingTypeDataTable.Rows.Count > 0)
                    billingTypes = new List<BillingType>();

                // Iterate each row.
                foreach (DataRow row in billingTypeDataTable.Rows)
                {
                    // Create an instance of BillingType.
                    BillingType billingType = new BillingType(Int32.Parse(row["BillingTypeId"].ToString()));
                    billingType.Name = row["Name"].ToString();
                    billingType.Description = row["Description"].ToString();
                    billingType.Reason = row["Reason"].ToString();
                    billingType.IsActive = bool.Parse(row["IsActive"].ToString());
                    billingType.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    billingType.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    billingType.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    billingTypes.Add(billingType);
                }

                // Return the list.
                return billingTypes;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(BillingType entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateBillingTypes";
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


        public List<BillingType> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchBillingTypes";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable billingTypeDataTable = new DataTable("BillingTypes");
                adapter.Fill(billingTypeDataTable);

                // Create a list.
                List<BillingType> billingTypes = null;
                if (billingTypeDataTable.Rows.Count > 0)
                    billingTypes = new List<BillingType>();

                // Iterate each row.
                foreach (DataRow row in billingTypeDataTable.Rows)
                {
                    // Create an instance of BillingType.
                    BillingType billingType = new BillingType(Int32.Parse(row["BillingTypeId"].ToString()));
                    billingType.Name = row["Name"].ToString();
                    billingType.Description = row["Description"].ToString();
                    billingType.Reason = row["Reason"].ToString();
                    billingType.IsActive = bool.Parse(row["IsActive"].ToString());
                    billingType.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    billingType.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    billingType.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    billingTypes.Add(billingType);
                }

                // Return the list.
                return billingTypes;
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


        public List<BillingType> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAllBillingTypes";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable billingTypeDataTable = new DataTable("BillingTypes");
                adapter.Fill(billingTypeDataTable);

                // Create a list.
                List<BillingType> billingTypes = null;
                if (billingTypeDataTable.Rows.Count > 0)
                    billingTypes = new List<BillingType>();

                // Iterate each row.
                foreach (DataRow row in billingTypeDataTable.Rows)
                {
                    // Create an instance of BillingType.
                    BillingType billingType = new BillingType(Int32.Parse(row["BillingTypeId"].ToString()));
                    billingType.Name = row["Name"].ToString();
                    billingType.Description = row["Description"].ToString();
                    billingType.Reason = row["Reason"].ToString();
                    billingType.IsActive = bool.Parse(row["IsActive"].ToString());
                    billingType.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    billingType.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    billingType.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    billingTypes.Add(billingType);
                }

                // Return the list.
                return billingTypes;
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
