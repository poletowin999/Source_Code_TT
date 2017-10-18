using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Tks.Model;
using Tks.Entities;


namespace Tks.Services
{
    public interface IReportService
        : IEntityService
    {
        // Retrieve clients.
        System.Data.DataTable RetrieveClients();
        // Retrieve Proejcts based on client.
        System.Data.DataTable RetrieveProjectByClients(int[] ClientIds);
        // Retrieve activity summary.
        System.Data.DataTable RetrieveActivitySummaryReportData(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, bool IsMiscellaneous);

        System.Data.DataTable RetrieveActivitySummaryReportDataNew(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, int[] iManagerIds, bool IsMiscellaneous);
        // Retrieve activity details.
        System.Data.DataTable RetrieveActivityDeatilsReportData(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, bool IsMiscellaneous);

        System.Data.DataTable RetrieveActivityDeatilsReportDataNew(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, int[] iManagerIds, bool IsMiscellaneous);

        System.Data.DataTable RetrieveUSPayrollDeatilsReportData(string fromDate, string toDate, int[] clientIds, int[] projectIds, int loginUser, int[] iUserIds, int[] iLocationIds, int[] iManagerIds, bool IsMiscellaneous, int Reporttype);
        
        // Retrieve user not entered activity.
        System.Data.DataTable RetrieveUserNotEnteredAcitvity(DateTime fromDate, DateTime toDate, int loginUser, int[] iUserIds, int[] iLocationIds, int Typee);
        // Retrieve payroll users.
        System.Data.DataTable RetrieveParollUsers(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);
        // RetrieveProjects
        System.Data.DataTable RetrieveProjects();
        // RetrieveProjects
        System.Data.DataTable RetrieveBillTypes();
        // RetrieveProjects
        System.Data.DataTable RetrieveWorkTypes();
        // RetrieveProjects
        System.Data.DataTable RetrieveLanguages();
        // RetrievePayrollActivity.
        System.Data.DataTable RetrieveAgencyWorkers(int loginUserId, int[] Locations, int[] UserTypes);
        // RetrievePayrollActivity.
        System.Data.DataTable RetrievePayrollActivity(int loginUserId, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users,int[] Clients);
        System.Data.DataTable RetrievePayrollActivityNew(int loginUserId, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] Clients);
        // RetrievePayrollActivity.
        // Utilization
        System.Data.DataTable RetrieveUtilization(int loginUserId, int rpt, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] Clients);

        System.Data.DataTable RetrieveUtilizationDetail(int loginUserId, int rpt, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] Clients);

        System.Data.DataTable RetrieveBillingActivity(int loginUserId, string fromDate, string toDate, int[] Projects, int[] Languages, int[] Locations, int[] Users);
        System.Data.DataTable RetrieveBillingActivityNew(int loginUserId, string fromDate, string toDate, int[] Projects, int[] Languages, int[] Locations, int[] Users);
        // RetrieveOverTimeHours.
        System.Data.DataTable RetrieveOverTimeHours(int loginUserId, int[] Projects, int[] Languages, int[] Locations, int[] Users, string PaydateB);
        // Retrieve clients.
        System.Data.DataTable RetrieveClientsForProject();
        // Retrieve locations in users.
        System.Data.DataTable RetrieveLocationsInUsers();
        // Retrieve locationsForUSA.
        System.Data.DataTable RetrieveLocationsForUSA();
        // Retrieve users.
        System.Data.DataTable RetrieveUsers();

        System.Data.DataTable BuildUsersSupervisor(int[] ClientIds);

        // Retrieve Supervisors.
        System.Data.DataTable RetrieveSupervisors();
        // Retrieve users based on location.
        System.Data.DataTable RetrieveUsersByLocations(int[] ClientIds);

        System.Data.DataTable RetrieveUsersByLocations1(int[] ClientIds);

        // Retrieve users based on location.
        System.Data.DataTable RetrieveManagersByLocations(int[] ClientIds);

        // Retrieve Employee check in/out Details.
        System.Data.DataTable RetrieveCheckinoutDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);

        //Retrieve SubordinatesDetails.
        System.Data.DataTable RetrieveSubordinatesDetails(int loginUserId, int[] iLocationIds, int[] iUserIds);
        
            // Retrieve unApproved activities.
        System.Data.DataTable RetrieveUnApprovedAcitvities(DateTime fromDate, DateTime toDate, int loginUser, int[] iUserIds, int[] iLocationIds);
        
        //Retrieve User Information.
     //   System.Data.DataTable RetrieveUserInfo(DateTime formDate, DateTime todate, int[] iLocationIds,  String DateFilter, String UserFilter);

        //Retrieve User Information.
        System.Data.DataTable RetrieveUserInfo(DateTime formDate, DateTime todate, int[] iLocationIds, String DateFilter, String UserFilter, int loginUser);

        // Retrieve ActivityDurationMismatchDetails.
        System.Data.DataTable RetrieveActivityDurationMismatchDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);

        System.Data.DataTable AdminCheckinoutDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);

        // Retrieve Employee Attendance Details.
        //Added on 03142012 by saravanan
        System.Data.DataTable RetrieveAttendanceDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);

        // Retrieve Employee Salary Details.
        //Added on 03142012 by saravanan
        System.Data.DataTable RetrieveSalaryDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);

        System.Data.DataTable RetrieveSwipeDetails(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);
        System.Data.DataTable RetrieveSwipeDetails1(int loginUserId, DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UserIds);
        
        // Retrieve clients.
        System.Data.DataTable Retrievepayrollschedule();

        // Retrieve Agency workers.
        //Added on 04262012 by saravanan
        System.Data.DataTable RetrieveAgencyDetails(DateTime fromDate, DateTime toDate, int[] LocationIds, int[] UsertypeIds);
        System.Data.DataTable RetrievePayrollActivity_LOB(int loginUserId, string fromDate, string toDate, int[] Projects, int[] BillTypes, int[] WorkTypes, int[] Locations, int[] Users, int[] Clients);
    }
}
