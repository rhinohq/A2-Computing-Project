using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Code_College.Models;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        private ExDBEntities ExDB = new ExDBEntities();
        // GET: Exercise
        public ActionResult Index()
        {
            return View();
        }

        public Exercise GetExercise(int ExerciseID)
        {
            Exercise Exercise = ExDB.Exercises.Where(x => x.ExID == ExerciseID).FirstOrDefault();

            return Exercise;
        }
    }
}