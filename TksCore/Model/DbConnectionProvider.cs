using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Tks.Model
{
    /// <summary>
    /// Provides the application database connections.
    /// </summary>
    public sealed class DbConnectionProvider
    {
        #region Class variables

        const string DEFAULT_CONNECTION_STRING_SETTING_NAME = "DEFAULT_DATABASE_CONNECTION_STRING";

        IAppManager mAppManager;
        int mAttemptCount;

        #endregion

        #region Constructor

        internal DbConnectionProvider(IAppManager appManager)
        {
            // In future, suppose this application give to multiple clients then 
            // IAppManager helps us to provide client/company database details.
            mAppManager = appManager;
            mAttemptCount = 0;
        }

        #endregion


        /// <summary>
        /// Provides the default database connection string information.
        /// </summary>
        /// <returns></returns>
        public SqlConnectionStringBuilder GetDefaultDbConnectionString()
        {
            try
            {
                // Get DPS connection string from configuration file.
                string connectionString = ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTION_STRING_SETTING_NAME].ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                    throw new Exception("Default database connection string setting is not defined in configuration file.");

                // Create instance.
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                string Password;
                string txtPassword;
                txtPassword = "";
                char txtPassword1;
                int lenPassword = builder.Password.Length;
                for (int lengthpwd = 0; lengthpwd < lenPassword; )
                {
                    Password = builder.Password.Substring(lengthpwd, 4);
                    txtPassword1 = Convert.ToChar(Convert.ToInt32(Password) - 1000);
                    char chargthpwd = (char)txtPassword1;
                    txtPassword = txtPassword + chargthpwd.ToString();
                    lengthpwd = lengthpwd + 4;
                }
                builder.Password = txtPassword;

                string DBIP;
                string texttxtDBIP;
                texttxtDBIP = "";
                char DBIP1;
                int lenDBIP = builder.DataSource.Length;
                for (int lengthDBIP = 0; lengthDBIP < lenDBIP; )
                {
                    DBIP = builder.DataSource.Substring(lengthDBIP, 4);
                    DBIP1 = Convert.ToChar(Convert.ToInt32(DBIP) - 1000);
                    char characterDBIP = (char)DBIP1;
                    texttxtDBIP = texttxtDBIP + characterDBIP.ToString();
                    lengthDBIP = lengthDBIP + 4;
                }
                builder.DataSource = texttxtDBIP;

                string server;
                string txtuserid = builder.UserID;
                string textuserid;
                textuserid = "";
                char server1;
                int len = builder.UserID.Length;
                for (int length = 0; length < len; )
                {
                    server = builder.UserID.Substring(length, 4);
                    server1 = Convert.ToChar(Convert.ToInt32(server) - 1000);
                    char character = (char)server1;
                    textuserid = textuserid + character.ToString();
                    length = length + 4;
                }
                builder.UserID = textuserid;


                return builder;
            }
            catch
            {
                // TODO: Log exception.
                throw;
            }
        }

        /// <summary>
        /// Provides the default database connection instance.
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetDefaultDbConnectionInstance()
        {
            SqlConnection connection = null;
            try
            {
                // Create instance.
                connection = new SqlConnection(this.GetDefaultDbConnectionString().ToString());
                try
                {
                    // Open database connection.
                    connection.Open();
                }
                catch (System.InvalidOperationException ioe)
                {
                    System.Diagnostics.Debug.Assert(false);
                    System.Diagnostics.Debug.Print("Inside GetDefaultConnectionInstance(): " + ioe.Message);

                    // Connection pool reached max.
                    if (ioe.Message.IndexOf("max pool size was reached") > -1)
                    {
                        SqlConnection.ClearPool(connection);
                        SqlConnection.ClearAllPools();

                        GC.Collect();

                        // Try once again.
                        if (mAttemptCount <= 3)
                        {
                            // Wait.
                            System.Threading.Thread.Sleep(1000);

                            // Increment the trial count.
                            mAttemptCount++;
                            System.Diagnostics.Debug.Print(string.Format("Default db connection trial count: {0}", mAttemptCount));

                            return this.GetDefaultDbConnectionInstance();
                        }
                    }
                    throw;
                }
                catch { throw; }

                // Return the instance.
                return connection;
            }
            catch
            {
                // TODO: Log the exception.
                throw;
            }
        }

    }
}
