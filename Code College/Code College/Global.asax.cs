using Code_College.App_Start;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace Code_College
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Models.ExDBEntities ExDB = new Models.ExDBEntities();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //  TODO: Check exercise database for entries.
        }
    }
}