using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Common
{
    public class PushSubscriptionModel
    {
        public string Endpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }
    }
}
