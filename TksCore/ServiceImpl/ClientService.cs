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
    internal sealed class ClientService
        : IClientService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion
        public Client Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Client > clients = this.Retrieve(ids);

                return (clients.Count > 0) ? clients[0] : null;
            }
            catch { throw; }
        }

        public List<Client> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Clients>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Client><Id>{0}</Id></Client>", element));
                }
                xml.Append("</Clients>");

                // Define command.
                mDbConnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveClients";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable clientDataTable = new DataTable("Clients");
                adapter.Fill(clientDataTable);

                // Create a list.
                List<Client> clients  = null;
                if (clientDataTable.Rows.Count > 0)
                    clients = new List<Client>();

                // Iterate each row.
                foreach (DataRow row in clientDataTable.Rows)
                {
                    // Create an instance of Client.
                    Client  client = new  Client (Int32.Parse(row["ClientId"].ToString()));
                    client.Name = row["Name"].ToString();
                    client.Description = row["Description"].ToString();
                    client.ResponsibleUserId=Int32.Parse(row["ResponsibleUserId"].ToString());
                    client.Reason = row["Reason"].ToString();
                    client.IsActive = bool.Parse(row["IsActive"].ToString());
                    client.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    client.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    
                    // Add to list.
                    clients.Add(client);
                }

                // Return the list.
                return clients;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(Client entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;
            try
            {
                //connection.
                //mDbConnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateClients";
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
                    //ValidationException exception = new ValidationException("Validation error(s) occurred.");
                    ValidationException exception = new ValidationException("");

                    if (errorDataTable != null)
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format( row["Value"].ToString()));
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

        public List<Client> Search(SearchCriteria criteria, int UserId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                mDbConnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchClients_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value =criteria.GetXml();
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
               
                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable clientDataTable = new DataTable("Clients");
                adapter.Fill(clientDataTable);

                // Create a list.
                List<Client> clients  = null;
                if (clientDataTable.Rows.Count > 0)
                    clients = new List<Client>();

                // Iterate each row.
                foreach (DataRow row in clientDataTable.Rows)
                {
                    // Create an instance of Client
                    Client  client = new  Client (Int32.Parse(row["ClientId"].ToString()));
                    client.Name = row["Name"].ToString();
                    if (row["Description"].ToString() != "")
                        client.Description = row["Description"].ToString();
                    else
                        client.Description = null;
                    client.ResponsibleUserId=Int32.Parse(row["ResponsibleUserId"].ToString());
                    if (row["Reason"].ToString() != "")
                        client.Reason = row["Reason"].ToString();
                    else
                        client.Reason = null;
                    client.IsActive= bool.Parse(row["IsActive"].ToString());
                    client.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    if (client.LastUpdateDate != null)
                    client.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    client.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    client.CustomData.Add("ResponsibleUserName",row["ResponsibleUserName"].ToString());
                    // Add to list.
                    clients.Add(client);
                }

                // Return the list.
                return clients;
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
