using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Tks.Model;
using Tks.Entities;
using Tks.Services;
using System.Web;


namespace Tks.ServiceImpl
{
    internal sealed class MenuService
        : IMenuService
    {

        #region Class Variables

        IAppManager mAppManager;
        SqlConnection mDbConnection;


        #endregion

        public IAppManager AppManager
        {
            get
            {
                return this.mAppManager;
            }
            set
            {
                mAppManager = value;
                if (value != null)
                    mDbConnection = this.mAppManager.DbConnectionProvider.GetDefaultDbConnectionInstance();

            }
        }


        public Menu Retrieve(int id)
        {
            try
            {
                int[] ids = { id };

                // Retrieve the menus.
                List<Menu> menus = this.Retrieve(ids);

                // Return the menu.
                return (menus.Count > 0) ? menus[0] : null;
            }
            catch { throw; }
        }

        public List<Menu> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;
            StringBuilder id;

            try
            {
                // Create an instance.
                id = new StringBuilder();

                // Getting ids.
                id = GetIds(ids);

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveMenus";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = id.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Menus");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Menu> menus = null;
                if (menuDataTable.Rows.Count > 0)
                    menus = new List<Menu>();

                // Retrieve the list of menus.
                menus = RetrieveMenus(menuDataTable);

                // Return the list.
                return menus;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }

        }


        private StringBuilder GetIds(int[] ids)
        {
            // Build xml.
            StringBuilder xml = new StringBuilder();
            xml.Append("<Menus>");
            foreach (int element in ids)
            {
                xml.Append(string.Format("<Menu><Id>{0}</Id></Menu>", element));
            }
            xml.Append("</Menus>");
            return xml;
        }

        private List<Menu> RetrieveMenus(DataTable menuDataTable)
        {

            List<Menu> listMenus = null;
            // Create an instance.
            listMenus = new List<Menu>();

            // Iterate each row.
            foreach (DataRow row in menuDataTable.Rows)
            {
                // Create an instance of Role.
                Menu menu = new Menu(Int32.Parse(row["MenuId"].ToString()));
                menu.Text = row["Text"].ToString();
                menu.Name = row["Name"].ToString();
                menu.Type = Int32.Parse(row["Type"].ToString());
                menu.IsPublic = bool.Parse(row["IsPublic"].ToString());
                menu.ParentId = Int32.Parse(row["ParentId"].ToString());
                menu.ModuleName = row["ModuleName"].ToString();
                menu.FormName = row["FormName"].ToString();
                menu.IsValid = bool.Parse(row["IsValid"].ToString());
                menu.Priority = Int32.Parse(row["Priority"].ToString());
                menu.ImageName = row["ImageName"].ToString();
                menu.NavigateUrl = row["NavigateUrl"].ToString();
                menu.CreateUser = Int32.Parse(row["AddedBy"].ToString());
                menu.CreateDate = DateTime.Parse(row["AddedOn"].ToString());
                menu.LastUpdateUser = Int32.Parse(row["LastUpdatedBy"].ToString());
                menu.LastUpdateDate = DateTime.Parse(row["LastUpdatedDate"].ToString());

                // Add to list.
                listMenus.Add(menu);

            }

            // Return the list.
            return listMenus;

        }

        public List<Menu> RetrieveByUser(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveMenusByUser";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Menus");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Menu> menus = null;
                if (menuDataTable.Rows.Count > 0)
                    menus = new List<Menu>();

                // Retrieve the list of menus.
                menus = RetrieveMenus(menuDataTable);

                // Return the list.
                return menus;

            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }

        }

        public List<Menu> RetrieveByUser(int userId, int LanguageId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveMenusByUser_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@LanguageId", SqlDbType.Int).Value = HttpContext.Current.Session["SesLanguageId"];

                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Menus");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Menu> menus = null;
                if (menuDataTable.Rows.Count > 0)
                    menus = new List<Menu>();

                // Retrieve the list of menus.
                menus = RetrieveMenus(menuDataTable);

                // Return the list.
                return menus;

            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }

        }

        public List<Menu> RetrieveChildren(int menuId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveMenusByParent";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@MenuId", SqlDbType.Int).Value = menuId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Menus");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Menu> menus = null;
                if (menuDataTable.Rows.Count > 0)
                    menus = new List<Menu>();

                // Retrieve the list of menus.
                menus = RetrieveMenus(menuDataTable);

                // Return the list.
                return menus;

            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
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

                    // Cleare the sqlconnection pool.
                    SqlConnection.ClearPool(mDbConnection);
                    SqlConnection.ClearAllPools();
                }

            }
            catch { throw; }
        }
    }

}
