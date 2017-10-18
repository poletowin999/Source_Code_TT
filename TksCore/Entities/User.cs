using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;

namespace Tks.Entities
{
    public sealed class User
        : EntityBase
    {
        public User()
            : base() { }
        public User(int id)
            : base(id) { }
     
        #region Public Members

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Initial { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public string EmailId { get; set; }

        public string EmployeeId { get; set; }

        public int TypeId { get; set; }

        public int AccessLevelId { get; set; }        

        public int RoleId { get; set; }

        public int IsactivityApprovalPending { get; set; }

        public int LocationId { get; set; }

        public int DepartmentId { get; set; }

        public string HomePhone { get; set; }

        public string OfficePhone { get; set; }

        public string OfficePhoneExt { get; set; }

        public bool HasAdminRights { get; set; }

        public bool IsPasswordChanged { get; set; }

        public bool HasAutoApproval { get; set; }

        public bool IsSysAdmin { get; set; }

        public bool IsLandDAdmin { get; set; }

        public bool IsActive { get; set; }

        public bool IsAnyLocation { get; set; } // Added on 05282012 by saravanan ;

        public string Reason { get; set; }

        public int LastUpdateUserId { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public DateTime LastUpdateDate { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? RelieveDate { get; set; }
        public int? ManagerId { get; set; }

        public int? LanguageId { get; set; }

        public int? ShiftId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool 기타 { get; set; }

        #endregion

        public string ToString(string format)
        {
            try
            {
                if (format.Equals("FIL", StringComparison.InvariantCultureIgnoreCase))
                    return string.Format("{0} {1} {2}", this.FirstName, this.Initial, this.LastName);
                else
                    return "";
            }
            catch { throw; }
        }

    }
    public class UserType : EntityBase
    {
        public UserType()
        {
        }
        public UserType(int id)
            : base(id)
        {
        }
        public string Name { get; set; }
        
    }
    public class UserShift : EntityBase
    {
        public UserShift()
        {
        }
        public UserShift(int id)
            : base(id)
        {
        }
        public string Name { get; set; }

    }



}
