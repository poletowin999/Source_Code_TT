using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;

using CalendarButton;
using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Activities_UserSwipeView : System.Web.UI.Page
{

    #region Class Variable

    List<UserSwipe> mSwipeList = null;
    IAppManager mAppManager = null;
    string mEntityEditPanelHeader = string.Empty;
    DateTime menddate;

    string lblCheckOut;
    string lblCheckIn;

    string lblWarning;
    string lblCheckinFirst;

    
    

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


    #region Internal members

    private void InitializeView()
    {
        try
        {
            if (!Page.IsPostBack)
                //Reterive the user swipe details of current month
                StartDate = CalUserSwipe.TodaysDate;


            //Initial view
            this.mAppManager = Master.AppManager;
            this.userSwipeEditControl.AppManager = this.mAppManager;
            this.FillUserSwipes();
            LoadLabels();
           
                
              
            
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "TIMESHEET");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.Equals("lblCheckout")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            lblCheckOut = Convert.ToString(GRID_TITLE.DisplayText);
            lblCheckIn = Convert.ToString(GRID_TITLE.SupportingText1);
        }

        var CHECKINOUTALERT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLLISTRESETACT")).FirstOrDefault();
        if (CHECKINOUTALERT != null)
        {

            lblWarning = Convert.ToString(CHECKINOUTALERT.DisplayText);
            //lblCheckinFirst = Convert.ToString(CHECKINOUTALERT.SupportingText1);
        }
    }

    private void FillUserSwipes()
    {
        IUserSwipeService service = null;
        try
        {

            DateTime fromDate = StartDate;
            
            DateTime toDate = this.RetrieveCurrentDateTime();

            // Create the service.
            service = AppService.Create<IUserSwipeService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            mSwipeList = service.Retrieve(this.mAppManager.LoginUser.Id, fromDate, toDate);

        }
        catch { throw; }
    }

    private DateTime RetrieveCurrentDateTime()
    {
        SettingProvider provider = null;
        try
        {
            // Create provider and get data.
            provider = new SettingProvider();
            provider.AppManager = this.mAppManager;

            return provider.GetSystemUtcDateTime();
        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    private HtmlGenericControl BuildCheckInLabel(UserSwipe item)
    {
        try
        {
            // Create as span.
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.InnerHtml = string.Format("<br/>" + lblCheckIn + " {0}", (item.CheckInTime.HasValue) ? item.CheckInTime.Value.ToString("<b>hh:mm tt</b>") : "NE");

            return span;
        }
        catch { throw; }
    }

    private HtmlGenericControl BuildCheckOutLabel(UserSwipe item)
    {
        // Create as span.
        HtmlGenericControl span = new HtmlGenericControl("span");
        span.InnerHtml = string.Format("<br/>" + lblCheckOut + " {0}", (item.CheckOutTime.HasValue) ? item.CheckOutTime.Value.ToString("<b>hh:mm tt</b>") : "NE");

        return span;
    }

    private HyperLink BuildCheckInLinkButton(UserSwipe item)
    {
        try
        {
            //Create a hyperlink 
            HyperLink link = new HyperLink();
            link.Text = "<br/>" + lblCheckIn;

            //pass the parameter as  json
            string arguments = string.Format("{{'Action': 'CheckIn', 'WorkDate': '{0}', 'Id': '{1}'}}", item.WorkDate.ToString("MM/dd/yyyy"), item.Id);
            link.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(this.CalendarLinkButton1, arguments, false);

            //return the link.
            return link;

        }
        catch { throw; }
    }

    private HyperLink BuildCheckOutLinkButton(UserSwipe item)
    {
        try
        {
            //create a hyperlink
            HyperLink link = new HyperLink();
            // Mohan
            link.Text = "<br/>" + lblCheckOut;

            //pass the parameter as json
            string arguments = string.Format("{{'Action': 'CheckOut', 'WorkDate': '{0}', 'Id': '{1}'}}", item.WorkDate.ToString("MM/dd/yyyy"), item.Id);
            link.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(this.CalendarLinkButton1, arguments, false);

            //return the link
            return link;

        }
        catch { throw; }
    }

    private HtmlGenericControl BuildTimeZoneShortName(UserSwipe item)
    {
        HtmlGenericControl span = new HtmlGenericControl("span");
        span.InnerHtml = item.CustomData["ShortName"].ToString();
        return span;
    }

   



    private void DisplayEntityEditPanel()
    {
        try
        {
            // Display edit panel.
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                System.Guid.NewGuid().ToString(),
                string.Format("showEditPanelDialog({{'width': '550px', 'title': '{0}'}});", mEntityEditPanelHeader),
                true);

        }
        catch { throw; }
    }
    #endregion


    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        //testing
       // DateTime dt = CalUserSwipe.SelectMonthText;

      
        try
        {
            this.InitializeView();
           
            //
        }
        catch { throw; }

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
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    protected  void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            this.FillUserSwipes();
        }
        catch { throw; }
    }
    protected void CalUserSwipe_DayRender(object sender, DayRenderEventArgs e)
    {

        try
        {
           
            DateTime calendarDate =DateTime.Parse( e.Day.Date.ToString("MM/dd/yyyy"));
            //System.Diagnostics.Debug.Assert(calendarDate.ToString("MM/dd/yyyy") != "09/23/2011");

            //  Remove time span.
            DateTime currentDate = DateTime.Parse(this.RetrieveCurrentDateTime().ToString("MM/dd/yyyy"));
            UserSwipe userswipe = null;
            // Check whether user swipe is available for the given calendar date.
            if(mSwipeList!=null)
              userswipe= mSwipeList.Where(
                x => x.WorkDate.ToString("MM/dd/yyyy") == calendarDate.ToString("MM/dd/yyyy"))
                .FirstOrDefault();
            
            // If available then display the check in and check out time span.
            HtmlGenericControl container = new HtmlGenericControl("div");
            //if (e.Day.Date.DayOfWeek.ToString().Equals("Sunday"))
            //{
            //    container.InnerText = "NE";
            //    e.Cell.Controls.Add(container);
            //    return;
            //}

            if (userswipe != null)
            {
               
                 if (userswipe.WorkDate.ToString("MM/dd/yyyy") == currentDate.ToString("MM/dd/yyyy"))
                {
                    // Calendar date is current date then display as text/link.
                    if (userswipe.CheckInTime.HasValue)
                    {
                        container.Controls.Add(this.BuildTimeZoneShortName(userswipe));
                        container.Controls.Add(this.BuildCheckInLabel(userswipe));
                        
                    }
                    else
                        container.Controls.Add(this.BuildCheckInLinkButton(userswipe));

                    if (userswipe.CheckOutTime.HasValue)
                        container.Controls.Add(this.BuildCheckOutLabel(userswipe));
                    else
                        container.Controls.Add(this.BuildCheckOutLinkButton(userswipe));
                     
                     

                }
                 else if (calendarDate.ToString("MM/dd/yyyy") == currentDate.AddDays(1).ToString("MM/dd/yyy"))
                {
                    if (userswipe.CheckInTime.HasValue)
                    {
                        container.Controls.Add(this.BuildTimeZoneShortName(userswipe));
                        container.Controls.Add(this.BuildCheckInLabel(userswipe));
                        
                    }
                    else
                        container.Controls.Add(this.BuildCheckInLinkButton(userswipe));
                    if (userswipe.CheckOutTime.HasValue)
                        container.Controls.Add(this.BuildCheckOutLabel(userswipe));
                    else
                        container.Controls.Add(this.BuildCheckOutLinkButton(userswipe));
                        
                }
                 else if (calendarDate.ToString("MM/dd/yyyy") == currentDate.AddDays(-1).ToString("MM/dd/yyyy"))
                {
                    if (userswipe.CheckInTime.HasValue)
                    {
                        container.Controls.Add(this.BuildTimeZoneShortName(userswipe));
                        container.Controls.Add(this.BuildCheckInLabel(userswipe));
                        
                    }
                    else
                        container.Controls.Add(this.BuildCheckInLinkButton(userswipe));
                    if (userswipe.CheckOutTime.HasValue)
                        container.Controls.Add(this.BuildCheckOutLabel(userswipe));
                    else
                        container.Controls.Add(this.BuildCheckOutLinkButton(userswipe));
                }
                else  if (userswipe.WorkDate < currentDate)
                {
                    // Calendar date less than current date then display as text.
                    container.Controls.Add(this.BuildTimeZoneShortName(userswipe));
                    container.Controls.Add(this.BuildCheckInLabel(userswipe));
                    container.Controls.Add(this.BuildCheckOutLabel(userswipe));
                   
                }


            }
            else if (userswipe == null)
            {
                if (e.Day.IsOtherMonth)
                {
                    e.Cell.Text = string.Empty;
                }
    
                else
                {
                    // Create dummy instance.
                    UserSwipe dummy = new UserSwipe(0);

                    dummy.WorkDate = calendarDate;

                 

                    // If calendar day is today and user swipe is not available then provide links to check in and check out.
                    if (calendarDate.ToString("MM/dd/yyyy") == currentDate.ToString("MM/dd/yyyy"))
                    {
                        // Display as link.
                        container.Controls.Add(this.BuildCheckInLinkButton(dummy));
                        container.Controls.Add(this.BuildCheckOutLinkButton(dummy));
                    }
                    else if (calendarDate.ToString("MM/dd/yyyy") == currentDate.AddDays(1).ToString("MM/dd/yyyy") || calendarDate.ToString("MM/dd/yyyy") == currentDate.AddDays(-1).ToString("MM/dd/yyyy"))
                    {
                        container.Controls.Add(this.BuildCheckInLinkButton(dummy));
                        container.Controls.Add(this.BuildCheckOutLinkButton(dummy));
                    }
                    // If not available then display the check in and check out time as NE (Not Entered).
                    else
                    {
                        if (calendarDate < currentDate)
                        {
                            // Display as text
                            container.Controls.Add(this.BuildCheckInLabel(dummy));
                            container.Controls.Add(this.BuildCheckOutLabel(dummy));
                        }
                    }
                }
            }
           
               
           

            // Add container to calendar day cell.
            e.Cell.Controls.Add(container);
        }
        catch { throw; }
    }

    protected void CalendarLinkButton1_CalendarClick(object sender, CalendarClickEventArgs e)
    {
        try
        {
            // Deserialize.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> swipeInfo = serializer.DeserializeObject(e.DataKey.ToString()) as Dictionary<string, object>;

              DateTime currentDate = DateTime.Parse(this.RetrieveCurrentDateTime().ToString("MM/dd/yyyy"));
            // Fetch values.
            string action = swipeInfo["Action"].ToString();
            int id = Int32.Parse(swipeInfo["Id"].ToString());
            DateTime workDate = DateTime.Parse(swipeInfo["WorkDate"].ToString());
            UserSwipe userSwipe=null;
            // Check whether selected item is available in the list.
            if(mSwipeList!=null)
                userSwipe = mSwipeList.Where(item => item.Id == id && item.WorkDate.ToString("MM/dd/yyyy") == workDate.ToString("MM/dd/yyyy")).FirstOrDefault();

            // Exists
            if (userSwipe == null)
            {
                userSwipe = new UserSwipe(id);
                userSwipe.UserId = this.mAppManager.LoginUser.Id;
                userSwipe.WorkDate = workDate;
                userSwipe.CreateDate = DateTime.Now;
                userSwipe.LastUpdateDate = DateTime.Now;
            }

            
            // Display edit panel as dialog.
            this.userSwipeEditControl.AppManager = this.mAppManager;
            this.userSwipeEditControl.Action = action;
            this.userSwipeEditControl.UserSwipe = userSwipe;

            if (mSwipeList != null)
            {
                UserSwipe userSwipe1 = null;
                //last swipe date 
                userSwipe1 = mSwipeList.OrderByDescending(x => x.WorkDate).FirstOrDefault();
                if (userSwipe1.CheckInTime != null && userSwipe1.CheckOutTime == null && userSwipe1.CheckInTime.Value < userSwipe.WorkDate)
                    this.userSwipeEditControl.WarningMessage = lblWarning + userSwipe1.WorkDate.ToString("MM/dd/yyyy");
                else
                    this.userSwipeEditControl.WarningMessage = string.Empty;
            }
            else
            {
                this.userSwipeEditControl.WarningMessage = string.Empty;
            }
               
            
            
            this.userSwipeEditControl.UserType = "Self";
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



    protected void CalUserSwipe_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        try
        {

            StartDate= e.NewDate;
            menddate = e.PreviousDate;
            this.FillUserSwipes();
        }
        catch
        {
            throw;
        }

    }
    #endregion

}