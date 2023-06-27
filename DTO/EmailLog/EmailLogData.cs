using DTO.EmailSend;
using DTO.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EmailLog
{
    public class EmailLogData
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? SourceId { get; set; }
        public string SourceName { get; set; }
        public string ToList { get; set; }
        public string Cclist { get; set; }
        public string Bcclist { get; set; }
        public int? Status { get; set; }
        public DateTime? SendOn { get; set; }
        public int? TryCount { get; set; }
        public IEnumerable<ReportBooking> BookingReportList { get; set; }
        public IEnumerable<FileResponse> FileList { get; set; }
    }


    public enum EmailStatus
    {
        NotStarted = 1,
        Success = 2,
        Failure = 3
    }
}
