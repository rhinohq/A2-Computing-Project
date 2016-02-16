using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Code_College.Models;

namespace Code_College.Controllers
{
    public class AddExerciseController : Controller
    {
        // GET: AddExercise
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase File)
        {

            if (File.ContentLength > 0)
            {
                string FileName = Path.GetFileName(File.FileName);
                string FilePath = Path.Combine(Server.MapPath("~/App_Data/Exercises"), FileName);
                File.SaveAs(FilePath);

                ExerciseParser.ParseExFile(FilePath);
            }

            return RedirectToAction("Index");
        }
    }
}