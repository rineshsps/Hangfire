namespace Hangfire.BusinessServices
{
    public interface IJobTestService
    {
        void FireAndForgetJob();

        void ReccuringJob();

        void DelayedJob();

        void ContinuationJob();

        void EmailNotificationJob();
    }

    public class JobTestService : IJobTestService
    {
        public void FireAndForgetJob()
        {

            Console.WriteLine("Hello from a Fire and Forget job!");
        }

        public void ReccuringJob()
        {
            Console.WriteLine("Hello from a Scheduled job!");

        }

        public void DelayedJob()
        {
            throw new Exception("failed...");
            Console.WriteLine("Hello from a Delayed job!");
        }

        public void ContinuationJob()
        {
            Console.WriteLine("Hello from a Continuation job!");
            // Send mail to the users. 

        }

        public void EmailNotificationJob()
        {

            // Send mail to the users. 
            Console.WriteLine("Hello  Email Notification Job job!");
        }
    }
}
