using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Xml.XPath;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


public partial class MasterPages_AppMaster : System.Web.UI.MasterPage
{
    #region Class Variables

    IAppManager mAppManager;
    List<Tks.Entities.Menu> listMenu;
    IMenuService menuService;
    DataTable dtMenu = null;
    bool Isallowed = false;
    #endregion

    #region Public Members

    public IAppManager AppManager
    {
        get { return mAppManager; }
    }

    public string PageTitle
    {
        get;
        set;
    }


    #endregion

    #region Internal members

    private bool IsSessionExpire()
    {
        bool isExpired;
        try
        {            

             if (Session["TKS_SESSION_ID"] == null
                   || !Session.SessionID.Equals(Session["TKS_SESSION_ID"])
                   || Session["APP_MANAGER"] == null) 
                isExpired = true;
            else
                isExpired = false;

            return isExpired;
        }
        catch { throw; }
    }

    private void OnPageInitialize()
    {        

        if (this.IsSessionExpire())
        {
            // Goto expire page.
            Response.Redirect("~/Misc/App-Expire", true);
            return;
        }

        // Get IAppManager reference from session.
        mAppManager = Session["APP_MANAGER"] as IAppManager;
    }

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            System.Diagnostics.Debug.Print("Master page Init event: " + DateTime.Now.ToString());

            OnPageInitialize();
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Workaround for asp.net menu control.
            if (Request.UserAgent.IndexOf("AppleWebKit") > 0
                    || Request.UserAgent.IndexOf("Chrome") > 0
                    || Request.UserAgent.IndexOf("Firefox") > 0
                    || Request.UserAgent.IndexOf("Safari") > 0
                    || Request.UserAgent.IndexOf("Unknown") > 0)
            {
                Request.Browser.Adapters.Clear();
            }
            LoadLabels();
            //Display LoginName in HomePage
            this.DisplayLoginName();

            // Attach help page url.
            this.AttachHelpPage();

            System.Diagnostics.Debug.Print("Master page Load event: " + DateTime.Now.ToString());

