using System.Collections.Generic;

namespace DTO.Schedule
{
    public class ScheduleProductResponse
    {
        public IEnumerable<ScheduleProductReport> ScheduleProductsReport { get; set; }
        public ScheduleProductResponseResult Result { get; set; }
    }
    public enum ScheduleProductResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
