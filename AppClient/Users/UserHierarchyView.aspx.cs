using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


public partial class Users_UserHierarchyView : System.Web.UI.Page
{
    #region Class Variables
    IUserService mUserService = null;
    IAppManager mAppManager = null;
    UserAuthentication muserauthentication = null;
   TreeNode _treeNode = null;
   StringBuilder mUserSearchViewDialogScript;
    List<Tks.Entities.User> LstUser = null;
    private string parentNode = null;

    string SELECTUSER;
       #endregion
   

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

         this.InitializeUserSearchViewDialogScript();
         muserauthentication = new UserAuthentication();
        this.mAppManager = muserauthentication.AppManager;
        this.UserSearchView.AppManager = this.mAppManager;
        if (!IsPostBack)
        {
           
            //_treeNode = new TreeNode("Organization", "0");
            
            //TreHierarchy.Nodes.Add(_treeNode);

            //RetrieveAttachedUsers("0");
            this.alertError.Style["display"] = "none";
            this.alertError.InnerHtml = "";
            Hidhierarchy.Value = "";
            pnlmanager.Visible = false;
            RetrieveAttachedUsers();
            //ShowAttachedUser("0");
            AttachedUsers.Visible = true;
            UnAttachedUsers.Visible = false;
            // Display current date as default date.
            this.txtmanagerdate.Text = this.RetrieveSystemCurrentDateTime().ToString("MM/dd/yyyy");
         
        }
        this.alertError.Style["display"] = "none";
        this.alertError.InnerHtml = "";
       
        }
        catch { throw; }
        finally
        {
            
        }
            
    }
    private void InitializeUserSearchViewDialogScript()
    {
        try
        {
            mUserSearchViewDialogScript = new StringBuilder();
            mUserSearchViewDialogScript.Append("$(document).ready(function() { refreshUserSearchView(); });");

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
    protected void ibtSearchUser_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.UserSearchView.ViewName = "SearchManagerforhierarchy";
            //Session.Add("UsermanagerId", SearchManagerid);
            this.UserSearchView.Display();
            this.mUserSearchViewDialogScript.Append("$(document).ready(function(){showUserSearchView();});");
        }
        catch { throw; }

    }
    private void RetrieveAttachedUsers(TreeNode SelectedNode)
    {
        try
        {
            List<Tks.Entities.User> ListedUser = null;
            this.mAppManager = muserauthentication.AppManager;
           
            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = mAppManager;
            parentNode = TreHierarchy.SelectedNode.Value.ToString();
            LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(parentNode));
            if (LstUser!=null)
            {
                TreHierarchy.SelectedNode.ChildNodes.Clear();
                foreach (User usr in LstUser)
                {
                    _treeNode = new TreeNode(usr.LastName+" "+usr.FirstName, usr.Id.ToString());

                    ListedUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(usr.Id.ToString()));

                    if (ListedUser != null)
                    {
                        foreach (User user in ListedUser)
                        {
                            TreeNode childnode = new TreeNode(user.LastName + " " + user.FirstName, user.Id.ToString());
                            _treeNode.ChildNodes.Add(childnode);
                        }
                    }

                    SelectedNode.ChildNodes.Add(_treeNode);
                 
                }
                SelectedNode.Expand();
                pnlmanager.Visible = false;
                if (txtuser.Value == "")
                {
                    this.txtuser.Value = "press F2 to search users";
                }
                if (parentNode == "0")
                    pnlmanager.Visible = false;
                if (UnAttachedUsers.Visible == true)
                    pnlmanager.Visible = false;

            }
            else
            {
                //this.alertError.Style["display"] = "Block";
                //this.alertError.InnerHtml = "User not Assigned";
                pnlmanager.Visible = false;
               
            }
           
        }
        catch { throw; }
        finally
        {
           
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }

    }

    private void RetrieveAttachedUsers()
    {
        try
        {
            List<Tks.Entities.User> ListedUser = null;
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = mAppManager;
            LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(0));
            if (LstUser != null)
            {
                TreHierarchy.Nodes.Clear();
                TreeNode parentnode = new TreeNode("Organization", "0");
                foreach (User usr in LstUser)
                {
                    _treeNode = new TreeNode(usr.LastName + " " + usr.FirstName, usr.Id.ToString());
                    ListedUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(usr.Id.ToString()));
                  
                    if (ListedUser != null)
                    {
                        foreach (User user in ListedUser)
                        {
                            TreeNode childnode = new TreeNode(user.LastName + " " + user.FirstName, user.Id.ToString());
                            _treeNode.ChildNodes.Add(childnode);
                        }
                    }
                    //_treeNode.Selected = true;
                    parentnode.ChildNodes.Add(_treeNode);
                    parentnode.CollapseAll();

                }
                TreHierarchy.Nodes.Add(parentnode);
                 parentnode.Expand();
                ShowAttachedUser("0");
                //pnlmanager.Visible = true;
            }
            else
            {
                pnlmanager.Visible = false;

            }

        }
        catch { throw; }
        finally
        {

            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }

    private void RetrieveAttachedUsers(string userid)
    {
        try
        {
            this.mAppManager = muserauthentication.AppManager;

            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = mAppManager;
            LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(userid));
            if (LstUser != null)
            {
                foreach (User usr in LstUser)
                {
                    _treeNode = new TreeNode(usr.LastName + " " + usr.FirstName, usr.Id.ToString());
                    _treeNode.ExpandAll();
                    TreHierarchy.CheckedNodes.Add(_treeNode);

                }
                pnlmanager.Visible = false;
                
            }
            else
            {
                
                this.alertError.Style["display"] = "Block";
                this.alertError.InnerHtml = "User not Assigned";
                pnlmanager.Visible = false;

            }

        }
        catch { throw; }
        finally
        {
           
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }

    }

  
    protected void TreHierarchy_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {

                this.alertError.Style["display"] = "none";
                this.alertError.InnerHtml = "";
                parentNode = TreHierarchy.SelectedNode.Value.ToString();
                Session.Add("UsermanagerId", parentNode);
                RetrieveAttachedUsers(TreHierarchy.SelectedNode);
                ShowAttachedUser(parentNode);
                txtuser.Value = "";
                txtmanagerdate.Text = "";
                hdnManagerId.Value = "";
        }
        catch { throw; }
        finally
        {
            
        }
        
    }

    private void ShowAttachedUser(string ParentUserID)
    {
        try{
           
            AttachedUsers.mParentUserId = ParentUserID.ToString();
            AttachedUsers.AssignuserId(ParentUserID.ToString());
            AttachedUsers.RetrieveAttachedUsers();
            AttachedUsers.Visible = true;
            UnAttachedUsers.Visible = false;
           
        }
        catch { throw; }
        finally
        {

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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "USERHIERARCHIEPAGE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var ACTSELECTUSER = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTMSGSELUSER")).FirstOrDefault();
        if (ACTSELECTUSER != null)
        {
            SELECTUSER = ACTSELECTUSER.DisplayText;

        }

    }
  
    protected void btnAddnew_Click(object sender, EventArgs e)
    {
        try
        {
            if (TreHierarchy.SelectedNode == null)
            {
                this.alertError.Style["display"] = "block";
                this.alertError.InnerHtml = SELECTUSER;
                return;
            }
            else
            {
                this.alertError.Style["display"] = "none";
                this.alertError.InnerHtml = "";
                UnAttachedUsers.ShowAttachUser(TreHierarchy.SelectedNode.Text.ToString(), TreHierarchy.SelectedValue.ToString());
                UnAttachedUsers.mAtttachUserId = TreHierarchy.SelectedValue.ToString();
                UnAttachedUsers.mAtttachUserName = TreHierarchy.SelectedNode.Text.ToString();
                AttachedUsers.Visible = false;
                UnAttachedUsers.Visible = true;
                pnlmanager.Visible = false;
            }
     
            
          }
        catch { throw; }
        finally
        {

        }
        
    }




    protected void btnReferesh_Click(object sender, EventArgs e)
    {
        if (TreHierarchy.SelectedNode != null)
        {
            parentNode = TreHierarchy.SelectedNode.Value.ToString();
            RetrieveAttachedUsers(TreHierarchy.SelectedNode);
        }
    }
    protected void Page_prerender(object sender, EventArgs e)
    {
        try
        {
            if (TreHierarchy.SelectedNode != null)
            {
                TreHierarchy.SelectedNode.ChildNodes.Clear();
                parentNode = TreHierarchy.SelectedNode.Value.ToString();
                RetrieveAttachedUsers(TreHierarchy.SelectedNode);

            }
            // Register scripts.
            if (this.mUserSearchViewDialogScript != null)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    System.Guid.NewGuid().ToString(),
                    this.mUserSearchViewDialogScript.ToString(),
                    true);
                          
            }
                          
        }
        catch
        {
            throw;
        }
      
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        if (txtuser.Value != "" && txtuser.Value != "press F2 to search users")
        {
            if (IsValiddate() == true)
            {
                if (AttachedUsers.Changemanager(txtmanagerdate.Text, hdnManagerId.Value.ToString()) == true)
                {
                    TreHierarchy.SelectedNode.ChildNodes.Clear();
                    RetrieveAttachedUsers(TreHierarchy.SelectedNode);
                    ShowAttachedUser(TreHierarchy.SelectedNode.Value.ToString());
                    txtuser.Value = "";
                    txtmanagerdate.Text = "";
                    hdnManagerId.Value = "";

                }
            }
        }
        else
        {
            this.alertError.Style["display"] = "block";
            this.alertError.InnerHtml = "Select the manager";
         }
    
       
    }
    private DateTime RetrieveSystemCurrentDateTime()
    {
        SettingProvider provider = null;
        try
        {
            provider = new SettingProvider();
            provider.AppManager = this.mAppManager;
            return provider.GetSystemDateTime();
        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    private bool IsValiddate()
    {
        DateTime value;
        DateTime dt1 = new DateTime();
         bool result = true;

        try
        {
            this.alertError.Style["display"] = "none";
            this.alertError.InnerHtml = "";
            if (txtmanagerdate.Text.Trim() != "")
            {
                if ((txtmanagerdate.Text.Trim().Length == 8) && (txtmanagerdate.Text.IndexOf("/") == -1))
                {
                    txtmanagerdate.Text = txtmanagerdate.Text.Insert(2, "/");
                    txtmanagerdate.Text = txtmanagerdate.Text.Insert(5, "/");
                }
                if (txtmanagerdate.Text.Trim() != "")
                {
                    if (DateTime.TryParse(txtmanagerdate.Text, out value))
                    {
                        dt1 = Convert.ToDateTime(txtmanagerdate.Text);
                    }
                    else
                    {
                        this.alertError.Style["display"] = "block";
                        this.alertError.InnerHtml = "Invalid manager change Date ,Date should be (MM/DD/YYYY) format";
                        result = false;
                    }
                }
            }
            else
            {
                this.alertError.Style["display"] = "block";
                this.alertError.InnerHtml = "Select manager change Date";
                result = false;
            }
         
            return result;
        }
        catch { throw; }
    }
   
}