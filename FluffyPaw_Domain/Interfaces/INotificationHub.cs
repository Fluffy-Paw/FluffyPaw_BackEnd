using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface INotificationHub
    {
        Task SendNotification(string noti, long accountId);
    }
}
