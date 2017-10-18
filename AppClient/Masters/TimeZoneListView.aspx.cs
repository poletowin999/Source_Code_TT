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

public partial class Masters_TimeZoneListView : System.Web.UI.Page
{

    #region Class Variable

    const string TIMEZONE_SEARCH_RESULT_LIST = "TimeZone_Search_Result_List";

    IAppManager mAppManager = null;
    List<Tks.Entities.TimeZone> mTimeZoneList = null;
    string mEntityEditPanelHeader = "Add Location";
    #endregion


    #region Internal Members

    #region Hide/show the Initial view control.
    private void HideIntialView (bool visible)
    {
        this.divGridHeader.Visible = visible;
        this.divEmptyRow.Visible = visible;
    }
    #endregion

    #region SearchTimeZone based on Name and ShortName
    private void SearchTimeZones()
    {
        ITimeZoneService service = null;
        try
        {
            // Get the values.
            string name = txtSearchName.Value;
            string shortName = txtSearchShortName.Value;

            // Validate.
            this.ValidateSearchEntity();

            // Create the service.
            service = AppService.Create<ITimeZoneService>();
            service.AppManager = this.mAppManager;

            // Build search criteria.
            TimeZoneSearchCriteria criteria = new TimeZoneSearchCriteria();
            criteria.Name = name;
            criteria.ShortName = shortName;

            // Invoke service method.
            mTimeZoneList = service.Search(criteria);

            if (mTimeZoneList.Count == 0)
            {
                this.ShowHideValidationMessage(true);
                this.HideIntialView(true);
                
                //Display a message when Data is Empty.
                this.divEmptyRow.InnerText = "No Data Found";
            }
            else
            {
                this.ShowHideValidationMessage(false);
                this.HideIntialView(false);
                this.hdrGridHeader.InnerText = "List of TimeZones: (" + mTimeZoneList.Count + " found)";
                
            }
            // Display the list.
            this.divSuccessMessage.InnerText = string.Empty;
            this.DisplayList(mTimeZoneList);
            this.DisableRefershControl();

        }
        catch (ValidationException ex)
        {
            
            //Display the validation  errors. 
            StringBuilder ErrorMessage = new StringBuilder();
            ErrorMessage.Append(string.Format("<table><tr><td>{0}</td></tr>", ex.Message));
            foreach(string s in ex.Data.Values)
            {
                ErrorMessage.Append(string.Format("<tr><td>{0}</td></tr></table>", s.ToString()));
            }



            //Hide the Error message
            this.divSuccessMessage.Style.Add("display", "block");
            this.divSuccessMessage.InnerHtml = ErrorMessage.ToString();

            this.HideIntialView(true);

            //Hide List Found.
            this.hdrGridHeader.InnerText = string.Empty;

            //Disable the Refersh control
            this.LnkRefersh.Enabled= false;

            
        }
        catch { throw; }
        finally
        {
            //Dispose the instance.
            if (service != null) service.Dispose();
        }

    }
    #endregion  

    #region Display EditDialogPanel Control
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
    #endregion

