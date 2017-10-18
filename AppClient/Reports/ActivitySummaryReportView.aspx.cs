using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Collections;
using Microsoft.Reporting.WebForms;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Reports_ActivitySummaryReportView : System.Web.UI.Page
{

    #region Class Variables

    // private ValidationException ValidClass() ;

    IAppManager mAppManager;
    IReportService reportService = null;

    string _strpath = null;
    string _strDatatablename = null;

    string FROMDATE;
    string TODATE;
    string DATE_RANGE;
    string INVALIDE_DATE;
    string VALIDATIONERROR;

    #endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        //ValidationException ValidClass = new ValidationException();

        try
        {
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;

            LoadLabels();

            if (!Page.IsPostBack)
            {
                // Retrieve client whether available in project.
                BuildClients();
                // Retrieve the projects.
                BuildProjects();

                // Retrieve client whether available in project.
                BuildLocations();

                // Retrieve the projects.
                BuildUsers();

                locationsupervisors();
                // Retrieve the workTypes.
                BuildWorkTypes();

                this.DisplayMessage(string.Empty, false);
                             

                ClearReportSummary();

            }

        }
        catch { throw; }

    }

    private void BuildClients()
    {
        try
        {
           
            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtClients = new DataTable();

            dtClients = reportService.RetrieveClientsForProject();

            chkClient.DataSource = dtClients;
            chkClient.DataTextField = "ClientName";
            chkClient.DataValueField = "ClientId";
            chkClient.DataBind();


        }
        catch { throw; }
    }


    private void BuildProjects()
    {
        try
        {
            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtProjects = new DataTable();

            // Retrieve projects based on clients.
            dtProjects = reportService.RetrieveProjects();

            // Bind the conrol.
            chkProject.DataSource = dtProjects;
            chkProject.DataTextField = "ProjectName";
            chkProject.DataValueField = "ProjectId";
            chkProject.DataBind();
        }
        catch { throw; }
    }

    private void BuildLocations()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtClients = new DataTable();

            dtClients = reportService.RetrieveLocationsInUsers();

            chkLocations.DataSource = dtClients;
            chkLocations.DataTextField = "LocationName";
            chkLocations.DataValueField = "LocationId";
            chkLocations.DataBind();


        }
        catch { throw; }
    }

    private void BuildWorkTypes()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtProjects = new DataTable();
            //DataTable dt = new DataTable();
            //DataColumn WorkTypeId = new DataColumn("WorkTypeId", typeof(System.Int32));
            //dt.Columns.Add(WorkTypeId);
            //DataColumn WorkTypeName = new DataColumn("WorkTypeName", typeof(System.String));
            //dt.Columns.Add(WorkTypeName);
            //DataColumn ConsiderForReport = new DataColumn("ConsiderForReport", typeof(System.Boolean));
            //dt.Columns.Add(ConsiderForReport);

            dtProjects = reportService.RetrieveWorkTypes();
            //DataRow[] dr = dtProjects.Select("ConsiderForReport<>" + false);
            //dtProjects = new DataTable();
            //foreach (DataRow thisRow in dr)
            //{
            //    dt.Rows.Add(thisRow.ItemArray);
            //}
            //dtProjects = dt;
            //chkWorkTypes.DataSource = dtProjects;
            //chkWorkTypes.DataTextField = "WorkTypeName";
            //chkWorkTypes.DataValueField = "WorkTypeId";
            //chkWorkTypes.DataBind();

        }
        catch { throw; }
    }
    private void BuildUsers()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtClients = new DataTable();

            dtClients = reportService.RetrieveUsers();

            chkUsers.DataSource = dtClients;
            chkUsers.DataTextField = "UserName";
            chkUsers.DataValueField = "UserId";
            chkUsers.DataBind();


        }
        catch { throw; }
    }

    private void BuildUsersforSupervisor()
    {
        int[] iClientIds;

        try
        {
            // Get Fileid
            // Retrieve clients.
            string getClientId = GetCheckBoxListManagerIds();

            if (getClientId != "")
            {

                // Create an instance.
                reportService = AppService.Create<IReportService>();
                reportService.AppManager = this.mAppManager;

                DataTable dtProjects = new DataTable();

                // Getting client ids.
                iClientIds = this.RetrieveManageruserIds(getClientId);

                // Retrieve projects based on clients.
                dtProjects = reportService.BuildUsersSupervisor(iClientIds);

                // Bind the conrol.
                chkUsers.DataSource = dtProjects;
                chkUsers.DataTextField = "UserName";
                chkUsers.DataValueField = "UserId";
                chkUsers.DataBind();
            }
            else
            {
                //BuildUsers();
                Locaionusers();
                locationsupervisors();
            }


        }
        catch { throw; }
    }

    private void BuildSupervisors()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtClients = new DataTable();

            dtClients = reportService.RetrieveSupervisors();

            CheckSupervisor.DataSource = dtClients;
            CheckSupervisor.DataTextField = "UserName";
            CheckSupervisor.DataValueField = "UserId";
            CheckSupervisor.DataBind();


        }
        catch { throw; }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {

        try
        {
            _strpath = @"Reports\ActivitySummaryReport.rdlc";
            //_strpath = @"Testing/SampleReport.rdlc";
            _strDatatablename = "DataSet1_RetrievesummaryReportForDesign";
            //_strDatatablename = "DataSet1";

            // Retrieve clients.
            //string getClientId = GetClientIds();


            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);

            if (txtFromDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_FROMDATE", FROMDATE);

            if (txtToDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_TODATE", TODATE);

            DateTime date;
            if (txtFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_FROMDATE", INVALIDE_DATE);
            }

            if (txtToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtToDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_TODATE", INVALIDE_DATE);
            }


            if ((txtFromDate.Text != string.Empty) && (txtToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == true && (DateTime.TryParse(txtToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("ACIVITY_SUMMARY_FROMDATETODATE", DATE_RANGE);
                    }

                }

            }

           this.DisplayMessage(string.Empty, false);


            if (exception.Data.Count > 0)
                throw exception;


            // Retrieve clientids.
            int[] iClientIds = new int[0];
            string getClientId;

            if (chkClientAll.Checked == false)
            {
                getClientId = GetCheckBoxListClientIds();
                if (getClientId != "")
                    iClientIds = this.RetrieveClientIds(getClientId);
            }
            else
            {
                getClientId = "0";
                iClientIds = new int[0];

            }



            // Retrieve projectids.
            int[] iProjectIds = new int[0];
            string getProjectId;

            getProjectId = GetCheckBoxListProjectIds();
            // Retrieve projects.
            if (getProjectId != "")
                iProjectIds = this.RetrieveProjectIds(getProjectId);

            // Retrieve Userid.
            int[] iUserIds = new int[0];
            string getUserId;

            getUserId = GetCheckBoxListUserIds();
            // Retrieve Userid.
            if (getUserId != "")
                iUserIds = this.RetrievetUserIds(getUserId);

            // Retrieve worktypes.
            int[] iWorkTypeIds = new int[0];
            string getWorkTypeId;

            //if (chkWorkTypesAll.Checked == false)
            //{
            //    getWorkTypeId = GetCheckBoxListWorkTypeIds();
            //    if (getWorkTypeId != "")
            //        iWorkTypeIds = this.RetrieveProjectIds(getWorkTypeId);
            //}
            //else
            //{
            //    getWorkTypeId = "0";
            //    iWorkTypeIds = new int[0];
            //}



            // Retrieve LocationIds.
            int[] iLocationIds = new int[0];
            string getLocationId="";

            int[] iManagerIds = new int[0];
            string getManagerId = "";

            getManagerId = GetCheckBoxListManagerIds();
            if (getManagerId != "")
                iManagerIds = this.RetrieveLocationIds(getManagerId);

            getLocationId = GetCheckBoxListLocationIds();
            if (getLocationId != "")
                iLocationIds = this.RetrieveLocationIds(getLocationId);
        
            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;


            DataTable dtAcivitySummary = new DataTable();
            // Retrieve activity summary.
           
            dtAcivitySummary = reportService.RetrieveActivitySummaryReportDataNew(txtFromDate.Text, txtToDate.Text, iClientIds, iProjectIds, mAppManager.LoginUser.Id, iUserIds, iLocationIds,iManagerIds, chkMiscellaneous.Checked);

            
            
//            dataTable.Columns["Marks"].ColumnName = "SubjectMarks";

            //if (dtAcivitySummary.Rows[0]["Type"].ToString() == string.Empty)
            //    dtAcivitySummary.Rows[0]["ProjectName"] = "Data's not found.";

            // Bind into report control.
            //ReportViewer ReportViewer1 = new ReportViewer();

            ReportViewer1.Visible = true;
            ReportViewer2.Visible = false;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            //ReportViewer1.LocalReport.Refresh();
            ReportViewer1.Height = Unit.Parse("300px");
            ReportViewer1.Width = Unit.Parse("100%");
            ReportViewer1.BorderColor = System.Drawing.Color.Black;
            ReportViewer1.BorderWidth = Unit.Parse("1px");
            ReportViewer1.BackColor = System.Drawing.Color.LightSkyBlue;
            ReportViewer1.ShowFindControls = true;
            ReportViewer1.ShowPrintButton = true;
            ReportViewer1.ShowZoomControl = true;
            ReportViewer1.ShowToolBar = true;
            ReportViewer1.ShowRefreshButton = true;
            ReportViewer1.LocalReport.Refresh();

            //divResult.Style.Add("display", "block");
            //divResult.Visible = true;
            //divResult.InnerText = "Result:";

            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {

                divReport.Visible = true;
                divData.Visible = true;

                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);

                ReportParameter rp1 = new ReportParameter("ActivityDate", lblActivityDate.Text);
                ReportParameter rp2 = new ReportParameter("EmployeeId", lblEmpNo.Text);
                ReportParameter rp3 = new ReportParameter("UserType", lblUserType.Text);
                ReportParameter rp4 = new ReportParameter("UserName", lblName.Text);
                ReportParameter rp5 = new ReportParameter("UserRole", lblUserRole.Text);
                ReportParameter rp6 = new ReportParameter("ActivityType", lblActivityType.Text);
                ReportParameter rp7 = new ReportParameter("ClientName", lblclients.Text);
                ReportParameter rp8 = new ReportParameter("CategoryName", lblCategoryName.Text);
                ReportParameter rp9 = new ReportParameter("ProjectName", lblProjects.Text);
                ReportParameter rp10 = new ReportParameter("PlatformName", lblPlatformName.Text);
                ReportParameter rp11 = new ReportParameter("LocationName", lblLocations.Text);
                ReportParameter rp12 = new ReportParameter("Worktype", lblWorktype.Text);
                ReportParameter rp13 = new ReportParameter("Billingtype", lblBillingtype.Text);
                ReportParameter rp14 = new ReportParameter("Hours", lblHours.Text);
                ReportParameter rp15 = new ReportParameter("Minutes", lblMinutes.Text);
                ReportParameter rp16 = new ReportParameter("Totalhours", lblHours.Text);
                ReportParameter rp17 = new ReportParameter("Manager", lblManager.Text);
                ReportParameter rp18 = new ReportParameter("Status", lblStatus.Text);
                ReportParameter rp19 = new ReportParameter("ContractDays", lblContractDays.Text);
                ReportParameter rp20 = new ReportParameter("JoinDate", lblJoinDate.Text);

                //ReportParameter rp21 = new ReportParameter("City", lblCity.Text);
                //ReportParameter rp22 = new ReportParameter("State", lblState.Text);
                //ReportParameter rp23 = new ReportParameter("Country", lblCountry.Text);
                //ReportParameter rp24 = new ReportParameter("TimeZone", lblTimeZone.Text);
                //ReportParameter rp25 = new ReportParameter("Test", lblTest.Text);
                //ReportParameter rp26 = new ReportParameter("Language", lblLanguage.Text);
                //ReportParameter rp27 = new ReportParameter("CommentsHistory", lblCommentsHistory.Text);


                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp2 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp3 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp4 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp5 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp6 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp7 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp8 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp9 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp10 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp11 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp12 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp13 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp14 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp15 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp16 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp17 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp18 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp19 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp20 });

                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp21 });
                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp22 });
                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp23 });
                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp24 });
                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp25 });
                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp26 });
                //this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp27 });


                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                
                ReportViewer1.LocalReport.Refresh();
                               
                //ReportViewer1.LocalReport.Refresh();
                //Session.Add("ActivityDetailsReportView_ActivityDetailsDataSource", rds);
                //Session.Add("RptName", this.Request.PhysicalApplicationPath + @"Reports\ActivitySummaryReport.rdlc");
                //divReport.Controls.Clear();
                //divReport.Controls.Add(ReportViewer1);
            }
            else
            {
                ClearReportSummary();

                ReportViewer1.LocalReport.Refresh();


                divReport.Visible = false;

                divData.Style.Add("display", "block");
                divData.Visible = true;

            }


        }
        catch (ValidationException ve)
        {
            ClearReportSummary();

            DisplayValiationMessage(ve);
            return;
        }

        catch { throw; }

    }

    public void LoadLabels()
    {

        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        //this.mAppManager = this.mAppManager;
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = mAppManager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(this.mAppManager.LoginUser.Id, "REPORTS");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);


        var DATE_VALUE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("FROMTODATE")).FirstOrDefault();
        if (DATE_VALUE != null)
        {

            FROMDATE = Convert.ToString(DATE_VALUE.DisplayText);
            TODATE = Convert.ToString(DATE_VALUE.SupportingText1);
        }

        var DATE_RANGE_GD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("DATERANGE")).FirstOrDefault();
        if (DATE_RANGE_GD != null)
        {

            DATE_RANGE = Convert.ToString(DATE_RANGE_GD.DisplayText);
            INVALIDE_DATE = Convert.ToString(DATE_RANGE_GD.SupportingText1);
        }

        var DATE_VALIDATION = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("VALIDATION")).FirstOrDefault();
        if (DATE_VALIDATION != null)
        {

            VALIDATIONERROR = Convert.ToString(DATE_VALIDATION.DisplayText);
            //INVALIDE_DATE = Convert.ToString(DATE_VALIDATION.SupportingText1);
        }

    }  

    #region Private Members


    private void ClearReportSummary()
    {

     /*
        ControlCollection coll = this.ReportViewer1.Parent.Controls;

        //remember the place, there the old ReportViewer was

        int oldIndex = coll.IndexOf(this.ReportViewer1);

        //remove in from the page

        coll.Remove(this.ReportViewer1);

        //then add new control

        ReportViewer1 = new Microsoft.Reporting.WebForms.ReportViewer();

        ReportViewer1.Height = Unit.Parse("490px");

        ReportViewer1.Width = Unit.Parse("100%");

        coll.AddAt(oldIndex, ReportViewer1);

        this.ReportViewer1.LocalReport.DataSources.Clear();
 * */

    }




    private int[] RetrieveClientIds(string getClientId)
    {
        try
        {
            string strClientIdd = getClientId;
            string[] clientIds = strClientIdd.Split(',');
            int[] iClientIds = new int[clientIds.Length];

            for (int i = 0; i < iClientIds.Length; ++i)
            {
                iClientIds[i] = Int32.Parse(clientIds[i]);
            }

            return iClientIds;

        }
        catch { throw; }
    }


    private int[] RetrieveProjectIds(string getProejctId)
    {
        try
        {
            string strProjectId = getProejctId;
            string[] ProjectIds = strProjectId.Split(',');
            int[] iProjectIds = new int[ProjectIds.Length];

            for (int i = 0; i < iProjectIds.Length; ++i)
            {
                iProjectIds[i] = Int32.Parse(ProjectIds[i]);
            }


            return iProjectIds;

        }
        catch { throw; }
    }


    #endregion


    private void DisplayValiationMessage(ValidationException validationMessage)
    {
        try
        {
            // Build validation error message.
            StringBuilder message = new StringBuilder(string.Format("{0}<ul>", validationMessage.Message));
            foreach (DictionaryEntry entry in validationMessage.Data)
            {
                message.Append(string.Format("<li>{0}</li>", entry.Value));
            }
            message.Append("</ul>");

            // Display validation error.
            this.DisplayMessage(message.ToString(), true);
        }
        catch { throw; }
    }



    private void DisplayMessage(string _message, bool enable)
    {
        try
        {
            if (enable == true)
                statusBar.Style.Add("display", "block");
            else
                statusBar.Style.Add("display", "none");

            statusBar.Visible = enable;
            statusBar.InnerHtml = _message;
        }
        catch { throw; }
    }


    protected void chkClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        clientchange();

    }

    private string GetCheckBoxListClientIds()
    {
        try
        {

            StringBuilder clientIds = null;
            clientIds = new StringBuilder();

            int x = chkClient.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkClient.Items[i].Selected)
                {
                    clientIds.Append(chkClient.Items[i].Value + ",");
                }
            }

            if (clientIds.ToString() != "")
            {

                clientIds = clientIds.Remove(Convert.ToInt32(clientIds.Length) - 1, 1);
            }
            return clientIds.ToString();

        }
        catch { throw; }
    }


    private string GetCheckBoxListProjectIds()
    {
        try
        {

            StringBuilder projectIds = null;
            projectIds = new StringBuilder();

            int x = chkProject.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkProject.Items[i].Selected)
                {
                    projectIds.Append(chkProject.Items[i].Value + ",");
                }
            }

            if (projectIds.ToString() != "")
            {

                projectIds = projectIds.Remove(Convert.ToInt32(projectIds.Length) - 1, 1);
            }
            return projectIds.ToString();

        }
        catch { throw; }
    }


    protected void btnViewDetails_Click(object sender, EventArgs e)
    {
        try
        {

            _strpath = @"Reports\ActivityDetailedReport.rdlc";
            _strDatatablename = "DataSet1_RetrieveDetailsReportForDesign";

            // Retrieve clients.
            //string getClientId = GetClientIds();


            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);

            if (txtFromDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_FROMDATE", FROMDATE);

            if (txtToDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_TODATE", TODATE);


            DateTime date;
            if (txtFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_FROMDATE", INVALIDE_DATE);
            }

            if (txtToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtToDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_TODATE", INVALIDE_DATE);
            }


            if ((txtFromDate.Text != string.Empty) && (txtToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == true && (DateTime.TryParse(txtToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("ACIVITY_SUMMARY_FROMDATETODATE", DATE_RANGE);
                    }

                }

            }

            this.DisplayMessage(string.Empty, false);


            if (exception.Data.Count > 0)
                throw exception;


            // Retrieve clientids.
            int[] iClientIds = new int[0];
            string getClientId;

            if (chkClientAll.Checked == false)
            {
                getClientId = GetCheckBoxListClientIds();
                if (getClientId != "")
                    iClientIds = this.RetrieveClientIds(getClientId);
            }
            else
            {
                getClientId = "0";
                iClientIds = new int[0];

            }



            // Retrieve projectids.
            int[] iProjectIds = new int[0];
            string getProjectId;

            if (chkProjectAll.Checked == false)
            {
                getProjectId = GetCheckBoxListProjectIds();
                // Retrieve projects.
                if (getProjectId != "")
                    iProjectIds = this.RetrieveProjectIds(getProjectId);
            }
            else
            {
                getProjectId = "0";
                iProjectIds = new int[0];
            }

            // Retrieve UserId.
            int[] iUserIds = new int[0];
            string getUserId;

            getUserId = GetCheckBoxListUserIds();
            // Retrieve UserId.
            if (getUserId != "")
                iUserIds = this.RetrievetUserIds(getUserId);



            // Retrieve LocationIds.
            int[] iLocationIds = new int[0];
            string getLocationId = "";

            getLocationId = GetCheckBoxListLocationIds();
            if (getLocationId != "")
                iLocationIds = this.RetrieveLocationIds(getLocationId);

            // Retrieve worktypes.
            int[] iWorkTypeIds = new int[0];
            string getWorkTypeId;

            //if (chkWorkTypesAll.Checked == false)
            //{
            //    getWorkTypeId = GetCheckBoxListWorkTypeIds();
            //    if (getWorkTypeId != "")
            //        iWorkTypeIds = this.RetrieveProjectIds(getWorkTypeId);
            //}
            //else
            //{
            //    getWorkTypeId = "0";
            //    iWorkTypeIds = new int[0];
            //}

            int[] iManagerIds = new int[0];
            string getManagerId = "";

            getManagerId = GetCheckBoxListManagerIds();
            if (getManagerId != "")
                iManagerIds = this.RetrieveLocationIds(getManagerId);

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtAcivitySummary = new DataTable();
            // Retrieve activity summary.
            dtAcivitySummary = reportService.RetrieveActivityDeatilsReportDataNew(txtFromDate.Text, txtToDate.Text, iClientIds, iProjectIds, mAppManager.LoginUser.Id, iUserIds, iLocationIds, iManagerIds, chkMiscellaneous.Checked);

            // Bind into report control.
            //ReportViewer ReportViewer1 = new ReportViewer();
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer1.Visible = false;
            ReportViewer2.Visible = true;
            ReportViewer2.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer2.LocalReport.Refresh();
            

            ReportViewer2.Height = Unit.Parse("300px");
            ReportViewer2.Width = Unit.Parse("100%");
            ReportViewer2.BorderColor = System.Drawing.Color.Black;
            ReportViewer2.BorderWidth = Unit.Parse("1px");
            ReportViewer2.BackColor = System.Drawing.Color.LightSkyBlue;
            ReportViewer2.ShowFindControls = true;
            ReportViewer2.ShowPrintButton = true;
            ReportViewer2.ShowZoomControl = true;
            ReportViewer2.ShowToolBar = true;
            ReportViewer2.ShowRefreshButton = true;
            //ReportViewer2.LocalReport.Dispose();
            
            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {

                divReport.Visible = true;
                divData.Visible = false;

                ReportViewer2.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
                ReportDataSource rdss = new ReportDataSource(_strDatatablename, dtAcivitySummary);


                ReportParameter rp1 = new ReportParameter("ActivityDate", lblActivityDate.Text);
                ReportParameter rp2 = new ReportParameter("EmployeeId", lblEmpNo.Text);
                ReportParameter rp3 = new ReportParameter("UserType", lblUserType.Text);
                ReportParameter rp4 = new ReportParameter("UserName", lblName.Text);
                ReportParameter rp5 = new ReportParameter("UserRole", lblUserRole.Text);
                ReportParameter rp6 = new ReportParameter("ActivityType", lblActivityType.Text);
                ReportParameter rp7 = new ReportParameter("ClientName", lblclients.Text);
                ReportParameter rp8 = new ReportParameter("CategoryName", lblCategoryName.Text);
                ReportParameter rp9 = new ReportParameter("ProjectName", lblProjects.Text);
                ReportParameter rp10 = new ReportParameter("PlatformName", lblPlatformName.Text);
                ReportParameter rp11 = new ReportParameter("City", lblCity.Text);
                ReportParameter rp12 = new ReportParameter("Worktype", lblWorktype.Text);
                ReportParameter rp13 = new ReportParameter("Billingtype", lblBillingtype.Text);
                ReportParameter rp14 = new ReportParameter("Hours", lblHours.Text);
                ReportParameter rp15 = new ReportParameter("Minutes", lblMinutes.Text);
                ReportParameter rp16 = new ReportParameter("Totalhours", lblHours.Text);
                ReportParameter rp17 = new ReportParameter("Manager", lblManager.Text);
                ReportParameter rp18 = new ReportParameter("Status", lblStatus.Text);
                ReportParameter rp19 = new ReportParameter("ContractDays", lblContractDays.Text);
                ReportParameter rp20 = new ReportParameter("JoinDate", lblJoinDate.Text);

                
                ReportParameter rp21 = new ReportParameter("State", lblState.Text);
                ReportParameter rp22 = new ReportParameter("Country", lblCountry.Text);
                ReportParameter rp23 = new ReportParameter("TimeZone", lblTimeZone.Text);

                ReportParameter rp24 = new ReportParameter("Test", lblTest.Text);
                ReportParameter rp25 = new ReportParameter("Language", lblLanguage.Text);
                ReportParameter rp26 = new ReportParameter("CommentsHistory", lblCommentsHistory.Text);

                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp2 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp3 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp4 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp5 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp6 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp7 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp8 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp9 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp10 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp11 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp12 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp13 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp14 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp15 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp16 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp17 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp18 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp19 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp20 });
                
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp21 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp22 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp23 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp24 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp25 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp26 });

                ReportViewer2.LocalReport.DataSources.Clear();
                ReportViewer2.LocalReport.DataSources.Add(rdss);
                ReportViewer2.LocalReport.Refresh();
                //Session.Add("ActivityDetailsReportView_ActivityDetailsDataSource", rdss);
                //Session.Add("RptName", this.Request.PhysicalApplicationPath + @"Reports\ActivityDetailedReport.rdlc");
                //divReport.Controls.Clear();
                //divReport.Controls.Add(ReportViewer1);
            }
            else
            {

                ClearReportSummary();
                ReportViewer1.LocalReport.Refresh();
                divReport.Visible = false;

                divData.Style.Add("display", "block");
                divData.Visible = true;
            }

        }
        catch (ValidationException ve)
        {
            ClearReportSummary();

            DisplayValiationMessage(ve);
            return;
        }

        catch { throw; }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Reports/View");
        }
        catch { throw; }
    }
    protected void chkClientAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkClientAll.Checked == true)
            {
                chkClient.Enabled = false;

                foreach (ListItem item in chkClient.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {

                chkClient.Enabled = true;

                foreach (ListItem item in chkClient.Items)
                {
                    item.Selected = false;
                }
            }
            clientchange();
        }
        catch { throw; }
    }
    protected void chkProjectAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkProjectAll.Checked == true)
            {
                chkProject.Enabled = false;

                foreach (ListItem item in chkProject.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {

                chkProject.Enabled = true;

                foreach (ListItem item in chkProject.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch { throw; }
    }

    protected void chkLocations_SelectedIndexChanged(object sender, EventArgs e)
    {

        Locaionusers();
        locationsupervisors();
    }

    private void locationsupervisors()
    {
        int[] iClientIds;

        try
        {
            // Get Fileid
            // Retrieve clients.
            string getClientId = GetCheckBoxListLocationIds();

            if (getClientId != "")
            {

                // Create an instance.
                reportService = AppService.Create<IReportService>();
                reportService.AppManager = this.mAppManager;

                DataTable dtProjects = new DataTable();

                // Getting client ids.
                iClientIds = this.RetrieveLocationIds(getClientId);

                // Retrieve projects based on clients.
                dtProjects = reportService.RetrieveManagersByLocations(iClientIds);

                // Bind the conrol.
                if (dtProjects != null)
                {
                    CheckSupervisor.DataSource = dtProjects;

                }
                else
                {
                    CheckSupervisor.DataSource = "";
                }

                CheckSupervisor.DataTextField = "UserName";
                CheckSupervisor.DataValueField = "UserId";
                CheckSupervisor.DataBind();
            }
            else
            {
                BuildSupervisors();
            }


        }
        catch { throw; }
    }
    private int[] RetrieveLocationIds(string getClientId)
    {
        try
        {
            string strClientIdd = getClientId;
            string[] clientIds = strClientIdd.Split(',');
            int[] iClientIds = new int[clientIds.Length];

            for (int i = 0; i < iClientIds.Length; ++i)
            {
                iClientIds[i] = Int32.Parse(clientIds[i]);
            }

            return iClientIds;

        }
        catch { throw; }
    }

    private int[] RetrieveManageruserIds(string getClientId)
    {
        try
        {
            string strClientIdd = getClientId;
            string[] clientIds = strClientIdd.Split(',');
            int[] iClientIds = new int[clientIds.Length];

            for (int i = 0; i < iClientIds.Length; ++i)
            {
                iClientIds[i] = Int32.Parse(clientIds[i]);
            }

            return iClientIds;

        }
        catch { throw; }
    }

    protected void chkUsersAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkUsersAll.Checked == false)
            {

                chkUsers.Enabled = true;

                foreach (ListItem item in chkUsers.Items)
                {
                    item.Selected = false;
                }
            }

            else if (chkUsersAll.Checked == true)
            

             {
                chkUsers.Enabled = false;

                foreach (ListItem item in chkUsers.Items)
                {
                    item.Selected = true;
                }

            }
           
        }
        catch { throw; }

    }


    private string GetCheckBoxListLocationIds()
    {
        try
        {

            StringBuilder clientIds = null;
            clientIds = new StringBuilder();

            int x = chkLocations.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkLocations.Items[i].Selected)
                {
                    clientIds.Append(chkLocations.Items[i].Value + ",");
                }
            }

            if (clientIds.ToString() != "")
            {

                clientIds = clientIds.Remove(Convert.ToInt32(clientIds.Length) - 1, 1);
            }
            return clientIds.ToString();

        }
        catch { throw; }
    }

    private string GetCheckBoxListManagerIds()
    {
        try
        {

            StringBuilder clientIds = null;
            clientIds = new StringBuilder();

            int x = CheckSupervisor.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (CheckSupervisor.Items[i].Selected)
                {
                    clientIds.Append(CheckSupervisor.Items[i].Value + ",");
                }
            }

            if (clientIds.ToString() != "")
            {

                clientIds = clientIds.Remove(Convert.ToInt32(clientIds.Length) - 1, 1);
            }
            return clientIds.ToString();

        }
        catch { throw; }
    }
  
    protected void chkLocationAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkLocationAll.Checked == true)
            {
                chkLocations.Enabled = false;

                foreach (ListItem item in chkLocations.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {

                chkLocations.Enabled = true;

                foreach (ListItem item in chkLocations.Items)
                {
                    item.Selected = false;
                }
            }
            Locaionusers();
            locationsupervisors();
        }
        catch { throw; }
    }

    private string GetCheckBoxListUserIds()
    {
        try
        {

            StringBuilder clientIds = null;
            clientIds = new StringBuilder();

            int x = chkUsers.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkUsers.Items[i].Selected)
                {
                    clientIds.Append(chkUsers.Items[i].Value + ",");
                }
            }

            if (clientIds.ToString() != "")
            {

                clientIds = clientIds.Remove(Convert.ToInt32(clientIds.Length) - 1, 1);
            }
            return clientIds.ToString();

        }
        catch { throw; }
    }

    private int[] RetrievetUserIds(string getClientId)
    {
        try
        {
            string strClientIdd = getClientId;
            string[] clientIds = strClientIdd.Split(',');
            int[] iClientIds = new int[clientIds.Length];

            for (int i = 0; i < iClientIds.Length; ++i)
            {
                iClientIds[i] = Int32.Parse(clientIds[i]);
            }

            return iClientIds;

        }
        catch { throw; }
    }

    private void Locaionusers()
    {
        int[] iClientIds;

        try
        {
            // Get Fileid
            // Retrieve clients.
            string getClientId = GetCheckBoxListLocationIds();

            if (getClientId != "")
            {

                // Create an instance.
                reportService = AppService.Create<IReportService>();
                reportService.AppManager = this.mAppManager;

                DataTable dtProjects = new DataTable();

                // Getting client ids.
                iClientIds = this.RetrieveLocationIds(getClientId);

                // Retrieve projects based on clients.
                dtProjects = reportService.RetrieveUsersByLocations(iClientIds);

                // Bind the conrol.
                chkUsers.DataSource = dtProjects;
                chkUsers.DataTextField = "UserName";
                chkUsers.DataValueField = "UserId";
                chkUsers.DataBind();
            }
            else
            {
                BuildUsers();
                
            }


        }
        catch { throw; }
    }

    private void clientchange()
    {
        int[] iClientIds;

        try
        {

            // Get Fileid
            // Retrieve clients.
            string getClientId = GetCheckBoxListClientIds();

            if (getClientId != "")
            {

                // Create an instance.
                reportService = AppService.Create<IReportService>();
                reportService.AppManager = this.mAppManager;

                DataTable dtProjects = new DataTable();

                // Getting client ids.
                iClientIds = this.RetrieveClientIds(getClientId);

                // Retrieve projects based on clients.
                dtProjects = reportService.RetrieveProjectByClients(iClientIds);

                chkProject.Visible = true;
                // Bind the conrol.
                chkProject.DataSource = dtProjects;
                chkProject.DataTextField = "ProjectName";
                chkProject.DataValueField = "ProjectId";
                chkProject.DataBind();
            }
            else
            {
                BuildProjects();
            }

        }
        catch { throw; }
    }

    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (CheckSupervisorAll.Checked == true)
            {
                CheckSupervisor.Enabled = false;

                foreach (ListItem item in CheckSupervisor.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {

                CheckSupervisor.Enabled = true;

                foreach (ListItem item in CheckSupervisor.Items)
                {
                    item.Selected = false;
                }
            }
            BuildUsersforSupervisor();
        }
        catch { throw; }
    }
    protected void CheckSupervisor_SelectedIndexChanged(object sender, EventArgs e)
    {
        BuildUsersforSupervisor();
    }
}