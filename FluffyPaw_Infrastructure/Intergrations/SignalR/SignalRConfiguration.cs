using Microsoft.Extensions.Configuration;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;


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

        public async Task SendNotification(string noti, long accountId)
        {
            await _notificationHub.Clients.User(accountId.ToString()).SendAsync("ReceiveNoti", noti);
        }
    }
}
