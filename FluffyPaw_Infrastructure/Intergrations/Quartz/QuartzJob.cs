using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Intergrations.Quartz
{
    public class QuartzJob : IJob
    {
        private readonly IQuartzTask _quartzTask;

        public QuartzJob(IQuartzTask quartzTask)
        {
            _quartzTask = quartzTask;
        }

        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
