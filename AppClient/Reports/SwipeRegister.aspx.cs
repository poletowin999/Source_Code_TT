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


public partial class Reports_SwipeRegister : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;

            LoadLabels();

            chkLocations.Enabled = false;
            chkLocations.Enabled = false;

            if (mAppManager.LoginUser.LocationId.ToString() == "1")
            {
                if (!Page.IsPostBack)
                {

                    // Retrieve client whether available in project.
                    BuildLocations();
                    // Retrieve the projects.
                    //BuildUsers();
                    Locationusers();

                    this.DisplayMessage(string.Empty, false);

                    //divResult.Style.Add("display", "none");
                    //divResult.Visible = false;

                    ClearReport();
                }
            }

        }
        catch { throw; }
    }

    public void LoadLabels()
    {

        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        this.mAppManager = Master.AppManager;
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = mAppManager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(Master.AppManager.LoginUser.Id, "REPORTS");

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

    private void BuildLocations()
    {
        try
        {


            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtClients = new DataTable();

            dtClients = reportService.RetrieveLocationsInUsers();
            DataTable dt = new DataTable();
            DataRow[] dr = dtClients.Select("LocationId=1");
            //dtClients = new DataTable();
            dt = dtClients.Clone();
            foreach (DataRow thisRow in dr)
            {
                dt.Rows.Add(thisRow.ItemArray);
            }
            dtClients = dt;

            if (dtClients.Rows.Count > 0)
            {

                chkLocations.DataSource = dtClients;
                chkLocations.DataTextField = "LocationName";
                chkLocations.DataValueField = "LocationId";
                chkLocations.DataBind();
                chkLocations.Items[0].Selected = true;
                chkLocationAll.Checked = true;
                chkLocations.Enabled = false;
                chkLocations.Enabled = false;
            }


        }
        catch { throw; }
    }


    private void BuildUsers()
    {
        try
        {

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
            ReportViewer1.Visible = false;
            _strpath = @"Reports/SwipeRegister.rdlc";
            // _strpath = @"Reports/UserCheckInOutDetail.rdlc";
            _strDatatablename = "DtSwiperegister";

            if ((txtActivityFromDate.Text.Trim().Length == 8) && (txtActivityFromDate.Text.IndexOf("/") == -1))
            {
                txtActivityFromDate.Text = txtActivityFromDate.Text.Insert(2, "/");
                txtActivityFromDate.Text = txtActivityFromDate.Text.Insert(5, "/");
            }

            if ((txtActivityToDate.Text.Trim().Length == 8) && (txtActivityToDate.Text.IndexOf("/") == -1))
            {
                txtActivityToDate.Text = txtActivityToDate.Text.Insert(2, "/");
                txtActivityToDate.Text = txtActivityToDate.Text.Insert(5, "/");
            }


            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);

            if (txtActivityFromDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_FROMDATE", FROMDATE);

            if (txtActivityToDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_TODATE", TODATE);

            DateTime date;
            if (txtActivityFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtActivityFromDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_FROMDATE", INVALIDE_DATE);
            }

            if (txtActivityToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtActivityToDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_TODATE", INVALIDE_DATE);
            }

            if ((txtActivityFromDate.Text != string.Empty) && (txtActivityToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtActivityFromDate.Text, out date)) == true && (DateTime.TryParse(txtActivityToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtActivityFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtActivityToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("ACIVITY_SUMMARY_FROMDATETODATE", DATE_RANGE);
                    }

                }

            }

            //divResult.Style.Add("display", "none");
            //divResult.Visible = false;

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
            int[] iUserids = new int[0];
            string getUserids;

            getUserids = GetCheckBoxListUserIds();
            // Retrieve projects.
            if (getUserids != "")
                iUserids = this.RetrievetUserIds(getUserids);



            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtAcivitySummary = new DataTable();
            // Retrieve activity summary.
            //dtAcivitySummary = reportService.RetrieveSwipeDetails(mAppManager.LoginUser.Id, DateTime.Parse(txtActivityFromDate.Text), DateTime.Parse(txtActivityToDate.Text), iClientIds, iUserids);

            SqlConnection myconn;
            

            string cnstr = ConfigurationManager.ConnectionStrings["DEFAULT_DATABASE_CONNECTION_STRING"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cnstr);
            
            string Password;
            string txtPassword;
            txtPassword = "";
            char txtPassword1;
            int lenPassword = builder.Password.Length;
            for (int lengthpwd = 0; lengthpwd < lenPassword; )
            {
                Password = builder.Password.Substring(lengthpwd, 4);
                txtPassword1 = Convert.ToChar(Convert.ToInt32(Password) - 1000);
                char chargthpwd = (char)txtPassword1;
                txtPassword = txtPassword + chargthpwd.ToString();
                lengthpwd = lengthpwd + 4;
            }
            builder.Password = txtPassword;

            string DBIP;
            string texttxtDBIP;
            texttxtDBIP = "";
            char DBIP1;
            int lenDBIP = builder.DataSource.Length;
            for (int lengthDBIP = 0; lengthDBIP < lenDBIP; )
            {
                DBIP = builder.DataSource.Substring(lengthDBIP, 4);
                DBIP1 = Convert.ToChar(Convert.ToInt32(DBIP) - 1000);
                char characterDBIP = (char)DBIP1;
                texttxtDBIP = texttxtDBIP + characterDBIP.ToString();
                lengthDBIP = lengthDBIP + 4;
            }
            builder.DataSource = texttxtDBIP;

            string server;
            string txtuserid = builder.UserID;
            string textuserid;
            textuserid = "";
            char server1;
            int len = builder.UserID.Length;
            for (int length = 0; length < len; )
            {
                server = builder.UserID.Substring(length, 4);
                server1 = Convert.ToChar(Convert.ToInt32(server) - 1000);
                char character = (char)server1;
                textuserid = textuserid + character.ToString();
                length = length + 4;
            }
            builder.UserID = textuserid;
            myconn = new SqlConnection(builder.ToString());
            myconn.Open();
            SqlCommand cmd = new SqlCommand("RetrieveSwipeDetails", myconn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@loginUserId", SqlDbType.Int).Value = mAppManager.LoginUser.Id;
            cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = DateTime.Parse(txtActivityFromDate.Text);
            cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = DateTime.Parse(txtActivityToDate.Text);
            cmd.Parameters.Add("@Data", SqlDbType.Xml).Value = BuildXmlforPayrollLocationUsers1(iClientIds, iUserids);
            cmd.CommandTimeout = 0;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ReportViewer1.Visible = true;
            ReportViewer2.Visible = false;
            ReportViewer1.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            divReport.Visible = true;
            divData.Visible = false;
            ReportViewer1.Height = Unit.Parse("350px");

            ReportViewer1.Width = Unit.Parse("100%");
            ReportViewer1.BorderColor = System.Drawing.Color.Black;
            ReportViewer1.BorderWidth = Unit.Parse("1px");
            ReportViewer1.BackColor = System.Drawing.Color.LightSkyBlue;

            ReportDataSource rds = new ReportDataSource(_strDatatablename, dt);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(rds);
            ReportViewer1.LocalReport.Refresh();

         

            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;

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

               // ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);
                // ReportDataSource rds = new ReportDataSource("UserCheckInOutDetailDataset", dtAcivitySummary);
                ReportParameter rp1 = new ReportParameter("Fromdate", txtActivityFromDate.Text);
                ReportParameter rp2 = new ReportParameter("Todate", txtActivityToDate.Text);

                ReportParameter rp3 = new ReportParameter("lblName", lblName.Text);
                ReportParameter rp4 = new ReportParameter("lblEmpNo", lblEmpNo.Text);
                ReportParameter rp5 = new ReportParameter("lblFromDate", lblFromDate.Text);
                ReportParameter rp6 = new ReportParameter("lblToDate", lblToDate.Text);
                ReportParameter rp7 = new ReportParameter("lblSwipeDetails", lblSwipeDetails.Text);

                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp2 });

                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp3 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp4 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp5 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp6 });
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp7 });

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.Refresh();
                             
          //      ReportViewer1.LocalReport.Refresh();
           //     ReportViewer1.DataBind();
                //ReportViewer1.LocalReport.Refresh();
                    
                // divReport.Controls.Add(ReportViewer1);

            }
            else
            {
                ClearReport();

                ReportViewer1.LocalReport.Refresh();

                divReport.Visible = false;

                divData.Style.Add("display", "block");
                divData.Visible = true;
                ReportViewer1.Visible = false;

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

    protected void btnDetailReport_Click(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.Visible = false;
            ReportViewer2.Visible = true;
            _strpath = @"Reports/SwipeRegister_bk25MAR14.rdlc";
            // _strpath = @"Reports/UserCheckInOutDetail.rdlc";
            _strDatatablename = "DtSwiperegister1";

            if ((txtActivityFromDate.Text.Trim().Length == 8) && (txtActivityFromDate.Text.IndexOf("/") == -1))
            {
                txtActivityFromDate.Text = txtActivityFromDate.Text.Insert(2, "/");
                txtActivityFromDate.Text = txtActivityFromDate.Text.Insert(5, "/");
            }

            if ((txtActivityToDate.Text.Trim().Length == 8) && (txtActivityToDate.Text.IndexOf("/") == -1))
            {
                txtActivityToDate.Text = txtActivityToDate.Text.Insert(2, "/");
                txtActivityToDate.Text = txtActivityToDate.Text.Insert(5, "/");
            }


            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);

            if (txtActivityFromDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_FROMDATE", FROMDATE);

            if (txtActivityToDate.Text == "")
                exception.Data.Add("ACIVITY_SUMMARY_TODATE", TODATE);

            DateTime date;
            if (txtActivityFromDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtActivityFromDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_FROMDATE", INVALIDE_DATE);
            }

            if (txtActivityToDate.Text != string.Empty)
            {
                if ((DateTime.TryParse(txtActivityToDate.Text, out date)) == false)
                    exception.Data.Add("ACTIVITY_INVALID_TODATE", INVALIDE_DATE);
            }

            if ((txtActivityFromDate.Text != string.Empty) && (txtActivityToDate.Text != string.Empty))
            {
                if ((DateTime.TryParse(txtActivityFromDate.Text, out date)) == true && (DateTime.TryParse(txtActivityToDate.Text, out date)) == true)
                {
                    DateTime fromDate = DateTime.Parse(txtActivityFromDate.Text);
                    DateTime toDate = DateTime.Parse(txtActivityToDate.Text);

                    if (DateTime.Compare(fromDate, toDate) > 0)
                    {
                        exception.Data.Add("ACIVITY_SUMMARY_FROMDATETODATE", DATE_RANGE);
                    }

                }

            }

            //divResult.Style.Add("display", "none");
            //divResult.Visible = false;

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
            int[] iUserids = new int[0];
            string getUserids;

            getUserids = GetCheckBoxListUserIds();
            // Retrieve projects.
            if (getUserids != "")
                iUserids = this.RetrievetUserIds(getUserids);



            reportService = AppService.Create<IReportService>();
            reportService.AppManager = this.mAppManager;

            DataTable dtAcivitySummary = new DataTable();
            // Retrieve activity summary.
            dtAcivitySummary = reportService.RetrieveSwipeDetails1(mAppManager.LoginUser.Id, DateTime.Parse(txtActivityFromDate.Text), DateTime.Parse(txtActivityToDate.Text), iClientIds, iUserids);


            ReportViewer2.Visible = true;
            ReportViewer2.ProcessingMode = ProcessingMode.Local;
            ReportViewer2.AsyncRendering = true;
            ReportViewer2.LocalReport.ReportPath = this.Request.PhysicalApplicationPath + _strpath;
            ReportViewer2.LocalReport.Refresh();




            // Check the condition whether datatable is value or not.
            if (dtAcivitySummary != null)
            {
                divReport.Visible = true;
                divData.Visible = false;

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

                ReportDataSource rds = new ReportDataSource(_strDatatablename, dtAcivitySummary);
                // ReportDataSource rds = new ReportDataSource("UserCheckInOutDetailDataset", dtAcivitySummary);
                ReportParameter rp1 = new ReportParameter("Fromdate", txtActivityFromDate.Text);
                ReportParameter rp2 = new ReportParameter("Todate", txtActivityToDate.Text);


                
                ReportParameter rp3 = new ReportParameter("lblName", lblName.Text);
                ReportParameter rp4 = new ReportParameter("lblEmpNo", lblEmpNo.Text);
                ReportParameter rp5 = new ReportParameter("lblFromDate", lblFromDate.Text);
                ReportParameter rp6 = new ReportParameter("lblToDate", lblToDate.Text);
                ReportParameter rp7 = new ReportParameter("lblSwipeDetails", lblSwipeDetails.Text);
                ReportParameter rp8 = new ReportParameter("lblDepartment", lblDepartment.Text);
                ReportParameter rp9 = new ReportParameter("lblENTRYTHROUGH", lblENTRYTHROUGH.Text);
                ReportParameter rp10 = new ReportParameter("lblENTRYEXITTIME", lblENTRYEXITTIME.Text);
                ReportParameter rp11 = new ReportParameter("lblCARDEVENTDATE", lblCARDEVENTDATE.Text);

                
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

                ReportViewer2.LocalReport.DataSources.Clear();
                ReportViewer2.LocalReport.DataSources.Add(rds);
                ReportViewer2.LocalReport.Refresh();

                //      ReportViewer2.LocalReport.Refresh();
                //     ReportViewer2.DataBind();
                //ReportViewer2.LocalReport.Refresh();

                // divReport.Controls.Add(ReportViewer2);

            }
            else
            {
                ClearReport();

                ReportViewer2.LocalReport.Refresh();

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
            Response.Redirect(@"~/Reports/View");
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

                //chkUsers.Enabled = true;

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
            string getClientId = "1"; GetCheckBoxListLocationIds();

            if (getClientId != "")
            {

                // Create an instance.
                reportService = AppService.Create<IReportService>();
                reportService.AppManager = this.mAppManager;

                DataTable dtProjects = new DataTable();

                // Getting client ids.
                iClientIds = this.RetrieveLocationIds(getClientId);

                // Retrieve projects based on clients.
                dtProjects = reportService.RetrieveUsersByLocations1(iClientIds);

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