using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

using Tks.Model;
using Tks.Entities;
using Tks.Services;


namespace Tks.ServiceImpl
{
    internal sealed partial class ProjectService
        : IProjectService
    {


        #region Class Variables

        IAppManager mAppManager;
        SqlConnection mDbConnection;

        #endregion

        public Project Retrieve(int id)
        {
            try
            {
                int[] ids = { id };

                // Retrieve the project.
                List<Project> project = this.Retrieve(ids);

                // Return the project.
                return (project.Count > 0) ? project[0] : null;
            }
            catch { throw; }
        }

        public List<Project> Retrieve(int[] ids)
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
                command.CommandText = "RetrieveProjects";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = id.ToString();

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

        private StringBuilder GetIds(int[] ids)
        {
            // Build xml.
            StringBuilder xml = new StringBuilder();
            xml.Append("<Projects>");
            foreach (int element in ids)
            {
                xml.Append(string.Format("<Project><Id>{0}</Id></Project>", element));
            }
            xml.Append("</Projects>");
            return xml;
        }

        private List<Project> RetrieveProjects(DataTable projectDataTable)
        {

            List<Project> listProjects = null;
            // Create an instance.
            listProjects = new List<Project>();

            // Iterate each row.
            foreach (DataRow row in projectDataTable.Rows)
            {
                // Create an instance of Role.
                Project project = new Project(Int32.Parse(row["ProjectId"].ToString()));
                project.Name = row["Name"].ToString();
                project.Description = row["Description"].ToString();
                project.ClientId = Int32.Parse(row["ClientId"].ToString());
                project.Reason = row["Reason"].ToString();
                project.IsActive = bool.Parse(row["IsActive"].ToString());
                project.ResponsibleUserId = Int32.Parse(row["ResponsibleUserId"].ToString());
                project.CategoryId = Int32.Parse(row["CategoryId"].ToString());
                project.LastUpdateUserId = Int32.Parse(row["LastUpdateUserId"].ToString());
                project.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                project.CustomData.Add("ClientName", row["ClientName"].ToString());
                project.CustomData.Add("ResponsibleUserName", row["ResponsibleUserName"].ToString());

                // Add to list.
                listProjects.Add(project);

            }

            // Return the list.
            return listProjects;

        }


        public void Update(Project entity, List<Location> locations, List<Platform> platforms, List<Test> tests)
        {
            SqlCommand command = null;
            SqlTransaction transaction = null;
            StringBuilder process = null;

            try
            {
                process = new StringBuilder();
                // Build xml criteria.

                // Define command.
                command = mDbConnection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateProjects";
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = this.BuildProjectXml(entity, locations, platforms, tests);
          //      command.Parameters.Add("@Category",SqlDbType.Int).Direction =                
                command.Parameters.Add("@IsUserLoggedError", SqlDbType.Bit).Direction = ParameterDirection.Output;
                command.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                try
                {
                    // Execute command within transaction.
                    transaction = mDbConnection.BeginTransaction();
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    if (transaction != null)
                        if (transaction.Connection != null)
                            transaction.Rollback();

                    throw;
                }

                // Get output parameters.
                bool IsUserLoggedError = bool.Parse(command.Parameters["@IsUserLoggedError"].Value.ToString());
                string errorMessage = command.Parameters["@ErrorMessage"].Value.ToString();

                if (IsUserLoggedError)
                {
                    // Create exception instance.
                    ValidationException exception = new ValidationException("Validation error occurred.");
                    exception.Data.Add("USER_LOGGED_Error", errorMessage);

                    throw exception;
                }

            }
            catch { throw; }
            finally
            {
                if (transaction != null) transaction.Dispose();
                if (command != null) command.Dispose();
            }

        }


        private string BuildProjectXml(Project entity, List<Location> projectLocations, List<Platform> ProjectPlatforms, List<Test> projectTests)
        {
            // Build all nodes.
            XmlDocument document = new XmlDocument();
            XmlElement rootNode = document.CreateElement("Projects");

            XmlElement projectElement, tmpElement;

            // Create the project entity.
            projectElement = document.CreateElement("Project");


            tmpElement = Utility.CreateElement(document, "Id", entity.Id.ToString());
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "Name", entity.Name.ToString());
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "Description", entity.Description.ToString());
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "ClientId", entity.ClientId.ToString());
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "ResponsibleUserId", entity.ResponsibleUserId.ToString());
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "Reason", entity.Reason);
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "IsActive", (entity.IsActive) ? "1" : "0");
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "LastUpdateUserId", entity.LastUpdateUserId.ToString());
            projectElement.AppendChild(tmpElement);

            tmpElement = Utility.CreateElement(document, "CategoryId", entity.CategoryId.ToString());
            projectElement.AppendChild(tmpElement);

            rootNode.AppendChild(projectElement);

            // Create the location entity.
            XmlElement locationsElement;

            locationsElement = document.CreateElement("Locations");

            foreach (Location location in projectLocations)
            {

                // Create location element.
                XmlElement locationElement = document.CreateElement("Location");

                locationElement.Attributes.Append(Utility.CreateAttribute(document, "Id", location.Id.ToString()));

                tmpElement = Utility.CreateElement(document, "IsActive", location.CustomData["IsActive"].ToString());
                locationElement.AppendChild(tmpElement);

                tmpElement = Utility.CreateElement(document, "LastUpdateUserId", location.LastUpdateUserId.ToString());
                locationElement.AppendChild(tmpElement);

                tmpElement = Utility.CreateElement(document, "LocationManagerId", location.CustomData["LocationManagerId"].ToString());
                locationElement.AppendChild(tmpElement);

                // Add to location element.
                locationsElement.AppendChild(locationElement);
            }

            // Add locations to root node.
            rootNode.AppendChild(locationsElement);

            // Create the platform entity.
            XmlElement platformsElement;

            platformsElement = document.CreateElement("Platforms");

            foreach (Platform platform in ProjectPlatforms)
            {
                // Create platform element.
                XmlElement platformElement = document.CreateElement("platform");


                platformElement.Attributes.Append(Utility.CreateAttribute(document, "Id", platform.Id.ToString()));

                tmpElement = Utility.CreateElement(document, "IsActive", platform.CustomData["IsActive"].ToString());
                platformElement.AppendChild(tmpElement);

                tmpElement = Utility.CreateElement(document, "LastUpdateUserId", platform.LastUpdateUserId.ToString());
                platformElement.AppendChild(tmpElement);

                // Add to platform element.
                platformsElement.AppendChild(platformElement);
            }

            // Add platforms to root node.
            rootNode.AppendChild(platformsElement);

            // Create the platform entity.
            XmlElement testsElement;

            testsElement = document.CreateElement("Tests");

            foreach (Test test in projectTests)
            {
                // Create test element.
                XmlElement testElement = document.CreateElement("Test");

                // Add id as attribute.
                testElement.Attributes.Append(Utility.CreateAttribute(document, "PlatformId", test.CustomData["PlatformId"].ToString()));

                tmpElement = Utility.CreateElement(document, "Id", test.Id.ToString());
                testElement.AppendChild(tmpElement);

                tmpElement = Utility.CreateElement(document, "IsActive", test.CustomData["IsActive"].ToString());
                testElement.AppendChild(tmpElement);

                tmpElement = Utility.CreateElement(document, "LastUpdateUserId", test.LastUpdateUserId.ToString());
                testElement.AppendChild(tmpElement);

                // Add to test element.
                testsElement.AppendChild(testElement);
            }

            // Add tests to root node.
            rootNode.AppendChild(testsElement);

            // Add root node to document.
            document.AppendChild(rootNode);

            // Return the document.
            return document.InnerXml;

        }

        public List<Project> Search(SearchCriteria criteria)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            List<Project> Iproject = null;

            Project project = null;
            try
            {
                // Define the command.
                command = mDbConnection.CreateCommand();
                command.CommandText = "SearchTestprojects";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = criteria.GetXml();
                adapter = new SqlDataAdapter(command);
                //Define the DataTable.
                DataTable ProjectDataTable = new DataTable();
                adapter.Fill(ProjectDataTable);
                if (ProjectDataTable.Rows.Count > 0)
                {
                    Iproject = new List<Project>(ProjectDataTable.Rows.Count);
                    foreach (DataRow row in ProjectDataTable.Rows)
                    {
                        //create a  instances for Project.
                        project = new Project(Convert.ToInt32(row["ProjectId"].ToString()));
                        // project.Name = row["Name"].ToString();
                        project.CustomData.Add("ClientName", row["ClientName"].ToString());
                        project.Name = row["Name"].ToString();
                        project.Description = row["Description"].ToString();
                        project.CustomData.Add("Manager", row["Manager"].ToString());
                        project.IsActive = bool.Parse(row["IsActive"].ToString());
                        project.CustomData.Add("LastUpdateUserName", row["LastUpdateUserName"].ToString());
                        project.LastUpdateDate = DateTime.Parse(row["LastUpdateDate"].ToString());
                        //project.CustomData.Add("LocationName",row["LocationName"].ToString());

                        //Add project to the List
                        Iproject.Add(project);
                    }

                }

                return Iproject;
            }


            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }


            }
        }

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
