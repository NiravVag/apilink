using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class ScheduleBookingDetailsRepoItem
    {
        public int InspectionId { get; set; }
        public string CustomerName { get; set; }
        public string FactoryName { get; set; }
        public string SupplierName { get; set; }
        public int? BusinessLine { get; set; }
    }

    public class ScheduleJobCsEmail
    {
        public string CSName { get; set; }
        public string CSEmail { get; set; }
        public string Entity { get; set; }
        public string ScheduleDate { get; set; }
        public bool IsAnySoftLineItems { get; set; }
        public List<ScheduleJobCsInspectionDetails> Inspections { get; set; }
    }

    public class ScheduleJobCsInspectionDetails
    {
        public int InspectionId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ProductRef { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
        public string ProductName { get; set; }
        public string ServiceType { get; set; }
        public string QcNames { get; set; }
        public string ScheduleDate { get; set; }
        public int? BusinessLine { get; set; }
        public string PoNumber { get; set; }
        public string ReportNo { get; set; }
        public string FillingStatus { get; set; }
        public string ReviewStatus { get; set; }
    }
}
