using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Text;
using CalendarButton;
using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Activities_UserSwipeEditView : System.Web.UI.Page
{

    #region Class Variables
    List<UserSwipe> mSwipeList = null;
    IAppManager mAppManager = null;
    StringBuilder mUserSearchViewDialogScript;

    string lblCheckOut;
    string lblCheckIn;

    #endregion


    #region Public Property
    public DateTime StartDate
    {
        get
        {

            return DateTime.Parse(ViewState["startdate"].ToString());

        }
        set
        {
            ViewState["startdate"] = value;
        }
    }
    #endregion


    #region Internal Members


    #region Iinital View
    private void InitializeView()
    {
        try
        {

            if (!Page.IsPostBack)

                //Assign the start date 
                StartDate = CalendUserSwipe.TodaysDate;

            this.mAppManager = Master.AppManager;
            this.userSwipeEditControl.AppManager = this.mAppManager;
            this.HideCalenderControl(false);
            this.txtuser.Focus();



        }
        catch { throw; }
    }
    #endregion


    #region Fill User Swipes Detail of Current Month

    private void FillUserSwipes()
    {
        IUserSwipeService service = null;
        try
        {

            DateTime fromDate = this.StartDate;
            DateTime toDate = this.RetrieveCurrentDateTime().AddDays(1);

            //Validate
            this.ValidateUserSearch();

            // Create the service.
            service = AppService.Create<IUserSwipeService>();
            service.AppManager = this.mAppManager;

           
            // Call service method.
            mSwipeList = service.Retrieve(Int32.Parse(hdnuserid.Value), fromDate, toDate);


            this.divMessage.InnerText = string.Empty;

            this.HideCalenderControl(true);
            //if(this.divMessage.InnerText.Trim()!="")
            if (Session["ResposeMessage"] != null)
            {
                this.divMessage.Style.Add("display", "block");
                this.divMessage.InnerText = Session["ResposeMessage"].ToString();
                this.Session.Remove("ResposeMessage");
            }
            else
                this.divMessage.Style.Add("display", "none");


        }

        catch (ValidationException ex)
        {

            //display the validation Message.
            StringBuilder message = new StringBuilder("<ul>");
            foreach (object value in ex.Data.Values)
            {
                message.Append(string.Format("<li>{0}</li>", value.ToString()));
            }
            message.Append("</ul>");
            this.divMessage.Style.Add("display", "block");
            this.divMessage.InnerHtml = message.ToString();
            this.HideCalenderControl(false);
            this.lblChooseEmployeeName.Text = "No Record Found";

        }
        catch { throw; }
    }
    #endregion


    #region Hide Controls
    private void HideCalenderControl(bool visible)
    {
        //Hide the Calender Control
        this.CalendUserSwipe.Visible = visible;
        this.spntag.Visible = visible;
        this.gridviewPanel.Visible = visible == true ? false : true;
    }
    #endregion


    #region Validate the User Search
    private void ValidateUserSearch()
    {
        try
        {
            //create a Exception instance.
            ValidationException exception = new ValidationException();
            if (string.IsNullOrEmpty(txtuser.Value) || txtuser.Value.Trim() == "")
                exception.Data.Add("EMPLOYEE_NAME", "Employee Name is a Mandatory Field");

            //Throw the Exception 
            if (exception.Data.Count > 0)
                throw exception;

        }

        catch { throw; }
    }
    #endregion


    #region Initialize  User Search View Dialog
    private void InitializeUserSearchViewDialogScript()
    {
        try
        {
            mUserSearchViewDialogScript = new StringBuilder();
            mUserSearchViewDialogScript.Append("refreshUserSearchView();");
        }
        catch { throw; }

    }
    #endregion


    #region Retrieve Current Date and Time
    private DateTime RetrieveCurrentDateTime()
    {
        SettingProvider provider = null;
        try
        {
            // Create provider and get data.
            provider = new SettingProvider();
            provider.AppManager = this.mAppManager;

            return DateTime.Parse(provider.GetSystemUtcDateTime().ToString("MM/dd/yyyy"));
        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }
    #endregion


    #region Build  Check in Label

    private HtmlGenericControl BuildCheckInLabel(UserSwipe item)
    {
        try
        {
            // Create as span.
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerHtml = string.Format("Check In: {0}", (item.CheckInTime.HasValue) ? item.CheckInTime.Value.ToString("<b>HH:mm tt<br/></b>") : "NE<br/>");

            return span;
        }
        catch { throw; }
    }
    #endregion


    #region Build CheckOut Label
    private HtmlGenericControl BuildCheckOutLabel(UserSwipe item)
    {
        // Create as span.
        HtmlGenericControl span = new HtmlGenericControl("span");
        span.InnerHtml = string.Format("Check Out: {0}", (item.CheckOutTime.HasValue) ? item.CheckOutTime.Value.ToString("<b>HH:mm tt</b>") : "NE");

        return span;
    }
    #endregion


    #region Build CheckIn LinkButton

    private HyperLink BuildCheckInLinkButton(UserSwipe item)
    {
        try
        {
            string buildtext = "<br/>" + lblCheckIn;
            HyperLink link = new HyperLink();
            if (item.CheckInTime.HasValue)
                buildtext += item.CheckInTime.Value.ToString("<b>hh:mm tt</br></b>");
            else
                buildtext += "<b>NE</br></b>";
            link.Text = buildtext;
            string arguments = string.Format("{{'Action': 'CheckIn', 'WorkDate': '{0}', 'Id': '{1}'}}", item.WorkDate.ToString("MM/dd/yyyy"), item.Id);
            link.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(this.CalendarLinkButton1, arguments, false);

            return link;
        }
        catch { throw; }
    }
    #endregion

    private HtmlGenericControl BuildTimeZoneShortName(UserSwipe item)
    {
        try
        {
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerText = item.CustomData["ShortName"].ToString();
            return span;
        }
        catch
        {
            throw;
        }
    }


    #region Build CheckOut LinkButton

    private HyperLink BuildCheckOutLinkButton(UserSwipe item)
    {
        try
        {

            string buildtext = "<br/>" + " " +lblCheckOut;

            //Create a HyperLink instance.
            HyperLink link = new HyperLink();
            if (item.CheckOutTime.HasValue)
                buildtext += item.CheckOutTime.Value.ToString("<b>hh:mm tt</br></b>");
            else
                buildtext += "<b>NE</br></b>";
            link.Text = buildtext;

            //Build the argument in json format.
            string arguments = string.Format("{{'Action': 'CheckOut', 'WorkDate': '{0}', 'Id': '{1}'}}", item.WorkDate.ToString("MM/dd/yyyy"), item.Id);

            //Pass the argument in json format.
            link.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(this.CalendarLinkButton1, arguments, false);

            //return the link.
            return link;
        }
        catch { throw; }
    }
    #endregion

    #endregion


    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
                this.txtuser.Value = "";

                //this.txtuser.Value = "Press F2 to search users";

            LoadLabels();

            this.mAppManager = Master.AppManager;
            this.UserSearchView.AppManager = this.mAppManager;
          
            this.InitializeUserSearchViewDialogScript();
            this.InitializeView();


        }
        catch
        {
            throw;
        }

    }

    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;
        try
        {
            // Current exception.
            Exception exception = HttpContext.Current.Error;

            // Insert error log.
            provider = new ErrorLogProvider();
            provider.AppManager = this.mAppManager;
            provider.Insert(exception);
        }
        catch
        {
            throw;
        }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtuser.Value = string.Empty;
        }
        catch
        {
            throw;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Register scripts.
            if (this.mUserSearchViewDialogScript != null)
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    System.Guid.NewGuid().ToString(),
                    this.mUserSearchViewDialogScript.ToString(),
                    true);

            if (hdnuserid.Value != "")
                this.FillUserSwipes();
        }
        catch { throw; }
    }
    protected void CalendUserSwipe_DayRender(object sender, DayRenderEventArgs e)
    {



        try
        {
            bool IsParentuser = true;
            List<UserSwipe> uslist = new List<UserSwipe>();
            //calender date.
            DateTime calendarDate = DateTime.Parse(e.Day.Date.ToString("MM/dd/yyyy"));

            //get the Current Date.
            DateTime CurrentDate = DateTime.Parse(this.RetrieveCurrentDateTime().AddDays(1).ToString("MM/dd/yyyy"));
            string DTdate = null;
            UserSwipe userswipe = null;

            IUserService mUserService = null;
            List<Tks.Entities.User> LstUser = null;
            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = this.mAppManager;
            LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(mAppManager.LoginUser.Id));
            if (LstUser != null)
            {
                // Filter only active location.
                IEnumerable<User> list = from item in LstUser
                                         where item.Id.ToString() == hdnuserid.Value.ToString()
                                         select item;
                LstUser = list.ToList<User>();
                if (LstUser.Count == 0)
                {
                    IsParentuser = false;
                }
                else
                {
                    IsParentuser = true;
                }
            }

            // Check whether user swipe is available for the given calendar date.
            if (mSwipeList != null)
            {

                //if (mSwipeList != null)
                //{
                //    // Filter only active location.
                //    IEnumerable<UserSwipe> list = from item in mSwipeList
                //                                  where item.CustomData["Parentuserid"].ToString() == mAppManager.LoginUser.Id.ToString()
                //                                            && item.CustomData["ActiveStatus"].ToString() == "InActive"
                //                                  // && item.CustomData["ShortName"].ToString() == "det"
                //                                  select item;
                //    uslist = list.ToList<UserSwipe>();


                //    if (uslist.Count > 0)
                //    {
                //        //mSwipeList = uslist;
                //        DTdate = list.ElementAtOrDefault(0).CustomData["DetachDate"].ToString();
                //    }

                //    // Filter only active location.
                //    //IEnumerable<UserSwipe> list2 = from item in mSwipeList
                //    //                              where item.CustomData["Parentuserid"].ToString() == mAppManager.LoginUser.Id.ToString()
                //    //                                        && item.CustomData["ActiveStatus"].ToString() == "InActive"
                //    //                                        select item;
                //    if (DTdate != null)
                //    {
                //        IEnumerable<UserSwipe> list2 = from item in mSwipeList
                //                                       where item.CustomData["ActiveStatus"].ToString() == "InActive" && item.WorkDate <= DateTime.Parse(DTdate)
                //                                       select item;
                //        uslist = list2.ToList<UserSwipe>();

                //        if (uslist.Count > 0)
                //        {
                //            mSwipeList = uslist;
                //        }
                //    }

                //}

                userswipe = mSwipeList.Where(
                  x => x.WorkDate.ToString("MM/dd/yyyy") == calendarDate.ToString("MM/dd/yyyy"))
                  .FirstOrDefault();
            }
            // If available then display the check in and check out time span.
            HtmlGenericControl container = new HtmlGenericControl("div");
            //If  not available the dispaly the the Link
            if (userswipe == null && calendarDate <= CurrentDate)
            {
                if (e.Day.IsOtherMonth)
                {
                    e.Cell.Text = "";
                    //else if (e.Day.IsWeekend)
                    //{
                    //    e.Cell.Text = "";
                    //    container.InnerText = "NE";
                    //    e.Cell.Controls.Add(container);
                    //}
                }
                else if (Convert.ToDateTime(DTdate) < Convert.ToDateTime(calendarDate) && DTdate != null)
                {
                    //create dummy userswipe instance.
                    UserSwipe dummy = new UserSwipe(0);
                    dummy.WorkDate = calendarDate;
                }

                else
                {
                    //create dummy userswipe instance.
                    UserSwipe dummy = new UserSwipe(0);
                    dummy.WorkDate = calendarDate;

                    container.Controls.Add(this.BuildCheckInLinkButton(dummy));
                    container.Controls.Add(this.BuildCheckOutLinkButton(dummy));
                }

            }
            else if (userswipe != null && calendarDate <= CurrentDate)
            {
                container.Controls.Add(this.BuildTimeZoneShortName(userswipe));
                container.Controls.Add(this.BuildCheckInLinkButton(userswipe));
                container.Controls.Add(this.BuildCheckOutLinkButton(userswipe));

            }
            else
            {
                if (e.Day.IsOtherMonth)
                    e.Cell.Text = "";

            }


            e.Cell.Controls.Add(container);

        }
        catch
        {
            throw;
        }


    }

    protected void CalendarLinkButton1_CalendarClick(object sender, CalendarClickEventArgs e)
    {
        try
        {
            // Deserialize.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> swipeInfo = serializer.DeserializeObject(e.DataKey.ToString()) as Dictionary<string, object>;

            // Fetch values.
            string action = swipeInfo["Action"].ToString();
            int id = Int32.Parse(swipeInfo["Id"].ToString());
            DateTime workDate = DateTime.Parse(swipeInfo["WorkDate"].ToString());
            this.FillUserSwipes();
            UserSwipe userSwipe = null;

            if (mSwipeList != null)
                // Check whether selected item is available in the list.
                userSwipe = mSwipeList.Where(item => item.Id == id && item.WorkDate.ToString("MM/dd/yyyy") == workDate.ToString("MM/dd/yyyy")).FirstOrDefault();

            // Exists
            if (userSwipe == null)
            {
                userSwipe = new UserSwipe(id);
                userSwipe.UserId = Int32.Parse(this.hdnuserid.Value);
                userSwipe.WorkDate = workDate;
                userSwipe.CreateDate = DateTime.Now;
                userSwipe.LastUpdateDate = DateTime.Now;
            }

            // Display edit panel as dialog.
            this.userSwipeEditControl.AppManager = this.mAppManager;
            this.userSwipeEditControl.Action = action;
            this.userSwipeEditControl.UserSwipe = userSwipe;

            this.userSwipeEditControl.UserType = "Sup";
            this.userSwipeEditControl.InitializeView();


            // Register startup script.
            string script = string.Format("showEditPanelDialog({{'title': '{0}','width':'{1}'}})", action.ToString(), "400px");
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                System.Guid.NewGuid().ToString(),
                script,
                true);
        }
        catch
        {
            throw;
        }

    }
    protected void ibtSearchUser_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.UserSearchView.ViewName = "HieraricalSearch";
            this.UserSearchView.Display();
            this.mUserSearchViewDialogScript.Append("showUserSearchView();");
        }
        catch { throw; }
    }
    protected void btnSearchUser_Click(object sender, EventArgs e)
    {
        try
        {
            //Retrive the user swipe based on userid and month
            StartDate = CalendUserSwipe.TodaysDate;
            if(hdnuserid.Value!="")
            this.FillUserSwipes();
            else
                this.lblChooseEmployeeName.Text = "No Record Found";

        }
        catch { throw; }
    }

    protected void CalendUserSwipe_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {
            StartDate = e.NewDate;
            this.FillUserSwipes();
        }
        catch { throw; }
    }
    #endregion

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "TIMESHEET");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.Equals("lblCheckout")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            lblCheckOut = Convert.ToString(GRID_TITLE.DisplayText);
            lblCheckIn = Convert.ToString(GRID_TITLE.SupportingText1);
        }

    }
    protected void btnClear_Click1(object sender, EventArgs e)
    {
//        this.txtuser.Value = "Press F2 to search users";
        this.txtuser.Value = "";
        this.hdnuserid.Value = string.Empty;
        this.HideCalenderControl(false);
        //this.tdMessageNotFound.InnerText = lblChooseEmployeeName.Text;
    }
}