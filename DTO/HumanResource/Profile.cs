using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class Profile
    {
        public int Id { get; set;  }

        public string Name { get; set;  }
    }
    public class HRProfileResponse
    {
        public IEnumerable<Profile> ProfileList { get; set; }
        public DataSourceResult Result { get; set; }
    }
}
