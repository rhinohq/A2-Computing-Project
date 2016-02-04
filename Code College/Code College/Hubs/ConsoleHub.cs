using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Code_College.Hubs
{
    [HubName("Console")]
    public class ConsoleHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void UpdateConsole(string ConsoleOutput)
        {
            Clients.Caller.ChangeConsoleContents(ConsoleOutput);
        }
    }
}