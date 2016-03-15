using Code_College.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class AddExerciseController : Controller
    {
        public static List<ExRow> ExerciseRows = new List<ExRow>();

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
                string FilePath = Path.Combine(Server.MapPath("~/Exercises"), FileName);

                if (File.FileName.EndsWith(".ex"))
                {
                    File.SaveAs(FilePath);
                    ExerciseParser.ParseExFile(FilePath);
                }
            }

            return RedirectToAction("Index");
        }

        public class ExRow : IEquatable<ExRow>
        {
            public int ExID { get; set; }
            public string ExTitle { get; set; }

            public bool Equals(ExRow other)
            {
                throw new NotImplementedException();
            }
        }
    }
}