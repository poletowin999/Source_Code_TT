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
using System.Collections;
using Microsoft.Reporting.WebForms;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Reports_ActivityNotEnteredReportView : System.Web.UI.Page
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
        
        try
        {
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;
            LoadLabels();
            if (!Page.IsPostBack)
            {
                this.DisplayMessage(string.Empty, false);

                ClearReport();
                // Retrieve client whether available in project.
                BuildLocations();
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


    protected void btnView_Click(object sender, EventArgs e)
    {

        try
        {



            _strpath = @"Reports/ActivityNotEnteredReport.rdlc";
            _strDatatablename = "DataSet1_UserNotEnteredActivityForDesign";


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


            // Retrieve Userid.
            int[] iUserIds = new int[0];
            string getUserId;

            getUserId = GetCheckBoxListUserIds();
            // Retrieve Userid.
            if (getUserId != "")
                iUserIds = this.RetrievetUserIds(getUserId);



            // Retrieve LocationIds.
            int[] iLocationIds = new int[0];
            string getLocationId = "";

            getLocationId = GetCheckBoxListLocationIds();
            if (getLocationId != "")
                iLocationIds = this.RetrieveLocationIds(getLocationId);
                 

            this.DisplayMessage(string.Empty, false);

            if (exception.Data.Count > 0)
                throw exception;

            int ddvalue;
            ddvalue = Convert.ToInt32(DDreptype.SelectedValue);
            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtAcivitySummary = new DataTable();
            // Retrieve activity summary.
            dtAcivitySummary = reportService.RetrieveUserNotEnteredAcitvity(DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text), mAppManager.LoginUser.Id,iUserIds,iLocationIds,ddvalue);

            //if (dtAcivitySummary.Rows[0]["FirstName"].ToString() == string.Empty && dtAcivitySummary.Rows[0]["LastName"].ToString() == string.Empty)
            //    dtAcivitySummary.Rows[0]["LastName"] = "Data's not found.";

            // Bind into report control.
            // ReportViewer ReportViewer1 = new ReportViewer();
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer1.LocalReport.Refresh();
                     

            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;

                ReportViewer1.Height = Unit.Parse("350px");

                ReportViewer1.Width = Unit.Parse("100%");
                ReportViewer1.BorderColor = System.Drawing.Color.Black;
                ReportViewer1.BorderWidth = Unit.Parse("1px");
                ReportViewer1.BackColor = System.Drawing.Color.LightSkyBlue;

                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);


                ReportParameter rp1 = new ReportParameter("EmployeeId", lblEmpNo.Text);
                ReportParameter rp2 = new ReportParameter("FirstName", lblFirstName.Text);
                ReportParameter rp3 = new ReportParameter("LastName", lblLastName.Text);
                ReportParameter rp4 = new ReportParameter("UserRole", lblUserRole.Text);
                ReportParameter rp5 = new ReportParameter("Department", lblDepartment.Text);
                ReportParameter rp6 = new ReportParameter("ActivityDate", lblActivityDate.Text);
                ReportParameter rp7 = new ReportParameter("EmailId", lblEmailId.Text);
                ReportParameter rp8 = new ReportParameter("ContractDays", lblContractDays.Text);
                ReportParameter rp9 = new ReportParameter("lblFromDate", lblFromDate.Text);
                ReportParameter rp10 = new ReportParameter("lblToDate", lblToDate.Text);
                ReportParameter rp11 = new ReportParameter("ActivityNotEnteredReport", lblActivityNotEnteredReport.Text);


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

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();

                Session.Add("ActivityNotEnteredReportView_ActivitySummaryDataSource", rds);
                // divReport.Controls.Clear();
                // divReport.Controls.Add(ReportViewer1);
            }
            else
            {
                ClearReport();

                ReportViewer1.LocalReport.Refresh();

                divReport.Visible = false;

                divData.Style.Add("display", "block");
                divData.Visible = true;

            }


        }
        catch (ValidationException ve)
        {
            ClearReport();
            DisplayValiationMessage(ve);
            return;
        }

        catch { throw; }

    }



    #region Private Members


    private void ClearReport()
    {

        //divResult.Style.Add("display", "none");
        //divResult.Visible = false;
        //divResult.InnerText = string.Empty;

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

    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Reports/View");
        }
        catch { throw; }
    }

    private void BuildLocations()
    {
        try
        {
            
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

    protected void chkLocations_SelectedIndexChanged(object sender, EventArgs e)
    {

        locationusers();
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
            locationusers();
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

    private void BuildUsers()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtClients = new DataTable();

            //dtClients = reportService.RetrieveUsers();

            chkUsers.DataSource = dtClients;
            chkUsers.DataTextField = "UserName";
            chkUsers.DataValueField = "UserId";
            chkUsers.DataBind();


        }
        catch { throw; }
    }

    private void locationusers()
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
                    chkUsers.DataSource = dtProjects;
                }
                else
                {
                    chkUsers.DataSource = "";
                }
               
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


}