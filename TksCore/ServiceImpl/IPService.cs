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
    internal sealed class IPService : IIPservice
    {

        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion


        public DataTable Search(string IPAddress, string Location, string Linktype, string status)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchIp";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IpAddress", SqlDbType.VarChar, 100).Value = IPAddress;
                command.Parameters.Add("@Location", SqlDbType.VarChar, 100).Value = Location;
                command.Parameters.Add("@Linktype", SqlDbType.VarChar, 100).Value = Linktype;
                command.Parameters.Add("@status", SqlDbType.VarChar, 30).Value = status;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable IPDataTable = new DataTable("IP");
                adapter.Fill(IPDataTable);

                // Return the list.
                return IPDataTable;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(string IPAddress, string Location, string Linktype, string status,string Remarks,int ipid)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;
            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "InsertIpconfig";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Linktype", SqlDbType.VarChar,20).Value = Linktype;
                command.Parameters.Add("@IpAddress", SqlDbType.VarChar, 50).Value = IPAddress;
                command.Parameters.Add("@LocationId", SqlDbType.VarChar, 20).Value = Location;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = Remarks;
                command.Parameters.Add("@UpdateUserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;
                command.Parameters.Add("@Isactive", SqlDbType.Bit).Value = status;
                command.Parameters.Add("@IPId", SqlDbType.Int).Value = ipid;
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

                    //if (errorDataTable != null)
                    //{
                    //    StringBuilder message = new StringBuilder();
                    //    foreach (DataRow row in errorDataTable.Rows)
                    //    {
                    //        message.Append(string.Format("{1}", row["City"].ToString(), row["Value"].ToString()));
                    //    }
                    //    exception.Data.Add("IsExists", message);
                    //}
                    exception.Data.Add("IsExists", "IP Address already Exists");
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
