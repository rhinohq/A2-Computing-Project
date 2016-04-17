using Code_College.Models;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class DashboardController : Controller
    {
        public static ExTile[] ExerciseTiles;
        private static ExDBEntities ExDB = new ExDBEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            int NoOfExercises = 0;

            foreach (Exercise Exercise in ExDB.Exercises)
                NoOfExercises++;

            ExerciseTiles = new ExTile[NoOfExercises];

            return View();
        }

        public class ExTile
        {
            public bool CompletedByUser { get; set; }
            public bool Locked { get; set; }
            public int ExID { get; set; }
            public string ExTitle { get; set; }
        }
    }
}