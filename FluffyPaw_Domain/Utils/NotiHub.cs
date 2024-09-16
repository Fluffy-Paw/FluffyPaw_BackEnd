using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FluffyPaw_Domain.Utils
{
    public class NotiHub : Hub
    {
        public async Task sendNoti(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",message);
        }
    }
}
