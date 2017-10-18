using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;

using Tks.Model;
using Tks.Entities;
using Tks.Services;


public partial class Widgets_UserActivityStatusWidget : System.Web.UI.UserControl
{

    #region Class variables
  

    #endregion

    #region Public members

    public IAppManager AppManager { get; set; }

    public void DisplayData()
    {
        DashboardProvider provider = null;
        try
        {
            // Create provider.
            provider = new DashboardProvider();
            provider.AppManager = this.AppManager;

            // Get dashboard item command.
            SqlCommand itemCommand = provider.CreateItemCommand(DashboardProvider.USER_ACTIVITY_STATUS);
            // Assign parameters value.
            itemCommand.Parameters["@UserId"].Value = this.AppManager.LoginUser.Id;           

            // Retrieve item data.
            DataSet dataSet = provider.RetrieveItemData(itemCommand);
            DataTable dataTable = dataSet.Tables[0];

            // Bind with chart.
            this.Chart1.Series.Clear();
            this.Chart1.DataBindCrossTable(dataTable.DefaultView, "Status", "ActivityDate", "ActivityCount", "");     


            foreach (Series series in this.Chart1.Series)
            {
                if (series.Name.Equals("Waiting For Approval", StringComparison.InvariantCultureIgnoreCase))
                {
                    series.Color = System.Drawing.Color.FromArgb(236,188,67);
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

    #endregion

    public void LoadLabels()
    {
        DashboardProvider provider = null;
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();

        provider = new DashboardProvider();
        provider.AppManager = this.AppManager;

        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = AppManager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(AppManager.LoginUser.Id, "USERDURATIONWIDGET");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);
    } 

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadLabels();
    }
}