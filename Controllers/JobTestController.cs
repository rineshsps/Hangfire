using Hangfire.BusinessServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTestController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        public JobTestController(IJobTestService jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _jobTestService = jobTestService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }
        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());

            return Ok();
        }

        [HttpGet("/ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("jobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);
            //_recurringJobManager.AddOrUpdate("jobId-123", () => _jobTestService.ReccuringJob(), Cron.Daily);
            return Ok();
        }

        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));
            return Ok();
        }

        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());

            return Ok();
        }


        [HttpGet("/NotificationJob")]
        public ActionResult NotificationJob()
        {

            _recurringJobManager.AddOrUpdate("jobWeeklyId", () => _jobTestService.EmailNotificationJob(), Cron.Weekly);
            _recurringJobManager.AddOrUpdate("MonthlyMailNotificationId", () => _jobTestService.EmailNotificationJob(), Cron.Monthly);
            // With New job id sample 
            //_recurringJobManager.AddOrUpdate(Guid.NewGuid().ToString(), () => _jobTestService.EmailNotificationJob(), Cron.Daily);

            //Crone expression 
            //_recurringJobManager.AddOrUpdate("cron expression", () => _jobTestService.EmailNotificationJob(), "0 0 2,8 2,11 *");


            return Ok();
        }
    }
}
