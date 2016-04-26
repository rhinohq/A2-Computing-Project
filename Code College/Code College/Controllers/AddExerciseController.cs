using Code_College.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class AddExerciseController : Controller
    {
        public static ExRow[] ExerciseRows;
        private static ExDBEntities ExDB = new ExDBEntities();

        // GET: AddExercise
        public ActionResult Index()
        {
            // Determines how many exercises are in the database and creates an array of that size
            int NoOfExercises = 0;

            foreach (Exercise Exercise in ExDB.Exercises)
                NoOfExercises++;

            ExerciseRows = new ExRow[NoOfExercises];

            return View();
        }

        // Handles file submission
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase File)
        {
            // Checks to see if the file is empty
            if (File.ContentLength > 0)
            {
                // Saves file in an 'Exercises' directory on the server
                string FileName = Path.GetFileName(File.FileName);
                string FilePath = Path.Combine(Server.MapPath("~/Exercises"), FileName);

                // Checks to see if file is an exercise file
                if (File.FileName.EndsWith(".ex"))
                {
                    File.SaveAs(FilePath);
                    ExerciseParser.ParseExFile(FilePath);
                }
            }

            return RedirectToAction("Index");
        }

        public class ExRow
        {
            public int ExID { get; set; }
            public string ExTitle { get; set; }
        }
    }
}