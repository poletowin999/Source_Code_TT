using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Masters_WorkTypeListView : System.Web.UI.Page
{
    //Common  variable used through out the page 
    #region Class Variable

    const string LOCATION_SEARCH_RESULT_LIST = "WorkType_Search_Result_List";
    IAppManager mAppManager = null;
    List<Tks.Entities.WorkType> mWorkTypeList = null;
    IList mSearchResult = null;
    private string mSelectedGridTitle = null;
    string mEntityEditPanelHeader = "Add WorkType";
    string ADDEDMSG;
    string UPDATEMSG;

    string NameAlert;
    string CommentsAlert;

    string ActivityTypeAlert;

    string cntlist;
    string cntFound;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            if (mAppManager.LoginUser.HasAdminRights == false)
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
                //   this.DisplayMessage("YOU DO NOT HAVE ACCESS TO THIS PAGE");
                Response.Redirect("~/Homepage.aspx");

            }

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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "WORKTYPE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

               this.hideSpan.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
               this.InitalspnMessage.InnerHtml = Convert.ToString(GRID_TITLE.SupportingText1);
        }

        var ADDUPDATE_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ADDUPDATEMSG")).FirstOrDefault();
        if (ADDUPDATE_MSG != null)
        {
            ADDEDMSG = Convert.ToString(ADDUPDATE_MSG.DisplayText);
            UPDATEMSG = Convert.ToString(ADDUPDATE_MSG.SupportingText1);
        }

        var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
        if (NoofRecordFound != null)
        {
            cntlist = NoofRecordFound.DisplayText;
            cntFound = NoofRecordFound.SupportingText1;
        }

        var TXTALERT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("TXTALERTMSG")).FirstOrDefault();
        if (TXTALERT != null)
        {
            NameAlert = Convert.ToString(TXTALERT.DisplayText);
            CommentsAlert = Convert.ToString(TXTALERT.SupportingText1);
        }

        var TXTACTALERT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("TXTACTALERTMSG")).FirstOrDefault();
        if (TXTACTALERT != null)
        {
            ActivityTypeAlert = Convert.ToString(TXTACTALERT.DisplayText);
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
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    private void InitializeView()
    {
        try
        {
            // TODO: Don't use.
            //UserAuthentication authentication = new UserAuthentication();
            //mAppManager = authentication.AppManager;

            this.mAppManager = this.Master.AppManager;
//            this.hideSpan.InnerHtml = "Enter the criteria and click on Search button to view data.";
            // Disable and clear the status.
            this.HideStatusMessage(string.Empty, false);

            this.DisplaySuccess(string.Empty, false);

            this.hdrGridHeader.Visible = false;

            if (!Page.IsPostBack)
            {
                this.PanelEditEntityEnable(false);
                //this.SetFocus(this.txtName);

                this.DisableErrorValidation(false);
            }

        }
        catch { throw; }
    }
    private List<WorkType> SearchWorkType(string name, string status)
    {
        IWorkTypeService service = null;
        try
        {
            // Create service.
            service = AppService.Create<IWorkTypeService>();
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
    private void DisplaySearchResult(IList result)
    {
        try
        {
            // Set grid title.
            if (result == null)
                this.hdrGridHeader.InnerText = string.Format(mSelectedGridTitle +  cntlist + " ({0} "+  cntFound +")", 0);
            else
            {
                // set visibility
                this.hdrGridHeader.Visible = true;
                this.hdrGridHeader.InnerText = string.Format(mSelectedGridTitle +  cntlist + " ({0} " + cntFound + ")", result.Count);

               
            }

            // Bind with grid.
            this.gvwEntityList.DataSource = result;
            this.gvwEntityList.DataBind();

            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "WORKTYPE");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, gvwEntityList);
            dvinitalvalue.Visible = false;
        }
        catch { throw; }
    }

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

            this.mSearchResult = this.SearchWorkType(name, status);

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


    #region Common Internal Members

    // validate controls for update
    private Boolean ValidateControls()
    {
        try
        {
            ValidationException exception = new ValidationException(string.Empty);

            if (String.IsNullOrEmpty(txtEntityName.Value.Trim().ToString()))
                exception.Data.Add("Name", NameAlert);
            if (drpActivity.SelectedIndex.ToString() == "0")
                exception.Data.Add("Activity", ActivityTypeAlert);

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
                //exception.Data.Add("Search", "No Data found");
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
        this.drpActivity.SelectedIndex = 0;
        chkEntityActive.Checked = false;
        chkEntityConsiderForReport.Checked = false;
        txtEntityReason.Value = "";
        //this.ddlStatus.SelectedIndex = 1;
    }

    // display success msg
    private void DisplaySuccess()
    {
        divSuccess.Style.Add("display", "block");
        divSuccess.Visible = true;
        divSuccess.InnerHtml = "Data Updated Sucessfully.";
    }
    // display success msg
    private void DisplaySuccess(string message, bool enable)
    {
        if (enable == true)
            divSuccess.Style.Add("display", "block");
        else
            divSuccess.Style.Add("display", "none");

        divSuccess.InnerHtml = message;
        divSuccess.Visible = enable;

    }

    // hide status panel
    private void HideStatusMessage(string message, Boolean enable)
    {
        if (enable == true)
            divStatusInfo.Style.Add("display", "block");
        else
            divStatusInfo.Style.Add("display", "none");

        divStatusInfo.InnerHtml = message;
        divStatusInfo.Visible = enable;

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
                string.Format("showEditPanelDialog({{'width': '400px', 'title': '{0}'}});", mEntityEditPanelHeader),
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

    // Enable (Or) Disable the Referesh control.
    private void DisableRefershControl()
    {
        try
        {
            if (gvwEntityList.Rows.Count > 0)
            {
                this.lbtRefresh.Enabled = true;

            }
            else
                this.lbtRefresh.Enabled = false;

        }
        catch { throw; }
    }

    private void DisableErrorValidation(Boolean enable)
    {
        divSuccessMessage.Visible = enable;
    }


    #endregion



    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            this.SearchEntity(this.txtName.Value, ddlStatus.Value);


            if (gvwEntityList.Rows.Count == 0)
            {
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = "";
                this.InitalspnMessage.InnerHtml = "";
                this.InitalspnMessage.Visible=true;

            }
            else
            {
                this.InitalspnMessage.InnerHtml = "";
                this.hideSpan.InnerHtml = "";
                //this.dvinitalvalue.Visible = false;
                this.hideSpan.Visible = false;
                this.InitalspnMessage.Visible = false;
            }
        }
        catch { throw; }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            // clear
            this.ClearControls();
            // set 0 for adding new data
            hiddenEntityId.Value = "0";

            // hide controls for add mode
            this.PanelEditEntityEnable(false);
            //Clear The No Data Found
            this.InitalspnMessage.InnerHtml = "";

            gvwEntityList.DataSource = null;
            gvwEntityList.DataBind();

            this.dvinitalvalue.Visible = true;

            txtName.Value = string.Empty;
            this.ddlStatus.SelectedIndex = 1;

            this.divSuccessMessage.InnerText = string.Empty;
            this.DisableErrorValidation(false);
        }
        catch { throw; }
    }
    protected void lbtAddNew_Click(object sender, EventArgs e)
    {
        try
        {

            
           
            txtEntityName.Focus();
            this.AddEntity();

            this.DisplayEntityEditPanel();

            this.DisableErrorValidation(false);
            //Button Text Changed
            this.btnUpdate.Text = lbtAddNew.Text;

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

            this.mEntityEditPanelHeader = lbtAddNew.Text + ' ' + lblWorkType.Text;
            //this.DisplayEntityEditPanel();
            chkEntityConsiderForReport.Checked = true;
        }
        catch { throw; }
    }
    protected void lbtRefresh_Click(object sender, EventArgs e)
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
//                this.InitalspnMessage.InnerHtml = "No Data Found";
            }
            else
            {
                this.dvinitalvalue.Visible = false;
                this.hdrGridHeader.Visible = true;
            }
        }
        catch { throw; }
    }
    protected void gvwEntityList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //Button Text Changed
            this.btnUpdate.Text = lblupdate.Text;

            this.hdrGridHeader.Visible = true;
            // selected id

            // Display the record for edit.
            if (e.CommandName.Equals("WorkTypeEdit", StringComparison.InvariantCultureIgnoreCase))
            {

                this.DisplayWorkTypeInfo(Int32.Parse(gvwEntityList.DataKeys[Int32.Parse(e.CommandArgument.ToString())].Value.ToString()));
                // edit panel
                this.PanelEditEntityEnable(true);
                // Display edit panel as dialog to modify.
                this.mEntityEditPanelHeader = lblupdate.Text + ' ' + lblWorkType.Text; ;
                this.DisplayEntityEditPanel();
                //clear the message.
                this.divSuccessMessage.InnerText = string.Empty;
           }

        }
        catch { throw; }
    }

    private void DisplayWorkTypeInfo(int id)
    {
        IWorkTypeService WorkTypeService = null;
        WorkType mWorkType = null;
        try
        {
            // Create service.
            WorkTypeService = AppService.Create<IWorkTypeService>();
            WorkTypeService.AppManager = this.mAppManager;
            // retrieve data
            mWorkType = new WorkType(id);
            mWorkType = WorkTypeService.Retrieve(id);
            //this.spnMessage.InnerHtml = "";
            hiddenEntityId.Value = "0";
            if (mWorkType != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityName.Value = mWorkType.Name;
                txtEntityDescription.Value = mWorkType.Description;
                //drpActivity.SelectedIndex = mWorkType.ActivityTypeId;
                drpActivity.SelectedValue = mWorkType.ActivityTypeId.ToString();
                //drpActivity.Items.FindByValue(mWorkType.ActivityTypeId.ToString()).Selected = true;for Html Drop Control
                chkEntityConsiderForReport.Checked = mWorkType.ConsiderForReport;
                chkEntityActive.Checked = mWorkType.IsActive;
                txtEntityReason.Value = string.Empty;
                if (drpActivity.SelectedValue.ToString() == "1")
                {
                    chkEntityConsiderForReport.Disabled = true;
                }
                else
                {

                    chkEntityConsiderForReport.Disabled = false;
                    //chkEntityConsiderForReport.Checked = mWorkType.ConsiderForReport;
                    //chkEntityActive.Checked = mWorkType.IsActive;
                    //txtEntityReason.Value = string.Empty;
                }
            }
            // Show/Hide the controls.
            //this.ShowHideEditControls(true);
        }
        catch { throw; }
        finally
        {
            if (WorkTypeService != null) WorkTypeService.Dispose();
            WorkTypeService = null;
        }
    }

    private Boolean UpdateWorkTypeInfo(int id)
    {
        IWorkTypeService WorkTypeService = null;
        WorkType mWorkType = null;
        bool validatelocation = true;

        this.HideStatusMessage(string.Empty, false);
        try
        {

            // Validate the entity values.
            if (this.ValidateControls() == false)
                return false;
            else
            {
                // create instance
                mWorkType = new WorkType(id);
                mWorkType.Name = txtEntityName.Value.ToString().Trim();
                mWorkType.Description = txtEntityDescription.Value.ToString().Trim();
                if (id != 0)
                {
                    mWorkType.IsActive = chkEntityActive.Checked;
                    //mWorkType.ConsiderForReport = chkEntityConsiderForReport.Checked;
                }

                else
                {
                    mWorkType.IsActive = true;
                    //mWorkType.ConsiderForReport = true;
                }

                mWorkType.ConsiderForReport = chkEntityConsiderForReport.Checked;
                mWorkType.Reason = txtEntityReason.Value.ToString().Trim();
                mWorkType.ActivityTypeId = Convert.ToInt32(drpActivity.Items[drpActivity.SelectedIndex].Value);
                mWorkType.LastUpdateUserId = 1;
                mWorkType.LastUpdateDate = DateTime.Now;

                // Create service and call method.
                WorkTypeService = AppService.Create<IWorkTypeService>();
                WorkTypeService.AppManager = mAppManager;

                // data to update
                WorkTypeService.Update(mWorkType);

                // clear
                this.ClearControls();
                this.CloseDialogControl();

            }

        }
        catch (ValidationException ve)
        {
            // Display validation erros.

            this.DisplayValidationError(this.ErrorMessage(ve));
            validatelocation = false;
        }



        finally
        {
            if (WorkTypeService != null) WorkTypeService.Dispose();
        }
        return validatelocation;
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
    protected void gvwEntityList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.WorkType Entity = (Tks.Entities.WorkType)e.Row.DataItem;
                HtmlGenericControl active = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                HtmlGenericControl considerforreport = (HtmlGenericControl)e.Row.FindControl("spnConsiderForReport");

                //Bind the Location is Active or not
                active.InnerText = Entity.IsActive == true ? "Active" : "InActive";
                considerforreport.InnerText = Entity.ConsiderForReport == true ? "Yes" : "No";

            }
        }
        catch
        { throw; }

    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int WorkTypeId;
            // update info
            if (hiddenEntityId.Value != "")
                WorkTypeId = Int32.Parse(hiddenEntityId.Value.ToString());

            else
                WorkTypeId = 0;

            if (this.UpdateWorkTypeInfo(WorkTypeId) == true)


                if (WorkTypeId != 0)
                {
                    if ((divStatusInfo.InnerHtml == ""))
                        DisplaySuccess(ADDEDMSG, true);
                }
                else
                {

                    DisplaySuccess(UPDATEMSG, true);
                }


            if (gvwEntityList.Rows.Count > 0)
                this.hdrGridHeader.Visible = true;

        }
        catch { throw; }
    }
    protected void drpActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpActivity.SelectedValue.ToString() != "1")
        {
            chkEntityConsiderForReport.Disabled = false;
        }
        else
        {
            chkEntityConsiderForReport.Disabled = true;
        }

    }
}
