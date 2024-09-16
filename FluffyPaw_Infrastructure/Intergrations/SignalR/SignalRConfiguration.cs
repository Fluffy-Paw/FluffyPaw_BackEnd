using Microsoft.Extensions.Configuration;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.SignalR
{
    public class SignalRConfiguration : ISignalRConfiguration
    {
        private readonly IConfiguration _configuration;

        public SignalRConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
