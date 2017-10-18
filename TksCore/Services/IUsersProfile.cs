using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.Entities;

namespace Tks.Services
{   
        public interface IUsersProfile
        : IEntityService
        {
            DataTable RetrieveReports(int LanguageId); 
            DataTable RetrieveLocations();
            DataTable RetrieveDepartments();
            DataTable RetrieveClients();
            DataTable RetrieveUserPermissionById(int PermissionId);
            DataTable RetrieveUserPermissionAccess(int PermissionId);
            DataTable RetrieveUserClientAccess(int PermissionId);
            DataTable RetrieveUserReportAccess(int LanguageId, int PermissionId);
            void AddEditUserPermissions(UsersPermission Permission, List<PermissionAccess> PermissionAccessList, List<ClientAccess> ClientAccessList, List<ReportAccess> ReportAccessList);
            void AddEditAccessLevelDetails(List<AccessLevelDetails> AccessLevelDetailsList);
            List<UsersPermission> SearchPermissions(string PermissionNo, string Status);
            List<AccessLevel> SearchAccessLevel(string AccessLevelName, string Status);
            DataTable RetrieveAllAccessLevel();
            DataTable RetrieveAllPermissions();
            DataTable RetrieveAccessLevelDetailsById(int AccessLevelId);
        }   
}
