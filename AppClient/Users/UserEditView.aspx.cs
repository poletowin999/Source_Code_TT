using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Users_UserEditView : System.Web.UI.Page
{


    #region class variable
    //define global variable.
    IAppManager mappmanager = null;
    StringBuilder mUserSearchViewDialogScript;
    string AlertInformation;
    string PasswordMismatch;
    string PWDlengthMismatch;
    string DatelengthMismatch;
    string Datelength;
    string JOINDATE_RANGE;
    string RELEIVEDATE_RANGE;
    string INVALIDE_MAIL;
    string PAGEHEADINGADD;
    string PAGEHEADINGEDIT;

    string BTNMODIFY;
    string BTNADD;

    #endregion



    #region Internal Members
    
    List<Control> lstControl = new List<Control>();

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        this.mappmanager = Master.AppManager;
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = mappmanager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(Master.AppManager.LoginUser.Id, "USERMODULE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var ALERT_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTINFORMATION")).FirstOrDefault();
        if (ALERT_MSG != null)
        {

            AlertInformation = Convert.ToString(ALERT_MSG.DisplayText);
            PasswordMismatch = Convert.ToString(ALERT_MSG.SupportingText1);
        }

        var ALERT_PWD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTPASSWORDLEN")).FirstOrDefault();
        if (ALERT_PWD != null)
        {

            PWDlengthMismatch = Convert.ToString(ALERT_PWD.DisplayText);
        }

        var DATE_LEN = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTINVALIDDATE")).FirstOrDefault();
        if (DATE_LEN != null)
        {

            Datelength = Convert.ToString(DATE_LEN.DisplayText);
            DatelengthMismatch = Convert.ToString(DATE_LEN.SupportingText1);
        }

        var DATE_RANGE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTDATERANGE")).FirstOrDefault();
        if (DATE_RANGE != null)
        {

            RELEIVEDATE_RANGE = Convert.ToString(DATE_RANGE.DisplayText);
            JOINDATE_RANGE = Convert.ToString(DATE_RANGE.SupportingText1);
        }

        var INVALIDEMAIL = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("INVALIDEMAIL")).FirstOrDefault();
        if (INVALIDEMAIL != null)
        {
            INVALIDE_MAIL = Convert.ToString(INVALIDEMAIL.DisplayText);
        }

        var PAGE_HEADING = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLUSERSMANAGEMENTADDEDIT")).FirstOrDefault();
        if (PAGE_HEADING != null)
        {
            PAGEHEADINGADD = Convert.ToString(PAGE_HEADING.DisplayText);
            PAGEHEADINGEDIT = Convert.ToString(PAGE_HEADING.SupportingText1);
        }

        var btnADDMOD = lblLanguagelst.Where(c => c.LabelId.Equals("btnUpdate")).FirstOrDefault();
        if (btnADDMOD != null)
        {
            BTNMODIFY = Convert.ToString(btnADDMOD.SupportingText1);
            BTNADD = Convert.ToString(btnADDMOD.DisplayText);

        }

    }  

    private List<Label> getLabels() // Add all Lables to a list
    {
        List<Label> lLabels = new List<Label>();

        foreach (Control oControl in Page.Controls)
        {
            GetAllControlsInWebPage(oControl);
        }

        foreach (Control oControl in lstControl)
        {
            if (oControl.GetType() == typeof(Label))
            {
                lLabels.Add((Label)oControl);
            }
        }
        return lLabels;
    }

    private List<Button> getButtons() // Add all Lables to a list
    {
        List<Button> lLabels = new List<Button>();

        foreach (Control oControl in Page.Controls)
        {
            GetAllControlsInWebPage(oControl);
        }

        foreach (Control oControl in lstControl)
        {
            if (oControl.GetType() == typeof(Button))
            {
                lLabels.Add((Button)oControl);
            }
        }
        return lLabels;
    }

    protected void GetAllControlsInWebPage(Control oControl)
    {
        foreach (Control childControl in oControl.Controls)
        {
            lstControl.Add(childControl); //lstControl - Global variable
            if (childControl.HasControls())
                GetAllControlsInWebPage(childControl);
        }
    }


    private void ClearMessageControl(HtmlContainerControl container)
    {
        try
        {
            container.InnerText = string.Empty;
        }
        catch { throw; }
    }
    //Show the Control in Edit mode.
    private void MaKeControlVisible()
    {
        try
        {
            this.tbxReason.Visible = true;

            this.lblreason.Visible = true;

            this.chkIsactive.Visible = true;
            this.lblStatus.Visible = true;
            spncpwd.Attributes.Add("class", "normal");
            spnpwd.Attributes.Add("class", "normal");
        }
        catch { throw; }
    }
    private void FillUserType()
    {

        IUserService service = null;
        try
        {
            //create a  service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;

            //Invoke a method.
            List<UserType> UserTypeList = service.RetrieveUserType();
            drpusertype.DataTextField = "Name";
            drpusertype.DataValueField = "Id";
            drpusertype.DataSource = UserTypeList;
            drpusertype.DataBind();
            this.drpusertype.Items.Insert(0, new ListItem("-- Select --", "0"));
            if (UserTypeList.Count > 0)
                drpusertype.SelectedItem.Value = "0";

        }
        catch
        {
            throw;
        }
    }

    private void FillUserProfile()
    {
        IUsersProfile mUserProfile = null;
        try
        {
            mUserProfile = AppService.Create<IUsersProfile>();
            mUserProfile.AppManager = this.mappmanager;
            // retrieve
            List<AccessLevel> AccessLevelList = mUserProfile.SearchAccessLevel("", "Active");

            if (AccessLevelList != null)
            {
                if (AccessLevelList.Count > 0)
                {
                    ddlUserProfile.Items.Clear();
                    ddlUserProfile.DataSource = AccessLevelList;
                    ddlUserProfile.DataTextField = "AccessLevelName";
                    ddlUserProfile.DataValueField = "Id";
                    ddlUserProfile.DataBind();
                }
            }

            ddlUserProfile.Items.Insert(0, new ListItem("-- Select --", "0"));
        }
        catch
        {
            throw;
        }
    }

    //private void FillUserShift()
    //{
    //    IUserService service = null;
    //    try
    //    {
    //        //create a service.
    //        service = AppService.Create<IUserService>();
    //        service.AppManager = this.mappmanager;

    //        //Invoke the method.
    //        List<UserShift> LstShift = service.RetrieveUserShift();
    //        this.ddlShift.DataTextField = "Name";
    //        this.ddlShift.DataValueField = "Id";
    //        this.ddlShift.DataSource = LstShift;
    //        this.ddlShift.DataBind();

    //        //add default item.
    //        this.ddlShift.Items.Insert(0, new ListItem("--Select--", "0"));

    //        //select the default item
    //        if (this.ddlShift.Items.Count > 0) this.ddlShift.Items[0].Selected = true;


    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //    finally
    //    {
    //        if (service != null) { service.Dispose(); }
    //    }
    //}

    //reterive from entity.
    private void Reterive(int id)
    {
        IUserService service = null;
        try
        {
            //create a service.
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;

            //Invoke the reterive  method.
            User user = service.Retrieve(id);
            //Display the Data
            if (user != null)
                this.DispalyEntity(user);

        }
        catch { throw; }
        finally
        {
            if (service != null)
            {
                //Dispose the Service.
                service.Dispose();
            }
        }

    }
    //Display the Error Message
    private void DisplayErrorMessage(Exception exception)
    {

        //txtpasswd.Text = Utility.ConvertASCII2Stringchn(txtpasswd.Text.Trim());
        //txtComfirmPassword.Text = Utility.ConvertASCII2Stringchn(txtComfirmPassword.Text.Trim());
        txtpasswd.Text = "";
        txtComfirmPassword.Text = "";

        //Create a  stringbuilder instances.
        StringBuilder message = new StringBuilder("<ul>");
        foreach (object value in exception.Data.Values)
        {
            message.Append(string.Format("<li>{0}</li>", value.ToString()));
        }
        message.Append("</ul>");

        // Display message.
        this.divMessage.Style.Add("display", "block");
        this.divMessage.Visible = true;
        this.divMessage.InnerHtml = message.ToString();





    }
    private bool UpdateUsers(User userentity)
    {
        IUserService service = null;
        bool values = false;
        try
        {

            //validate
            if (userentity.Id == 0)
                this.ValidateUserEntity(string.Empty);
            else
                this.ValidateUserEntity("Update");
            //create a service
            service = AppService.Create<IUserService>();
            service.AppManager = this.mappmanager;
            //Invoke the update method
            service.Update(userentity);
        }
        catch (ValidationException ve)
        {
            this.DisplayErrorMessage(ve);
            values = true;

        }
        catch
        {
            values = true;
            throw;

        }
        finally
        {
            if (service != null) { service.Dispose(); }
        }
        return values;


    }
    private bool ValidateEmailID(string EmailID)
    {
        try
        {
            bool falsevalue = false;
            if (Regex.Match(EmailID, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*").Length > 0)
                falsevalue = true;

            return falsevalue;
        }
        catch { throw; }




    }
    private void ValidateUserEntity(string opt)
    {
        try
        {
            DateTime value;
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            //create a Exception Instance
            ValidationException exception = new ValidationException("validation failure");

            //validate the required elements
            if (string.IsNullOrEmpty(txtfirstname.Value) || txtfirstname.Value.Trim() == "")
                exception.Data.Add("FIRSTNAME", lblFirstName.Text + " " + AlertInformation);
            if (string.IsNullOrEmpty(txtlastname.Value) || txtlastname.Value.Trim() == "")
                exception.Data.Add("LASTNAME", lblLastName.Text + " " + AlertInformation);

            if (drpdnGender.SelectedIndex.ToString() == "0")
                exception.Data.Add("GENDER", lblGender.Text + " " + " " + AlertInformation);

            if (drpusertype.SelectedIndex.ToString() == "0")
                exception.Data.Add("USERTYPE", lblUserType.Text + " " + " " + AlertInformation);

            if (ddlUserProfile.SelectedIndex.ToString() == "0")
                exception.Data.Add("USERPROFILE", lblUserProfile.Text + " " + " " + AlertInformation);


            if (string.IsNullOrEmpty(txtRoleName.Value) || txtRoleName.Value.Trim() == "")
                exception.Data.Add("ROLE", lblRoleName.Text + " " + AlertInformation);

            //if (string.IsNullOrEmpty(txtLanguage.Value) || txtLanguage.Value.Trim() == "")
            //    exception.Data.Add("Language", "Language  is a Mandatory Field");

            //if (ddlShift.SelectedIndex.ToString() == "0")
            //    exception.Data.Add("Shift", "Shift is a Mandatory Field");

            if (string.IsNullOrEmpty(txtLoginName.Value) || txtLoginName.Value.Trim() == "")
                exception.Data.Add("LOGINNAME", lblLoginName.Text + " " + AlertInformation);
            
            if (string.IsNullOrEmpty(txtLocation.Value) || txtLocation.Value.Trim() == "")
                exception.Data.Add("LOCATION", lblLocation.Text + " " + AlertInformation);

            if (string.IsNullOrEmpty(txtDepartment.Value) || txtDepartment.Value.Trim() == "")
                exception.Data.Add("DEPARTMENT", lblDepartment.Text + " " + AlertInformation);

            if (string.IsNullOrEmpty(txtDateOfJoining.Text) || txtDateOfJoining.Text.Trim() == "")
                exception.Data.Add("DATEOFJOINING", lblDateOfJoining.Text + " " + AlertInformation);

            if ((txtConEnDt.Text.Trim().Length == 8) && (txtConEnDt.Text.IndexOf("/") == -1))
            {
                txtConEnDt.Text = txtConEnDt.Text.Insert(2, "/");
                txtConEnDt.Text = txtConEnDt.Text.Insert(5, "/");
            }

            if (txtConEnDt.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtConEnDt.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtConEnDt.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH7", Datelength + lblContractendDate.Text + DatelengthMismatch);
                }
            }

            if (drpusertype.SelectedValue == "2")
            {
                //if (string.IsNullOrEmpty(txtConEnDt.Text) || txtConEnDt.Text.Trim() == "")
                //    exception.Data.Add("CONTRACTDATE", "Contract End Date is a Mandatory Field");

                DateTime date;

                if ((txtDateOfJoining.Text != string.Empty) && (txtConEnDt.Text != string.Empty))
                {
                    if ((DateTime.TryParse(txtDateOfJoining.Text, out date)) == true && (DateTime.TryParse(txtConEnDt.Text, out date)) == true)
                    {
                        DateTime fromDate = DateTime.Parse(txtDateOfJoining.Text);
                        DateTime toDate = DateTime.Parse(txtConEnDt.Text);

                        if (DateTime.Compare(fromDate, toDate) > 0)
                        {
                            exception.Data.Add("JOINDATECONTRACTDATE", JOINDATE_RANGE);
                        }

                    }

                }
            }
            if (txtEmailId.Value.Trim() != "")
            {
                if (!this.ValidateEmailID(txtEmailId.Value))
                    exception.Data.Add("INVALID", INVALIDE_MAIL);
            }
            if (chkIsactive.Checked == false)
            {
                if (string.IsNullOrEmpty(txtDateOfRelieving.Text) || txtDateOfRelieving.Text.Trim() == "")
                    exception.Data.Add("DATEOFRELIEVING", lblDateOfRelieving.Text + " " + AlertInformation);
            }
            if (chkIsactive.Checked == true)
            {
                if (!string.IsNullOrEmpty(txtDateOfRelieving.Text) || txtDateOfRelieving.Text.Trim() != "")
                    exception.Data.Add("DATEOFRELIEVING", lblDateOfRelieving.Text + " " + AlertInformation);
            }

            if (opt == "Update")
            {
                if (string.IsNullOrEmpty(tbxReason.Value) || tbxReason.Value.Trim() == "")
                    exception.Data.Add("REASON", lblreason.Text + " " + AlertInformation);

                if (txtpasswd.Text.Trim().Equals(txtComfirmPassword.Text.Trim()) == false)
                    exception.Data.Add("MISMATCH", PasswordMismatch);
            }
            else
            {
                if (txtpasswd.Text.Trim() == "")
                    exception.Data.Add("PASSWORD", lblPassword.Text + " " + AlertInformation);

                if (!string.IsNullOrEmpty(txtpasswd.Text.Trim()))
                {
                    if (txtComfirmPassword.Text.Trim() == "")
                    {
                        exception.Data.Add("CPASSWORD", lblConfirmPassword.Text + " " + AlertInformation);
                    }
                    else
                    {
                        if (txtpasswd.Text.Trim().Equals(txtComfirmPassword.Text.Trim()) == false)
                            exception.Data.Add("MISMATCH", PasswordMismatch);
                    }
                }                
            }

            if (txtpasswd.Text.Trim() != "" && txtComfirmPassword.Text.Trim() != "")
            {
                bool isValid = false;
                var regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,}$");
                //var regex = new Regex(@"^[\p{L}0-9\s](?=.*?[0-9])(?=.*?[^\w\s]).{7,}$");
                //Match m = regex.Match(Utility.ConvertASCII2Stringchn(txtpasswd.Text));
                Match m = regex.Match(Utility.ConvertASCII2Stringchn(txtpasswd.Text));
                if (m.Success)
                {
                    var regex2 = new Regex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$", RegexOptions.IgnoreCase);
                    //var regex2 = new Regex(@"^[\p{L}0-9\s](?=.*?[0-9])(?=.*?[^\w\s]).{7,}$", RegexOptions.IgnoreCase);
                    //Match m2 = regex2.Match(Utility.ConvertASCII2Stringchn(txtComfirmPassword.Text));
                    Match m2 = regex2.Match(Utility.ConvertASCII2Stringchn(txtComfirmPassword.Text));
                    if (m2.Success)
                    {
                        isValid = true;
                    }
                }

                if (isValid == false)
                {
                    exception.Data.Add("PasswordCharacter2", PWDlengthMismatch);
                }
            }

            if ((txtDateOfJoining.Text.Trim().Length == 8) && (txtDateOfJoining.Text.IndexOf("/") == -1))
            {
                txtDateOfJoining.Text = txtDateOfJoining.Text.Insert(2, "/");
                txtDateOfJoining.Text = txtDateOfJoining.Text.Insert(5, "/");
            }

            if (txtDateOfJoining.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtDateOfJoining.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtDateOfJoining.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH1", Datelength + lblDateOfJoining.Text + DatelengthMismatch); 
                }
            }

            if (opt == "Update")
            {
               
                if ((txtDateOfRelieving.Text.Trim().Length == 8) && (txtDateOfRelieving.Text.IndexOf("/") == -1))
                {
                    txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(2, "/");
                    txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(5, "/");
                }

                if (txtDateOfRelieving.Text.Trim() != "")
                {
                    if (DateTime.TryParse(txtDateOfRelieving.Text, out value))
                    {
                        dt1 = Convert.ToDateTime(txtDateOfRelieving.Text);
                    }
                    else
                    {
                        exception.Data.Add("MISMATCH2", Datelength + lblDateOfRelieving.Text + DatelengthMismatch);
                    }
                }

                if (dt2 > dt1 && chkIsactive.Checked == false && txtDateOfRelieving.Text !="" )
                {
                    exception.Data.Add("MISMATCH3", RELEIVEDATE_RANGE);
                }
            }
        

            //throw the exceptions
            if (exception.Data.Count > 0)
            {
                throw exception;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    private bool DispalyEntity(User UserEntity)
    {
        bool resultOperation = false;
        

        if ((txtDateOfJoining.Text.Trim().Length == 8) && (txtDateOfJoining.Text.IndexOf("/") == -1))
        {
            txtDateOfJoining.Text = txtDateOfJoining.Text.Insert(2, "/");
            txtDateOfJoining.Text = txtDateOfJoining.Text.Insert(5, "/");
        }
        if ((txtDateOfRelieving.Text.Trim().Length == 8) && (txtDateOfRelieving.Text.IndexOf("/") == -1))
        {
            txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(2, "/");
            txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(5, "/");
        }

        if ((txtConEnDt.Text.Trim().Length == 8) && (txtConEnDt.Text.IndexOf("/") == -1))
        {
            txtConEnDt.Text = txtConEnDt.Text.Insert(2, "/");
            txtConEnDt.Text = txtConEnDt.Text.Insert(5, "/");
        }

        try
        {
            if (UserEntity != null)
            {

                //Assign value to the Control

                txtfirstname.Value = UserEntity.FirstName;
                txtlastname.Value = UserEntity.LastName;
                txtInital.Value = UserEntity.Initial;
                if (UserEntity.Gender.Trim() != "")
                    drpdnGender.Items[UserEntity.Gender == "M" ? 1 : 2].Selected = true;

                txtEmpID.Value = UserEntity.EmployeeId.ToString();
                txtHomePhone.Text = UserEntity.HomePhone.ToString();
                txtofficephone.Text = UserEntity.OfficePhone.ToString();
                txtExtension.Text = UserEntity.OfficePhoneExt;
                chkadmin.Checked = UserEntity.HasAdminRights;
                chkAutoapporal.Checked = UserEntity.HasAutoApproval;
                chkIsactive.Checked = UserEntity.IsActive;
                landd.Checked = UserEntity.IsLandDAdmin;
                chkAnylocation.Checked = UserEntity.IsAnyLocation;
                drpusertype.SelectedValue = UserEntity.TypeId.ToString();

               this.ddlUserProfile.SelectedValue = Convert.ToString(UserEntity.AccessLevelId);

               //if (UserEntity.AccessLevelId != 0)
               //     this.ddlUserProfile.Items.FindByValue(Convert.ToString(UserEntity.AccessLevelId)).Selected = true;           

                txtEmailId.Value = UserEntity.EmailId.ToString();
                txtRoleName.Value = UserEntity.CustomData["RoleName"].ToString();
                txtLocation.Value = UserEntity.CustomData["Location"].ToString();
                txtDepartment.Value = UserEntity.CustomData["DepartmentName"].ToString();
                hdnRoleId.Value = UserEntity.RoleId.ToString();
                hdnLocationId.Value = UserEntity.LocationId.ToString();
                hdnDepartmentId.Value = UserEntity.DepartmentId.ToString();
                txtLoginName.Value = UserEntity.LoginName.ToString();
                hdnuserid.Value = UserEntity.Id.ToString();
                HdnNoofActivities.Value = UserEntity.IsactivityApprovalPending.ToString();


                if (string.IsNullOrEmpty(UserEntity.ContractEndDate.ToString()))
                {
                    txtConEnDt.Text = "";
                }
                else
                {
                    txtConEnDt.Text = Convert.ToDateTime(UserEntity.ContractEndDate).ToString("MM/dd/yyyy");
                }
               

                if (drpusertype.SelectedValue == "2")
                {
                    lblContractendDate.EnableViewState = true;
                   // txtConEnDt.Enabled = true;
                    ImageButton1.Enabled = true;
                    ddMonths.Enabled = true;
                    lblContractMonths.EnableViewState = true;
                    if (txtConEnDt.Text == "01/01/1900")
                    {
                        txtConEnDt.Text = "";
                    }
                }
                else
                {
                    lblContractendDate.EnableViewState = false;
                    txtConEnDt.Enabled = false;
                    ImageButton1.Enabled = false;
                    ddMonths.Enabled = false;
                    lblContractMonths.EnableViewState = false;
                    txtConEnDt.Text = "";

                }

               

                txtDateOfJoining.Text = Convert.ToString(UserEntity.JoinDate);
                if (string.IsNullOrEmpty(UserEntity.JoinDate.ToString()))
                {
                    txtDateOfJoining.Text = "";
                }
                else
                {
                    txtDateOfJoining.Text = Convert.ToDateTime(UserEntity.JoinDate).ToString("MM/dd/yyyy");
                }
                if (string.IsNullOrEmpty(UserEntity.RelieveDate.ToString()))
                {
                    txtDateOfRelieving.Text = "";
                }
                else
                {
                    txtDateOfRelieving.Text = Convert.ToDateTime(UserEntity.RelieveDate).ToString("MM/dd/yyyy");
                }
                if (chkIsactive.Checked == true)
                {
                    txtDateOfRelieving.Text = "";
                }
                txtuser.Value = UserEntity.CustomData["Managername"].ToString(); 
                hdnManagerId.Value = UserEntity.ManagerId.ToString();
                //txtLanguage.Value = UserEntity.CustomData["Language"].ToString();
                //hdnLangaugeId.Value = UserEntity.LanguageId.ToString();
                //ddlShift.SelectedValue = UserEntity.ShiftId.ToString();
                //hdnShiftId.Value = UserEntity.ShiftId.ToString();
            }
            else
            {
                resultOperation = IsValiddate();
                if (resultOperation == true)
                    return resultOperation;

                    //Build a user Entity
                    UserEntity = new User(Int32.Parse(hdnuserid.Value));
                    UserEntity.FirstName = txtfirstname.Value.Trim();
                    UserEntity.LastName = txtlastname.Value.Trim();
                    UserEntity.Initial = txtInital.Value.Trim();
                    //if (txtDateOfJoining.Text == "")
                    //{ UserEntity.DateOfJoining = "01/01/1800"; }
                    //if (txtDateOfJoining.Text == "")
                    //{ }

                    if (txtConEnDt.Text != "")
                    {
                        UserEntity.ContractEndDate = Convert.ToDateTime(txtConEnDt.Text);
                    }
                    else
                    {
                        UserEntity.ContractEndDate = null;
                    }

                    if (txtDateOfJoining.Text != "")
                    {
                        UserEntity.JoinDate = Convert.ToDateTime(txtDateOfJoining.Text);
                    }
                    if (txtDateOfRelieving.Text == "")
                    {
                        UserEntity.RelieveDate = null;
                    }
                    else
                    {
                        UserEntity.RelieveDate = Convert.ToDateTime(txtDateOfRelieving.Text);
                    }

                    UserEntity.Gender = drpdnGender.Items[drpdnGender.SelectedIndex].Text == "Male" ? "M" : "F";
                    if (hdnRoleId.Value != "")
                        UserEntity.RoleId = Int32.Parse(hdnRoleId.Value);
                    UserEntity.EmailId = txtEmailId.Value;
                    UserEntity.EmployeeId = txtEmpID.Value;
                    UserEntity.TypeId = Convert.ToInt32(drpusertype.Items[drpusertype.SelectedIndex].Value);
                    UserEntity.AccessLevelId = Convert.ToInt32(ddlUserProfile.SelectedValue);
                    UserEntity.HomePhone = txtHomePhone.Text;
                    UserEntity.OfficePhone = txtofficephone.Text;
                    UserEntity.LoginName = txtLoginName.Value;
                    UserEntity.OfficePhoneExt = txtExtension.Text;

                    UserEntity.IsLandDAdmin = Convert.ToBoolean(landd.Checked == true ? 1 : 0);

                    if (hdnLocationId.Value != "")
                        UserEntity.LocationId = Int32.Parse(hdnLocationId.Value);

                    if (hdnDepartmentId.Value != "")
                        UserEntity.DepartmentId = Int32.Parse(hdnDepartmentId.Value);

                    UserEntity.HasAdminRights = Convert.ToBoolean(chkadmin.Checked == true ? 1 : 0);
                    UserEntity.HasAutoApproval = Convert.ToBoolean(chkAutoapporal.Checked == true ? 1 : 0);
                    UserEntity.IsAnyLocation = Convert.ToBoolean(chkAnylocation.Checked == true ? 1 : 0);
                    if (hdnuserid.Value != "0")
                        UserEntity.IsActive = Convert.ToBoolean(chkIsactive.Checked == true ? 1 : 0);

                    else
                        UserEntity.IsActive = true;
                    UserEntity.Reason = tbxReason.Value.Trim();
                    UserEntity.LastUpdateUserId = this.mappmanager.LoginUser.Id;
                    UserEntity.LastUpdateDate = DateTime.Now;
                    //UserEntity.Password = Utility.ConvertASCII2Stringchn(txtpasswd.Text.Trim());
                    UserEntity.Password = Utility.ConvertASCII2Stringchn(txtpasswd.Text.Trim());
                    UserEntity.LoginName = txtLoginName.Value.Trim();
                    if (hdnManagerId.Value != "")
                       UserEntity.ManagerId = int.Parse(hdnManagerId.Value);
                    //if (hdnLangaugeId.Value != "")
                    //UserEntity.LanguageId = Int32.Parse(hdnLangaugeId.Value);
                    //UserEntity.ShiftId = Convert.ToInt32(ddlShift.Items[ddlShift.SelectedIndex].Value);
                        //Convert.ToInt32(drpusertype.Items[drpusertype.SelectedIndex].Value);
                    //Update the Entity
                    //temp.InnerHtml = UserEntity.GetXml();
                    //return false;

                    resultOperation = this.UpdateUsers(UserEntity);
                }

        }

        catch { throw; }

        return resultOperation;


    }
    private void ClearInputControls(HtmlInputControl InputCtrl)
    {
        try
        {
            //clear the input controls
            InputCtrl.Value = string.Empty;
        }
        catch { throw; }
    }
    private void ClearControls()
    {
        try
        {
            //clear the control 
            this.ClearInputControls(txtfirstname);
            this.ClearInputControls(txtlastname);
            this.txtofficephone.Text = "";
            //this.ClearInputControls(txtofficephone);
            this.ClearInputControls(txtLoginName);
            this.txtDateOfRelieving.Text = "";
            this.txtDateOfJoining.Text = "";
            this.txtConEnDt.Text = "";
            this.ClearInputControls(txtRoleName);
            this.txtHomePhone.Text = "";
            //this.ClearInputControls(txtHomePhone);
            this.ClearInputControls(txtEmailId);
            this.ClearInputControls(txtEmpID);
            this.txtExtension.Text = "";
            //this.ClearInputControls(txtExtension);
            this.ClearInputControls(landd);
            this.ClearInputControls(chkadmin);
            this.ClearInputControls(chkAutoapporal);

           // Response.Redirect("USERMODULEView.aspx", false);
            Response.Redirect("~/Users/List", false);

        }
        catch { throw; }
    }
    #endregion


    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.InitializeUserSearchViewDialogScript();
            //create a instance.

            
            this.mappmanager = Master.AppManager;
            this.UserSearchView.AppManager = this.mappmanager;

            LoadLabels();

            if (hdnuserid.Value != "0")
            {
                btnUpdate.Text = BTNMODIFY;
            }
            else
            {
                btnUpdate.Text = BTNADD;
            }

            string aa;
            aa = this.mappmanager.LoginUser.HasAdminRights.ToString();

            if (aa == "False")
            {
                ErrorLogProvider provider = null;
                try
                {
                    //create a exception.
                    Exception exception = HttpContext.Current.Error;

                    //insert into Error log.
                    provider = new ErrorLogProvider();
                    provider.AppManager = this.mappmanager;
                    provider.Insert1(exception);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (provider != null) { provider.Dispose(); }
                }
//                this.DisplayMessage("YOU DO NOT HAVE ACCESS TO THIS PAGE");
                Response.Redirect("~/Homepage.aspx");

            }

            if (!Page.IsPostBack)
            {

                //focus the control.
                txtlastname.Focus();

                this.FillUserType();

                this.FillUserProfile();

                //this.FillUserShift();
                if (((object)this.Page.RouteData.Values["Id"]) != null)      
                //if (Request.QueryString["id"] != null)
                {

                    //this.Reterive(Int32.Parse(Request.QueryString["id"].ToString()));
                    this.Reterive(Int32.Parse(this.Page.RouteData.Values["Id"].ToString()));

                    //Default value.
                    this.btnUpdate.Text = BTNMODIFY;
                    this.heading.InnerText = PAGEHEADINGEDIT;

                    //Show the Controls.
                    this.MaKeControlVisible();
                    this.txtDateOfRelieving.Visible = true;
                    this.lblDateOfRelieving.Visible = true;
                    this.ImgDateOfRelieving.Visible = true;
                    this.divOtherinfo.Visible= true;


                }
                else
                {
                    //Default value while add new User.
                    this.heading.InnerText = PAGEHEADINGADD;
                    this.txtDateOfRelieving.Visible = false;
                    this.lblDateOfRelieving.Visible = false;
                    this.ImgDateOfRelieving.Visible = false;
                    this.chkIsactive.Checked = true;
                    this.divOtherinfo.Visible = false;
                    //this.txtuser.Value = "press F2 to search users";
                    this.txtuser.Value = "";
                }
            }

        }
        catch { throw; }



    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Register scripts.
            if (this.mUserSearchViewDialogScript != null)
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    System.Guid.NewGuid().ToString(),
                    this.mUserSearchViewDialogScript.ToString(),
                    true);
        }
        catch { throw; }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;
        try
        {
            //create a exception.
            Exception exception = HttpContext.Current.Error;

            //insert into Error log.
            provider = new ErrorLogProvider();
            provider.AppManager = this.mappmanager;
            provider.Insert(exception);
        }
        catch
        {
            throw;
        }
        finally
        {
            if (provider != null) { provider.Dispose(); }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        try
        {


            //Build the Entity and update data.
            if (!this.DispalyEntity(null))
            {


                if (hdnuserid.Value != "0")
                {

                    Session.Add("Userid", 0);
                    //Response.Redirect("UserListView.aspx", false);
                    Response.Redirect("~/Users/List", false);

                }
                else
                {
                    Session.Add("Userid", 1);
                    //Response.Redirect("UserListView.aspx", false);
                    Response.Redirect("~/Users/List", false);
                }

            }


        }
        catch { throw; }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            this.ClearControls();

           // Response.Redirect("UserListView.aspx", false);
            Response.Redirect("~/Users/List", false);
        }
        catch { throw; }

    }
    #endregion


    protected void txtComfirmPassword_PreRender(object sender, EventArgs e)
    {
        //if (Request.QueryString["id"] == null)
        if (((object)this.Page.RouteData.Values["Id"]) != null)      
        {

            txtComfirmPassword.Attributes.Add("value", txtComfirmPassword.Text);
        }
    }
    protected void txtpasswd_PreRender(object sender, EventArgs e)
    {
        //if (Request.QueryString["id"] == null)
        if (((object)this.Page.RouteData.Values["Id"]) != null)      
        {

            txtpasswd.Attributes.Add("value", txtpasswd.Text);
        }
    }

    private bool IsValiddate()
    {
        DateTime value;
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        bool result = false;
        //create a Exception Instance
        ValidationException exception = new ValidationException("validation failure");
        try
        {

            if ((txtDateOfJoining.Text.Trim().Length == 8) && (txtDateOfJoining.Text.IndexOf("/") == -1))
            {
                txtDateOfJoining.Text = txtDateOfJoining.Text.Insert(2, "/");
                txtDateOfJoining.Text = txtDateOfJoining.Text.Insert(5, "/");
            }

            if ((txtDateOfRelieving.Text.Trim().Length == 8) && (txtDateOfRelieving.Text.IndexOf("/") == -1))
            {
                txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(2, "/");
                txtDateOfRelieving.Text = txtDateOfRelieving.Text.Insert(5, "/");
            }

            if (txtDateOfJoining.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtDateOfJoining.Text, out value))
                {
                    dt1 = Convert.ToDateTime(txtDateOfJoining.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH4", Datelength + lblDateOfJoining.Text + DatelengthMismatch);
                    result = true;
                }
            }
            if (txtDateOfRelieving.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtDateOfRelieving.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtDateOfRelieving.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH5", Datelength + lblDateOfRelieving.Text + DatelengthMismatch);
                    result = true;
                }
            }

            if (txtConEnDt.Text.Trim() != "")
            {
                if (DateTime.TryParse(txtConEnDt.Text, out value))
                {
                    dt2 = Convert.ToDateTime(txtConEnDt.Text);
                }
                else
                {
                    exception.Data.Add("MISMATCH6", Datelength + lblContractendDate.Text + DatelengthMismatch);
                    result = true;
                }
            }

            

            if (txtpasswd.Text.Trim() != "" && txtComfirmPassword.Text.Trim() != "")
            {
                try
                {
                    bool isValid = false;
                    var regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,}$");
                    //var regex = new Regex(@"^[\p{L}0-9\s](?=.*?[0-9])(?=.*?[^\w\s]).{7,}$");
                    Match m = regex.Match(Utility.ConvertASCII2Stringchn(txtpasswd.Text));
                    if (m.Success)
                    {
                        var regex2 = new Regex("^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$", RegexOptions.IgnoreCase);
                        //var regex2 = new Regex(@"^[\p{L}0-9\s](?=.*?[0-9])(?=.*?[^\w\s]).{7,}$");
                        Match m2 = regex2.Match(Utility.ConvertASCII2Stringchn(txtComfirmPassword.Text));
                        if (m2.Success)
                        {
                            isValid = true;
                        }
                    }

                    if (isValid == false)
                    {
                        result = true;
                        exception.Data.Add("PasswordCharacter", PWDlengthMismatch);

                    }
                }
                catch (Exception e)
                {
                    exception.Data.Add("PasswordCharacter", PWDlengthMismatch);
                }
            }
            if (exception.Data.Count > 0)
            {
                this.DisplayErrorMessage(exception);
            }
            return result;
        }
        catch
        {
            return result;
        }
    }

    #region Initialize  User Search View Dialog
    private void InitializeUserSearchViewDialogScript()
    {
        try
        {
            mUserSearchViewDialogScript = new StringBuilder();
            mUserSearchViewDialogScript.Append("$(document).ready(function() { refreshUserSearchView(); });");

        }
        catch { throw; }

    }
    #endregion
    protected void ibtSearchUser_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            this.UserSearchView.ViewName = "SearchManager";
            Session.Add("UsermanagerId",hdnuserid.Value.ToString());
            this.UserSearchView.Display();
            this.mUserSearchViewDialogScript.Append("showUserSearchView();");
        }
        catch { throw; }

    }
    protected void drpusertype_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (drpusertype.SelectedValue == "2")
        {
            lblContractendDate.EnableViewState = true;
            //txtConEnDt.Enabled = true;
            ImageButton1.Enabled = true;
            ddMonths.Enabled = true;
            lblContractMonths.EnableViewState = true;
            if (txtConEnDt.Text == "01/01/1900")
            {
                txtConEnDt.Text = "";
            }
        }
        else
        {
            lblContractendDate.EnableViewState = false;
            txtConEnDt.Enabled = false;
            ImageButton1.Enabled = false;
            ddMonths.Enabled = false;
            lblContractMonths.EnableViewState = false;
            txtConEnDt.Text = "";

        }
    }
    protected void ddMonths_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime date;
        DateTime dt;
        int mnths = Convert.ToInt32(ddMonths.SelectedValue);
        if (DateTime.TryParse(txtDateOfJoining.Text, out date) == true)
        {

            dt = DateTime.Parse(txtDateOfJoining.Text);

            String.Format("{0:y yy yyy yyyy}", dt);
            txtConEnDt.Text = dt.AddMonths(mnths).ToString("MM/dd/yyyy");
        }
        else
        {
            txtDateOfJoining.Text = DateTime.Now.ToString("MM/dd/yyyy");
            dt = DateTime.Now;

            String.Format("{0:y yy yyy yyyy}", dt);
            txtConEnDt.Text = dt.AddMonths(mnths).ToString("MM/dd/yyyy");
        }
    }
}