using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerComponent
{
    public class EventLog
    {
        public int Id { get; set; }

        public int EventID { get; set; }

        public string LogLevel { get; set; }

        public string Message { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Exception { get; set; }

        public string Name { get; set; }
    }
}