            // Load the page.
            OnPageLoad();
        }
        catch (Exception ex)
        {
            throw ex;
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "MASTERPAGE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        //var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        //if (GRID_TITLE != null)
        //{

        //    this.InitalspnMessage.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        //}

    }

    private void DisplayLoginName()
    {

        try
        {
            //Display the loginName
            if (this.mAppManager.LoginUser != null)
            {
                string loginusername;
                loginusername = this.mAppManager.LoginUser.LoginName;
                LoginName.InnerHtml = loginusername;
            }
        }
        catch { throw; }
    }

    private void AttachHelpPage()
    {
        try
        {
            // Get current page path.
            this.hlkPageHelp.NavigateUrl = this.RetrieveHelpPageUrl(this.Request.AppRelativeCurrentExecutionFilePath);
        }
        catch { throw; }
    }

    private string RetrieveHelpPageUrl(string pageUrl)
    {
        try
        {
            // Refer the xml file.
            XPathDocument document = new XPathDocument(string.Format("{0}\\{1}", Server.MapPath("~"), "Data\\PageHelpFiles.xml"));
            XPathNavigator navigator = document.CreateNavigator();

            // Get url.
            string xPathExpression = string.Format("//Page[@url='{0}']/HelpUrl", pageUrl);
            XPathNavigator nodeNavigator = navigator.SelectSingleNode(xPathExpression);

            // Return the url.
            return (nodeNavigator != null) ? nodeNavigator.Value : "~/HelpPages/Construction.htm";
        }
        catch { throw; }
    }



    private void OnPageLoad()
    {
        try
        {
            IUserService userService;
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            userService = AppService.Create<IUserService>();
            userService.AppManager = mAppManager;

            HttpRequest request = base.Request;
            string address = request.UserHostAddress;

            if (userService.CheckUserSession_Secure(Session.SessionID, mAppManager.LoginUser.Id, Utility.GetIpAddress()) == false)
            {
                // Redirect the Loginpage.
                //this.Response.Redirect("~/Default.aspx", false);
                this.Response.Redirect("~/Misc/Logoff", false);
            }

            if (!IsPostBack)
            {
                PopulateMenuItem();
                this.mainMenu.Visible = true;

                //Added by saravanan on 05112012
                // if logged in user don't have a rights to view the page then redirect to Loginpage
                if (Isallowed == false && Page.Request.AppRelativeCurrentExecutionFilePath.ToString() != "~/HomePage.aspx")
                {
                    //this.Response.Redirect("~/Default.aspx", false);
                    this.Response.Redirect("~/Misc/Userrestriction.aspx", false);
                    
                }
            }
        }
        catch { throw; }
    }



    private void PopulateMenuItem()
    {
        try
        {
            // Create an instance.
            menuService = AppService.Create<IMenuService>();

            menuService.AppManager = this.mAppManager;
            // Retrieve the menus of based on user.

            // Below commented and added by Mohan 03AUG17 for Localization
            //listMenu = menuService.RetrieveByUser(this.mAppManager.LoginUser.Id);
            listMenu = menuService.RetrieveByUser(this.mAppManager.LoginUser.Id, this.mAppManager.LoginUser.Id);
            // Retreive the menu.
            AddTopMenuItems(RetrieveMenuTables(listMenu), this.mainMenu);

            //mainMenu.
        }
        catch { throw; }

        finally
        {
            if (menuService != null)
            {
                menuService.Dispose();
                menuService = null;
            }
        }

    }


    /// Filter the data to get only the rows that have a
    /// null ParentID (This will come on the top-level menu items)

    private void AddTopMenuItems(DataTable menuData, System.Web.UI.WebControls.Menu applicationMenu)
    {
        DataView view = new DataView(menuData);
        view.RowFilter = "ParentId = 0";

        // Create an instance.

      
        //  this.Menu1.Items.Clear();

        foreach (DataRowView row in view)
        {

            MenuItem newMenuItem = new MenuItem(row["Text"].ToString());
            newMenuItem.Value = row["MenuId"].ToString();
            newMenuItem.NavigateUrl = row["NavigateUrl"].ToString();
            applicationMenu.Items.Add(newMenuItem);
            AddChildMenuItems(menuData, newMenuItem);
            //if (!string.IsNullOrEmpty(newMenuItem.NavigateUrl))
            //    Response.Redirect(newMenuItem.NavigateUrl, true);

            //Added by saravanan on 05112012
            // checking whether logged in user have rights to view the page or not
            if (row["NavigateUrl"].ToString().Contains("?"))
            {
                if (Page.Request.AppRelativeCurrentExecutionFilePath.ToString() == row["NavigateUrl"].ToString().Substring(0, row["NavigateUrl"].ToString().IndexOf("?")))
                {
                    Isallowed = true;
                }
            }
            else
            {
                if (Page.Request.AppRelativeCurrentExecutionFilePath.ToString() == row["NavigateUrl"].ToString())
                {
                    Isallowed = true;
                }
            }
        }

       
    }


    //This code is used to recursively add child menu items by filtering by ParentID

    private void AddChildMenuItems(DataTable menuData, MenuItem parentMenuItem)
    {
        DataView view = new DataView(menuData);
        view.RowFilter = "ParentId = " + parentMenuItem.Value;
        foreach (DataRowView row in view)
        {
            MenuItem newMenuItem = new MenuItem(row["Text"].ToString(), row["MenuId"].ToString());
            newMenuItem.Value = row["MenuId"].ToString();
            newMenuItem.ImageUrl = row["ImageName"].ToString();
            newMenuItem.NavigateUrl = row["NavigateUrl"].ToString();

            if (row["NavigateUrl"].ToString() != "~/Activities/ActivityEditView.aspx" && row["NavigateUrl"].ToString() != "~/Users/UserEditView.aspx" && row["Text"].ToString() != "Report")
            {
                parentMenuItem.ChildItems.Add(newMenuItem);
                AddChildMenuItems(menuData, newMenuItem);

            }
            else
            {
                Isallowed = true;
            }

            //Added by saravanan on 05112012
            // checking whether logged in user have rights to view the page or not
            if (row["NavigateUrl"].ToString().Contains("?"))
            {
                if (Page.Request.AppRelativeCurrentExecutionFilePath.ToString() == row["NavigateUrl"].ToString().Substring(0, row["NavigateUrl"].ToString().IndexOf("?")))
                {
                    Isallowed = true;
                }
            }
            else
            {
                if (Page.Request.AppRelativeCurrentExecutionFilePath.ToString() == row["NavigateUrl"].ToString())
                {
                    Isallowed = true;
                }
            }
        }
    }



    private DataTable RetrieveMenuTables(List<Tks.Entities.Menu> listMenu)
    {
        try
        {
            DataRow dr = null;
            dtMenu = new DataTable();
            dtMenu.Columns.Add("MenuId");
            dtMenu.Columns.Add("Text");
            dtMenu.Columns.Add("Name");
            dtMenu.Columns.Add("Type");
            dtMenu.Columns.Add("IsPublic");
            dtMenu.Columns.Add("ParentId");
            dtMenu.Columns.Add("ModuleName");
            dtMenu.Columns.Add("FormName");
            dtMenu.Columns.Add("IsValid");
            dtMenu.Columns.Add("Priority");
            dtMenu.Columns.Add("ImageName");
            dtMenu.Columns.Add("ToolTip");
            dtMenu.Columns.Add("NavigateUrl");

            for (int i = 0; i <= listMenu.Count() - 1; i++)
            {
                dr = dtMenu.NewRow();
                dr["MenuId"] = listMenu[i].Id;
                dr["Text"] = listMenu[i].Text;
                dr["Name"] = listMenu[i].Name;
                dr["Type"] = listMenu[i].Type;
                dr["IsPublic"] = listMenu[i].IsPublic;
                dr["ParentId"] = listMenu[i].ParentId;
                dr["ModuleName"] = listMenu[i].ModuleName;
                dr["FormName"] = listMenu[i].FormName;
                dr["IsValid"] = listMenu[i].IsValid;
                dr["Priority"] = listMenu[i].Priority;
                dr["ImageName"] = listMenu[i].ImageName;
                dr["ToolTip"] = listMenu[i].Tooltip;
                dr["NavigateUrl"] = listMenu[i].NavigateUrl;
                dtMenu.Rows.Add(dr);
            }

            return dtMenu;

        }
        catch { throw; }

    }



    //protected void mainMenu_MenuItemClick(object sender, MenuEventArgs e)
    //{
    //    try
    //    {
    //        System.Diagnostics.Debug.Print("Menu clicked: " + e.Item.Text);

    //        // Check whether selected menu item is available or not.


    //        // Store selected root menu.

    //        // Build navigation menu for the selected main menu item.
    //        PopulateMenuItem();
    //        // Navigate to page.


    //    }

    //    catch { throw; }

    //}
}

