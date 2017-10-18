using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

using Tks.Entities;
using Tks.Model;
using Tks.Services;


public partial class Activities_MiscActivityEditViewControl : System.Web.UI.UserControl
{
    #region Class variables

    StringBuilder mClientScript;
    #endregion


    #region Public members

    public IAppManager AppManager { get; set; }

    public Activity Activity { get; set; }

    public void ClearActivity()
    {
        try
        {
            this.ClearTimeZones();
            this.ClearWorkTypes();
            this.ClearActivityPeriod();
            this.Duration();
        }
        catch { throw; }
    }

    public void DisplayActivity()
    {
        try
        {
            // Fill the given activity values.
            if (this.Activity == null)
                throw new NullReferenceException("Activity should not be null.");

            // Fill all lookup data.
            this.FillAllLookupData();


            if (this.Activity.Id != 0)
            {
                // TODO: Add inactive reference item in lookup list.
            }

            this.FillActivity(this.Activity);
        }
        catch { throw; }
    }

    public Activity RetrieveActivity()
    {
        try
        {
            // Validate data type of properties.
            ValidationException exception = new ValidationException("Validation error.");

            if (ddlMiscActivityTimeZoneList.SelectedIndex < 1)
                exception.Data.Add("TIME_ZONE", lblTimeZoneMisc.Text + " " + SHOULDNOTBEEMPTYMISC.Text);

            if (ddlMiscActivityWorkTypeList.SelectedIndex < 1)
                exception.Data.Add("WORK_TYPE", lblWorktypeMISC.Text + " " + SHOULDNOTBEEMPTYMISC.Text);

             if (txthours.Text.Trim() == "")
              {
                  exception.Data.Add("Hours", "Hour" + " " + SHOULDNOTBEEMPTYMISC.Text);
              }
            if (txtMinutes.Text.Trim() == "")
            {
                exception.Data.Add("Minutes", "Minutes" + " " + SHOULDNOTBEEMPTYMISC.Text);
            }
              if (txthours.Text.Trim() != "")
            {
                 if(IsNumeric(txthours.Text.Trim())==true)
                 {
                     if(Convert.ToInt32(txthours.Text.Trim()) > 24)
                     {
                         exception.Data.Add("Hours", "Hours should be less than or equal to 24 Hrs.");
                     }
                     if (Convert.ToInt32(txthours.Text.Trim()) < 0)
                     {
                         exception.Data.Add("Hours", "Hours should be Positive value");
                     }
                 }
                 else
                 {
                     exception.Data.Add("Hours", "Hours should be numeric and less than or equal to 24 Hrs");
                 }
             }

             if (txtMinutes.Text.Trim() != "")
            {
                 if(IsNumeric(txtMinutes.Text.Trim())==true)
                 {
                     if(Convert.ToInt32(txtMinutes.Text.Trim()) > 60 )
                     {
                         exception.Data.Add("Minutes", "Minutes should be less than or equal to 60 mins.");
                     }
                     if (Convert.ToInt32(txtMinutes.Text.Trim()) < 0)
                     {
                         exception.Data.Add("Minutes", "Minutes should be Positive value");
                     }
                    
                 }
                 else
                 {
                     exception.Data.Add("Minutes", "Minutes should be numeric and less than or equal to 60 mins");
                 }
             }
             if (txtMinutes.Text.Trim() != "" && txthours.Text.Trim()!="")
             {
                 if (IsNumeric(txtMinutes.Text.Trim()) == true && IsNumeric(txthours.Text.Trim()) == true)
                 {
                     if ((Convert.ToInt32(Convert.ToInt32(txthours.Text) * 60 + Convert.ToInt32(txtMinutes.Text)) == 0) || (Convert.ToInt32(Convert.ToInt32(txthours.Text) * 60 + Convert.ToInt32(txtMinutes.Text)) > 1440))
                     {
                         exception.Data.Add("Duration", "Enter Valid Duration");
                     }
                 }
             }

             if (hdnLocationId.Value.Trim() == "0")
             {
                 hdnLocationId.Value = "";
             } 
            if (string.IsNullOrEmpty(hdnLocationId.Value) || string.IsNullOrEmpty(hdnLocationId.Value.Trim()))
                exception.Data.Add("Location", lblLocationMISC.Text + " " + SHOULDNOTBEEMPTYMISC.Text);
            
            if (string.IsNullOrEmpty(this.txtMiscActivityStartDateTime.Text) || string.IsNullOrEmpty(this.txtMiscActivityStartDateTime.Text.Trim()))
                exception.Data.Add("START_DATE", "Start date time should not be empty.");
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(this.txtMiscActivityStartDateTime.Text, out temp))
                    exception.Data.Add("START_DATE", "Start date time is invalid.");
            }

