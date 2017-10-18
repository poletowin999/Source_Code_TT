using System;
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

public partial class Reports_PayrollReportView : System.Web.UI.Page
{

    #region Class Variables

    IAppManager mAppManager;
    IReportService reportService = null;

    string _strpath = null;
    string _strDatatablename = null;

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (this.IsPostBack)
            {
                // Post back.
                ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + "Reports/PayrollReport.rdlc";

                ReportDataSource rds = Session["ActivityPayrollReportView_ActivitySummaryDataSource"] as ReportDataSource;

                if (rds != null)
                {
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    // ReportViewer1.LocalReport.Refresh();
                }
            }
        }
        catch { throw; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //ValidationException ValidClass = new ValidationException();


        try
        {
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;

            if (!Page.IsPostBack)
            {

                // Retrieve client whether available in project.
                BuildLocations();
                // Retrieve the projects.
                BuildUsers();

                this.DisplayMessage(string.Empty, false);

                ClearReport();
            }

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


    protected void btnViewReport_Click(object sender, EventArgs e)
    {
        try
        {

            _strpath = @"Reports/PayrollReport.rdlc";
            _strDatatablename = "DataSet1_UserSwipeByMonthForDesign";


            // Create an instance for exception.
            //ValidationException exception = new ValidationException("Validation error(s) occurred.");
            ValidationException exception = new ValidationException("");

            if (txtActivityFromDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_FROMDATE", "Fromdate should not empty.");

            if (txtActivityToDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_TODATE", "Todate should not empty.");

            DateTime date;
            if (txtActivityFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtActivityFromDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_FROMDATE", "Invalid fromdate.");
            }

            if (txtActivityToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtActivityToDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_TODATE", "Invalid todate.");
            }

            if ((txtActivityFromDate.Text != string.Empty) && (txtActivityToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtActivityFromDate.Text, out date)) == true && (DateTime.TryParse(txtActivityToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtActivityFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtActivityToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("ACIVITY_SUMMARY_FROMDATETODATE", "Fromdate should not be greater than todate.");
                    }

                }

            }


            this.DisplayMessage(string.Empty, false);

            if (exception.Data.Count > 0)
                throw exception;

            // Retrieve clientids.
            int[] iClientIds = new int[0];
            string getClientId;

            getClientId = GetCheckBoxListLocationIds();
            if (getClientId != "")
                iClientIds = this.RetrieveLocationIds(getClientId);




            // Retrieve projectids.
            int[] iProjectIds = new int[0];
            string getProjectId;

            getProjectId = GetCheckBoxListUserIds();
            // Retrieve projects.
            if (getProjectId != "")
                iProjectIds = this.RetrievetUserIds(getProjectId);



            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtAcivitySummary = new DataTable();
            // Retrieve activity summary.
            dtAcivitySummary = reportService.RetrieveParollUsers(mAppManager.LoginUser.Id, DateTime.Parse(txtActivityFromDate.Text), DateTime.Parse(txtActivityToDate.Text), iClientIds, iProjectIds);

          
            // Bind into report control.
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer1.LocalReport.Refresh();

            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;

                ReportViewer1.Height = Unit.Parse("490px");

                ReportViewer1.Width = Unit.Parse("100%");

                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();

                Session.Add("ActivityPayrollReportView_ActivitySummaryDataSource", rds);

                divReport.Controls.Add(ReportViewer1);

            }
            else
            {
                ClearReport();

                ReportViewer1.LocalReport.Refresh();

                divReport.Visible = false;

                divData.Style.Add("display", "block");
                divData.Visible = true;

                //exception.Data.Add("PROJECT_USERNOTENTERED_ACTIVITY", "Data not exists.");
                //throw exception;
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


    private void ClearReport()
    {

      
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

}