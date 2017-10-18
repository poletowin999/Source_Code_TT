using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
using System.Text.RegularExpressions;
using System.Net.Mail;


public partial class Users_PasswordChangeViewControl : System.Web.UI.UserControl
{
    private static readonly object DialogCancelEvent = new object();
    private static readonly object DialogSucceedEvent = new object();

    #region Public members

    string ALERTPASSWORDLEN_Text;

    public IAppManager AppManager { get; set; }

    public event EventHandler DialogCancel
    {
        add { base.Events.AddHandler(DialogCancelEvent, value); }
        remove { base.Events.RemoveHandler(DialogCancelEvent, value); }
    }

    public event EventHandler DialogSucceed
    {
        add { base.Events.AddHandler(DialogSucceedEvent, value); }
        remove { base.Events.RemoveHandler(DialogSucceedEvent, value); }
    }

    protected virtual void OnDialogCancel(EventArgs e)
    {
        // Raise event.
        EventHandler handler = base.Events[DialogCancelEvent] as EventHandler;
        if (handler != null)
            handler(this, e);
    }

    protected virtual void OnDialogSucceed(EventArgs e)
    {
        // Raise event.
        EventHandler handler = base.Events[DialogSucceedEvent] as EventHandler;
        if (handler != null)
            handler(this, e);
    }

    string OldpwdAlert;
    string NewpwdAlert;
    string ConfirmpwdAlert;
    string ConfirmpwdAlertmismatch;


    #endregion

    #region Internal members

    private void DisplayValidationErrors(Exception exception)
    {
        // Build message.
        HtmlGenericControl span = new HtmlGenericControl("span");
        span.InnerHtml = exception.Message;

        BulletedList list = new BulletedList();
        list.DataTextField = "Value";
        list.DataSource = exception.Data;
        list.DataBind();

        // Assign to control.
        this.divErrorPanel.Controls.Clear();
        this.divErrorPanel.Controls.Add(span);
        this.divErrorPanel.Controls.Add(list);

        // Show the control.
        this.ShowHideValidationErrorPanel(true);
    }

    private void DisplaySucceedMessage(string message)
    {
        // Assign to control.
        this.divInfoPanel.InnerHtml = message;

        // Show the control.
        this.ShowHideSucceedMessage(true);
    }

    private void ShowHideValidationErrorPanel(bool visible)
    {
        this.divErrorPanel.Visible = visible;
    }

    private void ShowHideSucceedMessage(bool visible)
    {
        this.divInfoPanel.Visible = visible;
    }

    private void ClearValidationErrors()
    {
        // Clear the content of control.
        this.divErrorPanel.InnerHtml = "";

        // Hide the control.
        this.ShowHideValidationErrorPanel(false);
    }

    private void ClearInfoMessage()
    {
        // Clear the content of control.
        this.divInfoPanel.InnerHtml = "";

        // Hide the control.
        this.ShowHideSucceedMessage(false);
    }

    private void InitializeView()
    {
        try
        {
            // Clear and hide error and message panels.
            this.ClearValidationErrors();
            this.ClearInfoMessage();

            if (Page.IsPostBack)
            {

            }
            else
            {
                // Not post back.
                // Clear the controls.
                this.ClearControls();
            }
        }
        catch { throw; }
    }

    private void ClearControls()
    {
        this.txtOldPassword.Text = "";
        this.txtNewPassWord.Text = "";
        this.txtConfirmPassword.Text = "";
    }

    private void ChangePassword()
    {
        IUserService service = null;
        try
        {
            // Validate the values.
            this.ValidateEntity();

            // Create the service and invoke the method.
            service = AppService.Create<IUserService>();
            service.AppManager = this.AppManager;
            //            service.ChangePassword(this.AppManager.LoginUser.LoginName, Utility.ConvertASCII2String(txtOldPassword.Text), Utility.ConvertASCII2String(txtNewPassWord.Text));
            service.ChangePassword(this.AppManager.LoginUser.LoginName, Utility.ConvertASCII2Stringchn(txtOldPassword.Text), Utility.ConvertASCII2Stringchn(txtNewPassWord.Text));

            // Display succeed message.
            this.DisplaySucceedMessage("Password changed successfully.");
            btnCancel.Visible = true;

            //MailMessage mail = new MailMessage();
            //mail.From = new MailAddress("donotreply@ptw-i.com");
            //mail.IsBodyHtml = true;
            //mail.Subject = "Test";
            //mail.To.Clear();
            //mail.To.Add("Moganavel.b@Ptw-i.com");
            //mail.Body = "<tr><td>Dear,"+ this.AppManager.LoginUser.FirstName +"<br><br><b>Attention Please:</b> Your login password with the TT is changed now. If you have not changed, please contact at <b>softwaresupport@ptw-i.com.</b><br>To sign-in : http://TT.poletowininternational.com. <br><br>Thank You,<br>Software Team.<br><br></tr>";
            //SmtpClient smtp = new SmtpClient("outlook.office365.com", 587);

            //smtp.Credentials = new System.Net.NetworkCredential("donotreply@ptw-i.com", "P@ssw0rd123#"); smtp.EnableSsl = true;
            ////smtp.Credentials = new System.Net.NetworkCredential("corporatehrotwie@poletowininternamtional.com", "password123ptw"); smtp.EnableSsl = true;
            //smtp.Send(mail);

            // Raise DialogSucceed event.
            this.OnDialogSucceed(EventArgs.Empty);
        }
        catch (ValidationException ve)
        {
            // Display the validation error.
            this.DisplayValidationErrors(ve);
        }
        catch { throw; }
    }

