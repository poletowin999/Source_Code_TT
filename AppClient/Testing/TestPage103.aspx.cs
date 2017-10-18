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

public partial class Testing_TestPage103 : System.Web.UI.Page
{

    private string GetMailContent()
    {
        try
        {
            TextReader reader = new StreamReader(Page.MapPath("~") + "/Data/PasswordResetMailContent.xml");
            string content = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            return content;
        }
        catch { throw; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Error(object sender, EventArgs e)
    {

    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            string fromAddressDisplayName = ConfigurationManager.AppSettings["Alert_Mail_From_Address_Display_Name"];

            // Mail addresses.
            MailAddress fromAddress = new MailAddress(mailSettings.Smtp.From, fromAddressDisplayName);
            MailAddress toAddress = new MailAddress("prasanna.j@ptw-i.com");

            string messageContent = "Test mail from TKS Mailing System on "
                    + DateTime.Now.ToString()
                    + string.Format(this.GetMailContent(), "Prakash R", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath + "/Users/ResetPasswordView.aspx");


            // Mail message.
            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.Subject = "Test Mail from TKS";
            message.Body = messageContent;
            message.IsBodyHtml = true;

            // Create to smtp client.
            SmtpClient client = new SmtpClient();
            if (mailSettings != null)
            {
                client.Host = mailSettings.Smtp.Network.Host;
                client.Port = mailSettings.Smtp.Network.Port;
                client.Credentials = new NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);
                client.EnableSsl = true;
            }
            client.Send(message);
        }
        catch { throw; }
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