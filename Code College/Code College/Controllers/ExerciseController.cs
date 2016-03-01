using Code_College.Models;
using Language.Lua;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static UserDBEntities UserDB = new UserDBEntities();
        private Regex Regex = new Regex("[a-zA-Z0-9'-_.]", RegexOptions.Compiled);

        public static int ID = 1;

        // GET: Exercise
        public ActionResult Index()
        {
            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == ID).FirstOrDefault();

            ViewBag.Title = CurrentExercise.ExTitle + " - Code College";
            ViewBag.ExerciseTitle = CurrentExercise.ExTitle;
            ViewBag.Desc = CurrentExercise.ExDescription;
            ViewBag.ExerciseID = CurrentExercise.ExID;
            ViewBag.CodeTemplate = CurrentExercise.ExCodeTemplate ?? "";
            ViewBag.Exercise = CurrentExercise;

            if (!Regex.IsMatch(ViewBag.CodeTemplate))
                ViewBag.CodeTemplate = "";

            return View();
        }

        public static string SubmitCode(string Code, Exercise CurrentExercise, string Username)
        {
            bool Correct;

            Code += CurrentExercise.ExAppendCode ?? "";

            Marker.Marker.MarkScheme = CurrentExercise.ExMarkScheme;

            LuaInterpreter.RunCode(Code);
            Correct = Marker.Marker.FullMark();

            if (Correct)
            {
                Account.LevelUp(Username);

                return LuaInterpreter.CodeReport.Output;
            }
            else
            {
                return "Sorry, that was incorrect. Please, read the task and try again.";
            }
        }
    }
}