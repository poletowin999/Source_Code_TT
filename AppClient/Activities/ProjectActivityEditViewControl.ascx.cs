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


public partial class Activities_ProjectActivityEditViewControl : System.Web.UI.UserControl
{
    #region Class members

    const string LOCATION_LIST = "ProjectActivityEditViewControl_LocationList";

    List<Location> mLocation = null;

    StringBuilder mClientScript;
    #endregion

    #region Public members

    public IAppManager AppManager { get; set; }

    public Activity Activity { get; set; }

    public void ClearActivity()
    {
        try
        {
            this.ClearClient();
            this.ClearProject();
            this.ClearLangauge();
            this.ClearWorkTypes();
            this.ClearBillingTypes();
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

            if (string.IsNullOrEmpty(hdnClientId.Value) || string.IsNullOrEmpty(hdnClientId.Value.Trim()))
                exception.Data.Add("CLIENT", lblClientName.Text + " " + SHOULDNOTBEEMPTY.Text);
            else
            {
                int temp;
                if (!Int32.TryParse(hdnClientId.Value, out temp))
                    exception.Data.Add("CLIENT", "Client is invalid.");
                else if (temp == 0)
                    exception.Data.Add("CLIENT", lblClientName.Text + " " + SHOULDNOTBEEMPTY.Text);
            }

            if (string.IsNullOrEmpty(hdnProjectId.Value) || string.IsNullOrEmpty(hdnProjectId.Value.Trim()))
                exception.Data.Add("PROJECT", lblProjectName.Text + " " + SHOULDNOTBEEMPTY.Text);
            else
            {
                int temp;
                if (!Int32.TryParse(hdnProjectId.Value, out temp))
                    exception.Data.Add("PROJECT", "Project is invalid.");
                else if (temp == 0)
                    exception.Data.Add("PROJECT", lblProjectName.Text + " " + SHOULDNOTBEEMPTY.Text);
            }

            if (string.IsNullOrEmpty(ddlLocationList.SelectedValue) || string.IsNullOrEmpty(ddlLocationList.SelectedValue.Trim()))
                exception.Data.Add("LOCATION", lblLocation.Text + " " + SHOULDNOTBEEMPTY.Text);
            else
            {
                int temp;
                if (!Int32.TryParse(ddlLocationList.SelectedValue, out temp))
                    exception.Data.Add("LOCATION", "Location is invalid.");
                else if (temp == 0)
                    exception.Data.Add("LOCATION", lblLocation.Text + " " + SHOULDNOTBEEMPTY.Text);
            }

            if (string.IsNullOrEmpty(hdnTimeZoneId.Value) || string.IsNullOrEmpty(hdnTimeZoneId.Value.Trim()))
                exception.Data.Add("TIME_ZONE", lblTimeZone.Text + " " + SHOULDNOTBEEMPTY.Text);
            else
            {
                int temp;
                if (!Int32.TryParse(hdnTimeZoneId.Value, out temp))
                    exception.Data.Add("TIME_ZONE", "Time zone is invalid.");
                else if (temp == 0)
                    exception.Data.Add("TIME_ZONE", lblTimeZone.Text + " " + SHOULDNOTBEEMPTY.Text);
            }

            if (ddlPlatformList.SelectedIndex < 1)
                exception.Data.Add("PLATFORM", lblPlatform.Text + " " + SHOULDNOTBEEMPTY.Text);

            if (RetriveUserLocation() == true)
            {
                if (ddlTestList.SelectedIndex < 1)
                    exception.Data.Add("TEST", lblTest.Text + " " + SHOULDNOTBEEMPTY.Text);
            }

            if (string.IsNullOrEmpty(hdnLangaugeId.Value) || string.IsNullOrEmpty(hdnLangaugeId.Value.Trim()))
                exception.Data.Add("LANGUAGE", lblLanguages.Text + " " + SHOULDNOTBEEMPTY.Text);
            else
            {
                int temp;
                if (!Int32.TryParse(hdnLangaugeId.Value, out temp))
                    exception.Data.Add("LANGUAGE", "Language is invalid.");
                else if (temp == 0)
                    exception.Data.Add("LANGUAGE", lblLanguages.Text + " " + SHOULDNOTBEEMPTY.Text);
            }

            if (ddlWorkTypeList.SelectedIndex < 1)
                exception.Data.Add("WORK_TYPE", lblWorktype.Text + " " + SHOULDNOTBEEMPTY.Text);

            if (ddlBillingTypeList.SelectedIndex < 1)
                exception.Data.Add("BILLING_TYPE", lblBillingType.Text + " " + SHOULDNOTBEEMPTY.Text);

            if (txthours.Text.Trim() == "")
            {
                exception.Data.Add("Hours", "Hours " + SHOULDNOTBEEMPTY.Text);
            }
            if (txtMinutes.Text.Trim() == "")
            {
                exception.Data.Add("Minutes", "Minutes" + " " + SHOULDNOTBEEMPTY.Text);
            }
            if (txthours.Text.Trim() != "")
            {
                if (IsNumeric(txthours.Text.Trim()) == true)
                {
                    if (Convert.ToInt32(txthours.Text.Trim()) > 24)
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
                if (IsNumeric(txtMinutes.Text.Trim()) == true)
                {
                    if (Convert.ToInt32(txtMinutes.Text.Trim()) > 60)
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
            if (txtMinutes.Text.Trim() != "" && txthours.Text.Trim() != "")
            {
                if (IsNumeric(txtMinutes.Text.Trim()) == true && IsNumeric(txthours.Text.Trim()) == true)
                {
                    if ((Convert.ToInt32(Convert.ToInt32(txthours.Text) * 60 + Convert.ToInt32(txtMinutes.Text)) == 0) || (Convert.ToInt32(Convert.ToInt32(txthours.Text) * 60 + Convert.ToInt32(txtMinutes.Text)) > 1440))
                    {
                        exception.Data.Add("Duration", "Enter Valid Duration");
                    }
                }
            }

            if (string.IsNullOrEmpty(this.txtActivityStartDateTime.Text) || string.IsNullOrEmpty(this.txtActivityStartDateTime.Text.Trim()))
                exception.Data.Add("START_DATE", "Start date time should not be empty.");
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(this.txtActivityStartDateTime.Text, out temp))
                    exception.Data.Add("START_DATE", "Start date time is invalid.");
            }

            if (string.IsNullOrEmpty(this.txtActivityEndDateTime.Text) || string.IsNullOrEmpty(this.txtActivityEndDateTime.Text.Trim()))
                exception.Data.Add("END_DATE", "End date time should not be empty.");
            else
            {
                DateTime temp;
                if (!DateTime.TryParse(this.txtActivityEndDateTime.Text, out temp))
                    exception.Data.Add("END_DATE", "End date time is invalid.");
            }

            // Throw exception if any.
            if (exception.Data.Count > 0) throw exception;


            // Assign values to instance.
            this.Activity.TypeId = 1;    // Project activity.
            this.Activity.ClientId = Int32.Parse(this.hdnClientId.Value);
            this.Activity.ProjectId = Int32.Parse(this.hdnProjectId.Value);
            this.Activity.LocationId = Int32.Parse(this.ddlLocationList.SelectedValue);
            this.Activity.TimeZoneId = Int32.Parse(this.hdnTimeZoneId.Value);
            this.Activity.PlatformId = Int32.Parse(this.ddlPlatformList.SelectedValue);
            this.Activity.TestId = Int32.Parse(this.ddlTestList.Value);
            this.Activity.LanguageId = Int32.Parse(this.hdnLangaugeId.Value);
            this.Activity.WorkTypeId = Int32.Parse(this.ddlWorkTypeList.SelectedValue);
            this.Activity.BillingTypeId = Int32.Parse(this.ddlBillingTypeList.Value);
            this.Activity.StartDateTime = DateTime.Parse(this.txtActivityStartDateTime.Text);
            this.Activity.EndDateTime = DateTime.Parse(this.txtActivityEndDateTime.Text);
            this.Activity.Duration = Convert.ToInt32(Convert.ToInt32(txthours.Text) * 60 + Convert.ToInt32(txtMinutes.Text));

      


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
            // Initialize.
            this.mClientScript = new StringBuilder();

            // Fetch values from session.
            this.FetchValuesFromStateManagement();

            // Not post back.
            if (!Page.IsPostBack)
            {
                // Clear all controls.
                this.ClearActivity();

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
            this.Session.Add(LOCATION_LIST, this.mLocation);
        }
        catch { throw; }
    }

    private void FetchValuesFromStateManagement()
    {
        try
        {
            // Restore values from session.
            if (Session[LOCATION_LIST] != null)
                this.mLocation = Session[LOCATION_LIST] as List<Location>;
        }
        catch { throw; }
    }

    private void FillActivity(Activity activity)
    {
        try
        {
            int id = activity.Id;

            if (id != 0)
            {
                // Fill required data of activity to edit.
                this.FillLocations(id, activity.ProjectId.Value, activity.LocationId.Value);
                this.FillPlatforms(id, activity.ProjectId.Value, activity.PlatformId.Value);
                this.FillPlatformTests(activity.ProjectId.Value, activity.PlatformId.Value,id,activity.TestId.Value);


                this.ddlWorkTypeList.SelectedValue = activity.WorkTypeId.ToString();
                this.ddlBillingTypeList.Value = (activity.BillingTypeId.HasValue) ? activity.BillingTypeId.Value.ToString() : "0";
                this.ddlTestList.Value = (activity.TestId.HasValue) ? activity.TestId.Value.ToString() : "0";
                this.ddlLocationList.SelectedValue = (activity.LocationId.HasValue) ? activity.LocationId.Value.ToString() : "0";
                this.ddlPlatformList.SelectedValue = (activity.PlatformId.HasValue) ? activity.PlatformId.Value.ToString() : "0";
                Hoursdisplay(activity.Duration);
                
            }

            this.hdnClientId.Value = (activity.ClientId.HasValue) ? activity.ClientId.Value.ToString() : "0";
            this.txtClient.Value = (id == 0) ? "" : activity.CustomData["ClientName"].ToString();

            this.hdnProjectId.Value = (activity.ProjectId.HasValue) ? activity.ProjectId.Value.ToString() : "0";
            this.txtProject.Value = (id == 0) ? "" : activity.CustomData["ProjectName"].ToString();

           
            this.hdnTimeZoneId.Value = activity.TimeZoneId.ToString();
            this.spnTimeZone.InnerHtml = (id == 0) ? "" : activity.CustomData["TimeZoneName"].ToString();

          
          

            this.hdnLangaugeId.Value = (activity.LanguageId.HasValue) ? activity.LanguageId.Value.ToString() : "0";
            this.txtLanguage.Value = (id == 0) ? "" : activity.CustomData["LanguageName"].ToString();


            if (Session["ssActivitydate"] != null && ID == "0")
            {
                this.txtActivityEndDateTime.Text = Session["ssActivitydate"].ToString();
                this.txtActivityStartDateTime.Text = Session["ssActivitydate"].ToString();
            }
            else
            {
                this.txtActivityStartDateTime.Text = (activity.StartDateTime.HasValue) ? activity.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null;
                this.txtActivityEndDateTime.Text = (activity.EndDateTime.HasValue) ? activity.EndDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null;
                
            }
            //this.spnActivityDuration.InnerHtml = (id == 0) ? "" : activity.Duration.ToString();
            if (id == 0)
            {
                LoadLanguages();
                LoadClients();
            }
           
        }

        catch { throw; }
    }

    private void ClearClient()
    {
        try
        {
            this.txtClient.Value = "";
            this.hdnClientId.Value = "0";
        }
        catch { throw; }
    }

    private void ClearProject()
    {
        try
        {
            this.txtProject.Value = "";
            this.hdnProjectId.Value = "0";

            this.ClearLocations();
            this.ClearPlatforms();
        }
        catch { throw; }
    }

    private void ClearLocations()
    {
        try
        {
            this.mLocation = null;

            // Bind.
            this.ddlLocationList.Items.Clear();

            // Add default item.
            this.ddlLocationList.Items.Insert(0, new ListItem("-- Select project to view locations --", "0"));

            // Select first item as default.
            if (this.ddlLocationList.Items.Count > 0) this.ddlLocationList.SelectedIndex = 0;

            // Clear the time zone.
            this.ClearTimeZone();
        }
        catch { throw; }
    }

    private void ClearTimeZone()
    {
        try
        {
            this.hdnTimeZoneId.Value = "0";
            this.spnTimeZone.InnerHtml = "";
        }
        catch { throw; }
    }

    private void ClearPlatforms()
    {
        try
        {
            // Bind.
            this.ddlPlatformList.Items.Clear();

            // Add default item.
            this.ddlPlatformList.Items.Insert(0, new ListItem("-- Select project to view --", "0"));

            // Select first item as default.
            if (this.ddlPlatformList.Items.Count > 0) this.ddlPlatformList.SelectedIndex = 0;

            // Clear platform test.
            this.ClearPlatformTests();
        }
        catch { throw; }
    }

    private void ClearPlatformTests()
    {
        try
        {
            this.ddlTestList.Items.Clear();

            // Add default item.
            this.ddlTestList.Items.Insert(0, new ListItem("-- Select project platform to view --", "0"));

            // Select first item as default.
            if (this.ddlTestList.Items.Count > 0) this.ddlTestList.SelectedIndex = 0;
        }
        catch { throw; }
    }

    private void ClearLangauge()
    {
        try
        {
            this.txtLanguage.Value = "";
            this.hdnLangaugeId.Value = "0";
        }
        catch { throw; }
    }

    private void ClearWorkTypes()
    {
        try
        {
            this.ddlWorkTypeList.Items.Clear();

            // Add default item.
            this.ddlWorkTypeList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlWorkTypeList.Items.Count > 0) this.ddlWorkTypeList.SelectedIndex = 0;
        }
        catch { throw; }
    }

    private void ClearBillingTypes()
    {
        try
        {
            this.ddlBillingTypeList.Items.Clear();

            // Add default item.
            this.ddlBillingTypeList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlBillingTypeList.Items.Count > 0) this.ddlBillingTypeList.SelectedIndex = 0;
        }
        catch { throw; }
    }

