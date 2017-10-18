using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

using Tks.Entities;
using Tks.Model;
using Tks.Services;



public partial class Masters_ClientListView : System.Web.UI.Page
{
    #region Class Variables

    IAppManager mAppManager = null;
    List<Tks.Entities.Client> mclientlist = null;
    //Tks.Entities.Client client = null;
    string mEntityEditPanelHeader = "Add Client";
    const string CLIENT_SEARCH_RESULT_LIST = "Client_Search_Result_List";

    string cntlist;
    string cntFound;
    string ADDEDMSG;
    string UPDATEMSG;

    string ADDModeDisplay;
    string ADDeditModeDisplay;
    string BTNMODIFY;
    string BTNADD;

    #endregion

    StringBuilder mUserSearchViewDialogScript;
    private void InitializeUserSearchViewDialogScript()
    {
        try
        {
            mUserSearchViewDialogScript = new StringBuilder();
            mUserSearchViewDialogScript.Append("refreshUserSearchView();");
        }
        catch { throw; }
    }

    #region Class Functions
    private void SearchClient()
    {
        IClientService service = null;
        try
        {
            // Get the values.
            string name = txtSearchName.Value;
            string status = ddlStatus.Items[ddlStatus.SelectedIndex].Value;

            // Validate.
            this.ValidateSearchEntity();


            // Create the service.
            service = AppService.Create<IClientService>();
            service.AppManager = this.mAppManager;

            // Build search criteria.
            ClientSearchCriteria criteria = new ClientSearchCriteria();
            criteria.Name = name;
            criteria.Status = status;
            
            // Invoke service method.
            mclientlist = service.Search(criteria,0);

            // Display the list.
            this.DisplayList(mclientlist);
            

            if (gvwEntityList.Rows.Count == 0)
            {

                dvinitalvalue.Visible = true;
                divEmptyRow.Visible = true;
                this.ShowHideValidationMessage(true);
                this.HideIntialView(true);
                //this.divEmptyRow.InnerHtml = "<b>No Record Found </b>";
                this.hdrGridHeader.InnerText = string.Empty;
                dvinitalvalue.Visible = true;
              
               
            }
            else
            {
                dvinitalvalue.Visible = false;
                //gvwEntityList.Columns[0].wid = 200;
//                System.Windows.Forms.DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();

             //   gvwEntityList.Columns[1].Width = 108;
                divEmptyRow.Visible = false;
                this.ShowHideValidationMessage(false);
                //this.HideIntialView(false);
                // this.hdrGridHeader.InnerText = "Clients List: (" + mclientlist.Count + " found)";
                this.hdrGridHeader.InnerText = cntlist +" (" + mclientlist.Count + " " + cntFound + ")";

                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "CLIENT");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwEntityList);
                dvinitalvalue.Visible = false;
              
            }


