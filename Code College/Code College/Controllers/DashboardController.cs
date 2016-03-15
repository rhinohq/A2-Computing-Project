using Code_College.Models;
using System;
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

        public class ExTile : IEquatable<ExTile>
        {
            public bool CompletedByUser { get; set; }
            public int ExID { get; set; }
            public string ExTitle { get; set; }

            public bool Equals(ExTile other)
            {
                throw new NotImplementedException();
            }
        }
    }
}