using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Masters_LocationListView : System.Web.UI.Page
{
    //Common  variable used through out the page 
    #region Class Variable

    const string LOCATION_SEARCH_RESULT_LIST = "Location_Search_Result_List";

    IAppManager mAppManager = null;
    List<Tks.Entities.Location> mLocationList = null;
    IList mSearchResult;
    private string mSelectedGridTitle = null;
    string mEntityEditPanelHeader = "Add Location";
    string NORECFOUND;

    string ADDEDMSG;
    string UPDATEMSG;

    string cntlist;
    string cntFound;
    string AddNewLocatoin;
    string EditLocatoin;

    string ALERTCITY;
    string ALERTSTATE;
    
    string ALERTCOUNTRY;
    string ALERTTIMEZONE;

    string BTNMODIFY;
    string BTNADD;

    string CommentsAlert;

    #endregion

    protected void gvwLocationList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      
        try
        {
            //Button Text Changed
            this.btnUpdate.Text = BTNMODIFY;

            this.hdrGridHeader.Visible = true;
            // selected id

            // Display the record for edit.
            if (e.CommandName.Equals("TimeZoneEdit", StringComparison.InvariantCultureIgnoreCase))
            {

                this.DisplaylocationInfo(Int32.Parse(gvwLocationList.DataKeys[Int32.Parse(e.CommandArgument.ToString())].Value.ToString()));
                // edit panel
                this.PanelEditEntityEnable(true);
                // Display edit panel as dialog to modify.
                this.mEntityEditPanelHeader = EditLocatoin;
                this.DisplayEntityEditPanel();
                //clear the message.
                this.divSuccessMessage.InnerText = string.Empty;
          
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "LOCATION");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            this.hideSpan.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
            NORECFOUND = Convert.ToString(GRID_TITLE.SupportingText1);

        }

        var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
        if (NoofRecordFound != null)
        {
            cntlist = NoofRecordFound.DisplayText;
            cntFound = NoofRecordFound.SupportingText1;
        }

        var AddNewLocatoin1 = lblLanguagelst.Where(c => c.LabelId.Equals("lblAddNewLocatoin")).FirstOrDefault();
        if (AddNewLocatoin1 != null)
        {
            AddNewLocatoin = AddNewLocatoin1.DisplayText;
            EditLocatoin = AddNewLocatoin1.SupportingText1;
        }

        var ALERTCITYSTATE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLALERTCITYSTATE")).FirstOrDefault();
        if (ALERTCITYSTATE != null)
        {
            ALERTCITY = ALERTCITYSTATE.DisplayText;
            ALERTSTATE = ALERTCITYSTATE.SupportingText1;
        }

        var ALERTCOUNTRYTZ = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLALERTCOUNTRYTZ")).FirstOrDefault();
        if (ALERTCOUNTRYTZ != null)
        {
            ALERTCOUNTRY = ALERTCOUNTRYTZ.DisplayText;
            ALERTTIMEZONE = ALERTCOUNTRYTZ.SupportingText1;
        }

        var ADDUPDATE_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ADDUPDATEMSG")).FirstOrDefault();
        if (ADDUPDATE_MSG != null)
        {
            ADDEDMSG = Convert.ToString(ADDUPDATE_MSG.DisplayText);
            UPDATEMSG = Convert.ToString(ADDUPDATE_MSG.SupportingText1);
        }

        var btnADDMOD = lblLanguagelst.Where(c => c.LabelId.Equals("btnUpdate")).FirstOrDefault();
        if (btnADDMOD != null)
        {
            BTNMODIFY = Convert.ToString(btnADDMOD.SupportingText1);
            BTNADD = Convert.ToString(btnADDMOD.DisplayText);

        }

        var TXTALERT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("TXTALERTMSG")).FirstOrDefault();
        if (TXTALERT != null)
        {
           // NameAlert = Convert.ToString(TXTALERT.DisplayText);
            CommentsAlert = Convert.ToString(TXTALERT.SupportingText1);
        }
    } 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            LoadLabels();

            

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
            this.InitializeView();

            
           
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

    private List<Location> SearchLocation(string city, string country, string state, string status)
    {
        ILocationService service = null;
        try
        {

            // Create service.
            service = AppService.Create<ILocationService>();
            service.AppManager = this.mAppManager;

            // Call service method.
            return service.Search(
              new LocationSearchCriteria()
              {
                  City = city,
                  Country = country,
                  State = state,
                  Status = status
              }, this.mAppManager.LoginUser.Id);
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }
    // Display Location info
    private void DisplaylocationInfo(int id)
    {
        ILocationService locationService = null;
        Location mLocation = null;
        try
        {
            // Create service.
            locationService = AppService.Create<ILocationService>();
            locationService.AppManager = this.mAppManager;
            // retrieve data
            mLocation = new Location(id);
            mLocation = locationService.Retrieve(id);           
           //this.spnMessage.InnerHtml = "";
            hiddenEntityId.Value = "0";
            if (mLocation != null)
            {
                hiddenEntityId.Value = id.ToString();
                txtEntityCity.Value = mLocation.City;
                txtEntityState.Value = mLocation.State;
                txtEntityCountry.Value = mLocation.Country;
                txtTimeZone.Value = mLocation.CustomData["TimeZoneName"].ToString();
                hdnTimeZoneId.Value = mLocation.TimeZoneId.ToString();
                chkEntityActive.Checked =mLocation.IsActive;               
                txtEntityReason.Value =string.Empty;              
            }
            // Show/Hide the controls.
            //this.ShowHideEditControls(true);
        }
        catch { throw; }
        finally
        {
            if (locationService != null) locationService.Dispose();
            locationService = null;
        }
    }
    private Boolean UpdateLocationInfo(int id)
    {
        ILocationService locationService = null;
        Location mlocation = null;
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
                mlocation = new Location(id);
                mlocation.City = txtEntityCity.Value.ToString().Trim();
                mlocation.Country = txtEntityCountry.Value.ToString().Trim();
                mlocation.State = txtEntityState.Value.ToString().Trim();
                mlocation.TimeZoneId = Convert.ToInt32(hdnTimeZoneId.Value.Trim());
                if (id != 0)
                    mlocation.IsActive = chkEntityActive.Checked;

                else
                    mlocation.IsActive = true;
                mlocation.Reason = txtEntityReason.Value.ToString().Trim();
                mlocation.LastUpdateUserId = 1;
                mlocation.LastUpdateDate = DateTime.Now;

                // Create service and call method.
                locationService = AppService.Create<ILocationService>();
                locationService.AppManager = mAppManager;

                // data to update
                locationService.Update(mlocation);

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
            if (locationService != null) locationService.Dispose();
        }
        return validatelocation;
    }
    private void RetrieveValuesFromSession()
    {
        try
        {
            // Fetch values.
            if (Session[LOCATION_SEARCH_RESULT_LIST] != null)
                mLocationList = (List<Tks.Entities.Location>)Session[LOCATION_SEARCH_RESULT_LIST];

        }
        catch { throw; }
    }

    private void SearchEntity(string city, string country, string state, string status)
    {
        try
        {
            // Validate search criteria.

            if (!this.ValidateSearchCriteria())
            {
                hideSpan.Visible = false;
                return;
            }

            this.mSearchResult = this.SearchLocation(city, country, state, status);

            // Display search result.
            this.DisplaySearchResult(this.mSearchResult);

            // edit panel
            this.PanelEditEntityEnable(false);

            this.divSuccessMessage.InnerHtml = string.Empty;

            if (gvwLocationList.Rows.Count > 0)
            {
                this.hdrGridHeader.Visible = true;

                List<LblLanguage> lblLanguagelst = null;
                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "LOCATION");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwLocationList);
                dvinitalvalue.Visible = false;

            }
            else
            {
                //                this.hdrGridHeader.InnerHtml = NORECFOUND;
                this.hdrGridHeader.Visible = true;
            }
        }
        catch { throw; }
    }
    private void DisplaySearchResult(IList result)
    {
        try
        {
            // Set grid title.
            if (result == null)
                this.hdrGridHeader.InnerText = string.Format(mSelectedGridTitle + " " + cntlist + " ({0} "+ cntFound + ")", 0);
            else
            {
                // set visibility
                this.hdrGridHeader.Visible = true;
                this.hdrGridHeader.InnerText = string.Format(mSelectedGridTitle + " " + cntlist + " ({0} " + cntFound + ")", result.Count);
            }

            // Bind with grid.
            this.gvwLocationList.DataSource = result;
            this.gvwLocationList.DataBind();
        }
        catch { throw; }
    }

    private void InitializeView()
    {
        try
        {
            // TODO: Don't use.
            //UserAuthentication authentication = new UserAuthentication();
            //mAppManager = authentication.AppManager;
            this.mAppManager = this.Master.AppManager;
            //this.hideSpan.InnerHtml = "Enter the criteria and click on Search button to view data.";
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
    
    #region Common Internal Members

    // validate controls for update
    private Boolean ValidateControls()
    {
        try
        {
            ValidationException exception = new ValidationException(string.Empty);

            if (String.IsNullOrEmpty(txtEntityCity.Value.Trim().ToString()))
                exception.Data.Add("City", ALERTCITY);
            if (String.IsNullOrEmpty(txtEntityState.Value.Trim().ToString()))
                exception.Data.Add("State", ALERTSTATE);

            if (String.IsNullOrEmpty(txtEntityCountry.Value.Trim().ToString()))
                exception.Data.Add("Country", ALERTCOUNTRY);
            
            if (String.IsNullOrEmpty(txtTimeZone.Value.Trim().ToString()))
                exception.Data.Add("TimeZone", ALERTTIMEZONE);

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
            if (string.IsNullOrEmpty(txtCity.Value) && string.IsNullOrEmpty(txtCountry.Value)
                && string.IsNullOrEmpty(txtState.Value) && string.IsNullOrEmpty(ddlStatus.Value))
            {
                exception.Data.Add("Search", NORECFOUND);
                this.DisableErrorValidation(true);
                this.divSuccessMessage.InnerHtml = "";
            }
            else
            {
                this.divSuccessMessage.InnerHtml = "";
                this.DisableErrorValidation(false);
            }

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

        this.HideStatusMessage(message, true);
        // Show panel.
        //this.ShowHideValidationPanel(true);
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
        txtEntityCity.Value = "";
        txtEntityCountry.Value = "";
        txtEntityState.Value = "";
        txtTimeZone.Value = "";
        //this.ddlStatus.SelectedIndex = 1;
        chkEntityActive.Checked = false;
        txtEntityReason.Value = "";
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
            if (gvwLocationList.Rows.Count > 0)
            {
                this.lbtRefreshLocationList.Enabled = true;

            }
            else
                this.lbtRefreshLocationList.Enabled = false;

        }
        catch { throw; }
    }

    private void DisableErrorValidation(Boolean enable)
    {
        divSuccessMessage.Visible = enable;
    }


    #endregion
    protected void lbtRefreshLocationList_Click(object sender, EventArgs e)
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
            this.SearchEntity(txtCity.Value, txtCountry.Value, txtState.Value, ddlStatus.Value);
            if (!string.IsNullOrEmpty(txtCity.Value.Trim()) && string.IsNullOrEmpty(txtCountry.Value.Trim())
                 && string.IsNullOrEmpty(txtState.Value.Trim()))
                 this.SearchEntity(txtCity.Value, txtCountry.Value, txtState.Value, ddlStatus.Value);

            if (gvwLocationList.Rows.Count == 0)
            {
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = "";
                this.InitalspnMessage.InnerHtml = NORECFOUND;
            }
            else
            {
                //this.dvinitalvalue.Visible = false;
                this.hideSpan.InnerHtml = "";
                hideSpan.Visible = false;
                this.hdrGridHeader.Visible = true;
            }
         }
        catch { throw; }
    }
    protected void lbtAddNewLocation_Click(object sender, EventArgs e)
    {
        try
        {
            txtEntityCity.Focus();
            this.AddEntity();

            this.DisplayEntityEditPanel();

            this.DisableErrorValidation(false);
            //Button Text Changed
            this.btnUpdate.Text = BTNADD;

        }
        catch { throw; }
    }
    private void AddEntity()
    {
        try
        {
            if (gvwLocationList.Rows.Count > 0)
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

            this.mEntityEditPanelHeader = AddNewLocatoin;
            //this.DisplayEntityEditPanel();
        }
        catch { throw; }
    }
    protected void gvwLocationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvwLocationList.PageIndex = e.NewPageIndex;
            this.SearchEntity(txtCity.Value, txtCountry.Value, txtState.Value, ddlStatus.Value);
        }
        catch { throw; }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            this.SearchEntity(this.txtCity.Value, txtCountry.Value, txtState.Value, ddlStatus.Value);


            if (gvwLocationList.Rows.Count == 0)
            {
                //this.divEmptyRow.Visible=false;
                //this.divEmptyRow.InnerHtml = "";
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = "";
                this.InitalspnMessage.InnerHtml = NORECFOUND;
            }
            else
            {
                hideSpan.Visible = false;
                //this.hideSpan.InnerHtml = "";
                //this.dvinitalvalue.Visible = false;
                this.InitalspnMessage.Visible = false;
            }
        }
        catch { throw; }

    }
    protected void lbtClear_Click(object sender, EventArgs e)
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

            gvwLocationList.DataSource = null;
            gvwLocationList.DataBind();

            this.dvinitalvalue.Visible = true;

            txtCity.Value = string.Empty;
            txtCountry.Value = string.Empty;
            txtState.Value = string.Empty;
            this.ddlStatus.SelectedIndex = 1;
            //ddlStatus.Value =string.Empty;

            this.divSuccessMessage.InnerText = string.Empty;
            this.DisableErrorValidation(false);
        }
        catch { throw; }
    }
    protected void gvwLocationList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.Location Entity = (Tks.Entities.Location)e.Row.DataItem;
                HtmlGenericControl active = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                //HtmlGenericControl LastUpdateuser = (HtmlGenericControl)e.Row.FindControl("spnLastUpdateUser");
                HtmlGenericControl TimeZoneName = (HtmlGenericControl)e.Row.FindControl("spntimeZonename");
                HtmlGenericControl TimeZoneId = (HtmlGenericControl)e.Row.FindControl("spnTimeZoneId");
                //Bind the Location is Active or not
                active.InnerText = Entity.IsActive == true ? "Active" : "InActive";
                //Bind  the Last modified Username 
                //LastUpdateuser.InnerText = Entity.CustomData["LastUpdateUserName"].ToString();
                TimeZoneName.InnerText = Entity.CustomData["TimeZoneName"].ToString();
                hdnTimeZoneId.Value = Entity.CustomData["TimeZoneId"].ToString();
            }
        }
        catch
        { throw; }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
   try
        {
            int locationId;
            // update info
            if (hiddenEntityId.Value != "")
                locationId = Int32.Parse(hiddenEntityId.Value.ToString());

            else
                locationId = 0;

            if (this.UpdateLocationInfo(locationId) == true)


                if  (locationId != 0)
                {
                    if ((divStatusInfo.InnerHtml == ""))
                        DisplaySuccess(ADDEDMSG, true); 
                }
                else
                {
                   
                    DisplaySuccess(UPDATEMSG, true);
                }
      

            if (gvwLocationList.Rows.Count > 0)
                this.hdrGridHeader.Visible = true;

        }
        catch { throw; }
    }
}


