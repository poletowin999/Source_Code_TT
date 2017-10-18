using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Tks.Model;
using Tks.Entities;
using Tks.Services;


public partial class Activities_ActivityListView : System.Web.UI.Page
{
    #region Class variables

    const string ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE = "ActivityEditViewPage_SucceedMessage";

    IAppManager mAppManager;

    string SELECTACTDATE;
    string SELECTCOPYDATE;
    string COPYDATE;
    string DELETEDATE;
    string ACTIVITYNOTFOUND;
    string INVALIDACTDATE;
    string INVALIDCOPYDATE;

    string cntlist;
    string cntFound;

    #endregion

    #region Internal members

    private void InitializeView()
    {
        try
        {
            // Get an instance of IAppManager.
            this.mAppManager = this.Master.AppManager;
             Hidloc.Value = this.mAppManager.LoginUser.LocationId.ToString();
            // Hide info panels.
            this.DisplayOrClearSucceedMessage(string.Empty);
            this.ShowOrHideValidationError(false);
            if (Page.IsPostBack)
            {

            }
            else
            {
                // Not post page.
                if (Session["Edit_Activitydate"] != null)
                {
                    this.txtActivityDate.Text = Session["Edit_Activitydate"].ToString();
                    LoadActivityList(Session["Edit_Activitydate"].ToString());
                    
                }
                else
                {
                    // Display current date as default date.
                    this.txtActivityDate.Text = this.RetrieveSystemCurrentDateTime().ToString("MM/dd/yyyy");
                    if (txtActivityDate.Text.Trim() != "")
                    {
                        LoadActivityList(txtActivityDate.Text);
                    }
                }

                // Display succeed message if any from other page.
                this.DisplayOrClearSucceedMessage(this.RetrieveOtherPageSucceedMessage());
                //if (this.RetrieveOtherPageSucceedMessage().Trim() != "")
                //{
                //    this.divErrorPanel.InnerHtml = this.RetrieveOtherPageSucceedMessage();
                //    this.ShowOrHideValidationError(true);
                //}
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
    private string RetrieveOtherPageSucceedMessage()
    {
        try
        {
            string message = string.Empty;

            // Read from session.
            if (Session[ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE] != null)
                message = Session[ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE].ToString();

            // Next time need not to display this message so remove from session.
            Session.Remove(ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE);

            return message;
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


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            LoadLabels();
            this.InitializeView();
            
            //this.spnMessage.InnerHtml = "<b> Activity not found.</b>";
        }
        catch { throw; }
    }
    protected void lbtRefreshActivity_Click(object sender, EventArgs e)
    {
        if (txtActivityDate.Text.Trim() != "")
        {
            if (IsValiddate() == true)
            {
                LoadActivityList(txtActivityDate.Text);
            }
        }
        else
        {
            DisplayOrClearSucceedMessage(SELECTACTDATE);
            this.divgvwListactivity.Visible = false;
            this.divgvwListactivityHeader.Visible = true;

            gvwListactivity.DataSource = null;
            gvwListactivity.DataBind();
            txtCopyActivityDate.Text = "";
        }
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
            this.spnMessage.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
            //this.divEmptyRow.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

        var SELECTACT_DATE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("SELECTACTDATE")).FirstOrDefault();
        if (SELECTACT_DATE != null)
        {
            SELECTACTDATE = Convert.ToString(SELECTACT_DATE.DisplayText);
            SELECTCOPYDATE = Convert.ToString(SELECTACT_DATE.SupportingText1);
        }

        var COPYDELDATE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("COPYDELDATE")).FirstOrDefault();
        if (COPYDELDATE != null)
        {
            COPYDATE = Convert.ToString(COPYDELDATE.DisplayText);
            DELETEDATE = Convert.ToString(COPYDELDATE.SupportingText1);
        }

        var ACTIVITYNOT_FOUND = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ACTIVITYNOTFOUND")).FirstOrDefault();
        if (ACTIVITYNOT_FOUND != null)
        {
            ACTIVITYNOTFOUND = Convert.ToString(ACTIVITYNOT_FOUND.DisplayText);
        }

        var INVALID_DATE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("INVALIDDATE")).FirstOrDefault();
        if (INVALID_DATE != null)
        {
            INVALIDACTDATE = Convert.ToString(INVALID_DATE.DisplayText);
            INVALIDCOPYDATE = Convert.ToString(INVALID_DATE.SupportingText1);
        }

        var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
        if (NoofRecordFound != null)
        {
            cntlist = NoofRecordFound.DisplayText;
            cntFound = NoofRecordFound.SupportingText1;
        }

    } 

    protected void lbtClear_Click(object sender, EventArgs e)
    {
        txtActivityDate.Text = "";
        DisplayOrClearSucceedMessage("");
        this.divgvwListactivity.Visible = false;
        this.divgvwListactivityHeader.Visible = true;
        this.hdrListHeader.InnerText = "";

        gvwListactivity.DataSource = null;
        gvwListactivity.DataBind();
        txtCopyActivityDate.Text = "";

    }
    private void DisplayList(List<Tks.Entities.Client> list)
    {
        try
        {
            // Bind with grid.
            this.gvwListactivity.DataSource = list;
            this.gvwListactivity.DataBind();
        }
        catch { throw; }
    }

