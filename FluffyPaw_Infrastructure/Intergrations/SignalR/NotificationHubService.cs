using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.SignalR
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHubService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(string notification, long accountId, string type, long referenceId)
        {
            await _hubContext.Clients.User(accountId.ToString()).SendAsync("ReceiveNoti", accountId.ToString(), notification, type, referenceId.ToString());
        }

        public async Task MessageNotification(long senderId, long receiverId, string notification, string type, long referenceId)
        {
            await _hubContext.Clients.User(receiverId.ToString()).SendAsync("MessageNoti", senderId, receiverId, notification, type, referenceId.ToString());
        }
    }
}
