using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class LoginMobileRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
        public bool isEncrypt { get; set; }
    }
}
