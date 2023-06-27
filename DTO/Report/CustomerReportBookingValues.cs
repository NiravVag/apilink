using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public class CustomerReportBookingValues
    {
        public int BookingId { get; set; }

        public string CustomerBookingNo { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public int? FactoryId { get; set; }

        public int? missionId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ProductCategory { get; set; }

        public string ServiceType { get; set; }

        public DateTime ServiceDateFrom { get; set; }

        public DateTime ServiceDateTo { get; set; }

        public string Office { get; set; }

        public int? OfficeId { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string MissionStatus { get; set; }

        public bool? IsPicking { get; set; }

        public bool? IsEAQF { get; set; }

        public int? PreviousBookingNo { get; set; }
        public int? FbReportId { get; set; }

        public InspStatus Status { get; set; }

        public int StatusPriority { get; set; }

        public IEnumerable<ReportProducts> ReportProducts { get; set; }

        public IEnumerable<SchScheduleQc> SchScheduleQcs { get; set; }
        public DateTime? ReportDate { get; set; }
        public int? BookingType { get; set; }

    }


    public class BookingPONumbers
    {
        public string PoNumber { get; set; }
        public int? ProductRefId { get; set; }
        public int? ContainerRefId { get; set; }
    }

    public class ReportProducts
    {
        public int BookingId { get; set; }
        public int BookingStatusId { get; set; }
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSubCategory2Id { get; set; }
        public string ProductSubCategory2Name { get; set; }
        public int FbReportId { get; set; }
        public int ApiReportId { get; set; }
        public int FbReportContainerId { get; set; }
        public int? CombineProductId { get; set; }
        public string ColorCode { get; set; }
        public string PONumber { get; set; }
        public int TotalBookingQuantity { get; set; }
        public int CombineProductCount { get; set; }
        public bool IsParentProduct { get; set; }
        public int? CombineAqlQuantity { get; set; }
        public int? ContainerId { get; set; }
        public IEnumerable<HrStaff> SchScheduleQcs { get; set; }
        public string Unit { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int? Aql { get; set; }
        public int? AqlQuantity { get; set; }
        public int ProductRefId { get; set; }
    }

    public class InternalReportBookingValues
    {
        public int BookingId { get; set; }

        public string CustomerBookingNo { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public int? FactoryId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ProductCategory { get; set; }

        public string ServiceType { get; set; }

        public DateTime ServiceDateFrom { get; set; }

        public DateTime ServiceDateTo { get; set; }

        public string Office { get; set; }

        public int? OfficeId { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public bool IsPicking { get; set; }

        public int? PreviousBookingNo { get; set; }
        public int? FbReportId { get; set; }

        public InspStatus Status { get; set; }

        public IEnumerable<InternalReportProducts> ReportProducts { get; set; }
    }

    public class InternalReportProducts
    {
        public int BookingId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int FbReportId { get; set; }
        public int? CombineProductId { get; set; }
        public string ColorCode { get; set; }
        public string PONumber { get; set; }
        public int CombineProductCount { get; set; }
        public bool IsParentProduct { get; set; }
        public int? CombineAqlQuantity { get; set; }
        public int? ContainerId { get; set; }
        public int FbContainerReportId { get; set; }
        public int SupplierId { get; set; }
        public string ReportTitle { get; set; }
        public string ReportSummaryLink { get; set; }
        public string ReportPath { get; set; }
        public string FinalReportManualPath { get; set; }
        public string ProductImageUrl { get; set; }
        public DateTime? ServiceDateFrom { get; set; }
        public DateTime? ServiceDateTo { get; set; }
        public int? ReportStatusId { get; set; }
        public int? BookingStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ProductSubCategory2Name { get; set; }
    }

    public class BookingReportSummaryLinkRepo
    {
        public int BookingId { get; set; }
        public string ReportSummaryLink { get; set; }
    }

}
