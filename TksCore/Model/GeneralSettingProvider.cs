using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace Tks.Model
{
    public sealed class GeneralSettingProvider
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

        public List<GeneralSetting> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM GeneralSettings (nolock)";
                command.CommandType = CommandType.Text;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable settingDataTable = new DataTable("GeneralSettings");
                adapter.Fill(settingDataTable);

                // Create a list.
                List<GeneralSetting> settings = null;
                if (settingDataTable.Rows.Count > 0)
                    settings = new List<GeneralSetting>();

                // Iterate each row.
                foreach (DataRow row in settingDataTable.Rows)
                {
                    // Create an instance of Platform.
                    GeneralSetting setting = new GeneralSetting();
                    setting.Id = Int32.Parse(row["SettingId"].ToString());
                    setting.Category = row["Category"].ToString();
                    setting.Name = row["Name"].ToString();
                    setting.Value = row["Value"].ToString();
                    setting.Description = row["Description"].ToString();
                    setting.IsActive = bool.Parse(row["IsActive"].ToString());
                    setting.CreateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    setting.CreateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    setting.LastUpateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    setting.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    
                    // Add to list.
                    settings.Add(setting);
                }

                // Return the list.
                return settings;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<GeneralSetting> RetrieveByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public GeneralSetting Retrieve(string category, string name)
        {
            throw new NotImplementedException();
        }

        public GeneralSetting Retrieve(int id)
        {
            throw new NotImplementedException();
        }
    }
}
