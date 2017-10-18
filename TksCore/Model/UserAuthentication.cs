using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Tks.Entities;
using Tks.Model;
using Tks.Services;
using Tks.ServiceImpl;

namespace Tks.Model
{

    /// <summary>
    /// Authenticate the user.
    /// </summary>
    /// 
    public sealed class UserAuthentication
    {

        #region Class Variables

        IAppManager _appManager;
        IUserService userService;
        User user;
        SqlConnection mDbConnection;


        #endregion

        public UserAuthentication()
        {
            this._appManager = new AppManager();
            mDbConnection = _appManager.DbConnectionProvider.GetDefaultDbConnectionInstance();
        }

        public IAppManager AppManager
        {
            get { return this._appManager; }
        }

        public User LoginUser
        {
            get { return user; }
        }


        public void Authenicate(string loginName, string password)
        {
            try
            {
                // Create an user service.
                userService = AppService.Create<IUserService>();
                userService.AppManager = _appManager;

                // Call method to authenticate.
                userService.Authenticate(loginName, password);

                // If succeed then get login user and assign to IAppManager.
                user = userService.RetrieveByLoginName(loginName);

                AppManager appManager = new AppManager();
                appManager.AssignLoginUser(user);
                this._appManager = appManager;

            }
            catch { throw; }

            finally
            {
                if (userService != null) userService.Dispose();
            }

        }
    }
}
