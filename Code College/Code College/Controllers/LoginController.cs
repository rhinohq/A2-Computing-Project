using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public static RedirectResult Dash()
        {
            return new RedirectResult("~/Dash");
        }
    }
}