using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace Tks.Model
{
    public sealed partial class DashboardProvider
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        #region Fields

        public readonly static string USER_WORK_DURATION = "User Work Duration";
        public readonly static string USER_ACTIVITY_STATUS = "User Activity Status";

        #endregion

        public IAppManager AppManager
        {
            get { return this._appManager; }
            set
            {
                this._appManager = value;
                this.mDbConnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
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

        public SqlCommand CreateItemCommand(string itemName)
        {
            SqlCommand command = null;
            try
            {
                // Create the instance.
                command = this.mDbConnection.CreateCommand();

                if (itemName.Equals(DashboardProvider.USER_WORK_DURATION, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Define parameters.
                    command.CommandText = "RetrieveDbUserWorkDuration";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int);   

                }
                else if (itemName.Equals(DashboardProvider.USER_ACTIVITY_STATUS, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Define parameters.
                    command.CommandText = "RetrieveDbActivityStatus";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int);                   
                }
                else
                {
                    // item name not found.
                    if (command != null) command.Dispose();
                    command = null;
                }

                // Return the command instance.
                return command;
            }
            catch { throw; }
        }


        public SqlCommand CreateItemCommand_Report(string itemName)
        {
            SqlCommand command = null;
            try
            {
                // Create the instance.
                command = this.mDbConnection.CreateCommand();

                if (itemName.Equals(DashboardProvider.USER_WORK_DURATION, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Define parameters.
                    command.CommandText = "RetrieveDbUserWorkDuration_Dashboard";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int);
                    command.Parameters.Add("@BeginDate", SqlDbType.DateTime);
                    command.Parameters.Add("@EndDate", SqlDbType.DateTime);

                }
                else if (itemName.Equals(DashboardProvider.USER_ACTIVITY_STATUS, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Define parameters.
                    command.CommandText = "RetrieveDbActivityStatus_Dashboard";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int);
                    command.Parameters.Add("@BeginDate", SqlDbType.DateTime);
                    command.Parameters.Add("@EndDate", SqlDbType.DateTime);
                }
                else
                {
                    // item name not found.
                    if (command != null) command.Dispose();
                    command = null;
                }

                // Return the command instance.
                return command;
            }
            catch { throw; }
        }

        public DataSet RetrieveItemData(SqlCommand command)
        {
            SqlDataAdapter adapter = null;
            try
            {
                if (command == null)
                    throw new ArgumentException();

                // Execute the command.
                command.Connection = this.mDbConnection;
                adapter = new SqlDataAdapter(command);
                DataSet itemData = new DataSet("Databoard Item Data");
                adapter.Fill(itemData);

                // Return the dataset.
                return itemData;
            }
            catch { throw; }
            finally
            {
                // Dispose
                if (adapter != null) adapter.Dispose();
                adapter = null;
                // Don't dispose command instance.
            }
        }
    }
}
