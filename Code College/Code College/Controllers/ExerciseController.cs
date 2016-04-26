using Code_College.Models;
using System.Linq;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        private static ExDBEntities ExDB = new ExDBEntities();

        // GET: Exercise
        public ActionResult Index(int id = 1)
        {
            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == id).FirstOrDefault();

            // Loads exercise data into the ViewBag for the view to display
            ViewBag.ExID = CurrentExercise.ExID;
            ViewBag.Title = CurrentExercise.ExTitle + " - Code College";
            ViewBag.ExerciseTitle = CurrentExercise.ExTitle;
            ViewBag.Desc = CurrentExercise.ExDescription;
            ViewBag.ExerciseID = CurrentExercise.ExID;
            ViewBag.CodeTemplate = CurrentExercise.ExCodeTemplate ?? "";
            ViewBag.AppendCode = CurrentExercise.ExAppendCode ?? "";

            return View();
        }
    }
}