using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;


namespace Tks.Model
{
    public sealed class ErrorLogProvider
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

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

        public void Insert1(Exception exception)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            try
            {
                // Create the command.
                command = this.mDbConnection.CreateCommand();
                command.CommandText = "InsertErrorLog";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Message", SqlDbType.VarChar, 1000).Value = "TRYING TO ACCESS THE ADMIN PAGE THROUGH URL";
                command.Parameters.Add("@Source", SqlDbType.VarChar, 1000).Value = "SYSTEM";
                command.Parameters.Add("@StackTrace", SqlDbType.Text).Value = "URL ACCESS";
                if (this._appManager.LoginUser == null)
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = DBNull.Value;
                else
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = this._appManager.LoginUser.Id;

                // Execute command.
                transaction = this.mDbConnection.BeginTransaction();
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                // Rollback the transaction.
                if (transaction != null)
                    if (transaction.Connection != null) transaction.Rollback();

                throw;
            }
            finally
            {
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Insert(Exception exception)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            try
            {
                // Create the command.
                command = this.mDbConnection.CreateCommand();
                command.CommandText = "InsertErrorLog";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Message", SqlDbType.VarChar, 1000).Value = exception.Message;
                command.Parameters.Add("@Source", SqlDbType.VarChar, 1000).Value = exception.Source;
                command.Parameters.Add("@StackTrace", SqlDbType.Text).Value = exception.StackTrace;
                if (this._appManager.LoginUser == null)
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = DBNull.Value;
                else
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = this._appManager.LoginUser.Id;

                // Execute command.
                transaction = this.mDbConnection.BeginTransaction();
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch
            {
                // Rollback the transaction.
                if (transaction != null)
                    if (transaction.Connection != null) transaction.Rollback();

                throw;
            }
            finally
            {
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
                //HttpContext.Current.Response.Redirect("~/Misc/AppExpireView.aspx"); 
            }
        }


    }
}
