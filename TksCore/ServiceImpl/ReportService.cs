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
    internal sealed partial class ReportService
        : IReportService
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

        public DataTable RetrieveClients()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RerieveClients";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Clients");
                adapter.Fill(dtClients);

                // Return clients.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveProjectByClients(int[] ClientIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;
            string Clientxml = null;
            try
            {

                // Build xml.
                Clientxml = this.BuildXmlforClient(ClientIds);

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveProjectsbyClients_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = Clientxml;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Projects");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }


        private string BuildXmlforClient(int[] ClientIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();
            arr.Append("<Clients>");
            int Clientid = 0;

            for (int i = 0; i <= ClientIds.Length - 1; i++)
            {
                Clientid = Convert.ToInt32(ClientIds[i].ToString());
                if (Clientid > 0)
                {
                    arr.Append("<Client Id=\" " + Clientid + " \"/>");
                }
            }

            arr.Append("</Clients>");

            return arr.ToString();
        }



        private string BuildXmlforLocations(int[] ClientIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();
            arr.Append("<Locations>");
            int Clientid = 0;

            for (int i = 0; i <= ClientIds.Length - 1; i++)
            {
                Clientid = Convert.ToInt32(ClientIds[i].ToString());
                if (Clientid > 0)
                {
                    arr.Append("<Location Id=\" " + Clientid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            return arr.ToString();
        }


        public DataTable RetrieveActivitySummaryReportDataNew(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, int[] iManagerIds, bool IsMiscellaneous)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveActivitySummaryReport_v5";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAcivitySummarynew(fromDate, toDate, clientIds, projectIds, loginUser, iUserIds, iLocationIds, iManagerIds);
                command.Parameters.Add("@IsMiscellaneous", SqlDbType.Bit).Value = IsMiscellaneous;
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrievePayrollActivity_LOB(int loginUserId, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] clients)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrievePayrollActivity_LOB_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollSummary(fromDate, toDate, Projects, BillTypes, WorkTypes, Locations, Users, clients);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveActivitySummaryReportData(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, bool IsMiscellaneous)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveActivitySummaryReport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAcivitySummary(fromDate, toDate, clientIds, projectIds, loginUser, iUserIds, iLocationIds);
                command.Parameters.Add("@IsMiscellaneous", SqlDbType.Bit).Value = IsMiscellaneous;
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        private string BuildXmlforAcivitySummarynew(string _fromDate, string _toDate, int[] _clientIds, int[] _projectIds, int _loginUser, int[] _iUserIds, int[] _iLocationIds, int[] _iManagerIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Activity>");
            arr.Append("<FromDate>");
            arr.Append(_fromDate.ToString());
            arr.Append("</FromDate>");
            arr.Append("<ToDate>");
            arr.Append(_toDate.ToString());
            arr.Append("</ToDate>");
            arr.Append("</Activity>");

            arr.Append("<Clients>");
            int Clientid = 0;

            for (int i = 0; i <= _clientIds.Length - 1; i++)
            {
                Clientid = Convert.ToInt32(_clientIds[i].ToString());
                if (Clientid > 0)
                {
                    arr.Append("<Client Id=\" " + Clientid + " \"/>");
                }
            }

            arr.Append("</Clients>");

            //

            arr.Append("<Managers>");
            int Managerid = 0;

            for (int i = 0; i <= _iManagerIds.Length - 1; i++)
            {
                Managerid = Convert.ToInt32(_iManagerIds[i].ToString());
                if (Managerid > 0)
                {
                    arr.Append("<Manager Id=\" " + Managerid + " \"/>");
                }
            }

            arr.Append("</Managers>");

            //

            arr.Append("<Projects>");
            int projectId = 0;

            for (int i = 0; i <= _projectIds.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_projectIds[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Project Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Projects>");

            //arr.Append("<WorkTypes>");
            //int workTypeId = 0;

            //for (int i = 0; i <= _iWorkTypes.Length - 1; i++)
            //{
            //    workTypeId = Convert.ToInt32(_iWorkTypes[i].ToString());
            //    if (workTypeId > 0)
            //    {
            //        arr.Append("<WorkType Id=\" " + workTypeId + " \"/>");
            //    }
            //}

            //arr.Append("</WorkTypes>");

            arr.Append("<Locations>");
            int Locationid = 0;

            for (int i = 0; i <= _iLocationIds.Length - 1; i++)
            {
                Locationid = Convert.ToInt32(_iLocationIds[i].ToString());
                if (Locationid > 0)
                {
                    arr.Append("<Location Id=\" " + Locationid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            arr.Append("<Users>");
            int Userid = 0;

            for (int i = 0; i <= _iUserIds.Length - 1; i++)
            {
                Userid = Convert.ToInt32(_iUserIds[i].ToString());
                if (Userid > 0)
                {
                    arr.Append("<User Id=\" " + Userid + " \"/>");
                }
            }

            arr.Append("</Users>");

            arr.Append("<LoginUserId>");
            arr.Append(_loginUser);
            arr.Append("</LoginUserId>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }

        private string BuildXmlforAcivitySummary(string _fromDate, string _toDate, int[] _clientIds, int[] _projectIds, int _loginUser, int[] _iUserIds, int[] _iLocationIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Activity>");
            arr.Append("<FromDate>");
            arr.Append(_fromDate.ToString());
            arr.Append("</FromDate>");
            arr.Append("<ToDate>");
            arr.Append(_toDate.ToString());
            arr.Append("</ToDate>");
            arr.Append("</Activity>");

            arr.Append("<Clients>");
            int Clientid = 0;

            for (int i = 0; i <= _clientIds.Length - 1; i++)
            {
                Clientid = Convert.ToInt32(_clientIds[i].ToString());
                if (Clientid > 0)
                {
                    arr.Append("<Client Id=\" " + Clientid + " \"/>");
                }
            }

            arr.Append("</Clients>");



            arr.Append("<Projects>");
            int projectId = 0;

            for (int i = 0; i <= _projectIds.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_projectIds[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Project Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Projects>");

            //arr.Append("<WorkTypes>");
            //int workTypeId = 0;

            //for (int i = 0; i <= _iWorkTypes.Length - 1; i++)
            //{
            //    workTypeId = Convert.ToInt32(_iWorkTypes[i].ToString());
            //    if (workTypeId > 0)
            //    {
            //        arr.Append("<WorkType Id=\" " + workTypeId + " \"/>");
            //    }
            //}

            //arr.Append("</WorkTypes>");

            arr.Append("<Locations>");
            int Locationid = 0;

            for (int i = 0; i <= _iLocationIds.Length - 1; i++)
            {
                Locationid = Convert.ToInt32(_iLocationIds[i].ToString());
                if (Locationid > 0)
                {
                    arr.Append("<Location Id=\" " + Locationid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            arr.Append("<Users>");
            int Userid = 0;

            for (int i = 0; i <= _iUserIds.Length - 1; i++)
            {
                Userid = Convert.ToInt32(_iUserIds[i].ToString());
                if (Userid > 0)
                {
                    arr.Append("<User Id=\" " + Userid + " \"/>");
                }
            }

            arr.Append("</Users>");

            arr.Append("<LoginUserId>");
            arr.Append(_loginUser);
            arr.Append("</LoginUserId>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }
        private string BuildXmlforLocationsAndSupervisors(int[] LocationIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();
            arr.Append("<Locations>");
            int Locationid = 0;

            for (int i = 0; i <= LocationIds.Length - 1; i++)
            {
                Locationid = Convert.ToInt32(LocationIds[i].ToString());
                if (Locationid > 0)
                {
                    arr.Append("<Location Id=\" " + Locationid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            return arr.ToString();     
        }


        public DataTable RetrieveSubordinatesDetails(int loginUserId, int[] iUserIds, int[] iLocationIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtsubordinatesDetails = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetreiveSubordinateDetailsForSupervisorAndLocations_new_v2";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@LoginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforHierarchyreport(iUserIds, iLocationIds);

                adapter = new SqlDataAdapter(command);
                dtsubordinatesDetails = new DataTable("SubordinatesDetails");
                adapter.Fill(dtsubordinatesDetails);

                // Return Subordinates.
                if (dtsubordinatesDetails.Rows.Count > 0)
                    return dtsubordinatesDetails;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        private string BuildXmlforPayrollSummary(string _fromDate, string _toDate, int[] _projects, int[] _billTypes, int[] _workTypes, int[] _locations, int[] _users,int[] _clients)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Activity>");
            arr.Append("<FromDate>");
            arr.Append(_fromDate.ToString());
            arr.Append("</FromDate>");
            arr.Append("<ToDate>");
            arr.Append(_toDate.ToString());
            arr.Append("</ToDate>");
            arr.Append("</Activity>");

            arr.Append("<Projects>");
            int projectId = 0;

            for (int i = 0; i <= _projects.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_projects[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Project Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Projects>");



            arr.Append("<BillTypes>");
            int billTypeId = 0;

            for (int i = 0; i <= _billTypes.Length - 1; i++)
            {
                billTypeId = Convert.ToInt32(_billTypes[i].ToString());
                if (billTypeId > 0)
                {
                    arr.Append("<BillType Id=\" " + billTypeId + " \"/>");
                }
            }

            arr.Append("</BillTypes>");

            arr.Append("<WorkTypes>");
            int workTypeId = 0;

            for (int i = 0; i <= _workTypes.Length - 1; i++)
            {
                workTypeId = Convert.ToInt32(_workTypes[i].ToString());
                if (workTypeId > 0)
                {
                    arr.Append("<WorkType Id=\" " + workTypeId + " \"/>");
                }
            }

            arr.Append("</WorkTypes>");

            arr.Append("<Locations>");
            int locationId = 0;

            for (int i = 0; i <= _locations.Length - 1; i++)
            {
                locationId = Convert.ToInt32(_locations[i].ToString());
                if (locationId > 0)
                {
                    arr.Append("<Location Id=\" " + locationId + " \"/>");
                }
            }

            arr.Append("</Locations>");


            arr.Append("<Users>");
            int userId = 0;

            for (int i = 0; i <= _users.Length - 1; i++)
            {
                userId = Convert.ToInt32(_users[i].ToString());
                if (userId > 0)
                {
                    arr.Append("<User Id=\" " + userId + " \"/>");
                }
            }

            arr.Append("</Users>");

            arr.Append("<Clients>");
            int clientTypeId = 0;

            for (int i = 0; i <= _clients.Length - 1; i++)
            {
                clientTypeId = Convert.ToInt32(_clients[i].ToString());
                if (clientTypeId > 0)
                {
                    arr.Append("<Client Id=\" " + clientTypeId + " \"/>");
                }
            }

            arr.Append("</Clients>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }

        private string BuildXmlRetrieveAgencyWorkersLocation(int[] _locaitons, int[] _usertypes)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");

            arr.Append("<Locations>");
            int projectId = 0;

            for (int i = 0; i <= _locaitons.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_locaitons[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Location Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Locations>");



            arr.Append("<UserTypes>");
            int billTypeId = 0;

            for (int i = 0; i <= _usertypes.Length - 1; i++)
            {
                billTypeId = Convert.ToInt32(_usertypes[i].ToString());
                if (billTypeId > 0)
                {
                    arr.Append("<UserType Id=\" " + billTypeId + " \"/>");
                }
            }

            arr.Append("</UserTypes>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }


        private string BuildXmlforPayrollLocationUsers(int[] _locaitons, int[] _users)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");

            arr.Append("<Locations>");
            int projectId = 0;

            for (int i = 0; i <= _locaitons.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_locaitons[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Location Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Locations>");



            arr.Append("<Users>");
            int billTypeId = 0;

            for (int i = 0; i <= _users.Length - 1; i++)
            {
                billTypeId = Convert.ToInt32(_users[i].ToString());
                if (billTypeId > 0)
                {
                    arr.Append("<User Id=\" " + billTypeId + " \"/>");
                }
            }

            arr.Append("</Users>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }

        private string BuildXmlforPayrollLocationUsers1(int[] _locaitons, int[] _users)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");

            arr.Append("<Locations>");
            int projectId = 0;

            for (int i = 0; i <= _locaitons.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_locaitons[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Location Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Locations>");



            arr.Append("<Users>");
            int billTypeId = 0;

            for (int i = 0; i <= _users.Length - 1; i++)
            {
                billTypeId = Convert.ToInt32(_users[i].ToString());
                if (billTypeId > 0)
                {
                    arr.Append("<User Id=\" " + billTypeId + " \"/>");
                }
            }

            arr.Append("</Users>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }



        private string BuildXmlforBillingSummary(string _fromDate, string _toDate, int[] _projects, int[] _laguages, int[] _locations, int[] _users)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Activity>");
            arr.Append("<FromDate>");
            arr.Append(_fromDate.ToString());
            arr.Append("</FromDate>");
            arr.Append("<ToDate>");
            arr.Append(_toDate.ToString());
            arr.Append("</ToDate>");
            arr.Append("</Activity>");

            arr.Append("<Projects>");
            int projectId = 0;

            for (int i = 0; i <= _projects.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_projects[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Project Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Projects>");



            arr.Append("<Languages>");
            int languageId = 0;

            for (int i = 0; i <= _laguages.Length - 1; i++)
            {
                languageId = Convert.ToInt32(_laguages[i].ToString());
                if (languageId > 0)
                {
                    arr.Append("<Language Id=\" " + languageId + " \"/>");
                }
            }

            arr.Append("</Languages>");

            arr.Append("<Locations>");
            int locationId = 0;

            for (int i = 0; i <= _locations.Length - 1; i++)
            {
                locationId = Convert.ToInt32(_locations[i].ToString());
                if (locationId > 0)
                {
                    arr.Append("<Location Id=\" " + locationId + " \"/>");
                }
            }

            arr.Append("</Locations>");


            arr.Append("<Users>");
            int userId = 0;

            for (int i = 0; i <= _users.Length - 1; i++)
            {
                userId = Convert.ToInt32(_users[i].ToString());
                if (userId > 0)
                {
                    arr.Append("<User Id=\" " + userId + " \"/>");
                }
            }

            arr.Append("</Users>");

            arr.Append("</Criteria>");

            return arr.ToString();

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

        public DataTable RetrieveActivityDeatilsReportData(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, bool IsMiscellaneous)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveActivityDetailsReport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAcivitySummary(fromDate, toDate, clientIds, projectIds, loginUser, iUserIds, iLocationIds);
                command.Parameters.Add("@IsMiscellaneous", SqlDbType.Bit).Value = IsMiscellaneous;
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveActivityDeatilsReportDataNew(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, int[] iManagerIds, bool IsMiscellaneous)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveActivityDetailsReport_v5";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAcivitySummarynew(fromDate, toDate, clientIds, projectIds, loginUser, iUserIds, iLocationIds, iManagerIds);
                command.Parameters.Add("@IsMiscellaneous", SqlDbType.Bit).Value = IsMiscellaneous;
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUSPayrollDeatilsReportData(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, int[] iManagerIds, bool IsMiscellaneous, int reporttype)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "USBillingReport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAcivitySummarynew(fromDate, toDate, clientIds, projectIds, loginUser, iUserIds, iLocationIds, iManagerIds);
                command.Parameters.Add("@IsMiscellaneous", SqlDbType.Bit).Value = IsMiscellaneous;
                command.Parameters.Add("@Reporttype", SqlDbType.Int).Value = reporttype;
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUserNotEnteredAcitvity(DateTime fromDate, DateTime toDate, int loginUser, int[] iUserIds, int[] iLocationIds,int Typee)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "UserNotEnteredActivity_new_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@LoginUserId", SqlDbType.Int).Value = loginUser;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAcivitynotentered(iUserIds, iLocationIds);
                command.Parameters.Add("@typee", SqlDbType.Int).Value = Typee;
                command.CommandTimeout = 0;
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("UserNotEnteredActivity");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }
        public DataTable RetrieveUnApprovedAcitvities(DateTime fromDate, DateTime toDate, int loginUser, int[] iUserIds, int[] iLocationIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "UnApprovedActivities_v3";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@LoginUserId", SqlDbType.Int).Value = loginUser;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforHierarchyreport(iUserIds, iLocationIds);
                command.CommandTimeout = 0;
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("UserNotEnteredActivity");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }
  /*      public DataTable RetrieveUserInfo(DateTime formDate, DateTime todate, int[] iLocationIds, String DateFilter, String UserFilter)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtUserInfo = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "UserMasterReport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = formDate;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforUserMasterreport(iLocationIds);
                command.Parameters.Add("@DateFilter", SqlDbType.VarChar).Value = DateFilter;
                command.Parameters.Add("@UserFilter", SqlDbType.VarChar).Value = UserFilter;
                command.CommandTimeout = 0;
                adapter = new SqlDataAdapter(command);
                dtUserInfo = new DataTable("UserMasterInfo");
                adapter.Fill(dtUserInfo);

                // Return clients.
                if (dtUserInfo.Rows.Count > 0)
                    return dtUserInfo;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }*/
        //added by venkatesh to include the user id
        public DataTable RetrieveUserInfo(DateTime formDate, DateTime todate, int[] iLocationIds, String DateFilter, String UserFilter,int loginUser)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtUserInfo = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "UserMasterReport_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = formDate;
                command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = todate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforUserMasterreport(iLocationIds);
                command.Parameters.Add("@DateFilter", SqlDbType.VarChar).Value = DateFilter;
                command.Parameters.Add("@UserFilter", SqlDbType.VarChar).Value = UserFilter;
                command.Parameters.Add("@LoginUser", SqlDbType.Int).Value = loginUser;
                command.CommandTimeout = 0;
                adapter = new SqlDataAdapter(command);
                dtUserInfo = new DataTable("UserMasterInfo");
                adapter.Fill(dtUserInfo);

                // Return clients.
                if (dtUserInfo.Rows.Count > 0)
                    return dtUserInfo;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveParollUsers(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "UserSwipeByMonth";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }


        public DataTable RetrieveProjects()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RerieveProjects_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Projects");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }


        public DataTable RetrieveBillTypes()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RetrieveBillTypes";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("BillTypes");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveWorkTypes()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RetrieveWorks";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("WorkTypes");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveLanguages()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RetrieveLanguagesActivity";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Languages");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrievePayrollActivityNew(int loginUserId, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] clients)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrievePayrollActivity_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollSummary(fromDate, toDate, Projects, BillTypes, WorkTypes, Locations, Users, clients);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }
        public DataTable RetrievePayrollActivity(int loginUserId, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users,int[] clients)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrievePayrollActivity_v3";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollSummary(fromDate, toDate, Projects, BillTypes, WorkTypes, Locations, Users, clients);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveUtilizationDetail(int loginUserId, int rpt, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] clients)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "Utilizationreport_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@rpt", SqlDbType.Int).Value = 1;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollSummary(fromDate, toDate, Projects, BillTypes, WorkTypes, Locations, Users, clients);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveUtilization(int loginUserId, int rpt, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] clients)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "Utilizationreport";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@rpt", SqlDbType.Int).Value = 2;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollSummary(fromDate, toDate, Projects, BillTypes, WorkTypes, Locations, Users, clients);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveBillingActivityNew(int loginUserId, string fromDate, string toDate, int[] Projects, int[] Languages, int[] Locations, int[] Users)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveBillingActivity_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforBillingSummary(fromDate, toDate, Projects, Languages, Locations, Users);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveBillingActivity(int loginUserId, string fromDate, string toDate, int[] Projects, int[] Languages, int[] Locations, int[] Users)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveBillingActivity_v4";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforBillingSummary(fromDate, toDate, Projects, Languages, Locations, Users);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }
        public DataTable RetrieveAgencyWorkers(int loginUserId, int[] Locations, int[] UserTypes)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveAgencyWorkers";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlRetrieveAgencyWorkersLocation(Locations, UserTypes);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }

        }

        public DataTable RetrieveOverTimeHours(int loginUserId, int[] Projects, int[] Languages, int[] Locations, int[] Users, string Paydate)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtAcivitySummary = null;
            //string Clientxml = null;
            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveOTHours";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@PayDate", SqlDbType.VarChar).Value = Paydate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforBillingSummary("", "", Projects, Languages, Locations, Users);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtAcivitySummary = new DataTable("AcivitySummary");
                adapter.Fill(dtAcivitySummary);

                // Return clients.
                if (dtAcivitySummary.Rows.Count > 0)
                    return dtAcivitySummary;
                else
                    return null;

            }
            catch { throw; }

            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveClientsForProject()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RerieveClientsForProjects_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Clients");
                adapter.Fill(dtClients);

                // Return clients.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }
        public DataTable RetrieveLocationsForUSA()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RerieveLocationsForUSALocation";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Locations");
                adapter.Fill(dtClients);

                // Return clients.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }
        public DataTable RetrieveLocationsInUsers()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RerieveLocationsInUsers_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Locations");
                adapter.Fill(dtClients);

                // Return clients.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUsers()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RerieveUsersForLocations_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Locations");
                adapter.Fill(dtClients);

                // Return clients.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveSupervisors()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtManagers = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RetrieveAllManagers_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtManagers = new DataTable("Managers");
                adapter.Fill(dtManagers);

                // Return clients.
                if (dtManagers.Rows.Count > 0)
                    return dtManagers;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable BuildUsersSupervisor(int[] ManagersIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;
            string Clientxml = null;
            try
            {

                // Build xml.
                Clientxml = this.BuildXmlforLocations(ManagersIds);

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RerieveUsersByManagers";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = Clientxml;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Managers");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUsersByLocations(int[] LocationIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtUsers = null;
            string Locationxml = null;
            try
            {

                // Build xml.
                Locationxml = this.BuildXmlforLocations(LocationIds);

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RerieveUsersByLocations_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = Locationxml;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtUsers = new DataTable("Users");
                adapter.Fill(dtUsers);

                // Return clients.
                if (dtUsers.Rows.Count > 0)
                    return dtUsers;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveUsersByLocations1(int[] ClientIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;
            string Clientxml = null;
            try
            {

                // Build xml.
                Clientxml = this.BuildXmlforLocations(ClientIds);

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RerieveUsersByLocations_swipe";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = Clientxml;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Projects");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable AdminCheckinoutDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "AdminUserCheckinout";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveCheckinoutDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveCheckinoutDetails";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveActivityDurationMismatchDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "ActivityDurationReport_v2";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);
                return dtProjects;
             
            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

       
       
        public DataTable RetrieveManagersByLocations(int[] LocationsIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;
            string Locationxml = null;
            try
            {

                // Build xml.
                Locationxml = this.BuildXmlforLocations(LocationsIds);

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveManagersByLocations_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = Locationxml;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Managers");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        private string BuildXmlforAcivitynotentered(int[] _iUserIds, int[] _iLocationIds )
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Locations>");
            int Locationid = 0;

            for (int i = 0; i <= _iLocationIds.Length - 1; i++)
            {
                Locationid = Convert.ToInt32(_iLocationIds[i].ToString());
                if (Locationid > 0)
                {
                    arr.Append("<Location Id=\" " + Locationid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            arr.Append("<Users>");
            int Userid = 0;

            for (int i = 0; i <= _iUserIds.Length - 1; i++)
            {
                Userid = Convert.ToInt32(_iUserIds[i].ToString());
                if (Userid > 0)
                {
                    arr.Append("<User Id=\" " + Userid + " \"/>");
                }
            }

            arr.Append("</Users>");
            arr.Append("</Criteria>");
            return arr.ToString();

        }
        private string BuildXmlforUserMasterreport(int[] _iLocationIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Locations>");
            int Locationid = 0;

            for (int i = 0; i <= _iLocationIds.Length - 1; i++)
            {
                Locationid = Convert.ToInt32(_iLocationIds[i].ToString());
                if (Locationid > 0)
                {
                    arr.Append("<Location Id=\" " + Locationid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            arr.Append("</Criteria>");
            return arr.ToString();

        }

        private string BuildXmlforHierarchyreport(int[] userIds, int[] locationIds)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Locations>");
            int Locationid = 0;

            for (int i = 0; i <= locationIds.Length - 1; i++)
            {
                Locationid = Convert.ToInt32(locationIds[i].ToString());
                if (Locationid > 0)
                {
                    arr.Append("<Location Id=\" " + Locationid + " \"/>");
                }
            }

            arr.Append("</Locations>");

            arr.Append("<Users>");
            int Userid = 0;

            for (int i = 0; i <= userIds.Length - 1; i++)
            {
                Userid = Convert.ToInt32(userIds[i].ToString());
                if (Userid != 0)
                {
                    arr.Append("<User Id=\" " + Userid + " \"/>");
                }
            }

            arr.Append("</Users>");
            arr.Append("</Criteria>");
            return arr.ToString();

        }

        // Retrieve Employee Attendance Details.
        //Added on 03142012 by saravanan
        public DataTable RetrieveAttendanceDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveAttendanceDetails_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        // Retrieve Employee Salary Details.
        //Added on 03142012 by saravanan
        public DataTable RetrieveSalaryDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveSalaryDetails_v1";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveSwipeDetails1(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveSwipeDetails_detail";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers1(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveSwipeDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtProjects = null;

            try
            {

                command = mDbConnection.CreateCommand();

                // Define the command.
                command.CommandText = "RetrieveSwipeDetails";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = loginUserId;
                command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = fromDate;
                command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = toDate;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers1(LocationIds, UserIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtProjects = new DataTable("Users");
                adapter.Fill(dtProjects);

                // Return clients.
                if (dtProjects.Rows.Count > 0)
                    return dtProjects;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable Retrievepayrollschedule()
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtClients = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "Retrievepayrollschedule";
                command.CommandType = CommandType.StoredProcedure;

                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtClients = new DataTable("Retrievepayrollschedule");
                adapter.Fill(dtClients);

                // Return clients.
                if (dtClients.Rows.Count > 0)
                    return dtClients;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        public DataTable RetrieveAgencyDetails(DateTime _fromDate, DateTime _toDate, int[] LocationIds, int[] UsertypeIds)
        {
            SqlCommand command = null;
            SqlDataAdapter adapter = null;
            DataTable dtdetails = null;

            try
            {

                command = mDbConnection.CreateCommand();
                // Define the command.
                command.CommandText = "RetrieveAgencyWorkers";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@loginUserId", SqlDbType.Int).Value = AppManager.LoginUser.Id;
                command.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforAgencyworkers(_fromDate, _toDate, LocationIds, UsertypeIds);
                command.CommandTimeout = 0;
                // Execute the command.
                adapter = new SqlDataAdapter(command);
                dtdetails = new DataTable("RetrieveAgencyWorkers");
                adapter.Fill(dtdetails);

                // Return clients.
                if (dtdetails.Rows.Count > 0)
                    return dtdetails;
                else
                    return null;

            }
            catch { throw; }
            finally
            {
                if (adapter != null) { adapter.Dispose(); }
                if (command != null) { command.Dispose(); }

            }
        }

        private string BuildXmlforAgencyworkers(DateTime _fromDate, DateTime _toDate, int[] _locaitons, int[] _usertype)
        {
            StringBuilder arr = null;

            arr = new StringBuilder();

            arr.Append("<Criteria>");
            arr.Append("<Activity>");
            arr.Append("<FromDate>");
            arr.Append(_fromDate.ToString());
            arr.Append("</FromDate>");
            arr.Append("<ToDate>");
            arr.Append(_toDate.ToString());
            arr.Append("</ToDate>");
            arr.Append("</Activity>");

            arr.Append("<Locations>");
            int projectId = 0;

            for (int i = 0; i <= _locaitons.Length - 1; i++)
            {
                projectId = Convert.ToInt32(_locaitons[i].ToString());
                if (projectId > 0)
                {
                    arr.Append("<Location Id=\" " + projectId + " \"/>");
                }
            }

            arr.Append("</Locations>");



            arr.Append("<UserTypes>");
            int billTypeId = 0;

            for (int i = 0; i <= _usertype.Length - 1; i++)
            {
                billTypeId = Convert.ToInt32(_usertype[i].ToString());
                if (billTypeId > 0)
                {
                    arr.Append("<Usertype Id=\" " + billTypeId + " \"/>");
                }
            }

            arr.Append("</UserTypes>");

            arr.Append("</Criteria>");

            return arr.ToString();

        }

    }
}
