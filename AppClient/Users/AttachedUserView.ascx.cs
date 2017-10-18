using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Users_AttachedUserView : System.Web.UI.UserControl
{

    #region Class Variables
    IUserService mUserService = null;
    IAppManager mAppManager = null;
    UserAuthentication muserauthentication = null;
    List<Tks.Entities.User> User = null;
    public string mischeck="0";
    public string mParentUserId = "0";

    string SELECTUSER;
    string DETETCHMSG;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            LoadLabels();
            if (!IsPostBack)
            {
                RetrieveAttachedUsers();
                txtDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
            }
            this.alertError.Style["display"] = "none";
            this.alertError.InnerHtml = "";
            
        }
        catch
        {
            throw;
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            //this.divEmptyRow.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

        var ADDUPDATE_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ADDUPDATEMSG")).FirstOrDefault();
        if (ADDUPDATE_MSG != null)
        {
            SELECTUSER = Convert.ToString(ADDUPDATE_MSG.DisplayText);
            DETETCHMSG = Convert.ToString(ADDUPDATE_MSG.SupportingText1);
        }


    }

    public void AssignuserId(string ParentuserId)
    {
        Hidparentid.Value = ParentuserId;
    }
    public void RetrieveAttachedUsers()
    {
        try
       {
            
           muserauthentication = new UserAuthentication();
           this.mAppManager = muserauthentication.AppManager;
            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = mAppManager;
           // retrieve
            if (Hidparentid.Value != null && Hidparentid.Value != "")
            {
                User = mUserService.RetrieveAttachedUsers(Convert.ToInt32(Hidparentid.Value));
            }
            if (User != null)
            {
                gvwattachedUser.DataSource = User;
                gvwattachedUser.DataBind();

                List<LblLanguage> lblLanguagelst = null;
                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIE");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwattachedUser);
                divGridHeader.Visible = false;
                //Intialize the View
                //this.divGridHeader.Visible = false;
                //this.spnMessage.InnerHtml = "<b> No user Assigned.</b>";
                //divGridview.Visible = true;
            }
            else
            {

               
                //divGridHeader.Visible = false;

                gvwattachedUser.DataSource = null;
                gvwattachedUser.DataBind();

                List<LblLanguage> lblLanguagelst = null;
                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIE");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwattachedUser);
                //this.divGridHeader.Visible = true;
                //this.spnMessage.InnerHtml = "<b> No user Assigned.</b>";
                divGridview.Visible = false;
            }

        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }

    }

    private void DetachUser()
    {
        try
        {

            bool isselected = false;
            List<User> DetachUser = null;
            //User usr = null;
            DetachUser = new List<User>();
            for (int i = 0; i < gvwattachedUser.Rows.Count; i++)
            {
                GridViewRow row = gvwattachedUser.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("Selecteduser")).Checked;
                if (isChecked)
                {
                    try
                    {
                        isselected = true;

                        User usr = new User(Int32.Parse(gvwattachedUser.DataKeys[i].Values["Id"].ToString()));
                        DetachUser.Add(usr);
                    }
                    catch
                    {
                    }
                }
            }
            if (isselected == true)
            {
                mUserService = AppService.Create<IUserService>();
                mUserService.AppManager = mAppManager;
                mUserService.AppManager = Session["APP_MANAGER"] as IAppManager;
                mUserService.DetachFromHierarchy(DetachUser, Convert.ToDateTime(txtDate.Text));
                this.alertError.InnerHtml = DETETCHMSG;
                this.alertError.Style["display"] = "Block";
                txtDate.Text = "";
            }
            else
            {
                this.alertError.InnerHtml = SELECTUSER;
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


    protected void bntDetach_Click(object sender, EventArgs e)
    {
        if (Validation() == true)
        {
            DetachUser();
            RetrieveAttachedUsers();
            
        }
    }

    protected void checkselectall_Click(object sender, EventArgs e)
    {
        CheckBox chkSelect=(CheckBox) sender;
        if (gvwattachedUser.Rows.Count > 0)
        {
            List<LblLanguage> lblLanguagelst = null;
            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIE");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, gvwattachedUser);
            divGridHeader.Visible = false;

            if (chkSelect.Checked == true)
            {
                for (int irow = 0; irow < gvwattachedUser.Rows.Count; irow++)
                {
                    CheckBox chk = (CheckBox)gvwattachedUser.Rows[irow].FindControl("Selecteduser");
                    chk.Checked = true;


                }
                return;
            }
            else
            {
                for (int irow = 0; irow < gvwattachedUser.Rows.Count; irow++)
                {
                    CheckBox chk = (CheckBox)gvwattachedUser.Rows[irow].FindControl("Selecteduser");
                    chk.Checked = false;
                }
            }
        }
        else
        {
            divGridHeader.Visible = true;
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
                    this.alertError.InnerHtml = "Invalid Detach Date ,Date should be (MM/DD/YYYY) format";
                    result = false;
                }
            }
            return result;
        }
    }

    public bool Changemanager(string managerchangedate,string managerid)
    {
        bool isselected = false;
        try
        {
                      
            List<User> DetachUser = null;
            //User usr = null;
            DetachUser = new List<User>();
            for (int i = 0; i < gvwattachedUser.Rows.Count; i++)
            {

                List<LblLanguage> lblLanguagelst = null;
                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIE");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwattachedUser);
                divGridHeader.Visible = false;

                GridViewRow row = gvwattachedUser.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("Selecteduser")).Checked;
                if (isChecked)
                {
                    try
                    {
                        isselected = true;

                        User usr = new User(Int32.Parse(gvwattachedUser.DataKeys[i].Values["Id"].ToString()));
                        if (usr.Id.ToString() == managerid)
                        {
                            this.alertError.InnerHtml = "Selected manager also in selected user list";
                            this.alertError.Style["display"] = "Block";
                            return false;
                        }
                        else
                        {
                            DetachUser.Add(usr);
                        }
                       
                    }
                    catch
                    {
                    }
                }
            }
            if (isselected == true)
            {
                mUserService = AppService.Create<IUserService>();
                mUserService.AppManager = mAppManager;
                mUserService.AppManager = Session["APP_MANAGER"] as IAppManager;
                mUserService.ChangeManager(DetachUser, Convert.ToDateTime(managerchangedate));
                mUserService.AttachToHierarchy(DetachUser, Convert.ToDateTime(managerchangedate), Convert.ToInt32(managerid));
                this.alertError.InnerHtml = "User changed Sucessfully";
                this.alertError.Style["display"] = "Block";
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
        return isselected;
    }


 }