using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Data;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public enum UIMODEACCESS
{
    NEW,
    EDIT,
    VIEW
}
public partial class UsersProfile_wfrmAddEditAccessLevel : System.Web.UI.Page
{    
    #region Class Variables
    IAppManager mAppManager;

    string ALERTMSG;
    string PAGEHEADINGADD;
    string ALERTMSGFIELD;
    string ALERTINFO;
    string VALIDATIONERROR;

    #endregion


    public UIMODEACCESS UIMODEACCESS
    {
        get
        {
            if (ViewState["UIMODEACCESS"] == null)
                ViewState["UIMODEACCESS"] = new UIMODEACCESS();
            return (UIMODEACCESS)ViewState["UIMODEACCESS"];
        }
        set
        {
            ViewState["UIMODEACCESS"] = value;
        }
    }

    private int AccessLevelId
    {
        get
        {
            if (ViewState["AccessLevelId"] == null)
                ViewState["AccessLevelId"] = 0;
            return (int)ViewState["AccessLevelId"];
        }
        set
        {
            ViewState["AccessLevelId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Inilizethecontrols();
            this.DisplayMessage(string.Empty);
            this.DisplayValidationError(null);
            
            if (!Page.IsPostBack)
            {      
                
                //*** Load Access Levels ***//
                LoadAccessLevels();

                //*** Load Treeview Permissions ***//
                LoadPermissions();               

                string qsUIMODE = Convert.ToString(this.Page.RouteData.Values["UIMODEACCESS"]);
                if (string.IsNullOrEmpty(qsUIMODE) == false)
                {
                    UIMODEACCESS = (UIMODEACCESS)Enum.Parse(typeof(UIMODEACCESS), qsUIMODE);
                    AccessLevelId = Convert.ToInt32((Convert.ToString(this.Page.RouteData.Values["Id"]).Replace('$', '/')));
                    LoadAccessLevel();
                }

                if (UIMODEACCESS == UIMODEACCESS.NEW)
                {
                    AccessLevelId = 0;
                    divActive.Visible = false;                    
                }
                else if (UIMODEACCESS == UIMODEACCESS.EDIT)
                {
                    divActive.Visible = true;
                    ddlAccessLevel.Enabled = false;
                }

            }
            LoadLabels();
            lblHeader.InnerText = PAGEHEADINGADD;            
        }
        catch (Exception ex)
        {
            throw ex;
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERSACCESSLEVEL");

        if (lblLanguagelst != null)
        {

            Utility _objUtil = new Utility();
            _objUtil.LoadLabels(lblLanguagelst);

            var ALERT_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTMSG")).FirstOrDefault();
            if (ALERT_MSG != null)
            {
                ALERTMSG = Convert.ToString(ALERT_MSG.DisplayText);
            }

            var ALERT_MSGFIELD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTMSGFIELD")).FirstOrDefault();
            if (ALERT_MSGFIELD != null)
            {
                ALERTMSGFIELD = Convert.ToString(ALERT_MSGFIELD.DisplayText);
            }

            var ALERT_INFO = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTINFO")).FirstOrDefault();
            if (ALERT_INFO != null)
            {
                ALERTINFO = Convert.ToString(ALERT_INFO.DisplayText);

                if (AccessLevelId != 0)
                    ALERTINFO = Convert.ToString(ALERT_INFO.SupportingText1);
            }
            

            var PAGEHEADING_ADD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLHEADERADDEDIT")).FirstOrDefault();
            if (PAGEHEADING_ADD != null)
            {
                PAGEHEADINGADD = Convert.ToString(PAGEHEADING_ADD.DisplayText);

                if (AccessLevelId != 0)
                    PAGEHEADINGADD = Convert.ToString(PAGEHEADING_ADD.SupportingText1);
            }

            var VALIDATIONS = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("VALIDATION")).FirstOrDefault();
            if (VALIDATIONS != null)
            {
                VALIDATIONERROR = Convert.ToString(VALIDATIONS.DisplayText);
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

    private void Inilizethecontrols()
    {
        try
        {
            // Getting appmagner.
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;

        }
        catch { throw; }
    }

    protected void tvPermissions_SelectedNodeChanged(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(Convert.ToString(tvPermissions.SelectedNode.Value)))
        {
            lblPermissionName.Text = "Permission : " + Convert.ToString(tvPermissions.SelectedNode.Text);

            BindLocationsAndDepartments(Convert.ToInt32(tvPermissions.SelectedNode.Value));

            BindClients(Convert.ToInt32(tvPermissions.SelectedNode.Value));

            BindReports(Convert.ToInt32(tvPermissions.SelectedNode.Value));

            DisplayEntityEditPanel();

            tvPermissions.SelectedNode.Selected = false;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // validate
            this.ValidateAccess();


            if (AddEditAccessLevel() == 1)
            {
                if (UIMODEACCESS == UIMODEACCESS.NEW)
                {
                    DisplayMessage(ALERTINFO);
                }
                else if (UIMODEACCESS == UIMODEACCESS.EDIT)
                {
                    DisplayMessage(ALERTINFO);
                }

                ScriptManager.RegisterStartupScript(UpdateSearchPanel, UpdateSearchPanel.GetType(), "onSuccess", "onSuccess();", true);

                Session["AccessLevelsAddOrUpdate"] = "TRUE";
            }  

        }
        catch (ValidationException ve)
        {
            // Display error.
            this.DisplayValidationError(ve);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
       

    private void LoadAccessLevels()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;
        // retrieve
        DataTable dt = mUserProfile.RetrieveAllAccessLevel();

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                ddlAccessLevel.Items.Clear();
                ddlAccessLevel.DataSource = dt.DefaultView;
                ddlAccessLevel.DataTextField = "AccessLevelName";
                ddlAccessLevel.DataValueField = "AccessLevelId";
                ddlAccessLevel.DataBind();

                ddlAccessLevel.Items.Insert(0,new ListItem("-- Select a Access Level --", "0"));
            }
        }       
    }

    private void LoadPermissions()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;
        // retrieve
        DataTable dt = mUserProfile.RetrieveAllPermissions();

        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = Convert.ToString(dt.Rows[i]["PermissionNo"]);
                childNode.Value = Convert.ToString(dt.Rows[i]["PermissionId"]);
                tvPermissions.Nodes.Add(childNode);
            }
        }

