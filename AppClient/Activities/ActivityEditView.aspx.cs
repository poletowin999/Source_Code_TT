using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;


using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Activities_ActivityEditView : System.Web.UI.Page
{
    #region Class variables

    const string PROJECT_ACTIVITY = "1";
    const string MISCELLANEOUS_ACTIVITY = "2";
    const string SELECTED_ACTIVITY = "ActivityEditViewPage_SelectedActivity";
    const string SUCCEED_MESSAGE = "ActivityEditViewPage_SucceedMessage";
    StringBuilder mUserSearchViewDialogScript;

    IAppManager mAppManager;

    private int mSelectedActivityId;
    private Activity mSelectedActivity;
    private string mFormname;
    string ADDBTN;
    string EDITBTN;
    string ACTCANNOTEDIT;
    string SELECTUSER;
    string NOMANAGER;
    string NOSUPERVISOR;

    string ACTADDED;
    string ACTUPDATED;

    StringBuilder mClientScript;


    #endregion

    #region Internal members


    private void InitializeView()
    {
        try
        {
            // Get an instance of IAppManager.
            this.mAppManager = this.Master.AppManager;
            this.projectActivityEditView.AppManager = this.mAppManager;
            this.miscActivityEditView.AppManager = this.mAppManager;

            this.mClientScript = new StringBuilder();

            // Fetch values from session.
            this.FetchValuesFromStateManagement();

            // Hide info panels.
            this.DisplayOrClearSucceedMessage(string.Empty);
            this.DisplayOrClearValidationError(null);
            this.DisplayOrClearWarningMessage(string.Empty);
            this.FillUserAndReporingUsersDetail();

            if (!Page.IsPostBack)
            {
                // Check whether activity id is available in query string or not.
                int id;                     
                //if (Int32.TryParse(Request.QueryString["id"], out id))
                if (Int32.TryParse(Convert.ToString(this.Page.RouteData.Values["Id"]), out id))
                {
                    this.mSelectedActivityId = id;
                    //if (((object)Request.QueryString["Action"]) != null)
                    if (((object)this.Page.RouteData.Values["Action"]) != null)
                    {
                        this.mFormname = Convert.ToString(this.Page.RouteData.Values["Action"]); //Request.QueryString["Action"].ToString();
                        Session.Add("mFormname", mFormname);

                    }
                    ddlActivityTypeList.Enabled = false;
                    //btnUpdate.Text = EDITBTN;

                }
                else
                {
                    this.mSelectedActivityId = 0;
                    ddlActivityTypeList.Enabled = true;
                    //btnUpdate.Text = ADDBTN;
                    //checkActivitystatus();
                }
                //string action;
                //if (String.TryParse(Request.QueryString["Action"], out action))
                //    this.mFormname = Request.QueryString["Action"];
                //else
                //    this.mFormname = "";

                // Clear all controls.
                this.ClearActivityPanels();
            }
            // Post back.
            else
            {
                // ReInitialize user controls.

                // Project activity edit control.
                this.projectActivityEditView.Activity = this.mSelectedActivity;
                // Misc activity edit control.
                this.miscActivityEditView.Activity = this.mSelectedActivity;
            }

        }
        catch { throw; }
    }

    private void LoadView()
    {
        try
        {
            if (!Page.IsPostBack)
            {
                // Initialize control values.

                // Display user and their reporting users.
                this.FillUserAndReporingUsersDetail();

                // Fill activity.
                this.FillActivity(this.mSelectedActivityId);

                //Assign login user added on 01072011 by saravanan
                txtuser.Value = Convert.ToString(mAppManager.LoginUser.LastName + "," + mAppManager.LoginUser.FirstName);
                hdnuserid.Value = Convert.ToString(mAppManager.LoginUser.Id);
                //check user type
                if (mSelectedActivityId == 0)
                {
                    CheckIssupervisor();
                    checkActivitystatus();
                }
                else
                {
                    ibtSearchUser.Visible = false;
                    txtuser.Disabled = false;
                    chkIsAutoapproval.Visible = false;

                }
            }
            if (hdnuserid.Value.ToString() == mAppManager.LoginUser.Id.ToString())
            {
                chkIsAutoapproval.Checked = false;
                chkIsAutoapproval.Enabled = false;
            }
            else
            {
                chkIsAutoapproval.Checked = true;
                chkIsAutoapproval.Enabled = true;
            }
            checkActivitystatus();
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

        var ADDEDITBTN = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("BTNUPDATE")).FirstOrDefault();
        if (ADDEDITBTN != null)
        {
            ADDBTN = ADDEDITBTN.DisplayText;
            EDITBTN = ADDEDITBTN.SupportingText1;
        }

        var ACTIVITYAPPROVE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ACTIVITYAPPROVE")).FirstOrDefault();
        if (ACTIVITYAPPROVE != null)
        {
            ACTCANNOTEDIT = ACTIVITYAPPROVE.DisplayText;
            
        }
        var ACTSELECTUSER = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTMSGSELUSER")).FirstOrDefault();
        if (ACTSELECTUSER != null)
        {
            SELECTUSER = ACTSELECTUSER.DisplayText;

        }

        var LBLMANAGERSUPERVISOR = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTLBLMANAGER")).FirstOrDefault();
        if (LBLMANAGERSUPERVISOR != null)
        {
            NOMANAGER = LBLMANAGERSUPERVISOR.DisplayText;
            NOSUPERVISOR = LBLMANAGERSUPERVISOR.SupportingText1;

        }

        var LBLACTADDUPD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ACTADDUPD")).FirstOrDefault();
        if (LBLACTADDUPD != null)
        {
            ACTADDED = LBLACTADDUPD.DisplayText;
            ACTUPDATED = LBLACTADDUPD.SupportingText1;

        }

    } 

    private void FetchValuesFromStateManagement()
    {
        try
        {
            // Restore from hidden fields.
            this.mSelectedActivityId = Int32.Parse(this.hdnActivityId.Value);

            // Restore values from session.
            if (Session[SELECTED_ACTIVITY] != null)
                this.mSelectedActivity = Session[SELECTED_ACTIVITY] as Activity;

        }
        catch { throw; }
    }

    private void FillActivity(int activityId)
    {
        try
        {
            // Current date is default activity date.
            this.txtActivityDate.Text = this.RetrieveSystemCurrentDateTime().ToString("MM/dd/yyyy");

            // Retrieve activity based on id.
            this.mSelectedActivity = this.RetrieveActivity(activityId);

            // Display warning message, if activity is approved.
            if (this.mSelectedActivity.StatusId == 2)
            {
                this.DisplayOrClearWarningMessage(ACTCANNOTEDIT);
                this.btnUpdate.Enabled = false;
            }

            // Display values.
            this.hdnActivityId.Value = this.mSelectedActivity.Id.ToString();
            this.txtActivityDate.Text = this.mSelectedActivity.Date.ToString("MM/dd/yyyy");

            if (activityId > 0 && this.mSelectedActivity.CustomData["Comment"] != null)
            {
                // txtComment.InnerText = this.mSelectedActivity.CustomData["Comment"].ToString();
            }
            else
            {
                txtComment.InnerText = "";
            }

            if (this.mSelectedActivity.Id == 0)
            {
                // Select project activity as default.
                this.ddlActivityTypeList.SelectedValue = PROJECT_ACTIVITY;
                this.ChangeActivityType(PROJECT_ACTIVITY);
            }
            else
            {
                // Based on selected activity.
                this.ddlActivityTypeList.SelectedValue = this.mSelectedActivity.TypeId.ToString();
                this.ChangeActivityType(this.ddlActivityTypeList.SelectedValue);
                RetrieveComments(activityId);
            }
        }
        catch { throw; }
    }

    private void FillUserAndReporingUsersDetail()
    {
        IUserService service = null;
        try
        {
            // Display user info.
            User loginUser = this.mAppManager.LoginUser;
            this.divUserInfo.Controls.Clear();
            this.divUserInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = loginUser.ToString("FIL") });
            this.divUserInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = loginUser.EmailId });

            // Create the service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mAppManager;

            // Retrieve user's supervisor.
            User supervisor = service.RetrieveSupervisor(loginUser.Id);
            // Display supervisor info.
            this.divSupervisorInfo.Controls.Clear();
            if (supervisor != null)
            {
                this.divSupervisorInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = supervisor.ToString("FIL") });
                this.divSupervisorInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = supervisor.EmailId });
            }
            else
            {
                this.divSupervisorInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = "-- " + NOSUPERVISOR + " --" });
            }

            // Retrieve user's manager.
            User manager = service.RetrieveManager(loginUser.Id);
            // Display manager info.
            this.divManagerInfo.Controls.Clear();
            if (manager != null)
            {
                this.divManagerInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = manager.ToString("FIL") });
                this.divManagerInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = manager.EmailId });
            }
            else
            {
                this.divManagerInfo.Controls.Add(new HtmlGenericControl("span") { InnerHtml = "-- " + NOMANAGER +"-- " });
            }
        }
        catch { throw; }
    }

    private DateTime RetrieveSystemCurrentDateTime()
    {
        SettingProvider provider = null;
        try
        {
            provider = new SettingProvider();
            provider.AppManager = this.mAppManager;
            return provider.GetSystemDateTime();
        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    private void ChangeActivityType(string activityType)
    {
        try
        {
            // Clear both activity controls.
            this.projectActivityEditView.ClearActivity();
            this.miscActivityEditView.ClearActivity();

            // Initialize controls based on selected activity type.
            if (activityType == "2")
            {
                this.mSelectedActivity.TypeId = 2;

                // Initialize control.
                this.miscActivityEditView.Activity = this.mSelectedActivity;
                this.miscActivityEditView.DisplayActivity();

                // Hide project activity control.
                this.projectActivityEditView.Visible = false;

                // Show misc activity control.
                this.miscActivityEditView.Visible = true;
            }
            else
            {
                // Default activity is project activity.
                this.mSelectedActivity.TypeId = 1;

                // Initialize control.
                this.projectActivityEditView.Activity = this.mSelectedActivity;
                this.projectActivityEditView.DisplayActivity();

                // Show project activity control.
                this.projectActivityEditView.Visible = true;

                // Hide misc activity control.
                this.miscActivityEditView.Visible = false;

            }

            // Set focus on type.
            if (mFormname == null && mFormname != "Approval")
            {
                this.mClientScript.Append(
                    string.Format("setTimeout(function() {{ $get('{0}').focus(); }}, 100);",
                        this.ddlActivityTypeList.ClientID)
                            );
            }

        }
        catch { throw; }
    }


    private void ClearActivityPanels()
    {
        try
        {
            // Clear project activity controls.
            this.projectActivityEditView.ClearActivity();

            // Clear misc activity controls.
            this.miscActivityEditView.ClearActivity();
        }
        catch { throw; }
    }

    private Activity RetrieveActivity(int id)
    {
        IActivityService service = null;
        try
        {
            // Create empty new activity if id is 0.
            if (id == 0)
            {
                return new Activity()
                {
                    Id = 0,
                    TypeId = 1,
                    Date = DateTime.Parse(txtActivityDate.Text),
                    StartDateTime = DateTime.Parse(txtActivityDate.Text),
                    EndDateTime = DateTime.Parse(txtActivityDate.Text)

                };
            }

            // Create service.
            service = AppService.Create<IActivityService>();
            service.AppManager = this.mAppManager;
            return service.Retrieve(id);
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void UpdateActivity()
    {
        IActivityService service = null;
        try
        {
            // Validate 
            this.ValidateActivity();

            // Initialize values.
            this.mSelectedActivity.Date = DateTime.Parse(this.txtActivityDate.Text);
            //this.mSelectedActivity.StatusId = 1;        // Set as Waiting For Approval.
            //Added by saravanan on 01072011
            if (hdnuserid.Value.ToString() == mAppManager.LoginUser.Id.ToString())
            {
                chkIsAutoapproval.Checked = false;
            }
            if (chkIsAutoapproval.Checked == true)
            {
                this.mSelectedActivity.StatusId = 2;
            }
            else
            {
                this.mSelectedActivity.StatusId = 1;
            }
            this.mSelectedActivity.Comment = this.txtComment.InnerHtml;

            if (this.mSelectedActivity.Id == 0)
            {
                //this.mSelectedActivity.CreateUserId = this.mAppManager.LoginUser.Id;
                if (hdnuserid.Value == "")
                {
                    hdnuserid.Value = this.mAppManager.LoginUser.Id.ToString();
                }
                //Added by saravan on 01072011
                this.mSelectedActivity.CreateUserId = Convert.ToInt16(hdnuserid.Value.ToString());
                this.mSelectedActivity.CreateDate = DateTime.Now;
            }
            this.mSelectedActivity.LastUpdateUserId = this.mAppManager.LoginUser.Id;
            this.mSelectedActivity.LastUpdateDate = DateTime.Now;


            // Update.
            service = AppService.Create<IActivityService>();
            service.AppManager = this.mAppManager;
            service.Update(this.mSelectedActivity);

            if (this.mSelectedActivity.Id == 0)
            {
                // Display succeed message.
                this.DisplayOrClearSucceedMessage(ACTADDED);
            }
            else
            {
                this.DisplayOrClearSucceedMessage(ACTUPDATED);
            }

            // After update new activity, allow user to enter next new activity.
            if (this.mSelectedActivity.Id == 0)
            {
                this.FillActivity(0);
            }
            // After update the existing activity then move to list view.
            else
            {
                // Add succeed message in session to display in list view page.
                Session.Add(SUCCEED_MESSAGE, ACTUPDATED);

                if (Session["mFormname"] != null)
                    mFormname = Session["mFormname"].ToString();
                else
                    mFormname = "";
                //if (mFormname == "Approval")
                //{
                //    this.Page.Response.Redirect("ActivityReviewView.aspx?action=retain", true);
                //}
                //else
                //{

                //    this.Page.Response.Redirect("ActivityListView.aspx?action=retain", true);
                //}

                if (mFormname == "Approval")
                {
                    this.Page.Response.Redirect("Review-retain", true);
                }
                else
                {

                    this.Page.Response.Redirect("List-retain", true);
                }


                //if (mFormname == "" || mFormname == null)
                //{
                //    // Display list view page.
                //    Page.Response.Redirect("ActivityListView.aspx?action=retain", true);
                //}
                //else
                //{
                //    // Display list view page.
                //    Page.Response.Redirect("ActivityReviewView.aspx?action=retain", true);
                //}
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (ValidationException ve)
        {
            // Display error.
            this.DisplayOrClearValidationError(ve);
        }
        catch { throw; }
    }

    private void ValidateActivity()
    {
        try
        {

            DateTime dt3 = new DateTime();
            DateTime dt4 = new DateTime();

            dt3 = Convert.ToDateTime(txtActivityDate.Text);
            dt4 = Convert.ToDateTime(hdndt.Text);

            ValidationException exception = new ValidationException(lblValError.Text);
            hdn1.Value = this.mAppManager.LoginUser.LocationId.ToString();
            if (hdn1.Value == "1")
            {
                if (dt3 > dt4)
                {

                }
                else
                {
                        exception.Data.Add("ACTIVITY_DATE", "Activity date should be greater than" + dt4.ToString());
                }
            }

            if (string.IsNullOrEmpty(this.txtActivityDate.Text) || string.IsNullOrEmpty(this.txtActivityDate.Text.Trim()))
                exception.Data.Add("ACTIVITY_DATE", "Activity date should not be empty.");
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(this.txtActivityDate.Text, out temp))
                    exception.Data.Add("ACTIVITY_DATE", "Activity date is invalid.");
            }

            if (this.ddlActivityTypeList.SelectedIndex == -1)
                exception.Data.Add("ACTIVITY_TYPE", "Activity type should not be empty.");
            if (this.hdnuserid.Value.ToString() == "")
                exception.Data.Add("ACTIVITY_TYPE", SELECTUSER);


            // Retrieve values from user controls.
            try
            {
                // If any data type or data error exists, then it throw exception.
                if (this.ddlActivityTypeList.SelectedValue == MISCELLANEOUS_ACTIVITY)
                    this.mSelectedActivity = this.miscActivityEditView.RetrieveActivity();
                else
                    this.mSelectedActivity = this.projectActivityEditView.RetrieveActivity();
            }
            catch (ValidationException ve)
            {
                foreach (DictionaryEntry entry in ve.Data)
                    exception.Data.Add(entry.Key, entry.Value);
            }
            catch { throw; }

            // Throw an exception.
            if (exception.Data.Count > 0) throw exception;
        }
        catch { throw; }
    }

    private void DisplayOrClearSucceedMessage(string message)
    {
        try
        {
            // Display message.
            this.divInfoPanel.InnerHtml = message;

            // Show or hide control.
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(message.Trim()))
                this.ShowOrHideSucceedMessage(false);
            else
                this.ShowOrHideSucceedMessage(true);
        }
        catch { throw; }
    }

    private void ShowOrHideSucceedMessage(bool visible)
    {
        try
        {
            this.divInfoPanel.Visible = visible;
        }
        catch { throw; }
    }

    private void DisplayOrClearWarningMessage(string message)
    {
        try
        {
            // Display message.
            this.divWarnInfoPanel.InnerHtml = message;

            // Show or hide control.
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(message.Trim()))
                this.ShowOrHideWarningMessage(false);
            else
                this.ShowOrHideWarningMessage(true);
        }
        catch { throw; }
    }

    private void ShowOrHideWarningMessage(bool visible)
    {
        try
        {
            this.divWarnInfoPanel.Visible = visible;
        }
        catch { throw; }
    }

    private void DisplayOrClearValidationError(Exception exception)
    {
        try
        {
            if (exception == null)
            {
                this.divErrorPanel.InnerHtml = "";
                this.ShowOrHideValidationError(false);
                return;
            }

            // Build error message.
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerHtml = exception.Message;

            BulletedList errors = new BulletedList();
            errors.DataTextField = "Value";
            errors.DataSource = exception.Data;
            errors.DataBind();

            // Add to error control.
            this.divErrorPanel.Controls.Clear();
            this.divErrorPanel.Controls.Add(span);
            this.divErrorPanel.Controls.Add(errors);

            // Show the control.
            this.ShowOrHideValidationError(true);
        }
        catch { throw; }
    }

    private void ShowOrHideValidationError(bool visible)
    {
        try
        {
            this.divErrorPanel.Visible = visible;
        }
        catch { throw; }
    }

    #endregion

    #region PageMethod members



    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
          
            this.InitializeUserSearchViewDialogScript();

            this.InitializeView();
            LoadLabels();

            if (hdnActivityId.Value != "0")
            {
                btnUpdate.Text = ADDBTN;
            }
            else
            {
                btnUpdate.Text = EDITBTN;
            }

            System.Diagnostics.Debug.Print("Page Load event.");
        }
        catch { throw; }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            this.LoadView();
            System.Diagnostics.Debug.Print("Page Complete event.");
        }
        catch { throw; }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            try
            {

                // Store in session.
                Session.Add(SELECTED_ACTIVITY, this.mSelectedActivity);

                // Register scripts.
                if (this.mUserSearchViewDialogScript != null)
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        System.Guid.NewGuid().ToString(),
                        this.mUserSearchViewDialogScript.ToString(),
                        true);

                // Page script.
                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    System.Guid.NewGuid().ToString(),
                    this.mClientScript.ToString(),
                    true);

                // Startup script.
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    System.Guid.NewGuid().ToString(),
                    this.mClientScript.ToString(),
                    true);


            }
            catch { throw; }
        }
        catch { throw; }
    }

    protected void btnUpdate_ServerClick(object sender, EventArgs e)
    {
        try
        {
            this.UpdateActivity();
        }
        catch { throw; }
    }


    protected void ddlActivityTypeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Change activity type.
            this.ChangeActivityType(ddlActivityTypeList.SelectedValue);
        }
        catch { throw; }
    }

    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        try
        {
            if (Session["mFormname"] != null)
                mFormname = Session["mFormname"].ToString();
            else
                mFormname = "";
            if (mFormname == "Approval")
            {
                this.Page.Response.Redirect("Review-close", true);
            }
            else
            {

                this.Page.Response.Redirect("List-retain", true);
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch {  }

    }


    protected void txtActivityDate_TextChanged(object sender, EventArgs e)
    {
        IsValiddate();
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

    private bool IsValiddate()
    {
        DateTime value;
        DateTime dt1 = new DateTime();
        bool result = true;

        try
        {

            if ((txtActivityDate.Text.Trim().Length == 8) && (txtActivityDate.Text.IndexOf("/") == -1))
            {
                txtActivityDate.Text = txtActivityDate.Text.Insert(2, "/");
                txtActivityDate.Text = txtActivityDate.Text.Insert(5, "/");
            }
            this.divErrorPanel.InnerHtml = "";
            if (txtActivityDate.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtActivityDate.Text, out value))
                {
                    dt1 = Convert.ToDateTime(txtActivityDate.Text);
                    // Set focus on location.
                    this.mClientScript.Append(
                        string.Format("setTimeout(function() {{ $get('{0}').focus(); }}, 100);",
                            this.ddlActivityTypeList.ClientID)
                        );
                }
                else
                {
                    this.divErrorPanel.InnerHtml = "Invalid Activity Date ,Date should be (MM/DD/YYYY) format";
                    this.ShowOrHideValidationError(true);
                    this.DisplayOrClearSucceedMessage(string.Empty);
                    result = false;
                }
            }
            return result;
        }
        catch { throw; }
    }

    private void RetrieveComments(int ActivityId)
    {
        IActivityService service = null;
        List<ActivityComment> ActivityCommentList = null;
        // Update.
        service = AppService.Create<IActivityService>();
        service.AppManager = this.mAppManager;
        ActivityCommentList = service.RetrieveComments(ActivityId);
        if (ActivityCommentList != null)
        {
            RepComments.DataSource = ActivityCommentList;
            RepComments.DataBind();
        }
    }
    #region Initialize  User Search View Dialog
    private void InitializeUserSearchViewDialogScript()
    {
        try
        {
            mUserSearchViewDialogScript = new StringBuilder();
            mUserSearchViewDialogScript.Append("$(document).ready(function() { refreshUserSearchView(); });");

        }
        catch { throw; }

    }
    #endregion
    protected void ibtSearchUser_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.UserSearchView.Display();
            this.mUserSearchViewDialogScript.Append("showUserSearchView();");
        }
        catch { throw; }
    }

    private void CheckIssupervisor()
    {
        IUserService mUserService = null;
        List<Tks.Entities.User> LstUser = null;
        mUserService = AppService.Create<IUserService>();
        mUserService.AppManager = this.mAppManager;
        LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(mAppManager.LoginUser.Id));
        if (LstUser == null)
        {
            ibtSearchUser.Visible = false;
            txtuser.Disabled = true;
        }
        else
        {
            txtuser.Disabled = false;
            ibtSearchUser.Visible = true;
        }
        if (mAppManager.LoginUser.HasAdminRights == true)
        {
            ibtSearchUser.Visible = true;
            txtuser.Disabled = false;
        }
    }

    private void checkActivitystatus()
    {
        List<Activity> Activitylst = null;
        IActivityService mActivityService = null;
        Activitylst = new List<Activity>();
        this.mAppManager = Master.AppManager;
        mActivityService = AppService.Create<IActivityService>();
        mActivityService.AppManager = mAppManager;
        // retrieve
        if (hdnuserid.Value != "")
        {
            Activitylst = mActivityService.Retrieve(Convert.ToInt32(hdnuserid.Value), DateTime.Parse(txtActivityDate.Text));
            if (Activitylst != null)
            {
                var resultList = from item in Activitylst
                                 where item.StatusId == 2
                                 select item;

                Activitylst = resultList.ToList<Activity>();

                if (Activitylst.Count > 0)
                {
                    chkIsAutoapproval.Enabled = false;
                    chkIsAutoapproval.Checked = true;
                    txtuser.Disabled = false;
                }
                else
                {
                    if (hdnuserid.Value.ToString() == mAppManager.LoginUser.Id.ToString())
                    {
                        txtuser.Disabled = true;
                    }
                    chkIsAutoapproval.Enabled = false;
                    chkIsAutoapproval.Checked = false;
                }
            }
        }
        else
        {
            Activitylst = mActivityService.Retrieve(Convert.ToInt32(mAppManager.LoginUser.Id), DateTime.Parse(txtActivityDate.Text));
            if (Activitylst != null)
            {
                var resultList = from item in Activitylst
                                 where item.StatusId == 2
                                 select item;

                Activitylst = resultList.ToList<Activity>();

                if (Activitylst.Count > 0)
                {
                    chkIsAutoapproval.Enabled = false;
                    chkIsAutoapproval.Checked = true;
                    txtuser.Disabled = false;
                }
                else
                {
                    chkIsAutoapproval.Enabled = false;
                    chkIsAutoapproval.Checked = false;
                    txtuser.Disabled = true;
                }
            }
        }
    }

}