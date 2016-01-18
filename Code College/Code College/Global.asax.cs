using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace Code_College
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Models.ExDBEntities ExDB = new Models.ExDBEntities();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            if (ExDB.Exercises.Where(x => x.ExID == 1).First() == null)
            {
                
            }
        }
    }
}