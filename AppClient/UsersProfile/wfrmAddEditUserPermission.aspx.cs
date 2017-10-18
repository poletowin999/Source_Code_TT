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

public enum UIMODEPERMISSION
{
    NEW,
    EDIT,
    VIEW
}
public partial class UsersProfile_wfrmAddEditUserPermission : System.Web.UI.Page
{
    #region Class Variables
    IAppManager mAppManager;

    string ALERTMSG;
    string PAGEHEADINGADD;
    string ALERTMSGFIELD;
    string ALERTINFO;
    string VALIDATIONERROR;    
    #endregion   


    public UIMODEPERMISSION UIMODEPERMISSION
    {
        get
        {
            if (ViewState["UIMODEPERMISSION"] == null)
                ViewState["UIMODEPERMISSION"] = new UIMODEPERMISSION();
            return (UIMODEPERMISSION)ViewState["UIMODEPERMISSION"];
        }
        set
        {
            ViewState["UIMODEPERMISSION"] = value;
        }
    }

    private int PermissionId
    {
        get
        {
            if (ViewState["PermissionId"] == null)
                ViewState["PermissionId"] = 0;
            return (int)ViewState["PermissionId"];
        }
        set
        {
            ViewState["PermissionId"] = value;
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
                

                //*** Load Treeview Locations And Department ***//        
                LoadLocationsAndDepartments();

                //*** Load Treeview Clients ***//
                LoadClients();

                //*** Load Treeview Reports ***//
                LoadReports();

                tvLocations.Attributes.Add("onclick", "postBackByObject(event);");

                tvClients.Attributes.Add("onclick", "postBackByObject(event);");

                tvReports.Attributes.Add("onclick", "postBackByObject(event);");

                string qsUIMODE = Convert.ToString(this.Page.RouteData.Values["UIMODEPERMISSION"]);
                if (string.IsNullOrEmpty(qsUIMODE) == false)
                {
                    UIMODEPERMISSION = (UIMODEPERMISSION)Enum.Parse(typeof(UIMODEPERMISSION), qsUIMODE);
                    PermissionId = Convert.ToInt32((Convert.ToString(this.Page.RouteData.Values["Id"]).Replace('$', '/')));
                    LoadPermission();
                }

                if (UIMODEPERMISSION == UIMODEPERMISSION.NEW)
                {
                    PermissionId = 0;
                    divActive.Visible = false;
                }
                else if (UIMODEPERMISSION == UIMODEPERMISSION.EDIT)
                {
                    divActive.Visible = true;
                }
               
            }
            LoadLabels();
            lblHeader.InnerText = PAGEHEADINGADD;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "locationschkbox", "locationsCheckUncheck();", true);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "clientschkbox", "clientsCheckUncheck();", true);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "reportschkbox", "reportsCheckUncheck();", true);
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERSPROFILE");

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

                if (PermissionId != 0)
                    ALERTINFO = Convert.ToString(ALERT_INFO.SupportingText1);
            }

            var PAGEHEADING_ADD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLHEADERADDEDIT")).FirstOrDefault();
            if (PAGEHEADING_ADD != null)
            {
                PAGEHEADINGADD = Convert.ToString(PAGEHEADING_ADD.DisplayText);

                if (PermissionId != 0)
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

    protected void tvLocations_CheckChanged(Object sender, TreeNodeEventArgs e)
    {
        try
        {
            CreateUserPermissionName();            
        }
        catch
        {
            throw;
        }
    }

    protected void tvClients_CheckChanged(Object sender, TreeNodeEventArgs e)
    {
        try
        {
            CreateUserPermissionName();            
        }
        catch
        {
            throw;
        }
    }

    protected void tvReports_CheckChanged(Object sender, TreeNodeEventArgs e)
    {
        try
        {
            CreateUserPermissionName();            
        }
        catch
        {
            throw;
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // validate
            this.ValidatePermission();

            
                if (AddEditPermissions() == 1)
                {
                    if (UIMODEPERMISSION == UIMODEPERMISSION.NEW)
                    {
                        DisplayMessage(ALERTINFO);
                    }
                    else if (UIMODEPERMISSION == UIMODEPERMISSION.EDIT)
                    {
                        DisplayMessage(ALERTINFO);
                    }

                   ScriptManager.RegisterStartupScript(UpdateSearchPanel, UpdateSearchPanel.GetType(), "onSuccess", "onSuccess();", true);

                   Session["UserPermissionAddOrUpdate"] = "TRUE";                   
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

    

    private void LoadLocationsAndDepartments()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;

        // retrieve
        DataTable dtLocations = mUserProfile.RetrieveLocations();
        DataTable dtDepts = mUserProfile.RetrieveDepartments();

        if (dtLocations != null && dtDepts != null)
        {
            for (int i = 0; i < dtLocations.Rows.Count; i++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = Convert.ToString(dtLocations.Rows[i]["City"]);
                childNode.Value = Convert.ToString(dtLocations.Rows[i]["LocationId"]);
                childNode.ToolTip = Convert.ToString(dtLocations.Rows[i]["ShortName"]);

                for (int j = 0; j < dtDepts.Rows.Count; j++)
                {
                    TreeNode SubNode = new TreeNode();
                    SubNode.Text = Convert.ToString(dtDepts.Rows[j]["Name"]);
                    SubNode.Value = Convert.ToString(dtDepts.Rows[j]["DepartmentId"]);
                    SubNode.ToolTip = Convert.ToString(dtDepts.Rows[j]["ShortName"]);
                    childNode.ChildNodes.Add(SubNode);                    
                }
                childNode.Expanded = false;
                tvLocations.Nodes.Add(childNode);
                
            }
        }

       

        foreach (TreeNode node in tvLocations.Nodes)
        {
            RemoveNodesLink(node);
        }
    }

    private void LoadClients()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;
        // retrieve
        DataTable dtClients = mUserProfile.RetrieveClients();

        if (dtClients != null)
        {
            TreeNode AllNode = new TreeNode();
            AllNode.Text = "All Clients";
            AllNode.Value = "0";

            for (int i = 0; i < dtClients.Rows.Count; i++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = Convert.ToString(dtClients.Rows[i]["ClientName"]);
                childNode.Value = Convert.ToString(dtClients.Rows[i]["ClientId"]);                
                AllNode.ChildNodes.Add(childNode);                
            }
            AllNode.Expanded = true;
            tvClients.Nodes.Add(AllNode);

        }

        foreach (TreeNode node in tvClients.Nodes)
        {
            RemoveNodesLink(node);
        }
    }

    private void LoadReports()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;
        // retrieve
        DataTable dtReports = mUserProfile.RetrieveReports(1);

        if (dtReports != null)
        {
            TreeNode AllNode = new TreeNode();
            AllNode.Text = "All Reports";
            AllNode.Value = "0";

            for (int i = 0; i < dtReports.Rows.Count; i++)
            {
                TreeNode childNode = new TreeNode();
                childNode.Text = Convert.ToString(dtReports.Rows[i]["Name"]);
                childNode.Value = Convert.ToString(dtReports.Rows[i]["Id"]);
                AllNode.ChildNodes.Add(childNode);   
            }

            AllNode.Expanded = true;
            tvReports.Nodes.Add(AllNode);
        }

        foreach (TreeNode node in tvReports.Nodes)
        {
            RemoveNodesLink(node);
        }
    }

    private void LoadPermission()
    {

        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;
        // retrieve
        DataTable dt = mUserProfile.RetrieveUserPermissionById(PermissionId);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                lblPermissionName.Text = Convert.ToString(dt.Rows[0]["PermissionName"]);
                this.chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
            }
            BindPermissonAccess();
            BindClientAccess();
            BindReportAccess();
        }      
    }

    private void BindPermissonAccess()
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
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (TreeNode node in tvLocations.Nodes)
                    {
                        if(Convert.ToInt32(node.Value) == Convert.ToInt32(dr["LocationId"]))
                        {
                            node.Checked = true;

                            if (node.ChildNodes.Count > 0)
                            {
                                //Check all the child nodes.
                                foreach (TreeNode childNode in node.ChildNodes)
                                {
                                    if (Convert.ToInt32(childNode.Value) == Convert.ToInt32(dr["DepartmentId"]))
                                    {
                                        childNode.Checked = true;
                                    }
                                }
                            }

                        }
                    }
                }

            }
        }
    }

    private void BindClientAccess()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;

        // retrieve existing clients
        DataTable dt = mUserProfile.RetrieveUserClientAccess(PermissionId);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (TreeNode node in tvClients.Nodes)
                {
                    node.Checked = true;
                    if (node.ChildNodes.Count > 0)
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToInt32(childNode.Value) == Convert.ToInt32(dr["ClientId"]))
                                {
                                    childNode.Checked = true;
                                }
                            }
                        }
                    }
                }
            }

        }
    }
    


    private void BindReportAccess()
    {
        IUsersProfile mUserProfile = null;
        mUserProfile = AppService.Create<IUsersProfile>();
        mUserProfile.AppManager = this.mAppManager;

        // retrieve existing reports
        DataTable dt = mUserProfile.RetrieveUserReportAccess(1,PermissionId);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (TreeNode node in tvReports.Nodes)
                {
                    node.Checked = true;
                    if (node.ChildNodes.Count > 0)
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToInt32(childNode.Value) == Convert.ToInt32(dr["ReportId"]))
                                {
                                    childNode.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void CreateUserPermissionName()
    {
        try
        {
            string _objPermissionName = string.Empty;

            //*** Location and Department ***//
            foreach (TreeNode node in tvLocations.Nodes)
            {
                //If node has child nodes
                if (node.Checked == true)   //it is better to first check if it is "checked" then proceed to count child nodes
                {
                    //_objPermissionName += node.Text.Substring(0, 3).ToUpper() + "-";
                    _objPermissionName += string.IsNullOrEmpty(Convert.ToString(node.ToolTip)) ? node.Value : node.ToolTip.ToUpper();

                    if (node.ChildNodes.Count > 0)   //check if node has any child nodes
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            if (childNode.Checked == true)
                            {
                                string _objDept = string.IsNullOrEmpty(Convert.ToString(childNode.ToolTip)) ? childNode.Value : childNode.ToolTip.ToUpper();
                                _objPermissionName += "-" + _objDept;
                            }
                        }
                    }

                    _objPermissionName += "~";
                }
            }

            if (!string.IsNullOrEmpty(_objPermissionName))
            {
                _objPermissionName = _objPermissionName.Remove(_objPermissionName.Length - 1, 1);                
            }

            //*** Clients ***//
            foreach (TreeNode node in tvClients.Nodes)
            {
                //If node has child nodes
                if (node.Checked == true)   //it is better to first check if it is "checked" then proceed to count child nodes
                {
                    if (node.ChildNodes.Count > 0)   //check if node has any child nodes
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            if (childNode.Checked == true)
                            {
                                _objPermissionName += "-CLI" + childNode.Value;
                            }
                        }
                    }
                }
            }

            //*** Reports ***//
            foreach (TreeNode node in tvReports.Nodes)
            {
                //If node has child nodes
                if (node.Checked == true)   //it is better to first check if it is "checked" then proceed to count child nodes
                {
                    if (node.ChildNodes.Count > 0)   //check if node has any child nodes
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            if (childNode.Checked == true)
                            {
                                _objPermissionName += "-RE" + childNode.Value;
                            }
                        }
                    }
                }
            }

            lblPermissionName.Text = _objPermissionName;
        }
        catch
        {
            throw;
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

    private int AddEditPermissions()
    {
        int id = 0;
        try
        {
            UsersPermission _objUsersPermission = new UsersPermission(PermissionId);
            _objUsersPermission.PermissionId = PermissionId;
            _objUsersPermission.PermissionName = lblPermissionName.Text;
            _objUsersPermission.LastUpdateUserId = mAppManager.LoginUser.Id;

            if (UIMODEPERMISSION == UIMODEPERMISSION.EDIT)                    
            {
                _objUsersPermission.IsActive = chkActive.Checked;
                _objUsersPermission.Reason = Convert.ToString(txtReason.Value);
            }


            //*** Locations and Departments ***//
            List<PermissionAccess> _objPermissionAccessList = new List<PermissionAccess>();
            foreach (TreeNode node in tvLocations.Nodes)
            {          

                //If node has child nodes
                if (node.Checked == true)   //it is better to first check if it is "checked" then proceed to count child nodes
                {
                     int _objLocationId = string.IsNullOrEmpty(node.Value) ? 0 : Convert.ToInt32(node.Value);                   

                    if (node.ChildNodes.Count > 0)   //check if node has any child nodes
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            if (childNode.Checked == true)
                            {
                                PermissionAccess _objPermissionAccess = new PermissionAccess();
                                _objPermissionAccess.LocationId = _objLocationId;
                                _objPermissionAccess.DepartmentId = string.IsNullOrEmpty(childNode.Value) ? 0 : Convert.ToInt32(childNode.Value);
                                _objPermissionAccessList.Add(_objPermissionAccess);
                            }
                        }
                    }                    
                }
               
            }

            //*** Clients ***//
            List<ClientAccess> _objClientAccessList = new List<ClientAccess>();
            foreach (TreeNode node in tvClients.Nodes)
            {                
                if (node.Checked == true)  
                {
                    if (node.ChildNodes.Count > 0)   //check if node has any child nodes
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            if (childNode.Checked == true)
                            {
                                _objClientAccessList.Add(new ClientAccess { ClientId = string.IsNullOrEmpty(childNode.Value) ? 0 : Convert.ToInt32(childNode.Value) });
                            }
                        }
                    }
                }
            }

            //*** Reports ***//
            List<ReportAccess> _objReportAccessList = new List<ReportAccess>();
            foreach (TreeNode node in tvReports.Nodes)
            {                
                if (node.Checked == true)   
                {
                    if (node.ChildNodes.Count > 0)   //check if node has any child nodes
                    {
                        //Check all the child nodes.
                        foreach (TreeNode childNode in node.ChildNodes)
                        {
                            if (childNode.Checked == true)
                            {
                                _objReportAccessList.Add(new ReportAccess { ReportId = string.IsNullOrEmpty(childNode.Value) ? 0 : Convert.ToInt32(childNode.Value) });
                            }
                        }
                    }
                }
            }

            IUsersProfile mUserProfile = null;
            mUserProfile = AppService.Create<IUsersProfile>();
            mUserProfile.AppManager = this.mAppManager;

            mUserProfile.AddEditUserPermissions(_objUsersPermission, _objPermissionAccessList, _objClientAccessList, _objReportAccessList);
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

    private void ValidatePermission()
    {
        try
        {

            ValidationException exception = new ValidationException(VALIDATIONERROR);

            //*** Locations ***//
            bool isLocationChecked = false;
            foreach (TreeNode node in tvLocations.Nodes)
            {
                if (node.Checked == true)
                {
                    isLocationChecked = true;
                }
            }

            if (!isLocationChecked)
                exception.Data.Add("LOCATIONS", ALERTMSGFIELD + " " +  lblLocations.Text);

            
            //*** Client ***//
            bool isClientChecked = false;
            foreach (TreeNode node in tvClients.Nodes)
            {
                if (node.Checked == true)
                {
                    isClientChecked = true;
                }
            }

            if (!isClientChecked)
                exception.Data.Add("CLIENTS", ALERTMSGFIELD + " " + lblClients.Text);


            //*** Report ***//
            bool isReportChecked = false;
            foreach (TreeNode node in tvReports.Nodes)
            {
                if (node.Checked == true)
                {
                    isReportChecked = true;
                }
            }

            if (!isReportChecked)
                exception.Data.Add("REPORTS", ALERTMSGFIELD + " " + lblReports.Text);


            if (UIMODEPERMISSION == UIMODEPERMISSION.EDIT)
            {
                if(string.IsNullOrEmpty(Convert.ToString(txtReason.Value)))
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

    #endregion
}