using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tks.Entities;
using Tks.Model;
using Tks.Services;
using Tks.ServiceImpl;

public partial class HomePage : System.Web.UI.Page
{

    #region Class Variables

    IAppManager mAppManager;
    UserAuthentication authenticate;

    #endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            LoadLabels();

            this.UserWorkDurationWidget1.AppManager = this.Master.AppManager;
            this.UserWorkDurationWidget1.DisplayData();

            this.UserActivityStatusWidget1.AppManager = this.Master.AppManager;
            this.UserActivityStatusWidget1.DisplayData();

            // Create the instance
            authenticate = new UserAuthentication();

            if (mAppManager == null)
                mAppManager = authenticate.AppManager;

            LoadBirthDayList();
            LoadalertList();
            LoadSurveyList();           
           

        }
        catch { throw; }
    }


    public void LoadLabels()
    {
        List<LblLanguage> lblLanguagelst = null;

        ILblLanguage mLanguageService = null;
        lblLanguagelst = new List<LblLanguage>();
        this.mAppManager = Master.AppManager;
        mLanguageService = AppService.Create<ILblLanguage>();
        mLanguageService.AppManager = mAppManager;
        // retrieve
        lblLanguagelst = mLanguageService.RetrieveLabel(Master.AppManager.LoginUser.Id, "USERMODULE");

        Utility _objUtil = new Utility();
        _objUtil.LoadLabels(lblLanguagelst);
    } 

    public void LoadBirthDayList()
    {
        List<User> Birthdaylst = null;

        IUserService mBirthdayService = null;
        Birthdaylst = new List<User>();
        this.mAppManager = Master.AppManager;
        mBirthdayService = AppService.Create<IUserService>();
        mBirthdayService.AppManager = mAppManager;
        // retrieve
        Birthdaylst = mBirthdayService.LandDadmin(Master.AppManager.LoginUser.Id);

        if (Birthdaylst != null)
        {
            dvbirthday.Visible = true;
            Grdlist.DataSource = Birthdaylst;
            Grdlist.DataBind();
        }
        else
        {
            dvbirthday.Visible = false;

        }
    }

    public void LoadalertList()
    {
        List<User> Birthdaylst = null;

        IUserService mBirthdayService = null;
        Birthdaylst = new List<User>();
        this.mAppManager = Master.AppManager;
        mBirthdayService = AppService.Create<IUserService>();
        mBirthdayService.AppManager = mAppManager;
        // retrieve
        Birthdaylst = mBirthdayService.LandDalert(Master.AppManager.LoginUser.Id);

        if (Birthdaylst != null)
        {
            dvalert.Visible = true;
            Gridalert.DataSource = Birthdaylst;
            Gridalert.DataBind();
        }
        else
        {
            dvalert.Visible = false;

        }
    }

    public void LoadSurveyList()
    {
        List<User> Birthdaylst = null;

        IUserService mBirthdayService = null;
        Birthdaylst = new List<User>();
        this.mAppManager = Master.AppManager;
        mBirthdayService = AppService.Create<IUserService>();
        mBirthdayService.AppManager = mAppManager;
        // retrieve
        Birthdaylst = mBirthdayService.TTalert(Master.AppManager.LoginUser.Id,1);

        if (Birthdaylst != null)
        {
            DivSurvey.Visible = true;
            GridSurvey.DataSource = Birthdaylst;
            GridSurvey.DataBind();
        }
        else
        {
            DivSurvey.Visible = false;

        }
    }    
}