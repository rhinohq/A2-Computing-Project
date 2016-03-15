using Code_College.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Code_College.Controllers
{
    public class DashboardController : Controller
    {
        public static List<ExTile> ExerciseTiles = new List<ExTile>();

        // GET: Dashboard
        public ActionResult Index()
        {
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