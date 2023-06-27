using DTO.Location;
using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class OfficeControlStaffResponse
    {
        public IEnumerable<Office> Data { get; set;  }

        public OfficeControlStaffResult Result { get; set;  }

    }

    public enum OfficeControlStaffResult
    {
        Success = 1
    }
}
