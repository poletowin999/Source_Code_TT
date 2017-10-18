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
    internal sealed class LanguageService
        : ILanguageService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion

        public Language Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Language> languages = this.Retrieve(ids);

                return (languages.Count > 0) ? languages[0] : null;
            }
            catch { throw; }
        }

        public List<Language> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Languages>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Language><Id>{0}</Id></Language>", element));
                }
                xml.Append("</Languages>");

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveLanguages";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable languageDataTable = new DataTable("Languages");
                adapter.Fill(languageDataTable);

                // Create a list.
                List<Language> languages = null;
                if (languageDataTable.Rows.Count > 0)
                    languages = new List<Language>();

                // Iterate each row.
                foreach (DataRow row in languageDataTable.Rows)
                {
                    // Create an instance of Language.
                    Language language = new Language(Int32.Parse(row["LanguageId"].ToString()));
                    language.Name = row["Name"].ToString();
                    language.Description = row["Description"].ToString();
                    language.Reason = row["Reason"].ToString();
                    language.IsActive = bool.Parse(row["IsActive"].ToString());
                    language.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    language.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    language.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    languages.Add(language);
                }

                // Return the list.
                return languages;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }

        public void Update(Language entity)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            SqlDataAdapter adapter = null;

            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateLanguages";
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
                    //ValidationException exception = new ValidationException("Validation error(s) occurred.");
                    ValidationException exception = new ValidationException("");

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


        public List<Language> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchLanguages";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable languageDataTable = new DataTable("Languages");
                adapter.Fill(languageDataTable);

                // Create a list.
                List<Language> languages = null;
                if (languageDataTable.Rows.Count > 0)
                    languages = new List<Language>();

                // Iterate each row.
                foreach (DataRow row in languageDataTable.Rows)
                {
                    // Create an instance of Language.
                    Language language = new Language(Int32.Parse(row["LanguageId"].ToString()));
                    language.Name = row["Name"].ToString();
                    language.Description = row["Description"].ToString();
                    language.Reason = row["Reason"].ToString();
                    language.IsActive = bool.Parse(row["IsActive"].ToString());
                    language.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    language.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    language.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());

                    // Add to list.
                    languages.Add(language);
                }

                // Return the list.
                return languages;
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
    }
}
