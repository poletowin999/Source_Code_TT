using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
public partial class UsersProfile_wfrmUserPermission : System.Web.UI.Page
{

    #region Class Variables
    List<UsersPermission> UsersPermissionList = null;
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
            if (Convert.ToString(Session["UserPermissionAddOrUpdate"]) != null)
            {
                if (Convert.ToString(Session["UserPermissionAddOrUpdate"]).Equals("TRUE"))
                {
                    Session["UserPermissionAddOrUpdate"] = null;
                    this.DisplayList(this.SearchUsersPermissions());                  
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

    private List<UsersPermission> SearchUsersPermissions()
    {
        IUsersProfile service = null;
        try
        {          

            //Build the search Entity
            string Name = txtPermissionNo.Value.Trim();           
            string Status = ddlStatus.Items[ddlStatus.SelectedIndex].Value;
           
            //create a service
            service = AppService.Create<IUsersProfile>();
            service.AppManager = this.mappmanager;
            this.divMessage.InnerHtml = "";
            //Invoke the service method           
            this.UsersPermissionList = service.SearchPermissions(Name, Status);

            if (UsersPermissionList != null)
            {
                // this.ShowHideValidationMessage(false);
                this.HideIntialView(true);
                this.hdrGridHeader.InnerText = cntlist + " (" + UsersPermissionList.Count + " " + cntFound + ")";
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
        return UsersPermissionList;


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
    private void DisplayList(IList<UsersPermission> UsersPermissionLst)
    {
        try
        {
            //Bind the Grid
            GvUserPermission.DataSource = UsersPermissionLst;
            GvUserPermission.DataBind();
            if (GvUserPermission.Rows.Count > 0)
            {

                this.spnMessage.Visible = false;
                this.DisplayMessage(string.Empty);

                if (GvUserPermission.HeaderRow != null)
                {

                    List<LblLanguage> lblLanguagelst = null;

                    ILblLanguage mLanguageService = null;
                    lblLanguagelst = new List<LblLanguage>();
                    mLanguageService = AppService.Create<ILblLanguage>();
                    mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                    // retrieve
                    lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "UsersPermission");

                    Utility _objUtil = new Utility();
                    _objUtil.LoadGridLabels(lblLanguagelst, GvUserPermission);
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
        if (GvUserPermission.Rows.Count > 0)
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
        mLanguageService.AppManager = this.mappmanager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERSPROFILE");

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
            this.DisplayList(this.SearchUsersPermissions());
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

    protected void GvUserPermission_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.UsersPermission Entity = (Tks.Entities.UsersPermission)e.Row.DataItem;
                //Find the control.             
               
                HtmlGenericControl Isactive = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                HtmlGenericControl Locations = (HtmlGenericControl)e.Row.FindControl("spnLocations");
                //Bind the Location is Active or not
                Isactive.InnerText = Entity.IsActive == true ? "Active" : "Inactive";
                Locations.InnerText = Convert.ToString(Entity.CustomData["LocationNames"]);
            }
        }
        catch { throw; }

    }
    protected void btnclear_Click(object sender, EventArgs e)
    {       
        //clear the controls
        this.ClearInputControls(txtPermissionNo);        
        this.ddlStatus.SelectedIndex = 1;
        //clear the Error Message
        this.DisplayMessage(string.Empty);
        //bind the Empty Data 
        UsersPermissionList = null;
        this.DisplayList(UsersPermissionList);
        this.hdrGridHeader.InnerText = "";  
    }

    protected void GvUserPermission_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Paging 
            GvUserPermission.PageIndex = e.NewPageIndex;
            this.DisplayList(this.SearchUsersPermissions());
        }
        catch { throw; }
    }
    protected void GvUserPermission_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Convert.ToString(e.CommandArgument)))
            {
                lblPermissionName.Text = "Permission : " + Convert.ToString(e.CommandName);

                BindLocationsAndDepartments(Convert.ToInt32(e.CommandArgument));

                BindClients(Convert.ToInt32(e.CommandArgument));

                BindReports(Convert.ToInt32(e.CommandArgument));

                DisplayEntityEditPanel();                
            }
        }
        catch { throw; }
    }

    #region "Permission Popup"
    private void BindLocationsAndDepartments(int PermissionId)
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mappmanager;

        // retrieve existing locations and departments
        DataTable dt = mUserProfile.RetrieveUserPermissionAccess(PermissionId);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                DataTable dtLocations = dt.DefaultView.ToTable(true, "City", "LocationId");

                tvLocations.Nodes.Clear();
                for (int i = 0; i < dtLocations.Rows.Count; i++)
                {
                    TreeNode childNode = new TreeNode();
                    childNode.Text = Convert.ToString(dtLocations.Rows[i]["City"]);
                    childNode.Value = Convert.ToString(dtLocations.Rows[i]["LocationId"]);
                    childNode.Expanded = true;

                    DataRow[] drDept = dt.Select("LocationId = " + Convert.ToString(dtLocations.Rows[i]["LocationId"]));

                    if (drDept.Length > 0)
                    {
                        foreach (DataRow dr in drDept)
                        {
                            TreeNode SubNode = new TreeNode();
                            SubNode.Text = Convert.ToString(dr["Name"]);
                            SubNode.Value = Convert.ToString(dr["DepartmentId"]);
                            childNode.ChildNodes.Add(SubNode);
                        }
                    }

                    tvLocations.Nodes.Add(childNode);
                }


            }
        }

        foreach (TreeNode node in tvLocations.Nodes)
        {
            RemoveNodesLink(node);
        }
    }

    private void BindClients(int PermissionId)
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mappmanager;

        // retrieve clients
        DataTable dt = mUserProfile.RetrieveUserClientAccess(PermissionId);

        if (dt != null)
        {
            tvClients.Nodes.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = Convert.ToString(dt.Rows[i]["Name"]);
                childNode.Value = Convert.ToString(dt.Rows[i]["ClientId"]);
                tvClients.Nodes.Add(childNode);
            }
        }

        foreach (TreeNode node in tvClients.Nodes)
        {
            RemoveNodesLink(node);
        }
    }

    private void BindReports(int PermissionId)
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mappmanager;

        // retrieve Reports        
        DataTable dt = mUserProfile.RetrieveUserReportAccess(1, PermissionId);

        if (dt != null)
        {
            tvReports.Nodes.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = Convert.ToString(dt.Rows[i]["Name"]);
                childNode.Value = Convert.ToString(dt.Rows[i]["ReportId"]);
                tvReports.Nodes.Add(childNode);
            }
        }

        foreach (TreeNode node in tvReports.Nodes)
        {
            RemoveNodesLink(node);
        }
    }

    private void RemoveNodesLink(TreeNode node)
    {
        node.SelectAction = TreeNodeSelectAction.None;
        if (node.ChildNodes.Count > 0)
        {
            foreach (TreeNode ChildNode in node.ChildNodes)
            {
                RemoveNodesLink(ChildNode);
            }
        }
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
                string.Format("showEditPanelDialog({{'width': '800px', 'title': '{0}'}});", "User Permission Details"),
                true);
        }
        catch { throw; }
    }
    #endregion
}
