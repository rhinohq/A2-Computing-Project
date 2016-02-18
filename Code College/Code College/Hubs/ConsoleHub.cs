using Code_College.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Code_College.Hubs
{
    [HubName("Console")]
    public class ConsoleHub : Hub
    {
        public void GetConnectionID(string ID)
        {
            ExerciseController.ConnectionID = ID;
        }
    }
}