    #region Close DialogEditPanel Control
    private void CloseDialogControl()
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), System.Guid.NewGuid().ToString(), "closeEditPanelDialog()", true);
        }
        catch { throw; }
    }
    #endregion

    #region Disable Refersh Control 
    private void DisableRefershControl()
    {
        try
        {
            //Enable/Disable the Referesh control 
            if (mTimeZoneList.Count>=0)
            {
                this.DisableControl(LnkRefersh, true);
                
               
            }
            


        }
        catch { throw; }
    }
    #endregion


    private void DisableControl(WebControl webctrl, bool enable)
    {
        try
        {
            //Disable or Enable a Control
            webctrl.Enabled = enable;
        }
        catch { throw; }
    }
    private void DisplayList(List<Tks.Entities.TimeZone> list)
    {
        try
        {
            // Bind with grid.
            this.gvwTimeZoneList.DataSource = list;
            this.gvwTimeZoneList.DataBind();
           
            
        }
        catch { throw; }
    }
    


    public bool UpdateEntity()
    {
        ITimeZoneService service = null;
        bool validateupdate = false;
        try
        {

            // Validate the entity values.
            if (hdnTimeZoneId.Value != "0")
                this.ValidateEntity("Update");
            else
                this.ValidateEntity(string.Empty);

            // Build entity.
            Tks.Entities.TimeZone entity = new Tks.Entities.TimeZone();
            entity.Id = Int32.Parse(this.hdnTimeZoneId.Value);
            entity.Name = this.txtName.Value;
            entity.ShortName = this.txtShortName.Value;
            entity.Description = this.txtDescription.InnerText.ToString().Replace("<","");
            if (hdnTimeZoneId.Value != "0")
                entity.IsActive = this.chkIsActive.Checked;
            else
                entity.IsActive = true;
            entity.Reason = this.txtReason.Value;
            entity.LastUpdateUserId = 1;
            entity.LastUpdateDate = DateTime.Now;
            // Create service and call method.
            service = AppService.Create<ITimeZoneService>();
            service.AppManager = mAppManager;
            service.Update(entity);

            // Display succeed message.





        }

        catch (ValidationException ve)
        {
            // Display validation erros.
            this.DisplayValidationMessage(ve);
            validateupdate = true;

        }
        catch (Exception ex)
        {
           
            validateupdate = true;
            throw ex;
        }
        finally
        {
            if (service != null) service.Dispose();
        }
        return validateupdate;
    }

    private void ValidateEntity(string Operation)
    {
        try
        {
            // Create exception instance.
            ValidationException exception = new ValidationException(string.Empty);

            if (string.IsNullOrEmpty(txtName.Value) || txtName.Value.Trim() == "")
                exception.Data.Add("NAME", "Name should not be empty.");

            if (string.IsNullOrEmpty(txtShortName.Value) || txtShortName.Value.Trim() == "")
                exception.Data.Add("SHORT_NAME", "Short name should not be empty.");
            if (Operation == "Update")
            {
                if (string.IsNullOrEmpty(txtReason.Value) || txtReason.Value.Trim() == "")
                    exception.Data.Add("REASON", "Reason should not be empty.");
            }
            // Throw the exception, if any.
            if (exception.Data.Count > 0)
                throw exception;

        }
        catch { throw; }
    }
    private void ValidateSearchEntity()
    {
        try
        {
            //create exception instance.
            ValidationException exception = new ValidationException(string.Empty);
            if (string.IsNullOrEmpty(txtSearchName.Value) && string.IsNullOrEmpty(txtSearchShortName.Value))
                exception.Data.Add("search", "Name and shortname any one is Mandatory for search");


            //Throw the Exception if any
            if (exception.Data.Count > 0)
            {
                Session.Remove(TIMEZONE_SEARCH_RESULT_LIST);
                this.mTimeZoneList = (List<Tks.Entities.TimeZone>)Session[TIMEZONE_SEARCH_RESULT_LIST];
                this.DisplayList(mTimeZoneList);

                throw exception;
            }

        }
        catch { throw; }
    }

    private void DisplayValidationMessage(Exception exception)
    {
        try
        {
            //claer the error message
            this.ClearErrorMessage(divMessage);
            // Create bullet.

            BulletedList error = new BulletedList();
            error.DataTextField = "value";
            error.DataSource = exception.Data;
            error.DataBind();

            // Display message.
            HtmlGenericControl control = new HtmlGenericControl("span");
            control.InnerText = exception.Message;

            this.divMessage.Controls.Add(control);
            this.divMessage.Controls.Add(error);

            // Show message control.

        }
        catch { throw; }
    }

    //show/hide the error Message 
    private void ShowHideValidationMessage(bool visible)
    {
        try
        {
            // divMessage.InnerText = "";
            divMessage.Visible = visible;
        }
        catch { throw; }
    }
    private void ClearErrorMessage(HtmlContainerControl HtmlContainer)
    {
        try
        {
            //clear the Error message.
            HtmlContainer.InnerText = string.Empty;
        }
        catch { throw; }
    }



    private void RetrieveValuesFromSession()
    {
        try
        {
            // Fetch values.
            if (Session[TIMEZONE_SEARCH_RESULT_LIST] != null)
                mTimeZoneList = (List<Tks.Entities.TimeZone>)Session[TIMEZONE_SEARCH_RESULT_LIST];

        }
        catch { throw; }
    }

    private void DisplayEntityForEdit(int index)
    {
        ITimeZoneService service = null;
        try
        {
            //Create a Instance
           
            Tks.Entities.TimeZone entity = mTimeZoneList[index];
            // Assign to controls.
            this.hdnTimeZoneId.Value = entity.Id.ToString();
            this.txtName.Value = entity.Name;
            this.txtShortName.Value = entity.ShortName;
            this.txtDescription.InnerText = entity.Description;
            this.chkIsActive.Checked = entity.IsActive;

            // Show/Hide the controls.
            this.ShowHideEditControls(true);
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void ShowHideEditControls(bool visible)
    {
        try
        {
            //hide/show the edit Control
            this.divEditControl.Visible = visible;
        }
        catch { throw; }
    }
    private void ClearControl()
    {
        // clear the Control 
        try
        {
            txtName.Value = "";
            txtShortName.Value = "";
            txtDescription.InnerText = "";
            txtReason.InnerText = "";
            this.ShowHideEditControls(false);
        }
        catch { throw; }


    }
    //private void 

    #endregion


    #region Page  Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            this.mAppManager = Master.AppManager;

            

            // Retrieve values from session.
            this.RetrieveValuesFromSession();




            if (!Page.IsPostBack)
            {
                ShowHideEditControls(false);
                txtSearchName.Focus();
                this.LnkRefersh.Enabled = false;
            }
               



        }
        catch { throw; }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add values to session.
            Session.Add(TIMEZONE_SEARCH_RESULT_LIST, mTimeZoneList);
           
        }
        catch { throw; }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //search the Timezone by name 
            this.SearchTimeZones();
            
            
            
            
            

        }
        catch { throw; }
    }



    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Update the entity.

            if (!this.UpdateEntity())
            {
                if (hdnTimeZoneId.Value != "0")
                {
                     mEntityEditPanelHeader = "Edit Location";
                     ShowHideEditControls(false);
                     this.SearchTimeZones();
                     divSuccessMessage.InnerText = "Record Updated Sucessfully";
                    


                }
                else
                {
                    //add  a new record
                    divSuccessMessage.InnerText = "Record Inserted Sucessfully";
                  


                }
                
                this.ClearControl();
                this.CloseDialogControl();
             

            }



            this.ShowHideValidationMessage(true);



        }
        catch { throw; }
    }


    protected void LnkAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //Hide the Edit Controls
            ShowHideEditControls(false);
            //clear the Control 
            this.ClearControl();
            //Hide the Error 
            this.ShowHideValidationMessage(false);
            hdnTimeZoneId.Value = "0";
            mEntityEditPanelHeader = "Add TimeZone";
            this.divSuccessMessage.InnerText = string.Empty;
            
            //this.HideIntialView(true);
            //this.DisplayList(new List<Tks.Entities.TimeZone>());

            this.DisplayEntityEditPanel();
            txtName.Focus();
            



        }
        catch { throw; }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {

            this.CloseDialogControl();
        }
        catch { throw; }
    }

    protected void LnkRefersh_Click(object sender, EventArgs e)
    {
        try
        {
            //refresh paage 
            this.ClearControl();
            this.SearchTimeZones();
            this.divMessage.InnerText = "";
           


        }
        catch { throw; }
    }
    protected void gvwTimeZoneList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.TimeZone Entity = (Tks.Entities.TimeZone)e.Row.DataItem;
                HtmlGenericControl active = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                HtmlGenericControl LastUpdateuser = (HtmlGenericControl)e.Row.FindControl("spnLastUpdateUser");
                //Bind the TimeZone is Active or not
                active.InnerText = Entity.IsActive == true ? "Yes" : "No";


                //Bind  the Last modified Username 

                LastUpdateuser.InnerText = Entity.CustomData["LastUpdateUserName"].ToString();

            }
           
        }
        catch
        { throw; }

    }
    protected void gvwTimeZoneList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            // Display the record for edit.
            if (e.CommandName.Equals("TimeZoneEdit", StringComparison.InvariantCultureIgnoreCase))
            {
                this.DisplayEntityForEdit(Int32.Parse(e.CommandArgument.ToString()));
               
                //clear the success message.
                this.divSuccessMessage.InnerText = string.Empty;
               
                mEntityEditPanelHeader = "Edit TimeZone.";
                

                //Display the Dialog.
                this.DisplayEntityEditPanel();
                txtName.Focus();
            }
            

        }
        catch { throw; }
    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        // clear the controls
        this.ClearControl();
        this.ClearErrorMessage(divMessage);
        //clear the search controls
        txtSearchName.Value = string.Empty;
        txtSearchShortName.Value= string.Empty;
        
        this.hdrGridHeader.InnerText = string.Empty;
        //Bind the Empty Data
        this.DisplayList(null);
        this.LnkRefersh.Enabled = false;
        this.divEmptyRow.InnerText = "Enter the criteria and click on Search button to view data.";
        this.HideIntialView(true);
    }
    protected void gvwTimeZoneList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvwTimeZoneList.PageIndex = e.NewPageIndex;
            this.SearchTimeZones();
        }
        catch { throw; }

    }
    #endregion
}