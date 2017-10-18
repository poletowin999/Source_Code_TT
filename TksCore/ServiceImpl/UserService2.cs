using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


namespace Tks.ServiceImpl
{
    internal partial class UserService
        : IUserService
    {


        public List<User> RetrieveChildren(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveUnAttahcedUserDetails";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtUnAttachedUsers = new DataTable("UnAttachedUsers");
                adapter.Fill(DtUnAttachedUsers);

                // Create a list.
                List<User> LstUnAttachedUsers = null;
                if (DtUnAttachedUsers.Rows.Count > 0)
                    LstUnAttachedUsers = new List<User>();

                // Iterate each row.
                foreach (DataRow row in DtUnAttachedUsers.Rows)
                {
                    // Create an instance of Role.
                    User usr = new User(Int32.Parse(row["UserId"].ToString()));
                    usr.LastName = row["LastName"].ToString();
                   
                    // Add to list.
                    LstUnAttachedUsers.Add(usr);
                }

                // Return the list.
                return LstUnAttachedUsers;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<User> RetrieveAttachedUsers(int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAttahcedUserDetails";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ParentUserId", SqlDbType.Int).Value = userId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtAttachedUsers = new DataTable("AttachedUsers");
                adapter.Fill(DtAttachedUsers);

                // Create a list.
                List<User> LstAttachedUsers = null;
                if (DtAttachedUsers.Rows.Count > 0)
                    LstAttachedUsers = new List<User>();

                // Iterate each row.
                foreach (DataRow row in DtAttachedUsers.Rows)
                {
                    // Create an instance of Role.
                    User usr = new User(Int32.Parse(row["UserId"].ToString()));
                    usr.LastName = row["LastName"].ToString();
                    usr.FirstName = row["FirstName"].ToString();
                    //usr.CustomData.Add("Designation", row["Designation"].ToString());
                    usr.CustomData.Add("Role", row["Role"].ToString());
                    usr.CustomData.Add("Reporting user", row["Reporting user"].ToString());

                    // Add to list.
                    LstAttachedUsers.Add(usr);
                }

                // Return the list.
                return LstAttachedUsers;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<User> RetrieveUnAttachedUsers(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveUnAttahcedUserDetails";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();
               
                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtUnAttachedUsers = new DataTable("UnAttachedUsers");
                adapter.Fill(DtUnAttachedUsers);

                // Create a list.
                List<User> LstUnAttachedUsers = null;
                if (DtUnAttachedUsers.Rows.Count > 0)
                    LstUnAttachedUsers = new List<User>();

                // Iterate each row.
                foreach (DataRow row in DtUnAttachedUsers.Rows)
                {
                  
                    User usr = new User(Int32.Parse(row["UserId"].ToString()));
                    
                    usr.LastName = row["LastName"].ToString();
                    usr.FirstName = row["FirstName"].ToString();
                    //usr.CustomData.Add("Designation", row["Designation"].ToString());
                    usr.CustomData.Add("Role", row["Role"].ToString());
                    usr.CustomData.Add("EmailId", row["EmailId"].ToString());

                    // Add to list.
                    LstUnAttachedUsers.Add(usr);
                }

                // Return the list.
                return LstUnAttachedUsers;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void AttachToHierarchy(List<User> users, DateTime attachDate,int AttahcUserId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Clients>");
                foreach (User  element in users)
                {
                    xml.Append(string.Format("<Client><Id>{0}</Id></Client>", element.Id));
                }
                xml.Append("</Clients>");

                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "InsertNewHierarchies";
                // Assign the parameters.
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = xml.ToString();
                command.Parameters.Add("@AttachDate", SqlDbType.DateTime).Value = attachDate;
                command.Parameters.Add("@ParentUserId", SqlDbType.Int).Value = AttahcUserId;
                command.Parameters.Add("@AttachUserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtUnAttachedUsers = new DataTable("UnAttachedUsers");
                adapter.Fill(DtUnAttachedUsers);
                
            }


            catch { throw; }

            finally
            {
                if (command != null) command.Dispose();
                command = null;

            }
        }

        public void DetachFromHierarchy(List<User> users, DateTime detachDate)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Clients>");
                foreach (User element in users)
                {
                    xml.Append(string.Format("<Client><Id>{0}</Id></Client>", element.Id));
                }
                xml.Append("</Clients>");

                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "updatehierarchies";
                // Assign the parameters.
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = xml.ToString();
                command.Parameters.Add("@Detachdate", SqlDbType.DateTime).Value = detachDate;
                command.Parameters.Add("@DetachUserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtUnAttachedUsers = new DataTable("AttachedUsers");
                adapter.Fill(DtUnAttachedUsers);

            }


            catch { throw; }

            finally
            {
                if (command != null) command.Dispose();
                command = null;

            }
        }


        public User RetrieveSupervisor(int userId)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveSupervisor";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                // Execute
                int? supervisorId = null;
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader["UserId"] != DBNull.Value)
                        supervisorId = Int32.Parse(reader["UserId"].ToString());
                }
                reader.Close();

                if (supervisorId.HasValue)
                    return this.Retrieve(supervisorId.Value);
                else
                    return null;
            }
            catch { throw; }
        }

        public User RetrieveManager(int userId)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveManager";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                // Execute
                int? managerId = null;
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader["UserId"] != DBNull.Value)
                        managerId = Int32.Parse(reader["UserId"].ToString());
                }
                reader.Close();

                if (managerId.HasValue)
                    return this.Retrieve(managerId.Value);
                else
                    return null;
            }
            catch { throw; }
        }


        public void ChangeManager(List<User> users, DateTime detachDate)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Clients>");
                foreach (User element in users)
                {
                    xml.Append(string.Format("<Client><Id>{0}</Id></Client>", element.Id));
                }
                xml.Append("</Clients>");

                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "ChangeManager";
                // Assign the parameters.
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = xml.ToString();
                command.Parameters.Add("@Detachdate", SqlDbType.DateTime).Value = detachDate;
                command.Parameters.Add("@DetachUserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable DtUnAttachedUsers = new DataTable("AttachedUsers");
                adapter.Fill(DtUnAttachedUsers);

            }


            catch { throw; }

            finally
            {
                if (command != null) command.Dispose();
                command = null;

            }
        }

    }
}
