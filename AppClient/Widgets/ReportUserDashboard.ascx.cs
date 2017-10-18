using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Linq;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
using Tks.ServiceImpl;

public partial class Widgets_ReportUserDashboard : System.Web.UI.UserControl
{

    #region Class variables
    IAppManager mAppManager;

    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }

    string ActivityStatus;
    string WorkDuration;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        this.mAppManager = Session["APP_MANAGER"] as IAppManager;

        LoadLabels();
        lblActivityStat.InnerHtml = ActivityStatus;
        lblWorkStat.InnerHtml = WorkDuration;

        if (!Page.IsPostBack)
        {
            LoadWorkDuration();
            LoadActivityStatus();
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
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "DASHBOARDREPORT");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.ToUpper().Equals("MSG_IN_GRID")).FirstOrDefault();
        if (GRID_TITLE != null)
        {

          //  this.InitalspnMessage.InnerHtml = Convert.ToString(GRID_TITLE.DisplayText);
        }

        var lblActivityStatus_text = lblLanguagelst.Where(c => c.LabelId.Equals("lblActivityStatus")).FirstOrDefault();
        if (lblActivityStatus_text != null)
        {

            ActivityStatus = Convert.ToString(lblActivityStatus_text.DisplayText);
            WorkDuration = Convert.ToString(lblActivityStatus_text.SupportingText1);
        }


    }

    public void LoadWorkDuration()
    {

        DashboardProvider provider = null;
        try
        {

            BeginDate = string.IsNullOrEmpty(this.hfStartDate.Value) ? DateTime.Parse(DateTime.UtcNow.AddDays(-15).ToString("yyyy-MM-dd").Replace("/", "-")) : DateTime.Parse(this.hfStartDate.Value);
            EndDate = string.IsNullOrEmpty(this.hfEndDate.Value) ? DateTime.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd").Replace("/", "-")) : DateTime.Parse(this.hfEndDate.Value);

            // Create provider.
            provider = new DashboardProvider();
            provider.AppManager = this.mAppManager;

            // Get dashboard item command.
            SqlCommand itemCommand = provider.CreateItemCommand_Report(DashboardProvider.USER_WORK_DURATION);
            // Assign parameters value.
            itemCommand.Parameters["@UserId"].Value = this.mAppManager.LoginUser.Id;
            itemCommand.Parameters["@BeginDate"].Value = BeginDate;
            itemCommand.Parameters["@EndDate"].Value = EndDate;

            // Retrieve item data.
            DataSet dataSet = provider.RetrieveItemData(itemCommand);
            DataTable dataTable = dataSet.Tables[0];

            // Bind with chart.
            Series series = this.chtWrkDuration.Series["Default"];
            series.Points.DataBind(dataTable.DefaultView, "WorkDate", "WorkDuration1", "");

            // Label values shown angel
            series.LabelAngle = -90;
            series.LabelFormat = "N2";
            series.LabelForeColor = System.Drawing.Color.DarkBlue;
            series.SmartLabelStyle.Enabled = false;

            int i = 0;
            foreach (var point in series.Points)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(point.YValues[0])) && !Convert.ToString(point.YValues[0]).Equals("0"))
                {
                    point.ToolTip = string.Concat("Hours ", Convert.ToString(dataTable.Rows[i]["WorkDuration1"]).Replace(".", ":"));
                    point.IsValueShownAsLabel = true;
                }

                i++;
            }

        }
        catch { throw; }
    }

    public void LoadActivityStatus()
    {
        DashboardProvider provider = null;
        try
        {
            // Create provider.
            provider = new DashboardProvider();
            provider.AppManager = this.mAppManager;

            // Get dashboard item command.
            SqlCommand itemCommand = provider.CreateItemCommand_Report(DashboardProvider.USER_ACTIVITY_STATUS);
            // Assign parameters value.
            itemCommand.Parameters["@UserId"].Value = this.mAppManager.LoginUser.Id;
            itemCommand.Parameters["@BeginDate"].Value = BeginDate;
            itemCommand.Parameters["@EndDate"].Value = EndDate;

            // Retrieve item data.
            DataSet dataSet = provider.RetrieveItemData(itemCommand);
            DataTable dataTable = dataSet.Tables[0];

            // Bind with chart.
            this.chtActStatus.Series.Clear();
            this.chtActStatus.DataBindCrossTable(dataTable.DefaultView, "Status", "ActivityDate", "ActivityCount", "");


            foreach (Series series in this.chtActStatus.Series)
            {
                if (series.Name.Equals("Waiting For Approval", StringComparison.InvariantCultureIgnoreCase))
                {
                    series.Color = System.Drawing.Color.FromArgb(236, 188, 67);
                }
                else if (series.Name.Equals("Approved Activity", StringComparison.InvariantCultureIgnoreCase))
                {
                    series.LegendText = "Approved";
                    series.Color = System.Drawing.Color.FromArgb(67, 142, 83);
                }
                else if (series.Name.Equals("Rejected Activity", StringComparison.InvariantCultureIgnoreCase))
                {
                    series.LegendText = "Rejected";
                    series.Color = System.Drawing.Color.FromArgb(236, 100, 74);
                }
                else if (series.Name.Equals("Resetted Activity", StringComparison.InvariantCultureIgnoreCase))
                {
                    series.LegendText = "Resetted";
                    series.Color = System.Drawing.Color.FromArgb(85, 93, 131);
                }
                series.ChartType = SeriesChartType.StackedColumn;
                series.SetCustomProperty("PointWidth", "0.8");
                series.SetCustomProperty("DrawBySide", "false");

                // Label values shown angel
                series.LabelAngle = -90;
                series.LabelForeColor = System.Drawing.Color.Black;
                series.SmartLabelStyle.Enabled = false;

                foreach (var point in series.Points)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(point.YValues[0])) && !Convert.ToString(point.YValues[0]).Equals("0"))
                    {
                        point.ToolTip = string.Concat("Activity Count : ", Convert.ToString(point.YValues[0]));
                        point.IsValueShownAsLabel = true;
                    }
                }


            }
        }
        catch { throw; }
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            LoadWorkDuration();
            LoadActivityStatus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "DateRange", "DateRangePicker();", true);
        }
        catch { throw; }
    }
}