using System;
using System.Collections.Generic;
using System.Linq;
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

public partial class Users_UserSettings : System.Web.UI.Page
{

    #region Class Variables

    const string USER_SEARCH_RESULT_LIST = "User_Search_Result_List";

    List<Tks.Entities.User> mUserList = null;
    IAppManager mappmanager = null;
    string mEntityEditPanelHeader = "Edit User";
    #endregion

    #region Internal Members
    
    #region Hide/show the Initial view control.
    private void HideIntialView(bool visible)
    {
        this.divGridHeader.Visible = visible;
        this.divEmptyRow.Visible = visible;
    }
    #endregion
    #region DialogView
    private void CloseDialogControl()
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), System.Guid.NewGuid().ToString(), "closeEditPanelDialog()", true);
        }
        catch { throw; }
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
    #endregion

    private void FillUserShift()
    {
        IUserService service = null;
        try
        {
            //create a service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;

            //Invoke the method.
            List<UserShift> LstShift = service.RetrieveUserShift();
            this.drpShift.DataTextField = "Name";
            this.drpShift.DataValueField = "Id";
            this.drpShift.DataSource = LstShift;
            this.drpShift.DataBind();

            //add default item.
            this.drpShift.Items.Insert(0, new ListItem("--Select--", "0"));

            //select the default item
            if (this.drpShift.Items.Count > 0) this.drpShift.Items[0].Selected = true;


        }
        catch
        {
            throw;
        }
        finally
        {
            if (service != null) { service.Dispose(); }
        }
    }

    private void BindLocation()
    {
        ILocationService service = null;
        try
        {
            service = AppService.Create<ILocationService>();
            service.AppManager = this.mappmanager;
            List<Location> lstLocation = service.ReatrieveAll();
            ddlLocation.DataTextField = "City";
            ddlLocation.DataValueField = "Id";
            ddlLocation.DataSource = lstLocation;
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, new ListItem("--Select--", "0"));
            if (ddlLocation.Items.Count > 0)
                ddlLocation.SelectedIndex = 0;

        }
        catch
        {
            throw;
        }
        finally
        {
            if (service != null)
            {
                service.Dispose();
            }
        }
    }

    private List<User> SearchUsers()
    {
        IUserService service = null;
        try
        {
            //validate 
            // this.ValidateSearchEntity();

            //Build the search Entity

            string Name = txtName.Value.Trim();
            string RoleName = txtRoleName.Value.Trim();
            //string Email = txtEmail.Value.Trim();
            string Location = ddlLocation.SelectedIndex != 0 ? ddlLocation.SelectedItem.Text : string.Empty;
            string Status = "Active";

            //create a service
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;

            //Invoke the service method
            this.mUserList = service.Search(new UserSearchCriteria { Name = Name, RoleName = RoleName, Status = Status, CityName = Location });

            // Display the list.
            this.DisplayList(mUserList);

            if (GvwUserList.Rows.Count == 0)
            {
                this.ShowHideValidationMessage(true);
                this.HideIntialView(true);
                
                //Display a message when Data is Empty.
                this.divEmptyRow.InnerText = "No Data Found";
                this.LstofRecord.InnerText = string.Empty;
            }
            else
            {
                this.ShowHideValidationMessage(false);
                this.HideIntialView(false);
                this.LstofRecord.InnerText = "List of Users: (" + mUserList.Count + " found)";

            }
            // Display the list.
            this.divSuccessMessage.InnerText = string.Empty;
            

            //if (mUserList != null)
            //{
            //    //Display the List of Users Found
            //    LstofRecord.InnerText = " Users List (" + mUserList.Count + " Users)";

            //}
            //this.InitalBind.Visible = false;
            /// DisableRefershControl();


        }

        catch (ValidationException ex)
        {
            //Display the validation  errors. 
            StringBuilder ErrorMessage = new StringBuilder();
            ErrorMessage.Append(string.Format("<table><tr><td>{0}</td></tr>", ex.Message));
            foreach (string s in ex.Data.Values)
            {
                ErrorMessage.Append(string.Format("<tr><td>{0}</td></tr></table>", s.ToString()));
            }

            //Hide the Error message
            this.divSuccessMessage.Style.Add("display", "block");
            this.divSuccessMessage.InnerHtml = ErrorMessage.ToString();

            this.HideIntialView(true);

            //Hide List Found.
            this.LstofRecord.InnerText = string.Empty;

            
            //display the validation error Message.
            //this.DisplayValidationError(ve);

        }
        catch { throw; }

        finally
        {
            //Dispose the instance.
            if (service != null) service.Dispose();
        }
        return mUserList;

    }
    

    private void DisplayEntityForEdit(int id)
    {
        IUserService service = null;
        User mUser = null;
        try
        {
            // Create service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;
            // retrieve data
            mUser = new User(id);
            mUser = service.RetrieveUsersettings(id); 

            // Assign to controls.
            
            if (mUser != null)
            {
                this.hdnuserid.Value = mUser.Id.ToString();
                this.txtFirstname.Value = mUser.FirstName.ToString();
                this.txtLastname .Value = mUser.LastName;
                this.hdnLocationId.Value = mUser.LocationId.ToString();
                this.txtLocation.Value = mUser.CustomData["Location"].ToString();
                this.hdnLanguageId.Value = mUser.LanguageId.ToString();
                this.txtLanguage.Value = mUser.CustomData["Language"].ToString();
                this.drpShift.SelectedValue = mUser.ShiftId.ToString();
                this.txtDateOfJoining.Text = Convert.ToString(mUser.JoinDate);
                this.txtDateOfRelieving.Text = Convert.ToString(mUser.ToDate);

                if (string.IsNullOrEmpty(mUser.FromDate.ToString()))
                {
                    txtDateOfJoining.Text = "";
                }
                else
                {
                    txtDateOfJoining.Text = Convert.ToDateTime(mUser.FromDate).ToString("MM/dd/yyyy");
                }
                if (string.IsNullOrEmpty(mUser.ToDate .ToString()))
                {
                    txtDateOfRelieving.Text = "";
                }
                else
                {
                    txtDateOfRelieving.Text = Convert.ToDateTime(mUser.ToDate).ToString("MM/dd/yyyy");
                }
            } 

            
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }
    
    
    private Boolean UpdateUserSettingsInfo()
    {
        IUserService  UserService = null;
        bool validateupdate = false;

        try
        {
            
            // Validate the entity values.
               //if(hdnuserid.Value!= "0")
               // this.ValidateUserEntity("Update");
               //else
                this.ValidateUserEntity(string.Empty);

               // Build entity.

               Tks.Entities.User entity = new Tks.Entities.User();
            
                 // create instance
                entity.Id = Int32.Parse(this.hdnuserid.Value);
                //entity.FirstName = txtFirstname.Value.ToString().Trim();
                //entity.LastName = txtLastname .Value.ToString().Trim();
                entity.LocationId = Convert.ToInt32(hdnLocationId.Value.Trim());
                entity.LanguageId = Convert.ToInt32(hdnLanguageId.Value.Trim());
                entity.ShiftId = Convert.ToInt32(drpShift.Items[drpShift.SelectedIndex].Value);
                if (txtDateOfJoining.Text != "")
                {
                    entity.FromDate = Convert.ToDateTime(txtDateOfJoining.Text);
                }
                if (txtDateOfRelieving.Text != "")
                {
                    entity.ToDate = Convert.ToDateTime(txtDateOfRelieving.Text);
                }
               
                
              // Create service and call method.
                UserService = AppService.Create<IUserService>();
                UserService.AppManager = mappmanager ;

                // data to update
                UserService.UpdateUserSettings(entity);

                // clear
                //this.ClearControls();
                this.CloseDialogControl();

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
            if (UserService != null) UserService.Dispose();
        }
        return validateupdate;
    }
    private void ValidateUserEntity(String Operation)
    {
        try
        {
            DateTime value;
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            //create a Exception Instance
            ValidationException exception = new ValidationException("validation failure");

            //validate the required elements
            if (string.IsNullOrEmpty(txtFirstname.Value) || txtFirstname.Value.Trim() == "")
                exception.Data.Add("FIRSTNAME", "FirstName is a Mandatory Field");

            if (string.IsNullOrEmpty(txtLastname.Value) || txtLastname.Value.Trim() == "")
                exception.Data.Add("LASTNAME", "LastName is a Mandatory Field");

            if (string.IsNullOrEmpty(txtLocation.Value) || txtLocation.Value.Trim() == "")
                exception.Data.Add("LOCATION", "Location is a Mandatory Field");

            if (string.IsNullOrEmpty(txtLanguage.Value) || txtLanguage.Value.Trim() == "")
                exception.Data.Add("Language", "Language  is a Mandatory Field");

            if (drpShift.SelectedIndex.ToString() == "0")
                exception.Data.Add("Shift", "Shift is a Mandatory Field");

            if (string.IsNullOrEmpty(txtDateOfJoining.Text) || txtDateOfJoining.Text.Trim() == "")
                exception.Data.Add("FromDate", "From Date is a Mandatory Field");

            if (string.IsNullOrEmpty(txtDateOfRelieving.Text) || txtDateOfRelieving.Text.Trim() == "")
                exception.Data.Add("ToDate", "JTo Date is a Mandatory Field");
            if (txtDateOfJoining.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtDateOfJoining.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtDateOfJoining.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH1", "Invalid From Date ,Date should be (MM/DD/YYYY) format");
                }
            }


            if ((txtDateOfRelieving.Text.Trim().Length == 8) && (txtDateOfRelieving.Text.IndexOf("/") == -1))
            {
                txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(2, "/");
                txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(5, "/");
            }

            if (txtDateOfRelieving.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtDateOfRelieving.Text, out value))
                {
                    dt1 = Convert.ToDateTime(txtDateOfRelieving.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH2", "Invalid To Date ,Date should be (MM/DD/YYYY) format");
                }
            }

            if (dt2 > dt1)
            {
                exception.Data.Add("MISMATCH3", "To Date should be greater than From Date");
            }
            //throw the exceptions
            if (exception.Data.Count > 0)
            {
                throw exception;
            }
           
        }
                    
        catch (Exception ex)
        {
            throw ex;
        }
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
            this.divMessage.Style.Add("Display", "block");
            this.divMessage.Controls.Add(control);
            this.divMessage.Controls.Add(error);

            // Show message control.

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
    private void ShowHideValidationMessage(bool visible)
    {
        try
        {
            //hide(Or)show the edit Control
            this.divMessage.Visible = visible;
        }
        catch { throw; }
    }

    private void RetrieveValuesFromSession()
    {
        try
        {
            // Fetch values.
            if (Session[USER_SEARCH_RESULT_LIST] != null)
                mUserList = (List<Tks.Entities.User>)Session[USER_SEARCH_RESULT_LIST];

        }
        catch { throw; }
    }

    private void ClearControl()
    {
        // clear the Control 
        try
        {
            txtName.Value = "";
            txtFirstname.Value = "";
            txtLastname.Value = "";
            txtLocation.Value = "";
            txtLanguage.Value = "";
            drpShift.SelectedIndex = 0;
            //this.ShowHideValidationMessage(false);
            
        }
        catch { throw; }


    }

    
    private void DisplayMessage(string Message)
    {
        try
        {

            if (divSuccessMessage.InnerHtml != "")
                divSuccessMessage.InnerHtml = string.Empty;
            if (Message != "")
                divSuccessMessage.Style.Add("display", "block");
            else
                divSuccessMessage.Style.Add("display", "none");
            divSuccessMessage.InnerHtml = Message.ToString();
        }
        catch { throw; }

    }
    private void DisplayList(List<Tks.Entities.User> list)
    {
        try
        {
            // Bind with grid.
            this.GvwUserList.DataSource = list;
            this.GvwUserList.DataBind();


        }
        catch { throw; }
    }
    
   
    #endregion
    
    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.mappmanager = Master.AppManager;

            // Retrieve values from session.
            this.RetrieveValuesFromSession();

            if (!Page.IsPostBack)
                this.BindLocation();
            //Display the Inital view.
            //this.InitalView();
            if (!Page.IsPostBack)
            this.FillUserShift();
            
        }
        catch { throw; }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add values to session.
            Session.Add(USER_SEARCH_RESULT_LIST, mUserList);

        }
        catch { throw; }
    }
    protected void GvwUserList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            // Display the record for edit.
            if (e.CommandName.Equals("UserEdit", StringComparison.InvariantCultureIgnoreCase))
            {
                //this.DisplayEntityForEdit(Int32.Parse(e.CommandArgument.ToString()));
                this.DisplayEntityForEdit(Int32.Parse(GvwUserList.DataKeys[Int32.Parse(e.CommandArgument.ToString())]["Id"].ToString()));

                mEntityEditPanelHeader = "Edit User";
                divSuccessMessage.InnerText = string.Empty;

                this.divSuccessMessage.Style.Add("display", "none");

                //Display the Dialog.
                this.DisplayEntityEditPanel();

            }

        }
        catch { throw; }
    }
    protected void GvwUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Get a each row.
                User user = (User)e.Row.DataItem;

                //Find the control.
                // HtmlGenericControl UpdatedUser = (HtmlGenericControl)e.Row.FindControl("spnUsername");
                HtmlGenericControl Shift = (HtmlGenericControl)e.Row.FindControl("spnShift");
                HtmlGenericControl Language = (HtmlGenericControl)e.Row.FindControl("spnLanguage");
                HtmlGenericControl RoleName = (HtmlGenericControl)e.Row.FindControl("spnRoleName");
                HtmlGenericControl location = (HtmlGenericControl)e.Row.FindControl("spnLocation");

                //Covert string to Datetime
                //DateTime Modifieddate = new DateTime();
                //Modifieddate = DateTime.Parse(user.CustomData["LastUpdateDate"].ToString());

                //Assign to the control.
                //   UpdatedUser.InnerText = user.CustomData["LastUpdateUser"].ToString();
                // LastupdateDate.InnerText = Modifieddate.ToString("MM/dd/yyyy hh:mm tt");
                //Isactive.InnerText = user.IsActive == true ? "Active" : "Inactive";
                RoleName.InnerText = user.CustomData["RoleName"].ToString();

                //check the key.
                if (user.CustomData.ContainsKey("Location"))
                    location.InnerHtml = user.CustomData["Location"].ToString();
                else
                    location.InnerText = string.Empty;

                if (user.CustomData.ContainsKey("Language"))
                    Language.InnerHtml = user.CustomData["Language"].ToString();
                else
                    Language.InnerText = string.Empty;

                if (user.CustomData.ContainsKey("Shift"))
                    Shift.InnerHtml = user.CustomData["Shift"].ToString();
                else
                    Shift.InnerText = string.Empty;
            }
        }
        catch { throw; }
    }
    protected void GvwUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Paging 
           GvwUserList.PageIndex = e.NewPageIndex;
           this.SearchUsers();
        }
        catch { throw; }
    }
   
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
           
            //Search the users and display the data
            this.SearchUsers();

        }
        catch { throw; }

    }
    protected void btnclear_Click(object sender, EventArgs e)
    {
        
        txtName.Value = "";
        txtRoleName.Value = "";
        ddlLocation.SelectedItem.Value = "";
        //Bind the Empty Data
        this.DisplayList(null);
        this.divSuccessMessage.InnerText = string.Empty;
        this.LstofRecord.InnerText = string.Empty;
        this.divMessage.Style.Add("display", "none");
        this.divSuccessMessage .Style.Add("display", "none");
        this.divEmptyRow.InnerHtml = " Enter the criteria and click on Search button to view data.";
        this.HideIntialView(true);
        this.ShowHideValidationMessage(false);
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            // Update the entity.
            if (!this.UpdateUserSettingsInfo())
            {

                mEntityEditPanelHeader = "Edit Client";

                //divSuccessMessage.Visible = true;
                //divSuccessMessage.InnerText = "Record Updated Sucessfully";

                divSuccessMessage.Style.Add("display", "block");
                DisplayMessage("Record Updated Sucessfully");

            }
            //this.ClearControl();
            //this.CloseDialogControl();

            //this.divMessage.Style.Add("display", "none");
            ShowHideValidationMessage(true);
        }
        catch { throw; }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            // Close The Dialog.

            this.CloseDialogControl();
            this.divMessage.Style.Add("display", "none");
            //this.divSuccessMessage .Style.Add("display", "none");
        }
        catch { throw; }

    }
    #endregion
}