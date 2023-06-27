using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Mail;
using System.IO;
using Microsoft.Extensions.Logging;

namespace EmailSchedulerService
{
    public class EmailScheduler
    {
        static void Main(string[] args)
        {
            SchedulerDataAccess schedulerDataAccess = new SchedulerDataAccess();
            schedulerDataAccess.ScheduleProcess(args);
        }
    }
}
