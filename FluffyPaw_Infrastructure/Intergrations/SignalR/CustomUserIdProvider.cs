using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var claims = connection.User?.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            Console.WriteLine($"Claims: {string.Join(", ", claims)}");

            var userId = connection.User?.FindFirst("id")?.Value;

            Console.WriteLine($"CustomUserIdProvider: Retrieved UserId = {userId}");

            return userId;
        }

    }
}
