using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Code_College.Models;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        // GET: Exercise
        public ActionResult Index()
        {
            return View();
        }
    }
}