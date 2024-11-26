using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FluffyPaw_Infrastructure.Intergrations.SignalR
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Lấy User Identifier từ CustomUserIdProvider
            Console.WriteLine($"NotificationHub: User connected with ID: {userId}");

            // Log thêm toàn bộ Claims để kiểm tra
            var claims = Context.User?.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            Console.WriteLine($"NotificationHub: User Claims: {string.Join(", ", claims ?? new List<string>())}");

            return base.OnConnectedAsync();
        }
    }
}
