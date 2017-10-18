using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Users_UnAttachedUserView : System.Web.UI.UserControl
{

    #region Class Variables
    IUserService mUserService = null;
    IAppManager mAppManager = null;
    UserAuthentication muserauthentication = null;
    List<Tks.Entities.User> LstUser = null;
    public string mAtttachUserId = "0";
    public string mAtttachUserName = "";

    string SEARCHCRITERIA;
   
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        this.alertError.Style["display"] = "none";
        this.alertError.InnerHtml = "";
        LoadLabels();
        if (!IsPostBack)
        {
            txtDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
        }
    }
       
    public void ShowAttachUser(string Username,string AttachUserId)
    {
        lblSelectedUsername.Text = Username;
        HidAttachUser.Value = AttachUserId.ToString();
        gvwunattachedUser.DataSource = null;
        gvwunattachedUser.DataBind();
        this.divGridHeader.Visible = true;
        this.spnMessage.InnerHtml = SEARCHCRITERIA;
        divGridview.Visible = false;
        divmatchedusers.InnerText = "";
        txtRole.Text = "";
        txtUserName.Text = "";
    }

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIEPAGE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var SEARCHCRITERIATXT = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLSEARCHCRITERIA")).FirstOrDefault();
        if (SEARCHCRITERIATXT != null)
        {

            SEARCHCRITERIA = Convert.ToString(SEARCHCRITERIATXT.DisplayText);
        }

    }

    public void  RetrieveUnAttachedUsers()
     {
       
         try{
            
               this.alertError.Style["display"] = "none";
               this.alertError.InnerHtml = "";
                muserauthentication = new UserAuthentication();
                this.mAppManager = muserauthentication.AppManager;
                mUserService = AppService.Create<IUserService>();
                mUserService.AppManager = mAppManager;
              
                LstUser=mUserService.RetrieveUnAttachedUsers(new UserSearchCriteria { Name = txtUserName.Text.Trim(), RoleName = txtRole.Text.Trim() });
                //LstUser = this.RetrieveUnAttachedUsers(txtUserName.Text, txtRole.Text);
                this.mUserService.Dispose();
                if (LstUser != null)
                {
                    gvwunattachedUser.DataSource = LstUser;
                    gvwunattachedUser.DataBind();
                    divmatchedusers.InnerText =  "Matched user " + gvwunattachedUser.Rows.Count.ToString() + " Found";

                    List<LblLanguage> lblLanguagelst = null;
                    ILblLanguage mLanguageService = null;
                    lblLanguagelst = new List<LblLanguage>();
                    mLanguageService = AppService.Create<ILblLanguage>();
                    mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                    // retrieve
                    lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIEPAGE");

                    Utility _objUtil = new Utility();
                    _objUtil.LoadGridLabels(lblLanguagelst, gvwunattachedUser);
                    divGridHeader.Visible = false;

                    //Intialize the View
                    this.divGridHeader.Visible = false;
                    this.spnMessage.InnerHtml = SEARCHCRITERIA;
                    divGridview.Visible = true;
                    divmatchedusers.Visible = true;
                }
                else
                {
                    gvwunattachedUser.DataSource = null;
                    gvwunattachedUser.DataBind();
                    this.divGridHeader.Visible = true;
                    this.spnMessage.InnerHtml = SEARCHCRITERIA;
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = "User not found";
                    divGridview.Visible = false;
                    divmatchedusers.InnerText = "";
                }
         }
         catch { throw; }
         finally
         {

         }

    }
    public void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUserName.Text.Trim() == "" && txtRole.Text.Trim() == "")
            {
                this.alertError.Style["display"] = "Block";
                this.alertError.InnerHtml = SEARCHCRITERIA;
                ClearControls();
            }
            else
            {

                this.alertError.Style["display"] = "none";
                this.alertError.InnerHtml = "";
                RetrieveUnAttachedUsers();
            }
        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }

    }
    protected void Clear_Click(object sender, EventArgs e)
    {
         try
        {
            ClearControls();
        }
         catch { throw; }
         finally
         {
             if (mUserService != null) mUserService.Dispose();
             mUserService = null;
         }
    }
    protected void bntAttach_Click(object sender, EventArgs e)
    {
        if (Validation() == true)
        {
            AttachUser();
            
          
        }
    }

    private void AttachUser()
    {
          try
        {
                List<User> AttachUser = null;
                //User usr = null;
                bool isselected = false;
                AttachUser = new List<User>();
                for (int i = 0; i < gvwunattachedUser.Rows.Count; i++)
                {
                    GridViewRow row = gvwunattachedUser.Rows[i];
                    bool isChecked = ((CheckBox)row.FindControl("Selecteduser")).Checked;
                    if (isChecked)
                    {
                        try
                        {
                            isselected = true;
                            User usr = new User(Int32.Parse(gvwunattachedUser.DataKeys[i].Values["Id"].ToString()));
                            AttachUser.Add(usr);
                        }
                        catch
                        {
                        }
                    }
                }

                if (isselected == true)
                {
                    muserauthentication = new UserAuthentication();
                    this.mAppManager = muserauthentication.AppManager;
                    mUserService = AppService.Create<IUserService>();
                    mUserService.AppManager = mAppManager;
                    mUserService.AppManager = Session["APP_MANAGER"] as IAppManager;
                    mUserService.AttachToHierarchy(AttachUser, Convert.ToDateTime(txtDate.Text), Convert.ToInt32(HidAttachUser.Value));
                    this.alertError.InnerHtml = "User Attached Sucessfully";
                    this.alertError.Style["display"] = "Block";
                    //ClearControls();
                    RetrieveUnAttachedUsers();
                }
                else
                {

                    this.alertError.InnerHtml = "Select User";
                    this.alertError.Style["display"] = "Block";
                }
        }
          catch { throw; }
          finally
          {
              if (mUserService != null) mUserService.Dispose();
              mUserService = null;
          }
    }

    private bool Validation()
    {
        DateTime value;
        DateTime dt1 = new DateTime();
        bool result = true;
        if (txtDate.Text == "")
        {
            this.alertError.Style["display"] = "Block";
            this.alertError.InnerHtml = "Select Date";
            return false;
        }
        else
        {
            this.alertError.Style["display"] = "none";
            this.alertError.InnerHtml = "";
            if ((txtDate.Text.Trim().Length == 8) && (txtDate.Text.IndexOf("/") == -1))
            {
                txtDate.Text = txtDate.Text.Insert(2, "/");
                txtDate.Text = txtDate.Text.Insert(5, "/");
            }
            if (txtDate.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtDate.Text, out value))
                {
                    dt1 = Convert.ToDateTime(txtDate.Text);
                }
                else
                {
                    this.alertError.Style["display"] = "block";
                    this.alertError.InnerHtml = "Invalid Attach Date ,Date should be (MM/DD/YYYY) format";
                    result = false;
                }
            }
            return result;
        }
    }
    private void ClearControls()
    {
        txtUserName.Text = "";
        txtRole.Text = "";
        txtDate.Text = "";
        gvwunattachedUser.DataSource = null;
        gvwunattachedUser.DataBind();
        this.divGridHeader.Visible = true;
        divGridview.Visible = false;
        divmatchedusers.InnerText = "";
    }

    protected void checkselectall_Click(object sender, EventArgs e)
    {
        CheckBox chkSelect = (CheckBox)sender;
        if (gvwunattachedUser.Rows.Count > 0)
        {
            if (chkSelect.Checked == true)
            {
                for (int irow = 0; irow < gvwunattachedUser.Rows.Count; irow++)
                {
                    CheckBox chk = (CheckBox)gvwunattachedUser.Rows[irow].FindControl("Selecteduser");
                    chk.Checked = true;


                }
                return;
            }
            else
            {
                for (int irow = 0; irow < gvwunattachedUser.Rows.Count; irow++)
                {
                    CheckBox chk = (CheckBox)gvwunattachedUser.Rows[irow].FindControl("Selecteduser");
                    chk.Checked = false;
                }
            }
        }
    }


}