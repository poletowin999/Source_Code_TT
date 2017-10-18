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
    internal sealed partial class ProjectService
        : IProjectService
    {


        List<Project> IProjectService.RetrieveByClient(int clientId, int userid)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable menuDataTable = null;

            try
            {
                // Define command.
                command = mDbConnection.CreateCommand();

                command.CommandText = "RetrieveProjectsByClient_v3";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ClientId", SqlDbType.Int).Value = clientId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userid;

                // Execute command.
                adapter = new SqlDataAdapter(command);
                menuDataTable = new DataTable("Projects");
                adapter.Fill(menuDataTable);

                // Create a list.
                List<Project> projects = null;
                if (menuDataTable.Rows.Count > 0)
                    projects = new List<Project>();

                // Retrieve the list of project.
                projects = RetrieveProjects(menuDataTable);

                // Return the list.
                return projects;
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
