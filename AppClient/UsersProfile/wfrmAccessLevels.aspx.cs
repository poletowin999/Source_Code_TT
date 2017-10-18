using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class UsersProfile_wfrmAccessLevels : System.Web.UI.Page
{
    #region Class Variables
    List<AccessLevel> AccessLevelList = null;
    IAppManager mappmanager = null;

    string cntlist;
    string cntFound;

    string NODATAFOUND;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        mappmanager = Session["APP_MANAGER"] as IAppManager;
        if (mappmanager.LoginUser.HasAdminRights == false)
        {
            ErrorLogProvider provider = null;
            try
            {
                //create a exception.
                Exception exception = HttpContext.Current.Error;

                //insert into Error log.
                provider = new ErrorLogProvider();
                provider.AppManager = this.mappmanager;
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

        UserAuthentication muserauthentication = new UserAuthentication();
        this.mappmanager = muserauthentication.AppManager;
        LoadLabels();

        if (!Page.IsPostBack)
        {
            if (Convert.ToString(Session["AccessLevelsAddOrUpdate"]) != null)
            {
                if (Convert.ToString(Session["AccessLevelsAddOrUpdate"]).Equals("TRUE"))
                {
                    Session["AccessLevelsAddOrUpdate"] = null;
                    this.DisplayList(this.SearchAccessLevels());                    
                }
            }
                
        }
    }
    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;
        try
        {
            Exception exception = HttpContext.Current.Error;
            provider = new ErrorLogProvider();
            provider.AppManager = mappmanager;
            provider.Insert(exception);

        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //Disable the Control
        this.DisableRefershControl();
    }

    private List<AccessLevel> SearchAccessLevels()
    {
        IUsersProfile service = null;
        try
        {

            //Build the search Entity
            string Name = txtAccessLevelName.Value.Trim();
            string Status = ddlStatus.Items[ddlStatus.SelectedIndex].Value;

            //create a service
            service = AppService.Create<IUsersProfile>();
            service.AppManager = this.mappmanager;
            this.divMessage.InnerHtml = "";
            //Invoke the service method           
            this.AccessLevelList = service.SearchAccessLevel(Name, Status);

            if (AccessLevelList != null)
            {
                // this.ShowHideValidationMessage(false);
                this.HideIntialView(true);
                this.hdrGridHeader.InnerText = cntlist + " (" + AccessLevelList.Count + " " + cntFound + ")";
            }

        }

        catch (ValidationException ve)
        {
            this.DisplayValidationError(ve);
            this.hdrGridHeader.InnerText = "";

        }
        catch { throw; }

        finally
        {
            if (service != null)
            {
                //Dispose the services.
                service.Dispose();
            }
        }
        return AccessLevelList;


    }

    private void HideIntialView(bool visible)
    {
        this.hdrGridHeader.Visible = visible;
        //this.spnMessage.Visible = visible;
    }

    private void DisplayValidationError(Exception ex)
    {
        StringBuilder message = new StringBuilder(ex.Message);
        foreach (string datas in ex.Data.Values)
        {
            message.Append("<li>" + datas.ToString() + "</li>");
        }
        this.DisplayMessage(message.ToString());
    }
    private void DisplayMessage(string Message)
    {
        if (divMessage.InnerHtml != "")
            divMessage.InnerHtml = string.Empty;

        divMessage.InnerHtml = Message.ToString();

    }
    private void DisplayList(IList<AccessLevel> AccessLevelLst)
    {
        try
        {
            //Bind the Grid
            GvAccessLevel.DataSource = AccessLevelLst;
            GvAccessLevel.DataBind();
            if (GvAccessLevel.Rows.Count > 0)
            {

                this.spnMessage.Visible = false;
                this.DisplayMessage(string.Empty);

                if (GvAccessLevel.HeaderRow != null)
                {

                    List<LblLanguage> lblLanguagelst = null;

                    ILblLanguage mLanguageService = null;
                    lblLanguagelst = new List<LblLanguage>();
                    mLanguageService = AppService.Create<ILblLanguage>();
                    mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                    // retrieve
                    lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "AccessLevel");

                    Utility _objUtil = new Utility();
                    _objUtil.LoadGridLabels(lblLanguagelst, GvAccessLevel);
                    InitalBind.Visible = false;
                }

            }
            else
            {
                //Show No Result Found Message.
                InitalBind.Visible = true;
                this.spnMessage.Visible = true;
                this.spnMessage.InnerHtml = NODATAFOUND;
            }


        }

        catch { throw; }
    }

    //clear the input controls
    private void ClearInputControls(HtmlInputControl Inputctrl)
    {
        Inputctrl.Value = string.Empty;
    }

    private void DisableRefershControl()
    {
        if (GvAccessLevel.Rows.Count > 0)
            LnkRefersh.Enabled = true;
        else
            LnkRefersh.Enabled = false;
    }


    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERSACCESSLEVEL");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        if (lblLanguagelst != null)
        {
            var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
            if (GRID_TITLE != null)
            {

                this.spnMessage.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
                NODATAFOUND = Convert.ToString(GRID_TITLE.SupportingText1);
            }

            var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
            if (NoofRecordFound != null)
            {
                cntlist = NoofRecordFound.DisplayText;
                cntFound = NoofRecordFound.SupportingText1;
            }
        }

    }



    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            //this.InitalBind.Visible = false;
            this.DisplayList(this.SearchAccessLevels());
        }
        catch { throw; }

    }

    protected void LnkRefersh_Click(object sender, EventArgs e)
    {
        try
        {
            //refersh the page 
            btnsearch_Click(sender, e);
        }
        catch
        {
            throw;
        }
    }

    protected void GvAccessLevel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.AccessLevel Entity = (Tks.Entities.AccessLevel)e.Row.DataItem;
                //Find the control.             

                HtmlGenericControl Isactive = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                HtmlGenericControl Permissions = (HtmlGenericControl)e.Row.FindControl("spnPermissions");

                Permissions.InnerText = Convert.ToString(Entity.CustomData["PermissionIds"]);

                //Bind the Location is Active or not
                Isactive.InnerText = Entity.IsActive == true ? "Active" : "Inactive";
            }
        }
        catch { throw; }

    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        //clear the controls
        this.ClearInputControls(txtAccessLevelName);
        this.ddlStatus.SelectedIndex = 1;
        //clear the Error Message
        this.DisplayMessage(string.Empty);
        //bind the Empty Data 
        AccessLevelList = null;
        this.DisplayList(AccessLevelList);
        this.hdrGridHeader.InnerText = "";
    }

    protected void GvAccessLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Paging 
            GvAccessLevel.PageIndex = e.NewPageIndex;
            this.DisplayList(this.SearchAccessLevels());
        }
        catch { throw; }
    }
}
