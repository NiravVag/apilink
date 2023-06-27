using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Report
{
    public class UploadCustomReportRequest
    {
        public int ApiReportId { get; set; }
        public string FileUrl { get; set; }
        public string UniqueId { get; set; }
        public string FileName { get; set; }
    }

    public class UploadCustomReportResponse
    {
        public UploadCustomReportResult Result { get; set; }
    }

    public enum UploadCustomReportResult
    {
        Success=1,
        NotSaved=2
    }

    public class ReportFilePath
    {
        public string FinalReportPath { get; set; }
        public string FinalManualReportPath { get; set; }
    }

    public class ReportFileResponse
    {
        public string FileUrl { get; set; }
        public ReportFileResult Result { get; set; }
    }

    public enum ReportFileResult
    {
        Success=1,
        NotFound=2
    }
}