    private void ValidateEntity()
    {
        try
        {
            // Create exception instance.
            ValidationException exception = new ValidationException(string.Empty);

            if (string.IsNullOrEmpty(txtOldPassword.Text) || txtOldPassword.Text.Trim() == "")
                exception.Data.Add("OldPassWord", "Old password should not be empty.");

            if (string.IsNullOrEmpty(txtNewPassWord.Text) || txtNewPassWord.Text.Trim() == "")
                exception.Data.Add("NewPassWord", "New password should not be empty.");

            if (string.IsNullOrEmpty(txtConfirmPassword.Text) || txtConfirmPassword.Text.Trim() == "")
                exception.Data.Add("ConfirmPassWord", "Confirm password should not be empty.");

            //            if (!Utility.ConvertASCII2String(txtNewPassWord.Text).Equals(Utility.ConvertASCII2String(txtConfirmPassword.Text)))
            if (!Utility.ConvertASCII2Stringchn(txtNewPassWord.Text).Equals(Utility.ConvertASCII2Stringchn(txtConfirmPassword.Text)))
                exception.Data.Add("MISMATCH2", "Confirm password is mismatch with new password.");

            if (txtNewPassWord.Text.Trim() != "" && txtNewPassWord.Text.Trim() != "")
            {
                bool isValid = false;

                //var regex = new Regex(@"^[\p{L}0-9\s](?=.*?[0-9])(?=.*?[^\w\s]).{7,}$");
                var regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,}$");
                Match m = regex.Match(Utility.ConvertASCII2Stringchn(txtConfirmPassword.Text));
                if (m.Success)
                {
                    //var regex2 = new Regex(@"^[\p{L}0-9\s](?=.*?[0-9])(?=.*?[^\w\s]).{7,}$", RegexOptions.IgnoreCase);
                    var regex2 = new Regex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$", RegexOptions.IgnoreCase);
                    Match m2 = regex2.Match(Utility.ConvertASCII2Stringchn(txtConfirmPassword.Text));
                    if (m2.Success)
                    {
                        isValid = true;
                    }
                }

                if (isValid == false)
                {
                    exception.Data.Add("MISMATCH", ALERTPASSWORDLEN_Text);
                }
            }
            // Throw the exception, if any.
            if (exception.Data.Count > 0)
                throw exception;
        }
        catch { throw; }
    }

    private void CancelPasswordChange()
    {
        try
        {
            // Clear controls.
            this.ClearControls();

            // Raise DialogCancel event.
            this.OnDialogCancel(EventArgs.Empty);
            //TODO:close the dialog  properly
            //Response.Redirect("../HomePage.aspx",false);
            Response.Redirect("~/Misc/Logoff", false);

        }
        catch { throw; }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            LoadLabels();
            this.InitializeView();
        }
        catch { throw; }


    }

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "CHANGEPASSWORD");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);


        var ALERT_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTPASSWORDLEN")).FirstOrDefault();
        if (ALERT_MSG != null)
        {
            ALERTPASSWORDLEN_Text = Convert.ToString(ALERT_MSG.DisplayText);
        }

        var LBLpwdAlert = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("OLDNEWPWDALERT")).FirstOrDefault();
        if (LBLpwdAlert != null)
        {
            OldpwdAlert = Convert.ToString(LBLpwdAlert.DisplayText);
            NewpwdAlert = Convert.ToString(LBLpwdAlert.SupportingText1);
        }

        var LBLConfirmpwd = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("CONFIRMPWDALERT")).FirstOrDefault();
        if (LBLConfirmpwd != null)
        {
            ConfirmpwdAlert = Convert.ToString(LBLConfirmpwd.DisplayText);
            ConfirmpwdAlertmismatch = Convert.ToString(LBLConfirmpwd.SupportingText1);
        }

    }

    protected void btnUpdate_ServerClick(object sender, EventArgs e)
    {
        try
        {
            this.ChangePassword();
        }
        catch { throw; }
    }

    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        try
        {
            this.CancelPasswordChange();
        }
        catch { throw; }
    }

}