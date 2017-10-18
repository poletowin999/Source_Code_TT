using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;

using Tks.Model;
using Tks.Entities;
using Tks.Services;


public partial class Masters_CommonMastersListView : System.Web.UI.Page
{
    #region Class variables

    IAppManager mAppManager;
    IList mSearchResult;
    // constants
    const string ROLE_MASTER = "role";
    const string PLATFORM_MASTER = "Platform";
    const string DEPARTMENT_MASTER = "Department";
    const string TEST_MASTER = "test";
    const string BILLING_TYPE_MASTER = "Billingtype";
    const string LANGUAGE_MASTER = "language";
    //const string WORK_TYPE_MASTER = "worktype";


    private string ROLE_Name = "Role:";
    private string PLATFORM_Name = "Platform:";
    private string DEPARTMENT_Name = "Department:";
    private string TEST_Name = "Test:";
    private string BILLING_Name = "Billingtype:";
    private string LANGUAGE_Name = "Language:";

    private string GRID_ROLES_TITLE = "Roles";
    private string GRID_PLATFORMS_TITLE = "Platforms";
    private string GRID_DEPARTMENT_TITLE = "Department";
    private string GRID_TESTS_TITLE = "Tests";
    private string GRID_BILLINGTYPES_TITLE = "Billing Types";
    //const string GRID_WORKTYPES_TITLE = "Work Types";
    private string GRID_LANGUAGES_TITLE = "Languages";

    private string mSelectedMaster;

    private string mSelectedGridTitle;

    private string mDialogTitle;

    string ADDModeDisplay;
    string ADDeditModeDisplay;

    string NameAlert;
    string CommentsAlert;
    string ADDEDMSG;
    string UPDATEMSG;

    string cntlist;
    string cntFound;

    string spnNameRole;
    string spnNamePlatform;
    string spnNameDepartment;
    string spnNameTest;
    string spnNameBillingtype;
    string spnNameLanguage;

    #endregion


    #region Internal members

