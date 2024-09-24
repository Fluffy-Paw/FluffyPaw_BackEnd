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
        private readonly IHubContext<NotiHub> _notiHub;

        public SignalRConfiguration(IConfiguration configuration, IHubContext<NotiHub> notiHub)
        {
            _configuration = configuration;
            _notiHub = notiHub;
        }

        public async Task SendNoti(string noti)
        {
            await _notiHub.Clients.All.SendAsync("ReceiveNoti", noti);
        }
    }
}
