using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Quartz
{
    public class QuartzJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(QuartzJob));
            options.AddJob<QuartzJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                        trigger.ForJob(jobKey)
                               .WithIdentity($"{nameof(QuartzJob)}-trigger")
                               .WithCronSchedule("0 0 0 * * ?"));
        }
    }
}
