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


public partial class Widgets_UserWorkDurationWidget : System.Web.UI.UserControl
{
    #region Class variables   

    #endregion

    #region Public members

    public IAppManager AppManager { get; set; }



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

    public void DisplayData()
    {
        DashboardProvider provider = null;
        try
        {
            // Create provider.
            provider = new DashboardProvider();
            provider.AppManager = this.AppManager;

            // Get dashboard item command.
            SqlCommand itemCommand = provider.CreateItemCommand(DashboardProvider.USER_WORK_DURATION);
            // Assign parameters value.
            itemCommand.Parameters["@UserId"].Value = this.AppManager.LoginUser.Id;         

            // Retrieve item data.
            DataSet dataSet = provider.RetrieveItemData(itemCommand);
            DataTable dataTable = dataSet.Tables[0];

            // Bind with chart.
            Series series = this.Chart1.Series["Default"];
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

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        LoadLabels();
    }



   



  
}