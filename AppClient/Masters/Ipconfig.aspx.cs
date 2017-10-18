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
using System.Data;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
public partial class Masters_Ipconfig : System.Web.UI.Page
{

    //Common  variable used through out the page 
    #region Class Variable

    const string LOCATION_SEARCH_RESULT_LIST = "Location_Search_Result_List";

    IAppManager mAppManager = null;
     private string mSelectedGridTitle = null;
    string mEntityEditPanelHeader = "Add Location";

    #endregion
    protected void gvwIPList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            //Button Text Changed
            this.btnUpdate.Text = "Update";

            this.hdrGridHeader.Visible = true;
            // selected id

            // Display the record for edit.
            if (e.CommandName.Equals("TimeZoneEdit", StringComparison.InvariantCultureIgnoreCase))
            {

                this.DisplayIPInfo((gvwIPList.DataKeys[Int32.Parse(e.CommandArgument.ToString())].Value.ToString()));
                

                // edit panel
                this.PanelEditEntityEnable(true);
                // Display edit panel as dialog to modify.
                this.mEntityEditPanelHeader = "Edit IPConfig";
                this.DisplayEntityEditPanel();
                //clear the message.
                this.divSuccessMessage.InnerText = string.Empty;

            }
           

        }
        catch { throw; }
    }

 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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

    private DataTable SearchIP(string IPAddress, string Location, string Linktype, string status)
    {
        IIPservice service = null;
        try
        {

            // Create service.
            service = AppService.Create<IIPservice>();
            service.AppManager = this.Master.AppManager;

            // Call service method.
            return service.Search(IPAddress,Location,Linktype,status);
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }
    // Display Location info
    private void DisplayIPInfo(string IPAddress)
    {
        IIPservice service = null;
        try
        {
            // Create service.
            service = AppService.Create<IIPservice>();
            service.AppManager = this.Master.AppManager;

            // Call service method.
            DataTable dt =  service.Search(IPAddress, "", "", "ALL");
           
            if (dt.Rows.Count>0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                hiddenEntityId.Value = dr["IpId"].ToString();
                ddlEntitylink.Value =dr["Linktype"].ToString();
                txtEntityIPAddress.Value = dr["IPAddress"].ToString();
                txtEntityLocation.Value = dr["Location"].ToString();
                hdnEntityLocationId.Value=dr["LocationId"].ToString();
                txtEntityReason.Value = "";
                chkEntityActive.Checked = Convert.ToBoolean(dr["Status"]);
                }
            }
             //Show/Hide the controls.
            //this.ShowHideEditControls(true);
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
            service = null;
        }
    }
    private Boolean UpdateIPconfigInfo(int id)
    {
        IIPservice service = null;

        // Create service.
        service = AppService.Create<IIPservice>();
        service.AppManager = this.Master.AppManager;
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

                if (id != 0)
                    service.Update(txtEntityIPAddress.Value, hdnEntityLocationId.Value.ToString(), ddlEntitylink.Value, chkEntityActive.Checked.ToString(), txtEntityReason.Value, Convert.ToInt16(hiddenEntityId.Value.ToString()));

                else
                    service.Update(txtEntityIPAddress.Value, hdnEntityLocationId.Value.ToString(), ddlEntitylink.Value, chkEntityActive.Checked.ToString(), txtEntityReason.Value, 0);

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
            if (service != null) service.Dispose();
        }
        return validatelocation;
    }
   

   
    private void DisplaySearchResult(IList result)
    {
        try
        {
            // Set grid title.
            if (result == null)
                this.hdrGridHeader.InnerText = string.Format(mSelectedGridTitle + " " + "Locations List: ({0} found)", 0);
            else
            {
                // set visibility
                this.hdrGridHeader.Visible = true;
                this.hdrGridHeader.InnerText = string.Format(mSelectedGridTitle + " " + "Locations List: ({0} found)", result.Count);
            }

            // Bind with grid.
            this.gvwIPList.DataSource = result;
            this.gvwIPList.DataBind();
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
            this.hideSpan.InnerHtml = "Enter the criteria and click on Search button to view data.";
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

            if (String.IsNullOrEmpty(txtEntityIPAddress.Value.Trim().ToString()))
                exception.Data.Add("IPAddr", "IP Address should not be empty");
            if (String.IsNullOrEmpty(txtEntityLocation.Value.Trim().ToString()))
                exception.Data.Add("Location", "Location should not be empty");

            if (hiddenEntityId.Value != "0" && txtEntityReason.Value.Trim() == "")
                exception.Data.Add("Reason", "Reason should not be empty");

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
            if (string.IsNullOrEmpty(txtIpAddress.Value) && string.IsNullOrEmpty(txtLocation.Value)
                && string.IsNullOrEmpty(ddlLink.Value) && string.IsNullOrEmpty(ddlStatus.Value))
            {
                exception.Data.Add("Search", "No Records found");
                this.DisableErrorValidation(true);
                this.divSuccessMessage.InnerHtml = "Enter the criteria and click on Search button to view data.";
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
        txtEntityIPAddress.Value = "";
        txtEntityLocation.Value = "";
        txtEntityReason.Value = "";
        this.ddlEntitylink.SelectedIndex = 0;
        chkEntityActive.Checked = true;
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
            if (gvwIPList.Rows.Count > 0)
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
            
            this.DisableErrorValidation(false);

            if (gvwIPList.Rows.Count == 0)
            {
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = "";
                this.InitalspnMessage.InnerHtml = "No Data Found";
            }
            else
            {
                this.dvinitalvalue.Visible = false;
                this.hdrGridHeader.Visible = true;
            }
        }
        catch { throw; }
    }
    protected void lbtAddNewLocation_Click(object sender, EventArgs e)
    {
        try
        {
            ddlEntitylink.Focus();
            this.AddEntity();

            this.DisplayEntityEditPanel();

            this.DisableErrorValidation(false);
            //Button Text Changed
            this.btnUpdate.Text = "Add";

        }
        catch { throw; }
    }
    private void AddEntity()
    {
        try
        {
            if (gvwIPList.Rows.Count > 0)
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

            this.mEntityEditPanelHeader = "Add New IP config";
            //this.DisplayEntityEditPanel();
        }
        catch { throw; }
    }
    protected void gvwIPList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvwIPList.PageIndex = e.NewPageIndex;
            //this.SearchEntity(txtIpAddress.Value, txtLocation.Value, ddlLink.Value, ddlStatus.Value);
        }
        catch { throw; }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();

    }

    private void LoadData()
    {
        try
        {
            IIPservice service = null;

            // Create service.
            service = AppService.Create<IIPservice>();
            service.AppManager = this.Master.AppManager;

            // Call service method.
            gvwIPList.DataSource = service.Search(txtIpAddress.Value, txtLocation.Value, ddlLink.Value, ddlStatus.Value);
            gvwIPList.DataBind();


            if (gvwIPList.Rows.Count == 0)
            {
                this.dvinitalvalue.Visible = true;
                this.hideSpan.InnerHtml = "";
                this.InitalspnMessage.InnerHtml = "No Data Found";
            }
            else
                this.dvinitalvalue.Visible = false;
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

            gvwIPList.DataSource = null;
            gvwIPList.DataBind();

            this.dvinitalvalue.Visible = true;

            txtIpAddress.Value = string.Empty;
            txtLocation.Value = string.Empty;
            this.ddlLink.SelectedIndex = 0;
            this.ddlStatus.SelectedIndex = 0;
            //ddlStatus.Value =string.Empty;

            this.divSuccessMessage.InnerText = string.Empty;
            this.DisableErrorValidation(false);
        }
        catch { throw; }
    }
  
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int IPId;
            // update info
            if (hiddenEntityId.Value != "")
                IPId = Int32.Parse(hiddenEntityId.Value.ToString());

            else
                IPId = 0;

            if (this.UpdateIPconfigInfo(IPId) == true)


                if (IPId != 0)
                {
                    if ((divStatusInfo.InnerHtml == ""))
                        DisplaySuccess("Data updated successfully.", true);
                    LoadData();
                }
                else
                {

                    DisplaySuccess("Data Added successfully.", true);
                    LoadData();
                }


            if (gvwIPList.Rows.Count > 0)
                this.hdrGridHeader.Visible = true;

        }
        catch { throw; }
    }
}


