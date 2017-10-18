using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

using Tks.Model;

public partial class Testing_TestPage107 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            UserAuthentication ua = new UserAuthentication();

            GeneralSettingProvider provider = new GeneralSettingProvider();
            provider.AppManager = ua.AppManager;
            List<GeneralSetting> generalSettings = provider.RetrieveAll();
            provider.Dispose();

            ReportDataSource rds = new ReportDataSource("GeneralSettingDataset", generalSettings);

            this.ReportViewer1.Reset();
            LocalReport localReport = this.ReportViewer1.LocalReport;
            localReport.ReportPath = "Testing\\SampleReport.rdlc";
            localReport.DataSources.Clear();
            localReport.DataSources.Add(rds);

            this.ReportViewer1.ShowRefreshButton = true;
            this.ReportViewer1.LocalReport.Refresh();
        }
    }
}