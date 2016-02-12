using Code_College.Models;
using System.Linq;
using System.Web.Mvc;

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