    private void ClearActivityPeriod()
    {
        try
        {
            this.txtActivityStartDateTime.Text = "";
            this.txtActivityEndDateTime.Text = "";
            //this.spnActivityDuration.InnerHtml = "";
        }
        catch { throw; }
    }
    private void Duration()
    {
        txthours.Text = "";
        txtMinutes.Text = "";
    }

    private void FillAllLookupData()
    {
        try
        {
            // Worktype.
            this.FillWorkTypes();

            // Billing type.
            this.FillBillingTypes();
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

    private void ChangeClient()
    {
        try
        {
            // Retrieve locations and platforms.
            if (string.IsNullOrEmpty(this.hdnClientId.Value) || string.IsNullOrEmpty(this.hdnClientId.Value.Trim()))
                this.hdnClientId.Value = "0";

            int selectedClientId = Int32.Parse(this.hdnClientId.Value);

            if (selectedClientId == 0)
            {
                this.ClearClient();
                this.ClearProject();
            }

            // Set focus on project.
            this.mClientScript.Append(
                string.Format("setTimeout(function() {{ $get('{0}').focus(); }}, 200);",
                    this.txtProject.ClientID)
                );
        }
        catch { throw; }
    }

    private void ChangeProject()
    {
        try
        {
            // Retrieve locations and platforms.
            if (string.IsNullOrEmpty(this.hdnProjectId.Value) || string.IsNullOrEmpty(this.hdnProjectId.Value.Trim()))
                this.hdnProjectId.Value = "0";

            int selectedProjectId = Int32.Parse(this.hdnProjectId.Value);

            if (selectedProjectId == 0)
            {
                this.ClearProject();
            }
            else
            {
                this.FillLocations(0, selectedProjectId, 0);
                this.FillPlatforms(0,selectedProjectId,0);
            }

            // Set focus on location.
            this.mClientScript.Append(
                string.Format("setTimeout(function() {{ $get('{0}').focus(); }}, 100);",
                    this.ddlLocationList.ClientID)
                );
        }
        catch { throw; }
    }

    private void FillLocations(int activityId, int projectId, int locationId)
    {
        ILocationService service = null;
        try
        {
            // Create the service.
            service = AppService.Create<ILocationService>();
            service.AppManager = this.AppManager;
            // Get all locations (both active and inactive).
            this.mLocation = service.RetrieveByProject(projectId);

            // Filter only active location.
            IEnumerable<Location> list = from item in mLocation
                                         where item.CustomData["IsActive"].ToString() == "True"
                                         select item;

            List<Location> projectLocations = list.ToList<Location>();

            if (activityId != 0)
            {
                // When editing the activity.
                // If current activity location is removed from project location list then 
                // we need to show that location also. Otherwise location will not populate in the drop down list.
                // Check whether current activity location is exists in the list.
                Location projectLocation = projectLocations.Where(item => item.Id == locationId).FirstOrDefault();
                // If it is not found then add to list.
                if (projectLocation == null)
                {
                    projectLocations.Add(this.mLocation.Where(item => item.Id == locationId).FirstOrDefault());
                }
            }

            // Bind.
            this.ddlLocationList.Items.Clear();

            // Iterate each location.
            ListItemCollection items = this.ddlLocationList.Items;
            //ddlLocationList.DataValueField="id";
            //ddlLocationList.DataTextField = "City";
            //ddlLocationList.DataSource = projectLocations;
            //ddlLocationList.DataBind();
            foreach (Location item in projectLocations)
            {
                if (item != null)
                {
                    items.Add(new ListItem(item.ToString(), item.Id.ToString()));
                }
             }

            // Add default item.
            this.ddlLocationList.Items.Insert(0, new ListItem("-- Select --", "0"));

            if (activityId == 0)
            {
                // Select first item as default.
                if (this.ddlLocationList.Items.Count > 0) this.ddlLocationList.SelectedIndex = 0;
                if (this.ddlLocationList.Items.Count == 2)
                {
                    this.ddlLocationList.SelectedIndex = 1;
                    ChangeLocation();
                }
            }
            else
            {
                ddlLocationList.SelectedValue = locationId.ToString();
            }
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }



    private void FillPlatforms(int activityId, int projectId, int PlatformId)
    {
        IPlatformService service = null;
        List<Platform> platforms = null;
        try
        {
            // Create the service.
            service = AppService.Create<IPlatformService>();
            service.AppManager = this.AppManager;
            platforms = new List<Platform>();
            // Retrieve platforms of the project.
             platforms = service.RetrieveByProject(projectId);

            // Filter only active location.
            IEnumerable<Platform> list = from item in platforms
                                         where item.CustomData["IsActive"].ToString() == "True"
                                         select item;

            List<Platform> platformsList = list.ToList<Platform>();

            if (activityId != 0)
            {
                // When editing the activity.
                // If current activity location is removed from project location list then 
                // we need to show that location also. Otherwise location will not populate in the drop down list.
                // Check whether current activity location is exists in the list.
                Platform platformslst = platformsList.Where(item => item.Id == PlatformId).FirstOrDefault();
                // If it is not found then add to list.
                if (platformslst == null)
                {
                    platformsList.Add(platforms.Where(item => item.Id == PlatformId).FirstOrDefault());
                }
            }

          

            // Bind.
            this.ddlPlatformList.Items.Clear();
            this.ddlPlatformList.DataTextField = "Name";
            this.ddlPlatformList.DataValueField = "Id";
            this.ddlPlatformList.DataSource = platformsList;
            this.ddlPlatformList.DataBind();

            // Add default item.
            this.ddlPlatformList.Items.Insert(0, new ListItem("-- Select --", "0"));

            if (activityId == 0)
            {
                // Select first item as default.
                if (this.ddlPlatformList.Items.Count > 0) this.ddlPlatformList.SelectedIndex = 0;
                if (this.ddlPlatformList.Items.Count == 2)
                {
                    this.ddlPlatformList.SelectedIndex = 1;
                    ChangePlatform();
                }
            }
            else
            {
                ddlPlatformList.SelectedValue = PlatformId.ToString();
            }

           
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

   

    private void ChangeLocation()
    {
        try
        {
            // Fill time zone.
            int selectedLocationId = Int32.Parse(this.ddlLocationList.SelectedValue);

            if (selectedLocationId == 0)
            {
                // Clear the time zone.
                this.ClearTimeZone();
            }
            else
            {
                // Fill time zone.
                this.FillTimeZone(selectedLocationId);
                ddlPlatformList.Focus();
            }
        }
        catch { throw; }
    }

    private void FillTimeZone(int locationId)
    {
        ITimeZoneService service = null;
        try
        {
            // Get time zone id.
            Location selectedLocation = this.mLocation.Where(item => item.Id == locationId).FirstOrDefault();
            if (selectedLocation == null)
            {
                this.hdnTimeZoneId.Value = "0";
                this.spnTimeZone.InnerHtml = "";
                return;
            }

            // Create the service.
            service = AppService.Create<ITimeZoneService>();
            service.AppManager = this.AppManager;
            Tks.Entities.TimeZone timeZone = service.Retrieve(selectedLocation.TimeZoneId);

            // Bind.
            this.hdnTimeZoneId.Value = timeZone.Id.ToString();
            this.spnTimeZone.InnerHtml = timeZone.Name;
            
        }
        catch { throw; }
    }

    private void ChangePlatform()
    {
        try
        {
            // Fill tests based on platform.
            int selectedProjectId = Int32.Parse(this.hdnProjectId.Value);

            int selectedPlatformId = 0;
            if (this.ddlPlatformList.SelectedIndex > 0)
                selectedPlatformId = Int32.Parse(this.ddlPlatformList.SelectedValue);

            if (selectedPlatformId == 0)
            {
                // Clear tests.
                this.ClearPlatformTests();
            }
            else
            {
                // Fill tests.
                this.FillPlatformTests(selectedProjectId, selectedPlatformId,0,0);
                ddlTestList.Focus();
            }

        }
        catch { throw; }
    }



    private void FillPlatformTests(int projectId, int platformId,int activityId,int testID)
    {
        ITestService service = null;
        try
        {
            // Create the service.
            service = AppService.Create<ITestService>();
            service.AppManager = this.AppManager;

            // Retrieve locations of the project.
            // TODO: Use overload method.
            List<Test> tests = service.RetrieveByProject(projectId);
            IEnumerable<Test> result = from test in tests
                                       where test.CustomData["PlatformId"].ToString() == platformId.ToString() && test.CustomData["IsActive"].ToString()=="True"
                                       select test;


            List<Test> platformsTestList = result.ToList<Test>();
            if (activityId != 0)
            {
                // When editing the activity.
                // If current activity location is removed from project location list then 
                // we need to show that location also. Otherwise location will not populate in the drop down list.
                // Check whether current activity location is exists in the list.
                Test platformslst = platformsTestList.Where(item => item.Id==testID).FirstOrDefault();
                // If it is not found then add to list.
                if (platformslst == null)
                {
                    platformsTestList.Add(tests.Where(item => item.Id == testID).FirstOrDefault());
                    platformsTestList.Remove(tests.Where(item => item.Id == 0).FirstOrDefault());
                }
            }
            
            
            // Bind.
            this.ddlTestList.Items.Clear();
            this.ddlTestList.DataTextField = "Name";
            this.ddlTestList.DataValueField = "Id";
            this.ddlTestList.DataSource = platformsTestList;
            this.ddlTestList.DataBind();

            // Add default item.
            this.ddlTestList.Items.Insert(0, new ListItem("-- Select --", "0"));


            if (activityId == 0)
            {

                // Select first item as default.
                if (this.ddlTestList.Items.Count > 0) this.ddlTestList.SelectedIndex = 0;
                if (this.ddlTestList.Items.Count == 2) this.ddlTestList.SelectedIndex = 1;
            }
            else
            {
                ddlTestList.Value = testID.ToString();
            }
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private void ChangeLanguage()
    {
        // TODO: Is this function required?
        try
        {
            // Retrieve locations and platforms.
            if (string.IsNullOrEmpty(this.hdnLangaugeId.Value) || string.IsNullOrEmpty(this.hdnLangaugeId.Value.Trim()))
                this.hdnLangaugeId.Value = "0";

            int selectedLanguageId = Int32.Parse(this.hdnLangaugeId.Value);

            if (selectedLanguageId == 0)
            {
                this.ClearLangauge();
            }

            // Set focus on project.
            txtLanguage.Focus();
        }
        catch { throw; }
    }

    private void FillBillingTypes()
    {
        IBillingTypeService service = null;
        try
        {
            // Create service.
            service = AppService.Create<IBillingTypeService>();
            service.AppManager = this.AppManager;
            // Call service method.
            List<BillingType> billingTypes = service.RetrieveAll();

            // Filter active entities.
            IEnumerable<BillingType> activeList = from item in billingTypes
                                                  where item.IsActive == true
                                                  orderby item.Name
                                                  select item;

            // Bind.
            this.ddlBillingTypeList.Items.Clear();
            this.ddlBillingTypeList.DataTextField = "Name";
            this.ddlBillingTypeList.DataValueField = "Id";
            this.ddlBillingTypeList.DataSource = activeList;
            this.ddlBillingTypeList.DataBind();

            // Add default item.
            this.ddlBillingTypeList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlBillingTypeList.Items.Count > 0) this.ddlBillingTypeList.SelectedIndex = 0;
            if (this.ddlBillingTypeList.Items.Count == 2) this.ddlBillingTypeList.SelectedIndex = 1;
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

            // Filter active entities.
            IEnumerable<WorkType> activeList = from item in workTypes
                                               where item.IsActive == true && item.ActivityTypeId == 1
                                               orderby item.Name
                                               select item;

            // Bind.
            Session.Add("ActivityEntry_Worktype", workTypes);
            this.ddlWorkTypeList.Items.Clear();
            this.ddlWorkTypeList.DataTextField = "Name";
            this.ddlWorkTypeList.DataValueField = "Id";
            this.ddlWorkTypeList.DataSource = activeList;
            this.ddlWorkTypeList.DataBind();

            // Add default item.
            this.ddlWorkTypeList.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.ddlWorkTypeList.Items.Count > 0) this.ddlWorkTypeList.SelectedIndex = 0;
            if (this.ddlWorkTypeList.Items.Count == 2)
            {
                this.ddlWorkTypeList.SelectedIndex = 1;
                SetDefaultmins();
            }
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }

    private string BuildClientScripts()
    {
        StringBuilder script;
        try
        {
            script = new StringBuilder();
            script.Append("$(document).ready(function() {");          

            // Client auto complete.
            script.Append(string.Format("initializeClientAutoComplete('{0}');",
                string.Format("{{\"Client\": \"{0}\", \"ClientId\": \"{1}\", \"SelectClient\": \"{2}\",\"UserId\": \"{3}\"}}",
                    this.txtClient.ClientID,
                    this.hdnClientId.ClientID,
                    this.btnSelectClient.ClientID,
                    this.AppManager.LoginUser.Id)
                ));

            // Project auto complete.
            script.Append(string.Format("initializeProjectAutoComplete('{0}');",
                string.Format("{{\"Project\": \"{0}\", \"ProjectId\": \"{1}\", \"ClientId\": \"{2}\", \"SelectProject\": \"{3}\",\"UserId\": \"{4}\"}}",
                    this.txtProject.ClientID,
                    this.hdnProjectId.ClientID,
                    this.hdnClientId.ClientID,
                    this.btnSelectProject.ClientID,
                    this.AppManager.LoginUser.Id)
                ));

            // Language auto complete.
            script.Append(string.Format("initializeLanguageAutoComplete('{0}');",
                string.Format("{{\"Language\": \"{0}\", \"LanguageId\": \"{1}\"}}",
                    this.txtLanguage.ClientID,
                    this.hdnLangaugeId.ClientID)
                ));

            script.Append(" });");

            return script.ToString();
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
            System.Diagnostics.Debug.Print("User Control Load event.");
            txtActivityEndDateTime.Attributes.Add("onleave", "this.Text = 5");
            
        }
        catch { throw; }
    }
    protected void txtActivityEndDateTime_Leave(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Store values in state management.
            this.StoreValuesInStateManagement();

            // Initialize auto complete features.
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

    protected void btnSelectClient_OnServerClick(object sender, EventArgs e)
    {
        try
        {
            this.ChangeClient();
            LoadProjects(Int32.Parse(this.hdnClientId.Value));
        }
        catch { throw; }
    }

    protected void btnSelectProject_OnServerClick(object sender, EventArgs e)
    {
        try
        {
            this.ChangeProject();
        }
        catch { throw; }
    }

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ChangeLocation();
        }
        catch { throw; }
    }

    protected void ddlPlatformList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.ChangePlatform();
        }
        catch { throw; }
    }

    protected void btnSelectLanguage_OnServerClick(object sender, EventArgs e)
    {
        try
        {
            // this.ChangeLanguage();
        }
        catch { throw; }
    }

    public void LoadDates(DateTime date)
    {
        string d = "";
        d = date.ToString("MM/dd/yyyy hh:mm tt");
        this.txtActivityEndDateTime.Text = "";
        this.txtActivityStartDateTime.Text = "";
       
    }
    protected void txtActivityEndDateTime_TextChanged(object sender, EventArgs e)
        {
        DateTime value;
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        System.TimeSpan  tm = new TimeSpan();
        int i = 0;
        try
        {
            if (DateTime.TryParse(txtActivityStartDateTime.Text, out value))
            {
                dt1 = Convert.ToDateTime(txtActivityStartDateTime.Text);
            }
            else
            {
                i++;
            }
            if (DateTime.TryParse(txtActivityEndDateTime.Text, out value))
            {
                dt2 = Convert.ToDateTime(txtActivityEndDateTime.Text);
            }
            else
            {
                    i++;
            }
            if (i == 0)
            {
                tm = dt2 - dt1;
                //spnActivityDuration.InnerHtml = tm.TotalMinutes.ToString().Replace("-","");
            }
        }
        catch { throw; }
    }

    private void LoadClients()
    {

        IClientService service = AppService.Create<IClientService>();
        service.AppManager = this.AppManager; 

        List<Client> clients = service.Search(
            new ClientSearchCriteria()
            {
                Name = ""
            }, this.AppManager.LoginUser.Id);

        if (clients != null)
        {

            if (clients.Count == 1)
            {
                foreach (Client cln in clients)
                {

                    this.hdnClientId.Value = cln.Id.ToString();
                    this.txtClient.Value = cln.Name.ToString();
                }
                LoadProjects(Convert.ToInt32(hdnClientId.Value.ToString()));

            }
        }

    }
    private void LoadLanguages()
    {

        ILanguageService service = AppService.Create<ILanguageService>();
        service.AppManager = this.AppManager;

        List<Language> Language = service.Search(
            new ClientSearchCriteria()
            {
                Name = ""
            });
        if (Language.Count == 1)
        {
            foreach (Language lan in Language)
            {

                this.hdnLangaugeId.Value = lan.Id.ToString();
                this.txtLanguage.Value = lan.Name.ToString();
            }

        }

    }
    private void LoadProjects(int selectedClientId)
    {

        IProjectService service = AppService.Create<IProjectService>();
        service.AppManager = this.AppManager;
        int selectedProjectId = 0;
        List<Project> projects = service.RetrieveByClient(selectedClientId, this.AppManager.LoginUser.Id);
          
        if (projects.Count == 1)
        {
            foreach (Project prj in projects)
            {

                this.hdnProjectId.Value = prj.Id.ToString();
                this.txtProject.Value = prj.Name.ToString();
                selectedProjectId = Convert.ToInt32(prj.Id.ToString());
            }
            this.FillLocations(0, selectedProjectId, 0);
            this.FillPlatforms(0, selectedProjectId, 0);
           

        }

    }
    public void  Hoursdisplay(int mins)
    {
      int hours =(mins - mins % 60) / 60;
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
    protected void ddlWorkTypeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWorkTypeList.SelectedIndex > 0)
        {
            SetDefaultmins();
            ddlBillingTypeList.Focus();
        }
    }

    private void SetDefaultmins()
    {
        //txthours.Text = "";
        //txtMinutes.Text = "";
        List<WorkType> WT = null;
        WT = new List<WorkType>();
        if (Session["ActivityEntry_Worktype"] != null)
            WT = Session["ActivityEntry_Worktype"] as List<WorkType>;
        WorkType WTlst = WT.Where(item => item.Name == ddlWorkTypeList.SelectedItem.Text).FirstOrDefault();
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

    private bool RetriveUserLocation()
    {
        IUserService IU = null;
        IU =  AppService.Create<IUserService>();
        IU.AppManager = this.AppManager;
        User USER = new User();
        USER = IU.Retrieve(this.AppManager.LoginUser.Id);
        if (USER.CustomData["Location"].ToString()=="Glasgow")
        {
            return false;
        }
        return true;
    }
}