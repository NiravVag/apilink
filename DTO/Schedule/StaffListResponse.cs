using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace DTO.Schedule
{
    public class StaffListResponse
    {
        public IEnumerable<StaffSchedule> Data { get; set; }
        public StaffListResponseResult Result { get; set; }
    }

    public enum StaffListResponseResult
    {
        Success = 1,
        SaveFailed = 2,
        CannotGetUserAccount = 3
    }
}
