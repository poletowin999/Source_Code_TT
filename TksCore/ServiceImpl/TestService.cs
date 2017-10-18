using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Tks.Model;
using Tks.Entities;
using Tks.Services;

namespace Tks.ServiceImpl
{
    internal sealed class TestService
        : ITestService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        #region Members

        public Test Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Test> tests = this.Retrieve(ids);

                return (tests.Count > 0) ? tests[0] : null;
            }
            catch { throw; }
        }

        public List<Test> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Tests>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Test><Id>{0}</Id></Test>", element));
                }
                xml.Append("</Tests>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveTests";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable testDataTable = new DataTable("Tests");
                adapter.Fill(testDataTable);

                // Create a list.
                List<Test> tests = null;
                if (testDataTable.Rows.Count > 0)
                    tests = new List<Test>();

                // Iterate each row.
                foreach (DataRow row in testDataTable.Rows)
                {
                    // Create an instance of Test.
                    Test test = new Test(Int32.Parse(row["TestId"].ToString()));
                    test.Name = row["Name"].ToString();
                    test.Description = row["Description"].ToString();
                    test.Reason = row["Reason"].ToString();
                    test.IsActive = bool.Parse(row["IsActive"].ToString());
                    test.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    test.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    test.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    tests.Add(test);
                }

                // Return the list.
                return tests;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(Test entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateTests";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DataXml", SqlDbType.Xml).Value = entity.GetXml();
                command.Parameters.Add("@hasError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                //command.ExecuteNonQuery();

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
                    ValidationException exception = new ValidationException(string.Empty);

                    if (errorDataTable != null)
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (DataRow row in errorDataTable.Rows)
                        {
                            message.Append(string.Format("{1}", row["Name"].ToString(), row["Value"].ToString()));
                        }
                        exception.Data.Add("IsExists", message);
                    }

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
                        transaction.Rollback();

                throw;
            }
            finally
            {
                // Dispose.
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }
        }


        public List<Test> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchTests";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable testDataTable = new DataTable("Tests");
                adapter.Fill(testDataTable);

                // Create a list.
                List<Test> tests = null;
                if (testDataTable.Rows.Count > 0)
                    tests = new List<Test>();

                // Iterate each row.
                foreach (DataRow row in testDataTable.Rows)
                {
                    // Create an instance of Test.
                    Test test = new Test(Int32.Parse(row["TestId"].ToString()));
                    test.Name = row["Name"].ToString();
                    test.Description = row["Description"].ToString();
                    test.Reason = row["Reason"].ToString();
                    test.IsActive = bool.Parse(row["IsActive"].ToString());
                    test.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    test.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    test.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    tests.Add(test);
                }

                // Return the list.
                return tests;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
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

                    // Clear pool.
                    SqlConnection.ClearPool(mDbConnection);
                    SqlConnection.ClearAllPools();
                }
            }
            catch { throw; }
        }

        #endregion


        public List<Test> RetrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveAllTests";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable testDataTable = new DataTable("Tests");
                adapter.Fill(testDataTable);

                // Create a list.
                List<Test> tests = null;
                if (testDataTable.Rows.Count > 0)
                    tests = new List<Test>();

                // Iterate each row.
                foreach (DataRow row in testDataTable.Rows)
                {
                    // Create an instance of Test.
                    Test test = new Test(Int32.Parse(row["TestId"].ToString()));
                    test.Name = row["Name"].ToString();
                    test.Description = row["Description"].ToString();
                    test.Reason = row["Reason"].ToString();
                    test.IsActive = bool.Parse(row["IsActive"].ToString());
                    test.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    test.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    test.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    test.CustomData.Add("IsActive", row["TIsActive"].ToString());
                    test.CustomData.Add("CustomActive", row["Active"].ToString());


                    // Add to list.
                    tests.Add(test);
                }

                // Return the list.
                return tests;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public List<Test> RetrieveByProject(int projectId, int platformId)
        {
            throw new NotImplementedException();
        }

        public List<Test> RetrieveByProject(int projectId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveTestsByProject";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ProjectId", SqlDbType.Int).Value = projectId;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable testDataTable = new DataTable("Tests");
                adapter.Fill(testDataTable);

                // Create a list.
                List<Test> tests = null;
                if (testDataTable.Rows.Count > 0)
                    tests = new List<Test>();

                // Iterate each row.
                foreach (DataRow row in testDataTable.Rows)
                {
                    // Create an instance of Test.
                    Test test = new Test(Int32.Parse(row["TestId"].ToString()));
                    test.Name = row["Name"].ToString();
                    test.Description = row["Description"].ToString();
                    test.Reason = row["Reason"].ToString();
                    test.IsActive = bool.Parse(row["IsActive"].ToString());
                    test.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    test.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());

                    test.CustomData.Add("ProjectId", row["ProjectId"].ToString());
                    test.CustomData.Add("PlatformId", row["PlatformId"].ToString());
                    test.CustomData.Add("PlatformName", row["PlatformName"].ToString());
                    test.CustomData.Add("IsActive", row["TIsActive"].ToString());
                    test.CustomData.Add("CreateUserId", row["CreateUserId"].ToString());
                    test.CustomData.Add("CreateUserName", row["CreateUserName"].ToString());
                    test.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    test.CustomData.Add("CustomActive", row["Active"].ToString());

                    // Add to list.
                    tests.Add(test);
                }

                // Return the list.
                return tests;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }
    }
}
