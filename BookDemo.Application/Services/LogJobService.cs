using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BookDemo.Application.Services
{
    public class LogJobService :IJob
    {
        private readonly ILogger<LogJobService> _logger;

        // ILogger'ın doğru şekilde alındığından emin olun
        public LogJobService(ILogger<LogJobService> logger)
        {
            _logger = logger;
        }

        // Job'ı çalıştıran metot
        public Task Execute(IJobExecutionContext context)
        {
            // Konsola yazı yazdırma
            _logger.LogInformation($"Log Job Started! Time: {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}
