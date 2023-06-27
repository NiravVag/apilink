using DTO.Location;
using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class SaveOfficeControlRequest
    {
        public int StaffId { get; set;  }

        public IEnumerable<Office> Data { get; set;  }
    }
}
