using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface INotificationHubService
    {
        Task SendNotification(string noti, long accountId, string notiType, long referenceId);
        Task MessageNotification(string noti, long accountId, string notiType, long referenceId);
    }
}
