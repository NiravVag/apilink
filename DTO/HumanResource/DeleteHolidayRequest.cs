using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class DeleteHolidayRequest
    {
        public int Id { get; set;  }

        public bool ForAllIterations { get; set; }
    }
}
