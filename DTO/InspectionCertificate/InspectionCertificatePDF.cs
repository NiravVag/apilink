using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class InspectionCertificatePDF
    {
        public string ICNo { get; set; }
        public string ICTitle { get; set; }
        public int ICTitleId { get; set; }
        public string CustomerName { get; set; }
        public string BeneficiaryName { get; set; }
        public string SupplierAddress { get; set; }
        public string ApprovalDate { get; set; }
        public int Id { get; set; }
        public List<InspectionCertificateProductPDF> ProductDetails { get; set; }
        public int? FactoryCountryId { get; set; }
        public bool IsDraft { get; set; }
        public string Comment { get; set; }
        public string BuyerName { get; set; }
        public IEnumerable<EntMasterConfig> EntityMasterConfigs { get; set; }
        public int? BusinessLine { get; set; }
    }
    public class InspectionCertificateProductPDF
    {
        public string DestinationCountry { get; set; }
        public string AQL { get; set; }
        public double? Critical { get; set; }
        public double? Major { get; set; }
        public double? Minor { get; set; }
        public string PONo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
        public int BookingNumber { get; set; }
        public DateTime serviceDateFrom { get; set; }
        public DateTime serviceDateTo { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
    }
}
