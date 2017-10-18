using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;


using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Activities_UserSwipeEditControl : System.Web.UI.UserControl
{

    #region Class Variables
    IAppManager mappmanager = null;
    string CommandProcess = string.Empty;
    List<Tks.Entities.TimeZone> mTimeZones = null;
    DateTime currentDateTime;

    string lblCheckinFirst;

    #endregion


    #region Public members
    public IAppManager AppManager { get; set; }

    public string Action
    {
        get
        {

            return ViewState["Action"] != null ? ViewState["Action"].ToString() : string.Empty;
        }
        set
        {
            ViewState["Action"] = value;
        }

    }


    public UserSwipe UserSwipe
    {
        get
        {


            return Session["SwipeUser"] != null ? Session["SwipeUser"] as UserSwipe : null;
        }
        set
        {
            Session["SwipeUser"] = value;
        }

    }

    public string UserType
    {
        get
        {
            return ViewState["UserType"] != null ? ViewState["UserType"].ToString() : string.Empty;
        }
        set
        {
            ViewState["UserType"] = value;
        }
    }

    public string WarningMessage
    {
        get
        {
            return ViewState["WrnMessage"] != "" ? ViewState["WrnMessage"].ToString() : string.Empty;
        }
        set
        {
            ViewState["WrnMessage"] = value;
        }
    }

    public void InitializeView()
    {
        LoadLabels();
        // File time zone list.
        this.FillTimeZoneList();

        // Fill the shift list.
        this.FillUserShift();

        this.FillUserWorktypes();

        // Display system current time.
        this.FillCurrentTime();

        //Display the Controls
        DisplayControls();

    }
    private void DisplayControls()
    {

        this.tbxreason.InnerText = string.Empty;
        SwipePanel.Visible = true;
        btndisplay.Visible = false;
        this.btnDelete.Visible = false;
        this.tblswipedetail.Visible = true;
        popupClickOk.Visible = false;
        this.chkRemove.Visible = false;
        this.btnOk.Visible = true;
        
       //Check the action
        if (Action.Equals("CheckIn", StringComparison.InvariantCultureIgnoreCase))
        {

            //check usertype

            if (UserType.Equals("Self", StringComparison.InvariantCultureIgnoreCase))
            {
                this.txtSwipeTime.Visible = false;
                this.LblSwipeTime.Visible = false;
                this.ddlTimeZoneList.Visible = true;
                this.ddlTimeZoneList.Disabled = false;
                this.ddlShift.Enabled = true;
                this.ddWFH.Enabled = true;
                this.ddlShift.SelectedItem.Text = "--Select--";
                this.LabTimeZone.Visible = false;
               
                reasonGridView.Visible = false;
                if (WarningMessage != "")
                {
                    DisplayWarningMsg(WarningMessage);
                }
                
            }
            else
            {
                if (UserType != "Self" && UserSwipe.Id != 0)
                {
                    if (this.UserSwipe.CheckInTime.HasValue)
                    {
                        this.ddlTimeSpan.SelectedValue = UserSwipe.CheckInTime.Value.ToString("tt");
                        this.txtSwipeTime.Text = UserSwipe.CheckInTime.Value.ToString("hh:mm");
                        this.txtSwipeDate.Text = UserSwipe.CheckInTime.Value.ToString("MM/dd/yyyy");
                    }
                   
                    this.ddlTimeZoneList.Visible = true;
                    this.LabTimeZone.Visible = false;
                    this.ddlTimeZoneList.Disabled = false;
                    ddlTimeZoneList.Items.FindByValue(UserSwipe.TimeZoneId.ToString()).Selected = true;
                    ddlShift.SelectedValue = UserSwipe.Shift.ToString();
                    ddWFH.SelectedValue = UserSwipe.UserworktypeId.ToString();
                   // ddlShift.Items.FindByValue(UserSwipe.Shift.ToString()).Selected = true;
                    this.txtSwipeTime.Visible = true;
                    this.LblSwipeTime.Visible = true;
                    reasonGridView.Visible = true;
                    this.ddlShift.Enabled = true;
                    this.ddWFH.Enabled = true;
                    this.chkRemove.Visible =true;
                    this.chkRemove.Checked = false;
                    this.btnOk.Visible = true;
                    
                    

                }

                else
                {
                    this.ddlTimeZoneList.Visible = true;
                    this.ddlTimeZoneList.Disabled = false;
                    this.LabTimeZone.Visible = false;
                    this.txtSwipeTime.Visible = true;
                    this.LblSwipeTime.Visible = true;
                    reasonGridView.Visible = true;
                    if (ddlTimeZoneList.Items.Count > 0)
                    ddlTimeZoneList.Items.FindByValue("0").Selected = true;
                    this.ddlShift.Enabled = true;
                    this.ddWFH.Enabled = true;
                    this.ddlShift.SelectedItem.Text = "--Select--";
                  

                }
            }

        }
        else if (Action.Equals("CheckOut", StringComparison.InvariantCultureIgnoreCase) && UserSwipe.Id != 0)
        {
            
            this.ddlTimeZoneList.Visible = false;
            this.LabTimeZone.Visible = true;
            this.LblSwipeTime.Visible = false;
            this.txtSwipeTime.Visible = false;
            this.ddlShift.Enabled = false;
            this.ddWFH.Enabled = false;
            this.LabTimeZone.InnerText = mTimeZones.Where(x => x.Id == UserSwipe.TimeZoneId).FirstOrDefault().Name.ToString();
            this.ddlShift.SelectedValue = UserSwipe.Shift.ToString();
            this.ddWFH.SelectedValue = UserSwipe.UserworktypeId.ToString();
            
          //  this.ddlShift.Items.FindByValue(UserSwipe.Shift.ToString()).Selected=true;
            if (UserType != "Self")
            {

                this.ddlTimeSpan.Visible = true;
               
                if (UserSwipe.CheckOutTime.HasValue)
                {
                    this.txtSwipeTime.Text = UserSwipe.CheckOutTime.Value.ToString("hh:mm");
                    this.ddlTimeSpan.SelectedValue = UserSwipe.CheckOutTime.Value.ToString("tt");
                    this.txtSwipeDate.Text = UserSwipe.CheckOutTime.Value.ToString("MM/dd/yyyy");
                }
                else if (UserSwipe.CheckInTime.HasValue)
                {
                    this.txtSwipeDate.Text = UserSwipe.CheckInTime.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    this.txtSwipeTime.Text = currentDateTime.ToString("hh:mm");
                }

                this.LblSwipeTime.Visible = true;
                this.txtSwipeTime.Visible = true;
                this.chkRemove.Visible = true;
                this.chkRemove.Checked = false;
                this.btnOk.Visible = true;

            }
            else
            {
                reasonGridView.Visible = false;
                this.txtSwipeTime.Visible = false;
                this.LblSwipeTime.Visible = false;
            }


        }
        else
        {
            
            //this.ddlTimeZoneList.Visible = true;
            //this.ddlTimeZoneList.Disabled = true;
            //this.LabTimeZone.Visible = false;
            //this.ddlShift.Enabled = false;
            //this.lblreason.Style.Add("display", "none");
            //this.tbxreason.Visible = false;
            //this.txtSwipeTime.Visible = false;
            //this.LblSwipeTime.Visible = false;
            SwipePanel.Visible = false;
            popupClickOk.Visible = true;


            MessagePanel.InnerHtml = lblCheckinFirst;

        }

    }
    #endregion


    #region Internal Members

    private void FillTimeZoneList()
    {
        ITimeZoneService service = null;
        try
        {
            // Create the service.
            service = AppService.Create<ITimeZoneService>();
            service.AppManager = this.AppManager;

            // Call method.
            mTimeZones = service.RetrieveAll();
            // Filter active time zones.
            IEnumerable<Tks.Entities.TimeZone> validTimeZones = from item in mTimeZones
                                                                where item.IsActive = true 
                                                                select item;

            // Fill in control.
            this.ddlTimeZoneList.Items.Clear();
            this.ddlTimeZoneList.DataTextField = "Name";
            this.ddlTimeZoneList.DataValueField = "Id";
            this.ddlTimeZoneList.DataSource = validTimeZones;
            this.ddlTimeZoneList.DataBind();

            // Add default item.
            this.ddlTimeZoneList.Items.Insert(0,new ListItem("-- Select --", "0"));
            

            // Select first item as default.
            //if (this.ddlTimeZoneList.Items.Count > 0) this.ddlTimeZoneList.Items.FindByValue("0").Selected=true;
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void FillUserShift()
    {
        IUserService service = null;
        try
        {
            //create a service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.AppManager;

            //Invoke the method.
            List<UserShift> LstShift = service.RetrieveUserShift();
            this.ddlShift.DataTextField = "Name";
            this.ddlShift.DataValueField = "Id";
            this.ddlShift.DataSource = LstShift;
            this.ddlShift.DataBind();

            //add default item.
            this.ddlShift.Items.Insert(0, new ListItem("--Select--", "0"));
            
            //select the default item
            if (this.ddlShift.Items.Count > 0) this.ddlShift.Items[0].Selected = true;


        }
        catch
        {
            throw;
        }
        finally
        {
            if (service != null) { service.Dispose(); }
        }
    }

    private void FillUserWorktypes()
    {
        IUserService service = null;
        try
        {
            //create a service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.AppManager;

            //Invoke the method.
            List<UserShift> LstShift = service.RetrieveUserWorkTypes();
            this.ddWFH.DataTextField = "Name";
            this.ddWFH.DataValueField = "Id";
            this.ddWFH.DataSource = LstShift;
            this.ddWFH.DataBind();

            //add default item.
          //  this.ddWFH.Items.Insert(0, new ListItem("--Select--", "0"));

            //select the default item
            if (this.ddWFH.Items.Count > 0) this.ddWFH.Items[0].Selected = true;


        }
        catch
        {
            throw;
        }
        finally
        {
            if (service != null) { service.Dispose(); }
        }
    }
    private void ValidateCheckInTime()
    {
        try
        {
            //create a Exception Instances
            ValidationException exception = new ValidationException();
          
                if (ddlTimeZoneList.SelectedIndex == 0)
                    exception.Data.Add("CHECK_IN_TIMEZONE", "Please Choose the TimeZones.");

                if (ddlShift.SelectedIndex == 0)
                    exception.Data.Add("CHECK_IN_SHIFT", "Please Choose the Shift.");
              

                if (UserType != "Self")
                {
                    if (txtSwipeDate.Text.Trim() == "" || string.IsNullOrEmpty(txtSwipeDate.Text))
                        exception.Data.Add("CHECK_IN_SWIPEDATE", "WorkDate is a Mandatory Field");



                    if (txtSwipeTime.Text.Trim() == "" || string.IsNullOrEmpty(txtSwipeTime.Text))
                        exception.Data.Add("CHECK_IN_SWIPETIME", "Time is a Mandatory Field");
                    else
                    {
                        string[] srgs = txtSwipeTime.Text.Split(':');
                        if (Int32.Parse(srgs[0].ToString()) > 12)
                            exception.Data.Add("CHECK_IN_SWIPETIME", "Please Enter the  valid 12 hours  Time Format.");
                        else if (Int32.Parse(srgs[1].ToString()) >= 60)
                            exception.Data.Add("CHECK_IN_SWIPETIME", "Please Enter the valid 12 hours  Time Format.");
                    }
                      
                    if (UserSwipe.Id != 0)
                    {
                        if (tbxreason.InnerText.Trim() == "" || string.IsNullOrEmpty(tbxreason.InnerText))
                            exception.Data.Add("CHECK_IN_REASON", "Reason is a Mandatory Field");

                    }
                }
                  
                // Throw exception.
                if (exception.Data.Count > 0)
                    throw exception;
            
        }
        catch
        {
            throw;
        }
    }

    //validte the checkInTime.
    private void ValidateCheckInTime(DateTime checkInTime, UserSwipe swipe)
    {
        try
        {
            //create a Exception Instances
            ValidationException exception = new ValidationException();
            if (UserType != "Self")
            {
                //if (checkInTime.ToString("MM/dd/yyyy") != swipe.WorkDate.ToString("MM/dd/yyyy"))
                //    exception.Data.Add("CHECK_IN_TIME", "Check-In date should be in work date.");

                //else if (swipe.CheckOutTime.HasValue)
                if(swipe.CheckOutTime.HasValue)
                {
                    if (checkInTime > swipe.CheckOutTime.Value)
                        exception.Data.Add("CHECK_IN_TIME", "Check-In time should be earlier than Check-Out time.");
                }
               
            }
            

            // Throw exception.
            if (exception.Data.Count > 0)
                throw exception;
        }
        catch { throw; }


    }
    private void ValidateCheckOutTime()
    {
        try
        {
            ValidationException exception = new ValidationException();
            if (this.UserType != "Self")
            {
                //create a Exception Instances.
               
                if (txtSwipeDate.Text.Trim() == "" || string.IsNullOrEmpty(txtSwipeDate.Text))
                    exception.Data.Add("CHECK_OUT_SWIPEDATE", "Workdate is a Mandatory Field");
                if (txtSwipeTime.Text.Trim() == "" || string.IsNullOrEmpty(txtSwipeTime.Text))
                    exception.Data.Add("CHECK_OUT_SWIPETIME", "Time is  a Mandatory Field");
                else
                {
                    string[] srgs = txtSwipeTime.Text.Split(':');
                    if (Int32.Parse(srgs[0].ToString()) > 12)
                        exception.Data.Add("CHECK_IN_SWIPETIME", "Please Enter the  valid 12 hours  Time Format.");
                    else if (Int32.Parse(srgs[1].ToString()) >= 60)
                        exception.Data.Add("CHECK_IN_SWIPETIME", "Please Enter the valid 12 hours  Time Format.");
                }
                if (UserSwipe.Id != 0 && UserSwipe.CheckOutTime!=null)
                {
                    if (tbxreason.InnerText.Trim() == "" || string.IsNullOrEmpty(tbxreason.InnerText))
                        exception.Data.Add("CHECK_OUT_REASON", "Reason is a Mandatory Field");
                }
            }
            // Throw exception.
            if (exception.Data.Count > 0)
                throw exception;
            
        }
        catch
        {
            throw;
        }

    }

    //validate the checkoutTime.
    private void ValidateCheckOutTime(DateTime CheckOutTime, UserSwipe swipe)
    {
        try
        {
            //create a Exception Instances.
            ValidationException exception = new ValidationException();
            if (this.UserType != "Self")
            {
                //if (CheckOutTime.ToString("MM/dd/yyyy") != swipe.WorkDate.ToString("MM/dd/yyyy") && CheckOutTime.ToString("MM/dd/yyyy") != swipe.WorkDate.AddDays(1).ToString("MM/dd/yyyy") )
                //    exception.Data.Add("CHECK_OUT_DATE", "Check-Out date should be in work date or Tommorrow date.");
                
                //else if (swipe.CheckInTime.HasValue)
                if(swipe.CheckInTime.HasValue)
                {
                    if (CheckOutTime < swipe.CheckInTime.Value)

                        exception.Data.Add("CHECK_OUT_DATE", "Check-Out  time should be after then Check-In Time");
                }
                
            }

            //Throw exception.
            if (exception.Data.Count > 0)
                throw exception;
        }
        catch
        {
            throw;
        }
    }

    

    private void FillCurrentTime()
    {
        this.divmessage.InnerText = string.Empty;
        this.divmessage.Style.Add("display", "none");
        SettingProvider provider = null;
        try
        {
            // Create provider and get data.
            provider = new SettingProvider();
            provider.AppManager = this.AppManager;

            currentDateTime = provider.GetSystemUtcDateTime();

            // Display time in 12 hour format.
            // Display time span.
            

            // Enable/disable based on user type.
            
            if (this.UserType.Equals("Self", StringComparison.InvariantCultureIgnoreCase))
            {
                this.txtSwipeTime.Enabled = false;
                this.spnSwipeDate.InnerHtml = this.UserSwipe.WorkDate.ToString("MM/dd/yyyy");
                this.txtSwipeTime.Text = currentDateTime.ToString("hh:mm tt");
                this.ddlTimeSpan.Visible = false;
                this.spnSwipeDate.Visible = true;
                this.txtSwipeDate.Visible = false;
            }
            else
            {
                // Allow manager to edit the time span.
                this.txtSwipeTime.Enabled = true;
                this.spnSwipeDate.Visible = false;
                this.txtSwipeDate.Visible = true;
                this.ddlTimeSpan.Visible = true;
                this.txtSwipeDate.Text = this.UserSwipe.WorkDate.ToString("MM/dd/yyyy");
                this.ddlTimeZoneList.Visible = false;
                this.LabTimeZone.Visible = true;
                if (this.UserSwipe.Id == 0)
                {
                    this.txtSwipeTime.Text = currentDateTime.ToString("hh:mm");
                }



            }
        }
        catch { throw; }
    }

    //Close the Dialog
    private void CloseDialogControl()
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), System.Guid.NewGuid().ToString(), "closeEditPanelDialog()", true);
        }
        catch { throw; }
    }

    private void CheckInOut()
    {
        try
        {
           
                
            if (Action.Equals("CheckIn", StringComparison.InvariantCultureIgnoreCase))
            {
                //this.ddlTimeSpan.Visible = false;
                this.ddlTimeZoneList.Visible = true;
                
                this.CheckIn();
            }
            else
            {
                //this.ddlTimeZoneList.Visible = true;
                //this.LabTimeZone.InnerText = this.UserSwipe.TimeZoneId.ToString();
                this.CheckOut();
            }
        }
        catch { throw; }
    }


    private void DisplayMessage(Exception ex)
    {
        //display the error message.
        StringBuilder message = new StringBuilder();
        foreach (object data in ex.Data.Values)
        {
            message.Append(string.Format("{0}", data.ToString()+"<br/>"));
        }

        //display the Error message.
        this.divmessage.Style.Add("display", "block");
        this.divmessage.InnerHtml = message.ToString();
        

    }


    private void CheckIn()
    {
        IUserSwipeService service = null;
        try
        {
            // Assign values.
            string checkInDateTime;
            DateTime checkInTime;

            this.ValidateCheckInTime();

                if (UserType.Equals("Self", StringComparison.InvariantCultureIgnoreCase))
                {
                    checkInDateTime = string.Format("{0} {1}",
                               DateTime.Parse(this.spnSwipeDate.InnerText).ToString("MM/dd/yyyy"),
                               this.txtSwipeTime.Text);

                    checkInTime = DateTime.Parse(checkInDateTime);
                    
                }
                else
                {
                    checkInDateTime = string.Format("{0} {1} {2}",
                               DateTime.Parse(this.txtSwipeDate.Text).ToString("MM/dd/yyyy"),
                               this.txtSwipeTime.Text,
                               this.ddlTimeSpan.SelectedValue);

                    checkInTime = DateTime.Parse(checkInDateTime);
                }
            
        

            // Validate check in time.
            this.ValidateCheckInTime(checkInTime, this.UserSwipe);

            this.UserSwipe.CheckInTime = checkInTime;
            this.UserSwipe.TimeZoneId = Int32.Parse(ddlTimeZoneList.Items[ddlTimeZoneList.SelectedIndex].Value);
            this.UserSwipe.CreateUserId = this.AppManager.LoginUser.Id;
            this.UserSwipe.CreateDate = DateTime.Now;
            this.UserSwipe.LastUpdateUserId = this.AppManager.LoginUser.Id;
            this.UserSwipe.LastUpdateDate = DateTime.Now;
            this.UserSwipe.Shift = Int32.Parse(ddlShift.SelectedItem.Value);
            this.UserSwipe.UserworktypeId = Int32.Parse(ddWFH.SelectedItem.Value);
            this.UserSwipe.Reason = tbxreason.InnerText;
            if (!this.UserSwipe.CustomData.ContainsKey("EditBy"))
                this.UserSwipe.CustomData.Add("EditBy", UserType);
            //else
            //    this.UserSwipe.CustomData["EditBy"] =  "Self";


            // Create the service and invoke method.
            service = AppService.Create<IUserSwipeService>();
            service.AppManager = this.AppManager;
            service.CheckIn(this.UserSwipe);

            //Close the Dialog.
            this.CloseDialogControl();
        }
        catch (ValidationException ve)
        {

            throw ve;
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void CheckOut()
    {
        IUserSwipeService service = null;
        try
        {
          
            DateTime checkoutTime;
            string checkoutDateTime;

            this.ValidateCheckOutTime();
            // Assign values.

            if (UserType.Equals("Self", StringComparison.InvariantCultureIgnoreCase))
            {
                 checkoutDateTime = string.Format("{0} {1}",
                            spnSwipeDate.InnerText.ToString(),
                            txtSwipeTime.Text);
                checkoutTime = DateTime.Parse(checkoutDateTime);
            }
            else
            {
                //checkout time  
                
                
                    checkoutDateTime = string.Format("{0} {1} {2}",
                                 txtSwipeDate.Text.ToString(),
                                 txtSwipeTime.Text,
                                 ddlTimeSpan.SelectedValue);
                    checkoutTime = DateTime.Parse(checkoutDateTime);


            }

            //Validate in CheckOutTime
            this.ValidateCheckOutTime(checkoutTime,this.UserSwipe);
            this.UserSwipe.CheckOutTime = checkoutTime;
            //this.UserSwipe.TimeZoneId = Int32.Parse(ddlTimeZoneList.Items[ddlTimeZoneList.SelectedIndex].Value);
            this.UserSwipe.CreateUserId = this.AppManager.LoginUser.Id;
            this.UserSwipe.LastUpdateUserId = this.AppManager.LoginUser.Id;
            this.UserSwipe.Shift =Int32.Parse(this.ddlShift.SelectedItem.Value);
            this.UserSwipe.Reason = this.tbxreason.InnerText;
            if (!this.UserSwipe.CustomData.ContainsKey("EditBy"))
                this.UserSwipe.CustomData.Add("EditBy", UserType);

            // Create the service and invoke method.
            service = AppService.Create<IUserSwipeService>();
            service.AppManager = this.AppManager;

            // Invoke method.
            service.CheckIn(this.UserSwipe);
            

            //Close the Dialog.
            this.CloseDialogControl();
        }
        catch (ValidationException ve)
        {
            
            throw ve;
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void DisplayWarningMsg(string Message)
    {
        SwipePanel.Visible = false;
        popupClickOk.Visible = true;
        btndisplay.Visible = true;
        MessagePanel.InnerHtml = "<b>" + Message + "</b>";
    }

    #endregion


    #region Page Events 
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

   

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            //check the action.
            this.CheckInOut();


        }
        catch (ValidationException ve)
        {
            this.DisplayMessage(ve);
            //this.FillTimeZoneList();
            //this.DisplayControls();
            
        }
        catch { throw; }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
   

    protected void btndisplay_Click(object sender, EventArgs e)
    {
        SwipePanel.Visible = true;
        popupClickOk.Visible = false;
        
    }
    #endregion
    private void ClearControl()
    {
        // clear the Control 
        ddlTimeZoneList.Items.Clear();
        txtSwipeDate.Text  = "";
        //ddlShift.Text  = "";
        txtSwipeTime.Text = "";
        //ddlTimeSpan.Text = "";
        tbxreason.InnerText = "";
     
    }
    private void RemoveUserSwipeCheckInOutDetails()
    {
        IUserSwipeService service = null;
      
        try
        {
            // Create the service.
            service = AppService.Create<IUserSwipeService>();
            service.AppManager = this.AppManager;

            //validate the data.
            ValidationException validexception = new ValidationException();
            if (tbxreason.InnerText.Trim() == "" || string.IsNullOrEmpty(tbxreason.InnerText))
                validexception.Data.Add("USER_SWIPE_REASON", "Reason is a Mandatory Field");
            
            if (validexception.Data.Count>0)

                //Throw the exception.
                throw validexception;

            //Assing the varibles.
            DateTime swipeDate = this.UserSwipe.WorkDate;
            int userId =this.UserSwipe.UserId;
            int movedUserId = this.AppManager.LoginUser.Id;
            string movedreason = this.tbxreason.InnerText.ToString();

            // Call method.
            service.RemoveUserSwipeCheckInOutDetailsint(userId, swipeDate, movedUserId, movedreason);
         
        }
        
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITYRESET");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            //this.divEmptyRow.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

        var CHECKINOUTALERT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLCHECKINALERT")).FirstOrDefault();
        if (CHECKINOUTALERT != null)
        {

            //lblWarning = Convert.ToString(CHECKINOUTALERT.DisplayText);
            lblCheckinFirst = Convert.ToString(CHECKINOUTALERT.SupportingText1);
        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //this.ClearControl();
            RemoveUserSwipeCheckInOutDetails();
            Session.Add("ResposeMessage", "Check In/Out Details Deleted Successfully.");
            this.CloseDialogControl();
        }
        catch (ValidationException ex)
        {
            this.DisplayMessage(ex);

        }
        catch
        {
            throw;
        }
       
    }
    protected void chkRemove_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkRemove.Checked)
            {
                tblswipedetail.Visible = false;
                this.btnOk.Visible = false;
                this.btnDelete.Visible = true;
                chkRemove.Visible = false;
                this.divmessage.Style.Add("display", "none");
                this.divmessage.InnerHtml = "";
            }
        }
        catch
        {
            throw;
        }
    }
}