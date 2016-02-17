using Code_College.Models;
using Code_College.Hubs;

using System.Linq;
using System.Web.Mvc;

using Language.Lua;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static UserDBEntities UserDB = new UserDBEntities();
        private static ConsoleHub ConsoleHub = new ConsoleHub();

        public static int ID = 1;

        // GET: Exercise
        public ActionResult Index()
        {
            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == ID).FirstOrDefault();

            ViewBag.Title = CurrentExercise.ExTitle + " - Code College";
            ViewBag.ExerciseTitle = CurrentExercise.ExTitle;
            ViewBag.Desc = CurrentExercise.ExDescription;
            ViewBag.CodeTemplate = CurrentExercise.ExCodeTemplate;
            ViewBag.Exercise = CurrentExercise;

            return View();
        }
        
        public void SubmitCode(string Code, Exercise CurrentExercise)
        {
            bool Correct;

            Code = Code + CurrentExercise.ExAppendCode;

            LuaInterpreter.RunCode(Code);
            Correct = Marker.Marker.FullMark();

            if (Correct)
            {
                ConsoleHub.UpdateConsole(LuaInterpreter.CodeReport.Output);

                Account.LevelUp(ViewBag.Username);
            }
            else
            {
                ConsoleHub.UpdateConsole("Sorry, that was incorrect. Please, read the task and try again.");
            }
        }
    }
}