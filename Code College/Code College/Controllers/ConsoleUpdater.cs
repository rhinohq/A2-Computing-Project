using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Code_College.Controllers
{
    public class ConsoleUpdater : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void UpdateConsole(string ConsoleOutput)
        {
            Clients.All.ChangeConsoleContents(ConsoleOutput);
        }
    }
}