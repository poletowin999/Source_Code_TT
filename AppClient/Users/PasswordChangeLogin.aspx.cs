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

public partial class Users_PasswordChangeLogin : System.Web.UI.Page
{
    #region Class Variable

    IAppManager mAppManager = null;

    #endregion

    #region Public Members
    public IAppManager AppManager
    {
        get { return mAppManager; }
    }
    #endregion

    #region Internal members

    //private void DisplayValidationErrors(Exception exception)
    //{
    //    // Build message.
    //    HtmlGenericControl span = new HtmlGenericControl("span");
    //    span.InnerHtml = exception.Message;

    //    BulletedList list = new BulletedList();
    //    list.DataTextField = "Value";
    //    list.DataSource = exception.Data;
    //    list.DataBind();

    //    // Assign to control.
    //    this.divErrorPanel.Controls.Clear();
    //    this.divErrorPanel.Controls.Add(span);
    //    this.divErrorPanel.Controls.Add(list);

    //    // Show the control.
    //    this.ShowHideValidationErrorPanel(true);
    //}

    //private void DisplaySucceedMessage(string message)
    //{
    //    // Assign to control.
    //    this.divInfoPanel.InnerHtml = message;

    //    // Show the control.
    //    this.ShowHideSucceedMessage(true);
    //}

    //private void ShowHideValidationErrorPanel(bool visible)
    //{
    //    this.divErrorPanel.Visible = visible;
    //}

    //private void ShowHideSucceedMessage(bool visible)
    //{
    //    this.divInfoPanel.Visible = visible;
    //}

    //private void ClearValidationErrors()
    //{
    //    // Clear the content of control.
    //    this.divErrorPanel.InnerHtml = "";

    //    // Hide the control.
    //    this.ShowHideValidationErrorPanel(false);
    //}

    //private void ClearInfoMessage()
    //{
    //    // Clear the content of control.
    //    this.divInfoPanel.InnerHtml = "";

    //    // Hide the control.
    //    this.ShowHideSucceedMessage(false);
    //}

    private void InitializeView()
    {
        try
        {
            this.mAppManager = this.AppManager;
            this.PasswordChangeViewControl1.AppManager = this.mAppManager;
           // this.Response.Redirect("~/Homepage.aspx", false);
            // Clear and hide error and message panels.
            //this.ClearValidationErrors();
            //this.ClearInfoMessage();

            //if (Page.IsPostBack)
            //{

            //}
            //else
            //{
            //    // Not post back.
            //    // Clear the controls.
            //    this.ClearControls();
            //}
        }
        catch { throw; }
    }

    //private void ClearControls()
    //{
    //    this.txtOldPassword.Text = "";
    //    this.txtNewPassWord.Text = "";
    //    this.txtConfirmPassword.Text = "";
    //}

    //private void ChangePassword()
    //{
    //    IUserService service = null;
    //    try
    //    {
    //        // Validate the values.
    //        this.ValidateEntity();

    //        // Create the service and invoke the method.
    //        service = AppService.Create<IUserService>();
    //        service.AppManager = this.mAppManager;
    //        service.ChangePassword(this.mAppManager.LoginUser.LoginName, txtOldPassword.Text, txtNewPassWord.Text);

    //        // Display succeed message.
    //        this.DisplaySucceedMessage("Password changed successfully.");
    //    }
    //    catch (ValidationException ve)
    //    {
    //        // Display the validation error.
    //        this.DisplayValidationErrors(ve);
    //    }
    //    catch { throw; }
    //}

    //private void ValidateEntity()
    //{
    //    try
    //    {
    //        // Create exception instance.
    //        ValidationException exception = new ValidationException("Validation error(s).");

    //        if (string.IsNullOrEmpty(txtOldPassword.Text) || txtOldPassword.Text.Trim() == "")
    //            exception.Data.Add("OldPassWord", "Old password should not be empty.");

    //        if (string.IsNullOrEmpty(txtNewPassWord.Text) || txtNewPassWord.Text.Trim() == "")
    //            exception.Data.Add("NewPassWord", "New password should not be empty.");

    //        if (string.IsNullOrEmpty(txtConfirmPassword.Text) || txtConfirmPassword.Text.Trim() == "")
    //            exception.Data.Add("ConfirmPassWord", "Confirm password should not be empty.");

    //        if (!txtNewPassWord.Text.Equals(txtConfirmPassword.Text))
    //            exception.Data.Add("MISMATCH", "Confirm password is mismatch with new password.");

    //        // Throw the exception, if any.
    //        if (exception.Data.Count > 0)
    //            throw exception;
    //    }
    //    catch { throw; }
    //}


    private bool IsSessionExpire()
    {
        bool isExpired;
        try
        {
            if (Session["TKS_SESSION_ID"] == null
                   || !Session.SessionID.Equals(Session["TKS_SESSION_ID"])
                   || Session["APP_MANAGER"] == null) 
                isExpired = true;
            else
                isExpired = false;

            return isExpired;
        }
        catch { throw; }
    }


    private void OnPageInitialize()
    {
        if (this.IsSessionExpire())
        {
            // Goto expire page.
            Response.Redirect("~/Misc/App-Expire", true);
            return;
        }

        // Get IAppManager reference from session.
        mAppManager = Session["APP_MANAGER"] as IAppManager;
    }


    

    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            System.Diagnostics.Debug.Print("Master page Init event: " + DateTime.Now.ToString());

            OnPageInitialize();
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.InitializeView();
        }
        catch { throw; }
    }
    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;
        try
        {
            Exception exception = HttpContext.Current.Error;
            provider = new ErrorLogProvider();
            provider.AppManager = mAppManager;
            provider.Insert(exception);

        }
        catch { throw; }
        finally
        {
            if (provider != null) provider.Dispose();
        }
    }
    protected void btnUpdate_ServerClick(object sender, EventArgs e)
    {
        try
        {
            // this.ChangePassword();
            
        }
        catch { throw; }
    }
}