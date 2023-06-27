using DTO.Common;
using System;
using System.Collections.Generic;

namespace DTO.InspectionCertificate
{
    public class InspectionCertificateRequest
    {
        public int CustomerId { get; set; }
        public DateObject ApprovalDate { get; set; }
        public int ICTitleId { get; set; }
        public int SupplierId { get; set; }
        public string BeneficiaryName { get; set; }
        public string SupplierAddress { get; set; }
        public string ICNo { get; set; }
        public int Id { get; set; }
        public int ICStatus { get; set; }
        public string ICStatusName { get; set; }
        public string Comment { get; set; }
        public List<InspectionCertificateBookingRequest> ICBookingList { get; set; }
        public string BuyerName { get; set; }
    }
    public class EditInspectionCertificateResponse
    {
        public InspectionCertificateRequest editInspectionCertificate { get; set; }
        public InspectionCertificateResult Result { get; set; }
    }

    public class InspectionCertificateResponse
    {
        public int id { get; set; }
        public InspectionCertificateResult Result { get; set; }
    }
    public enum InspectionCertificateResult
    {
        Success = 1,
        Failure = 2,
        RequestNotCorrectFormat = 3,
        ICNoNotInserted = 4,
        NoDataFound = 5
    }
}
