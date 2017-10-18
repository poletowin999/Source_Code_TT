using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;
using Tks.ServiceImpl;


namespace Tks.Services
{
    /// <summary>
    /// Provides various services of the application.
    /// </summary>
    public sealed class AppService
    {
        /// <summary>
        /// Create the service instance of given service type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            try
            {
                if (typeof(T) == typeof(IUserService))
                {
                    // Create the service.
                    return (T)(object)new UserService();
                }
                else if (typeof(T) == typeof(IRoleService))
                {
                    // Create the service.
                    return (T)(object)new RoleService();
                }
                else if (typeof(T) == typeof(IClientService))
                {
                    // Create the service.
                    return (T)(object)new ClientService();
                }
                else if (typeof(T) == typeof(ILocationService))
                {
                    // Create the service.
                    return (T)(object)new LocationService();
                }
                else if (typeof(T) == typeof(ITimeZoneService))
                {
                    // Create the service.
                    return (T)(object)new TimeZoneService();
                }
                else if (typeof(T) == typeof(IMenuService))
                {
                    // Create the service.
                    return (T)(object)new MenuService();
                }
                else if (typeof(T) == typeof(IPlatformService))
                {
                    // Create the service.
                    return (T)(object)new PlatformService();
                }
                else if (typeof(T) == typeof(IDepartmentService))
                {
                    // Create the service.
                    return (T)(object)new DepartmentService();
                }
                else if (typeof(T) == typeof(ITestService))
                {
                    // Create the service.
                    return (T)(object)new TestService();
                }
                else if (typeof(T) == typeof(IBillingTypeService))
                {
                    // Create the service.
                    return (T)(object)new BillingTypeService();
                }
                else if (typeof(T) == typeof(ILanguageService))
                {
                    // Create the service.
                    return (T)(object)new LanguageService();
                }
                else if (typeof(T) == typeof(IWorkTypeService))
                {
                    // Create the service.
                    return (T)(object)new WorkTypeService();
                }
                    // Mohan
 /*               else if (typeof(T) == typeof(ICategoryService))
                {
                    // Create the service.
                    return (T)(object)new CategoryService();
                }
                */
                else if (typeof(T) == typeof(IActivityService))
                {
                    // Create the service.
                    return (T)(object)new ActivityService();
                }
                else if (typeof(T) == typeof(IProjectService))
                {
                    // Create the service.
                    return (T)(object)new ProjectService();
                }
                else if (typeof(T) == typeof(IUserSwipeService))
                {
                    //create the service.
                    return (T)(object)new UserSwipeService();
                }
                else if (typeof(T) == typeof(IReportService))
                {
                    //create the service.
                    return (T)(object)new ReportService();
                }
                //else if (typeof(T) == typeof(IErrorService))
                //{
                //    //create the service.
                //    return (T)(object)new ErrorService();
                //}
                else if (typeof(T) == typeof(IIPservice))
                {
                    //create the service.
                    return (T)(object)new IPService();
                }
                else if (typeof(T) == typeof(ILblLanguage))
                {
                    //create the service.
                    return (T)(object)new LblLanguageService();
                }
                else if (typeof(T) == typeof(IUsersProfile))
                {
                    //create the service.
                    return (T)(object)new UsersProfile();
                }
                else
                {
                    // Default value.
                    return default(T);
                }
            }
            catch { throw; }
        }
    }
}
