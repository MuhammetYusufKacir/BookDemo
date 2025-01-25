using BookDemo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace BookDemoAPI.Controllers
{
    [ApiController]
    [Route("api/scheduler")]
    public class SchedulerController : Controller
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public SchedulerController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        [HttpPost("start-log-job")]
        public async Task<IActionResult> StartLogJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            if (!scheduler.IsStarted)
            {
                await scheduler.Start();
            }

            var jobKey = new JobKey("logJobKey");
            if (!await scheduler.CheckExists(jobKey))
            {
                var job = JobBuilder.Create<LogJobService>()
                                    .WithIdentity(jobKey)
                                    .Build();

                var trigger = TriggerBuilder.Create()
                                            .WithIdentity("logJobTrigger")
                                            .StartNow()
                                            .WithSimpleSchedule(x => x
                                                .WithIntervalInSeconds(10)
                                                .RepeatForever())
                                            .Build();

           
                await scheduler.ScheduleJob(job, trigger);

                return Ok("Log job successfully started!");
            }

            return Ok("Log job is already running.");
        }

        [HttpPost("stop-log-job")]
        public async Task<IActionResult> StopLogJob()
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKey = new JobKey("logJobKey");
            if (await scheduler.CheckExists(jobKey))
            {
                await scheduler.DeleteJob(jobKey);
                return Ok("Log job stopped!");
            }

            return NotFound("Log job is not running.");
        }
    }

}
