using Code_College.Models;
using Language.Lua;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static UserDBEntities UserDB = new UserDBEntities();

        public static string SubmitCode(string Code, Exercise CurrentExercise, string Username)
        {
            bool Correct;
            string ConsoleOutput = "Sorry, that was incorrect. Please, read the task and try again.";

            Code += CurrentExercise.ExAppendCode ?? "";

            Marker.Marker.MarkScheme = CurrentExercise.ExMarkScheme;

            try
            {
                LuaInterpreter.RunCode(Code);
                Correct = Marker.Marker.FullMark();
            }
            catch (Exception ex)
            {
                Correct = false;

                ConsoleOutput = ex.Message;
            }

            if (Correct)
            {
                Account.LevelUp(Username, CurrentExercise);
                ConsoleOutput = LuaInterpreter.CodeReport.Output;

                return ConsoleOutput;
            }
            else
                return ConsoleOutput;
        }

        // GET: Exercise
        public ActionResult Index(int id = 1)
        {
            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == id).FirstOrDefault();

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