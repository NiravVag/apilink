using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LoggerComponent
{
    public  class DataBaseLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, DBLogger> _loggers = new ConcurrentDictionary<string, DBLogger>();

        private string _connectionString = null;

        public DataBaseLoggerProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new DBLogger(name, _connectionString));
        }

        public void Dispose()
        {

        }
    }
}
