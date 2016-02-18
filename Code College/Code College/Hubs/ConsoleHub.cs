using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Code_College.Controllers;

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