using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.UserProfile
{
    public class RolesResponse
    {
        public IEnumerable<Role> List { get; set;  }

        public RolesResult Result { get; set;  }
    }

    public enum RolesResult
    {
        Success = 1,
        NotFound = 2
    }

    public class Role
    {
        public int Id { get; set;  }

        public string Name { get; set;  }
    }


}
