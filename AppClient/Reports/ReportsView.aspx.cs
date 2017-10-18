using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tks.Entities;
using Tks.Model;
using Tks.Services;

public partial class Reports_ReportsView : System.Web.UI.Page
{
    SqlConnection myconn, myconn1;
    string cnString = ConfigurationManager.ConnectionStrings["DEFAULT_DATABASE_CONNECTION_STRING"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        myconn = new SqlConnection(cnString);
        LoadLabels();
        Bindreports();
    }

    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = ((IAppManager)Session["APP_MANAGER"]);
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(((IAppManager)Session["APP_MANAGER"]).LoginUser.Id, "REPORTHOME");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);

        //var GRID_TITLE = lblLanguagelst.Where(c => c.LabelId.Equals("lblCheckout")).FirstOrDefault();
        //if (GRID_TITLE != null)
        //{

        //    lblCheckOut = Convert.ToString(GRID_TITLE.DisplayText);
        //    lblCheckIn = Convert.ToString(GRID_TITLE.SupportingText1);
        //}

    }

    private void Bindreports()
    {

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cnString);

        string Password;
        string txtPassword;
        txtPassword = "";
        char txtPassword1;
        int lenPassword = builder.Password.Length;
        for (int lengthpwd = 0; lengthpwd < lenPassword; )
        {
            Password = builder.Password.Substring(lengthpwd, 4);
            txtPassword1 = Convert.ToChar(Convert.ToInt32(Password) - 1000);
            char chargthpwd = (char)txtPassword1;
            txtPassword = txtPassword + chargthpwd.ToString();
            lengthpwd = lengthpwd + 4;
        }
        builder.Password = txtPassword;

        string DBIP;
        string texttxtDBIP;
        texttxtDBIP = "";
        char DBIP1;
        int lenDBIP = builder.DataSource.Length;
        for (int lengthDBIP = 0; lengthDBIP < lenDBIP; )
        {
            DBIP = builder.DataSource.Substring(lengthDBIP, 4);
            DBIP1 = Convert.ToChar(Convert.ToInt32(DBIP) - 1000);
            char characterDBIP = (char)DBIP1;
            texttxtDBIP = texttxtDBIP + characterDBIP.ToString();
            lengthDBIP = lengthDBIP + 4;
        }
        builder.DataSource = texttxtDBIP;

        string server;
        string txtuserid = builder.UserID;
        string textuserid;
        textuserid = "";
        char server1;
        int len = builder.UserID.Length;
        for (int length = 0; length < len; )
        {
            server = builder.UserID.Substring(length, 4);
            server1 = Convert.ToChar(Convert.ToInt32(server) - 1000);
            char character = (char)server1;
            textuserid = textuserid + character.ToString();
            length = length + 4;
        }

        builder.UserID = textuserid;
        myconn = new SqlConnection(builder.ToString());
        myconn.Open();

        SqlCommand myCommand = new SqlCommand("RetrieveReportsforTT", myconn);
        myCommand.Parameters.AddWithValue("@Languageid", Session["SesLanguageId"]);
        myCommand.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter ad = new SqlDataAdapter(myCommand);
        myCommand.CommandTimeout = 0;

        DataSet ds = new DataSet();
        ad.Fill(ds);
        
        dtlReportList.DataSource = ds;
        dtlReportList.DataBind();
    }
}