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
    internal sealed partial class LocationService
        : ILocationService
    {
        #region Class variables

        IAppManager _appManager;
        SqlConnection mDbConnection;

        #endregion
        public Location Retrieve(int id)
        {
            try
            {
                int[] ids = { id };
                List<Location> Locations = this.Retrieve(ids);

                return (Locations.Count > 0) ? Locations[0] : null;
            }
            catch { throw; }
        }

        public List<Location> Retrieve(int[] ids)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<Location> Locations = null;
            try
            {
                // Build xml.
                StringBuilder xml = new StringBuilder();
                xml.Append("<Locations>");
                foreach (int element in ids)
                {
                    xml.Append(string.Format("<Location><Id>{0}</Id></Location>", element));
                }
                xml.Append("</Locations>");

                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveLocationData";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = xml.ToString();

                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable locationDataTable = new DataTable("Locations");
                adapter.Fill(locationDataTable);

                // Create a list.

                if (locationDataTable.Rows.Count > 0)
                    Locations = new List<Location>();

                // Iterate each row.
                foreach (DataRow row in locationDataTable.Rows)
                {
                    // Create an instance of Locations.
                    Location location = new Location(Int32.Parse(row["locationId"].ToString()));
                    location.City = row["City"].ToString();
                    location.State = row["State"].ToString();
                    location.Country = row["Country"].ToString();
                    location.TimeZoneId = Int32.Parse(row["TimeZoneId"].ToString());
                    location.CustomData.Add("TimeZoneName", row["TimeZoneName"].ToString());
                    location.Reason = row["Reason"].ToString();
                    location.IsActive = bool.Parse(row["IsActive"].ToString());
                    location.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    location.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    // Add to list.
                    Locations.Add(location);
                }

                // Return the list.

            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();

            }
            return Locations;
        }

        public void Update(Location entity)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            SqlTransaction transaction = null;
            try
            {
                // Define command.
                command = new SqlCommand();
                command = mDbConnection.CreateCommand();
                command.CommandText = "UpdateLocations";
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
                            message.Append(string.Format("{1}", row["City"].ToString(), row["Value"].ToString()));
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

        public List<Location> Search(SearchCriteria criteria, int userId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchLocations_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();
                // Execute command.
                adapter = new SqlDataAdapter(command);
                DataTable locationDataTable = new DataTable("Locations");
                adapter.Fill(locationDataTable);
                // Create an instance of file list.
                List<Location> locationSearch = null;
                if (locationDataTable.Rows.Count > 0)
                    locationSearch = new List<Location>();

                foreach (DataRow row in locationDataTable.Rows)
                {
                    // Create an instance of Locations.
                    Location location = new Location(Int32.Parse(row["locationId"].ToString()));
                    location.City = row["City"].ToString();
                    location.Country = row["Country"].ToString();
                    location.State = row["State"].ToString();
                    location.TimeZoneId = Int32.Parse(row["TimeZoneId"].ToString());
                    location.Reason = row["Reason"].ToString();
                    location.IsActive = bool.Parse(row["IsActive"].ToString());
                    //if (location.LastUpdateUserId >= 0)
                    // location.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    if (location.LastUpdateDate != null)
                        location.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    location.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                    location.CustomData.Add("TimeZoneName", row["TimeZoneName"].ToString());
                    location.CustomData.Add("TimeZoneId", row["TimeZoneId"].ToString());
                    //location.IsActive = bool.Parse(row["IsActive"].ToString());
                    //location.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                    //location.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                    // Add to list.
                    locationSearch.Add(location);
                }

                // Return the list.
                return locationSearch;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }


        //public List<Location> Search(SearchCriteria criteria)
        //{
        //    SqlCommand command = null;
        //    SqlDataAdapter adapter = null;
        //    try
        //    {
        //        // Define command.
        //        command = mDbConnection.CreateCommand();
        //        command.CommandText = "SearchLocations";
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();
        //        // Execute command.
        //        adapter = new SqlDataAdapter(command);
        //        DataTable locationDataTable = new DataTable("Locations");
        //        adapter.Fill(locationDataTable);
        //        // Create an instance of file list.
        //        List<Location> locationSearch = null;
        //        if (locationDataTable.Rows.Count > 0)
        //            locationSearch = new List<Location>();

        //        foreach (DataRow row in locationDataTable.Rows)
        //        {
        //            // Create an instance of Locations.
        //            Location location = new Location(Int32.Parse(row["locationId"].ToString()));
        //            location.City = row["City"].ToString();
        //            location.Country = row["Country"].ToString();
        //            location.State = row["State"].ToString();
        //            location.TimeZoneId = Int32.Parse(row["TimeZoneId"].ToString());
        //            location.Reason = row["Reason"].ToString();
        //            location.IsActive = bool.Parse(row["IsActive"].ToString());
        //            //if (location.LastUpdateUserId >= 0)
        //            // location.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
        //            if (location.LastUpdateDate != null)
        //                location.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
        //            location.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
        //            location.CustomData.Add("TimeZoneName", row["TimeZoneName"].ToString());
        //            location.CustomData.Add("TimeZoneId", row["TimeZoneId"].ToString());
        //            //location.IsActive = bool.Parse(row["IsActive"].ToString());
        //            //location.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
        //            //location.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
        //            // Add to list.
        //            locationSearch.Add(location);
        //        }

        //        // Return the list.
        //        return locationSearch;
        //    }
        //    catch { throw; }
        //    finally
        //    {
        //        // Dispose.
        //        if (adapter != null) adapter.Dispose();
        //        if (command != null) command.Dispose();
        //    }
        //}
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
