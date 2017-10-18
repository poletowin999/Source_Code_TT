using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;
using Tks.Entities;
using Tks.Model;
using Tks.Services;
using System.Web;


namespace Tks.ServiceImpl
{
    internal sealed partial class LblLanguageService : ILblLanguage
    {
        #region Class Variables

        IAppManager _appManager;
        SqlConnection mDbConnection;
        bool _isAuthenticated;
        int _UserId;

        #endregion

        public List<LblLanguage> RetrieveLabel(int userId,string Pagename)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<LblLanguage> LblLanguageList = null;
            try
            {
                //create a command instance.
                command = mDbConnection.CreateCommand();

                command.CommandText = "RetrieveLablesToDisplay";

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@PageId", SqlDbType.VarChar, 50).Value = Pagename;
                command.Parameters.Add("@LanguageId", SqlDbType.Int).Value = HttpContext.Current.Session["SesLanguageId"];

                adapter = new SqlDataAdapter(command);

                //create a Datatable instance.
                DataTable UserDataTable = new DataTable("UserTable");

                //Fill the UserDatatable.
                adapter.Fill(UserDataTable);

                if (UserDataTable.Rows.Count > 0)
                {
                    LblLanguageList = new List<LblLanguage>();
                    foreach (DataRow row in UserDataTable.Rows)
                    {

                        //create a  instances for user.
                        LblLanguage lblLanguage = new LblLanguage(Convert.ToInt32(row["UserId"].ToString()));
                        lblLanguage.LabelId = Convert.ToString(row["LabelId"]);
                        lblLanguage.DisplayText = Convert.ToString(row["DisplayText"]);
                        lblLanguage.SupportingText1 = Convert.ToString(row["SupportingText1"]);
                        LblLanguageList.Add(lblLanguage);
                    }

                }
            }
            catch { throw; }
            finally
            {
                //Dispose 
                if (command != null) { command.Dispose(); }
                if (adapter != null) { adapter.Dispose(); }

            }

            return LblLanguageList;
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
