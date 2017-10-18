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
    internal partial class UserService 
        : IUserService
    {
        #region Class Variables

        IAppManager _appManager;
        SqlConnection mDbConnection;
        bool _isAuthenticated;
        int _UserId;

        #endregion


        public void Authenticate(string loginName, string password)
        {
            SqlCommand command = null;

            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "AuthenticateUsernew";
                // Assign the parameters.
                command.Parameters.Add("@LoginName", SqlDbType.NVarChar, 40).Value = loginName;
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 200).Value = password;
                command.Parameters.Add("@IsAuthenticate", SqlDbType.Bit).Direction = ParameterDirection.Output;
                command.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                command.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;

                //  Execute command and get values from output parameters.
                command.ExecuteNonQuery();

                // Return the whethere is authenticate.
                this._isAuthenticated = Convert.ToBoolean(command.Parameters["@isAuthenticate"].Value);

                if (this._isAuthenticated == false)
                {
                    // Get error message.
                    string authenicatedMessage = command.Parameters["@errorMessage"].Value.ToString();
                    // Create an exception and throw it.
                    AuthenticationException authenticationException;
                    authenticationException = new AuthenticationException(authenicatedMessage);
                    throw authenticationException;
                }

                // Return the userid.
                this._UserId = Convert.ToInt32(command.Parameters["@UserId"].Value);
            }


            catch { throw; }

            finally
            {
                if (command != null) command.Dispose();
                command = null;

            }
        }

        public User RetrieveByLoginName(string loginName)
        {

            SqlCommand command = null;

            SqlDataAdapter adapter = null;

            DataTable dtUsers;
            User user = null;
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                //command.CommandText = "RetrieveUsersByLogin_temp";
                command.CommandText = "RetrieveUsersByLogin_temp_v1";

                // Assign the parameters.
                command.Parameters.Add("@LoginName", SqlDbType.NVarChar).Value = loginName;

                adapter = new SqlDataAdapter(command);

                // Create an instance for datatable.
                dtUsers = new DataTable();
                adapter.Fill(dtUsers);

                // Add to user.
                if (dtUsers.Rows.Count > 0)
                {
                    // Create an instance for datarow.
                    DataRow row = null;
                    row = dtUsers.Rows[0];

                    // Create an instance for user.
                    user = new User(Int32.Parse(row["UserId"].ToString()));
                    user.LastName = row["LastName"].ToString();
                    user.FirstName = row["FirstName"].ToString();
                    user.LoginName = row["LoginName"].ToString();
                    user.Password = row["Password"].ToString();
                    user.Gender = row["Gender"].ToString();
                    user.EmailId = row["EmailId"].ToString();
                    user.EmployeeId = row["EmployeeId"].ToString();
                    user.TypeId = Int32.Parse(row["TypeId"].ToString());
                    user.HomePhone = row["HomePhone"].ToString();
                    user.OfficePhone = row["OfficePhone"].ToString();
                    user.OfficePhoneExt = row["OfficePhoneExt"].ToString();
                    user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());
                    user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
                    user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                    user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                    user.IsActive = bool.Parse(row["IsActive"].ToString());
                    user.LocationId = Int32.Parse(row["Location"].ToString());
                    user.IsPasswordChanged = bool.Parse(row["IsPasswordChanged"].ToString()); 
                }

                // Return the user.
                return user;
            }
            catch { throw; }

            finally
            {
                if (command != null) command.Dispose();
                if (adapter != null) adapter.Dispose();
                command = null;

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

                    // Cleare the sqlconnection pool.
                    SqlConnection.ClearPool(mDbConnection);
                    SqlConnection.ClearAllPools();
                }

            }
            catch { throw; }
        }


        public List<User> Search(SearchCriteria criteria)
        {

            SqlCommand command = null;

            SqlDataAdapter adapter = null;

            List<User> Iusers = null;
          
            User user = null;
            try
            {
                if (criteria.ViewName == "HieraricalSearch")
                {
                    
                    //Define the Command.
                    command = mDbConnection.CreateCommand();

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "SearchHieraricalUser";

                    command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml(criteria.ViewName);

                    adapter = new SqlDataAdapter(command);

                    //Create a datatable instance.
                    DataTable HieraricalDataTable = new DataTable();

                    adapter.Fill(HieraricalDataTable);
                    if (HieraricalDataTable.Rows.Count > 0)
                    {
                        Iusers = new List<User>(HieraricalDataTable.Rows.Count);
                        foreach (DataRow row in HieraricalDataTable.Rows)
                        {
                            
                            //create a  instances for user.
                            user = new User(Convert.ToInt32(row["UserId"].ToString()));
                            user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            user.LoginName = row["LoginName"].ToString();
                            user.Gender = row["Gender"].ToString();
                            user.EmailId = row["EmailId"].ToString();
                            user.EmployeeId = row["EmployeeId"].ToString();
                            user.TypeId = Int32.Parse(row["TypeId"].ToString());
                            user.HomePhone = row["HomePhone"].ToString();
                            user.OfficePhone = row["OfficePhone"].ToString();
                   //         user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());
                            user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
                            user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                            user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                            user.IsActive = bool.Parse(row["IsActive"].ToString());
                            user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());
                            user.CustomData.Add("RoleName", row["RoleName"].ToString());
                            user.CustomData.Add("LastUpdateDate", row["LastUpdateDate"].ToString());
                            user.CustomData.Add("Location", row["Location"].ToString());
                           

                            //Add user to the List
                            Iusers.Add(user);

                        }

                    }

                }
                else if (criteria.ViewName == "ActivityResetUserSearch")
                {

                    //Define the Command.
                    command = mDbConnection.CreateCommand();

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "SearchUsersForActivityReset";

                    command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml(criteria.ViewName);

                    adapter = new SqlDataAdapter(command);

                    //Create a datatable instance.
                    DataTable ActivityReset = new DataTable();

                    adapter.Fill(ActivityReset);
                    if (ActivityReset.Rows.Count > 0)
                    {
                        Iusers = new List<User>(ActivityReset.Rows.Count);
                        foreach (DataRow row in ActivityReset.Rows)
                        {

                            //create a  instances for user.
                            user = new User(Convert.ToInt32(row["UserId"].ToString()));
                            //user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            //user.LoginName = row["LoginName"].ToString();
                            user.Gender = row["Gender"].ToString();
                            user.EmailId = row["EmailId"].ToString();
                            user.EmployeeId = row["EmployeeId"].ToString();
                            user.TypeId = Int32.Parse(row["TypeId"].ToString());
                            user.HomePhone = row["HomePhone"].ToString();
                            user.OfficePhone = row["OfficePhone"].ToString();
                            user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
                        //    user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());

                            user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                            user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                            user.IsActive = bool.Parse(row["IsActive"].ToString());
                            user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());
                            //user.CustomData.Add("RoleName", row["RoleName"].ToString());
                            user.CustomData.Add("LastUpdateDate", row["LastUpdateDate"].ToString());
                            user.CustomData.Add("StatusId", row["StatusId"].ToString());

                            //Add user to the List
                            Iusers.Add(user);

                        }

                    }

                }
                else if (criteria.ViewName == "SearchManager")
                {

                    //Define the Command.
                    command = mDbConnection.CreateCommand();

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "SearchManager";

                    command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml(criteria.ViewName);

                    adapter = new SqlDataAdapter(command);

                    //Create a datatable instance.
                    DataTable HieraricalDataTable = new DataTable();

                    adapter.Fill(HieraricalDataTable);
                    if (HieraricalDataTable.Rows.Count > 0)
                    {
                        Iusers = new List<User>(HieraricalDataTable.Rows.Count);
                        foreach (DataRow row in HieraricalDataTable.Rows)
                        {

                            //create a  instances for user.
                            user = new User(Convert.ToInt32(row["UserId"].ToString()));
                            user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            user.LoginName = row["LoginName"].ToString();
                            user.Gender = row["Gender"].ToString();
                            user.EmailId = row["EmailId"].ToString();
                            user.EmployeeId = row["EmployeeId"].ToString();
                            user.TypeId = Int32.Parse(row["TypeId"].ToString());
                            user.HomePhone = row["HomePhone"].ToString();
                            user.OfficePhone = row["OfficePhone"].ToString();
                            user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
     //                       user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());

                            user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                            user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                            user.IsActive = bool.Parse(row["IsActive"].ToString());
                            user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());
                            user.CustomData.Add("RoleName", row["RoleName"].ToString());
                            user.CustomData.Add("LastUpdateDate", row["LastUpdateDate"].ToString());
                            user.CustomData.Add("Location", row["Location"].ToString());

                            //Add user to the List
                            Iusers.Add(user);

                        }

                    }

                }
                else if (criteria.ViewName == "SearchManagerforhierarchy")
                {

                    //Define the Command.
                    command = mDbConnection.CreateCommand();

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "SearchManagerforhierarchy";

                    command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml(criteria.ViewName);

                    adapter = new SqlDataAdapter(command);

                    //Create a datatable instance.
                    DataTable HieraricalDataTable = new DataTable();

                    adapter.Fill(HieraricalDataTable);
                    if (HieraricalDataTable.Rows.Count > 0)
                    {
                        Iusers = new List<User>(HieraricalDataTable.Rows.Count);
                        foreach (DataRow row in HieraricalDataTable.Rows)
                        {

                            //create a  instances for user.
                            user = new User(Convert.ToInt32(row["UserId"].ToString()));
                            user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            user.LoginName = row["LoginName"].ToString();
                            user.Gender = row["Gender"].ToString();
                            user.EmailId = row["EmailId"].ToString();
                            user.EmployeeId = row["EmployeeId"].ToString();
                            user.TypeId = Int32.Parse(row["TypeId"].ToString());
                            user.HomePhone = row["HomePhone"].ToString();
                            user.OfficePhone = row["OfficePhone"].ToString();
                            user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
                            user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                            user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                            user.IsActive = bool.Parse(row["IsActive"].ToString());
//                            user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());

                            user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());
                            user.CustomData.Add("RoleName", row["RoleName"].ToString());
                            user.CustomData.Add("LastUpdateDate", row["LastUpdateDate"].ToString());
                            user.CustomData.Add("Location", row["Location"].ToString());

                            //Add user to the List
                            Iusers.Add(user);

                        }

                    }

                }
                else if (criteria.ViewName == "SearchUsersforuserSetting")
                {

                    //Define the Command.
                    command = mDbConnection.CreateCommand();

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "SearchUsersforuserSetting";

                    command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml(criteria.ViewName);

                    adapter = new SqlDataAdapter(command);

                    //Create a datatable instance.
                    DataTable HieraricalDataTable = new DataTable();

                    adapter.Fill(HieraricalDataTable);
                    if (HieraricalDataTable.Rows.Count > 0)
                    {
                        Iusers = new List<User>(HieraricalDataTable.Rows.Count);
                        foreach (DataRow row in HieraricalDataTable.Rows)
                        {

                            //create a  instances for user.
                            user = new User(Convert.ToInt32(row["UserId"].ToString()));
                            user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            user.LoginName = row["LoginName"].ToString();
                            user.Gender = row["Gender"].ToString();
                            user.EmailId = row["EmailId"].ToString();
                            user.EmployeeId = row["EmployeeId"].ToString();
                            user.TypeId = Int32.Parse(row["TypeId"].ToString());
                            user.HomePhone = row["HomePhone"].ToString();
                            user.OfficePhone = row["OfficePhone"].ToString();
//                            user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());

                            user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
                            user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                            user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                            user.IsActive = bool.Parse(row["IsActive"].ToString());
                            user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());
                            user.CustomData.Add("RoleName", row["RoleName"].ToString());
                            user.CustomData.Add("LastUpdateDate", row["LastUpdateDate"].ToString());
                            user.CustomData.Add("Location", row["Location"].ToString());
                            user.CustomData.Add("Language", row["Language"].ToString());
                            user.CustomData.Add("Shift", row["Shift"].ToString());
                          
                            if (string.IsNullOrEmpty(row["FromDate"].ToString()))
                            {
                                user.CustomData.Add("FromDate", "");
                            }
                            else
                            {
                                user.CustomData.Add("FromDate", DateTime.Parse(row["FromDate"].ToString()));
                            }

                            if (string.IsNullOrEmpty(row["ToDate"].ToString()))
                            {
                                user.CustomData.Add("ToDate", "");
                            }
                            else
                            {
                                user.CustomData.Add("ToDate", DateTime.Parse(row["ToDate"].ToString()));
                            }
                            //Add user to the List
                            Iusers.Add(user);

                        }

                    }

                }

                else
                {

                   
                    // Define the command.
                    command = mDbConnection.CreateCommand();

                    command.CommandText = "SearchUsers";

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                    adapter = new SqlDataAdapter(command);

                    //Define the DataTable.
                    DataTable UserDataTable = new DataTable();

                    adapter.Fill(UserDataTable);
                    if (UserDataTable.Rows.Count > 0)
                    {
                        Iusers = new List<User>(UserDataTable.Rows.Count);
                        foreach (DataRow row in UserDataTable.Rows)
                        {

                            //create a  instances for user.
                            user = new User(Convert.ToInt32(row["UserId"].ToString()));
                            user.FirstName = row["FirstName"].ToString();
                            user.LastName = row["LastName"].ToString();
                            user.FirstName = row["FirstName"].ToString();
                            user.LoginName = row["LoginName"].ToString();
                            user.Gender = row["Gender"].ToString();
                            user.EmailId = row["EmailId"].ToString();
                            user.EmployeeId = row["EmployeeId"].ToString();
                            user.TypeId = Int32.Parse(row["TypeId"].ToString());
                            user.HomePhone = row["HomePhone"].ToString();
                            user.OfficePhone = row["OfficePhone"].ToString();
                            user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());
                            user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());
                            user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());

                            user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());
                            user.IsActive = bool.Parse(row["IsActive"].ToString());
                            user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());
                            user.CustomData.Add("RoleName", row["RoleName"].ToString());
                            user.CustomData.Add("LastUpdateDate", row["LastUpdateDate"].ToString());
                            user.CustomData.Add("Location", row["Location"].ToString());
                            if (string.IsNullOrEmpty(row["JoinDate"].ToString()))
                            {
                                user.JoinDate = null;
                            }
                            else
                            {
                                user.JoinDate = DateTime.Parse(row["JoinDate"].ToString());
                            }
                            //Add user to the List
                            Iusers.Add(user);

                        }

                    }
                }

                return Iusers;
            }
               
                
            catch { throw; }
            finally
            {

                //Dispose
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }
                

            }


        }


        public void  Update(User entity)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;
            
            try
            {
                command = mDbConnection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;

//                command.CommandText = "UpdateUsers";

                command.CommandText = "UpdateUsers_v3";

                command.Parameters.Add("@Data", SqlDbType.Xml).Value = entity.GetXml();
                
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                
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
                            message.Append(string.Format("{0}", row["Value"].ToString()));
                        }
                        exception.Data.Add("IsExists", message);
                    }

                    //Throw the exception.
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

                        //Rollback the transaction
                        transaction.Rollback();

                throw;
            }
            finally
            {
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
                if (adapter != null) adapter.Dispose();
            }
           
        }
        public User Retrieve(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<User> UserList = null;
            try
            {
                //create a command instance.
                command = mDbConnection.CreateCommand();

                command.CommandText = "RetrieveUsers_v4";

                command.CommandType = CommandType.StoredProcedure;

                StringBuilder xml = new StringBuilder("<Users>");
                xml.Append(string.Format("<User><Id>{0}</Id></User>", userId));
                xml.Append("</Users>");

                //pass the value.
                command.Parameters.Add("@data", SqlDbType.Xml).Value = xml.ToString();

                adapter = new SqlDataAdapter(command);

                //create a Datatable instance.
                DataTable UserDataTable = new DataTable("UserTable");

                //Fill the UserDatatable.
                adapter.Fill(UserDataTable);

                if (UserDataTable.Rows.Count > 0)
                {
                    UserList = new List<User>();
                    foreach (DataRow row in UserDataTable.Rows)
                    {

                        //create a  instances for user.
                        User user = new User(Convert.ToInt32(row["UserId"].ToString()));

                        user.FirstName = row["FirstName"].ToString();

                        user.LastName = row["LastName"].ToString();

                        user.Initial = row["Initial"].ToString();

                        user.LoginName = row["LoginName"].ToString();

                        user.Gender = row["Gender"].ToString();

                        user.EmailId = row["EmailId"].ToString();

                        user.EmployeeId = row["EmployeeId"].ToString();

                        user.RoleId = Int32.Parse(row["RoleId"].ToString());

                        user.LocationId = string.IsNullOrEmpty(row["Location"].ToString()) ? 0: Int32.Parse(row["Location"].ToString());

                        user.DepartmentId = string.IsNullOrEmpty(row["DepartmentId"].ToString()) ? 0 : Int32.Parse(row["DepartmentId"].ToString());

                        user.TypeId = Int32.Parse(row["TypeId"].ToString());

                        user.AccessLevelId = string.IsNullOrEmpty(Convert.ToString(row["AccessLevelId"])) ? 0 : Int32.Parse(Convert.ToString(row["AccessLevelId"]));  

                        user.HomePhone = row["HomePhone"].ToString();

                        user.OfficePhone = row["OfficePhone"].ToString();

                        user.OfficePhoneExt = row["OfficePhoneExtension"].ToString();

                        user.TypeId = Int32.Parse(row["TypeId"].ToString());

                        user.IsLandDAdmin = bool.Parse(row["IsLandDAdmin"].ToString());

                        user.EmployeeId = row["EmployeeId"].ToString();

                        user.HasAdminRights = bool.Parse(row["HasAdminRights"].ToString());

                        user.IsPasswordChanged = bool.Parse(row["IsPasswordChanged"].ToString());
                                                 //bool.Parse(row["IsPasswordChanged"].ToString());

                        user.IsactivityApprovalPending = Int32.Parse(row["IsactivityApprovalPending"].ToString());

                        user.HasAutoApproval = bool.Parse(row["HasAutoApproval"].ToString());

                        user.IsAnyLocation = bool.Parse(row["IsAnyLocation"].ToString());

                        user.IsSysAdmin = bool.Parse(row["IsSysAdmin"].ToString());

                        user.IsActive = bool.Parse(row["IsActive"].ToString());

                        user.LastUpdateDate =DateTime.Parse( row["LastUpdateDate"].ToString());

                        user.CustomData.Add("LastUpdateUser", row["LastUpdateUser"].ToString());

                        user.CustomData.Add("RoleName", row["RoleName"].ToString());

                        user.CustomData.Add("Location", row["LocationName"].ToString());

                        user.CustomData.Add("DepartmentName", row["DepartmentName"].ToString());

                        if (string.IsNullOrEmpty(row["JoinDate"].ToString()))
                        {
                            user.JoinDate = null;
                        }
                        else
                        {
                            user.JoinDate = DateTime.Parse(row["JoinDate"].ToString());
                        }
                        if (string.IsNullOrEmpty(row["RelieveDate"].ToString()))
                        {
                            user.RelieveDate = null;
                        }
                        else
                        {
                            user.RelieveDate = DateTime.Parse(row["RelieveDate"].ToString());
                        }

                        if (string.IsNullOrEmpty(row["ContractEndDate"].ToString()))
                        {
                            user.ContractEndDate = null;
                        }
                        else
                        {
                            user.ContractEndDate = DateTime.Parse(row["ContractEndDate"].ToString());
                        }

                        if (string.IsNullOrEmpty(row["Managername"].ToString()))
                        {
                            user.ManagerId = null;
                            user.CustomData.Add("Managername", "");
                        }
                        else
                        {
                            user.ManagerId = Convert.ToInt32(row["ManagerId"].ToString());
                            user.CustomData.Add("Managername", row["Managername"].ToString());
                        }

                        //if (string.IsNullOrEmpty(row["Language"].ToString()))
                        //{
                        //    user.LanguageId = null;
                        //    user.CustomData.Add("Language", "");
                        //}
                        //else
                        //{
                        //    user.LanguageId = Int32.Parse(row["LanguageId"].ToString());
                        //    user.CustomData.Add("Language", row["Language"].ToString());
                        //}
                        //if (string.IsNullOrEmpty(row["Shift"].ToString()))
                        //{
                        //    user.ShiftId = null;
                        //    user.CustomData.Add("Shift", "");
                        //}
                        //else
                        //{
                        //    user.ShiftId = Int32.Parse(row["ShiftId"].ToString());
                        //    user.CustomData.Add("Shift", row["Shift"].ToString());
                        //}
                                              
                       //Add user to the List
                        UserList.Add(user);
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

            return UserList.FirstOrDefault();
        }
        public List<User> LandDalert(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "Pr_ViewDocs";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtActivity = new DataTable();
                adapter.Fill(DtActivity);

                // Create a list.
                List<User> BirthDaylst = null;
                if (DtActivity.Rows.Count > 0)
                    BirthDaylst = new List<User>();

                // Iterate each row.
                foreach (DataRow row in DtActivity.Rows)
                {
                    User birthdaylist = new User();
                    birthdaylist.CustomData.Add("Row", row["Row"].ToString());
                    birthdaylist.FirstName = row["Name"].ToString();
                    BirthDaylst.Add(birthdaylist);
                }

                // Return the list.
                return BirthDaylst;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<User> TTalert(int userId, int Sno)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "Pr_TTAlert";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@Sno", SqlDbType.Int).Value = Sno;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtActivity = new DataTable();
                adapter.Fill(DtActivity);

                // Create a list.
                List<User> BirthDaylst = null;
                if (DtActivity.Rows.Count > 0)
                    BirthDaylst = new List<User>();

                // Iterate each row.
                foreach (DataRow row in DtActivity.Rows)
                {
                    User birthdaylist = new User();
                    birthdaylist.CustomData.Add("Row", row["Row"].ToString());
                    birthdaylist.FirstName = row["Name"].ToString();
                    BirthDaylst.Add(birthdaylist);
                }

                // Return the list.
                return BirthDaylst;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<User> LandDadmin(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "Pr_LandDadmin";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtActivity = new DataTable();
                adapter.Fill(DtActivity);

                // Create a list.
                List<User> BirthDaylst = null;
                if (DtActivity.Rows.Count > 0)
                    BirthDaylst = new List<User>();

                // Iterate each row.
                foreach (DataRow row in DtActivity.Rows)
                {
                    User birthdaylist = new User();
                    birthdaylist.CustomData.Add("Row", row["Row"].ToString());
                    birthdaylist.FirstName = row["Name"].ToString();
                    BirthDaylst.Add(birthdaylist);
                }

                // Return the list.
                return BirthDaylst;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public User RetrieveUsersettings(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<User> UserList = null;
            try
            {
                //create a command instance.
                command = mDbConnection.CreateCommand();

                command.CommandText = "RetrieveUserSettings";

                command.CommandType = CommandType.StoredProcedure;

                StringBuilder xml = new StringBuilder("<Users>");
                xml.Append(string.Format("<User><Id>{0}</Id></User>", userId));
                xml.Append("</Users>");

                //pass the value.
                command.Parameters.Add("@data", SqlDbType.Xml).Value = xml.ToString();

                adapter = new SqlDataAdapter(command);

                //create a Datatable instance.
                DataTable UserDataTable = new DataTable("UserTable");

                //Fill the UserDatatable.
                adapter.Fill(UserDataTable);

                if (UserDataTable.Rows.Count > 0)
                {
                    UserList = new List<User>();
                    foreach (DataRow row in UserDataTable.Rows)
                    {

                        //create a  instances for user.
                        User user = new User(Convert.ToInt32(row["UserId"].ToString()));

                        user.FirstName = row["FirstName"].ToString();

                        user.LastName = row["LastName"].ToString();

                        user.LocationId = string.IsNullOrEmpty(row["LocationId"].ToString()) ? 0 : Int32.Parse(row["LocationId"].ToString());

                        //user.LocationId =Convert.ToInt32(row["Location"].ToString());

                        user.LanguageId = string.IsNullOrEmpty(row["LanguageId"].ToString()) ? 0 : Int32.Parse(row["LanguageId"].ToString());

                        user.ShiftId = string.IsNullOrEmpty(row["ShiftId"].ToString()) ? 0 : Int32.Parse(row["ShiftId"].ToString()); 

                        user.CustomData.Add("Location", row["Location"].ToString()); 

                        user.CustomData.Add("Language", row["Language"].ToString());

                        user.CustomData.Add("Shift", row["Shift"].ToString());

                        if (string.IsNullOrEmpty(row["FromDate"].ToString()))
                        {
                            user.FromDate  = null;
                        }
                        else
                        {
                            user.FromDate = DateTime.Parse(row["FromDate"].ToString());
                        }
                        if (string.IsNullOrEmpty(row["ToDate"].ToString()))
                        {
                            user.ToDate  = null;
                        }
                        else
                        {
                            user.ToDate = DateTime.Parse(row["ToDate"].ToString());
                        }
                        //Add user to the List
                        UserList.Add(user);
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

            return UserList.FirstOrDefault();
        }
        public void  UpdateUserSettings(User entity)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;
            
            try
            {
                command = mDbConnection.CreateCommand();

                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "UpdateUsersettings";
                
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = entity.GetXml();

                command.ExecuteNonQuery();
                
                
                //// execute
                //transaction = mDbConnection.BeginTransaction();
                //command.Transaction = transaction;

                //// fill error datatable
                //adapter = new SqlDataAdapter(command);
                //DataTable errorDataTable = new DataTable();
                //adapter.Fill(errorDataTable);

                //// commit
                //transaction.Commit();
            }
            catch (ValidationException ve)
            {
                throw ve;
            }
            catch
            {
                if (transaction != null)
                    if (transaction.Connection != null)

                        //Rollback the transaction
                        transaction.Rollback();

                throw;
            }
            finally
            {
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
                if (adapter != null) adapter.Dispose();
            }

        }
        public List<UserType> RetrieveUserType()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                //create a command instance 
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                //procedure name.
                command.CommandText = "RetrieveUserType";

                //create a adapter instance.
                adapter = new SqlDataAdapter(command);

                //create a Datatable instance.
                DataTable dtUserType = new DataTable();
                adapter.Fill(dtUserType);

                List<UserType> LstUserType = null;

                if (dtUserType.Rows.Count > 0)

                    //create a List usertype instance.
                    LstUserType = new List<UserType>();


                //iterate the row in datatable
                foreach (DataRow drUserType in dtUserType.Rows)
                {
                    UserType utype = new UserType(Int32.Parse(drUserType["TypeId"].ToString()));
                    utype.Name = drUserType["Name"].ToString();
                    LstUserType.Add(utype);
                }
                return LstUserType;


            }
            catch
            {
                throw;
            }
            finally
            {
                if (command != null) { command.Dispose(); }
                if (adapter != null) { adapter.Dispose(); }
            }
        }

        public List<UserShift> RetrieveUserWorkTypes()
        {

            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                //create a command instance.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.Text;

                //procedure name.
                command.CommandText = "RetrieveUserWorktypes";

                //create a adapter instance.
                adapter = new SqlDataAdapter(command);

                //create  a datatable instance
                DataTable dtUsershift = new DataTable();
                adapter.Fill(dtUsershift);

                List<UserShift> LstUsershift = null;

                //create a List UserShift instance.
                if (dtUsershift.Rows.Count > 0)
                    LstUsershift = new List<UserShift>();

                //iterate the row in datatable.
                foreach (DataRow drusershift in dtUsershift.Rows)
                {
                    UserShift shift = new UserShift(Int32.Parse(drusershift["Id"].ToString()));
                    shift.Name = drusershift["Name"].ToString();

                    //Add the shift to the  List.
                    LstUsershift.Add(shift);
                }

                return LstUsershift;

            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose the object.
                if (command != null) { command.Dispose(); }
                if (adapter != null) { adapter.Dispose(); }
            }


        }

        public List<UserShift> RetrieveUserShift()
        {

            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                //create a command instance.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.Text;

                //procedure name.
                command.CommandText = "RetrieveUserShift";

                //create a adapter instance.
                adapter = new SqlDataAdapter(command);

                //create  a datatable instance
                DataTable dtUsershift = new DataTable();
                adapter.Fill(dtUsershift);

                List<UserShift> LstUsershift = null;

                //create a List UserShift instance.
                if (dtUsershift.Rows.Count > 0)
                    LstUsershift = new List<UserShift>();

                //iterate the row in datatable.
                foreach (DataRow drusershift in dtUsershift.Rows)
                {
                    UserShift shift = new UserShift(Int32.Parse(drusershift["shiftid"].ToString()));
                    shift.Name = drusershift["Name"].ToString();

                    //Add the shift to the  List.
                    LstUsershift.Add(shift);
                }

                return LstUsershift;

            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose the object.
                if (command != null) { command.Dispose(); }
                if (adapter != null) { adapter.Dispose(); }
            }


        }

        public void InsertSession(string Sessionid, int Userid)
        {
            SqlCommand command = null;
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "InsertSession";
                // Assign the parameters.
                command.Parameters.Add("@se_id", SqlDbType.VarChar, 50).Value = Sessionid;
                command.Parameters.Add("@se_userid", SqlDbType.Int, 40).Value = Userid;
                //  Execute command and get values from output parameters.
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose the object.
                if (command != null) { command.Dispose(); }
            }
        }

        public bool CheckUserSession(string Sessionid, int Userid)
        {
            SqlCommand command = null;
            bool IsLoggedin = false;
          
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CheckUserSession";
                // Assign the parameters.
                command.Parameters.Add("@se_id", SqlDbType.VarChar, 50).Value = Sessionid;
                command.Parameters.Add("@se_userid", SqlDbType.Int, 40).Value = Userid;
                command.Parameters.Add("@IsLoggedin", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //  Execute command and get values from output parameters.
                command.ExecuteNonQuery();

               IsLoggedin = Convert.ToBoolean(command.Parameters["@IsLoggedin"].Value);

               return IsLoggedin;
            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose the object.
                if (command != null) { command.Dispose(); }
            }
        }

        public bool CheckUserSession_Secure(string Sessionid, int Userid,string SystemName)
        {
            SqlCommand command = null;
            bool IsLoggedin = false;

            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CheckUserSession_Secure";
                // Assign the parameters.
                command.Parameters.Add("@se_id", SqlDbType.VarChar, 50).Value = Sessionid;
                command.Parameters.Add("@se_userid", SqlDbType.Int, 40).Value = Userid;
                command.Parameters.Add("@Systemname", SqlDbType.VarChar, 50).Value = SystemName;
                command.Parameters.Add("@IsLoggedin", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //  Execute command and get values from output parameters.
                command.ExecuteNonQuery();

                IsLoggedin = Convert.ToBoolean(command.Parameters["@IsLoggedin"].Value);

                return IsLoggedin;
            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose the object.
                if (command != null) { command.Dispose(); }
            }
        }

        public string InsertUserlog(string Sessionid, int Userid, string systemname, string loginname, string Browserinfo,bool Logout,bool IsValidIP)
        {
            SqlCommand command = null;
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "InsertUserlog";
                // Assign the parameters.
                command.Parameters.Add("@Userid", SqlDbType.Int, 40).Value = Userid;
                command.Parameters.Add("@UserLogin", SqlDbType.VarChar, 50).Value = loginname;
                command.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = Sessionid;
                command.Parameters.Add("@Sytemname", SqlDbType.VarChar, 50).Value = systemname;
                command.Parameters.Add("@Browserinfo", SqlDbType.VarChar, 50).Value = Browserinfo;
                command.Parameters.Add("@logout", SqlDbType.Bit, 50).Value = Logout;
                command.Parameters.Add("@IsValidIP", SqlDbType.Bit, 50).Value = IsValidIP;
                command.Parameters.Add("@Result", SqlDbType.VarChar,50).Direction = ParameterDirection.Output;
                //  Execute command and get values from output parameters.
                command.ExecuteNonQuery();

              return  command.Parameters["@Result"].Value.ToString();
            }
            catch
            {
                throw;
            }
            finally
            {
                //Dispose the object.
                if (command != null) { command.Dispose(); }
            }
        }
    }
}
