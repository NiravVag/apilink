using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InvoicePreview
{
    public class InvoicePreview
    {

    }
    public class DataCommon
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class InvoicePreviewReportResult
    {
        public int? InspectionId { get; set; }
        public int? ProductRefId { get; set; }
        public int POId { get; set; }
        public string Result { get; set; }
    }
}
