using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoggerComponent
{
    public class APILogger : IAPILogger
    {
        private static object _lock = new object();
        private readonly IConfiguration _configuration = null;

        public APILogger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SaveRestAPILog(RestApiLog restApiLog)
        {
            try
            {
                Task.Run(() =>
                {
                    lock (_lock)
                    {
                        using (var context = new LoggerDbContext(_configuration.GetConnectionString("APIConnection")))
                        {
                            context.RestApiLog.Add(restApiLog);

                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {

            }

        }
    }
}
