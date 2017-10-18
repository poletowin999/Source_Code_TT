using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_TestPage106 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblTime.InnerHtml = DateTime.Now.ToString();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        this.lblTime.InnerHtml = DateTime.Now.ToString();
    }
}