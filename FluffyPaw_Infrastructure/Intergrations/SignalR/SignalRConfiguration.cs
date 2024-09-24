using Microsoft.Extensions.Configuration;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace FluffyPaw_Infrastructure.Intergrations.SignalR
{
    public class SignalRConfiguration : ISignalRConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public SignalRConfiguration(IConfiguration configuration, IHubContext<NotificationHub> notificationHub)
        {
            _configuration = configuration;
            _notificationHub = notificationHub;
        }

        public async Task SendNotification(string noti)
        {
            await _notificationHub.Clients.All.SendAsync("ReceiveNoti", noti);
        }
    }
}
