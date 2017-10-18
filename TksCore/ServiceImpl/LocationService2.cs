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


        public List<Location> RetrieveByProject(int projectId)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;

            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveLocationsByProject_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ProjectId", SqlDbType.Int).Value = projectId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = _appManager.LoginUser.Id;
                 
                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Projects");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Location> locations = null;
                if (menuDataTable.Rows.Count > 0)
                    locations = new List<Location>();

                // Retrieve the list of location.
                locations = Retrieveloations(menuDataTable);

                // Return the list.
                return locations;
            }
            catch { throw; }
            finally
            {
                // Dispose.
                if (adapter != null) adapter.Dispose();
                if (command != null) command.Dispose();
            }
        }


        private List<Location> Retrieveloations(DataTable projectDataTable)
        {

            List<Location> listlocations = null;
            // Create an instance.
            listlocations = new List<Location>();

            // Iterate each row.
            foreach (DataRow row in projectDataTable.Rows)
            {
                // Create an instance of Role.
                Location location = new Location(Int32.Parse(row["LocationId"].ToString()));
                location.City = row["City"].ToString();
                location.Country = row["Country"].ToString();
                location.State = row["State"].ToString();
                location.Reason = row["Reason"].ToString();
                location.CustomData.Add("LocationId", row["LocationId"].ToString());
                location.TimeZoneId = Int32.Parse(row["TimeZoneId"].ToString());
                location.IsActive = bool.Parse(row["MIsActive"].ToString());
                location.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                location.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());

                location.CustomData.Add("ProjectId", row["ProjectId"].ToString());
                location.CustomData.Add("IsActive", row["IsActive"].ToString());
                location.CustomData.Add("TimeZoneName", row["TimeZoneName"].ToString());
                location.CustomData.Add("CreateUserId", row["CreateUserId"].ToString());
                location.CustomData.Add("CreateUserName", row["CreateUserName"].ToString());
                location.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                location.CustomData.Add("CustomActive", row["Active"].ToString());
                location.CustomData.Add("LocationManagerId", row["ManagerId"].ToString());
                location.CustomData.Add("LocationManager", row["LocationManager"].ToString());

                // Add to list.
                listlocations.Add(location);

            }

            // Return the list.
            return listlocations;

        }


        public List<Location> ReatrieveAll()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;

            try
            {

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "RetrieveLocationsAll";
                command.CommandType = CommandType.StoredProcedure;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Locations");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Location> locations = null;
                if (menuDataTable.Rows.Count > 0)
                    locations = new List<Location>();

                // Retrieve the list of location.
                locations = Retrieveloations(menuDataTable);

                // Return the list.
                return locations;
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
