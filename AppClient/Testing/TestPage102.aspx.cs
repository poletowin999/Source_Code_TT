using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CalendarButton;

public partial class Testing_TestPage102 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
       
    
        
        
    }
    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
      
        HyperLink checkInLink = new HyperLink();
        checkInLink.Text = "Check In";
        checkInLink.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(
            CalendarLinkButton1, 
            string.Format("{{'Action': 'CheckIn', 'Date': {0}}}", e.Day.Date.ToString("MM/dd/yyyy")), 
            false);

        HyperLink checkOutLink = new HyperLink();
        checkOutLink.Text = "Check Out";
        checkOutLink.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(
            CalendarLinkButton1,
            string.Format("{{'Action': 'CheckOut', 'Date': {0}}}", e.Day.Date.ToString("MM/dd/yyyy")),
            false);

        HtmlGenericControl checkInLabel = new HtmlGenericControl("span");
        checkInLabel.InnerHtml = string.Format("Check In: {0}", DateTime.Now.ToString("HH:mm"));

        HtmlGenericControl panel = new HtmlGenericControl("div");
        panel.Controls.Add(checkInLabel);
        panel.Controls.Add(checkInLink);
        panel.Controls.Add(checkOutLink);

        e.Cell.Controls.Add(panel);
    }

    protected void CalendarLinkButton1_CalendarClick(object sender, CalendarClickEventArgs e)
    {
        
    }

    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {

    }
}