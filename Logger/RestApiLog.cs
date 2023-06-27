using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerComponent
{
    public class RestApiLog
    {
        public int Id { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
        public string RequestQuery { get; set; }
        public string RequestBody { get; set; }
        public DateTime? RequestTime { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int? ResponseInMilliSeconds { get; set; }
        public string ResponseStatus { get; set; }
        public int? EntityId { get; set; }
        public int? CreatedBy { get; set; }
    }
}
