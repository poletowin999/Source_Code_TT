using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Net.Mail;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


namespace Tks.ServiceImpl
{
    internal partial class UserService
        : IUserService
    {
        public string ResetPasswordRequest(string loginName)
        {
            
            SqlCommand command = null;
            
            User _user;
            string resetId=string.Empty;

            try
            {

                //Get The Login User Name
                _user = this.RetrieveByLoginName(loginName);

                // Define Command.
                if (_user != null)
                {
                    command = new SqlCommand();
                    command = mDbConnection.CreateCommand();
                    command.CommandText = "InsertResetPassword";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = _user.Id;
                    command.Parameters.Add("@ResetId", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();
                    // TODO: Check whether reset id is generated or not.
                    // If it is null or empty then throw validation exception.
                    resetId = command.Parameters["@ResetId"].Value.ToString();
                    if (resetId=="")
                    {
                        ValidationException exception = new ValidationException();
                        exception.Data.Add("Restid", "RestId is not Available");
                        throw exception;
                    }

                }
                else
                {
                    ValidationException exception = new ValidationException();
                    exception.Data.Add("LoginUser", "LoginUser is not Available");
                    throw exception;
                }
                return resetId;
                
            }



            catch { throw; }
        }

        public bool IsPasswordResetIdAvailable(string resetId)
        {
            SqlCommand command = null;

            bool IsPwdAvailable = false;

            try
            {
                // Define command.
              
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "PasswordResetIdAvailable";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ResetId", SqlDbType.VarChar).Value = resetId;
                command.Parameters.Add("@Result", SqlDbType.Bit).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                //return resetId;

                IsPwdAvailable = (bool)command.Parameters["@Result"].Value;

                return IsPwdAvailable;
            }

            catch { throw; }
            finally
            {
                // Dispose.

                if (command != null) command.Dispose();
            }

        }

        public void ResetPassword(string resetId, string eMailId, string newPassword)
        {
            if (IsPasswordResetIdAvailable(resetId))
            {
                SqlCommand command = null;
                SqlDataAdapter adapter = null;
                SqlTransaction transaction = null;
                try
                {
                    System.Guid resetGuid = new Guid(resetId);
                    // Define command.
                    //command = new SqlCommand();
                    command = mDbConnection.CreateCommand();
                    command.CommandText = "UpdatePasswordRequest_new";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@EmailId", SqlDbType.VarChar, 150).Value = eMailId;
                    command.Parameters.Add("@ResetId", SqlDbType.UniqueIdentifier).Value = resetGuid;
                    command.Parameters.Add("@NewPassword", SqlDbType.VarChar, 25).Value = newPassword;
                    command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    // Define adapter.
                    adapter = new SqlDataAdapter(command);
                    DataTable errorDataTable = new DataTable();

                    // Start the transaction.
                    transaction = mDbConnection.BeginTransaction();
                    command.Transaction = transaction;

                    // Execute the command.
                    adapter.Fill(errorDataTable);

                    // Check whether stored procedure returns error or not.
                    if (Convert.ToBoolean(command.Parameters["@HasError"].Value.ToString()))
                    {
                        // Rollback the transaction.
                        transaction.Rollback();

                        // Collect the error.
                        ValidationException exception = new ValidationException("Validation error.");
                        // Iterate each row.
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            exception.Data.Add(row["Key"].ToString(), row["Value"].ToString());
                        }

                        // Throw the exception.
                        throw exception;
                    }

                    // Commit the transaction.
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
                    if (adapter != null) adapter.Dispose();
                    if (command != null) command.Dispose();
                    if (transaction != null) transaction.Dispose();
                }
            }
        }


       


        public User RetrieveByResetId(string resetId)
        {
            SqlCommand command = null;
            //int _userid;
            try
            {
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveUserByResetId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ResetId", SqlDbType.VarChar).Value = resetId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.ExecuteScalar();

                // Return the userid.
                this._UserId = Convert.ToInt32(command.Parameters["@UserId"].Value);
                return this.Retrieve(_UserId);

            }
            catch { throw; }
        }


        public void SendResetPasswordRequestMail(MailMessage message)
        {
            try
            {
                // Fetch values from Web.Config file.
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

                // Create to smtp client.
                SmtpClient client = new SmtpClient();
                if (mailSettings != null)
                {
                    client.Host = mailSettings.Smtp.Network.Host;
                    client.Port = mailSettings.Smtp.Network.Port;
                    client.Credentials = new NetworkCredential(Utility.ConvertASCII2String(mailSettings.Smtp.Network.UserName), Utility.ConvertASCII2String(mailSettings.Smtp.Network.Password));
                    client.EnableSsl = true;
                }
                client.Send(message);
            }
            catch (SmtpFailedRecipientException ex)
            {
                throw new Exception("Mail Delivered Failed",ex);
            }
            // TODO: Handler mail failures.
            catch { throw; }
        }
    }
}
