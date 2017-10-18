<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<script RunAt="server">
   
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        RegisterRoutes(RouteTable.Routes);
    }
    
    static void RegisterRoutes(RouteCollection routes)
    {
        routes.MapPageRoute("DefaultPage", "Default", "~/Default.aspx");
        routes.MapPageRoute("HomePage", "Home", "~/Homepage.aspx");
        routes.MapPageRoute("ReportsView", "Reports/View", "~/Reports/ReportsView.aspx");
        routes.MapPageRoute("Dashboard", "Reports/Dashboard", "~/Reports/ReportWorkDurationAndActivityStatus.aspx");
        routes.MapPageRoute("ReportsActivitySummary", "Reports/Activity-Summary", "~/Reports/ActivitySummaryReportView.aspx");
        routes.MapPageRoute("ReportsActivityNotEntered", "Reports/Activity-NotEntered", "~/Reports/ActivityNotEnteredReportView.aspx");
        routes.MapPageRoute("ReportsActivityPayroll", "Reports/Activity-Payroll", "~/Reports/PayrollActivityReportView.aspx");
        routes.MapPageRoute("ReportsBillingInvoice", "Reports/BillingInvoice", "~/Reports/BillingInvoiceReportView.aspx");
        routes.MapPageRoute("ReportsWorkedDuration", "Reports/WorkedDuration", "~/Reports/WorkedDuration.aspx");
        routes.MapPageRoute("ReportsUserHierarchyView", "Reports/UserHierarchy-View", "~/Reports/UserHierarchyReportView.aspx");
        routes.MapPageRoute("ReportsActivityUnApproved", "Reports/Activity-UnApproved", "~/Reports/UnApprovedActivities.aspx");
        routes.MapPageRoute("ReportsUserMaster", "Reports/User-Master", "~/Reports/UserMasterReportView.aspx");
        routes.MapPageRoute("ReportsActivityDurationMismatch", "Reports/Activity-DurationMismatch", "~/Reports/ActivityDurationMismatchReportView.aspx");
        routes.MapPageRoute("ReportsOverTime", "Reports/OverTime", "~/Reports/OverTimeReport.aspx");
        routes.MapPageRoute("ReportsAttendance", "Reports/Attendance", "~/Reports/AttendanceRegister.aspx");
        routes.MapPageRoute("ReportsSalary", "Reports/Salary", "~/Reports/SalaryRegister.aspx");
        routes.MapPageRoute("LOB", "Reports/LOB", "~/Reports/PayrollActivityReportView_LOB.aspx");

        routes.MapPageRoute("SwipeRegister", "Reports/SwipeRegister", "~/Reports/SwipeRegister.aspx");
        
        routes.MapPageRoute("ReportsAgencyworkers", "Reports/Agencyworkers", "~/Reports/Agencyworkers.aspx");
        routes.MapPageRoute("ReportsUtilization", "Reports/Utilization", "~/Reports/Utilization.aspx");
        routes.MapPageRoute("ReportsAdmin", "Reports/Admin", "~/Reports/AdminReport.aspx");
        routes.MapPageRoute("ReportsUSPayroll", "Reports/USPayroll", "~/Reports/USPayrollReport.aspx");

        routes.MapPageRoute("ActivitiesList", "Activities/List", "~/Activities/ActivityListView.aspx");
        routes.MapPageRoute("ActivitiesListDetails", "Activities/List-{Action}", "~/Activities/ActivityListView.aspx");
        routes.MapPageRoute("ActivitiesReview", "Activities/Review", "~/Activities/ActivityReviewView.aspx");
        routes.MapPageRoute("ActivitiesReviewDetails", "Activities/Review-{Action}", "~/Activities/ActivityReviewView.aspx");
        routes.MapPageRoute("ActivitiesEdit", "Activities/Edit", "~/Activities/ActivityEditView.aspx");
        routes.MapPageRoute("ActivitiesEditDetails", "Activities/033-{Id}-Edit", "~/Activities/ActivityEditView.aspx");
        routes.MapPageRoute("ActivitiesEditAction", "Activities/037-{Id}-{Action}-Edit", "~/Activities/ActivityEditView.aspx");
        routes.MapPageRoute("ActivitiesReset", "Activities/Reset", "~/Activities/ActivityResetView.aspx");
        routes.MapPageRoute("ActivitiesUserSwipe", "Activities/User-Swipe", "~/Activities/UserSwipeView.aspx");
        routes.MapPageRoute("Activities/UserSwipeEdit", "Activities/UserSwipe-Edit", "~/Activities/UserSwipeEditView.aspx");

        routes.MapPageRoute("UsersAdd", "Users/Add", "~/Users/UserEditView.aspx");
        routes.MapPageRoute("UsersList", "Users/List", "~/Users/UserListView.aspx");
        routes.MapPageRoute("UsersHierarchyView", "Users/UserHierarchy-View", "~/Users/UserHierarchyView.aspx");
        routes.MapPageRoute("UsersTempId", "Users/TempId", "~/Users/UserTempId.aspx");
        routes.MapPageRoute("UsersPasswordChangeView", "Users/PasswordChange", "~/Users/PasswordChangeView.aspx");
        routes.MapPageRoute("UsersEdit", "Users/031-{Id}-User-Edit", "~/Users/UserEditView.aspx");
        routes.MapPageRoute("UsersPasswordChange", "Users/PasswordChangeLogin", "~/users/PasswordChangeLogin.aspx");

        routes.MapPageRoute("MastersCommonList", "Masters/Common-{Master}-List", "~/Masters/CommonMastersListView.aspx");
        routes.MapPageRoute("MastersClientList", "Masters/Client-List", "~/Masters/ClientListView.aspx");
        routes.MapPageRoute("MastersProjectList", "Masters/Project-List", "~/Masters/ProjectListView.aspx");
        routes.MapPageRoute("MastersWorkList", "Masters/Work-List", "~/Masters/WorkTypeListView.aspx");
        routes.MapPageRoute("MastersLocationList", "Masters/Location-List", "~/Masters/LocationListView.aspx");
        routes.MapPageRoute("MastersTimeZoneList", "Masters/TimeZone-List", "~/Masters/TimeZoneListView.aspx");
        routes.MapPageRoute("MastersIPConfig", "Masters/IPConfig", "~/Masters/IPConfig.aspx");
        routes.MapPageRoute("MastersProjectEdit", "Masters/Project-Edit", "~/Masters/ProjectEditView.aspx");
        routes.MapPageRoute("MastersProjectEditDetails", "Masters/032-{ProjectId}-Project-Edit", "~/Masters/ProjectEditView.aspx");

        routes.MapPageRoute("UsersPermission", "UsersPermission/List", "~/UsersProfile/wfrmUserPermission.aspx");
        routes.MapPageRoute("UsersPermissionAdd", "UsersPermission/Add", "~/UsersProfile/wfrmAddEditUserPermission.aspx");
        routes.MapPageRoute("UsersPermissionEdit", "UsersPermission/{Id}-Permission-{UIMODEPERMISSION}", "~/UsersProfile/wfrmAddEditUserPermission.aspx");
        routes.MapPageRoute("AccessLevels", "AccessLevels/List", "~/UsersProfile/wfrmAccessLevels.aspx");
        routes.MapPageRoute("AccessLevelsAdd", "AccessLevels/Add", "~/UsersProfile/wfrmAddEditAccessLevel.aspx");
        routes.MapPageRoute("AccessLevelsEdit", "AccessLevels/{Id}-AccessLevel-{UIMODEACCESS}", "~/UsersProfile/wfrmAddEditAccessLevel.aspx");

        routes.MapPageRoute("MiscLogoff", "Misc/Logoff", "~/Misc/Logoff.aspx");
        routes.MapPageRoute("MiscAppExpire", "Misc/App-Expire", "~/Misc/AppExpireView.aspx");
        routes.MapPageRoute("MiscPageNotFound", "Misc/404", "~/Misc/404.aspx");

      
      
    }
    
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {

        Exception ex = Server.GetLastError();
        if (ex is HttpException)
        {
            if (((HttpException)(ex)).GetHttpCode() == 404)
                Response.Redirect("~/Misc/404");
                return;
        }
        
        // Code that runs when an unhandled error occurs
        if (!ex.Message.Equals("Missing URL parameter: IterationId"))
        {
            Response.Redirect("~/Misc/App-Expire");
        }              
 
       // clear error on server 
       Server.ClearError();  
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
  
</script>
