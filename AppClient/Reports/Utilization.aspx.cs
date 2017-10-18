using System;
using System.Collections.Generic;
using System.Configuration;
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
using Microsoft.Reporting.WebForms;
using System.Collections;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Reports_UtilizationReportView : System.Web.UI.Page
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
                // Display the projects.
                BuildProjects();
                // Retrieve client whether available in project.
                BuildClients();

                // Display the billtypes.
                BuildBillTypes();

                // Display the worktypes.
                BuildWorkTypes();

                // Retrieve client whether available in project.
                BuildLocations();

                // Retrieve the projects.
                BuildUsers();

                this.DisplayMessage(string.Empty, false);

                ClearReportSummary();



            }

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


    private void BuildProjects()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtProjects = new DataTable();

            dtProjects = reportService.RetrieveProjects();

            chkProjects.DataSource = dtProjects;
            chkProjects.DataTextField = "ProjectName";
            chkProjects.DataValueField = "ProjectId";
            chkProjects.DataBind();

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


    private void BuildBillTypes()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtProjects = new DataTable();

            dtProjects = reportService.RetrieveBillTypes();

            chkBillTypes.DataSource = dtProjects;
            chkBillTypes.DataTextField = "BillingName";
            chkBillTypes.DataValueField = "BillingTypeId";
            chkBillTypes.DataBind();

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
            DataTable dt = new DataTable();
            DataColumn WorkTypeId = new DataColumn("WorkTypeId", typeof(System.Int32));
            dt.Columns.Add(WorkTypeId);
            DataColumn WorkTypeName = new DataColumn("WorkTypeName", typeof(System.String));
            dt.Columns.Add(WorkTypeName);
            DataColumn ConsiderForReport = new DataColumn("ConsiderForReport", typeof(System.Boolean));
            dt.Columns.Add(ConsiderForReport);

            dtProjects = reportService.RetrieveWorkTypes();
            DataRow[] dr = dtProjects.Select("ConsiderForReport<>" + false);
            dtProjects = new DataTable();
            foreach (DataRow thisRow in dr)
            {
                dt.Rows.Add(thisRow.ItemArray);
            }
            dtProjects = dt;
            chkWorkTypes.DataSource = dtProjects;
            chkWorkTypes.DataTextField = "WorkTypeName";
            chkWorkTypes.DataValueField = "WorkTypeId";
            chkWorkTypes.DataBind();

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


 


    protected void btnView_Click(object sender, EventArgs e)
    {

        try
        {
            _strpath = @"Reports/Utilizationreport.rdlc";
            _strDatatablename = "Utilization";

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

            if ((txtToDate.Text.Trim().Length == 8) && (txtToDate.Text.IndexOf("/") == -1))
            {
                txtToDate.Text = txtToDate.Text.Insert(2, "/");
                txtToDate.Text = txtToDate.Text.Insert(5, "/");
            }

            this.DisplayMessage(string.Empty, false);

            if (exception.Data.Count > 0)
                throw exception;


            int[] iProjectIds = new int[0];
            string getProjectId;

            if (chkProjectAll.Checked == false)
            {
                getProjectId = GetCheckBoxListProjects();
                // Retrieve projects.
                if (getProjectId != "")
                    iProjectIds = this.RetrieveProjectIds(getProjectId);
            }
            else
            {
                getProjectId = "0";
                iProjectIds = new int[0];
            }

            // Retrieve billtypes.
            int[] iBillTypeIds = new int[0];
            string getBillTypeId;

            if (chkBillTypesAll.Checked == false)
            {
                getBillTypeId = GetCheckBoxListBillTypeIds();
                if (getBillTypeId != "")
                    iBillTypeIds = this.RetrieveProjectIds(getBillTypeId);
            }
            else
            {
                getBillTypeId = "0";
                iBillTypeIds = new int[0];

            }

            // Retrieve worktypes.
            int[] iWorkTypeIds = new int[0];
            string getWorkTypeId;

            if (chkWorkTypesAll.Checked == false)
            {
                getWorkTypeId = GetCheckBoxListWorkTypeIds();
                if (getWorkTypeId != "")
                    iWorkTypeIds = this.RetrieveProjectIds(getWorkTypeId);
            }
            else
            {
                getWorkTypeId = "0";
                iWorkTypeIds = new int[0];
            }


            // Retrieve clientids.
            int[] iClientIds = new int[0];
            string getClientId;

            getClientId = GetCheckBoxListLocationIds();
            if (getClientId != "")
                iClientIds = this.RetrieveLocationIds(getClientId);


            // Retrieve clientids.
            int[] iprojectClientIds = new int[0];
            string getprojectClientId;

            if (chkClientAll.Checked == false)
            {
                getprojectClientId = GetCheckBoxListClientIds();
                if (getprojectClientId != "")
                    iprojectClientIds = this.RetrieveClientIds(getprojectClientId);
            }
            else
            {
                getprojectClientId = "0";
                iprojectClientIds = new int[0];

            }




            // Retrieve projectids.
            int[] iUserIds = new int[0];
            string getUserId;

            getUserId = GetCheckBoxListUserIds();
            // Retrieve projects.
            if (getUserId != "")
                iUserIds = this.RetrievetUserIds(getUserId);




            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;


            DataTable dtAcivitySummary = new DataTable();

            // Retrieve activity summary.
            dtAcivitySummary = reportService.RetrieveUtilization(mAppManager.LoginUser.Id,1, txtFromDate.Text, txtToDate.Text, iProjectIds, iBillTypeIds, iWorkTypeIds, iClientIds, iUserIds, iprojectClientIds);

         
            // Bind into report control.
            ReportViewer1.Visible = true;
            ReportViewer2.Visible = false;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer1.LocalReport.Refresh();

             // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;


                // Bind into report control.
                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
                ReportViewer1.LocalReport.Refresh();
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

                if ((txtFromDate.Text.Trim().Length == 8) && (txtFromDate.Text.IndexOf("/") == -1))
                {
                    txtFromDate.Text = txtFromDate.Text.Insert(2, "/");
                    txtFromDate.Text = txtFromDate.Text.Insert(5, "/");
                }

                

                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);
                ReportParameter rp1 = new ReportParameter("Fromdate", txtFromDate.Text);
                ReportParameter rp2 = new ReportParameter("Todate", txtToDate.Text);

                ReportParameter rp3 = new ReportParameter("lblDesignation", lblDesignation.Text);
                ReportParameter rp4 = new ReportParameter("lblNonBillable", lblNonBillable.Text);
                ReportParameter rp5 = new ReportParameter("lblEmpNo", lblEmpNo.Text);
                ReportParameter rp6 = new ReportParameter("lblName", lblName.Text);
                ReportParameter rp8 = new ReportParameter("lblContractDays", lblContractDays.Text);
                ReportParameter rp9 = new ReportParameter("lblBillable", lblBillable.Text);
                ReportParameter rp10 = new ReportParameter("lblworkdate", lblworkdate.Text);
                ReportParameter rp11 = new ReportParameter("lblTotalhours", lblTotalhours.Text);
                ReportParameter rp12 = new ReportParameter("lblFromdate", lblFromDate.Text);
                ReportParameter rp13 = new ReportParameter("lblTodate", lblToDate.Text);
                ReportParameter rp14 = new ReportParameter("lblUtilizationReportDetail", lblUtilizationReportDetail.Text);


                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp2 });

                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp3 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp4 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp5 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp6 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp8 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp9 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp10 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp11 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp12 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp13 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp14 });

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                
                ReportViewer1.LocalReport.Refresh();
               
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

    protected void btnView_Click_detail(object sender, EventArgs e)
    {

        try
        {
            _strpath = @"Reports/Utilizationdetailreport.rdlc";
            _strDatatablename = "Utilization1";

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

            if ((txtToDate.Text.Trim().Length == 8) && (txtToDate.Text.IndexOf("/") == -1))
            {
                txtToDate.Text = txtToDate.Text.Insert(2, "/");
                txtToDate.Text = txtToDate.Text.Insert(5, "/");
            }

            this.DisplayMessage(string.Empty, false);

            if (exception.Data.Count > 0)
                throw exception;


            int[] iProjectIds = new int[0];
            string getProjectId;

            if (chkProjectAll.Checked == false)
            {
                getProjectId = GetCheckBoxListProjects();
                // Retrieve projects.
                if (getProjectId != "")
                    iProjectIds = this.RetrieveProjectIds(getProjectId);
            }
            else
            {
                getProjectId = "0";
                iProjectIds = new int[0];
            }

            // Retrieve billtypes.
            int[] iBillTypeIds = new int[0];
            string getBillTypeId;

            if (chkBillTypesAll.Checked == false)
            {
                getBillTypeId = GetCheckBoxListBillTypeIds();
                if (getBillTypeId != "")
                    iBillTypeIds = this.RetrieveProjectIds(getBillTypeId);
            }
            else
            {
                getBillTypeId = "0";
                iBillTypeIds = new int[0];

            }

            // Retrieve worktypes.
            int[] iWorkTypeIds = new int[0];
            string getWorkTypeId;

            if (chkWorkTypesAll.Checked == false)
            {
                getWorkTypeId = GetCheckBoxListWorkTypeIds();
                if (getWorkTypeId != "")
                    iWorkTypeIds = this.RetrieveProjectIds(getWorkTypeId);
            }
            else
            {
                getWorkTypeId = "0";
                iWorkTypeIds = new int[0];
            }


            // Retrieve clientids.
            int[] iClientIds = new int[0];
            string getClientId;

            getClientId = GetCheckBoxListLocationIds();
            if (getClientId != "")
                iClientIds = this.RetrieveLocationIds(getClientId);


            // Retrieve clientids.
            int[] iprojectClientIds = new int[0];
            string getprojectClientId;

            if (chkClientAll.Checked == false)
            {
                getprojectClientId = GetCheckBoxListClientIds();
                if (getprojectClientId != "")
                    iprojectClientIds = this.RetrieveClientIds(getprojectClientId);
            }
            else
            {
                getprojectClientId = "0";
                iprojectClientIds = new int[0];

            }




            // Retrieve projectids.
            int[] iUserIds = new int[0];
            string getUserId;

            getUserId = GetCheckBoxListUserIds();
            // Retrieve projects.
            if (getUserId != "")
                iUserIds = this.RetrievetUserIds(getUserId);




            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;


            DataTable dtAcivitySummary = new DataTable();

            // Retrieve activity summary.
            dtAcivitySummary = reportService.RetrieveUtilizationDetail(mAppManager.LoginUser.Id,2, txtFromDate.Text, txtToDate.Text, iProjectIds, iBillTypeIds, iWorkTypeIds, iClientIds, iUserIds, iprojectClientIds);


            // Bind into report control.
            ReportViewer2.Visible = true;
            ReportViewer1.Visible = false;
            ReportViewer2.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer2.LocalReport.Refresh();

            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;


                // Bind into report control.
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

                if ((txtFromDate.Text.Trim().Length == 8) && (txtFromDate.Text.IndexOf("/") == -1))
                {
                    txtFromDate.Text = txtFromDate.Text.Insert(2, "/");
                    txtFromDate.Text = txtFromDate.Text.Insert(5, "/");
                }



                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);
                ReportParameter rp1 = new ReportParameter("Fromdate", txtFromDate.Text);
                ReportParameter rp2 = new ReportParameter("Todate", txtToDate.Text);

                ReportParameter rp3 = new ReportParameter("lblFromDate", lblFromDate.Text);
                ReportParameter rp4 = new ReportParameter("lblToDate", lblToDate.Text);
                ReportParameter rp5 = new ReportParameter("lblDescription", lblDescription.Text);
                ReportParameter rp6 = new ReportParameter("lblStudio", lblStudio.Text);
                ReportParameter rp7 = new ReportParameter("lblIncludingStudio", lblIncludingStudio.Text);
                ReportParameter rp8 = new ReportParameter("lblexcludingStudio", lblexcludingStudio.Text);
                ReportParameter rp9 = new ReportParameter("lblUtilizationReportDetail", lblUtilizationReportDetail.Text);


                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp2 });

                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp3 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp4 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp5 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp6 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp7 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp8 });
                this.ReportViewer2.LocalReport.SetParameters(new ReportParameter[] { rp9 });

                ReportViewer2.LocalReport.DataSources.Clear();
                ReportViewer2.LocalReport.DataSources.Add(rds);

                ReportViewer2.LocalReport.Refresh();

            }
            else
            {
                ClearReportSummary();

                ReportViewer2.LocalReport.Refresh();

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

    #region Private Members


    private void ClearReportSummary()
    {


        //ControlCollection coll = this.ReportViewer1.Parent.Controls;

        ////remember the place, there the old ReportViewer was

        //int oldIndex = coll.IndexOf(this.ReportViewer1);

        ////remove in from the page

        //coll.Remove(this.ReportViewer1);

        ////then add new control

        //ReportViewer1 = new Microsoft.Reporting.WebForms.ReportViewer();

        //ReportViewer1.Height = Unit.Parse("490px");

        //ReportViewer1.Width = Unit.Parse("100%");

        //coll.AddAt(oldIndex, ReportViewer1);

        //this.ReportViewer1.LocalReport.DataSources.Clear();

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




    private string GetCheckBoxListProjects()
    {
        try
        {

            StringBuilder projectIds = null;
            projectIds = new StringBuilder();

            int x = chkProjects.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkProjects.Items[i].Selected)
                {
                    projectIds.Append(chkProjects.Items[i].Value + ",");
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


    private string GetCheckBoxListBillTypeIds()
    {
        try
        {

            StringBuilder projectIds = null;
            projectIds = new StringBuilder();

            int x = chkBillTypes.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkBillTypes.Items[i].Selected)
                {
                    projectIds.Append(chkBillTypes.Items[i].Value + ",");
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


    private string GetCheckBoxListWorkTypeIds()
    {
        try
        {

            StringBuilder projectIds = null;
            projectIds = new StringBuilder();

            int x = chkWorkTypes.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkWorkTypes.Items[i].Selected)
                {
                    projectIds.Append(chkWorkTypes.Items[i].Value + ",");
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

    protected void chkProjectAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkProjectAll.Checked == true)
            {
                chkProjects.Enabled = false;

                foreach (ListItem item in chkProjects.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {

                chkProjects.Enabled = true;

                foreach (ListItem item in chkProjects.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch { throw; }

    }
    protected void chkBillTypesAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkBillTypesAll.Checked == true)
            {
                chkBillTypes.Enabled = false;

                foreach (ListItem item in chkBillTypes.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {
                chkBillTypes.Enabled = true;
                foreach (ListItem item in chkBillTypes.Items)
                {
                    item.Selected = false;
                }
            }
        }
        catch { throw; }

    }

    protected void chkWorkTypesAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkWorkTypesAll.Checked == true)
            {
                chkWorkTypes.Enabled = false;
                foreach (ListItem item in chkWorkTypes.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {
                chkWorkTypes.Enabled = true;
                foreach (ListItem item in chkWorkTypes.Items)
                {
                    item.Selected = false;
                }
            }
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
            Locationusers();
        }
        catch { throw; }
    }
    protected void chkUsersAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkUsersAll.Checked == true)
            {
                chkUsers.Enabled = false;

                foreach (ListItem item in chkUsers.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {

                chkUsers.Enabled = true;

                foreach (ListItem item in chkUsers.Items)
                {
                    item.Selected = false;
                }
            }
           
        }
        catch { throw; }

    }

    protected void chkLocations_SelectedIndexChanged(object sender, EventArgs e)
    {

        Locationusers();
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
            Clientchange();
        }
        catch { throw; }
    }

    protected void chkClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        Clientchange();

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

    private void Clientchange()
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

                chkProjects.Visible = true;
                // Bind the conrol.
                chkProjects.DataSource = dtProjects;
                chkProjects.DataTextField = "ProjectName";
                chkProjects.DataValueField = "ProjectId";
                chkProjects.DataBind();
            }
            else
            {
                BuildProjects();
            }

        }
        catch { throw; }
    }

    private void Locationusers()
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


}