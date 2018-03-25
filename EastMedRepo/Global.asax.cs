using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EastMedRepo.Class;
using System.Web.Optimization;
using System.Data.Entity.Infrastructure.Interception;
using EastMedRepo.DAL;
using EastMedRepo.Backup;
using EastMed.Data.Model;
using System.Data.Entity;

namespace EastMedRepo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
            BootStrapper.RunConfig();
            //Register the Scripts and Css files when application start 
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbInterception.Add(new EastmedInterceptorTransientErrors());
            DbInterception.Add(new EastmedInterceptorLogging());
            //Trigs the scheduler on applicaton start point.
            ScheduleWrapper scheduler = new ScheduleWrapper();            
            scheduler.RunJob();
            MvcHandler.DisableMvcResponseHeader = true;
            Database.SetInitializer<EastMedDB>(null);
        }
        // Save the active user and total user visited on application to the session 
        protected void Session_Start()
        {
            if(Application["ActiveUser"] == null)
            {
                int count = 1;
                Application["ActiveUser"] = count;
            }
            else
            {
                int count = (int)Application["ActiveUser"];
                count++;
                Application["ActiveUser"] = count;
            }
            if(Application["TotalUser"] == null)
            {
                int count = 1;
                Application["TotalUser"] = count;
            }
            else
            {
                int count = (int)Application["TotalUser"];
                count++;
                Application["TotalUser"] = count;
            }
        }
        protected void Session_End()
        {
            int count = (int)Application["ActiveUser"];
            count--;
            Application["ActiveUser"] = count;
        }
    }
}
