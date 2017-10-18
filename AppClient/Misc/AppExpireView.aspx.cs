using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Misc_AppExpireView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Log logout info.


            // Remove session.
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            // Remove cache.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-10));
            Response.Redirect("~/Default", false);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch { throw; }
    }
}