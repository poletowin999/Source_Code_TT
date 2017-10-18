using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Text;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Activities_ActivityResetView : System.Web.UI.Page
{
    const string ACTIVITY_SUMMARY = "ACTIVITY_RESET_VIEW_ACTIVITY_SUMMARY";

    IAppManager mAppManager = null;
    List<CustomEntity> mActivitySummary = null;


    #region Class Function

    string Searchcriteria;
    string NoRecfound;
    string LBLACTIVITYENTEREDUSR;
    string LBLACTIVITYFRDT;
    string LBLACTIVITYTODT;
    string LBLACTTORESET;
    string LBLCMTEMTY;
    string LBLListofAct;
    string LBLFound;

    private void FillActivitySummary()
    {
        try
        {
            // Validation.
            this.ValidateEntity();

            // Fetch user values.
            int userId = Int32.Parse(hdnCreateUserId.Value);
            DateTime activityFromDate = DateTime.Parse(txtFromDate.Text);
            DateTime activityToDate = DateTime.Parse(txtToDate.Text);

            // Retrieve data.
            this.mActivitySummary = this.RetrieveActivitySummary(userId, activityFromDate, activityToDate);

            // Display data.
            this.DisplayActivitySummary(this.mActivitySummary);

            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, GridEntity);
            divGridHeader.Visible = false;

           

            var LBLLSTACT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLLISTRESETACT")).FirstOrDefault();
            if (LBLLSTACT != null)
            {
                LBLListofAct = LBLLSTACT.DisplayText;
                LBLFound = LBLLSTACT.SupportingText1;

            }

            if (GridEntity.Rows.Count == 0)
            {
                this.ShowHideValidationMessage(true);
                this.HideIntialView(true);
                this.divEmptyRow.InnerHtml = NoRecfound;
                this.divMessage.Visible = false;

                this.hdrGridHeader.InnerText = string.Empty;
            }
            else
            {
                this.ShowHideValidationMessage(false);
                //this.HideIntialView(false);
                this.hdrGridHeader.InnerText = LBLListofAct + mActivitySummary.Count + LBLFound;
            }

        }
        catch (ValidationException ex)
        {
            //Display the validation  errors. 
            StringBuilder ErrorMessage = new StringBuilder();
            ErrorMessage.Append(string.Format("<table><tr><td>{0}</td></tr>", ex.Message));
            foreach (string s in ex.Data.Values)
            {
                ErrorMessage.Append(string.Format("<tr><td>{0}</td></tr></table>", s.ToString()));
            }
            this.DisplayValidationMessage(ex);
            this.ShowHideValidationMessage(true);
            this.divMessage.Style.Add("display", "block");
            //Hide List Found.
            this.hdrGridHeader.InnerText = string.Empty;
        }
        catch { throw; }
    }

    private void DisplayActivitySummary(List<CustomEntity> activitySummary)
    {
        try
        {

            // Bind with grid.
            this.GridEntity.DataSource = activitySummary;
            this.GridEntity.DataBind();

            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, GridEntity);
            divGridHeader.Visible = false;

        }
        catch { throw; }
    }

    private List<CustomEntity> RetrieveActivitySummary(int userId, DateTime activityFromDate, DateTime activityToDate)
    {
        IActivityService service = null;
        try
        {
            // Create an service.
            service = AppService.Create<IActivityService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            List<CustomEntity> activitySummary = service.RetrieveActivitySummary(userId, activityFromDate, activityToDate);

            // Return.
            return activitySummary;
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
            service = null;
        }
    }

    private void FillUserActivities(int userId, DateTime activityDate)
    {

        try
        {
            // Retrieve data.
            List<Activity> LstActivity = this.RetrieveActivities(userId, activityDate);

            // Display data.
            this.DisplayActivities(LstActivity);

            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Activity Summary Details";
        }
        catch { throw; }
    }

    private List<Activity> RetrieveActivities(int userId, DateTime activityDate)
    {
        IActivityService service = null;
        try
        {
            // Create an service.
            service = AppService.Create<IActivityService>();
            service.AppManager = this.mAppManager;
            List<Activity> LstActivity = service.Retrieve(userId, activityDate);
            return LstActivity;

        }
        catch { throw; }
    }

    private void DisplayActivities(List<Activity> activities)
    {
        try
        {
            // Bind With Grid
            this.GridResetView.DataSource = activities;
            this.GridResetView.DataBind();

            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, GridResetView);
            divGridHeader.Visible = false;

        }
        catch { throw; }

    }

    private void HideIntialView(bool visible)
    {
        this.divGridHeader.Visible = visible;
        this.divEmptyRow.Visible = visible;
    }

    private bool UpdateEntity()
    {
        IActivityService Service = null;
        List<Activity> mActivity = null;
        DateTime[] ActivityDate = null;
        CheckBox chkSel = null;
        bool validateupdate = false;
        try
        {

            mAppManager = Session["APP_MANAGER"] as IAppManager;

            // Build entity.
            mActivity = new List<Activity>();

            this.ValidateResetApproval();

            foreach (GridViewRow gv in GridEntity.Rows)
            {
                chkSel = (CheckBox)gv.FindControl("chkSelect");
                if (chkSel.Checked)
                {
                    Tks.Entities.Activity entities = new Tks.Entities.Activity();
                    //ActivityDate[g] = new DateTime();
                    //ActivityDate.SetValue(DateTime.Parse(gv.Cells[2].Text), gv.RowIndex);
                    HtmlGenericControl spnactivitydate = (HtmlGenericControl)gv.FindControl("spnActivityDate");
                    entities.Date = DateTime.Parse(spnactivitydate.InnerText.ToString());
                    mActivity.Add(entities);
                }

                ActivityDate = new DateTime[mActivity.Count()];
                ActivityDate = mActivity.Select(x => x.Date).ToArray<DateTime>();


            }
            string comment = txtComment.Value;
            int userId = int.Parse(hdnCreateUserId.Value);
            Service = AppService.Create<IActivityService>();
            Service.AppManager = mAppManager;
            Service.ResetApproval(userId, ActivityDate, comment);
            //if (GridEntity.Rows.Count > 0)


            //else
            //{
            //    this.divMessage.InnerText = "please choose the Activities to Reset.";
            //    this.divMessage.Style.Add("display", "block");
            //    divMessage.Visible = true;
            //    validateupdate = true;

            //}
        }

        catch (ValidationException ex)
        {

            //Display the validation  errors. 
            this.ShowHideValidationMessage(true);
            this.DisplayValidationMessage(ex);
            validateupdate = true;

        }



        catch { throw; }
        return validateupdate;
    }
    private bool CheckSelect()
    {

        bool validselect = false;
        try
        {
            foreach (GridViewRow gv in GridEntity.Rows)
            {
                CheckBox chkSel = (CheckBox)gv.FindControl("chkSelect");
                if (chkSel.Checked)
                    validselect = true;
            }
        }
        catch
        {
            throw;
        }
        return validselect;

    }

    private void ValidateEntity()
    {
        try
        {
            // Create exception instance.
            ValidationException exception = new ValidationException("");

            if (string.IsNullOrEmpty(txtCreateUser.Value) || txtCreateUser.Value.Trim() == "")
                exception.Data.Add("ActivityCreatedUser", LBLACTIVITYENTEREDUSR);

            if (string.IsNullOrEmpty(txtFromDate.Text) || txtFromDate.Text.Trim() == "")
                exception.Data.Add("ActivityEnteredFromDate", LBLACTIVITYFRDT);

            if (string.IsNullOrEmpty(txtToDate.Text) || txtToDate.Text.Trim() == "")
                exception.Data.Add("ActivityEnteredToDate", LBLACTIVITYTODT);




            // Throw the exception, if any.
            if (exception.Data.Count > 0)
                throw exception;
        }
        catch { throw; }
    }
    private void ValidateResetApproval()
    {
        try
        {
            // Create exception instance.
            ValidationException exception = new ValidationException("");

            if (!this.CheckSelect())
                exception.Data.Add("ACTIVITY_NOTSELECT", LBLACTTORESET);
            if (string.IsNullOrEmpty(txtComment.Value) || txtComment.Value.Trim() == "")
                exception.Data.Add("Comment", LBLCMTEMTY);

            //this.CheckSelect(); 

            // Throw the exception, if any.
            if (exception.Data.Count > 0)
                throw exception;
        }
        catch { throw; }
    }
    private void DisplayValidationMessage(Exception exception)
    {
        try
        {

            // Create bullet.

            BulletedList error = new BulletedList();
            error.DataTextField = "value";
            error.DataSource = exception.Data;
            error.DataBind();

            // Display message.
            HtmlGenericControl control = new HtmlGenericControl("span");
            control.InnerText = exception.Message;

            this.divMessage.Style.Add("display", "block");
            //this.divMessage.Visible = true;
            this.divMessage.InnerHtml = string.Empty;
            this.divMessage.Controls.Add(control);
            this.divMessage.Controls.Add(error);

            // Show message control.

        }
        catch { throw; }
    }

    private void ClearControl()
    {
        // clear the Control 
        txtCreateUser.Value = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtComment.Value = "";


    }
    private void ShowHideValidationMessage(bool visible)
    {
        try
        {
            //divMessage.InnerText = "";
            divMessage.Visible = visible;

        }
        catch { throw; }
    }
    #endregion Class Function

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Get from master.
            this.mAppManager = this.Master.AppManager;

            if (mAppManager.LoginUser.HasAdminRights == false)
            {
                ErrorLogProvider provider = null;
                try
                {
                    //create a exception.
                    Exception exception = HttpContext.Current.Error;

                    //insert into Error log.
                    provider = new ErrorLogProvider();
                    provider.AppManager = this.mAppManager;
                    provider.Insert1(exception);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (provider != null) { provider.Dispose(); }
                }
                //   this.DisplayMessage("YOU DO NOT HAVE ACCESS TO THIS PAGE");
                Response.Redirect("~/Homepage.aspx");

            }

            LoadLabels();

            // Fetch from session.
            if (Session[ACTIVITY_SUMMARY] != null)
                this.mActivitySummary = Session[ACTIVITY_SUMMARY] as List<CustomEntity>;

        }
        catch { throw; }
    }

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            Searchcriteria = Convert.ToString(GRID_TITLE.DisplayText);
            divEmptyRow.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
            NoRecfound = Convert.ToString(GRID_TITLE.SupportingText1);
        }

        var ALERTDT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLACTIVITYFRDT")).FirstOrDefault();
        if (ALERTDT != null)
        {

            LBLACTIVITYFRDT = Convert.ToString(ALERTDT.DisplayText);
            LBLACTIVITYTODT = Convert.ToString(ALERTDT.SupportingText1);
        }

        var ALERTDT1 = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLACTIVITYFRDT")).FirstOrDefault();
        if (ALERTDT1 != null)
        {

            LBLACTIVITYENTEREDUSR = Convert.ToString(ALERTDT1.DisplayText);
        }

        var ACTTORESETTXT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ACTTORESET")).FirstOrDefault();
        if (ACTTORESETTXT != null)
        {

            LBLACTTORESET = Convert.ToString(ACTTORESETTXT.DisplayText);
            LBLCMTEMTY = Convert.ToString(ACTTORESETTXT.SupportingText1);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Store values in session.
            Session.Add(ACTIVITY_SUMMARY, this.mActivitySummary);
        }
        catch { throw; }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;
        try
        {
            Exception exception = HttpContext.Current.Error;
            provider = new ErrorLogProvider();
            provider.AppManager = mAppManager;
            provider.Insert(exception);

        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }
    protected void GridEntity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Fetch data item.
                CustomEntity entity = (CustomEntity)e.Row.DataItem;

                // Assign value to controls.
                HtmlInputHidden hdnUserId = (HtmlInputHidden)e.Row.FindControl("hdnUserId");
                HtmlGenericControl spnActivityDate = (HtmlGenericControl)e.Row.FindControl("spnActivityDate");
                HtmlGenericControl spnUserName = (HtmlGenericControl)e.Row.FindControl("spnUserName");
                HtmlGenericControl spnActivityCount = (HtmlGenericControl)e.Row.FindControl("spnActivityCount");

                hdnUserId.Value = entity.CustomData["UserId"].ToString();
                spnActivityDate.InnerHtml = DateTime.Parse(entity.CustomData["ActivityDate"].ToString()).ToString("MM/dd/yyyy");
                spnUserName.InnerHtml = entity.CustomData["UserName"].ToString();
                spnActivityCount.InnerHtml = entity.CustomData["ActivityCount"].ToString();

            }
        }
        catch { throw; }

    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValiddate() == true)
            {
                this.FillActivitySummary();
            }
            //Label2.Visible = true;
            //Label2.Text = "Activity Reset Details";
        }
        catch { throw; }
    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearControl();
            this.ShowHideValidationMessage(false);
            GridEntity.DataSource = null;
            GridEntity.DataBind();
            GridResetView.DataSource = null;
            GridResetView.DataBind();
            this.divEmptyRow.InnerHtml = Searchcriteria;
            this.HideIntialView(true);
            lblAlertMsg.Visible = false;


            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, GridResetView);
            divGridHeader.Visible = false;

            this.hdrGridHeader.InnerText = string.Empty;
        }
        catch { throw; }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect("~/HomePage.aspx");
        }
        catch { throw; }
    }
    protected void btnReSet_Click(object sender, EventArgs e)
    {
        try
        {
            if (!this.UpdateEntity())
            {

                this.divMessage.InnerText = lblResetsuccessfully.Text;
                this.divMessage.Style.Add("display", "block");
                this.ShowHideValidationMessage(true);
                this.FillActivitySummary();
                this.ShowHideValidationMessage(true);
                GridResetView.DataSource = null;
                GridResetView.DataBind();
                lblAlertMsg.Visible = false;
                //this.ClearControl();
                txtComment.Value = "";
            }


        }
        catch { throw; }
    }


    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        //this.divEditControl.Visible = true;
    }


    protected void GridEntity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            this.divMessage.Style.Add("display", "none");
            //this.divMessage.Visible = true;
            this.divMessage.InnerHtml = string.Empty;
            if (e.CommandName.Equals("ResetActivity", StringComparison.InvariantCultureIgnoreCase))
            {

                // Fetch the selected row index.
                int index = Int32.Parse(e.CommandArgument.ToString());

                // Get values from list.
                CustomEntity selectedEntity = this.mActivitySummary[index];

                // Retrieve selected user's activities.
                this.FillUserActivities(
                    Int32.Parse(selectedEntity.CustomData["UserId"].ToString()),
                    DateTime.Parse(selectedEntity.CustomData["ActivityDate"].ToString()));
            }
        }
        catch { throw; }
    }
    protected void GridResetView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (e.NewPageIndex < 0)
            {
                GridResetView.PageIndex = 0;

            }
            else
            {
                GridResetView.PageIndex = e.NewPageIndex;
            }
            //this.FillUserActivities();

        }
        catch { throw; }
    }

    private bool IsValiddate()
    {
        DateTime value;
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        bool result = true;

        try
        {
            this.divMessage.InnerText = "";
            this.divMessage.Style.Add("display", "none");
            this.ShowHideValidationMessage(false);

            if ((txtFromDate.Text.Trim().Length == 8) && (txtFromDate.Text.IndexOf("/") == -1))
            {
                txtFromDate.Text = txtFromDate.Text.Insert(2, "/");
                txtFromDate.Text = txtFromDate.Text.Insert(5, "/");
            }

            if ((txtToDate.Text.Trim().Length == 8) && (txtToDate.Text.IndexOf("/") == -1))
            {
                txtToDate.Text = txtToDate.Text.Insert(2, "/");
                txtToDate.Text = txtToDate.Text.Insert(5, "/");
            }

            if (txtFromDate.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtFromDate.Text, out value))
                {
                    dt1 = Convert.ToDateTime(txtFromDate.Text);
                }
                else
                {
                    this.divMessage.InnerText = " Invalid From Date ,Date should be (MM/DD/YYYY) format";
                    this.divMessage.Style.Add("display", "block");
                    this.ShowHideValidationMessage(true);
                    result = false;
                }
            }
            if (txtToDate.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtToDate.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtToDate.Text);
                }
                else
                {
                    this.divMessage.InnerText = this.divMessage.InnerText + " Invalid To Date ,Date should be (MM/DD/YYYY) format";
                    this.divMessage.Style.Add("display", "block");
                    this.ShowHideValidationMessage(true);
                    result = false;
                }


            }
            return result;
        }
        catch { throw; }
    }
}