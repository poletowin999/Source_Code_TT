using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;


using Tks.Model;
using Tks.Entities;
using Tks.Services;


public partial class SearchViews_ClientSearchView : System.Web.UI.UserControl
{
    public string onSearchResultSelect { get; set; }

    public string onDialogClose { get; set; }

    public void Display()
    {
        UserAuthentication authentication = new UserAuthentication();

        IClientService service = AppService.Create<IClientService>();
        service.AppManager = authentication.AppManager;

        List<Client> clients = service.Search(
            new ClientSearchCriteria()
            {
                Name = "c"
            },0);


        this.gvwClientList.DataSource = clients;
        this.gvwClientList.DataBind();

        this.Label1.Text = "Current time: " + DateTime.Now.ToString();

        this.phdContent.Visible = true;
        this.UpdatePanel1.Update();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Label1.Text = "Current time: " + DateTime.Now.ToString();

        // Attach client side script for button.
        if (string.IsNullOrEmpty(this.onDialogClose))
            this.btnClose.Attributes.Remove("onclick");
        else
            this.btnClose.Attributes.Add("onclick", string.Format("return {0}();", this.onDialogClose));
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.Label1.Text = "Current time: " + DateTime.Now.ToString();
    }
    protected void gvwClientList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlAnchor lnkName = (HtmlAnchor)e.Row.FindControl("lnkName");

                // Get bound data item.
                Client dataItem = (Client)e.Row.DataItem;
                // Serialize data item.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string jsonDataItem = serializer.Serialize(dataItem);

                // Assign to control.
                lnkName.InnerHtml = dataItem.Name;
                if (!string.IsNullOrEmpty(this.onSearchResultSelect) || !string.IsNullOrEmpty(this.onSearchResultSelect.Trim()))
                    lnkName.Attributes.Add("onclick", string.Format("return {0}('{1}');", this.onSearchResultSelect, jsonDataItem));
            }
        }
        catch { throw; }
    }
}