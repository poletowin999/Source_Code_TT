using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


public partial class Users_ResetPasswordView : System.Web.UI.Page
{
    #region Class Variable
    IAppManager mAppManager = null;
    Tks.Entities.User mUser = null;

    string mResetId;

    #endregion
    #region Class Functions

    private bool UpdateResetPassword()
    {
        IUserService service = null;
        UserAuthentication authentication = new UserAuthentication();
        this.mAppManager = authentication.AppManager;
        try
        {
            // Validate the input values.
            if (this.ValidateEntity() == false)
                return false;

            // Create the service and invoke the method.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mAppManager;

            service.ResetPassword(mResetId, txtEmailId.Text, txtNewPassword.Text);

            return true;

            // Display succeed message.

        }
        catch (ValidationException ve)
        {
            // Display erros.
            this.DisplayValidationErrors(ve);
            return false;
        }

        catch { throw; }
    }
    private bool ValidateEntity()
    {
        try
        {
            // Create exception instance.

            ValidationException exception = new ValidationException("");
            if (string.IsNullOrEmpty(txtEmailId.Text) || txtEmailId.Text.Trim() == "")
                exception.Data.Add("EMailId", "EMailId should not be empty.");

            if (string.IsNullOrEmpty(txtNewPassword.Text) || txtNewPassword.Text.Trim() == "")
                exception.Data.Add("NewPassWord", "NewPassword should not be empty.");

            if (string.IsNullOrEmpty(txtConfirmPassword.Text) || txtConfirmPassword.Text.Trim() == "")
                exception.Data.Add("ConfirmPassWord", "ConfirmPassword should not be empty.");

            if (txtNewPassword.Text.Equals(txtConfirmPassword.Text) == false)
                exception.Data.Add("MISMATCH", "ConfirmPassword is Mismatch with NewPassword");
            // Throw the exception, if any.
            if (exception.Data.Count > 0)
                throw exception;

            return true;

        }
        catch (ValidationException ve)
        {
            // Display erros.
            this.DisplayValidationErrors(ve);
            return false;
        }

        catch { throw; }
    }
    private void ShowOrHideEditPanel(bool visible)
    {
        divEditPanel.Visible = visible;
    }
    private void ShowOrHideResetIdErrorPanel(bool visible)
    {
        divResetIdErrorPanel.Visible = visible;
    }
    private void ShowOrHideSucceedMessage(bool visible)
    {
        this.divInfoPanel.Visible = visible;
    }
    private void ShowOrHideValidationErrorPanel(bool visible)
    {
        this.divErrorPanel.Visible = visible;
    }
    private void DisplaySucceedMessage(string message)
    {
        // Assign to control.
        this.divInfoPanel.InnerHtml = message;

        // Show the control.
        this.ShowOrHideSucceedMessage(true);
    }
    private void DisplayValidationErrors(Exception exception)
    {
        

        // Build validation error message.
        StringBuilder message = new StringBuilder(string.Format("{0}<ul>", exception.Message));
        foreach (DictionaryEntry entry in exception.Data)
        {
            message.Append(string.Format("<li>{0}</li>", entry.Value));
        }
        message.Append("</ul>");

        ValidationMessage(message.ToString(), true);


    }
    private void ClearValidationErrors()
    {
        // Clear the content of control.
        this.divErrorPanel.InnerHtml = "";

        // Hide the control.
        this.ShowOrHideValidationErrorPanel(false);
    }

    private void ClearInfoMessage()
    {
        // Clear the content of control.
        this.divInfoPanel.InnerHtml = "";

        // Hide the control.
        this.ShowOrHideSucceedMessage(false);
    }
    private void ClearControls()
    {
        txtEmailId.Text = " ";
        txtNewPassword.Text = "";
        txtConfirmPassword.Text = "";
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        IUserService service = null;
        try
        {
            // Restore page values.
            //mResetId =

            // Fetch value.
            mResetId = Request.QueryString["id"];

            UserAuthentication authentication = new UserAuthentication();
            this.mAppManager = authentication.AppManager;

            if (!Page.IsPostBack)
            {

                ValidationMessage(string.Empty, false);
                SuccessMessage(string.Empty, false);

                // Not post back.
                // Check whether url contains reset id.
                if (string.IsNullOrEmpty(Page.Request.QueryString["id"]))
                {
                    // Show info panel
                    this.ShowOrHideResetIdErrorPanel(true);
                    // Hide edit panel
                    this.ShowOrHideEditPanel(false);
                    return;
                }


                // Create the service and invoke the method.
                service = AppService.Create<IUserService>();
                service.AppManager = this.mAppManager;

                // Check whether reset id is valid or not.
                if (!service.IsPasswordResetIdAvailable(mResetId))
                {


                    // Display forgot password request not found.
                    this.ShowOrHideResetIdErrorPanel(true);
                    // Hide edit panel
                    this.ShowOrHideEditPanel(false);

                    return;
                }
                else
                {
                    // Display edit panel.
                    this.ShowOrHideEditPanel(true);
                    this.ShowOrHideResetIdErrorPanel(false);
                }
                                                           
            }
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
    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Store value in hidden field.

    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {


            // Calling The Update Method.
            if (this.UpdateResetPassword() != true)
            {
                //this.ClearValidationErrors();
                // this.DisplaySucceedMessage("New Password Created Sucessfully");
                this.SuccessMessage("New Password Updated Sucessfully.", false);
                //this.divErrorPanel.Visible = false;
            }
            else
            {
                this.SuccessMessage("New Password Updated Sucessfully.", true);
                this.divErrorPanel.Visible = false;
            }
            //this.ClearControls();
        }

        catch { throw; }
    }

    protected void BtnClear_Click(object sender, EventArgs e)
    {
        this.ClearControls();
        this.ClearInfoMessage();
        this.ClearValidationErrors();
    }


    private void ValidationMessage(string message, bool enable)
    {
        if (enable == true)
            divErrorPanel.Style.Add("display", "block");
        else
            divErrorPanel.Style.Add("display", "none");

        divErrorPanel.Visible = enable;
        divErrorPanel.InnerHtml = message;
    }


    private void SuccessMessage(string message, bool enable)
    {
        if (enable == true)
            divInfoPanel.Style.Add("display", "block");
        else
            divInfoPanel.Style.Add("display", "none");

        divInfoPanel.Visible = enable;
        divInfoPanel.InnerHtml = message;
    }


}