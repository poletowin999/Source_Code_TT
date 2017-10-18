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

        IAppManager mAppManager1;
        SqlConnection mDbConnection1;

        #endregion

        public IAppManager AppManager
        {
            get
            {
                return this.mAppManager1;
            }
            set
            {
                mAppManager1 = value;
                if (value != null)
                    mDbConnection1 = this.mAppManager1.DbConnectionProvider.GetDefaultDbConnectionInstance();

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


    }
}