    private void InitializeView()
    {
        try
        {

            // TODO: Don't use.
            UserAuthentication authentication = new UserAuthentication();
            mAppManager = Session["APP_MANAGER"] as IAppManager;


            //this.hideSpan.InnerHtml = "Enter the criteria and click on Search button to view data.";
            // retrieve master type from query string -- by ashok
            //if (!string.IsNullOrEmpty(Request.QueryString["master"]))
            if (((object)this.Page.RouteData.Values["Master"]) != null)
            {
                string aa;
                aa = this.mAppManager.LoginUser.HasAdminRights.ToString();

                if (aa == "False")
                {
                    ErrorLogProvider provider = null;
                    try
                    {
                        //create a exception.
                        Exception exception = HttpContext.Current.Error;

                        //insert into Error log.
                        provider = new ErrorLogProvider();
                        provider.AppManager = this.mAppManager;
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
                    //                this.DisplayMessage("YOU DO NOT HAVE ACCESS TO THIS PAGE");
                    Response.Redirect("~/Homepage.aspx");

                }

                mSelectedMaster = Convert.ToString(this.Page.RouteData.Values["Master"]); // Request.QueryString["master"];

                switch (mSelectedMaster.ToLower())
                {
                    case "role":
                        mSelectedMaster = ROLE_MASTER;
                        mSelectedGridTitle = GRID_ROLES_TITLE;
                        this.spnTitle.InnerText = GRID_ROLES_TITLE;
                        this.spnName.InnerText = spnNameRole;
                        //lblName.Text = "aaa";
                        break;
                    case "platform":
                        mSelectedMaster = PLATFORM_MASTER;
                        mSelectedGridTitle = GRID_PLATFORMS_TITLE;
                        this.spnTitle.InnerText = GRID_PLATFORMS_TITLE;
                        this.spnName.InnerText = spnNamePlatform;
                     //   lblName.Text = "bbb";
                        break;
                    case "department":
                        mSelectedMaster = DEPARTMENT_MASTER;
                        mSelectedGridTitle = GRID_DEPARTMENT_TITLE;
                        this.spnTitle.InnerText = GRID_DEPARTMENT_TITLE;
                        this.spnName.InnerText = spnNameDepartment;
                        //lblName.Text = "ccc";
                        break;
                    case "test":
                        mSelectedMaster = TEST_MASTER;
                        mSelectedGridTitle = GRID_TESTS_TITLE;
                        this.spnTitle.InnerText = GRID_TESTS_TITLE;
                        this.spnName.InnerText = spnNameTest;
                        
                        break;
                    case "billingtype":
                        mSelectedMaster = BILLING_TYPE_MASTER;
                        mSelectedGridTitle = GRID_BILLINGTYPES_TITLE;
                        this.spnTitle.InnerText = GRID_BILLINGTYPES_TITLE;
                        this.spnName.InnerText = spnNameBillingtype;
                        break;
                    case "language":
                        mSelectedMaster = LANGUAGE_MASTER;
                        mSelectedGridTitle = GRID_LANGUAGES_TITLE;
                        this.spnTitle.InnerText = GRID_LANGUAGES_TITLE;
                        this.spnName.InnerText = spnNameLanguage;
                        break;
                    //case "worktype":
                    //    mSelectedMaster = WORK_TYPE_MASTER;
                    //    mSelectedGridTitle = GRID_WORKTYPES_TITLE;
                    //    this.spnTitle.InnerText = GRID_WORKTYPES_TITLE;
                    //    break;
                    default:
                        mSelectedMaster = ROLE_MASTER;
                        mSelectedGridTitle = GRID_ROLES_TITLE;
                        this.spnTitle.InnerText = GRID_ROLES_TITLE;
                        this.spnName.InnerText = spnNameRole;
                        break;
                }
            }
            else
                mSelectedMaster = ROLE_MASTER; // default

            // Disable and clear the status.
            this.HideStatusMessage(false);

            this.hdrGridHeader.Visible = false;

            if (!Page.IsPostBack)
            {
                this.PanelEditEntityEnable(false);
                this.SetFocus(this.txtName);

                this.DisableErrorValidation(false);
            }

        }
        catch { throw; }
    }

    // Search Entities
    private void SearchEntity(string name, string status)
    {
        try
        {
            // Validate search criteria.

            if (!this.ValidateSearchCriteria())
            {
                hideSpan.Visible = false;
                return;
            }

            // Based on master view.
            switch (mSelectedMaster)
            {
                case ROLE_MASTER:
                    this.mSearchResult = this.SearchRole(name, status);
                    break;
                case PLATFORM_MASTER:
                    this.mSearchResult = this.SearchPlatform(name, status);
                    break;
                case DEPARTMENT_MASTER:
                    this.mSearchResult = this.SearchDepartment(name, status);
                    break;
                case TEST_MASTER:
                    this.mSearchResult = this.SearchTest(name, status);
                    break;
                case BILLING_TYPE_MASTER:
                    this.mSearchResult = this.SearchBillingType(name, status);
                    break;
                case LANGUAGE_MASTER:
                    this.mSearchResult = this.SearchLanguage(name, status);
                    break;
                //case WORK_TYPE_MASTER:
                //    this.mSearchResult = this.SearchWorkType(name, status);
                //    break;
            }

            // Display search result.
            this.DisplaySearchResult(this.mSearchResult);

            // edit panel
            this.PanelEditEntityEnable(false);

            this.divSuccessMessage.InnerHtml = string.Empty;

            if (gvwEntityList.Rows.Count > 0)
                this.hdrGridHeader.Visible = true;
            else
                this.hdrGridHeader.Visible = true;
        }
        catch { throw; }
    }

    private void DisplaySearchResult(IList result)
    {
        try
        {
            // Set grid title.
            if (result == null)
                this.hdrGridHeader.InnerText = string.Format(spnName.InnerText + " " + cntlist + " ({0} " + cntFound + ")", 0);
            else
            {
                // set visibility
                this.hdrGridHeader.Visible = true;
                this.hdrGridHeader.InnerText = string.Format(spnName.InnerText + " " + cntlist + " ({0} " + cntFound + ")", result.Count);
            }

            // Bind with grid.
            this.gvwEntityList.DataSource = result;
            this.gvwEntityList.DataBind();
        }
        catch { throw; }
    }

    private void AddEntity()
    {
        try
        {
            if (gvwEntityList.Rows.Count > 0)
                this.hdrGridHeader.Visible = true;
            else
                this.hdrGridHeader.Visible = false;

            // edit panel
            this.PanelEditEntityEnable(false);

            // hide controls for add mode
            this.PanelEnable(false);

            // clear
            this.ClearControls();

            // set 0 for adding new data
            hiddenEntityId.Value = "0";

            this.divSuccessMessage.InnerText = string.Empty;

            this.AddEditMode("Add New");
        }
        catch { throw; }
    }

    private void AddEditMode(string mode)
    {
        try
        {
            string modeLanguage;

            if (mode != "Add New")
            {
                btnUpdate.Text = ADDeditModeDisplay;
                modeLanguage = ADDeditModeDisplay;
            }

            else
            {
                btnUpdate.Text = ADDModeDisplay;
                modeLanguage = ADDModeDisplay;
            }

            //  modeLanguage = ADDeditModeDisplay;



            switch (mSelectedMaster.ToLower())
            {
                case "role":
                    mDialogTitle = modeLanguage + " " + spnNameRole;
                    break;
                case "platform":
                    mDialogTitle = modeLanguage + " " + spnNamePlatform;
                    break;
                case "department":
                    mDialogTitle = modeLanguage + " " + spnNameDepartment;
                    break;
                case "test":
                    mDialogTitle = modeLanguage + " " + spnNameTest;
                    break;
                case "billingtype":
                    mDialogTitle = modeLanguage + " " + spnNameBillingtype;
                    break;
                case "language":
                    mDialogTitle = modeLanguage + " " + spnNameLanguage;
                    break;
                //case "worktype":
                //    mDialogTitle = mode + " " + GRID_WORKTYPES_TITLE;
                //    break;
                default:
                    mDialogTitle = modeLanguage + " " + spnNameRole;
                    break;
            }
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "BILLING");
        if (lblLanguagelst != null)
        {
            Utility _objUtil = new Utility();
            _objUtil.LoadLabels(lblLanguagelst);

            //private string GRID_ROLES_TITLE = "Roles";
            //const string GRID_PLATFORMS_TITLE = "Platforms";
            //const string GRID_DEPARTMENT_TITLE = "Department";
            //const string GRID_TESTS_TITLE = "Tests";
            //const string GRID_BILLINGTYPES_TITLE = "Billing Types";
            ////const string GRID_WORKTYPES_TITLE = "Work Types";
            //const string GRID_LANGUAGES_TITLE = "Languages";


            var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
            if (GRID_TITLE != null)
            {

                this.hideSpan.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
            }

            var ROLES_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("GRID_ROLES_TITLE")).FirstOrDefault();
            if (ROLES_TITLE != null)
            {

                GRID_ROLES_TITLE = Convert.ToString(ROLES_TITLE.DisplayText);
                //ROLE_Name = Convert.ToString(ROLES_TITLE.SupportingText1);
                ROLE_Name = Convert.ToString(ROLES_TITLE.DisplayText);
                spnNameRole = Convert.ToString(ROLES_TITLE.SupportingText1);

            }

            var PLATFORMS_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("GRID_PLATFORMS_TITLE")).FirstOrDefault();
            if (PLATFORMS_TITLE != null)
            {
                GRID_PLATFORMS_TITLE = Convert.ToString(PLATFORMS_TITLE.DisplayText);
                PLATFORM_Name = Convert.ToString(PLATFORMS_TITLE.DisplayText);
                spnNamePlatform = Convert.ToString(PLATFORMS_TITLE.SupportingText1);
            }

            var DEPARTMENT_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("GRID_DEPARTMENT_TITLE")).FirstOrDefault();
            if (DEPARTMENT_TITLE != null)
            {
                GRID_DEPARTMENT_TITLE = Convert.ToString(DEPARTMENT_TITLE.DisplayText);
                //ROLE_Name = Convert.ToString(ROLES_TITLE.SupportingText1);
                DEPARTMENT_Name = Convert.ToString(DEPARTMENT_TITLE.DisplayText);
                spnNameDepartment = Convert.ToString(DEPARTMENT_TITLE.SupportingText1);
            }