        foreach (TreeNode node in tvPermissions.Nodes)
        {
            //RemoveNodesLink(node);
        }
    }

    private void LoadAccessLevel()
    {

        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;
        // retrieve
        DataTable dt = mUserProfile.RetrieveAccessLevelDetailsById(AccessLevelId);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["AccessLevelId"]) != 0)
                    this.ddlAccessLevel.SelectedValue = Convert.ToString(dt.Rows[0]["AccessLevelId"]);

                //this.ddlAccessLevel.Items.FindByValue(Convert.ToString(dt.Rows[0]["AccessLevelId"])).Selected = true;

                // Checking Permissions List
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (TreeNode node in tvPermissions.Nodes)
                    {
                        if (Convert.ToInt32(node.Value) == Convert.ToInt32(dr["PermissionId"]))
                        {
                            node.Checked = true;
                        }
                    }
                }
                
                this.chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
            }       
            
           
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

    private int AddEditAccessLevel()
    {
        int id = 0;
        try
        {            
            //*** Clients ***//
            List<AccessLevelDetails> _objAccessLevelList = new List<AccessLevelDetails>();
            foreach (TreeNode node in tvPermissions.Nodes)
            {
                if (node.Checked == true)
                {
                    AccessLevelDetails _objAccessLevelDetails = new AccessLevelDetails(AccessLevelId);                   
                   _objAccessLevelDetails.AccessLevelId = Convert.ToInt32(ddlAccessLevel.SelectedValue);
                   _objAccessLevelDetails.PermissionId = string.IsNullOrEmpty(node.Value) ? 0 : Convert.ToInt32(node.Value);

                   if (UIMODEACCESS == UIMODEACCESS.EDIT)
                   {
                       _objAccessLevelDetails.IsActive = chkActive.Checked;
                       _objAccessLevelDetails.Reason = Convert.ToString(txtReason.Value);
                   }

                   _objAccessLevelDetails.LastUpdateUserId = mAppManager.LoginUser.Id;
                  
                    _objAccessLevelList.Add(_objAccessLevelDetails);
                }
            }           

            IUsersProfile mUserProfile = null;
            mUserProfile = AppService.Create<IUsersProfile>();
            mUserProfile.AppManager = this.mAppManager;

            mUserProfile.AddEditAccessLevelDetails(_objAccessLevelList);
            id = 1;

        }
        catch (ValidationException ve)
        {
            throw ve;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return id;
    }

    #region Validation Message

    private void ValidateAccess()
    {
        try
        {

            ValidationException exception = new ValidationException(VALIDATIONERROR);

            if (ddlAccessLevel.SelectedIndex == 0)
                exception.Data.Add("ACCESSLEVEL", ALERTMSGFIELD + " " + lblAccessLevels.Text);


            //*** Permissions ***//
            bool isPermissionChecked = false;
            foreach (TreeNode node in tvPermissions.Nodes)
            {
                if (node.Checked == true)
                {
                    isPermissionChecked = true;
                }
            }

            if (!isPermissionChecked)
                exception.Data.Add("PERMISSIONS", ALERTMSGFIELD + " " + lblUserPermissions.Text);           


            if (UIMODEACCESS == UIMODEACCESS.EDIT)
            {
                if (string.IsNullOrEmpty(Convert.ToString(txtReason.Value)))
                    exception.Data.Add("REASON", lblreason.Text + ALERTMSG);
            }


            // Throw an exception.
            if (exception.Data.Count > 0) throw exception;
        }
        catch { throw; }
    }


    private void DisplayValidationError(Exception ex)
    {
        try
        {

            if (ex == null)
            {

                this.DisplayMessage(string.Empty);
                return;
            }

            //create string builder instance.
            StringBuilder message = new StringBuilder(ex.Message);

            message.Append("<ul>");

            foreach (object value in ex.Data.Values)
            {
                message.Append(string.Format("<li>{0}</li>", value.ToString()));
            }
            message.Append("</ul>");

            //display the error message.
            this.DisplayMessage(message.ToString());

        }
        catch
        {
            throw;
        }
    }
    private void DisplayMessage(string Message)
    {
        try
        {

            if (divMessage.InnerHtml != "")
                divMessage.InnerHtml = string.Empty;

            if (Message != "")
                divMessage.Style.Add("display", "block");
            else
                divMessage.Style.Add("display", "none");
            divMessage.InnerHtml = Message.ToString();
        }
        catch { throw; }

    }


    #region "Permission Popup"
    private void BindLocationsAndDepartments(int PermissionId)
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;

        // retrieve existing locations and departments
        DataTable dt = mUserProfile.RetrieveUserPermissionAccess(PermissionId);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                DataTable dtLocations = dt.DefaultView.ToTable(true, "City","LocationId");

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
        mUserProfile.AppManager = this.mAppManager;

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
        mUserProfile.AppManager = this.mAppManager;
       
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

    #endregion
}