            if (string.IsNullOrEmpty(this.txtMiscActivityEndDateTime.Text) || string.IsNullOrEmpty(this.txtMiscActivityEndDateTime.Text.Trim()))
                exception.Data.Add("END_DATE", "End date time should not be empty.");
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(this.txtMiscActivityEndDateTime.Text, out temp))
                    exception.Data.Add("END_DATE", "End date time is invalid.");
            }

            // Throw exception if any.
            if (exception.Data.Count > 0) throw exception;


            // Assign values to instance.
            this.Activity.TypeId = 2;   // Misc activity.
            this.Activity.TimeZoneId = Int32.Parse(this.ddlMiscActivityTimeZoneList.Value);
            this.Activity.WorkTypeId = Int32.Parse(this.ddlMiscActivityWorkTypeList.SelectedValue);
            this.Activity.StartDateTime = DateTime.Parse(this.txtMiscActivityStartDateTime.Text);
            this.Activity.EndDateTime = DateTime.Parse(this.txtMiscActivityEndDateTime.Text);
            this.Activity.Duration = Convert.ToInt32(Convert.ToInt32(txthours.Text) * 60 + Convert.ToInt32(txtMinutes.Text));
            this.Activity.LocationId = Int32.Parse(this.hdnLocationId.Value);

            // Return the instance.
            return this.Activity;
        }
        catch { throw; }
    }

    #endregion

    #region Internal members

    private void InitializeView()
    {
        try
        {
            mClientScript = new StringBuilder();

            // Fetch values from session.
            this.FetchValuesFromStateManagement();

            // Not post back.
            if (!Page.IsPostBack)
            {
                // Clear all controls.
                this.ClearActivity();

                // Fill all lookup data.
                this.FillAllLookupData();
            }
            // Post back.
            else
            {

            }
        }
        catch { throw; }
    }

    private void StoreValuesInStateManagement()
    {
        try
        {

        }
        catch { throw; }
    }

    private void FetchValuesFromStateManagement()
    {
        try
        {
            // Restore values from session.

        }
        catch { throw; }
    }


    private void FillAllLookupData()
    {
        try
        {
            // Time zone.
            this.FillTimeZones();

            // Worktype.
            this.FillWorkTypes();
        }
        catch { throw; }
    }

    private void FillTimeZones()
    {
        ITimeZoneService service = null;
        try
        {
            // Create the service.
            service = AppService.Create<ITimeZoneService>();
            service.AppManager = this.AppManager;
            List<Tks.Entities.TimeZone> timeZones = service.RetrieveAll();

            // Filter valid time zones.
            IEnumerable<Tks.Entities.TimeZone> validList = from item in timeZones
                                                           where item.IsActive = true
                                                           select item;

            // Bind.
            this.ddlMiscActivityTimeZoneList.Items.Clear();
            this.ddlMiscActivityTimeZoneList.DataTextField = "Name";
            this.ddlMiscActivityTimeZoneList.DataValueField = "Id";
            this.ddlMiscActivityTimeZoneList.DataSource = validList;
            this.ddlMiscActivityTimeZoneList.DataBind();

            // Add default item.
            this.ddlMiscActivityTimeZoneList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlMiscActivityTimeZoneList.Items.Count > 0) this.ddlMiscActivityTimeZoneList.SelectedIndex = 0;
            if (this.ddlMiscActivityTimeZoneList.Items.Count == 2) this.ddlMiscActivityTimeZoneList.SelectedIndex = 1;
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void FillWorkTypes()
    {
        IWorkTypeService service = null;
        try
        {
            // Create service.
            service = AppService.Create<IWorkTypeService>();
            service.AppManager = this.AppManager;
            // Call service method.
            List<WorkType> workTypes = service.RetrieveAll();
            Session.Add("ActivityEntry_MISWorktype", workTypes);

            // Filter active entities.
            IEnumerable<WorkType> activeList = from item in workTypes
                                               where item.IsActive == true && item.ActivityTypeId==2
                                               orderby item.Name
                                               select item;

            // Bind.
            this.ddlMiscActivityWorkTypeList.Items.Clear();
            this.ddlMiscActivityWorkTypeList.DataTextField = "Name";
            this.ddlMiscActivityWorkTypeList.DataValueField = "Id";
            this.ddlMiscActivityWorkTypeList.DataSource = activeList;
            this.ddlMiscActivityWorkTypeList.DataBind();

            // Add default item.
            this.ddlMiscActivityWorkTypeList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlMiscActivityWorkTypeList.Items.Count > 0) this.ddlMiscActivityWorkTypeList.SelectedIndex = 0;
            if (this.ddlMiscActivityWorkTypeList.Items.Count == 2)
            {
                this.ddlMiscActivityWorkTypeList.SelectedIndex = 1;
                SetDefaultmins();
            }
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void ClearTimeZones()
    {
        try
        {
            this.ddlMiscActivityTimeZoneList.Items.Clear();

            // Add default item.
            this.ddlMiscActivityTimeZoneList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlMiscActivityTimeZoneList.Items.Count > 0) this.ddlMiscActivityTimeZoneList.SelectedIndex = 0;
        }
        catch { throw; }
    }

    private void ClearWorkTypes()
    {
        try
        {
            this.ddlMiscActivityWorkTypeList.Items.Clear();

            // Add default item.
            this.ddlMiscActivityWorkTypeList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlMiscActivityWorkTypeList.Items.Count > 0) this.ddlMiscActivityWorkTypeList.SelectedIndex = 0;
        }
        catch { throw; }
    }

    private void ClearActivityPeriod()
    {
        try
        {
            this.txtMiscActivityStartDateTime.Text = "";
            this.txtMiscActivityEndDateTime.Text = "";
            //this.spnMiscActivityDuration.InnerHtml = "";
        }
        catch { throw; }
    }
    private void Duration()
    {
        txthours.Text = "";
        txtMinutes.Text = "";
    }

    private void FillActivity(Activity activity)
    {
        try
        {
            int id = activity.Id;

            if (id != 0)
            {
                this.ddlMiscActivityTimeZoneList.Value = activity.TimeZoneId.ToString();
                this.ddlMiscActivityWorkTypeList.SelectedValue = activity.WorkTypeId.ToString();
                Hoursdisplay(activity.Duration);
                hdnLocationId.Value = activity.LocationId.ToString();
                txtLocation.Value = activity.CustomData["City"].ToString();
            }

            this.txtMiscActivityStartDateTime.Text = (activity.StartDateTime.HasValue) ? activity.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null;
            this.txtMiscActivityEndDateTime.Text = (activity.EndDateTime.HasValue) ? activity.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null;
           // this.spnMiscActivityDuration.InnerHtml = (id == 0) ? "" : activity.Duration.ToString();
        }
        catch { throw; }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.InitializeView();
            LoadLabels();
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "ACTIVITY");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

            //this.divEmptyRow.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

    } 

   protected  void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Page script.
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                System.Guid.NewGuid().ToString(),
                this.BuildClientScripts() + this.mClientScript.ToString(),
                true);

            // Startup script.
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                System.Guid.NewGuid().ToString(),
                this.BuildClientScripts() + this.mClientScript.ToString(),
                            true);
        }
        catch { throw; }
    }
    protected void txtMiscActivityEndDateTime_TextChanged(object sender, EventArgs e)
    {
        DateTime value;
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        System.TimeSpan tm = new TimeSpan();
        int i = 0;
        try
        {
            if (DateTime.TryParse(txtMiscActivityStartDateTime.Text, out value))
            {
                dt1 = Convert.ToDateTime(txtMiscActivityStartDateTime.Text);
            }
            else
            {
                i++;
            }
            if (DateTime.TryParse(txtMiscActivityEndDateTime.Text, out value))
            {
                dt2 = Convert.ToDateTime(txtMiscActivityEndDateTime.Text);
            }
            else
            {
                i++;
            }
            if (i == 0)
            {
                tm = dt2 - dt1;
                //spnMiscActivityDuration.InnerHtml = tm.TotalMinutes.ToString().Replace("-","");
            }
        }
        catch { throw; }
    }
    public void Hoursdisplay(int mins)
    {
        int hours = (mins - mins % 60) / 60;
        if (hours <= 9)
        {
            txthours.Text = "0" + hours.ToString();
        }
        else
        {
            txthours.Text = hours.ToString();
        }
        if ((mins - hours * 60) <= 9)
        {
            txtMinutes.Text = "0" + Convert.ToString((mins - hours * 60));
        }
        else
        {
            txtMinutes.Text = Convert.ToString((mins - hours * 60));
        }
    }
    protected void ddlMiscActivityWorkTypeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMiscActivityTimeZoneList.SelectedIndex > 0)
        {
            SetDefaultmins();
            txtLocation.Focus();
        }
    }
    private void SetDefaultmins()
    {
        //txthours.Text = "";
        //txtMinutes.Text = "";
        List<WorkType> WT = null;
        WT = new List<WorkType>();
        if (Session["ActivityEntry_MISWorktype"] != null)
            WT = Session["ActivityEntry_MISWorktype"] as List<WorkType>;
        WorkType WTlst = WT.Where(item => item.Name == ddlMiscActivityWorkTypeList.SelectedItem.Text).FirstOrDefault();
        string defhours = WTlst.CustomData["DefaultHours"].ToString();
        if (defhours.Trim() != "")
        {
            Hoursdisplay(Convert.ToInt32(defhours));
        }
    }

    private bool IsNumeric(string number)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(number, @"^-?\d*[0-9]?(|.\d*[0-9]|,\d*[0-9])?$");
    }
    private string BuildClientScripts()
    {
        StringBuilder script;
        try
        {
            script = new StringBuilder();
            script.Append("$(document).ready(function() {");


            // Location auto complete.
            script.Append(string.Format("initializeLocationAutoComplete('{0}');",
                string.Format("{{\"Location\": \"{0}\", \"LocationId\": \"{1}\", \"UserId\": \"{2}\"}}",
                    this.txtLocation.ClientID,
                    this.hdnLocationId.ClientID,
                    this.AppManager.LoginUser.Id)
                ));

            script.Append(" });");

            return script.ToString();
        }
        catch { throw; }
    }

}