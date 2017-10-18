using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Tks.Model;


public partial class Testing_TestPage101 : System.Web.UI.Page
{
    StringBuilder mClientSearchViewDialogScript;

    private void InitializeClientSearchViewDialogScript()
    {
        try
        {
            mClientSearchViewDialogScript = new StringBuilder();
            mClientSearchViewDialogScript.Append("refreshClientSearchView();");
        }
        catch { throw; }
    }

    #region Public members



    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // this.ClientSearchView1.Visible = false;
            this.Label1.Text = string.Format("Update Panel 1: {0}", DateTime.Now.ToString());
            this.Label2.Text = string.Format("Update Panel 2: {0}", DateTime.Now.ToString());

            // Initialize scripts.
            this.InitializeClientSearchViewDialogScript();
        }
        catch { throw; }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Build clientside scripts.

            // Register scripts.
            if (this.mClientSearchViewDialogScript != null)
                ScriptManager.RegisterStartupScript(
                    Page, 
                    typeof(Page), 
                    System.Guid.NewGuid().ToString(), 
                    this.mClientSearchViewDialogScript.ToString(), 
                    true);
        }
        catch { throw; }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.Label1.Text = string.Format("Update Panel 1: {0}", DateTime.Now.ToString());
        this.Label2.Text = string.Format("Update Panel 2: {0}", DateTime.Now.ToString());
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        this.Label1.Text = string.Format("Update Panel 1: {0}", DateTime.Now.ToString());
        this.Label2.Text = string.Format("Update Panel 2: {0}", DateTime.Now.ToString());
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        this.ClientSearchView1.Display();
        ScriptManager.RegisterStartupScript(Page, typeof(Page), this.ClientID + DateTime.Now.ToString(), "showClientSearchView();", true);
    }
    protected void ibtSearchClient_Click(object sender, ImageClickEventArgs e)
    {
        this.ClientSearchView1.Display();
        // ScriptManager.RegisterStartupScript(Page, typeof(Page), this.ClientID + DateTime.Now.ToString(), "showClientSearchView();", true);
        this.mClientSearchViewDialogScript.Append("showClientSearchView();");
    }
}