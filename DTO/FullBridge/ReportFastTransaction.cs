using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.FullBridge
{
    public class ReportFastTransactionRequest
    {
        public int ReportId { get; set; }
        public ReportFastStatus Status { get; set; }
        public int? BookingId { get; set; }
    }

    public enum ReportFastStatus
    {
        NotStarted = 1,
        InProgress = 2,
        Completed = 3,
        Error = 4,
        Cancelled = 5,
        PushedToFB=6,
    }
    public class FbReportIdDto
    {
        public int? FbReportId{ get; set; }
        public int ReportId { get; set; }
    }
}
