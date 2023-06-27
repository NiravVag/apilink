using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class StaffKAMProfileResponse
    {
        public IEnumerable<StaffKAMLList> StaffList { get; set; }

        public StaffKAMProfileResult Result { get; set; }
    }

    public enum StaffKAMProfileResult
    {
        Success = 1,
        CannotGetStaffResult = 2
    }

    public class StaffKAMLList
    {
        public int Id { get; set; }
        public string StaffName { get; set; }
    }
}
