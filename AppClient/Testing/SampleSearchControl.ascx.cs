using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_SampleSearchControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        if (scriptManager == null) return;

        this.Page.ClientScript.RegisterClientScriptInclude(
            typeof(Page),
            "AutoCompleteHelperScript",
            string.Format("{0}/Scripts/AutoCompleteHelper.js", this.Request.ApplicationPath));

        string options = string.Format("{{\"Location\": \"{0}\", \"LocationId\": \"{1}\"}}", txtCityName.ClientID, hdnCityId.ClientID);
        ScriptManager.RegisterStartupScript(
                this.Page,
                typeof(Page),
                System.Guid.NewGuid().ToString(),
                string.Format("$(document).ready(function() {{ initializeLocationAutoComplete('{0}'); alert('inside ready'); }});", options),
                true);
        //if (scriptManager.IsInAsyncPostBack)
        //{
        //    ScriptManager.RegisterStartupScript(
        //        this.Page,
        //        typeof(Page),
        //        System.Guid.NewGuid().ToString(),
        //        string.Format("initializeLocationAutoComplete('{0}'); alert('load'); debugger;", options),
        //        true);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(
        //        this.Page,
        //        typeof(Page),
        //        System.Guid.NewGuid().ToString(),
        //        string.Format("$(document).ready(function() {{ initializeLocationAutoComplete('{0}'); }});", options),
        //        true);
        //}

    }
}