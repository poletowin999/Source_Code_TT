using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
using Tks.ServiceImpl;

public partial class Misc_Logoff : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Log logout info.
            // Added by saravanan on 05142012.
            //Capture the session in/out time.
            IUserService userService;
            IAppManager mAppManager;
            userService = AppService.Create<IUserService>();
            mAppManager = Session["APP_MANAGER"] as IAppManager;
            userService.AppManager = mAppManager;
            if (mAppManager != null && Session.SessionID != null)
            {
                userService.InsertUserlog(Session.SessionID, mAppManager.LoginUser.Id, "", "", "", true, true);
            }

            // Remove session.
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.Cookies.Clear();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));      

            // Remove cache.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-10));

            // Redirect to login page.
            Response.Redirect("~/Default");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch { throw; }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            Response.Cookies.Clear();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        }
        catch { throw; }
    }
}