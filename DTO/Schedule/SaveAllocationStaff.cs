using System;
using System.Collections.Generic;

namespace DTO.Schedule
{
    public class SaveAllocationStaff
    {
        public DateTime ServiceDate { get; set; }
        public double ActualManDay { get; set; }
        public IEnumerable<StaffSchedule> QC { get; set; }
        public IEnumerable<StaffSchedule> AdditionalQC { get; set; }
        public IEnumerable<StaffSchedule> CS { get; set; }
        public bool IsQcVisibility { get; set; }
        public IEnumerable<QcAutoExpense> QcAutoExpenses { get; set; }
    }
}
