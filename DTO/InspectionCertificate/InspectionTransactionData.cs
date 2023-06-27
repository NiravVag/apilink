using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class InspectionTransactionData
    {
        public int ICNo { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public int BookingNumber { get; set; }
        public int PoTransactionId { get; set; }
        public string PoNo { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ReportTitle { get; set; }
        public int? ReportStatus { get; set; }
        public string ReportStatusName { get; set; }
    }
}