            var TESTS_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("GRID_TESTS_TITLE")).FirstOrDefault();
            if (TESTS_TITLE != null)
            {
                GRID_TESTS_TITLE = Convert.ToString(TESTS_TITLE.DisplayText);
                TEST_Name = Convert.ToString(TESTS_TITLE.DisplayText);
                spnNameTest = Convert.ToString(TESTS_TITLE.SupportingText1);
            }

            var BILLINGTYPES_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("GRID_BILLINGTYPES_TITLE")).FirstOrDefault();
            if (BILLINGTYPES_TITLE != null)
            {
                GRID_BILLINGTYPES_TITLE = Convert.ToString(BILLINGTYPES_TITLE.DisplayText);
                BILLING_Name = Convert.ToString(BILLINGTYPES_TITLE.DisplayText);
                spnNameBillingtype = Convert.ToString(BILLINGTYPES_TITLE.SupportingText1);
            }

            var LANGUAGES_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("GRID_LANGUAGES_TITLE")).FirstOrDefault();
            if (LANGUAGES_TITLE != null)
            {
                GRID_LANGUAGES_TITLE = Convert.ToString(LANGUAGES_TITLE.DisplayText);
                LANGUAGE_Name = Convert.ToString(LANGUAGES_TITLE.DisplayText);
                spnNameLanguage = Convert.ToString(LANGUAGES_TITLE.SupportingText1);
            }

            var BTNTEXTVALUE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("BTNUPDATE")).FirstOrDefault();
            if (BTNTEXTVALUE != null)
            {
                ADDModeDisplay = Convert.ToString(BTNTEXTVALUE.DisplayText);
                ADDeditModeDisplay = Convert.ToString(BTNTEXTVALUE.SupportingText1);
                
            }

            var TXTALERT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("TXTALERTMSG")).FirstOrDefault();
            if (TXTALERT != null)
            {
                NameAlert = Convert.ToString(TXTALERT.DisplayText);
                CommentsAlert = Convert.ToString(TXTALERT.SupportingText1);
            }

            var ADDUPDATE_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ADDUPDATEMSG")).FirstOrDefault();
            if (ADDUPDATE_MSG != null)
            {
                ADDEDMSG = Convert.ToString(ADDUPDATE_MSG.SupportingText1);
                UPDATEMSG = Convert.ToString(ADDUPDATE_MSG.DisplayText);
            }

            var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
            if (NoofRecordFound != null)
            {
                cntlist = NoofRecordFound.DisplayText;
                cntFound = NoofRecordFound.SupportingText1;
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add in session.

            // Disable The Refresh Control.
            this.DisableRefershControl();

        }
        catch { throw; }
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

    protected void btnSearch_ServerClick(object sender, EventArgs e)
    {
        try
        {
            this.SearchEntity(this.txtName.Value.Trim(), this.ddlStatus.Items[ddlStatus.SelectedIndex].Value);


            if (gvwEntityList.Rows.Count == 0)
            {
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = string.Empty;
                //this.InitalspnMessage.InnerHtml = "No Data Found";
                this.divEmptyRow.Visible = true;
            }
            else
            {
                this.hideSpan.Visible = false;
                this.divEmptyRow.Visible = false;
                this.dvinitalvalue.Visible = false;
            }
        }
        catch { throw; }
    }

    protected void btnUpdate_ServerClick(object sender, EventArgs e)
    {
        try
        {

            // update info
            if (hiddenEntityId.Value != "0")
            {
                this.UpdateEntity(Int32.Parse(hiddenEntityId.Value.ToString()));
                btnUpdate.Text = ADDeditModeDisplay;
  //              modeLanguage = ADDeditModeDisplay;
            }
            else
            {
                this.UpdateEntity(0); // for inserting new record   
                btnUpdate.Text = ADDModeDisplay;
//                modeLanguage = ADDModeDisplay;
            }

            //this.btnSearch_ServerClick(sender, e);
            //this.SearchEntity(this.txtName.Value.Trim(), this.ddlStatus.Items[ddlStatus.SelectedIndex].Value);

            if (gvwEntityList.Rows.Count > 0)
            {
                //this.hdrGridHeader.Visible = true;
            }





        }


        catch { throw; }
    }

    protected void btnClear_ServerClick(object sender, EventArgs e)
    {
        try
        {
            // clear
            this.ClearControls();
            this.InitalspnMessage.InnerHtml = "";

            // set 0 for adding new data
            hiddenEntityId.Value = "0";

            // hide controls for add mode
            this.PanelEditEntityEnable(false);

            gvwEntityList.DataSource = null;
            gvwEntityList.DataBind();

            this.dvinitalvalue.Visible = true;

            txtName.Value = string.Empty;

            this.divSuccessMessage.InnerText = string.Empty;
            this.DisableErrorValidation(false);
        }
        catch { throw; }
    }

    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        try
        {
            // clear
            this.ClearControls();

            // hide controls
            this.PanelEnable(false);

            // set "0" for adding new data
            hiddenEntityId.Value = "0";

            if (gvwEntityList.Rows.Count > 0)
                this.hdrGridHeader.Visible = true;

            // hide controls 
            this.PanelEditEntityEnable(false);
        }
        catch { throw; }
    }

    protected void gvwEntityList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.MasterEntity Entity = (Tks.Entities.MasterEntity)e.Row.DataItem;
                HtmlGenericControl active = (HtmlGenericControl)e.Row.FindControl("spnIsActive");

                //Bind the Location is Active or not
                active.InnerText = Entity.IsActive == true ? "Active" : "InActive";

            }
        }
        catch
        { throw; }

    }

    protected void gvwEntityList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            this.hdrGridHeader.Visible = true;
            // selected id
            string selectedId = null;
            if (e.CommandName.Equals("ClientEdit", StringComparison.InvariantCultureIgnoreCase))
            {

                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                    selectedId = gvwEntityList.DataKeys[Int32.Parse(e.CommandArgument.ToString())]["Id"].ToString();

                if (selectedId != null)
                {
                    // edit mode
                    this.AddEditMode("Edit");

                    // edit panel
                    this.PanelEditEntityEnable(true);

                    // Retrieve Entity Details
                    this.RetrieveEntityInfo(Int32.Parse(selectedId.ToString()));

                    this.DisplayEntityEditPanel();

                    //clear the message.
                    this.divSuccessMessage.InnerText = string.Empty;

                    //Button text Changed
                    // this.btnUpdate.Text = "Update";
                    
                }
            }
        }
        catch { throw; }
    }

    protected void gvwEntityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvwEntityList.PageIndex = e.NewPageIndex;

            this.SearchEntity(txtName.Value, ddlStatus.Value);
        }
        catch { throw; }

    }

    protected void lbtAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            this.AddEntity();

            this.DisplayEntityEditPanel();
            //    this.btnUpdate.Text = "Add";
            this.DisableErrorValidation(false);


        }
        catch { throw; }
    }

    protected void lbtRefreshList_Click(object sender, EventArgs e)
    {

        try
        {
            // clear
            this.ClearControls();

            // set 0 for adding new data
            hiddenEntityId.Value = "0";
            // Display search result.
            this.DisplaySearchResult(this.mSearchResult);
            this.DisableErrorValidation(false);
            this.SearchEntity(txtName.Value, ddlStatus.Value);
            if (!string.IsNullOrEmpty(txtName.Value.Trim()))
                this.SearchEntity(txtName.Value, ddlStatus.Value);

            if (gvwEntityList.Rows.Count == 0)
            {
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = "";
                //                this.InitalspnMessage.InnerHtml = "<b>No Data Found</b>";
            }
            else
            {
                this.dvinitalvalue.Visible = false;
                this.hdrGridHeader.Visible = true;
            }

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value, ddlStatus.Value);
        }
        catch { throw; }
    }

    // Retrieve Entity Details
    private void RetrieveEntityInfo(int selectedId)
    {
        try
        {
            // set div visibility for edit mode
            this.PanelEnable(true);

            // Retrieve data based on master             

            switch (mSelectedMaster)
            {
                case ROLE_MASTER:
                    this.DisplayRoleInfo(selectedId);
                    break;
                case PLATFORM_MASTER:
                    this.DisplayPlatformInfo(selectedId);
                    break;
                case DEPARTMENT_MASTER:
                    this.DisplayDepartmentInfo(selectedId);
                    break;
                case TEST_MASTER:
                    this.DisplayTest(selectedId);
                    break;
                case BILLING_TYPE_MASTER:
                    this.DisplayBillingType(selectedId);
                    break;
                case LANGUAGE_MASTER:
                    this.DisplayLanguage(selectedId);
                    break;
                //case WORK_TYPE_MASTER:
                //    this.DisplayWorkType(selectedId);
                //    break;
            }
        }
        catch { throw; }
    }

    private void UpdateEntity(int selectedId)
    {
        try
        {
            // Update based on master
            switch (mSelectedMaster)
            {
                case ROLE_MASTER:
                    this.UpdateRoleInfo(selectedId);
                    break;
                case PLATFORM_MASTER:
                    this.UpdatePlatformInfo(selectedId);
                    break;
                case DEPARTMENT_MASTER:
                    this.UpdateDepartmentInfo(selectedId);
                    break;
                case TEST_MASTER:
                    this.UpdateTestInfo(selectedId);
                    break;
                case BILLING_TYPE_MASTER:
                    this.UpdateBillingTypeInfo(selectedId);
                    break;
                case LANGUAGE_MASTER:
                    this.UpdateLanguageInfo(selectedId);
                    break;
                //case WORK_TYPE_MASTER:
                //    this.UpdateWorkTypeInfo(selectedId);
                //    break;
                default:
                    this.UpdateRoleInfo(selectedId);
                    break;
            }
        }

        catch { throw; }
    }

    # region Role Master

    // Search Role
    private List<Role> SearchRole(string name, string status)
    {
        IRoleService service = null;
        try
        {

            // Create service.
            service = AppService.Create<IRoleService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Search(
              new MasterEntitySearchCriteria()
                    {
                        Name = name,
                        Status = status
                    });
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    // Display Role Info
    private void DisplayRoleInfo(int id)
    {
        IRoleService roleService = null;
        Role mRole = null;

        try
        {
            // Create service.
            roleService = AppService.Create<IRoleService>();
            roleService.AppManager = this.mAppManager;

            // retrieve data
            mRole = new Role(id);
            mRole = roleService.Retrieve(id);

            hiddenEntityId.Value = "0";
            if (mRole != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = Convert.ToString(mRole.Name);
                txtEntityDescription.Value = Convert.ToString(mRole.Description);
                chkEntityActive.Checked = Convert.ToBoolean(mRole.IsActive);
                //txtEntityReason.Value = string.Empty;
            }
        }
        catch { throw; }
        finally
        {
            if (roleService != null) roleService.Dispose();
            roleService = null;
        }
    }

    // Update Role info
    private void UpdateRoleInfo(int id)
    {
        IRoleService roleService = null;
        Role mRole = null;

        // hide status msg
        this.HideStatusMessage(false);

        try
        {

            // validate controls
            if (!ValidateControls())
                return;

            // create instance
            mRole = new Role(id);

            mRole.Name = txtEntityName.Value.ToString().Trim();
            mRole.Description = txtEntityDescription.Value.ToString().Trim();
            mRole.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
            mRole.Reason = txtEntityReason.Value.ToString().Trim();

            //mRole.LastUpdateUserId = Int32.Parse(this.mAppManager.LoginUser.Id.ToString());          
            mRole.LastUpdateUserId = 1; // for testing

            // call service
            roleService = AppService.Create<IRoleService>();
            roleService.AppManager = this.mAppManager;

            // data to update
            roleService.Update(mRole);

            // clear
            this.ClearControls();

            if (hiddenEntityId.Value != "0")
                // display success
                DisplaySuccess();
            else
                DisplayInsertSuccess();

            // close dialog
            this.CloseDialogControl();

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value,ddlStatus.Value);

        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
        }
        catch { throw; }
        finally
        {
            if (roleService != null) roleService.Dispose();
            roleService = null;
        }
    }

    # endregion

    #region Platform Master

    // Search Plaform
    private List<Platform> SearchPlatform(string name, string status)
    {
        IPlatformService service = null;
        try
        {
            // Create service.
            service = AppService.Create<IPlatformService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Search(
                    new MasterEntitySearchCriteria()
                    {
                        Name = name,
                        Status = status
                    });
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    // Display Platform Info
    private void DisplayPlatformInfo(int id)
    {
        IPlatformService platformService = null;
        Platform mPlatform = null;

        try
        {
            // Create service.
            platformService = AppService.Create<IPlatformService>();
            platformService.AppManager = this.mAppManager;

            // retrieve data
            mPlatform = new Platform(id);
            mPlatform = platformService.Retrieve(id);

            hiddenEntityId.Value = "0";
            if (mPlatform != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = Convert.ToString(mPlatform.Name);
                txtEntityDescription.Value = Convert.ToString(mPlatform.Description);
                chkEntityActive.Checked = Convert.ToBoolean(mPlatform.IsActive);
                //txtEntityReason.Value = string.Empty;
            }
        }
        catch { throw; }
        finally
        {
            if (platformService != null) platformService.Dispose();
            platformService = null;
        }
    }

    // Update Platform info
    private void UpdatePlatformInfo(int id)
    {
        IPlatformService platformService = null;
        Platform mPlatform = null;

        // hide status msg
        this.HideStatusMessage(false);

        try
        {

            // validate controls
            if (!ValidateControls())
                return;

            // create instance
            mPlatform = new Platform(id);

            mPlatform.Name = txtEntityName.Value.ToString().Trim();
            mPlatform.Description = txtEntityDescription.Value.ToString().Trim();
            mPlatform.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
            mPlatform.Reason = txtEntityReason.Value.ToString().Trim();

            //mPlatform.LastUpdateUserId = Int32.Parse(mAppManager.LoginUser.Id.ToString());          
            mPlatform.LastUpdateUserId = 1; // for testing

            // call service
            platformService = AppService.Create<IPlatformService>();
            platformService.AppManager = this.mAppManager;

            // data to update
            platformService.Update(mPlatform);

            // clear
            this.ClearControls();

            if (id != 0)
                // display success
                DisplaySuccess();
            else
                DisplayInsertSuccess();

            // close dialog
            this.CloseDialogControl();

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value,ddlStatus.Value);

        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
        }
        catch { throw; }
        finally
        {
            if (platformService != null) platformService.Dispose();
            platformService = null;
        }
    }

    # endregion

    #region Department Master

    // Search Department
    private List<Department> SearchDepartment(string name, string status)
    {
        IDepartmentService service = null;
        try
        {
            // Create service.
            service = AppService.Create<IDepartmentService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Searchdpt(
                    new MasterEntitySearchCriteria()
                    {
                        Name = name,
                        Status = status
                    });
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    // Display Department Info
    private void DisplayDepartmentInfo(int id)
    {
        IDepartmentService DepartmentService = null;
        Department mDepartment = null;

        try
        {
            // Create service.
            DepartmentService = AppService.Create<IDepartmentService>();
            DepartmentService.AppManager = this.mAppManager;

            // retrieve data
            mDepartment = new Department(id);
            mDepartment = DepartmentService.Retrieve(id);

            hiddenEntityId.Value = "0";
            if (mDepartment != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = Convert.ToString(mDepartment.Name);
                txtEntityDescription.Value = Convert.ToString(mDepartment.Description);
                chkEntityActive.Checked = Convert.ToBoolean(mDepartment.IsActive);
                //txtEntityReason.Value = string.Empty;
            }
        }
        catch { throw; }
        finally
        {
            if (DepartmentService != null) DepartmentService.Dispose();
            DepartmentService = null;
        }
    }

    // Update Department info
    private void UpdateDepartmentInfo(int id)
    {
        IDepartmentService DepartmentService = null;
        Department mDepartment = null;

        // hide status msg
        this.HideStatusMessage(false);

        try
        {

            // validate controls
            if (!ValidateControls())
                return;

            // create instance
            mDepartment = new Department(id);

            mDepartment.Name = txtEntityName.Value.ToString().Trim();
            mDepartment.Description = txtEntityDescription.Value.ToString().Trim();
            mDepartment.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
            mDepartment.Reason = txtEntityReason.Value.ToString().Trim();

            //mDepartment.LastUpdateUserId = Int32.Parse(mAppManager.LoginUser.Id.ToString());          
            mDepartment.LastUpdateUserId = 1; // for testing

            // call service
            DepartmentService = AppService.Create<IDepartmentService>();
            DepartmentService.AppManager = this.mAppManager;

            // data to update
            DepartmentService.Update(mDepartment);

            //            this.btnSearch_ServerClick(sender, e);
            //this.SearchEntity(this.txtName.Value.Trim(), this.ddlStatus.Items[ddlStatus.SelectedIndex].Value);

            // clear
            this.ClearControls();

            if (id != 0)
            // display success
            {
                DisplaySuccess();
                this.SearchEntity(this.txtName.Value.Trim(), this.ddlStatus.Items[ddlStatus.SelectedIndex].Value);
            }
            else
            {
                DisplayInsertSuccess();
                this.SearchEntity(this.txtName.Value.Trim(), this.ddlStatus.Items[ddlStatus.SelectedIndex].Value);
            }

            // close dialog
            this.CloseDialogControl();

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value,ddlStatus.Value);

        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
        }
        catch { throw; }
        finally
        {
            if (DepartmentService != null) DepartmentService.Dispose();
            DepartmentService = null;
        }
    }

    # endregion

    #region Test Master

    // Search Test
    private List<Test> SearchTest(string name, string status)
    {
        ITestService service = null;
        try
        {
            // Create service.
            service = AppService.Create<ITestService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Search(
                    new MasterEntitySearchCriteria()
                    {
                        Name = name,
                        Status = status
                    });

        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    // Display Test Info
    private void DisplayTest(int id)
    {
        ITestService testService = null;
        Test mTest = null;

        try
        {
            // Create service.
            testService = AppService.Create<ITestService>();
            testService.AppManager = this.mAppManager;

            // retrieve data
            mTest = new Test(id);
            mTest = testService.Retrieve(id);

            hiddenEntityId.Value = "0";
            if (mTest != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = Convert.ToString(mTest.Name);
                txtEntityDescription.Value = Convert.ToString(mTest.Description);
                chkEntityActive.Checked = Convert.ToBoolean(mTest.IsActive);
                //txtEntityReason.Value = string.Empty;
            }
        }
        catch { throw; }
        finally
        {
            if (testService != null) testService.Dispose();
            testService = null;
        }
    }

    // Update Test info
    private void UpdateTestInfo(int id)
    {
        ITestService testService = null;
        Test mTest = null;

        // hide status msg
        this.HideStatusMessage(false);

        try
        {

            // validate controls
            if (!ValidateControls())
                return;

            // create instance
            mTest = new Test(id);

            mTest.Name = txtEntityName.Value.ToString().Trim();
            mTest.Description = txtEntityDescription.Value.ToString().Trim();
            mTest.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
            mTest.Reason = txtEntityReason.Value.ToString().Trim();

            mTest.LastUpdateUserId = Int32.Parse(mAppManager.LoginUser.Id.ToString());
            //mTest.LastUpdateUserId = 1; // for testing

            // call service
            testService = AppService.Create<ITestService>();
            testService.AppManager = this.mAppManager;

            // data to update
            testService.Update(mTest);

            // clear
            this.ClearControls();

            if (id != 0)
                // display success
                DisplaySuccess();
            else
                DisplayInsertSuccess();

            // close dialog
            this.CloseDialogControl();

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value,ddlStatus.Value);

        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
        }
        catch { throw; }
        finally
        {
            if (testService != null) testService.Dispose();
            testService = null;
        }
    }

    # endregion

    #region BillingType Master

    // Search BillingType
    private List<BillingType> SearchBillingType(string name, string status)
    {
        IBillingTypeService service = null;
        try
        {
            // Create service.
            service = AppService.Create<IBillingTypeService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Search(
                    new MasterEntitySearchCriteria()
                    {
                        Name = name,
                        Status = status
                    });

        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    // Display BillingType Info
    private void DisplayBillingType(int id)
    {
        IBillingTypeService billingTypeService = null;
        BillingType mBillingType = null;

        try
        {
            // Create service.
            billingTypeService = AppService.Create<IBillingTypeService>();
            billingTypeService.AppManager = this.mAppManager;

            // retrieve data
            mBillingType = new BillingType(id);
            mBillingType = billingTypeService.Retrieve(id);

            hiddenEntityId.Value = "0";
            if (mBillingType != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = Convert.ToString(mBillingType.Name);
                txtEntityDescription.Value = Convert.ToString(mBillingType.Description);
                chkEntityActive.Checked = Convert.ToBoolean(mBillingType.IsActive);
                //txtEntityReason.Value = string.Empty;
            }
        }
        catch { throw; }
        finally
        {
            if (billingTypeService != null) billingTypeService.Dispose();
            billingTypeService = null;
        }
    }

    // Update BillingType info
    private void UpdateBillingTypeInfo(int id)
    {
        IBillingTypeService billingTypeService = null;
        BillingType mBillingType = null;

        // hide status msg
        this.HideStatusMessage(false);

        try
        {

            // validate controls
            if (!ValidateControls())
                return;

            // create instance
            mBillingType = new BillingType(id);

            mBillingType.Name = txtEntityName.Value.ToString().Trim();
            mBillingType.Description = txtEntityDescription.Value.ToString().Trim();
            mBillingType.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
            mBillingType.Reason = txtEntityReason.Value.ToString().Trim();

            mBillingType.LastUpdateUserId = ((IAppManager)Session["APP_MANAGER"]).LoginUser.Id;


            //mBillingType.LastUpdateUserId = 1; // for testing

            // call service
            billingTypeService = AppService.Create<IBillingTypeService>();
            billingTypeService.AppManager = this.mAppManager;

            // data to update
            billingTypeService.Update(mBillingType);

            // clear
            this.ClearControls();

            if (id != 0)
                // display success
                DisplaySuccess();
            else
                DisplayInsertSuccess();

            // close dialog
            this.CloseDialogControl();

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value,ddlStatus.Value);
        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
        }
        catch { throw; }
        finally
        {
            if (billingTypeService != null) billingTypeService.Dispose();
            billingTypeService = null;
        }
    }

    # endregion

    #region Language Master

    // Search Language
    private List<Language> SearchLanguage(string name, string status)
    {
        ILanguageService service = null;
        try
        {
            // Create service.
            service = AppService.Create<ILanguageService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Search(
                    new MasterEntitySearchCriteria()
                    {
                        Name = name,
                        Status = status
                    });
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    // Display Language Info
    private void DisplayLanguage(int id)
    {
        ILanguageService languageService = null;
        Language mLanguage = null;

        try
        {
            // Create service.
            languageService = AppService.Create<ILanguageService>();
            languageService.AppManager = this.mAppManager;

            // retrieve data
            mLanguage = new Language(id);
            mLanguage = languageService.Retrieve(id);

            hiddenEntityId.Value = "0";
            if (mLanguage != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = Convert.ToString(mLanguage.Name);
                txtEntityDescription.Value = Convert.ToString(mLanguage.Description);
                chkEntityActive.Checked = Convert.ToBoolean(mLanguage.IsActive);
                //txtEntityReason.Value = string.Empty;



            }
        }
        catch { throw; }
        finally
        {
            if (languageService != null) languageService.Dispose();
            languageService = null;
        }
    }

    // Update Language info
    private void UpdateLanguageInfo(int id)
    {
        ILanguageService languageService = null;
        Language mLanguage = null;

        // hide status msg
        this.HideStatusMessage(false);

        try
        {

            // validate controls
            if (!ValidateControls())
                return;

            // create instance
            mLanguage = new Language(id);

            mLanguage.Name = txtEntityName.Value.ToString().Trim();
            mLanguage.Description = txtEntityDescription.Value.ToString().Trim();
            mLanguage.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
            mLanguage.Reason = txtEntityReason.Value.ToString().Trim();

            mLanguage.LastUpdateUserId = Int32.Parse(mAppManager.LoginUser.Id.ToString());
            //mLanguage.LastUpdateUserId = 1; // for testing

            // call service
            languageService = AppService.Create<ILanguageService>();
            languageService.AppManager = this.mAppManager;

            // data to update
            languageService.Update(mLanguage);

            // clear
            this.ClearControls();

            if (id != 0)
                // display success
                DisplaySuccess();
            else
                DisplayInsertSuccess();

            // close dialog
            this.CloseDialogControl();

            //if (!string.IsNullOrEmpty(txtName.Value.Trim()))
            //    this.SearchEntity(txtName.Value,ddlStatus.Value);

        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
        }
        catch { throw; }
        finally
        {
            if (languageService != null) languageService.Dispose();
            languageService = null;
        }
    }

    # endregion

    #region WorkType Master

    //// Search WorkType
    //private List<WorkType> SearchWorkType(string name,string status)
    //{
    //    IWorkTypeService service = null;
    //    try
    //    {
    //        // Create service.
    //        service = AppService.Create<IWorkTypeService>();
    //        service.AppManager = this.mAppManager;

    //        // Call service method.
    //        return service.Search(
    //                new MasterEntitySearchCriteria()
    //                {
    //                    Name = name,
    //                    Status=status
    //                });

    //    }
    //    catch { throw; }
    //    finally
    //    {
    //        if (service != null) service.Dispose();
    //    }
    //}

    //// Display WorkType Info
    //private void DisplayWorkType(int id)
    //{
    //    IWorkTypeService workTypeService = null;
    //    WorkType mWorkType = null;

    //    try
    //    {
    //        // Create service.
    //        workTypeService = AppService.Create<IWorkTypeService>();
    //        workTypeService.AppManager = this.mAppManager;

    //        // retrieve data
    //        mWorkType = new WorkType(id);
    //        mWorkType = workTypeService.Retrieve(id);

    //        hiddenEntityId.Value = "0";
    //        if (mWorkType != null)
    //        {
    //            hiddenEntityId.Value = id.ToString();
    //            txtEntityName.Value = Convert.ToString(mWorkType.Name);
    //            txtEntityDescription.Value = Convert.ToString(mWorkType.Description);
    //            chkEntityActive.Checked = Convert.ToBoolean(mWorkType.IsActive);
    //            //txtEntityReason.Value = string.Empty;
    //        }
    //    }
    //    catch { throw; }
    //    finally
    //    {
    //        if (workTypeService != null) workTypeService.Dispose();
    //        workTypeService = null;
    //    }
    //}

    //// Update WorkType info
    //private void UpdateWorkTypeInfo(int id)
    //{
    //    IWorkTypeService workTypeService = null;
    //    WorkType mWorkType = null;

    //    // hide status msg
    //    this.HideStatusMessage(false);

    //    try
    //    {

    //        // validate controls
    //        if (!ValidateControls())
    //            return;

    //        // create instance
    //        mWorkType = new WorkType(id);

    //        mWorkType.Name = txtEntityName.Value.ToString().Trim();
    //        mWorkType.Description = txtEntityDescription.Value.ToString().Trim();
    //        mWorkType.IsActive = (id == 0) ? true : chkEntityActive.Checked; // if id is zero, then set active as true for ading new data
    //        mWorkType.Reason = txtEntityReason.Value.ToString().Trim();

    //        mWorkType.LastUpdateUserId = Int32.Parse(mAppManager.LoginUser.Id.ToString());          
    //       // mWorkType.LastUpdateUserId = 1; // for testing

    //        // call service
    //        workTypeService = AppService.Create<IWorkTypeService>();
    //        workTypeService.AppManager = this.mAppManager;

    //        // data to update
    //        workTypeService.Update(mWorkType);

    //        // clear
    //        this.ClearControls();
    //        if (id != 0)
    //            // display success
    //            DisplaySuccess();
    //        else
    //            DisplayInsertSuccess();

    //        // close dialog
    //        this.CloseDialogControl();

    //        if (!string.IsNullOrEmpty(txtName.Value.Trim()))
    //            this.SearchEntity(txtName.Value,ddlStatus.Value);

    //    }
    //    catch (ValidationException ve)
    //    {
    //        // Display validation error.
    //        this.DisplayValidationError(this.ErrorMessage(ve));
    //    }
    //    catch { throw; }
    //    finally
    //    {
    //        if (workTypeService != null) workTypeService.Dispose();
    //        workTypeService = null;
    //    }
    //}

    # endregion

    #region Common Internal Members

    // validate controls for update
    private Boolean ValidateControls()
    {
        try
        {
            ValidationException exception = new ValidationException(string.Empty);


            if (String.IsNullOrEmpty(txtEntityName.Value.Trim().ToString()))
                exception.Data.Add("Name", NameAlert);

            if (hiddenEntityId.Value != "0" && txtEntityReason.Value.Trim() == "")
                exception.Data.Add("Reason", CommentsAlert);

            if (exception.Data.Count > 0)
                throw exception;

            return true;
        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
            return false;
        }
        catch { throw; }
    }

    // validate for search
    private Boolean ValidateSearchCriteria()
    {
        try
        {
            //create exception instance.

            ValidationException exception = new ValidationException("validation failure");
            if (string.IsNullOrEmpty(txtName.Value) && string.IsNullOrEmpty(ddlStatus.Value))
            {
                exception.Data.Add("Search", "No Data found");
                this.DisableErrorValidation(true);
                //this.divSuccessMessage.InnerHtml = "Enter the criteria and click on Search button to view data.";
            }
            else
                this.DisableErrorValidation(false);

            if (exception.Data.Count > 0)
                throw exception;

            return true;
        }
        catch (ValidationException ve)
        {
            // Display validation error.
            this.DisplayValidationError(this.ErrorMessage(ve));
            return false;
        }
        catch { throw; }
    }

    private string ErrorMessage(ValidationException ve)
    {
        // Build validation error message.
        StringBuilder message = new StringBuilder(string.Format("{0}<ul>", ve.Message));
        foreach (DictionaryEntry entry in ve.Data)
        {
            message.Append(string.Format("<li>{0}</li>", entry.Value));
        }
        message.Append("</ul>");

        return message.ToString();
    }

    // set div visibility
    private void PanelEnable(Boolean enable)
    {
        //divEditModePart.Style.Add("display", "none");
        // divEditModePart.Visible = enable;
    }

    private void PanelEditEntityEnable(Boolean enable)
    {
        //divEditModePart.Style.Add("display", "none");
        this.divEditModePart.Visible = enable;
        //divEntityEditPanel.Visible = enable;
    }

    private void DisplayValidationError(string message)
    {
        // Display message.
        this.divStatusInfo.InnerHtml = message;
        // Show panel.
        this.ShowHideValidationPanel(true);
    }

    // display div
    private void ShowHideValidationPanel(bool visible)
    {
        this.divStatusInfo.Style.Add("display", "block");
        this.divStatusInfo.Visible = visible;
    }

    // clear controls
    private void ClearControls()
    {
        txtEntityName.Value = "";
        txtEntityDescription.Value = "";
        chkEntityActive.Checked = false;
        txtEntityReason.Value = "";
        this.ddlStatus.SelectedIndex = 1;
    }

    // display success msg
    private void DisplaySuccess()
    {
        divSuccess.Style.Add("display", "block");
        divSuccess.Visible = true;
        divSuccess.InnerHtml = UPDATEMSG;
    }
    // display success msg
    private void DisplayInsertSuccess()
    {
        divSuccess.Style.Add("display", "block");
        divSuccess.Visible = true;
        divSuccess.InnerHtml = ADDEDMSG;
    }

    // hide status panel
    private void HideStatusMessage(Boolean enable)
    {
        divStatusInfo.Style.Add("display", "none");
        divStatusInfo.Visible = enable;

        divSuccess.Style.Add("display", "none");
        divSuccess.Visible = enable;
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
                string.Format("showEditPanelDialog({{'width': '400px', 'title': '{0}'}});", mDialogTitle),
                true);
        }
        catch { throw; }
    }
    private void CloseDialogControl()
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), System.Guid.NewGuid().ToString(), "closeEditPanelDialog()", true);
        }
        catch { throw; }
    }

    // Enable (Or) Disable the Referesh control.
    private void DisableRefershControl()
    {
        try
        {
            if (gvwEntityList.Rows.Count > 0)
            {
                this.lbtRefresh.Enabled = true;
                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "BILLING");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwEntityList);
                dvinitalvalue.Visible = false;

            }
            else
            {
                dvinitalvalue.Visible = true;
                this.lbtRefresh.Enabled = false;
            }

        }
        catch { throw; }
    }

    private void DisableErrorValidation(Boolean enable)
    {
        divSuccessMessage.Visible = enable;
    }


    #endregion

    protected void lnkBtn_Click(object sender, EventArgs e)
    {
        

    }
}