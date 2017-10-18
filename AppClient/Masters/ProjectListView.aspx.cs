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


public partial class Masters_ProjectListView : System.Web.UI.Page
{
    #region Class Variables
    List<Project> ProjectList = null;
    IAppManager mappmanager = null;

    string cntlist;
    string cntFound;

    string NODATAFOUND;

    #endregion
    private List<Project> SearchProjects()
    {
        IProjectService service = null;
        try
        {
            //validate 
            this.ValidateSearchEntity();

            //Build the search Entity
            string Name = txtName.Value.Trim();
            string ClientName = txtClient.Value.Trim();
            string LocationName = txtLocation.Value.Trim();
            string PlatformName = txtPlatform.Value.Trim();
            string TestName = txtTest.Value.Trim();
            string status = ddlStatus.Items[ddlStatus.SelectedIndex].Value;
            string CategoryId = ddCategory.Items[ddCategory.SelectedIndex].Value;
            //create a service
            service = AppService.Create<IProjectService>();
            service.AppManager = this.mappmanager;
            this.divMessage.InnerHtml = "";
            //Invoke the service method           
            this.ProjectList = service.Search(new ProjectSearchCriteria
            {
                Name = Name,
                ClientName = ClientName,
                LocationName = LocationName,
                PlatformName = PlatformName,
                TestName = TestName,
                Status = status,
                CategoryId = CategoryId
            });

            if (ProjectList != null)
            {
                // this.ShowHideValidationMessage(false);
                this.HideIntialView(true);
                this.hdrGridHeader.InnerText = cntlist + " (" + ProjectList.Count + " " + cntFound + ")"; 
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
        return ProjectList;


    }
    private void HideIntialView(bool visible)
    {
        this.hdrGridHeader.Visible = visible;
        //this.spnMessage.Visible = visible;
    }
    private void ValidateSearchEntity()
    {
        try
        {
            //create a Exception instance.
            ValidationException exception = new ValidationException("Search Validation");
            if (txtName.Value.Trim() == "" && txtClient.Value.Trim() == "" && txtLocation.Value.Trim() == ""
                && txtPlatform.Value.Trim() == "" && txtTest.Value.Trim() == "" && ddlStatus.Value.Trim() == "")
                exception.Data.Add("Search", "For Search any one Field is Mandatory");
            if (exception.Data.Count > 0)
            {
                throw exception;
            }
        }
        catch
        {
            throw;
        }

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
    private void DisplayList(IList<Project> projectLst)
    {
        try
        {
            //Bind the Grid
            GvwUserList.DataSource = projectLst;
            GvwUserList.DataBind();
            if (GvwUserList.Rows.Count > 0)
            {

                this.spnMessage.Visible = false;
                this.DisplayMessage(string.Empty);

                if (GvwUserList.HeaderRow != null)
                {

                    List<LblLanguage> lblLanguagelst = null;

                    ILblLanguage mLanguageService = null;
                    lblLanguagelst = new List<LblLanguage>();
                    mLanguageService = AppService.Create<ILblLanguage>();
                    mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                    // retrieve
                    lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "PROJECT");

                    Utility _objUtil = new Utility();
                    _objUtil.LoadGridLabels(lblLanguagelst, GvwUserList);
                    InitalBind.Visible = false;

                }
                
            }
            else
            {
                //Show No Result Found Message.
                this.spnMessage.Visible = true;
                this.InitalBind.Visible = true;
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
        if (GvwUserList.Rows.Count > 0)
            LnkRefersh.Enabled = true;
        else
            LnkRefersh.Enabled = false;
    }
    private void InitalView()
    {
        try
        {
            this.InitalBind.Visible = true;
//            this.spnMessage.InnerHtml = "Enter the criteria and click on Search button to view data.";

            if (Session["PROJECT_UPDATE"] != null)
            {
                divmsg.Style.Add("display", "block");
                divmsg.Visible = true;
                divmsg.InnerText = Session["PROJECT_UPDATE"].ToString();
                Session.Remove("PROJECT_UPDATE");
            }
            else
            {
                divmsg.Style.Add("display", "none");
                divmsg.Visible = false;
                divmsg.InnerText = string.Empty;
            }

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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "PROJECT");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

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

            //Display the Inital view.
            this.InitalView();
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

    private void DisablePanel()
    {
        try
        {
            divmsg.Style.Add("display", "none");
            divmsg.Visible = false;
            divmsg.InnerText = string.Empty;
        }
        catch { throw; }
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            DisablePanel();

            //this.InitalBind.Visible = false;
            this.DisplayList(this.SearchProjects());
        }
        catch { throw; }

    }

    protected void LnkRefersh_Click(object sender, EventArgs e)
    {
        try
        {
            DisablePanel();
            //refersh the page 
            btnsearch_Click(sender, e);
        }
        catch
        {
            throw;
        }
    }

    protected void GvwUserList_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.Project Entity = (Tks.Entities.Project)e.Row.DataItem;
                //Find the control.
                HtmlGenericControl ClientName = (HtmlGenericControl)e.Row.FindControl("spnClient");
               // HtmlGenericControl Manager = (HtmlGenericControl)e.Row.FindControl("spnManager");
                //HtmlGenericControl LastUpdateUser = (HtmlGenericControl)e.Row.FindControl("spnUsername");
                HtmlGenericControl Isactive = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                //Bind the Location is Active or not
                Isactive.InnerText = Entity.IsActive == true ? "Active" : "Inactive";
                //Assign  LastUpdateuser to the control.
                ClientName.InnerText = Entity.CustomData["ClientName"].ToString();
                //Manager.InnerText = Entity.CustomData["Manager"].ToString();
                //LastUpdateUser.InnerText = Entity.CustomData["LastUpdateUserName"].ToString();
                //Isactive.InnerText = Entity.IsActive == true ? "Active" : "Inactive"

            }
        }
        catch { throw; }

    }
    protected void btnclear_Click1(object sender, EventArgs e)
    {
        DisablePanel();
        //clear the controls
        this.ClearInputControls(txtName);
        this.ClearInputControls(txtClient);
        this.ClearInputControls(txtLocation);
        this.ClearInputControls(txtPlatform);
        this.ClearInputControls(txtTest);
        this.ddlStatus.SelectedIndex = 1;
        //clear the Error Message
        this.DisplayMessage(string.Empty);
        //bind the Empty Data 
        ProjectList = null;
        this.DisplayList(ProjectList);
        this.hdrGridHeader.InnerText = "";
        this.InitalBind.Visible = true;
        //this.spnMessage.InnerHtml = "<b> Enter the criteria and click on Search button to view data.</b>";


    }
    protected void GvwUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Paging 
            GvwUserList.PageIndex = e.NewPageIndex;
            this.DisplayList(this.SearchProjects());
        }
        catch { throw; }
    }
}
