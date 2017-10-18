using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Tks.Model;


public partial class Testing_TestPage105 : System.Web.UI.Page
{
    IAppManager mAppManager;


    protected void Page_Load(object sender, EventArgs e)
    {
        UserAuthentication inst = new UserAuthentication();
        this.mAppManager = inst.AppManager;
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;

        try
        {
            // Current exception.
            Exception exception = HttpContext.Current.Error;

            // Insert error log.
            provider = new ErrorLogProvider();
            provider.AppManager = this.mAppManager;
            provider.Insert(exception);
        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }

    protected void btnError_Click(object sender, EventArgs e)
    {
        try
        {
            throw new Exception("My exception");
        }
        catch { throw; }
    }

}