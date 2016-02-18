using Code_College.Models;
using Code_College.Hubs;

using System;
using System.Linq;
using System.Web.Mvc;

using Language.Lua;
using Microsoft.AspNet.SignalR;

namespace Code_College.Controllers
{
    public class ExerciseController : Controller
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static UserDBEntities UserDB = new UserDBEntities();
        public static string ConnectionID { get; set; }

        public static int ID = 1;

        // GET: Exercise
        public ActionResult Index()
        {
            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == ID).FirstOrDefault();

            ViewBag.Title = CurrentExercise.ExTitle + " - Code College";
            ViewBag.ExerciseTitle = CurrentExercise.ExTitle;
            ViewBag.Desc = CurrentExercise.ExDescription;
            ViewBag.CodeTemplate = CurrentExercise.ExCodeTemplate ?? "";
            ViewBag.Exercise = CurrentExercise;

            return View();
        }
        
        public static void SubmitCode(string Code, Exercise CurrentExercise, string Username)
        {
            bool Correct;

            Code = Code + CurrentExercise.ExAppendCode ?? "";

            Marker.Marker.MarkScheme = CurrentExercise.ExMarkScheme;

            LuaInterpreter.RunCode(Code);
            Correct = Marker.Marker.FullMark();

            if (Correct)
            {
                UpdateConsole(LuaInterpreter.CodeReport.Output);

                Account.LevelUp(Username);
            }
            else
            {
                UpdateConsole("Sorry, that was incorrect. Please, read the task and try again.");
            }
        }

        public static void UpdateConsole(string ConsoleOutput)
        {
            IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<ConsoleHub>();

            HubContext.Clients.Client(ConnectionID).UpdateConsole(ConsoleOutput);
        }
    }
}