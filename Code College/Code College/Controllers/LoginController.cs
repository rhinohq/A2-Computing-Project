using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public static RedirectResult Dash()
        {
            return new RedirectResult("~/Dash");
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
    }
}