    protected void btnViewActivityList_Click(object sender, EventArgs e)
    {
        if (txtActivityDate.Text.Trim() != "")
        {
            if (IsValiddate() == true)
            {
                LoadActivityList(txtActivityDate.Text);
                Session.Add("Edit_Activitydate", txtActivityDate.Text.ToString());
                Activitymsg();
            }
        }
        else
        {
            this.divErrorPanel.InnerHtml = SELECTACTDATE;
            this.ShowOrHideValidationError(true);
            this.DisplayOrClearSucceedMessage(string.Empty);
        }
    }
    protected void gvwListactivity_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {

            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                string activityid = e.CommandArgument.ToString();
                if (e.CommandName == "EditActivity")
                {
                    HidActivity.Value = activityid;
                    ShowEditActivity(activityid);
                }
                else if (e.CommandName == "DeleteActivity")
                {
                    DeleteActivity(activityid);
                }
                else if (e.CommandName == "ViewActivity")
                {
                    HidActivity.Value = activityid;
                    ShowEditActivity(activityid);
                }

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
    protected void gvwListactivity_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    private void ShowEditActivity(string Activityid)
    {
        try
        {

            Page.Response.Redirect("~/Activities/033-" + Activityid + "-Edit", true);
            //Page.Response.Redirect("ActivityEditView.aspx?id=" + Activityid, true);
        }
        catch (System.Threading.ThreadAbortException)
        {
            // Suppress this ThreadAbortException error.
        }
        catch { throw; }
    }
    private void LoadActivityList(string Activitydate)
    {
        List<Activity> Activitylst = null;
        IActivityService mActivityService = null;
        Activitylst = new List<Activity>();
        this.mAppManager = Master.AppManager;
        mActivityService = AppService.Create<IActivityService>();
        mActivityService.AppManager = mAppManager;
        // retrieve
        Activitylst = mActivityService.Retrieve(Master.AppManager.LoginUser.Id, DateTime.Parse(Activitydate));
        if (Activitylst != null)
        {
            gvwListactivity.DataSource = Activitylst;
            gvwListactivity.DataBind();
            this.hdrListHeader.InnerText = cntlist + " " + gvwListactivity.Rows.Count + " " + cntFound;

            //this.divgvwListactivityHeader.Visible = false;
            this.divgvwListactivity.Visible = true;

            var resultList = from item in Activitylst
                             where item.StatusId == 2
                             select item;

            gvwListactivity.Columns[0].Visible = true;
            gvwListactivity.Columns[1].Visible = true;
            gvwListactivity.Columns[2].Visible = false;


            Activitylst = resultList.ToList<Activity>();
            lbtAddNew.Enabled = true;
            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, gvwListactivity);
            divgvwListactivityHeader.Visible = false;

            if (Activitylst.Count > 0)
            {
                lbtAddNew.Enabled = false;
                checkActivitystatus();
                gvwListactivity.Columns[0].Visible = false;
                gvwListactivity.Columns[1].Visible = false;
                gvwListactivity.Columns[2].Visible = true;

                

            }

            lbtRefreshActivity.Enabled = true;
            //this.spanmsgg.Visible = false;
        }
        else
        {
            this.divgvwListactivity.Visible = false;
            this.divgvwListactivityHeader.Visible = true;
            this.hdrListHeader.InnerText = "";
            //this.spanmsgg.Visible = false;
            gvwListactivity.DataSource = null;
            gvwListactivity.DataBind();
            lbtRefreshActivity.Enabled = false;
        }
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


    protected void btnCopyActivitydate_Click(object sender, EventArgs e)
    {
      
        
        
        if (txtActivityDate.Text.Trim() != "")
        {
            if (txtCopyActivityDate.Text.Trim() != "")
            {
                if (IsValiddate() == true)
                {
                    CopyActivity();
                    Session.Add("Edit_Activitydate", txtActivityDate.Text.ToString());
                    txtCopyActivityDate.Text = "";
                }
            }
            else
            {
                this.divErrorPanel.InnerHtml = SELECTCOPYDATE;
                this.ShowOrHideValidationError(true);
                this.DisplayOrClearSucceedMessage(string.Empty);
            }
        }
        else
        {
            this.divErrorPanel.InnerHtml = SELECTACTDATE;
            this.ShowOrHideValidationError(true);
            this.DisplayOrClearSucceedMessage(string.Empty);
        }
    }

    private void CopyActivity()
    {
        IActivityService service = null;
        try
        {

            // Update.
            service = AppService.Create<IActivityService>();
            service.AppManager = this.mAppManager;
            service.CopyActivities(Convert.ToDateTime(txtCopyActivityDate.Text), Convert.ToDateTime(txtActivityDate.Text), Convert.ToInt32(HttpContext.Current.Session["SesLanguageId"]));
            this.divErrorPanel.InnerHtml = COPYDATE;
            this.ShowOrHideValidationError(true);
            this.DisplayOrClearSucceedMessage(string.Empty);
            LoadActivityList(txtActivityDate.Text);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (ValidationException ve)
        {
            // Display error.
            this.DisplayOrClearValidationError(ve);
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
                this.DisplayOrClearSucceedMessage(string.Empty);
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
            this.DisplayOrClearSucceedMessage(string.Empty);
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
    private void DeleteActivity(string ActivityId)
    {
        IActivityService service = null;
        service = AppService.Create<IActivityService>();
        service.AppManager = this.mAppManager;
        service.DeleteActivity(Convert.ToInt32(ActivityId));
        LoadActivityList(txtActivityDate.Text);
        this.divErrorPanel.InnerHtml = DELETEDATE;
        this.ShowOrHideValidationError(true);
        this.DisplayOrClearSucceedMessage(string.Empty);
    }

    private void Activitymsg()
    {
        if (gvwListactivity.Rows.Count <= 0)
        {
            this.divErrorPanel.InnerHtml = ACTIVITYNOTFOUND;
            this.ShowOrHideValidationError(true);
            this.DisplayOrClearSucceedMessage(string.Empty);
            spnMessage.Visible = true;
        }
        else
        {
            this.divErrorPanel.InnerHtml = "";
            //spanmsgg.Visible = false;
            spnMessage.Visible = false;
        }
    }

    private bool IsValiddate()
    {
        DateTime value;
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        bool result = true;
        DateTime dt3 = new DateTime();
        DateTime dt4 = new DateTime();

        if (txtCopyActivityDate.Text == "")
        {
            dt3 = DateTime.Now;
        }
        else
            dt3 = Convert.ToDateTime(txtCopyActivityDate.Text);
            
        dt4 = Convert.ToDateTime(hdndt.Text);

        string aa = Hidloc.Value;
       
        try
        {

            if ((txtActivityDate.Text.Trim().Length == 8) && (txtActivityDate.Text.IndexOf("/") == -1))
            {
                txtActivityDate.Text = txtActivityDate.Text.Insert(2, "/");
                txtActivityDate.Text = txtActivityDate.Text.Insert(5, "/");
            }

            if ((txtCopyActivityDate.Text.Trim().Length == 8) && (txtCopyActivityDate.Text.IndexOf("/") == -1))
            {
                txtCopyActivityDate.Text = txtCopyActivityDate.Text.Insert(2, "/");
                txtCopyActivityDate.Text = txtCopyActivityDate.Text.Insert(5, "/");
            }

            this.divErrorPanel.InnerHtml = "";
            if (txtActivityDate.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtActivityDate.Text, out value))
                {
                    dt1 = Convert.ToDateTime(txtActivityDate.Text);
                }
                else
                {
                    this.divErrorPanel.InnerHtml = "Invalid Activity Date ,Date should be (MM/DD/YYYY) format";
                    this.ShowOrHideValidationError(true);
                    this.DisplayOrClearSucceedMessage(string.Empty);
                    result = false;
                }
            }

            if (aa == "1")
            {
                if (dt3 > dt4)
                {

                }
                else
                {
                    if (this.divErrorPanel.InnerHtml != "")
                    {
                        this.divErrorPanel.InnerHtml = this.divErrorPanel.InnerHtml.ToString() + "Copy activity date should be greater than" + dt4.ToString();
                    }
                    else
                    {
                        this.divErrorPanel.InnerHtml = "Copy activity date should be greater than" + dt4.ToString();
                    }
                    this.ShowOrHideValidationError(true);
                    this.DisplayOrClearSucceedMessage(string.Empty);
                    result = false;
                }
            }


            if (txtCopyActivityDate.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtCopyActivityDate.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtCopyActivityDate.Text);
                }
                else
                {
                    if (this.divErrorPanel.InnerHtml != "")
                    {
                        this.divErrorPanel.InnerHtml = this.divErrorPanel.InnerHtml.ToString() + " and Invalid Copy Activity Date , Date should be (MM/DD/YYYY) format";
                    }
                    else
                    {
                        this.divErrorPanel.InnerHtml = " Invalid Copy Activity Date , Date should be (MM/DD/YYYY) format";
                    }
                    this.ShowOrHideValidationError(true);
                    this.DisplayOrClearSucceedMessage(string.Empty);
                    result = false;

                }
            }


            return result;
        }
        catch { throw; }
    }

    private void checkActivitystatus()
    {
        List<Activity> Activitylst = null;
        IActivityService mActivityService = null;
        Activitylst = new List<Activity>();
        this.mAppManager = Master.AppManager;
        mActivityService = AppService.Create<IActivityService>();
        mActivityService.AppManager = mAppManager;
                 Activitylst = mActivityService.Retrieve(Convert.ToInt32(mAppManager.LoginUser.Id), DateTime.Parse(txtActivityDate.Text));
                 if (Activitylst != null)
                 {
                     var resultList = from item in Activitylst
                                      where item.StatusId == 2
                                      select item;

                     Activitylst = resultList.ToList<Activity>();

                     if (Activitylst.Count > 0)
                     {
                         lbtAddNew.Enabled = true;
                     }
                    
                 }
           
    }
}