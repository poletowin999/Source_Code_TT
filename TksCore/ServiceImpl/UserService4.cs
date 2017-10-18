using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


namespace Tks.ServiceImpl
{
    internal partial class UserService
        : IUserService
    {
        #region Class variables




        #endregion

        public void ChangePassword(string loginName, string oldPassword, string newPassword)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "ChangePassword_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@LoginName", SqlDbType.NVarChar, 100).Value = _appManager.LoginUser.LoginName;
                command.Parameters.Add("@OldPassword", SqlDbType.NVarChar, 25).Value = oldPassword;
                command.Parameters.Add("@NewPassword", SqlDbType.NVarChar, 25).Value = newPassword;
                command.Parameters.Add("@HasError", SqlDbType.Bit).Direction = ParameterDirection.Output;

                // Start the transaction.
                transaction = mDbConnection.BeginTransaction();
                command.Transaction = transaction;

                // Execute
                adapter = new SqlDataAdapter(command);
                DataTable errorDataTable = new DataTable();
                adapter.Fill(errorDataTable);

                // Check whether @HasError is true.
                if (Convert.ToBoolean(command.Parameters["@HasError"].Value.ToString()))
                {
                    // Rollback transaction.
                    transaction.Rollback();

                    // Build exception.
                    ValidationException exception = new ValidationException(string.Empty);
                    // Iterate each row.
                    foreach (DataRow row in errorDataTable.Rows)
                    {
                        exception.Data.Add(row["Key"].ToString(), row["Value"].ToString());
                    }

                    // Throw
                    throw exception;
                }

                // Commit transaction.
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
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }
    }
}

