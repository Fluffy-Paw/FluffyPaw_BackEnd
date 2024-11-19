using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace FluffyPaw_Infrastructure.Intergrations.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string noti, long accountId)
        {
            await Clients.All.SendAsync("ReceiveNoti", accountId.ToString(), noti);
        }
    }
}
