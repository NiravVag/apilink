using System.Collections.Generic;

namespace DTO.Schedule
{
    public class ScheduleProductReport
    {
        public string ProductId { get; set; }
        public string ProductDesc { get; set; }
        public int PoProductId { get; set; }
        public int? combineProductId { get; set; }
        public IEnumerable<ScheduleCombineProduct> ScheduleCombineProducts { get; set; }
        public IEnumerable<int> QC { get; set; }
        public IEnumerable<int> CS { get; set; }
        public IEnumerable<StaffSchedule> QCItems { get; set; }
        public IEnumerable<StaffSchedule> CSItems { get; set; }
    }
}
