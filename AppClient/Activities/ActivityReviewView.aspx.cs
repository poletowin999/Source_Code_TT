using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Data;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


public partial class Activities_ActivityReviewView : System.Web.UI.Page
{

    #region Class Variables
    IUserService mUserService = null;
    IActivityService mActivityService = null;
    IAppManager mAppManager = null;
    UserAuthentication muserauthentication = null;
    TreeNode _treeNode = null;
    List<Tks.Entities.User> LstUser = null;
    List<Tks.Entities.UserActivitySummary> LstActivitysummary = null;
    HiddenField hidactivitydate = null;
    HiddenField hiduserid = null;
    private string parentNode = null;
    const string ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE = "ActivityEditViewPage_SucceedMessage";
    private string mFormname = "";

    string ACTAPRSUS;
    string ACTREJSUS;

    string ACTNOTFND;
    string USRACTNOTFND;

    string SELECTACTIVITIES;
    string ENTERCOMMENTS;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            this.alertError.Style["display"] = "none";
            this.alertError.InnerHtml = "";

            //RetrieveAttachedUsers();
            loadActivities(mAppManager.LoginUser.Id);
            //if (((object)Request.QueryString["Action"]) != null)
            if (((object)this.Page.RouteData.Values["Action"]) != null)                    
            {
                this.mFormname = Convert.ToString(this.Page.RouteData.Values["Action"]);  // Request.QueryString["Action"].ToString();
            }
            if (mFormname == "retain")
            {
                this.alertError.Style["display"] = "Block";
                this.alertError.InnerHtml = RetrieveOtherPageSucceedMessage();
                loadUsersActivities(Convert.ToInt32(Session["VwUserid"].ToString()), Convert.ToDateTime(Session["VwActivitydate"].ToString()), Convert.ToString(Session["VwUsername"].ToString()));
            }
            else if (mFormname == "close")
            {
                loadUsersActivities(Convert.ToInt32(Session["VwUserid"].ToString()), Convert.ToDateTime(Session["VwActivitydate"].ToString()), Convert.ToString(Session["VwUsername"].ToString()));
            }

        }
        this.alertError.Style["display"] = "none";
        this.alertError.InnerHtml = "";
        showApporval();
        LoadLabels();
    }


    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLACTIVITYARRJT")).FirstOrDefault();
        if (GRID_TITLE != null)
        {
            ACTAPRSUS = Convert.ToString(GRID_TITLE.DisplayText);
            ACTREJSUS = Convert.ToString(GRID_TITLE.SupportingText1);
        }

        var AlertUserActivity = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLUSERACTIVITYNOTFOUND")).FirstOrDefault();
        if (GRID_TITLE != null)
        {
            USRACTNOTFND = Convert.ToString(AlertUserActivity.DisplayText);
        }

        var AlertActivity = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLACTIVITYNOTFOUND")).FirstOrDefault();
        if (AlertActivity != null)
        {
            ACTNOTFND = Convert.ToString(AlertActivity.DisplayText);
        }

        var LBLSELECTACTANDCMTS = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("SELECTACTANDCMTS")).FirstOrDefault();
        if (LBLSELECTACTANDCMTS != null)
        {
            SELECTACTIVITIES = Convert.ToString(LBLSELECTACTANDCMTS.DisplayText);
            ENTERCOMMENTS = Convert.ToString(LBLSELECTACTANDCMTS.SupportingText1);
        }

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
    private string RetrieveOtherPageSucceedMessage()
    {
        try
        {
            string message = string.Empty;

            // Read from session.
            if (Session[ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE] != null)
                message = Session[ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE].ToString();

            // Next time need not to display this message so remove from session.
            Session.Remove(ACTIVITY_EDIT_VIEW_SUCCEED_MESSAGE);

            return message;
        }
        catch { throw; }
    }

    private void loadActivities(int userid)
    {
        try
        {

            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mActivityService = AppService.Create<IActivityService>();
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mActivityService.AppManager = mAppManager;
            // retrieve
            //LstActivitysummary = mActivityService.RetrieveActivitySummaryByUser(Convert.ToInt32(TreHierarchy.SelectedValue.ToString()));
            List<Tks.Entities.UserActivitySummary> Activitysummarylst = null;
            LstActivitysummary = mActivityService.RetrieveActivitySummaryByUser(userid);
            if (LstActivitysummary != null)
            {
                // Filter only active location.
                IEnumerable<UserActivitySummary> list = from item in LstActivitysummary
                                                        where Convert.ToInt16(item.ActivityCount.ToString()) > 0
                                                        select item;

                Activitysummarylst = list.ToList<UserActivitySummary>();

                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwselectedactivity);
                //Gridlabel.Visible = false;
            }

            //Sample(LstActivitysummary);
            if (LstActivitysummary != null)
            {
                gvwselectedactivity.Columns[1].Visible = true;
                gvwselectedactivity.DataSource = Activitysummarylst;
                gvwselectedactivity.DataBind();
                divgvwselectedactivity.Visible = true;
                divgvwselectedactivityHeader.Visible = false;
                this.spnMsg.InnerHtml = USRACTNOTFND;
                divgvwListactivity.Visible = false;
                this.spnMessage.InnerHtml = ACTNOTFND;
                divgvwListactivityHeader.Visible = true;
                gvwselectedactivity.Columns[1].Visible = false;
            }
            else
            {
                gvwselectedactivity.DataSource = null;
                gvwselectedactivity.DataBind();
                gvwListactivity.DataSource = null;
                gvwListactivity.DataBind();
                divgvwselectedactivity.Visible = false;
                divgvwselectedactivityHeader.Visible = true;
                this.spnMsg.InnerHtml = USRACTNOTFND;
                divgvwListactivity.Visible = false;
                this.spnMessage.InnerHtml = ACTNOTFND;
                divgvwListactivityHeader.Visible = true;
            }
            //LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(mAppManager.LoginUser.Id));

            if (LstActivitysummary != null)
            {

                List<UserActivitySummary> lst = new List<UserActivitySummary>();

                int Count = 0;
                foreach (UserActivitySummary usr in LstActivitysummary)
                {
                    UserActivitySummary userls = new UserActivitySummary();
                    Count = 0;
                    foreach (UserActivitySummary usrcount in LstActivitysummary)
                    {
                        if (usr.UserName == usrcount.UserName & usrcount.ActivityCount > 0 & usr.UserId == usrcount.UserId)
                        {
                            Count = Count + 1;
                        }
                    }


                    userls.ActivityCount = Count;
                    userls.ActivityDate = usr.ActivityDate;
                    userls.UserId = usr.UserId;
                    userls.UserName = usr.UserName;
                    //Commented by saravanan on 07312012 release later
                    // userls.CustomData["ActiveStatus"] = usr.CustomData["ActiveStatus"].ToString();
                    //lst.Add(userls);
                    UserActivitySummary projectLocation = lst.Where(item => item.UserId == usr.UserId).FirstOrDefault();
                    // If it is not found then add to list.
                    if (projectLocation == null)
                    {
                        lst.Add(userls);

                    }
                }
                LstActivitysummary = lst;
                TreHierarchy.Nodes.Clear();
                //Commented by saravanan on 07312012 release later
                //TreeNode parentnode = new TreeNode(mAppManager.LoginUser.LastName.ToString() + " " + mAppManager.LoginUser.FirstName.ToString(), mAppManager.LoginUser.Id.ToString());
                TreeNode parentnode = new TreeNode(mAppManager.LoginUser.FirstName.ToString() + " " + mAppManager.LoginUser.LastName.ToString(), mAppManager.LoginUser.Id.ToString());
                foreach (UserActivitySummary usr in LstActivitysummary)
                {
                    //Commented by saravanan on 07312012 release later
                    //if (usr.CustomData["ActiveStatus"].ToString() == "InActive")
                    //{
                    //    _treeNode = new TreeNode(usr.UserName + " " + "(" + usr.ActivityCount + ") Days", usr.UserId.ToString());
                    //    _treeNode.Value = "0";
                    //    _treeNode.ToolTip = "changed user";
                    //    parentnode.ChildNodes.Add(_treeNode);
                    //}
                    //else
                    //{
                    _treeNode = new TreeNode(usr.UserName + " " + "(" + usr.ActivityCount + ") Days", usr.UserId.ToString());
                    parentnode.ChildNodes.Add(_treeNode);
                    // }
                }
                TreHierarchy.Nodes.Add(parentnode);
                TreHierarchy.ExpandAll();
                parentnode.Selected = true;
                //loadActivities();
            }
            else
            {
                RetrieveAttachedUsers();
            }
        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }

    private void loadActivities()
    {
        try
        {

            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mActivityService = AppService.Create<IActivityService>();
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mActivityService.AppManager = mAppManager;
            // retrieve
            LstActivitysummary = mActivityService.RetrieveActivitySummaryByUser(Convert.ToInt32(TreHierarchy.SelectedValue.ToString()));
            //LstActivitysummary = mActivityService.RetrieveActivitySummaryByUser(userid);
            //Sample(LstActivitysummary);
            if (LstActivitysummary != null)
            {
                gvwselectedactivity.Columns[1].Visible = true;
                gvwselectedactivity.DataSource = LstActivitysummary;
                gvwselectedactivity.DataBind();
                divgvwselectedactivity.Visible = true;
                divgvwselectedactivityHeader.Visible = false;
                divgvwListactivity.Visible = false;
                divgvwListactivityHeader.Visible = false;
                gvwselectedactivity.Columns[1].Visible = false;
            }
            else
            {
                gvwselectedactivity.DataSource = null;
                gvwselectedactivity.DataBind();
                gvwListactivity.DataSource = null;
                gvwListactivity.DataBind();
                divgvwselectedactivity.Visible = false;
                divgvwselectedactivityHeader.Visible = true;
                this.spnMsg.InnerHtml = USRACTNOTFND;
                divgvwListactivity.Visible = false;
                this.spnMessage.InnerHtml = ACTNOTFND;
                divgvwListactivityHeader.Visible = true;
            }

        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }
    private void loadUsersActivities(int Userid, DateTime Activitydate, string Username)
    {
        try
        {
            List<Activity> Activitylst = null;
            Activitylst = new List<Activity>();
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mActivityService = AppService.Create<IActivityService>();
            mActivityService.AppManager = mAppManager;
            // retrieve
            Activitylst = mActivityService.Retrieve(Userid, Activitydate);
            if (Activitylst != null)
            {
                var resultList = from item in Activitylst
                                 where item.StatusId == 1
                                 select item;


                //Activitylst = resultList t

                Activitylst = resultList.ToList<Activity>();


            }


            //Sample(LstActivitysummary);
            if (Activitylst != null && Activitylst.Count > 0)
            {
                gvwListactivity.DataSource = Activitylst;

                gvwListactivity.DataBind();
                divgvwselectedactivity.Visible = true;
                divgvwselectedactivityHeader.Visible = false;
                divgvwListactivity.Visible = true;
                divgvwListactivityHeader.Visible = false;
                Session.Add("SSActivitylst", Activitylst);
                this.spnuserdtls.InnerHtml = hdnActivitydetailsfor.Text + Username + ";" + lblActivityDate.Text +" " + Activitydate;

                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwListactivity);

            }
            else
            {
                divgvwListactivity.Visible = false;
                divgvwListactivityHeader.Visible = true;
                this.spnuserdtls.InnerHtml = "";
            }

        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }

    private void loadActivitiesDetails(List<Activity> Activitylst)
    {
        if (Activitylst != null)
        {
            gvwselectedactivity.Columns[1].Visible = true;
            gvwselectedactivity.DataSource = Activitylst;
            gvwselectedactivity.DataBind();
            gvwselectedactivity.Columns[1].Visible = false;
        }
    }
    private void RetrieveAttachedUsers(TreeNode SelectedNode)
    {
        try
        {
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = mAppManager;
            LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(parentNode));
            if (LstUser != null)
            {
                TreHierarchy.SelectedNode.ChildNodes.Clear();
                foreach (User usr in LstUser)
                {
                    //Commented by saravanan on 07312012 release later
                    // _treeNode = new TreeNode(usr.LastName + " " + usr.FirstName, usr.Id.ToString());
                    _treeNode = new TreeNode(usr.FirstName + " " + usr.LastName, usr.Id.ToString());
                    SelectedNode.ChildNodes.Add(_treeNode);

                }
                SelectedNode.Expand();
            }
            else
            {


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
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mUserService = AppService.Create<IUserService>();
            mUserService.AppManager = mAppManager;
            LstUser = mUserService.RetrieveAttachedUsers(Convert.ToInt32(mAppManager.LoginUser.Id));
            if (LstUser != null)
            {
                TreHierarchy.Nodes.Clear();
                //Commented by saravanan on 07312012 release later
                //TreeNode parentnode = new TreeNode(mAppManager.LoginUser.LastName.ToString() + " " + mAppManager.LoginUser.FirstName.ToString(), mAppManager.LoginUser.Id.ToString());
                TreeNode parentnode = new TreeNode(mAppManager.LoginUser.FirstName.ToString() + " " + mAppManager.LoginUser.LastName.ToString(), mAppManager.LoginUser.Id.ToString());
                foreach (User usr in LstUser)
                {
                    //Commented by saravanan on 07312012 release later
                    //_treeNode = new TreeNode(usr.LastName + " " + usr.FirstName, usr.Id.ToString());
                    _treeNode = new TreeNode(usr.FirstName + " " + usr.LastName, usr.Id.ToString());
                    parentnode.ChildNodes.Add(_treeNode);

                }
                TreHierarchy.Nodes.Add(parentnode);
                TreHierarchy.ExpandAll();
                parentnode.Selected = true;

            }
            else
            {


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
            if (TreHierarchy.SelectedNode.Depth.ToString() == "3")
            {
                mAppManager = Session["APP_MANAGER"] as IAppManager;
                if (mAppManager.LoginUser.HasAdminRights == true)
                {
                    if (TreHierarchy.SelectedNode.Text.Contains(" ") == false)
                    {
                        parentNode = TreHierarchy.SelectedNode.Value.ToString();
                        loadActivities(TreHierarchy.SelectedNode);
                    }
                }
                else
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = "You don't have a rights";
                    gvwselectedactivity.DataSource = null;
                    gvwselectedactivity.DataBind();
                    gvwListactivity.DataSource = null;
                    gvwListactivity.DataBind();
                    divgvwselectedactivity.Visible = false;
                    divgvwselectedactivityHeader.Visible = true;
                    this.spnMsg.InnerHtml = USRACTNOTFND;
                    divgvwListactivity.Visible = false;
                    this.spnMessage.InnerHtml = ACTNOTFND;
                    divgvwListactivityHeader.Visible = true;
                    this.spnuserdtls.InnerHtml = "";
                }

            }
            else
            {
                if (TreHierarchy.SelectedNode.Value.ToString() != "0")
                {
                    parentNode = TreHierarchy.SelectedNode.Value.ToString();
                    loadActivities(TreHierarchy.SelectedNode);
                }
            }

        }
        catch { throw; }
        finally
        {

        }
    }


    private void loadActivities(TreeNode SelectedNode)
    {
        try
        {

            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mActivityService = AppService.Create<IActivityService>();
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mActivityService.AppManager = mAppManager;
            // retrieve
            List<Tks.Entities.UserActivitySummary> Activitysummarylst = null;
            LstActivitysummary = mActivityService.RetrieveActivitySummaryByUser(Convert.ToInt32(TreHierarchy.SelectedValue.ToString()));

            if (LstActivitysummary != null)
            {
                // Filter only active location.
                IEnumerable<UserActivitySummary> list = from item in LstActivitysummary
                                                        where Convert.ToInt16(item.ActivityCount.ToString()) > 0
                                                        select item;

                Activitysummarylst = list.ToList<UserActivitySummary>();
            }

            if (Activitysummarylst != null)
            {
                gvwselectedactivity.Columns[1].Visible = true;
                gvwselectedactivity.Columns[2].Visible = true;
                gvwselectedactivity.DataSource = Activitysummarylst;
                gvwselectedactivity.DataBind();
                divgvwselectedactivity.Visible = true;
                divgvwselectedactivityHeader.Visible = false;
                divgvwListactivity.Visible = false;
                divgvwListactivityHeader.Visible = true;
                gvwselectedactivity.Columns[1].Visible = false;
                gvwselectedactivity.Columns[2].Visible = true;
                chkapproval.Checked = false;
            }
            else
            {
                gvwselectedactivity.DataSource = null;
                gvwselectedactivity.DataBind();
                gvwListactivity.DataSource = null;
                gvwListactivity.DataBind();
                divgvwselectedactivity.Visible = false;
                divgvwselectedactivityHeader.Visible = true;
                this.spnMsg.InnerHtml = USRACTNOTFND;
                divgvwListactivity.Visible = false;
                this.spnMessage.InnerHtml = ACTNOTFND;
                divgvwListactivityHeader.Visible = true;
                this.spnuserdtls.InnerHtml = "";
            }
            if (LstActivitysummary != null)
            {
                List<UserActivitySummary> lst = new List<UserActivitySummary>();

                int Count = 0;
                foreach (UserActivitySummary usr in LstActivitysummary)
                {
                    UserActivitySummary userls = new UserActivitySummary();
                    Count = 0;
                    foreach (UserActivitySummary usrcount in LstActivitysummary)
                    {
                        if (usr.UserName == usrcount.UserName & usrcount.ActivityCount > 0 & usr.UserId == usrcount.UserId)
                        {
                            Count = Count + 1;
                        }
                    }


                    userls.ActivityCount = Count;
                    userls.ActivityDate = usr.ActivityDate;
                    userls.UserId = usr.UserId;
                    userls.UserName = usr.UserName;
                    //Commented by saravanan on 07312012 release later
                    //  userls.CustomData["ActiveStatus"] = usr.CustomData["ActiveStatus"].ToString();
                    //lst.Add(userls);
                    UserActivitySummary projectLocation = lst.Where(item => item.UserId == usr.UserId).FirstOrDefault();
                    // If it is not found then add to list.
                    if (projectLocation == null)
                    {
                        lst.Add(userls);

                    }
                }
                LstActivitysummary = lst;

                TreHierarchy.SelectedNode.ChildNodes.Clear();
                foreach (UserActivitySummary usr in LstActivitysummary)
                {
                    //Commented by saravanan on 07312012 release later
                    //if (usr.CustomData["ActiveStatus"].ToString() == "InActive")
                    //{
                    //    _treeNode = new TreeNode(usr.UserName + " " + "(" + usr.ActivityCount + ") Days", usr.UserId.ToString());
                    //    _treeNode.Value = "0";
                    //    _treeNode.ToolTip = "Changed user";

                    //    SelectedNode.ChildNodes.Add(_treeNode);
                    //}
                    //else
                    //{
                    _treeNode = new TreeNode(usr.UserName + " " + "(" + usr.ActivityCount + ") Days", usr.UserId.ToString());
                    SelectedNode.ChildNodes.Add(_treeNode);
                    // }

                }
                SelectedNode.ExpandAll();
            }
        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }


    protected void gvwselectedactivity_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {


            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvwselectedactivity.Rows[index];
                hidactivitydate = (HiddenField)row.FindControl("hidactivitydate");
                hiduserid = (HiddenField)row.FindControl("hiduserid");
                HiddenField hiuser = (HiddenField)row.FindControl("Hidusername");
                Session["VwActivitydate"] = hidactivitydate.Value.ToString();
                Session["VwUserid"] = hiduserid.Value.ToString();
                string activitydate = hidactivitydate.Value.ToString();
                string userid = hiduserid.Value.ToString();
                Session["VwUsername"] = hiuser.Value.ToString();
                loadUsersActivities(Convert.ToInt32(userid), Convert.ToDateTime(activitydate), Convert.ToString(hiuser.Value.ToString()));

            }
            else
            {
                this.spnuserdtls.InnerHtml = "";
            }
        }
        catch { throw; }
    }


    private bool ApproveActivity(int Approved)
    {
        try
        {
            bool isselected = false;

            List<Activity> Activitylst = null;
            //User usr = null;
            Activitylst = new List<Activity>();
            if (gvwListactivity.Rows.Count > 0)
            {
                for (int i = 0; i < gvwListactivity.Rows.Count; i++)
                {
                    //GridViewRow row = gvwListactivity.Rows[i];
                    //bool isChecked = ((CheckBox)row.FindControl("SelectedActivity")).Checked;
                    //if (isChecked)
                    //{
                    try
                    {
                        User usr = new User();
                        isselected = true;
                        Activity OActivity = new Tks.Entities.Activity();
                        //Label lblActivity = (Label)row.FindControl("ActivityId");
                        //OActivity.Id = Convert.ToInt32(lblActivity.Text.Trim());
                        OActivity.Id = Int32.Parse(gvwListactivity.DataKeys[i].Values["Id"].ToString());
                        OActivity.Comment = txtcomment.Text.Trim();
                        OActivity.IsReviewed = true;
                        OActivity.StatusId = Approved;
                        Activitylst.Add(OActivity);
                    }
                    catch
                    {
                    }
                    //}
                }
            }
            if (isselected == true)
            {
                if (txtcomment.Text.Trim() == "" && Approved == 3)
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = ENTERCOMMENTS;
                    txtcomment.Focus();
                    return false;
                }
                gvwselectedactivity.DataSource = null;
                gvwselectedactivity.DataBind();
                gvwListactivity.DataSource = null;
                gvwListactivity.DataBind();
                divgvwselectedactivity.Visible = false;
                divgvwselectedactivityHeader.Visible = true;
                this.spnMsg.InnerHtml = USRACTNOTFND;
                divgvwListactivity.Visible = false;
                this.spnMessage.InnerHtml = ACTNOTFND;
                divgvwListactivityHeader.Visible = true;
                muserauthentication = new UserAuthentication();
                this.mAppManager = muserauthentication.AppManager;
                mActivityService = AppService.Create<IActivityService>();
                mAppManager = Session["APP_MANAGER"] as IAppManager;
                mActivityService.AppManager = mAppManager;
                mActivityService.Approve(Activitylst);
                parentNode = TreHierarchy.SelectedNode.Value.ToString();
                //loadActivities(Convert.ToInt32(parentNode));
                loadActivities(TreHierarchy.SelectedNode);
                loadUsersActivities(Convert.ToInt32(Session["VwUserid"].ToString()), Convert.ToDateTime(Session["VwActivitydate"].ToString()), Convert.ToString(Session["VwUsername"].ToString()));

                this.alertError.Style["display"] = "none";
                this.alertError.InnerHtml = "";
                if (Approved == 2)
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = ACTAPRSUS;
                    txtcomment.Text = "";
                }
                else
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = ACTREJSUS;
                }
                return true;
            }
            else
            {


                this.alertError.Style["display"] = "Block";
                this.alertError.InnerHtml = SELECTACTIVITIES;
                return false;
            }
        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        if (chkapproval.Checked == true)
        {
            BulkApproveActivity(2);
        }
        else
        {
            ApproveActivity(2);
        }
    }
    //protected void btnReject_Click(object sender, EventArgs e)
    //{

    //        this.alertError.Style["display"] = "none";
    //        this.alertError.InnerHtml = "";
    //        if (ApproveActivity(3) == true)
    //        {
    //            // Compose mail message.
    //            MailMessage message = this.SendRejectedActivityDetails();

    //            // Send the mail.
    //            muserauthentication = new UserAuthentication();
    //            this.mAppManager = muserauthentication.AppManager;
    //            mActivityService = AppService.Create<IActivityService>();
    //            mAppManager = Session["APP_MANAGER"] as IAppManager;
    //            mActivityService.AppManager = mAppManager;
    //            mActivityService.SendRejectMail(message);
    //            txtcomment.Text = "";
    //        }

    //}


    protected void gvwListactivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (e.NewPageIndex < 0)
            {
                gvwListactivity.PageIndex = 0;

            }
            else
            {
                gvwListactivity.PageIndex = e.NewPageIndex;
            }

            loadUsersActivities(Convert.ToInt32(Session["VwUserid"].ToString()), Convert.ToDateTime(Session["VwActivitydate"].ToString()), Convert.ToString(Session["VwUsername"].ToString()));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ShowEditActivity(string Activityid)
    {
        try
        {
            //Page.Response.Redirect("ActivityEditView.aspx?id=" + Activityid + "&Action=Approval");
            Page.Response.Redirect("~/Activities/037-" + Activityid + "-Approval-Edit");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch { throw; }

    }
    protected void gvwListactivity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()) && e.CommandName == "EditActivity")
            {
                string activityid = e.CommandArgument.ToString();
                ShowEditActivity(activityid);

            }
        }
        catch { throw; }
    }

    private MailMessage SendRejectedActivityDetails()
    {
        string Mailcc = "";
        string MailTo = "";
        string Username = "";
        string HTMLStructure = "";
        muserauthentication = new UserAuthentication();
        this.mAppManager = muserauthentication.AppManager;
        //this.mAppManager = this.Master.AppManager;
        mUserService = AppService.Create<IUserService>();
        this.mAppManager = Session["APP_MANAGER"] as IAppManager;
        mUserService.AppManager = this.mAppManager;
        User user = new Tks.Entities.User();
        List<Activity> AC = Session["SSActivitylst"] as List<Activity>;
        HTMLStructure = gethtmlstructure(AC);
        if (Session["VwUserid"] != null)
        {
            user = mUserService.Retrieve(Convert.ToInt32(Session["VwUserid"].ToString()));
            MailTo = user.EmailId.ToString();
            //Commented by saravanan on 07312012 release later
            // Username = user.LastName + " " + user.FirstName;
            Username = user.FirstName + " " + user.LastName;
        }
        Mailcc = mAppManager.LoginUser.EmailId.ToString();

        // Fetch values from Web.Config file.
        Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "/web.config");
        //Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("C:\\Developers\\Saravanan\\Projects\\Time Keeping System\\Code\\Version 1.0\\AppClient\\web.config");

        MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

        string fromAddressDisplayName = ConfigurationManager.AppSettings["Alert_Mail_From_Address_Display_Name"];

        // Mail addresses.
        MailAddress fromAddress = new MailAddress(mailSettings.Smtp.From, fromAddressDisplayName);
        MailAddress MailToAddress = new MailAddress(MailTo);
        MailAddress MailccAddress = new MailAddress(Mailcc);

        TextReader reader = new StreamReader(Page.MapPath("~") + "/Data/RejectActivityMailContent.xml");
        string content = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();

        string messageContent = string.Format(
                content,
                 Username, HTMLStructure, "Rejected Reason : " + txtcomment.Text.Trim());

        // Mail message.
        MailMessage message = new MailMessage(fromAddress, MailToAddress);
        message.CC.Add(MailccAddress);
        message.Subject = "Tick Tock : Rejected Activity Details";
        message.Body = messageContent;
        message.IsBodyHtml = true;

        return message;

    }

    private string gethtmlstructure(List<Activity> Activity)
    {
        string rowdata = "";
        // table definition
        StringBuilder Tabledef = new StringBuilder();
        Tabledef.Append(" <table cellpadding='1' cellspacing='1' border='1' bordercolor='#0066cc' style='table-layout: fixed; width:100%; border-collapse:collapse; border: 1px solid #0066cc; font-family: Tahoma,Verdana, Arial; font-size:12px;' >");
        // Define table header.
        string Tableheader = "";
        //Tableheader = "<tr><td>Activity Date</td><td>ClientName</td><td>ProjectName</td><td>PlatformName</td><td>TestName</td><td>BillingTypeName</td><td>WorkTypeName</td><td>LanguageName</td><td>From Date</td><td>To Date</td><td>Comment</td></tr>";
        Tableheader = "<tr><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Activity Date</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Client Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Project Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Platform Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Test Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Billing Type</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Work Type</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Language</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Duration (HH:MM)</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Comment</th></tr>";
        StringBuilder rowcollectionData = null;
        rowcollectionData = new StringBuilder();
        rowdata = "<tr><td style='height:25px;'>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td></tr>";
        foreach (Activity ac in Activity)
        {

            // sb.append(string.format(rowData, ac.Name, ac.Project));
            // Define table row and cell.
            if (ac.TypeId == 1)
            {
                Tableheader = "<tr><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Activity Date</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Client Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Project Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Platform Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Test Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Billing Type</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Work Type</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Language</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Duration (HH:MM)</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Comment</th></tr>";
                rowcollectionData.Append(string.Format(rowdata, ac.Date.ToString("MM/dd/yyyy"), ac.CustomData["ClientName"].ToString()
                                    , ac.CustomData["ProjectName"].ToString()
                                    , ac.CustomData["PlatformName"].ToString(), ac.CustomData["TestName"].ToString(), ac.CustomData["BillingTypeName"].ToString()
                                    , ac.CustomData["WorkTypeName"].ToString(), ac.CustomData["LanguageName"].ToString()
                                    , Hoursdisplay(ac.Duration)
                                    , (string.IsNullOrEmpty(ac.CustomData["Comment"].ToString())) ? "" : ac.CustomData["Comment"].ToString())).ToString();
                //, ac.StartDateTime, ac.EndDateTime
                //, ac.CustomData["Comment"].ToString()));
            }
            else
            {
                Tableheader = "<tr><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Activity Date</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>ClientName</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Project Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Platform Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Test Name</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Billing Type</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Work Type</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>TimeZone</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Duration (HH:MM)</th><th style='font-weight:bold; height:25px; background-color:#0066cc; color:#ffffff;'>Comment</th></tr>";
                rowcollectionData.Append(string.Format(rowdata, ac.Date.ToString("MM/dd/yyyy"), "", "", "", "", "", ac.CustomData["WorkTypeName"].ToString()
                                    , ac.CustomData["TimeZoneName"].ToString()//, ac.StartDateTime, ac.EndDateTime
                                    , Hoursdisplay(ac.Duration)
                                    , (string.IsNullOrEmpty(ac.CustomData["Comment"].ToString())) ? "" : ac.CustomData["Comment"].ToString())).ToString();
                //, ac.CustomData["Comment"].ToString()));
            }

            Session.Add("VwUserid", ac.CreateUserId);

        }
        Tabledef.Append(Tableheader);
        Tabledef.Append(rowcollectionData);
        Tabledef.Append("</table>");

        return Tabledef.ToString();
    }

    public string Hoursdisplay(int mins)
    {
        string Duration = "";
        int hours = (mins - mins % 60) / 60;
        if (hours <= 9)
        {
            Duration = "0" + hours.ToString();
        }
        else
        {
            Duration = hours.ToString();
        }
        if ((mins - hours * 60) <= 9)
        {
            Duration = Duration + " : " + "0" + Convert.ToString((mins - hours * 60));
        }
        else
        {
            Duration = Duration + " : " + Convert.ToString((mins - hours * 60));
        }

        return Duration;
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {

        this.alertError.Style["display"] = "none";
        this.alertError.InnerHtml = "";
        if (chkapproval.Checked == true)
        {
            BulkApproveActivity(3);
        }
        else if (ApproveActivity(3) == true)
        {
            // Compose mail message.
            MailMessage message = this.SendRejectedActivityDetails();

            // Send the mail.
            muserauthentication = new UserAuthentication();
            this.mAppManager = muserauthentication.AppManager;
            mActivityService = AppService.Create<IActivityService>();
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            mActivityService.AppManager = mAppManager;
            mActivityService.SendRejectMail(message);
            txtcomment.Text = "";
        }
    }

    private bool showApporval()
    {
        gvwselectedactivity.Columns[0].Visible = false;
        gvwselectedactivity.Columns[1].Visible = false;
        chkapproval.Visible = false;
        mActivityService = AppService.Create<IActivityService>();
        mAppManager = Session["APP_MANAGER"] as IAppManager;
        mActivityService.AppManager = mAppManager;
        DataTable dt = mActivityService.RetrieveAppovalusers(mAppManager.LoginUser.Id);
        if (dt.Rows.Count > 0)
        {
            gvwselectedactivity.Columns[0].Visible = true;

            chkapproval.Visible = false;
            return true;
        }
        return false;
    }
    protected void chkapproval_CheckedChanged(object sender, EventArgs e)
    {
        if (chkapproval.Checked == true)
        {

            gvwselectedactivity.Columns[1].Visible = true;
            gvwselectedactivity.Columns[2].Visible = false;
            for (int i = 0; i < gvwselectedactivity.Rows.Count; i++)
            {
                GridViewRow row = gvwselectedactivity.Rows[i];
                CheckBox isChecked = ((CheckBox)row.FindControl("SelectedActivity"));
                isChecked.Checked = true;
                //isChecked.Enabled = true;
            }
            divgvwListactivity.Visible = false;
            divgvwListactivityHeader.Visible = true;
            this.spnuserdtls.InnerHtml = "";
        }
        else
        {
            gvwselectedactivity.Columns[1].Visible = false;
            gvwselectedactivity.Columns[2].Visible = true;
            for (int i = 0; i < gvwselectedactivity.Rows.Count; i++)
            {
                GridViewRow row = gvwselectedactivity.Rows[i];
                CheckBox isChecked = ((CheckBox)row.FindControl("SelectedActivity"));
                isChecked.Checked = false;
            }

        }
    }
    protected void SelectedActivity_CheckedChanged(object sender, EventArgs e)
    {

        int j = 0;
        gvwselectedactivity.Columns[1].Visible = false;
        gvwselectedactivity.Columns[2].Visible = true;
        chkapproval.Checked = false;

        System.Web.UI.ICheckBoxControl ch = (CheckBox)gvwselectedactivity.HeaderRow.FindControl("checkselectall");
        ch.Checked = false;

        for (int i = 0; i < gvwselectedactivity.Rows.Count; i++)
        {
            GridViewRow row = gvwselectedactivity.Rows[i];
            bool isChecked = ((CheckBox)row.FindControl("SelectedActivity")).Checked;
            if (isChecked)
            {
                chkapproval.Checked = true;
                gvwselectedactivity.Columns[1].Visible = true;
                gvwselectedactivity.Columns[2].Visible = false;
                divgvwListactivity.Visible = false;
                divgvwListactivityHeader.Visible = true;
                this.spnuserdtls.InnerHtml = "";
                j++;
            }
        }
        if(gvwselectedactivity.Rows.Count==j)
        {
            ch.Checked = true;
        }

    }
    private List<Activity> RetrieveApprove(int Userid, DateTime Activitydate)
    {
        List<Activity> Activitylst = null;
        Activitylst = new List<Activity>();
        muserauthentication = new UserAuthentication();
        this.mAppManager = muserauthentication.AppManager;
        mActivityService = AppService.Create<IActivityService>();
        mActivityService.AppManager = mAppManager;
        // retrieve
        return Activitylst = mActivityService.Retrieve(Userid, Activitydate);

    }


    private bool BulkApproveActivity(int Approved)
    {
        try
        {

            List<Activity> Addlst = new List<Activity>();
            List<Activity> lst = new List<Activity>();

            for (int i = 0; i < gvwselectedactivity.Rows.Count; i++)
            {
                GridViewRow row = gvwselectedactivity.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("SelectedActivity")).Checked;
                if (isChecked)
                {

                    hidactivitydate = (HiddenField)row.FindControl("hidactivitydate");
                    hiduserid = (HiddenField)row.FindControl("hiduserid");
                    HiddenField hiuser = (HiddenField)row.FindControl("Hidusername");
                    Session["VwActivitydate"] = hidactivitydate.Value.ToString();
                    Session["VwUserid"] = hiduserid.Value.ToString();
                    Session["VwUsername"] = hiuser.Value.ToString();
                    string activitydate = hidactivitydate.Value.ToString();
                    string userid = hiduserid.Value.ToString();
                    lst = RetrieveApprove(Convert.ToInt32(userid), Convert.ToDateTime(activitydate));
                    Addlst.AddRange(lst);
                }
            }
            bool isselected = false;

            List<Activity> Activitylst = null;
            //User usr = null;
            Activitylst = new List<Activity>();
            if (Addlst.Count > 0)
            {
                foreach (Activity ac in Addlst)
                {

                    try
                    {
                        User usr = new User();
                        isselected = true;
                        Activity OActivity = new Tks.Entities.Activity();
                        OActivity.Id = ac.Id;
                        OActivity.Comment = txtcomment.Text.Trim();
                        OActivity.IsReviewed = true;
                        OActivity.StatusId = Approved;
                        Activitylst.Add(OActivity);
                    }
                    catch
                    {
                    }
                    //}
                }
            }
            if (isselected == true)
            {
                if (txtcomment.Text.Trim() == "" && Approved == 3)
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = ENTERCOMMENTS;
                    gvwselectedactivity.Columns[1].Visible = true;
                    gvwselectedactivity.Columns[2].Visible = false;
                    divgvwListactivity.Visible = false;
                    divgvwListactivityHeader.Visible = true;
                    txtcomment.Focus();
                    return false;
                }

                if (Approved == 2)
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = ACTAPRSUS;
                    txtcomment.Text = "";
                }
                else
                {
                    this.alertError.Style["display"] = "Block";
                    this.alertError.InnerHtml = ACTREJSUS;
                    List<Activity> Aclst = new List<Activity>();
                    for (int i = 0; i < gvwselectedactivity.Rows.Count; i++)
                    {
                        GridViewRow row = gvwselectedactivity.Rows[i];
                        bool isChecked = ((CheckBox)row.FindControl("SelectedActivity")).Checked;
                        if (isChecked)
                        {

                            hidactivitydate = (HiddenField)row.FindControl("hidactivitydate");
                            hiduserid = (HiddenField)row.FindControl("hiduserid");
                            HiddenField hiuser = (HiddenField)row.FindControl("Hidusername");
                            Session["VwActivitydate"] = hidactivitydate.Value.ToString();
                            Session["VwUserid"] = hiduserid.Value.ToString();
                            Session["VwUsername"] = hiuser.Value.ToString();
                            string activitydate = hidactivitydate.Value.ToString();
                            string userid = hiduserid.Value.ToString();
                            lst = RetrieveApprove(Convert.ToInt32(userid), Convert.ToDateTime(activitydate));
                            Session.Add("SSActivitylst", lst);

                            // Compose mail message.
                            MailMessage message = this.SendRejectedActivityDetails();

                            // Send the mail.
                            muserauthentication = new UserAuthentication();
                            this.mAppManager = muserauthentication.AppManager;
                            mActivityService = AppService.Create<IActivityService>();
                            mAppManager = Session["APP_MANAGER"] as IAppManager;
                            mActivityService.AppManager = mAppManager;
                            mActivityService.SendRejectMail(message);
                        }
                    }

                    txtcomment.Text = "";
                }

                gvwselectedactivity.DataSource = null;
                gvwselectedactivity.DataBind();
                gvwListactivity.DataSource = null;
                gvwListactivity.DataBind();
                divgvwselectedactivity.Visible = false;
                divgvwselectedactivityHeader.Visible = true;
                this.spnMsg.InnerHtml = USRACTNOTFND;
                divgvwListactivity.Visible = false;
                this.spnMessage.InnerHtml = ACTNOTFND;
                divgvwListactivityHeader.Visible = true;
                muserauthentication = new UserAuthentication();
                this.mAppManager = muserauthentication.AppManager;
                mActivityService = AppService.Create<IActivityService>();
                mAppManager = Session["APP_MANAGER"] as IAppManager;
                mActivityService.AppManager = mAppManager;
                mActivityService.Approve(Activitylst);
                parentNode = TreHierarchy.SelectedNode.Value.ToString();
                //loadActivities(Convert.ToInt32(parentNode));
                loadActivities(TreHierarchy.SelectedNode);
                loadUsersActivities(Convert.ToInt32(Session["VwUserid"].ToString()), Convert.ToDateTime(Session["VwActivitydate"].ToString()), Convert.ToString(Session["VwUsername"].ToString()));

                //this.alertError.Style["display"] = "none";
                //this.alertError.InnerHtml = "";
                return true;
            }
            else
            {


                this.alertError.Style["display"] = "Block";
                this.alertError.InnerHtml = SELECTACTIVITIES;
                return false;
            }
        }
        catch { throw; }
        finally
        {
            if (mUserService != null) mUserService.Dispose();
            mUserService = null;
        }
    }

    protected void checkselectall_Click(object sender, EventArgs e)
    {
        CheckBox chkSelect = (CheckBox)sender;
        if (gvwselectedactivity.Rows.Count > 0)
        {
            if (chkSelect.Checked == true)
            {
                for (int irow = 0; irow < gvwselectedactivity.Rows.Count; irow++)
                {
                    CheckBox chk = (CheckBox)gvwselectedactivity.Rows[irow].FindControl("SelectedActivity");
                    chk.Checked = true;
                    chkapproval.Checked = true;
                    chkapproval.Checked = true;
                    gvwselectedactivity.Columns[1].Visible = true;
                    gvwselectedactivity.Columns[2].Visible = false;
                    divgvwListactivity.Visible = false;
                    divgvwListactivityHeader.Visible = true;
                    this.spnuserdtls.InnerHtml = "";

                }
                return;
            }
            else
            {
                for (int irow = 0; irow < gvwselectedactivity.Rows.Count; irow++)
                {
                    CheckBox chk = (CheckBox)gvwselectedactivity.Rows[irow].FindControl("SelectedActivity");
                    chk.Checked = false;
                    chkapproval.Checked = false;
                    gvwselectedactivity.Columns[1].Visible = false;
                    gvwselectedactivity.Columns[2].Visible = true;
                    chkapproval.Checked = false;
                }
            }
        }
      
    }





}

