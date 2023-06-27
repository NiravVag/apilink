using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionBookingExportData
    {
        public int BookingNo { get; set; }
        public string CustomerBookingNo { get; set; }
        public int? CustomerId { get; set; }
        public int? OfficeId { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public string Factory { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public DateTime? FirstServiceDateFrom { get; set; }
        public DateTime? FirstServiceDateTo { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public string Office { get; set; }
        public string PriceCategory { get; set; }
        public string Collection { get; set; }
        public string CreatedByStaff { get; set; }
        public string CreatedByCustomer { get; set; }
        public string CreatedBySupplier { get; set; }
        public string CreatedByFactory { get; set; }
        public int UserTypeId { get; set; }
        public bool? Picking { get; set; }
        public string CustomerBookingRemarks { get; set; }
        public int StatusId { get; set; }
    }

    public class InspectionProductsExportData
    {
        public int ProductRefId { get; set; }

        public int BookingId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string FactoryReference { get; set; }

        public int ContainerId { get; set; }

        public DateTime? ServiceStartDate { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public int? CombineProductId { get; set; }

        public string Barcode { get; set; }

        public bool? IsEcoPack { get; set; }

        // FB Report Details

        public int? FbReportId { get; set; }

        public string StatusName { get; set; }

        public double? InspectedQuantity { get; set; }

        public string ReportResult { get; set; }

        public string ReportStatus { get; set; }

        public string ProductRemarks { get; set; }
        public bool? IsNewProduct { get; set; }
    }

    public class InspectionQuotationExportData
    {
        public int BookingId { get; set; }
        public double? ManDay { get; set; }
        public string QuotationStatus { get; set; }
        public int QuotationStatusId { get; set; }
    }

    public class InspectionPOExportData
    {
        public string PONumber { get; set; }
        public int BookingQuantity { get; set; }
        public DateTime? ETD { get; set; }
        public string DestinationCountry { get; set; }
        public int ProductRefId { get; set; }
        public int ProductId { get; set; }
        public int? ContainerId { get; set; }
        public int? InspectionId { get; set; }
    }

    public class InspectionBookingExportItem
    {
        public int BookingNo { get; set; }

        public string CustomerBookingNo { get; set; }

        public string Customer { get; set; }

        public string Supplier { get; set; }
        public string SupplierCode { get; set; }

        public string Factory { get; set; }

        public DateTime? ApplyDate { get; set; }

        public DateTime? FirstServiceDateFrom { get; set; }

        public DateTime? FirstServiceDateTo { get; set; }

        public DateTime ConfirmServiceDateFrom { get; set; }

        public DateTime ConfirmServiceDateTo { get; set; }

        public DateTime? InspectionDateFrom { get; set; }

        public DateTime? InspectionDateTo { get; set; }

        public string ServiceType { get; set; }

        public string Status { get; set; }

        public string DestinationCountry { get; set; }

        public string ETDDate { get; set; }

        public string SRDate { get; set; }

        public double? QuotationManDay { get; set; }

        public string PoNumber { get; set; }

        public string ContainerName { get; set; }

        public string ProductRef { get; set; }

        public int? CombineId { get; set; }

        public string ProductRefDescription { get; set; }

        public double OrderQuantity { get; set; }

        public double? InspectedQuantity { get; set; }

        public string FactoryReference { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string ReportStatus { get; set; }

        public string ReportResult { get; set; }

        public string Office { get; set; }

        public string CS { get; set; }

        public string QuotationStatus { get; set; }

        public string Department { get; set; }

        public string Brand { get; set; }

        public string Buyer { get; set; }

        public string PriceCategory { get; set; }

        public string Collection { get; set; }

        public string FactoryCountry { get; set; }

        public string FactoryProvince { get; set; }

        public string FactoryCity { get; set; }

        public string BarCode { get; set; }

        public bool EchoPack { get; set; }

        public bool Picking { get; set; }

        public string CreatedBy { get; set; }
        public string CustomerBookingRemarks { get; set; }
        public string ProductRemarks { get; set; }
        public bool NewProduct { get; set; }
        public DateTime? ReportSentDate { get; set; }
        public string ReportSentTime { get; set; }
        public string HoldType { get; set; }
        public string HoldReason { get; set; }

        public string CancelReasonType { get; set; }
        public string CancelReason { get; set; }


    }

    public class InspectionExportData
    {
        public DataTable bookingList { get; set; }
    }
}
