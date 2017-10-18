using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

using Tks.Model;
using Tks.Entities;

namespace Tks.Services
{
    public interface IUserService
        : IEntityService
    {
        void Authenticate(string loginName, string password);
        User RetrieveByLoginName(string loginName);

        List<User> Search(SearchCriteria criteria);

        void Update(User entity);

        // =================
        // Used in user hierarchy page.
        List<User> RetrieveChildren(int userId);
        List<User> RetrieveAttachedUsers(int userId);
        List<User> RetrieveUnAttachedUsers(SearchCriteria criteria);

        User Retrieve(int userId);
        User RetrieveUsersettings(int userId);

        void AttachToHierarchy(List<User> users, DateTime attachDate,int AttachUserid);
        void DetachFromHierarchy(List<User> users, DateTime detachDate);
        void ChangeManager(List<User> users, DateTime detachDate);
        // =================

        // Forgot password request functionalities.
        string ResetPasswordRequest(string loginName);
        void SendResetPasswordRequestMail(MailMessage message);
        // =================

        // Password Reset functionalities.
        bool IsPasswordResetIdAvailable(string resetId);
        User RetrieveByResetId(string resetId);
        void ResetPassword(string resetId, string eMailId, string newPassword);
        // =================

        void ChangePassword(string loginName, string oldPassword, string newPassword);

        // Activity entry process.
        User RetrieveSupervisor(int userId);
        User RetrieveManager(int userId);
        //=================

        //Retrieve the Usertype.
        List<UserType> RetrieveUserType();
        //Retrieve the Usershift.
        List<UserShift> RetrieveUserShift();
        List<UserShift> RetrieveUserWorkTypes();
        

        void UpdateUserSettings(User entity);

        void InsertSession(string Sessionid, int Userid);
        bool CheckUserSession(string Sessionid, int Userid);
        bool CheckUserSession_Secure(string Sessionid, int Userid, string SystemName);
        string InsertUserlog(string Sessionid, int Userid, string systemname, string loginname, string Browserinfo,bool logout,bool IsvalidIP);

        List<User> LandDadmin(int userId);
        List<User> LandDalert(int userId);
        List<User> TTalert(int userId, int sno);

    }
}
