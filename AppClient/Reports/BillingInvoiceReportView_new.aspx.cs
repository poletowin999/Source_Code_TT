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
using System.Collections;
using Microsoft.Reporting.WebForms;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Reports_BillingInvoiceReportView : System.Web.UI.Page
{
    #region Class Variables

    // private ValidationException ValidClass() ;

    IAppManager mAppManager;
    IReportService reportService = null;

    string _strpath = null;
    string _strDatatablename = null;

    #endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        //ValidationException ValidClass = new ValidationException();

        try
        {
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;

            if (!Page.IsPostBack)
            {
                // Display the projects.
                BuildProjects();

                // Display the languages.
                BuildLanguages();

                // Retrieve client whether available in project.
                BuildLocations();

                // Retrieve the projects.
                BuildUsers();

                this.DisplayMessage(string.Empty, false);

                //divResult.Style.Add("display", "none");
                //divResult.Visible = false;

                ClearReportSummary();



            }

        }
        catch { throw; }

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


    private void BuildLanguages()
    {
        try
        {
            //gvwClient.DataSource = this.RetrieveClients();
            //gvwClient.DataBind();

            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtProjects = new DataTable();

            dtProjects = reportService.RetrieveLanguages();

            chkLanguages.DataSource = dtProjects;
            chkLanguages.DataTextField = "LanguageName";
            chkLanguages.DataValueField = "LanguageId";
            chkLanguages.DataBind();

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
            _strpath = @"Reports/BillingInvoiceReport.rdlc";
            _strDatatablename = "DataSet1_RetrieveBillingActivityForDesign";

            // Retrieve clients.
            //string getClientId = GetClientIds();


            // Create an instance for exception.
            //ValidationException exception = new ValidationException("Validation error(s) occurred.");
            ValidationException exception = new ValidationException("");

            if (txtFromDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_FROMDATE", "Fromdate should not empty.");

            if (txtToDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_TODATE", "Todate should not empty.");

            DateTime date;
            if (txtFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_FROMDATE", "Invalid fromdate.");
            }

            if (txtToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtToDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_TODATE", "Invalid todate.");
            }


            if ((txtFromDate.Text != string.Empty) && (txtToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == true && (DateTime.TryParse(txtToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("ACIVITY_SUMMARY_FROMDATETODATE", "Fromdate should not be greater than todate.");
                    }

                }

            }

            //divResult.Style.Add("display", "none");
            //divResult.Visible = false;

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
            int[] iLanguageIds = new int[0];
            string getLanguageId;

            if (chkLanguageAll.Checked == false)
            {
                getLanguageId = GetCheckBoxListLanguagesIds();
                if (getLanguageId != "")
                    iLanguageIds = this.RetrieveProjectIds(getLanguageId);
            }
            else
            {
                getLanguageId = "0";
                iLanguageIds = new int[0];

            }


            // Retrieve clientids.
            int[] iClientIds = new int[0];
            string getClientId;

            getClientId = GetCheckBoxListLocationIds();
            if (getClientId != "")
                iClientIds = this.RetrieveLocationIds(getClientId);




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
            dtAcivitySummary = reportService.RetrieveBillingActivityNew(mAppManager.LoginUser.Id, txtFromDate.Text, txtToDate.Text, iProjectIds, iLanguageIds, iClientIds, iUserIds);

        

            // Bind into report control.
          //  ReportViewer ReportViewer1 = new ReportViewer();
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer1.LocalReport.Refresh();
                      
            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;

                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
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
               // divReport.Controls.Add(ReportViewer1);
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


    private string GetCheckBoxListLanguagesIds()
    {
        try
        {

            StringBuilder projectIds = null;
            projectIds = new StringBuilder();

            int x = chkLanguages.Items.Count;
            for (int i = 0; i < x; i++)
            {
                if (chkLanguages.Items[i].Selected)
                {
                    projectIds.Append(chkLanguages.Items[i].Value + ",");
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

    protected void chkLanguageAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkLanguageAll.Checked == true)
            {
                chkLanguages.Enabled = false;

                foreach (ListItem item in chkLanguages.Items)
                {
                    item.Selected = true;
                }

            }

            else
            {
                chkLanguages.Enabled = true;
                foreach (ListItem item in chkLanguages.Items)
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