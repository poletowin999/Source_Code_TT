using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;

using Tks.Model;
using Tks.Entities;
using Tks.Services;


/// <summary>
/// Summary description for MasterService
/// </summary>
[WebService(Namespace = "http://www.poletowininternational.com/Tks/WebServices/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MasterService : System.Web.Services.WebService
{
    public MasterService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        //TODO: Need to remove.
        return "Hello World";
    }


    [WebMethod]
    public string GetLocationsByCity(string cityName, string userid)
    {
        ILocationService service = null;
        try
        {
            // Create search criteria.
            LocationSearchCriteria criteria = new LocationSearchCriteria();
            criteria.City = cityName;

            // Create the service.
            service = AppService.Create<ILocationService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Location> locations = service.Search(criteria, Convert.ToInt32(userid));
            string locationsJson = "[]";

            if (locations != null)
            {
                var resultList = from item in locations
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     City = item.City,
                                     Country = item.Country
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                locationsJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return locationsJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod]
    public string GetDepartments(string DeptartmentName)
    {
        IDepartmentService service = null;
        try
        {
            // Create search criteria.
            DepartmentSearchCriteria criteria = new DepartmentSearchCriteria();
            criteria.City = DeptartmentName;

            // Create the service.
            service = AppService.Create<IDepartmentService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Department> Departments = service.Searchdpt(criteria);
            string DepartmentsJson = "[]";

            if (Departments != null)
            {
                var resultList = from item in Departments
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     City = item.Name
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                DepartmentsJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return DepartmentsJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod]
    public string GetUsersByLocation(string name)
    {
        IUserService service = null;
        try
        {
            // Create search criteria.
            UserSearchCriteria criteria = new UserSearchCriteria();
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<IUserService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<User> users = service.Search(criteria);
            string usersJson = "[]";

            if (users != null)
            {
                var resultList = from item in users
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     Name = item.FirstName,
                                     ShortName = item.LastName
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                usersJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return usersJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }


    [WebMethod]
    public string GetTimeZonesByName(string name)
    {
        ITimeZoneService service = null;
        try
        {
            // Create search criteria.
            TimeZoneSearchCriteria criteria = new TimeZoneSearchCriteria();
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<ITimeZoneService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Tks.Entities.TimeZone> timeZones = service.Search(criteria);
            var resultList = from item in timeZones
                             where item.IsActive = true
                             select new
                             {
                                 Id = item.Id,
                                 Name = item.Name,
                                 ShortName = item.ShortName
                             };

            // Serialize.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string timeZonesJson = serializer.Serialize(resultList);

            // Return the value.
            return timeZonesJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }


    [WebMethod(EnableSession = true)]
    public string GetRolesByName(string name)
    {
        IRoleService service = null;
        try
        {
            // Create search criteria.
            MasterEntitySearchCriteria criteria = new MasterEntitySearchCriteria();
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<IRoleService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;
            string rolesJson = "[]";
            // Call service method.
            List<Role> roles = service.Search(criteria);
            if (roles != null)
            {
                var resultList = from item in roles
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     Name = item.Name,
                                     Description = item.Description
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                rolesJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return rolesJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetClientsByName(string name, string userid)
    {
        // Used in activity entry screen.
        IClientService service = null;
        try
        {
            // Create search criteria.
            ClientSearchCriteria criteria = new ClientSearchCriteria();
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<IClientService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Client> clients = service.Search(criteria,Convert.ToInt32(userid));
            string clientJson = "[]";
            if (clients != null)
            {
                var resultList = from item in clients
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     Name = item.Name,
                                     Description = item.Description
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                clientJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return clientJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetProjectsByClient(int clientId, int userid)
    {
        // Used in activity entry screen.
        IProjectService service = null;
        try
        {
            // Create the service.
            service = AppService.Create<IProjectService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Project> projects = service.RetrieveByClient(clientId, userid);

            // Serialize.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string projectJson = serializer.Serialize(projects);

            // Return the value.
            return projectJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetLocationsByProject(int projectId)
    {
        // Used in activity entry screen.
        ILocationService service = null;
        try
        {
            // Create the service.
            service = AppService.Create<ILocationService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Location> locations = service.RetrieveByProject(projectId);

            // Serialize.
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string locationJson = serializer.Serialize(locations);

            // Return the value.
            return locationJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetLanguagesByName(string name)
    {
        // Used in activity entry screen.
        ILanguageService service = null;
        try
        {
            // Create search criteria.
            MasterEntitySearchCriteria criteria = new MasterEntitySearchCriteria();
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<ILanguageService>();
            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<Language> languages = service.Search(criteria);
            string languageJson = "[]";
            if (languages != null)
            {
                var resultList = from item in languages
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     Name = item.Name,
                                     Description = item.Description
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                languageJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return languageJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetUsersByName(string name)
    {
        IUserService service = null;
        try
        {
            // Create search criteria.
            UserSearchCriteria criteria = new UserSearchCriteria();
            criteria.ViewName = "ActivityResetUserSearch";
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<IUserService>();

            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<User> users = service.Search(criteria);
            string usersJson = "[]";
            if (users != null)
            {
                var resultList = from item in users
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     Name = item.LastName ,
                                     ShortName = item.FirstName 
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                usersJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return usersJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }

    [WebMethod(EnableSession = true)]
    public string GetManageruser(string name)
    {
        IUserService service = null;
        try
        {
            // Create search criteria.
            UserSearchCriteria criteria = new UserSearchCriteria();
            criteria.ViewName = "SearchManager";
            criteria.Name = name;

            // Create the service.
            service = AppService.Create<IUserService>();

            // TODO: Need to change.
            UserAuthentication authentication = new UserAuthentication();
            service.AppManager = authentication.AppManager;

            // Call service method.
            List<User> users = service.Search(criteria);
            string usersJson = "[]";
            if (users != null)
            {
                var resultList = from item in users
                                 where item.IsActive = true
                                 select new
                                 {
                                     Id = item.Id,
                                     Name = item.LastName,
                                     ShortName = item.FirstName
                                 };

                // Serialize.
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                usersJson = serializer.Serialize(resultList);
            }

            // Return the value.
            return usersJson;
        }
        catch { throw; }
        finally
        {
            // Dispose.
            if (service != null) service.Dispose();
        }
    }
}
