using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Entities;

namespace Tks.Model
{
    internal sealed class AppManager
        : IAppManager
    {
        #region Class variables

        User _loginUser;
        DbConnectionProvider _dbConnectionProvider;

        #endregion

        #region Constructor

        internal AppManager()
        {
            this._loginUser = null;
            this._dbConnectionProvider = new DbConnectionProvider(this);
        }

        #endregion

        #region Internal members

        internal void AssignLoginUser(User user)
        {
            try
            {
                this._loginUser = user;
            }
            catch { throw; }
        }

        #endregion

        public Entities.User LoginUser
        {
            get { return this._loginUser; }
            set { this._loginUser = value; }
        }

        public DbConnectionProvider DbConnectionProvider
        {
            get { return this._dbConnectionProvider; }
        }


    }
}
