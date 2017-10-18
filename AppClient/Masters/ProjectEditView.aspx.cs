using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using System.Data;

using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Masters_ProjectEditView : System.Web.UI.Page
{

    #region Class Variables

    IAppManager mAppManager;
    // DbConnectionProvider connectionProvider;
    UserAuthentication authenticate;
    IProjectService projectService;
    Project projectEntity = null;
    IPlatformService platformService = null;

    List<Platform> mValidPlatformList = null;
    List<Test> mValidTestList = null;
    List<Location> mValidLocationList = null;

    List<Platform> mProjectPlatformList = null;
    List<Test> mProjectTestList = null;
    List<Location> mProjectLocatonList = null;

    string mEntityEditPanelHeader = "Add Location";

    string ALERTMSG;
    string PAGEHEADINGADD;
    string PRESSF2_MSG;

    string BTNMODIFY;
    string BTNADD;

    string AddPlatformTestHeader;

    string VALIDATIONERROR;

    int ProjectId;
    StringBuilder mUserSearchViewDialogScript;

    int _id;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            // For error testing purpose.
            //Location loc = new Location();
            //mProjectLocatonList.Add(loc);

            // Initialize the view.
            Inilizethecontrols();

            this.InitializeUserSearchViewDialogScript();
            LoadLabels();
            //txtResponsibleUser.Value = "press F2 to search users";
            if (!Page.IsPostBack)
            {
               // ProjectId = Int32.Parse(Request.QueryString["ProjectId"].ToString());

                if (((object)this.Page.RouteData.Values["ProjectId"]) != null)
                {
                    ProjectId = Int32.Parse(this.Page.RouteData.Values["ProjectId"].ToString());
                }

                // For timebeeing.
                // ProjectId = 0;
                HeaderTitle.InnerText = PAGEHEADINGADD;
                txtResponsibleUser.Value = PRESSF2_MSG;
                hdnProjectId.Value = ProjectId.ToString();

                if (hdnProjectId.Value != "0")
                {
                    btnUpdate.Text = BTNMODIFY;
                }
                else
                {
                    btnUpdate.Text = BTNADD;
                }

                // Edit mode.
                if (ProjectId != 0)
                {
                   // btnUpdate.Text = "Update";
                    HeaderTitle.InnerText = PAGEHEADINGADD;
                    // Retrieve the Projects.
                    if (RetrieveProejcts(ProjectId) == false)
                    {
                        //ValidationException exception = new ValidationException("Validation error(s) occurred.");
                        ValidationException exception = new ValidationException("");
                        exception.Data.Add("ProjectValid", "Project invalid.");
                        throw exception;

                    }

                    // Retrieve the project Locations.
                    mProjectLocatonList = RetrieveLocationsByProejct(ProjectId);
                    // Display the project locations.
                    DisplayProjectloations(mProjectLocatonList);

                    // Retrieve the project platforms.
                    mProjectPlatformList = RetrievePlatformsByProject(ProjectId);

                    var plotformItem = from item in mProjectPlatformList
                                       orderby item.Name
                                       select item;

                    mProjectPlatformList = plotformItem.ToList<Platform>();

                    var platformList = from item in mProjectPlatformList
                                       orderby item.Name
                                       select item;

                    mProjectPlatformList = platformList.ToList<Platform>();


                    // Display the project platforms.
                    DisplayProjectPlatformList(mProjectPlatformList);

                    // Retrieve the project tests.
                    mProjectTestList = RetrieveTestsByProject(ProjectId);

                    var testList = from item in mProjectTestList
                                   orderby item.Name
                                   select item;

                    mProjectTestList = testList.ToList<Test>();


                    // Display test.
                    this.DisplayProjectPlatformTestList(0, mProjectTestList);

                    // Set the flag.
                    ViewState.Add("PROJECT_ADDFLAG", "False");
                    ViewState.Add("PROJECT_EDITFLAG", "True");

                    divActive.Visible = true;

                }
                // Add mode.
                else
                {

                    //btnUpdate.Text = "Add";

                    // Create an instance for location.
                    if (mProjectLocatonList == null)
                        mProjectLocatonList = new List<Location>();

                    // Create an instance for platforms.
                    if (mProjectPlatformList == null)
                        mProjectPlatformList = new List<Platform>();

                    // Create an instance for tests.
                    if (mProjectTestList == null)
                        mProjectTestList = new List<Test>();

                    chkActive.Checked = true;
                    divActive.Visible = false;
                    btnRefresh.Visible = false;

                    TreeNode rootNode = new TreeNode("All Platforms", "0");
                    rootNode.ExpandAll();
                    this.TreHierarchy.Nodes.Add(rootNode);
                    rootNode.Selected = true;

                    TreHierarchy_SelectedNodeChanged(sender, e);

                    // Set the flag.
                    ViewState.Add("PROJECT_ADDFLAG", "True");
                    ViewState.Add("PROJECT_EDITFLAG", "False");

                }
            }
            else
            {
                // Store from session.
                if (Session["VALID_PLATFORMS"] != null)
                    mValidPlatformList = (List<Platform>)Session["VALID_PLATFORMS"];
                if (Session["VALID_TESTS"] != null)
                    mValidTestList = (List<Test>)Session["VALID_TESTS"];
                if (Session["VALID_LOCATIONS"] != null)
                    mValidLocationList = (List<Location>)Session["VALID_LOCATIONS"];

                if (Session["PROJECT_PLATFORMS"] != null)
                    mProjectPlatformList = (List<Platform>)Session["PROJECT_PLATFORMS"];
                if (Session["PROJECT_TESTS"] != null)
                    mProjectTestList = (List<Test>)Session["PROJECT_TESTS"];
                if (Session["PROJECT_LOCATIONS"] != null)
                    mProjectLocatonList = (List<Location>)Session["PROJECT_LOCATIONS"];
            }

            LoadLabels();

            if (hdnProjectId.Value != "0")
            {
                btnUpdate.Text = BTNMODIFY;
            }
            else
            {
                btnUpdate.Text = BTNADD;
            }

        }

        catch (ValidationException ve)
        {
            DisplayValidationMessage(ve, "Project");
            return;
        }

        catch (Exception ex)
        {
            //ex.Source += "  ErrorOccured in : Project Edit VIew page load event";

            //Session.Add("TKS_ERROR", ex);
            throw ex;
            //

        }

    }


    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "PROJECT");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var ALERT_MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("ALERTMSG")).FirstOrDefault();
        if (ALERT_MSG != null)
        {
            ALERTMSG = Convert.ToString(ALERT_MSG.DisplayText); ;
        }

        var PAGEHEADING_ADD = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("LBLPROJECTMANAGEMENTADD")).FirstOrDefault();
        if (PAGEHEADING_ADD != null)
        {
            PAGEHEADINGADD = Convert.ToString(PAGEHEADING_ADD.DisplayText);

            if(ProjectId != 0)
                PAGEHEADINGADD = Convert.ToString(PAGEHEADING_ADD.SupportingText1);
        }

        var ALERT_F2MSG = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("PRESSF2MSG")).FirstOrDefault();
        if (ALERT_F2MSG != null)
        {
            PRESSF2_MSG = Convert.ToString(ALERT_F2MSG.DisplayText);
            AddPlatformTestHeader = Convert.ToString(ALERT_F2MSG.SupportingText1);
        }

        var btnADDMOD = lblLanguagelst.Where(c => c.LabelId.Equals("btnUpdate")).FirstOrDefault();
        if (btnADDMOD != null)
        {
            BTNMODIFY = Convert.ToString(btnADDMOD.SupportingText1);
            BTNADD = Convert.ToString(btnADDMOD.DisplayText);

        }

        var VALIDATIONS = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("VALIDATION")).FirstOrDefault();
        if (VALIDATIONS != null)
        {
            VALIDATIONERROR = Convert.ToString(VALIDATIONS.DisplayText);            
        }
        
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        ErrorLogProvider provider = null;

        try
        {

            Exception exception = HttpContext.Current.Error;
            provider = new ErrorLogProvider();
            provider.AppManager = this.mAppManager;
            provider.Insert(exception);

        }

        catch
        {
            throw;

        }
        finally
        {
            if (provider != null) provider.Dispose();
        }

    }


    private void InitializeUserSearchViewDialogScript()
    {
        try
        {
            mUserSearchViewDialogScript = new StringBuilder();
            mUserSearchViewDialogScript.Append("refreshUserSearchView();");
        }
        catch { throw; }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
            // Add values to session.
            Session.Add("VALID_PLATFORMS", mValidPlatformList);
            Session.Add("VALID_TESTS", mValidTestList);
            Session.Add("VALID_LOCATIONS", mValidLocationList);

            Session.Add("PROJECT_PLATFORMS", mProjectPlatformList);
            Session.Add("PROJECT_TESTS", mProjectTestList);
            Session.Add("PROJECT_LOCATIONS", mProjectLocatonList);

            // Register scripts.
            if (this.mUserSearchViewDialogScript != null)
                ScriptManager.RegisterStartupScript(
                    Page,
                    typeof(Page),
                    System.Guid.NewGuid().ToString(),
                    this.mUserSearchViewDialogScript.ToString(),
                    true);

        }
        catch { throw; }
    }

    private List<Location> RetrieveLocationsByProejct(int ProjectId)
    {
        ILocationService locationService = null;
        try
        {
            // Create an instance for platform service.
            locationService = AppService.Create<ILocationService>();
            locationService.AppManager = this.mAppManager;

            // Retrieve the platform details.
            List<Location> locations = locationService.RetrieveByProject(ProjectId);

            var list = from item in locations
                       where item.IsActive = true
                       select item;

            return list.ToList<Location>();
        }
        catch { throw; }
        finally
        {
            if (locationService != null) locationService.Dispose();
        }

    }


    private List<Platform> RetrievePlatformsByProject(int ProjectId)
    {
        IPlatformService platformService = null;
        try
        {
            // Create an instance for platform service.
            platformService = AppService.Create<IPlatformService>();
            platformService.AppManager = this.mAppManager;

            // Retrieve the platform details.
            List<Platform> platforms = platformService.RetrieveByProject(ProjectId);

            var list = from item in platforms
                       //where item.IsActive = true
                       select item;

            return list.ToList<Platform>();
        }
        catch { throw; }
        finally
        {
            if (platformService != null) platformService.Dispose();
        }

    }


    private List<Test> RetrieveTestsByProject(int ProjectId)
    {
        ITestService testService = null;
        try
        {
            // Create an instance for platform service.
            testService = AppService.Create<ITestService>();
            testService.AppManager = this.mAppManager;

            // Retrieve the platform details.
            List<Test> tests = testService.RetrieveByProject(ProjectId);

            var list = from item in tests
                       //where item.IsActive = true
                       select item;

            return list.ToList<Test>();
        }
        catch { throw; }
        finally
        {
            if (testService != null) testService.Dispose();
        }

    }

    private bool RetrieveProejcts(int projectId)
    {
        try
        {
            // Create an instance for project service.
            projectService = AppService.Create<IProjectService>();
            projectService.AppManager = this.mAppManager;

            // Retrievet the project details.
            Project project = projectService.Retrieve(projectId);
            txtName.Text = project.Name;
            txtDescription.Text = project.Description;
            txtClient.Value = project.CustomData["ClientName"].ToString();
            hdnClientId.Value = project.ClientId.ToString();
            txtResponsibleUser.Value = project.CustomData["ResponsibleUserName"].ToString();
            hdnUserId.Value = project.ResponsibleUserId.ToString();
            chkActive.Checked = bool.Parse(project.IsActive.ToString());
            ddcategory.SelectedValue = project.CategoryId.ToString();
            //txtReason.InnerText = project.Reason;

            if (project != null)
                return true;
            else
                return false;

        }
        catch { throw; }
        finally
        {
        }

    }


    private void Inilizethecontrols()
    {
        try
        {
            // Create an instance.
            authenticate = new UserAuthentication();
            // Getting appmagner.
            this.mAppManager = Session["APP_MANAGER"] as IAppManager;



            if (!IsPostBack)
            {
                // Disable controls.
                EnableControls(false);

                lnkPlatformEdit.Visible = false;
                // lnkPlatfromDelete.Visible = false;

                mValidLocationList = new List<Location>();

                mValidLocationList = this.RetrieveAllValidLocations();

            }

        }
        catch { throw; }
    }



    private List<Location> RetrieveAllValidLocations()
    {
        ILocationService locationsService = null;
        try
        {
            // Create an instance for platform service.
            locationsService = AppService.Create<ILocationService>();
            locationsService.AppManager = this.mAppManager;

            // Retrieve the platform details.
            List<Location> locations = locationsService.ReatrieveAll();

            var list = from item in locations
                       where item.IsActive = true
                       select item;

            return list.ToList<Location>();
        }
        catch { throw; }
        finally
        {
            if (platformService != null) platformService.Dispose();
        }

    }

    private void EnableControls(bool enable)
    {
        // Disablecontrols
        //divAddPlatformTest.Visible = enable;
        //divActive.Visible = enable;

    }


    private List<Location> GetLocationId()
    {
        try
        {

            List<Location> _ListLocation = new List<Location>();

            foreach (GridViewRow row in this.gvwLocation.Rows)
            {
                int LocationId;

                Location _location;
                LocationId = Int32.Parse(((HiddenField)row.FindControl("hdnLocationId")).Value);
                _location = new Location(Int32.Parse(hdnLocationId.ToString()));

                _ListLocation.Add(_location);

            }
            return _ListLocation;
        }
        catch { throw; }

    }


    protected void btnAddPlatform_Click(object sender, EventArgs e)
    {

        try
        {
            
            // Check the validation for project controls.
            //if (OnValidationProjects() == false)
            //    return;

            // divAddPlatformTest.Visible = true;
            //divActive.Visible = false;

            // RetrievePlatForm();

            if (Session["EDIT_PLATFORM"] != null)
                Session.Remove("EDIT_PLATFORM");

            if (Session["EDIT_TESTS"] != null)
                Session.Remove("EDIT_TESTS");


            this.BuildPlatformTestSelectionPanel();

            this.chkIsActive.Checked = true;

            this.mEntityEditPanelHeader = AddPlatformTestHeader;

            divErrorMessage.Style.Add("display", "none");
            divErrorMessage.InnerText = string.Empty;

            divSuccess.Style.Add("display", "none");
            divSuccess.InnerText = string.Empty;

            this.DisplayEntityEditPanel();


        }


        catch (Exception ex)
        {
            //ex.Source += "  ErrorOccured in : Project Edit VIew Add Platform event";

            //Session.Add("TKS_ERROR", ex);
            throw ex;
        }

    }

    private void DisplayEntityEditPanel()
    {
        try
        {
            // Display edit panel.
            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                System.Guid.NewGuid().ToString(),
                string.Format("showEditPanelDialog({{'width': '550px', 'title': '{0}'}});", mEntityEditPanelHeader),
                true);
        }
        catch { throw; }
    }

    private void BuildPlatformTestSelectionPanel()
    {
        try
        {
            // Retrieve all valid platforms.
            mValidPlatformList = this.RetrieveAllValidPlatForms();

            // TODO: Refactor...
            if (this.mProjectPlatformList == null)
                this.mProjectPlatformList = new List<Platform>();
            if (this.mProjectTestList == null)
                this.mProjectTestList = new List<Test>();

            // Filter non-selected platforms for the project.
            mProjectPlatformList.ForEach(
                item =>
                {
                    // Remove already selected platform from valid platform list.
                    mValidPlatformList.RemoveAll(
                        platform =>
                        {
                            return platform.Id == item.Id;
                        });
                });

            var platformlinq = from item in mValidPlatformList
                               where item.IsActive = true
                               select item;

            mValidPlatformList = platformlinq.ToList<Platform>();



            // Display in control.
            this.DisplayPlatforms(mValidPlatformList);

            // Retrieve all valid tests.
            mValidTestList = this.RetrieveAllVaildTests();

            var listLinq = from item in mValidTestList
                           where item.IsActive = true
                           orderby item.Name
                           select item;

            mValidTestList = listLinq.ToList<Test>();

            // Display in control.
            this.DisplayTests(mValidTestList);

            // Display platform and test selection panel as modal dialog.

        }
        catch { throw; }
    }

    private void DisplayPlatforms(IList<Platform> platforms)
    {
        try
        {
            this.drpPlatform.DataTextField = "Name";
            this.drpPlatform.DataValueField = "Id";
            this.drpPlatform.DataSource = platforms;
            this.drpPlatform.DataBind();

            // Add default item.
            this.drpPlatform.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.drpPlatform.Items.Count > 0) this.drpPlatform.SelectedIndex = 0;
        }
        catch { throw; }
    }


    private void DisplayEditPlatforms(IList<Platform> platforms)
    {
        try
        {
            this.drpPlatform.DataTextField = "Name";
            this.drpPlatform.DataValueField = "Id";
            this.drpPlatform.DataSource = platforms;
            this.drpPlatform.DataBind();

            // Add default item.
            this.drpPlatform.Items.Insert(0, new ListItem("-- Select --", "0"));

            // Select first item as default.
            if (this.drpPlatform.Items.Count > 0) this.drpPlatform.SelectedIndex = 1;
        }
        catch { throw; }
    }

    private void DisplayTests(IList<Test> tests)
    {
        try
        {
            this.gvwTests.DataSource = tests;
            this.gvwTests.DataBind();
        }
        catch { throw; }
    }



    private List<Platform> RetrieveAllValidPlatForms()
    {
        IPlatformService platformService = null;
        try
        {
            // Create an instance for platform service.
            platformService = AppService.Create<IPlatformService>();
            platformService.AppManager = this.mAppManager;

            // Retrieve the platform details.
            List<Platform> platforms = platformService.RetrieveAll();

            var list = from item in platforms
                       where item.IsActive = true
                       select item;

            return list.ToList<Platform>();
        }
        catch { throw; }
        finally
        {
            if (platformService != null) platformService.Dispose();
        }

    }

    private List<Test> RetrieveAllVaildTests()
    {
        ITestService service = null;
        try
        {
            // Create an instance for platform service.
            service = AppService.Create<ITestService>();
            service.AppManager = this.mAppManager;

            // Retrieve the platform details.
            List<Test> tests = service.RetrieveAll();

            var list = from item in tests
                       where item.IsActive = true
                       select item;

            return list.ToList<Test>();
        }
        catch { throw; }
        finally
        {
            if (service != null) service.Dispose();
        }
    }


    //private bool OnValidationProjects()
    //{
    //    try
    //    {
    //        ValidationException exception = new ValidationException("Validation error(s) occurred.");

    //        if (txtName.Text.Trim() == "")
    //            exception.Data.Add("Name", "Project name should not be empty.");
    //        if (txtClient.Value == "")
    //            exception.Data.Add("Client", "Client should not empty.");
    //        if (txtResponsibleUser.Value == "")
    //            exception.Data.Add("User", "Responsible user should not empty.");
    //        if (txtLocation.Value == "")
    //            exception.Data.Add("Location", "Location user should not empty.");

    //        if (exception.Data.Count > 0)
    //            throw exception;

    //        return true;
    //    }
    //    catch (ValidationException ve)
    //    {
    //        DisplayValidationMessage(ve, "Project");
    //        return false;
    //    }
    //    catch { throw; }
    //}


    private void DisplayValidationMessage(ValidationException errMessage, string Platform)
    {
        // Build validation error message.
        StringBuilder message = new StringBuilder(string.Format("{0}<ul>", errMessage.Message));
        foreach (DictionaryEntry entry in errMessage.Data)
        {
            message.Append(string.Format("<li>{0}</li>", entry.Value));
        }
        message.Append("</ul>");

        // Display validation error.
        this.DisplayValidationError(message.ToString(), Platform);


    }

    private void DisplayValidationError(string message, string Platform)
    {
        if (Platform == "Platform")
        {
            // Display message.
            this.divErrorMessage.InnerHtml = message;

            // Show panel.
            this.divErrorMessage.Style.Add("display", "block");
            this.divErrorMessage.Visible = true;
        }
        else
        {

            // Display message.
            this.divinf.InnerHtml = message;

            // Show panel.
            this.divinf.Style.Add("display", "block");
            this.divinf.Visible = true;
        }
    }

    private void DisplayValidationDisableError(string message)
    {
        // Display message.
        this.divinf.InnerHtml = message;

        // Show panel.
        this.divinf.Style.Add("display", "none");
        this.divinf.Visible = false;
    }


    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {

            // Validation
            // Check whether platform is selected or not.
            ValidationException exception = new ValidationException(VALIDATIONERROR);
            if (drpPlatform.SelectedIndex == 0)
                exception.Data.Add("ValidatePlatform", lblPlatformandTest.Text + ALERTMSG); //"Platfrom should not be empty.");

            if (chkIsActive.Checked == true)
            {

                // Check whether test is selected or not.
                List<Test> listValidTest = new List<Test>();
                listValidTest = RetrieveValidateTests();

                if (listValidTest.Count == 0)
                    exception.Data.Add("ValidTests", lblPlatformandTest.Text + ALERTMSG);

            }


            // Throw the exception.
            if (exception.Data.Count > 0)
                throw exception;

            DisplayValidationDisableError("");


            // Create an instance for edit platform and test list.
            List<Platform> mEditPlatformList = new List<Platform>();

            List<Test> mEditTestList = new List<Test>();


            if (Session["EDIT_PLATFORM"] != null)
                mEditPlatformList = (List<Platform>)Session["EDIT_PLATFORM"];


            // Remove edit platform
            mEditPlatformList.ForEach(
                item =>
                {
                    // Remove already selected platform from valid platform list.
                    mProjectPlatformList.RemoveAll(
                        platform =>
                        {
                            return platform.Id == item.Id;
                        });
                });



            // Create an instance.
            mValidPlatformList = new List<Platform>();
            // Retrieve all valid platforms.
            mValidPlatformList = this.RetrieveAllValidPlatForms();


            // Filter non-selected platforms for the project.
            mProjectPlatformList.ForEach(
                item =>
                {
                    // Remove already selected platform from valid platform list.
                    mValidPlatformList.RemoveAll(
                        platform =>
                        {
                            return platform.Id == item.Id;
                        });
                });




            if (chkIsActive.Checked == true)
            {

                if (Session["EDIT_TESTS"] != null)
                    mEditTestList = (List<Test>)Session["EDIT_TESTS"];

                // Filter non-selected test for the project.
                mEditTestList.ForEach(
                    item =>
                    {
                        // Remove already selected platform from valid platform list.
                        mProjectTestList.RemoveAll(
                            platform =>
                            {
                                return platform.CustomData["PlatformId"].ToString() == item.CustomData["PlatformId"].ToString();
                            });
                    });


            }



            // Retrieve selected platform.
            int selectedPlatformId = Int32.Parse(this.drpPlatform.SelectedValue);
            Platform selectedPlatform = mValidPlatformList.FirstOrDefault(item => item.Id == selectedPlatformId);

            if (chkIsActive.Checked == false)
                selectedPlatform.CustomData["IsActive"] = false;
            else
                selectedPlatform.CustomData["IsActive"] = true;

            // Add to project platform list.
            mProjectPlatformList.Add(selectedPlatform);

            // For order by name.
            var plotformItem = from item in mProjectPlatformList
                               orderby item.Name
                               select item;

            mProjectPlatformList = plotformItem.ToList<Platform>();

            // Display platform and it test.
            this.DisplayProjectPlatformList(mProjectPlatformList);

            if (chkIsActive.Checked == true)
            {
                // Retrieve selected tests.
                List<Test> selectedTests = this.RetrieveSelectedTests(selectedPlatform);
                selectedTests.ForEach(item => mProjectTestList.Add(item));


                // Display test.
                this.DisplayProjectPlatformTestList(selectedPlatformId, mProjectTestList);

            }

            //divAddPlatformTest.Visible = false;
            this.CloseDialogControl();

            this.btnRefresh.Visible = true;


        }
        catch (ValidationException ve)
        {
            DisplayValidationMessage(ve, "Platform");
            return;
        }

        catch (Exception ex)
        {
            //ex.Source += "  ErrorOccured in : Project Edit VIew Update Platform event";

            //Session.Add("TKS_ERROR", ex);
            throw ex;
        }

    }


    private void CloseDialogControl()
    {
        try
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), System.Guid.NewGuid().ToString(), "closeEditPanelDialog()", true);
        }
        catch { throw; }
    }


    private List<Test> RetrieveValidateTests()
    {

        try
        {
            List<Test> selectedList = new List<Test>();

            // Iterate each row.
            foreach (GridViewRow row in this.gvwTests.Rows)
            {
                CheckBox checkBox = (CheckBox)row.FindControl("ChkTestsId");
                if (checkBox.Checked)
                {
                    // Retrieve selected test id.
                    int testId = Int32.Parse(((HiddenField)row.FindControl("hdnTestsId")).Value);
                    Test test = new Test(testId);

                    selectedList.Add(test);

                }
            }

            return selectedList;
        }
        catch { throw; }
    }


    private void DisplayProjectPlatformList(List<Platform> platforms)
    {
        try
        {
            // Display in treeview control.

            // Create root node.
            TreeNode rootNode = new TreeNode("All Platforms", "0");
            // Add each project platform as child node.
            platforms.ForEach(
                item =>
                {
                    if (item.CustomData["IsActive"].ToString() == "True")
                        rootNode.ChildNodes.Add(new TreeNode(item.Name, item.Id.ToString()));
                    else
                        rootNode.ChildNodes.Add(new TreeNode(item.Name + " (InActive)", item.Id.ToString()));
                });


            // Clear all nodes.
            this.TreHierarchy.Nodes.Clear();
            rootNode.ExpandAll();
            this.TreHierarchy.Nodes.Add(rootNode);
            rootNode.Selected = true;
            lnkPlatformEdit.Visible = false;
        }
        catch { throw; }
    }

    private void DisplayProjectPlatformTestList(int platformId, List<Test> platformTests)
    {
        try
        {
            // Filter the test.
            //IEnumerable<Test> filteredTestList = null;

            List<Test> filteredTestList = null;


            if (platformId != 0)
            {
                filteredTestList = (from item in platformTests
                                    where item.CustomData["PlatformId"].ToString() == platformId.ToString()
                                    select item).ToList();
            }
            else
            {
                filteredTestList = (from item in platformTests
                                    select item).ToList();
            }

            // Bind with grid.
            this.gvwPlatformTests.DataSource = filteredTestList;
            this.gvwPlatformTests.DataBind();
            if (gvwPlatformTests.Rows.Count > 0)
            {
                lblTestnotavailable.Visible = false;
                List<LblLanguage> lblLanguagelst = null;

                ILblLanguage mLanguageService = null;
                lblLanguagelst = new List<LblLanguage>();
                mLanguageService = AppService.Create<ILblLanguage>();
                mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
                // retrieve
                lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "PROJECT");

                Utility _objUtil = new Utility();
                _objUtil.LoadGridLabels(lblLanguagelst, gvwPlatformTests);
                //dvinitalvalue.Visible = false;
                Tablelabel.Visible = false;
            }
        }
        catch { throw; }
    }

    private List<Test> RetrieveSelectedTests(Platform platform)
    {

        try
        {
            List<Test> selectedTestList = new List<Test>();

            // Iterate each row.
            foreach (GridViewRow row in this.gvwTests.Rows)
            {
                CheckBox checkBox = (CheckBox)row.FindControl("ChkTestsId");
                if (checkBox.Checked)
                {
                    // Retrieve selected test id.
                    int testId = Int32.Parse(((HiddenField)row.FindControl("hdnTestsId")).Value);

                    // Retrieve selected test entity.
                    Test selectedTest = mValidTestList.FirstOrDefault(item => item.Id == testId);
                    selectedTest.CustomData.Add("PlatformId", platform.Id);
                    selectedTest.CustomData.Add("PlatformName", platform.Name);

                    // Add to list.
                    selectedTestList.Add(selectedTest);
                }
            }

            return selectedTestList;
        }
        catch { throw; }
    }


    public int PlatformId()
    {
        return Int32.Parse(Session["EDIT_PROJECT_PLATFORMID"].ToString());
    }

    public string PlatformText()
    {
        return Session["EDIT_PROJECT_TextNode"].ToString();

    }

    protected void TreHierarchy_SelectedNodeChanged(object sender, EventArgs e)
    {

        try
        {

            // Retrieve selected node.
            int selectedPlatformId = Int32.Parse(this.TreHierarchy.SelectedNode.Value);

            string strActive = this.TreHierarchy.SelectedNode.Text.ToString();



            this.DisplayProjectPlatformTestList(selectedPlatformId, mProjectTestList);

            Session.Remove("EDIT_PROJECT_PLATFORMID");

            if (selectedPlatformId != 0)
            {

                lnkPlatformEdit.Visible = true;
                // lnkPlatfromDelete.Visible = true;
                Session.Add("EDIT_PROJECT_PLATFORMID", selectedPlatformId);
                Session.Add("EDIT_PROJECT_TextNode", strActive);

            }
            else
            {

                lnkPlatformEdit.Visible = false;
                //  lnkPlatfromDelete.Visible = false;
            }



        }

        catch { throw; }
        finally { }

    }

    protected void btnAddLocation_Click(object sender, EventArgs e)
    {

        try
        {
            
            if (this.mProjectLocatonList == null)
                this.mProjectLocatonList = new List<Location>();

            if (CheckValidationforLocation(hdnLocationId.Value) == false)
                return;

            divinf.Style.Add("display", "none");
            divinf.Visible = false;
            divinf.InnerText = string.Empty;

            int selectedLocationId = Int32.Parse(hdnLocationId.Value);

            // For reference.
            string selectedLocationName = txtLocation.Value;

            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);

            // Check the location whethere exists or not.
            Location availableLocation = mProjectLocatonList.FirstOrDefault(item => item.Id == selectedLocationId);


            // Add to exception.
            if (availableLocation != null)
            {
                if (availableLocation.CustomData["IsActive"].ToString() == "False")
                {
                    availableLocation.CustomData["IsActive"] = true;
                    availableLocation.CustomData["CustomActive"] = "Active";

                    List<Location> mEditLocationList = new List<Location>();

                    mEditLocationList.Add(availableLocation);

                    // Remove edit platform
                    mEditLocationList.ForEach(
                        item =>
                        {
                            // Remove already selected platform from valid platform list.
                            mProjectLocatonList.RemoveAll(
                                location =>
                                {
                                    return location.Id == item.Id;
                                });
                        });


                    // Add to location list.
                    mProjectLocatonList.Add(availableLocation);

                }
                else
                    exception.Data.Add("Location", lblLocation.Text + ALERTMSG);
            }
            else
            {
                // Retrieve the location details.
                Location selectedLocation = mValidLocationList.FirstOrDefault(item => item.Id == selectedLocationId);

                selectedLocation.CustomData["LocationManagerId"] = hdnLocationUser.Value;
                selectedLocation.CustomData["LocationManager"] = txtLocationUser.Value;

                // Add to location list.
                mProjectLocatonList.Add(selectedLocation);
            }


            // Return the exception
            if (exception.Data.Count > 0)
                throw exception;

            DisplayValidationDisableError("");

            var locationItem = from item in mProjectLocatonList
                               orderby item.State, item.Country, item.City
                               select item;

            mProjectLocatonList = locationItem.ToList<Location>();


            // Display the locations.
            DisplayProjectloations(mProjectLocatonList);

            txtLocation.Value = "";
            txtLocation.Focus();
            hdnLocationId.Value = "";
            txtLocationUser.Value = "";
            hdnLocationUser.Value = "";

        }
        catch (ValidationException ve)
        {
            DisplayValidationMessage(ve, "Project");
            return;
        }

        catch (Exception ex)
        {

            //ex.Source += "  ErrorOccured in : Project Edit VIew Add location event";

            //Session.Add("TKS_ERROR", ex);
            throw ex;
        }
    }


    private bool CheckValidationforLocation(string Id)
    {
        try
        {
            // Create an instance for exception.
            ValidationException exception = new ValidationException(VALIDATIONERROR);

            // Add to exception.
            if (Id.ToString() == string.Empty)
            {
                exception.Data.Add("VALIDATE_LOCATION", lblLocation.Text + ALERTMSG);
                txtLocation.Value = "";
            }

            if (hdnLocationUser.Value.ToString() == string.Empty)
            {
                exception.Data.Add("VALIDATE_LOCATIONUSER", lblLocationManager.Text + ALERTMSG);
                txtLocationUser.Value = "";
            }

            if (exception.Data.Count > 0)
                throw exception;

            return true;

        }
        catch (ValidationException ve)
        {
            DisplayValidationMessage(ve, "Project");
            return false;
        }

        catch { throw; }
    }

    private void DisplayProjectloations(List<Location> projectLocations)
    {
        try
        {
            // Bind with grid.
            this.gvwLocation.DataSource = projectLocations;
            this.gvwLocation.DataBind();

            List<LblLanguage> lblLanguagelst = null;

            ILblLanguage mLanguageService = null;
            lblLanguagelst = new List<LblLanguage>();
            mLanguageService = AppService.Create<ILblLanguage>();
            mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
            // retrieve
            lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "PROJECT");

            Utility _objUtil = new Utility();
            _objUtil.LoadGridLabels(lblLanguagelst, gvwLocation);
            dvinitalvalue.Visible = false;
        }
        catch { throw; }
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {

            ValidationException exception = new ValidationException(VALIDATIONERROR);

            if (txtName.Text == "")
                exception.Data.Add("VALIDATE_PROJECT_NAME", lblName.Text + ALERTMSG);

            if (hdnClientId.Value == "")
                exception.Data.Add("VALIDATE_PROJECT_CLIENT", lblClient.Text + ALERTMSG);

            if (hdnUserId.Value == "")
                exception.Data.Add("VALIDATE_PROJECT_RESPONSIBLEUSER", lblProjectManager.Text + ALERTMSG);

            if (mProjectLocatonList.Count == 0)
                exception.Data.Add("VALIDATE_PROJECT_LOCATION", lblLocation.Text + ALERTMSG);

            if (mProjectPlatformList.Count == 0)
                exception.Data.Add("VALIDATE_PROJECT_PLATFORM", lblPlatformandTest.Text + ALERTMSG);

            if (mProjectTestList.Count == 0)
                exception.Data.Add("VALIDATE_PROJECT_TEST", lblPlatformandTest.Text + ALERTMSG);

            if ((ViewState["PROJECT_ADDFLAG"].ToString().ToUpper() == "FALSE") && (ViewState["PROJECT_EDITFLAG"].ToString().ToUpper() == "TRUE"))
            {
                if (txtReason.InnerText.Trim() == string.Empty)
                    exception.Data.Add("VALIDATION_PROJECT_REASON", lblreason.Text + ALERTMSG);
            }

            if (exception.Data.Count > 0)
                throw exception;


            if ((ViewState["PROJECT_ADDFLAG"].ToString().ToUpper() == "FALSE") && (ViewState["PROJECT_EDITFLAG"].ToString().ToUpper() == "TRUE"))
            {
                UpdateEntity(Int32.Parse(hdnProjectId.Value));
                Session.Add("PROJECT_UPDATE", "Project updated Sucessfully.");

            }
            else if ((ViewState["PROJECT_ADDFLAG"].ToString().ToUpper() == "TRUE") && (ViewState["PROJECT_EDITFLAG"].ToString().ToUpper() == "FALSE"))
            {
                UpdateEntity(0);
                Session.Add("PROJECT_UPDATE", "Project added Sucessfully.");
            }


            divinf.Style.Add("display", "none");
            divinf.Visible = false;
            divmsg.InnerText = string.Empty;

            //divmsg.Style.Add("display", "block");
            //divmsg.InnerText = "Record updated Sucessfully.";

            lnkPlatformEdit.Visible = false;
            //lnkPlatfromDelete.Visible = false;



            Response.Redirect("~/Masters/ProjectListView.aspx", false);

            // Redirect the page.
            // this.Redirect();
        }
        catch (ValidationException ve)
        {
            DisplayValidationMessage(ve, "Project");
            return;
        }

        catch (Exception ex)
        {
            //ex.Source += "  ErrorOccured in : Project Edit VIew Update Project event";

            //Session.Add("TKS_ERROR", ex);
            throw ex;
        }

    }


    //public void Redirect()
    //{
    //    string redirectionScript = "<script language='javascript'>" +
    //            "function Delayer(){" +
    //            "setTimeout('Redirection()', 5000);" +
    //            "}" +
    //            "function Redirection(){" +
    //            "window.location = 'ViewUser.aspx';" +
    //            "}" +
    //            "Delayer()" +
    //            "</script>";
    //    this.Page.RegisterStartupScript("Startup", redirectionScript);
    //}
    private void UpdateEntity(int ProjectId)
    {
        try
        {

            projectService = AppService.Create<IProjectService>();
            projectService.AppManager = this.mAppManager;

            projectEntity = new Project();
            projectEntity.Id = ProjectId;
            projectEntity.Name = txtName.Text;
            projectEntity.ClientId = Int32.Parse(hdnClientId.Value);
            projectEntity.Description = txtDescription.Text;
            projectEntity.ResponsibleUserId = Int32.Parse(hdnUserId.Value);
            projectEntity.CategoryId = Int32.Parse(ddcategory.SelectedValue);

            if (ProjectId == 0)
                projectEntity.IsActive = true;
            else
                projectEntity.IsActive = chkActive.Checked;
            projectEntity.Reason = txtReason.InnerText;

            projectEntity.LastUpdateUserId = this.mAppManager.LoginUser.Id;

            // Update the project.
            projectService.Update(projectEntity, mProjectLocatonList, mProjectPlatformList, mProjectTestList);


        }

        catch { throw; }
    }

    //protected void btnRemove_Click(object sender, EventArgs e)
    //{
    //    divAddPlatformTest.Visible = false;
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {



            // Remove the list.
            RemoveLists();

            // Remove the sessions.
            RemoveSessions();

            divmsg.Style.Add("display", "none");
            divmsg.InnerText = "";

            Response.Redirect("~/Masters/ProjectListView.aspx", false);

        }
        catch (Exception ex) { throw ex; }

    }

    private void RemoveLists()
    {
        try
        {
            mProjectLocatonList = null;
            mProjectPlatformList = null;
            mProjectTestList = null;
        }
        catch { throw; }

    }

    private void ClearControls()
    {

        divErrorMessage.Style.Add("display", "none");
        divErrorMessage.InnerText = string.Empty;

        divSuccess.Style.Add("display", "none");
        divSuccess.InnerText = string.Empty;

    }

    private void RemoveSessions()
    {
        try
        {

            if (Session["VALID_PLATFORMS"] != null)
                Session.Remove("VALID_PLATFORMS");

            if (Session["VALID_TESTS"] != null)
                Session.Remove("VALID_TESTS");

            if (Session["VALID_LOCATIONS"] != null)
                Session.Remove("VALID_LOCATIONS");

            if (Session["PROJECT_PLATFORMS"] != null)
                Session.Remove("PROJECT_PLATFORMS");

            if (Session["PROJECT_TESTS"] != null)
                Session.Remove("PROJECT_TESTS");

            if (Session["PROJECT_LOCATIONS"] != null)
                Session.Remove("PROJECT_LOCATIONS");

            if (Session["EDIT_PROJECT_PLATFORMID"] != null)
                Session.Remove("EDIT_PROJECT_PLATFORMID");

            if (Session["PROJECT_UPDATE"] != null)
                Session.Remove("PROJECT_UPDATE");
        }
        catch { throw; }
    }
    protected void ibtSearchUsers_Click(object sender, ImageClickEventArgs e)
    {
        this.UserSearchView1.Display();
        this.mUserSearchViewDialogScript.Append("showUserSearchView()");
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            var plotformItem = from item in mProjectPlatformList
                               orderby item.Name
                               select item;

            mProjectPlatformList = plotformItem.ToList<Platform>();

            // Display platform and it test.
            this.DisplayProjectPlatformList(mProjectPlatformList);

            // Display test.
            this.DisplayProjectPlatformTestList(0, mProjectTestList);

            this.lnkPlatformEdit.Visible = false;
            // this.lnkPlatfromDelete.Visible = false;
        }
        catch { throw; }
    }

    protected void lnkPlatformEdit_Click(object sender, EventArgs e)
    {

        try
        {

            this.ClearControls();

            this.mEntityEditPanelHeader = "Edit Platforms and Tests";

            this.DisplayEntityEditPanel();


            // Create an instance for edit platform and test list.
            List<Platform> mEditPlatformList = new List<Platform>();
            List<Test> mEditTestList = new List<Test>();

            // Assign the platform from project platform list.
            // Retrieve selected platform.
            int selectedPlatformId = this.PlatformId();

            string lblIsActive = this.PlatformText();

            if (lblIsActive.Contains("InActive") == true)
                chkIsActive.Checked = false;
            else
                chkIsActive.Checked = true;


            Platform selectedPlatform = mProjectPlatformList.FirstOrDefault(item => item.Id == selectedPlatformId);


            // Add to edit platform list.
            mEditPlatformList.Add(selectedPlatform);

            Session.Add("EDIT_PLATFORM", mEditPlatformList);

            // Display in platfrom control.
            this.DisplayEditPlatforms(mEditPlatformList);



            var list = from item in mProjectTestList
                       where item.CustomData["PlatformId"].ToString() == selectedPlatformId.ToString()
                       select item;

            // Retrieve selected tests.
            mEditTestList = list.ToList<Test>();

            Session.Add("EDIT_TESTS", mEditTestList);


            // Retreive all test.

            mValidTestList = new List<Test>();

            // Retrieve all valid tests.
            mValidTestList = this.RetrieveAllVaildTests();


            //// Filter non-selected test for the project.
            //mEditTestList.ForEach(
            //    item =>
            //    {
            //        // Remove already selected platform from valid platform list.
            //        mValidTestList.RemoveAll(
            //            test =>
            //            {
            //                //return platform.CustomData["PlatformId"].ToString() == item.CustomData["PlatformId"].ToString();
            //                return test.Id == item.Id;
            //            });
            //    });

            ////// Retrieve selected tests.
            ////List<Test> selectedTests = this.RetrieveSelectedTests(selectedPlatform);

            //// Add edittestlist to valid list.
            //mEditTestList.ForEach(item => mValidTestList.Add(item));

            //var editTestList = from item in mValidTestList
            //                   orderby item.Name
            //                   select item;

            //mValidTestList = editTestList.ToList<Test>();


            // Display in test control.
            this.DisplayTests(mValidTestList);


            // Disable controls.
            // lnkPlatformEdit.Visible = false;
            //lnkPlatfromDelete.Visible = false;


        }

        catch (Exception ex)
        {
            //ex.Source += "  ErrorOccured in : Project Edit VIew Edit Platform event";

            //Session.Add("TKS_ERROR", ex);
            throw ex;
        }

    }

    // commented by sathiskumarv on 30.09.2011
    // Delete not required for i am puttnig active checkbox in popup window.
    /*
      protected void lnkPlatfromDelete_Click(object sender, EventArgs e)
      {
          try
          {
              this.ClearControls();

              // Create an instance for edit platform and test list.
              List<Platform> mEditPlatformList = new List<Platform>();
              List<Platform> mRemovePlatformList = new List<Platform>();
              List<Test> mEditTestList = new List<Test>();

              // Assign the platform from project platform list.
              // Retrieve selected platform.
              int selectedPlatformId = this.PlatformId();

              Platform selectedPlatform = mProjectPlatformList.FirstOrDefault(item => item.Id == selectedPlatformId);


              selectedPlatform.CustomData["IsActive"] = false;

              // Add to edit platform list.
              mEditPlatformList.Add(selectedPlatform);

              var list = from item in mProjectTestList
                         where item.CustomData["PlatformId"].ToString() == selectedPlatformId.ToString()
                         select item;

              // Retrieve selected tests.
              mEditTestList = list.ToList<Test>();

              // Commented for not remove manually, (InActive) is showing on treeveiw.
              // Remove edit platform
              mEditPlatformList.ForEach(
                  item =>
                  {
                      // Remove already selected platform from valid platform list.
                      mProjectPlatformList.RemoveAll(
                          platform =>
                          {
                              return platform.Id == item.Id;
                          });
                  });

              mProjectPlatformList.Add(selectedPlatform);

              var plotformItem = from item in mProjectPlatformList
                                 orderby item.Name
                                 select item;

              mProjectPlatformList = plotformItem.ToList<Platform>();

              //// Filter non-selected test for the project.
              //mEditTestList.ForEach(
              //    item =>
              //    {
              //        // Remove already selected platform from valid platform list.
              //        mProjectTestList.RemoveAll(
              //            platform =>
              //            {
              //                return platform.CustomData["PlatformId"].ToString() == item.CustomData["PlatformId"].ToString();
              //            });
              //    });


              // Display the project platforms.
              this.DisplayProjectPlatformList(mProjectPlatformList);

              // Display test.
              this.DisplayProjectPlatformTestList(0, mProjectTestList);

              // Disable controls.
              lnkPlatformEdit.Visible = false;
              lnkPlatfromDelete.Visible = false;

          }

          catch (Exception ex)
          {
              ex.Source += "  ErrorOccured in : Project Edit VIew Delete Platform event";

              Session.Add("TKS_ERROR", ex);
              throw ex;
          }
      }

      */


    //protected void gvwLocation_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {


    //            HiddenField hdnIsActive = (HiddenField)e.Row.FindControl("hdnIsActive");
    //            string strIsActive = hdnIsActive.Value.ToString();

    //            Label lblIsActive = (Label)e.Row.FindControl("lblIsActive");

    //            if (strIsActive == "True")
    //                lblIsActive.Text = "True";
    //            else
    //                lblIsActive.Text = "False";


    //            Tks.Entities.Location Entity = (Tks.Entities.Location)e.Row.DataItem;


    //            // Create an instance.
    //            List<Location> mRemoveLocationList = new List<Location>();

    //            //int testId = Int32.Parse(((HiddenField)e.Row.FindControl("hdnDeleteLocationId")).Value);

    //            Location selectedLocation = mValidLocationList.FirstOrDefault(item => item.Id == Entity.Id);


    //            // Disable the controls.
    //            DisplayValidationDisableError(string.Empty);

    //            // Retrieve the location details.
    //            //Location selectedLocation = mValidLocationList.FirstOrDefault(item => item.Id == selectedLocationId);

    //            mRemoveLocationList.Add(selectedLocation);


    //            // Filter non-selected test for the project.
    //            mRemoveLocationList.ForEach(
    //                item =>
    //                {
    //                    // Remove already selected platform from valid platform list.
    //                    mProjectLocatonList.RemoveAll(
    //                        platform =>
    //                        {
    //                            return platform.Id == item.Id;
    //                        });
    //                });


    //            // Display the locations.
    //            DisplayProjectloations(mProjectLocatonList);
    //        }
    //    }
    //    catch { throw; }

    //}

    protected void gvwLocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            
           
            // Create an instance.
            List<Location> mRemoveLocationList = new List<Location>();

            // Inistial the value.
            int selectedLocationId = 0;

            // Retrieve the location id.
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                selectedLocationId = Int32.Parse(this.gvwLocation.DataKeys[Int32.Parse(e.CommandArgument.ToString())]["Id"].ToString());

            // Disable the controls.
            DisplayValidationDisableError(string.Empty);

            // Retrieve the location details.
            //Location selectedLocation = mValidLocationList.FirstOrDefault(item => item.Id == selectedLocationId);
            Location selectedLocation = mProjectLocatonList.FirstOrDefault(item => item.Id == selectedLocationId);

            selectedLocation.CustomData["IsActive"] = false;
            selectedLocation.CustomData["CustomActive"] = "InActive";

            mRemoveLocationList.Add(selectedLocation);


            // Filter non-selected test for the project.
            mRemoveLocationList.ForEach(
                item =>
                {
                    // Remove already selected platform from valid platform list.
                    mProjectLocatonList.RemoveAll(
                        platform =>
                        {
                            return platform.Id == item.Id;
                        });
                });


            mProjectLocatonList.Add(selectedLocation);

            var locationItem = from item in mProjectLocatonList
                               orderby item.State, item.Country, item.City
                               select item;


            mProjectLocatonList = locationItem.ToList<Location>();


            // Display the locations.
            DisplayProjectloations(mProjectLocatonList);

            txtLocation.Value = "";
            hdnLocationId.Value = "";
            txtLocationUser.Value = "";
            hdnLocationUser.Value = "";


        }
        catch { throw; }

    }



    protected void gvwTests_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            List<Test> mGvwEditTestList = new List<Test>();

            // Retrieve selected test id.
            // int testId = Int32.Parse(((HiddenField)e.Row.FindControl("hdnTestsId")).Value);

            if (Session["EDIT_TESTS"] != null)
            {

                mGvwEditTestList = (List<Test>)Session["EDIT_TESTS"];

                foreach (Test test in mGvwEditTestList)
                {

                    //Test selectedTest = mGvwEditTestList.FirstOrDefault(item => item.Id == test.Id);

                    int testId = Int32.Parse(((HiddenField)e.Row.FindControl("hdnTestsId")).Value);
                    if (test.Id == testId)
                    {
                        if (test.CustomData["IsActive"].ToString() == "True")
                        {
                            CheckBox checkBox = (CheckBox)e.Row.FindControl("ChkTestsId");
                            checkBox.Checked = true;
                        }
                    }

                }
            }

        }


    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            lnkPlatformEdit.Visible = true;


        }
        catch (Exception ex)
        { throw ex; }
    }

    //protected void gvwLocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{

    //    try
    //    {
    //        //int projectId = 0;

    //        gvwLocation.PageIndex = e.NewPageIndex;


    //        //if (hdnProjectId.Value != "")
    //        //    projectId = Int32.Parse(hdnProjectId.Value.ToString());


    //        //// Retrieve the project Locations.
    //        //mProjectLocatonList = RetrieveLocationsByProejct(projectId);
    //        // Display the project locations.
    //        DisplayProjectloations(mProjectLocatonList);

    //        //// Retrieve the project tests.
    //        //mProjectTestList = RetrieveTestsByProject(ProjectId);

    //        //var testList = from item in mProjectTestList
    //        //               orderby item.Name
    //        //               select item;

    //        //mProjectTestList = testList.ToList<Test>();


    //        //// Display test.
    //        //this.DisplayProjectPlatformTestList(0, mProjectTestList);


    //    }
    //    catch { throw; }

    //}

    protected void gvwPlatformTests_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        try
        {
            //int projectId = 0;

            gvwPlatformTests.PageIndex = e.NewPageIndex;


            //if (hdnProjectId.Value != "")
            //    projectId = Int32.Parse(hdnProjectId.Value.ToString());


            //// Retrieve the project tests.
            //mProjectTestList = RetrieveTestsByProject(projectId);

            //var testList = from item in mProjectTestList
            //               orderby item.Name
            //               select item;

            //mProjectTestList = testList.ToList<Test>();


            // Display test.
            this.DisplayProjectPlatformTestList(0, mProjectTestList);


        }
        catch { throw; }

    }


}
