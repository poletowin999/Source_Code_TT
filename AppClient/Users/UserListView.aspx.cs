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

public partial class Users_UserListView : System.Web.UI.Page
{

    #region Class Variables
    List<User> mUserList = null;
    IAppManager mappmanager = null;
    string AlertNorecord;

    string cntlist;
    string cntFound;

    #endregion

    

    #region Internal Members
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
            string Email = txtEmail.Value.Trim();
            string Location = ddlLocation.SelectedIndex != 0 ? ddlLocation.SelectedItem.Text : string.Empty;
            string Status = drpstatus.Items[Convert.ToInt32(drpstatus.SelectedIndex.ToString())].Text.ToString();
            
            //create a service
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;

            //Invoke the service method
            this.mUserList = service.Search(new UserSearchCriteria { Name = Name, RoleName = RoleName, EMailId = Email, Status = Status, CityName = Location });

            
            if (mUserList != null)
            {
                //Display the List of Users Found
                LstofRecord.InnerText = cntlist + mUserList.Count + " " + cntFound;
              
                

            }
//            this.InitalBind.Visible = false;
            DisableRefershControl();

        
        }

        catch (ValidationException ve)
        {
            
            //display the validation error Message.
            this.DisplayValidationError(ve);

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
        return mUserList;


    }

    
    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        this.mappmanager = Master.AppManager;
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = mappmanager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(Master.AppManager.LoginUser.Id, "USERMODULE");

        if (lblLanguagelst != null)
        {
            Utility _objUtil = new Utility();

            _objUtil.LoadLabels(lblLanguagelst);

            var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
            if (GRID_TITLE != null)
            {

                this.spnMessage.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
                AlertNorecord = Convert.ToString(GRID_TITLE.SupportingText1);
            }

            var NoofRecordFound = lblLanguagelst.Where(c => c.LabelId.Equals("lblNoofRecordFound")).FirstOrDefault();
            if (NoofRecordFound != null)
            {
                cntlist = NoofRecordFound.DisplayText;
                cntFound = NoofRecordFound.SupportingText1;
            }

        }
    }  


    private void DisplayValidationError(Exception ex)
    {
        try
        {

            //create string builder instance.
            StringBuilder message = new StringBuilder(ex.Message);
            foreach (string datas in ex.Data.Values)
            {
                //Format the mesage.
                message.Append("<ul><li>" + datas.ToString() + "</li><ul>");

            }
            
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
            if(Message!="")
                divMessage.Style.Add("display", "block");
            else
                divMessage.Style.Add("display", "none");
            divMessage.InnerHtml = Message.ToString();
        }
        catch { throw; }
        
    }
    private void DisplayList(IList<User> userLst)
    {
        try
        {
            //Bind the Grid
           
            GvwUserList.DataSource = userLst;
            GvwUserList.DataBind();
            if (GvwUserList.Rows.Count > 0)
            {
                this.DisplayMessage(string.Empty);
                spnMessage.InnerHtml = "";

                foreach (DataControlField column in GvwUserList.Columns)
                {
                    column.ItemStyle.Width = Unit.Pixel(100);
                }

                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERMODULE");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, GvwUserList);
                InitalBind.Visible = false;
            }
            else
            {
                //Show Message when data is Empty.
                InitalBind.Visible = true;
                spnMessage.InnerHtml = AlertNorecord;
            }
            
           
        }
        catch { throw; }
    }

    //clear the input controls
    private void ClearInputControls(HtmlInputControl Inputctrl)
    {
        Inputctrl.Value = string.Empty;
    }
    private void ValidateSearchEntity()
    {
        try
        {
            //create a Exception instance.
            ValidationException exception = new ValidationException("Search Validation");
            if (txtName.Value.Trim() == "" && txtRoleName.Value.Trim() == "" && txtEmail.Value.Trim() == "" && drpstatus.SelectedIndex.ToString() == "0")
                exception.Data.Add("Search", "For Search any one Field is Mandatory");


            if (exception.Data.Count > 0)
            {
                //Throw the exception if any.
                throw exception;

            }
        }
        catch
        {
            throw;
        }
       
    }
    private void RetriveData(int id)
    {
        IUserService service = null;

        try
        {
            //create a service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;

            mUserList = new List<User>();

            //Invoke a  method
            mUserList.Add(service.Retrieve(id));
            this.InitalBind.Visible = false;

            //Display the data in grid
            this.DisplayList(mUserList);

        }
        catch { throw; }
        finally
        {
            //Dispose the service.
            if (service != null) { service.Dispose(); }

        }
    }
    private void DisableRefershControl()
    {
        try
        {
                LnkRefersh.Enabled = true;
            
        }
        catch
        {
            throw;
        }
    }
    //Intial view 
    private void InitalView()
    {
        try
        {
            //Initial view.
            this.InitalBind.Visible = true;
            this.spnMessage.InnerHtml = "<b>Enter the criteria and click on Search button to view data.</b>";
            LnkRefersh.Enabled = false;
        }
        catch { throw; }

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

            ddlLocation.Items.Insert(0, new ListItem("--Select--","0"));
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
    #endregion


    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            this.mappmanager = Master.AppManager;

            

            string aa;
            aa = this.mappmanager.LoginUser.HasAdminRights.ToString();

            if (aa == "False")
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
                this.DisplayMessage("YOU DO NOT HAVE ACCESS TO THIS PAGE");
                Response.Redirect("~/Homepage.aspx");

            }

                if (Session["Userid"] != null)
                {
                    this.BindLocation();
                    if (Session["Userid"].ToString() != "0")
                    {
                       
                        this.DisplayList(this.SearchUsers());
                        this.DisplayMessage("Record Inserted Sucessfully");


                    }
                    else
                    {
                     
                        //Bind the grid.
                 
                        this.DisplayList(this.SearchUsers());
                        this.DisplayMessage("Record Updated successfully");
                        this.InitalBind.Visible = false;
                    }
                    //clear the session.
                    Session["Userid"] = null;
                    
                }
               
                else
                {
                    if(!Page.IsPostBack)
                       this.BindLocation();
                    //Display the Inital view.
                    this.InitalView();
                    this.DisplayMessage(string.Empty);
            
                    
                }


                txtName.Focus();

                LoadLabels();
          
        }
        catch { throw; }
      
    }
    



    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;
        try
        {
            //create a exception.
            Exception exception = HttpContext.Current.Error;

            //insert into Error log.
            provider = new ErrorLogProvider();
            provider.AppManager = this.mappmanager;
            provider.Insert(exception);
        }
        catch
        {
            throw;
        }
        finally
        {
            if (provider != null) { provider.Dispose(); }
        }
    }


   protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            //Disable the Control
            //this.DisableRefershControl();
        }
        catch { throw; }
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            // System.Threading.Thread.Sleep(3000);

           //Search the users and display the data
            this.DisplayList(this.SearchUsers());

        }
        catch { throw; }


    }
    protected void btnclear_Click(object sender, EventArgs e)
    {

        try
        {
            //clear the controls
            this.ClearInputControls(txtEmail);
            this.ClearInputControls(txtName);
            this.ClearInputControls(txtRoleName);
          
            this.drpstatus.SelectedIndex = 1;
            this.ddlLocation.SelectedIndex = 0;

            //clear the Error Message
            this.DisplayMessage(string.Empty);

            //bind the Empty Data 
            mUserList = null;
            this.DisplayList(mUserList);
            this.InitalView();
            LstofRecord.InnerText = string.Empty;
            txtName.Focus();

            
            
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
                HtmlGenericControl desgination = (HtmlGenericControl)e.Row.FindControl("spndesignation");
                //HtmlGenericControl LastupdateDate = (HtmlGenericControl)e.Row.FindControl("spnLastUpdateDate");
                HtmlGenericControl Isactive = (HtmlGenericControl)e.Row.FindControl("spnIsActive");
                HtmlGenericControl location = (HtmlGenericControl)e.Row.FindControl("spnLocation");

                //Covert string to Datetime
                //DateTime Modifieddate = new DateTime();
                //Modifieddate = DateTime.Parse(user.CustomData["LastUpdateDate"].ToString());
               
                //Assign to the control.
             //   UpdatedUser.InnerText = user.CustomData["LastUpdateUser"].ToString();
               // LastupdateDate.InnerText = Modifieddate.ToString("MM/dd/yyyy hh:mm tt");
                Isactive.InnerText = user.IsActive == true ? "Active" : "Inactive";
                desgination.InnerText = user.CustomData["RoleName"].ToString();

                //check the key.
                if (user.CustomData.ContainsKey("Location"))
                    location.InnerHtml = user.CustomData["Location"].ToString();
                else
                    location.InnerText = string.Empty;


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
            this.DisplayList(this.SearchUsers());
        }
        catch { throw; }
    }
    #endregion

}