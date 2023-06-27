using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class StaffDeleteRequest
    {
        public int Id { get; set;  }

        public DateObject LeaveDate { get; set;  }
        
        public  string Reason { get; set;  }

        public int StatusId { get; set; }
    }
}
