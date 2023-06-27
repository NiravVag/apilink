using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class SignInRequest
    {
        public string UserName { get; set;  }

        public string Password { get; set;  }

        public bool IsEncrypt { get; set;  }
    }
}
