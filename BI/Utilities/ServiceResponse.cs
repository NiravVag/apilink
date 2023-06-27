using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Titan.UFC.Devices.WebAPI.Helpers
{
    public class ServiceResponse
    {
        public int ResultCode { get; set; }
        public string ResultStatus { get; set; }
        public dynamic CustomInfo { get; set; } = null;

    }
}
