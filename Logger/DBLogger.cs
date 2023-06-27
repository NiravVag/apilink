using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LoggerComponent
{
    internal class DBLogger : ILogger
    {
        private string _ConnectionString = null;

        private string _name;
        private static object _lock = new object();


        public DBLogger(string name, string connectionString)
        {
            _name = name;
            _ConnectionString = connectionString;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {

            //    if (!IsEnabled(logLevel))
            //    {
            //        return;
            //    }


            //    Task.Run(() =>
            //    {
            //        lock (_lock)
            //        {
            //            using (var context = new LoggerDbContext(_ConnectionString))
            //            {

            //                context.EventLog.Add(new EventLog
            //                {
            //                    CreatedTime = DateTime.Now,
            //                    LogLevel = logLevel.ToString(),
            //                    EventID = eventId.Id,
            //                    Message = formatter(state, exception),
            //                    Exception = exception?.ToString(),
            //                    Name = _name
            //                });

            //                try
            //                {
            //                    context.SaveChanges();
            //                }
            //                catch (Exception) { }                           

            //            }

            //        }
            //    }
            //);


            }
            catch (Exception ex)
            {

            }
        }
    }
}
