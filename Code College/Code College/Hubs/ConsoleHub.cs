using Code_College.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Code_College.Models;

using System.Linq;

namespace Code_College.Hubs
{
    [HubName("Console")]
    public class ConsoleHub : Hub
    {
        private static ExDBEntities ExDB = new ExDBEntities();

        public void UpdateConsole(string Code, int ExerciseID, string Username)
        {
            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == ExerciseID).FirstOrDefault();

            Clients.Caller.UpdateConsole(ExerciseController.SubmitCode(Code, CurrentExercise, Username));
        }
    }
}