            this.divSuccessMessage.InnerHtml = string.Empty;
            this.DisableRefershControl();

          
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
            this.divSuccessMessage.InnerHtml = ErrorMessage.ToString();
            this.HideIntialView(true);
            //Hide List Found.
            this.hdrGridHeader.InnerText = string.Empty;
            //Disable the Refersh control
            this.lnkrefresh.Enabled = false;


            
        }

        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }
    private void HideIntialView(bool visible)
    {
        this.dvinitalvalue.Visible = visible;
        this.divEmptyRow.Visible = visible;
    }

    private void DisplayList(List<Tks.Entities.Client> list)
    {
        try
        {
            // Bind with grid.
            this.gvwEntityList.DataSource = list;
            this.gvwEntityList.DataBind();

           

            if (gvwEntityList.HeaderRow != null)
            {

               
                
            }
        }
        catch { throw; }
    }


    public bool UpdateEntity()
    {
        IClientService Service = null;
        bool validateupdate = false;
        try
        {
            // Validate the entity values.
            if (hdnClientId.Value != "0")
            {
                this.ValidateEntity("Update");
                //mEntityEditPanelHeader = ADDModeDisplay;
            }
            else
            {
                this.ValidateEntity(string.Empty);
                btnUpdate.Text = ADDeditModeDisplay;
                mEntityEditPanelHeader = ADDeditModeDisplay;
            }

            // Build entity.

            Tks.Entities.Client client = new Tks.Entities.Client();
            client.Id = Int32.Parse(this.hdnClientId.Value);
            client.Name = this.txtName.Value;
            client.Description = this.txtDescription.Value;
            client.ResponsibleUserId = Int32.Parse(this.hdnResponsibleUserId.Value);
            if (hdnClientId.Value != "0")
                client.IsActive = this.chkIsActive.Checked;
            else
                client.IsActive = true;
            client.Reason = this.txtReason.Value;
            client.LastUpdateUserId = 1;
            client.LastUpdateDate = DateTime.Now;

            // Create service and call method.
            Service = AppService.Create<IClientService>();
            Service.AppManager = mAppManager;
            Service.Update(client);


            // Display succeed message.


        }
        catch (ValidationException ve)
        {
            // Display validation erros.
            this.DisplayValidationMessage(ve);
          
            validateupdate = true;
        }
       
        catch
        {
            throw;
        }

        finally
        {
            if (Service != null) Service.Dispose();

        }
        return validateupdate;
    }

    private void ValidateEntity(string Operation)
    {
        try
        {
            // Create exception instance.
            //ValidationException exception = new ValidationException("Validation error(s) occurred.");
            ValidationException exception = new ValidationException("");

            if (string.IsNullOrEmpty(txtName.Value) || txtName.Value.Trim() == "")
                exception.Data.Add("CLIENTNAME", "ClientName should not be empty.");

            if (string.IsNullOrEmpty(txtResponsibleUser.Value) || txtResponsibleUser.Value.Trim() == "")
                exception.Data.Add("ManagerName", "Manager Should not be empty");

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
            ValidationException exception = new ValidationException("");
            if (string.IsNullOrEmpty(txtSearchName.Value)&&string.IsNullOrEmpty(ddlStatus.Value))
                exception.Data.Add("Search", "Enter The Search Criteria");
            if (string.IsNullOrEmpty(ddlStatus.Value))
                exception.Data.Add("Search", "Select The Status Criteria");

            //Throw the Exception if any
            if (exception.Data.Count > 0)
            {
                Session.Remove(CLIENT_SEARCH_RESULT_LIST);
                this.mclientlist = (List<Tks.Entities.Client>)Session[CLIENT_SEARCH_RESULT_LIST];
                this.DisplayList(mclientlist);


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
            this.divMessage.Style.Add("Display", "block");
            this.divMessage.Controls.Add(control);
            this.divMessage.Controls.Add(error);
            this.UpdatePanel3.Update();

            System.Threading.Thread.Sleep(5000);


            // Show message control.

        }
        catch { throw; }
    }

    private void ClearErrorMessage(HtmlContainerControl HtmlContainer)
    {
        try
        {
            HtmlContainer.InnerText = string.Empty;
        }
        catch { throw; }
    }

    private void ShowHideValidationMessage(bool visible)
    {
        try
        {
            //divMessage.InnerText = "";
            divMessage.Visible = visible;


        }
        catch { throw; }
    }

    private void RetrieveValuesFromSession()
    {
        try
        {
            // Fetch values.
            if (Session[CLIENT_SEARCH_RESULT_LIST] != null)
                mclientlist = (List<Tks.Entities.Client>)Session[CLIENT_SEARCH_RESULT_LIST];

        }
        catch { throw; }
    }

    private void DisplayEntityForEdit(int id)
    {
        IClientService Service = null;
        try
        {
            // Retrieve entity based on index.
            Tks.Entities.Client entity = mclientlist.Where(x=>x.Id==id).FirstOrDefault();

            // Assign to controls.
            this.hdnClientId.Value = entity.Id.ToString();
            this.hdnResponsibleUserId.Value = entity.ResponsibleUserId.ToString();
            this.txtName.Value = entity.Name;
            this.txtDescription.Value = entity.Description;
            this.txtResponsibleUser.Value = entity.CustomData["ResponsibleUserName"].ToString();
            //this.txtReason.Value = entity.Reason;
            this.chkIsActive.Checked = entity.IsActive;

            // Show/Hide the controls.
            this.ShowHideEditControls(true);
        }
        catch { throw; }
        finally
        {
            if (Service != null) Service.Dispose();
        }
    }

    private void CloseDialogControl()
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), System.Guid.NewGuid().ToString(), "closeEditPanelDialog()", true);
        }
        catch { throw; }
    }

    private void ShowHideEditControls(bool visible)
    {
        try
        {
            //hide(Or)show the edit Control
            this.divEditControl.Visible = visible;
        }
        catch { throw; }
    }

    // Enable (Or) Disable the Referesh control.
    private void DisableRefershControl()
    {
        try
        {
            if (gvwEntityList.Rows.Count >= 0)

                this.DisableControl(lnkrefresh, true);
            // this.SearchClient();


        }
        catch { throw; }
    }

    private void DisableControl(WebControl webctrl, bool enable)
    {
        try
        {
            //Disable or Enable a Control
            webctrl.Enabled = enable;
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
                string.Format("showEditPanelDialog({{'width': '400px', 'title': '{0}'}});", mEntityEditPanelHeader + " " + lblClient.Text),
                true);
        }
        catch { throw; }
    }
    private void ShowOrHideSuccessmessage(bool visible)
    {
        try
        {
            this.divSuccessMessage.Visible = visible;
        }
        catch { throw; }
    }
    private void ClearControl()
    {
        // clear the Control 
        // txtSearchName.Value = "";
        txtName.Value = "";
        txtDescription.Value = "";
        //txtResponsibleUser.Value = "Press F2 to search user";
        txtReason.Value = "";
        this.ddlStatus.SelectedIndex = 1;
        this.ShowHideEditControls(false);

    }
    #endregion

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "CLIENT");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            this.divEmptyRow.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

        var PRESSF2MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("PRESSF2MSG")).FirstOrDefault();
        if (PRESSF2MSG != null)
        {
            if (hdnClientId.Value == "0")
            {
                txtResponsibleUser.Value = Convert.ToString(PRESSF2MSG.DisplayText);
            }
        }
        var NoRecordFound = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("lblNoRecordFound")).FirstOrDefault();
        if (NoRecordFound != null)
        {
            this.divEmptyRow.InnerHtml = Convert.ToString(NoRecordFound.DisplayText);
        }

        var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
        if (NoofRecordFound != null)
        {
            cntlist = NoofRecordFound.DisplayText;
            cntFound = NoofRecordFound.SupportingText1;
        }
        
        var ADDEDITBTN = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ADDEDITBTN")).FirstOrDefault();
        if (ADDEDITBTN != null)
        {
            if (hdnClientId.Value != "0")
            {
                ADDeditModeDisplay = Convert.ToString(ADDEDITBTN.SupportingText1);
                //mEntityEditPanelHeader = Convert.ToString(ADDEDITBTN.SupportingText1);
                //btnUpdate.Text = Convert.ToString(ADDEDITBTN.SupportingText1);
            }
            else
            {
                ADDModeDisplay = Convert.ToString(ADDEDITBTN.DisplayText);
                //mEntityEditPanelHeader = Convert.ToString(ADDEDITBTN.DisplayText);
                //btnUpdate.Text = Convert.ToString(ADDEDITBTN.DisplayText);
            }
        }


        var lblUserSearchValue = lblLanguagelst.Where(c => c.LabelId.Equals("lblUserSearchview")).FirstOrDefault();
        if (lblUserSearchValue != null)
        {
            lblUserSearchview.Text = lblUserSearchValue.DisplayText;
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
    } 

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //divMessage.Visible = false;

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
            
            this.InitializeUserSearchViewDialogScript();

            // TODO: Need to remove.
            UserAuthentication authentication = new UserAuthentication();
            this.mAppManager = authentication.AppManager;

            // Retrieve values from session.
            this.RetrieveValuesFromSession();
            
            if (!Page.IsPostBack)
            {
                ShowHideEditControls(false);
                txtSearchName.Focus();
                this.lnkrefresh.Enabled = false;
            }
            

            this.ShowOrHideSuccessmessage(false);

            LoadLabels();

            if (hdnClientId.Value != "0")
            {
                btnUpdate.Text = BTNMODIFY;
            }
            else
            {
                btnUpdate.Text = BTNADD;
            }

        }
        catch { throw; }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add values to session.
            Session.Add(CLIENT_SEARCH_RESULT_LIST, mclientlist);

             // Register scripts.
            if (this.mUserSearchViewDialogScript != null)
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    System.Guid.NewGuid().ToString(),
                    this.mUserSearchViewDialogScript.ToString(),
                    true);
            // Show Validation Errors.
              this.ShowHideValidationMessage(true);
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //Search Client By Name.
             this.SearchClient();

        }
        catch { throw; }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //this.InitializeUserSearchViewDialogScript();
        try
        {
            // Update the entity.
            if (!this.UpdateEntity())
            {

                if (hdnClientId.Value != "0")
                {
                    //mEntityEditPanelHeader = "Edit Client";

                    ShowHideEditControls(false);
                    this.SearchClient();
                    divSuccessMessage.Visible = true;
                    divSuccessMessage.InnerText = UPDATEMSG;
                 
                }
                else
                {
                    //add  a new record
                    divSuccessMessage.Visible = true;
                    divSuccessMessage.InnerText = ADDEDMSG;
                    //this.SearchClient();
                  
                }
                this.ClearControl();
                this.CloseDialogControl();
            }
            //this.divMessage.Style.Add("display", "none");
            this.ShowHideValidationMessage(true);
        }
        catch { throw; }
    }


    protected void gvwEntityList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            // Display the record for edit.
            if (e.CommandName.Equals("ClientEdit", StringComparison.InvariantCultureIgnoreCase))
            {
                
                //this.DisplayEntityForEdit(Int32.Parse(e.CommandArgument.ToString()));
                this.DisplayEntityForEdit(Int32.Parse(gvwEntityList.DataKeys[Int32.Parse(e.CommandArgument.ToString())].Value.ToString()));
                LoadLabels();
                btnUpdate.Text = ADDeditModeDisplay;
                mEntityEditPanelHeader = ADDeditModeDisplay;
                //clear the success message.
                this.divSuccessMessage.InnerText = string.Empty;
               // LoadLabels();
               // mEntityEditPanelHeader = "Edit Client";
                //Button changed text
              //  this.btnUpdate.Text = "Update";
                //this.btnUpdate.Text = btnUpdate.Text;
                this.ClearErrorMessage(divMessage);
                this.UpdatePanel3.Update();
                //Display the Dialog.
                this.DisplayEntityEditPanel();

            }

        }
        catch { throw; }
    }

    protected void gvwEntityList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {            

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tks.Entities.Client Entity = (Tks.Entities.Client)e.Row.DataItem;
                HtmlGenericControl active = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                //HtmlGenericControl LastUpdateuser = (HtmlGenericControl)e.Row.FindControl("spnLastUpdateUser");
                HtmlGenericControl ResponsibleUserName = (HtmlGenericControl)e.Row.FindControl("spnResponsibleUserName");
                //Bind the TimeZone is Active or not
                active.InnerText = Entity.IsActive == true ? "Active" : "InActive";


                //Bind  the Last modified Username 

                //LastUpdateuser.InnerText = Entity.CustomData["LastUpdateUserName"].ToString();
                ResponsibleUserName.InnerText = Entity.CustomData["ResponsibleUserName"].ToString();

            }
        }
        catch
        { throw; }
    }
    protected void LnkAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //Button changed text
          
            this.btnUpdate.Text = ADDModeDisplay;
            txtName.Focus();
            
            //clear the Control 
            this.ClearControl();
            // Hide The Error.
            this.ShowHideValidationMessage(true);
           // this.ShowOrHideIntialMessage(true);
            hdnClientId.Value = "0";
           // mEntityEditPanelHeader = "Add Client";
            mEntityEditPanelHeader = ADDModeDisplay;
            this.divSuccessMessage.InnerText = string.Empty;
            this.divMessage.Style.Add("display", "none");
            this.UpdatePanel3.Update();
            //this.divMessage.Visible = false;
            this.DisplayEntityEditPanel();

        }
           
        catch { throw; }
    }
    protected void LnkRefersh_Click(object sender, EventArgs e)
    {
        try
        {
            // clear the controls
            this.ClearControl();
            this.SearchClient();
            this.divMessage.InnerText = "";


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
            }
        catch { throw; }
    }
  
    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.ClearControl();
        this.ClearErrorMessage(divMessage);
        this.ShowHideValidationMessage(false);
        this.DisplayList(new List<Tks.Entities.Client>());
        txtSearchName.Value = "";
        this.divSuccessMessage.InnerText = string.Empty;
        this.hdrGridHeader.InnerText = string.Empty;
        //this.divEmptyRow.InnerHtml = "Enter the criteria and click on Search button to view data.";
        //this.dvinitalvalue.InnerText = string.Empty;
        this.HideIntialView(true);
        // Disable The Refresh Control.
        this.lnkrefresh.Enabled = false;


    }
    protected void ibtSearchUsers_Click(object sender, ImageClickEventArgs e)
    {
        this.UserSearchView1.Display();
        //this.InitializeUserSearchViewDialogScript();
        this.mUserSearchViewDialogScript.Append("showUserSearchView()");
        
       //this.divMessage.Style.Add("display", "none");
        
      
    }
    protected void gvwEntityList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvwEntityList.PageIndex = e.NewPageIndex;
        this.SearchClient();
    }

}

