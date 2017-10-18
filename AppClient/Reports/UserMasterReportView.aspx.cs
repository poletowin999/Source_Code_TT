using System;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using System.Collections;
using Microsoft.Reporting.WebForms;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Reports_UserMasterReportView : System.Web.UI.Page
{
    #region Class Variables

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
    #region UserDefinedmethods
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
            divReport.Visible = false;
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
    #endregion
    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;

            LoadLabels();
            if (!Page.IsPostBack)
            {
                this.DisplayMessage(string.Empty, false);

                BuildLocations();
                RdnAlldate.Checked = true;
                RdnAll.Checked = true;
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

    protected void btnView_Click1(object sender, EventArgs e)
    {
        try
        {
            string datefilter = "", userfilter = "", fromdate = "", todate = "";
            int loginUser = mAppManager.LoginUser.Id;


           
            _strDatatablename = "DataSet1_UserMasterReport";
            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);
            if (RdnJoinDate.Checked)
            {
                if (txtFromDate.Text == "")
                    exception.Data.Add("USER_MASTER_FROMDATE", FROMDATE);

                if (txtToDate.Text == "")
                    exception.Data.Add("USER_MASTER_TODATE", TODATE);
            }
            else if (RdnRelieveDate.Checked)
            {
                if (txtFromDate.Text == "")
                    exception.Data.Add("USER_MASTER_FROMDATE", FROMDATE);

                if (txtToDate.Text == "")
                    exception.Data.Add("USER_MASTER_TODATE", TODATE);
            }

            DateTime date;
            if (txtFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == false)
                    exception.Data.Add("USER_INVALID_FROMDATE", INVALIDE_DATE);
            }

            if (txtToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtToDate.Text, out date)) == false)
                    exception.Data.Add("USER_INVALID_TODATE", INVALIDE_DATE);
            }

            if ((txtFromDate.Text != string.Empty) && (txtToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtFromDate.Text, out date)) == true && (DateTime.TryParse(txtToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("USER_MASTER_FROMDATETODATE", "Fromdate should not be greater than todate.");
                    }

                }

            }
            if (RdnJoinDate.Checked)
            {
                datefilter = "JD";
                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
                
            }
            else if (RdnRelieveDate.Checked)
            {
                datefilter = "RD";
                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
               
            }
            else if (RdnAlldate.Checked)
            {
                datefilter = "All";
                fromdate = System.DateTime.Now.ToString(); //Dummy value
                todate = System.DateTime.Now.ToString(); //Dummy value
              
            }
            if (RdnAll.Checked)
            {
                userfilter = "2";
                _strpath = @"Reports/UserMasterReport.rdlc";
            }
            else if (RdnInActive.Checked)
            {
                userfilter = "0";
                _strpath = @"Reports/Attrition.rdlc";
            }
            else if (RdnActive.Checked)
            {
                userfilter = "1";
                _strpath = @"Reports/Manpower.rdlc";
            }

            // Retrieve LocationIds.
            int[] iLocationIds = new int[0];
            string getLocationId = "";

            getLocationId = GetCheckBoxListLocationIds();
            if (getLocationId != "")
                iLocationIds = this.RetrieveLocationIds(getLocationId);


            this.DisplayMessage(string.Empty, false);

            if (exception.Data.Count > 0)
                throw exception;
            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable UserMasterInfo = new DataTable();
            // Retrieve UserData.
            UserMasterInfo = reportService.RetrieveUserInfo(DateTime.Parse(fromdate), DateTime.Parse(todate), iLocationIds, datefilter, userfilter,loginUser);
            
            // Bind into report control.
           
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer1.LocalReport.Refresh();


            // Check the condition whether datatable is value or not.
            if (UserMasterInfo != null)
            {
                divReport.Visible = true;
                divData.Visible = false;

                ReportViewer1.Height = Unit.Parse("300px");

                ReportViewer1.Width = Unit.Parse("100%");
                ReportViewer1.BorderColor = System.Drawing.Color.Black;
                ReportViewer1.BorderWidth = Unit.Parse("1px");
                ReportViewer1.BackColor = System.Drawing.Color.LightSkyBlue;

                ReportDataSource rds = new ReportDataSource(_strDatatablename, UserMasterInfo);               

                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("EmployeeMasterReport", lblEmployeeMasterReport.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("EmpNo", lblEmpNo.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Employee", lblEmployeeName.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Rolename", lblRolename.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Locations", lblLocations.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("WorkType", lblWorktype.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Gender", lblGender.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("EmailId", lblEmailId.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Supervisor", lblSupervisor.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("HomePhone", lblHomePhone.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("OfficePhone", lblOfficePhone.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Extension", lblExtension.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("IsAdminuser", lblIsAdminuser.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("IsAutoapproval", lblIsAutoapproval.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("JoinDate", lblJoinDate.Text + " (MM/DD/YYYY)") });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("RelieveDate", lblRelieveDate.Text + " (MM/DD/YYYY)") });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Tenority", lblTenority.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Status", lblStatus.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("Reason", lblreason.Text) });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { new ReportParameter("ContractDays", lblContractDays.Text) });


                               

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();

               }
            else
            {
               

                ReportViewer1.LocalReport.Refresh();

                divReport.Visible = false;

                divData.Style.Add("display", "block");
                divData.Visible = true;

            }
        }
        catch (ValidationException ve)
        {

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
   
    
    #endregion
}