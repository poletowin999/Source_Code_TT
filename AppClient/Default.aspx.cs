using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
using Tks.ServiceImpl;
using System.Security.Cryptography;

public partial class _Default : System.Web.UI.Page
{
    #region Class Variables

    IAppManager mAppManager;
    // DbConnectionProvider connectionProvider;
    UserAuthentication authenticate;
    IUserService userService = null;

    string mResetId;
    User mForgotPasswordRequestUser = null;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {


            if (this.IsPostBack)
            {
                mAppManager = Session["APP_MANAGER"] as IAppManager;

            }
            else
            {
                // Clear the session variables.
                ClearSession();
                divSuccessPanel.Visible = false;
                divMessagePanel.Visible = false;
                divMessagePanel.Style.Add("display", "none");

                // Set focus on login name.
                txtUserName.Focus();
            }

            divMessagePanel.Visible = false;

            // Create the instance
            authenticate = new UserAuthentication();

            if (mAppManager == null)
                mAppManager = authenticate.AppManager;

            //Session.Add("APP_MANAGER", mAppManager);
        }
        catch { throw; }

    }



    private void ClearSession()
    {
        try
        {
            Session.RemoveAll();
        }
        catch { throw; }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {

            // Checking the userauthentication.
            _OnLogin();
        }
        catch (Exception ex)
        { throw ex; }
        finally
        {
            Session["SesLanguageId"] = drpLanguageId.SelectedValue;
        }
    }

    #region Class Function
    private void _OnLogin()
    {

        try
        {

            ValidationException exception = new ValidationException("");    


            if (txtUserName.Text.Trim() == "")
            {
                exception.Data.Add("LOGIN_NAME", "User name should not be empty.");
                txtUserName.Focus();
            }

            if (txtPassword.Text.Trim() == "")
            {
                exception.Data.Add("LOGIN_PASSWORD", "Password field should not be empty.");
                txtPassword.Focus();
            }

            if (drpLanguageId.SelectedIndex == 0)
            {
                txtPassword.Text = null;
                exception.Data.Add("LANGUAGE", "Please select a language.");
                drpLanguageId.Focus();
            }
         

            if (exception.Data.Count > 0)
                throw exception;

            // Verify the credential.
            _AuthenticateUser(this.txtUserName.Text, this.txtPassword.Text);

//            if (mAppManager.LoginUser.Id == 905)
            if (this.mAppManager.LoginUser.IsPasswordChanged.ToString() == "False")
            {
                this.Response.Redirect("~/users/PasswordChangeLogin", false);
            }
            else
            {
                // Redirect the homepage.
                //this.Response.Redirect("~/Homepage.aspx", false);
                this.Response.Redirect("~/Home", false);                
            }

        }
        catch (ValidationException ex)
        {
            DisplayValidationMessage(ex);
        }
        catch (AuthenticationException ve)
        {
            // Display user credential error.
            StringBuilder message = new StringBuilder(string.Format("{0}.", ve.Message));
            DisplayValidationErrors(message.ToString(), "Authenticate");

            // Set focus 
            if (ve.Message.ToLower().Contains("password"))
            {
                // on password control.
                Page.ClientScript.RegisterStartupScript(
                    typeof(Page),
                    System.Guid.NewGuid().ToString(),
                    string.Format("document.getElementById('{0}').focus();", txtPassword.ClientID),
                    true);
            }
            else
            {
                // on login name control.
                Page.ClientScript.RegisterStartupScript(
                    typeof(Page),
                    System.Guid.NewGuid().ToString(),
                    string.Format("document.getElementById('{0}').focus();", txtUserName.ClientID),
                    true);
            }

            // Clear the password.
            txtPassword.Text = "";
        }
        catch { throw; }
    }

    private void DisplayValidationMessage(ValidationException errMessage)
    {
        // Build validation error message.
        StringBuilder message = new StringBuilder(string.Format("{0}<ul class='loginError'>  ", errMessage.Message));
        foreach (DictionaryEntry entry in errMessage.Data)
        {
            message.Append(string.Format("<li>{0}</li>", entry.Value));
        }
        message.Append("</ul>");

        // Display validation error.
        DisplayValidationErrors(message.ToString(), "Login");

    }


    public static class EncryptionHelper
    {
        /// <method>
        /// Encrypt text using key
        /// </method>
        public static string Encrypt(string EncryptText)
        {
            string EncryptionKey = System.Configuration.ConfigurationManager.AppSettings["EncrytionKey"];

            byte[] clearBytes = Encoding.Unicode.GetBytes(EncryptText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    EncryptText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return EncryptText;
        }

        /// <method>
        /// Decrypt text using key
        /// </method>
        public static string Decrypt(string DecryptText)
        {
            string EncryptionKey = Convert.ToString(System.Configuration.ConfigurationManager.GetSection("appSettings"));
            DecryptText = DecryptText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(DecryptText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    DecryptText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return DecryptText;
        }
    }

//    byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
    private void _AuthenticateUser(string userName, string password)
    {
        try
        {
            //userName = EncryptionHelper.Encrypt(userName);
            //password = EncryptionHelper.Encrypt(password);
          //  byte[] asciiBytes = Encoding.ASCII.GetBytes(password);
          ////  userName = asciiBytes.ToString();
          //  string encpassword="";
          //  for (int i = 0; i < asciiBytes.Length; i++)
          //  {
          //      int bytevalue = 1000 + asciiBytes[i];
          //      encpassword += Convert.ToString(bytevalue);
          //  }

          //  password = encpassword;
            //Encoding enc = Encoding.GetEncoding(1252);
           // byte[] myByte = new byte[] { 67, 97, 102, 130 }; //Café

           // string str = enc.GetString(asciiBytes);

            // Create the instance
            authenticate = new UserAuthentication();

            // Check the whether user is authenticate.
            authenticate.Authenicate(userName, password);

            mAppManager = authenticate.AppManager;

            Session.Add("TKS_SESSION_ID", Session.SessionID);
            Session.Add("APP_MANAGER", mAppManager);
           
            HttpRequest request = base.Request;
            string address = request.UserHostAddress;
            string browser = request.Browser.Capabilities[""].ToString();
       
            // user Session insert into DB
            IUserService userService;
            userService = AppService.Create<IUserService>();
            userService.AppManager = mAppManager;

           

            userService.InsertSession(Session.SessionID, mAppManager.LoginUser.Id);
            if (userService.InsertUserlog(Session.SessionID, mAppManager.LoginUser.Id, Utility.GetIpAddress(), HttpContext.Current.User.Identity.Name.ToString(), browser, false, CheckValidIP()) != "")
            {
               
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                Response.Cookies.Clear();
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));


                // Remove cache.
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-10));
                Response.Redirect("~/Misc/Authorization.aspx");
            }
        }
        catch { throw; }
    }

  



    private void DisplayValidationErrors(string message, string validation)
    {
        if (string.IsNullOrEmpty(message))
        {
            divMessagePanel.InnerText = "";
            divMessagePanel.Visible = false;
        }
        else
        {

            if (validation == "Login")
                divMessagePanel.InnerHtml = message;
            else
                divMessagePanel.InnerText = message;

            divMessagePanel.Visible = true;
            divMessagePanel.Style.Add("display", "block");
        }


    }

    private User RetrieveUserByLoginName(string loginName)
    {
        IUserService service = null;
        try
        {
            service = AppService.Create<IUserService>();
            service.AppManager = mAppManager;

            return service.RetrieveByLoginName(loginName);


            //throw new NotFiniteNumberException();
        }
        catch { throw; }
    }

    private void ValidateEntity()
    {
        try
        {
            // Create exception instance.
            ValidationException exception = new ValidationException("");

            if (string.IsNullOrEmpty(txtUserName.Text) || txtUserName.Text.Trim() == "")
                exception.Data.Add("USER_NAME", "User name should not be empty.");

            // Throw the exception, if any.
            if (exception.Data.Count > 0)
                throw exception;
        }
        catch { throw; }
    }

    private MailMessage ComposeForgotPasswordResetRequestMail()
    {
        try
        {
            // Fetch values from Web.Config file.
            Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

            string fromAddressDisplayName = ConfigurationManager.AppSettings["Alert_Mail_From_Address_Display_Name"];

            // Mail addresses.
            MailAddress fromAddress = new MailAddress(mailSettings.Smtp.From, fromAddressDisplayName);
            MailAddress toAddress = new MailAddress(this.mForgotPasswordRequestUser.EmailId);

            TextReader reader = new StreamReader(Page.MapPath("~") + "/Data/PasswordResetMailContent.xml");
            string content = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            string messageContent = string.Format(
                    content,
                    string.Format("{0} {1}", this.mForgotPasswordRequestUser.LastName, this.mForgotPasswordRequestUser.FirstName),
                    HttpContext.Current.Request.Url.GetLeftPart(
                        UriPartial.Authority) +
                        HttpRuntime.AppDomainAppVirtualPath +
                        string.Format("/Users/ResetPasswordView.aspx?id={0}", this.mResetId));

            // Mail message.
            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.Subject = "Tick Tock - Reset Password Request";
            message.Body = messageContent;
            message.IsBodyHtml = true;

            return message;
        }
        catch { throw; }
    }
    #endregion
    protected void btnClear_Click(object sender, EventArgs s)
    {
        try
        {
            txtUserName.Text = "";
            txtUserName.Enabled = true;
            txtPassword.Text = "";
            txtPassword.Enabled = true;

        }
        catch (Exception ex) { throw ex; }
    }

    protected void txtPassword_PreRender(object sender, EventArgs e)
    {
        try
        {
            // To display password text.
            txtPassword.Attributes.Add("value", txtPassword.Text);
        }
        catch { throw; }
    }

    private string ResetPasswordRequest()
    {
        try
        {
            authenticate = new UserAuthentication();
            // Create Service
            userService = AppService.Create<IUserService>();
            userService.AppManager = mAppManager;
            mAppManager = authenticate.AppManager;

            return userService.ResetPasswordRequest(txtUserName.Text);


        }
        catch { throw; }
    }


    protected void lnkForgetPassword_Click(object sender, EventArgs e)
    {
        IUserService service = null;

        try
        {
            txtPassword.Text = "";
            service = AppService.Create<IUserService>();
            service.AppManager = mAppManager;

            divMessagePanel.Visible = false;

            this.ValidateEntity();
            // Create new reset.
            this.mResetId = this.ResetPasswordRequest();

            // Get user using login name.
            mForgotPasswordRequestUser = RetrieveUserByLoginName(txtUserName.Text);

            // Compose mail message.
            MailMessage message = this.ComposeForgotPasswordResetRequestMail();

            // Send the mail.
            service.SendResetPasswordRequestMail(message);

            // Display SuccessMessage

            divMessagePanel.Visible = true;
            this.divMessagePanel.InnerText = "Password Request Send To Your Mail Successfully";
            divMessagePanel.Style.Add("display", "block");
        }
        catch (ValidationException ve)
        {
            StringBuilder message = new StringBuilder();
            foreach (string data in ve.Data.Values)
            {
                message.Append(string.Format("{0}.", data));
            }
            DisplayValidationErrors(message.ToString(), "Forgot");
        }
        catch (SmtpFailedRecipientsException ex)
        {
            StringBuilder message = new StringBuilder();
            foreach (string data in ex.Data.Values)
            {
                message.Append(string.Format("{0}.", data));
            }
            DisplayValidationErrors(message.ToString(), "Forgot");
        }

        catch { throw; }
    }

    private bool CheckValidIP()
    {
        Page.Request.AppRelativeCurrentExecutionFilePath.ToString();
        string pageUrl = "";
        pageUrl = System.Web.HttpContext.Current.Request.Url.ToString();
        string[] Requesturl = pageUrl.Split('/');
        string RequestIp = Requesturl[2].ToString();
        string Connectthrough = ConfigurationManager.AppSettings["Connectthrough"];
        string[] IPs = Connectthrough.Split('|');
        foreach (string ip in IPs)
        {
            if (ip == RequestIp)
            {
                return true;
            }
        }
        return false;
    }


}