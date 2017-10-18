using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Data;
using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class SearchViews_UserSearchView : System.Web.UI.UserControl
{

    #region Class Variables
    IAppManager mappmanager = null;
    IList<User> UserLst = null;
    const string USER_SEARCH_RESULT_LIST = "USER_SEARCH_RESULT_LIST";

    string ALERTMSG;
    string ALERTNORECORDFOUND;
    string SEARCHANYONEFIELD;

    public string onSearchResultSelect { get; set; }
    public string onDialogClose { get; set; }

    public IAppManager AppManager
    {
        get;
        set;
    }
    public string ViewName
    {
        get
        {
            return ViewState["ViewName"]!=null? Convert.ToString(ViewState["ViewName"]):string.Empty;
        }
        set
        {
            ViewState["ViewName"] = value;
        }
    }
  
    #endregion

    #region public members 
    public  void Display()
    {
        //InitalspnMessage.InnerHtml = "<b>Enter the criteria and click on Search button to view data.</b>";
       

        //hide the cotainer 
        this.HideContainerControl(dvinitalvalue, true);
       
        this.PlcUser.Visible = true;
        //update the updatepanel 
        //this.UpdateUserSearch.Update();

        //clear the controls.
       this.ClearControls();
        

        //Disable the Referesh contriol
       this.MakeControlDisable(LknRefersh, false);

        //Bind the Location
       this.BindLocation();

        //Focus the btnsearch control
       // btnSearch.Focus();

       //Focus the Control
       this.txtName.Focus();

    }
    #endregion

    #region Internal Members

    #region Disable a Common  WebControl 
    private void MakeControlDisable(WebControl ctrlname, bool enable)
    {
        //Disable a webcontrol  
        ctrlname.Enabled = enable;
    }
    #endregion

    #region Enable Refersh Control
    private void DisableRefershControl()
    {

        //Enable/Disable the Refersh Control
         this.MakeControlDisable(LknRefersh, true);

    }
    #endregion

    #region Clear the Input Control and Show the Initial view
    private void ClearControls()
    {
        //clear the input controls
        this.ClearControls(txtEmail);
        this.ClearControls(txtName);
        this.ClearControls(txtRole);
       // this.ClearControls(txtlocation);

        //Bind the Empty Data.
        GvwUserList.DataSource = null;
        GvwUserList.DataBind();
        
    }

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "SEARCHUSER");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            this.InitalspnMessage.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

        var ALERTNORECORDFOUND_text = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTNORECORDFOUND")).FirstOrDefault();
        if (ALERTNORECORDFOUND_text != null)
        {

            ALERTNORECORDFOUND = Convert.ToString(ALERTNORECORDFOUND_text.DisplayText);
        }

        var LBLSEARCHANYONEFIELD_text = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLSEARCHANYONEFIELD")).FirstOrDefault();
        if (LBLSEARCHANYONEFIELD_text != null)
        {

            SEARCHANYONEFIELD = Convert.ToString(LBLSEARCHANYONEFIELD_text.DisplayText);
        }

    }

    private void BindLocation()
    {
        ILocationService service = null;
        try
        {
            service = AppService.Create<ILocationService>();
            service.AppManager = this.mappmanager;
            List<Location> lstLocation = service.ReatrieveAll();
            ddlLocation.DataTextField = "City";
            ddlLocation.DataValueField = "Id";
            ddlLocation.DataSource = lstLocation;
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, new ListItem("--Select--", "0"));
            if (ddlLocation.Items.Count > 0)
                ddlLocation.SelectedIndex = 0;

        }
        catch
        {
            throw;
        }
        finally
        {
            if (service != null)
            {
                service.Dispose();
            }
        }
    }
    #endregion

    #region Search Users based on Name and Role

    private void UserSearchEntity(string name, string RoleName, string Email,string CityName)
    {
        IUserService Service = null;
        try
        {
            //create  a service. 
            Service = AppService.Create<IUserService>();
            Service.AppManager = this.mappmanager;

            //Buid the Search criteria based on Viewname.
            if(ViewName!="")
                UserLst = Service.Search(new UserSearchCriteria { Name = name, RoleName = RoleName, EMailId = Email, CityName=CityName, Status="ACTIVE",UserId=AppManager.LoginUser.Id,ViewName=ViewName});
            else
            //Build the search criteria. 
                UserLst = Service.Search(new UserSearchCriteria { Name = name, RoleName = RoleName, EMailId = Email, CityName = CityName });

            this.DisplayList(UserLst);
        }
        catch { throw; }
        finally
        {
            //Dispose the Service.
            if (Service != null) Service.Dispose();

        }

    }

    private void UserSearchEntity(string name, string RoleName, string Email, string CityName,int Userid)
    {
        IUserService Service = null;
        try
        {
            //create  a service. 
            Service = AppService.Create<IUserService>();
            Service.AppManager = this.mappmanager;

            //Buid the Search criteria based on Viewname.
            if (ViewName != "")
                UserLst = Service.Search(new UserSearchCriteria { Name = name, RoleName = RoleName, EMailId = Email, CityName = CityName, Status = "ACTIVE", UserId = Userid, ViewName = ViewName });
            else
                //Build the search criteria. 
                UserLst = Service.Search(new UserSearchCriteria { Name = name, RoleName = RoleName, EMailId = Email, CityName = CityName });

            this.DisplayList(UserLst);
        }
        catch { throw; }
        finally
        {
            //Dispose the Service.
            if (Service != null) Service.Dispose();

        }

    }
    #endregion


    private void DisplayList(IList<User> LstUser)
    {
        try
        {
            //Bind the Data
            GvwUserList.DataSource = LstUser;
            GvwUserList.DataBind();
        }
        catch { throw; }
    }
    private void HideContainerControl(HtmlContainerControl ctrl, bool visible)
    {
        ctrl.Visible = visible;

    }
    //clear the control
    private void ClearControls(HtmlInputControl htmlctr)
    {
        htmlctr.Value = string.Empty;
    }
    private void ValidateSearchEntity()
    {
        try
        {
            //create exception instance.
            ValidationException exception = new ValidationException("");
            if (txtEmail.Value.Trim() == "" && txtName.Value.Trim() == "" && txtRole.Value.Trim() == "" && ddlLocation.SelectedIndex==0)
                exception.Data.Add("Record", SEARCHANYONEFIELD);
            if (exception.Data.Count > 0)
                //Throw a exception if any.
                throw exception;
        }
        catch { throw; }


    }
    //show/hide the error message.
    private void ShowHideErrorMessage(string  visible)
    {
        try
        {

            divMessage.Style.Add("display", visible);
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
            this.divMessage.Controls.Add(control);
            this.divMessage.Controls.Add(error);

            // Show message control.
            this.ShowHideErrorMessage("block");

        }
        catch { throw; }
    }
    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
     {

        UserAuthentication muserAuthentication = new UserAuthentication();
        this.mappmanager = Session["APP_MANAGER"] as IAppManager;

        // Attach client side script for button.
        if (string.IsNullOrEmpty(this.onDialogClose))
            this.btnClose.Attributes.Remove("onclick");
        else
            this.btnClose.Attributes.Add("onclick", string.Format("return {0}();", this.onDialogClose));

        LoadLabels();
        //inital view.
        

        if (!Page.IsPostBack)
            this.Display();

       
        ShowHideErrorMessage("none");
        
        
            

    }

   

 

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //Add the session
        Session.Add(USER_SEARCH_RESULT_LIST, UserLst);
      
      
    }
    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            //validate.
            this.ValidateSearchEntity();
            
            string Name = this.txtName.Value.Trim();
            string RoleName = this.txtRole.Value.Trim();
            string EmailID = this.txtEmail.Value.Trim();
            string CityName = ddlLocation.SelectedIndex!=0?ddlLocation.SelectedItem.Text:string.Empty;

            //build the  search Entity.
            if (ViewName == "SearchManager")
            {
                this.UserSearchEntity(Name, RoleName, EmailID, CityName,Convert.ToInt32(Session["UsermanagerId"].ToString()));
            }
            else if (ViewName == "SearchManagerforhierarchy")
            {
                this.UserSearchEntity(Name, RoleName, EmailID, CityName, Convert.ToInt32(Session["UsermanagerId"].ToString()));
            }
            else if (ViewName == "SearchUsersforuserSetting")
            {
                this.UserSearchEntity(Name, RoleName, EmailID, CityName);
            }
            else
            {
                this.UserSearchEntity(Name, RoleName, EmailID, CityName);
            }


            this.DisableRefershControl();

            if (GvwUserList.Rows.Count == 0)
            {
                this.HideContainerControl(dvinitalvalue, true);
                this.InitalspnMessage.InnerHtml = ALERTNORECORDFOUND;
            }
            else
            {
                this.HideContainerControl(dvinitalvalue, true);

                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "SEARCHUSER");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, GvwUserList);
                dvinitalvalue.Visible = false;

            }
            this.ShowHideErrorMessage("none");
            
          

        }
        catch (ValidationException ve)
        {

            //Display the Errors. 
            this.DisplayValidationMessage(ve);

            //Hide the Inital view 
            this.HideContainerControl(dvinitalvalue, true);
            //this.InitalspnMessage.InnerHtml = "<b>Enter the criteria and click on Search button to view data.</b>";

            //Clear the Grid
            this.DisplayList(new List<User>());
           
            //disable the Refersh control
            this.MakeControlDisable(LknRefersh, false);
            
        }
        catch { throw; }



    }

    protected void GvwUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Get each user.
                User user = (User)e.Row.DataItem;

                //Find the Control.
                HtmlGenericControl RoleName = (HtmlGenericControl)e.Row.FindControl("spnRoleName");
                HtmlGenericControl Location = (HtmlGenericControl)e.Row.FindControl("spnLocation");
                HtmlAnchor Lnkfirstname = (HtmlAnchor)e.Row.FindControl("lnkFirstname");

             
                // Serialize data item.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string jsonDataItem = serializer.Serialize(user);


                //Assign to the control.

                RoleName.InnerText = user.CustomData["RoleName"].ToString();
                Location.InnerText = user.CustomData["Location"].ToString();
               // Lnkfirstname.InnerHtml = user.FirstName!=""?user.FirstName:"Select";
                if (!string.IsNullOrEmpty(this.onSearchResultSelect) || !string.IsNullOrEmpty(this.onSearchResultSelect.Trim()))
                    Lnkfirstname.Attributes.Add("onclick", string.Format("return {0}('{1}');", this.onSearchResultSelect, jsonDataItem));

            }
          
            
            
        }
        catch { throw; }
    }
   
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
         {
            
            //hide the Error message
            this.ShowHideErrorMessage("none");
            
           //show the initial view
            this.Display();
            

            
            
    

        }
        catch { throw; }
    }
    protected void LnkRefersh_Click(object sender, EventArgs e)
    {
        try
        {
            //Refersh the page. 
            btnSearch_Click(sender, e);

        }
        catch { throw; }
    }

 
    #endregion

}