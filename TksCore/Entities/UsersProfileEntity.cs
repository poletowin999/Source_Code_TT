using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Tks.Model;

namespace Tks.Entities
{
   
    public sealed class UsersPermission
        : MasterEntity
    {
        public UsersPermission() : base() { }

        public UsersPermission(int id) : base(id) { }
        public int PermissionId { get; set; }
        public string PermissionNo { get; set; }
        public string PermissionName { get; set; }   

    }

    public sealed class PermissionAccess
     : MasterEntity
    {
        public PermissionAccess() : base() { }

        public PermissionAccess(int id) : base(id) { }

        public int PermissionId { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
    }
   

    public sealed class ClientAccess
       : MasterEntity
    {
        public ClientAccess() : base() { }

        public ClientAccess(int id) : base(id) { }

        public int PermissionId { get; set; }
        public int ClientId { get; set; }
    }

    public sealed class ReportAccess
      : MasterEntity
    {
        public ReportAccess() : base() { }

        public ReportAccess(int id) : base(id) { }

        public int PermissionId { get; set; }
        public int ReportId { get; set; }
    }
   


    public sealed class AccessLevel
     : MasterEntity
    {
        public AccessLevel() : base() { }

        public AccessLevel(int id) : base(id) { }
       
        public string AccessLevelName { get; set; }
        public string AccessLevelDescription { get; set; }        
    }

    public sealed class AccessLevelDetails
     : MasterEntity
    {
        public AccessLevelDetails() : base() { }

        public AccessLevelDetails(int id) : base(id) { }

        public int AccessLevelId { get; set; }
        public int PermissionId { get; set; }
    }
}
