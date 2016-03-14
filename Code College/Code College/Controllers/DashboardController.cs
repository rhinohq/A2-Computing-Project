using Code_College.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class DashboardController : Controller
    {
        public static List<ExTile> ExerciseTiles = new List<ExTile>();

        private static ExDBEntities ExDB = new ExDBEntities();
        private static UserDBEntities UserDB = new UserDBEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            foreach (Exercise Exercise in ExDB.Exercises)
            {
                ExerciseTiles.Add(new ExTile { ExID = Exercise.ExID, ExTitle = Exercise.ExTitle, CompletedByUser = false });

                // TODO: Get Logged in user and have the exercises create dynamically.
                //if (Exercise.ExID >= User.UserLevel)
                //    ExerciseTiles.Add(new ExTile { ExID = Exercise.ExID, ExTitle = Exercise.ExTitle, CompletedByUser = false });
                //else if (Exercise.ExID < User.UserLevel)
                //    ExerciseTiles.Add(new ExTile { ExID = Exercise.ExID, ExTitle = Exercise.ExTitle, CompletedByUser = true });
                //else
                //    ExerciseTiles.Add(new ExTile { ExID = Exercise.ExID, ExTitle = Exercise.ExTitle, CompletedByUser = false });
            }

            return View();
        }

        public class ExTile : IEquatable<ExTile>
        {
            public int ExID { get; set; }
            public string ExTitle { get; set; }
            public bool CompletedByUser { get; set; }

            public bool Equals(ExTile other)
            {
                throw new NotImplementedException();
            }
        }
    }
}