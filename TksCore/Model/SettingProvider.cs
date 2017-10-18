using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace Tks.Model
{
    public sealed class SettingProvider
    {
        #region Class Variable
        SqlConnection mDbconnection;
        IAppManager _appManager;
        #endregion
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
                    mDbconnection = this._appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
            }
        }

        public DateTime GetSystemDateTime()
        {
            SqlCommand command = null;

            try
            {
                //Define a Command.
                command = mDbconnection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT GETDATE()";

                // Get system current date time.
                DateTime currentdate = Convert.ToDateTime(command.ExecuteScalar());

                //return current date.
                return currentdate;
            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose.
                if (command != null) { command.Dispose(); }
            }

        }

        public DateTime GetSystemUtcDateTime()
        {
            SqlCommand command = null;

            try
            {
                //Define a Command.
                command = mDbconnection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT GETUTCDATE()";

                // Get system current date time.
                DateTime currentdate = Convert.ToDateTime(command.ExecuteScalar());

                //return current date.
                return currentdate;
            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose.
                if (command != null) { command.Dispose(); }
            }

        }
        public void Dispose()
        {
            try
            {
                // Dispose.
                if (mDbconnection != null)
                {
                    if (mDbconnection.State != ConnectionState.Closed)
                        mDbconnection.Close();
                    mDbconnection.Dispose();

                    // Clear pool.
                    SqlConnection.ClearPool(mDbconnection);
                    SqlConnection.ClearAllPools();
                }
            }
            catch { throw; }
        }
    }
}
