using System;
using System.Collections.Generic;
using Entities.Enums;

namespace DTO.Quotation
{
    public class QuotationSummaryStatus
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public string StatusColor { get; set; }

        public int TotalCount { get; set; }
    }

    public class QuotationSummaryStatusResponse
    {
        public List<QuotationSummaryStatus> QuotationStatusList { get; set; }
        public QuotationSummaryStatusResult Result { get; set; }
    }

    public class QuotationInsp
    {
        public int QuotationId { get; set; }
        public int BookingId { get; set; }
        public string CusBookingNo { get; set; }
        public int BookingStatusId { get; set; }
        public string BookingStatusName { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ServiceTypeName { get; set; }

        public bool? IsEAQF { get; set; }
    }

    public class QuotationInspProd
    {
        public int QuotationId { get; set; }
        public int BookingId { get; set; }
        public string CusBookingNo { get; set; }
        public int BookingStatusId { get; set; }
        public string BookingStatusName { get; set; }
    }
    public class QuotationInspAuditExportRepo
    {
        public int QuotationId { get; set; }
        public int BookingId { get; set; }
        public string CustomerBookingNo { get; set; }
        public int ProductRefId { get; set; }
        public string ProductReference { get; set; }
        public string ProductDescription { get; set; }
        public DateTime? FBReportStartDate { get; set; }
        public DateTime? FBReportEndDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public string CustomerLegalName { get; set; }
        public int SupplierId { get; set; }
        public double EstimatedManDay { get; set; }
        public string SupplierLegalName { get; set; }
        public string FactoryLegalName { get; set; }
        public string FactoryAddress { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string Office{ get; set; }
        public int QuotationStatusId { get; set; }
        public string QuotationStatus { get; set; }
        public string ReportResult { get; set; }
        public int BookingQty { get; set; }
        public int SampleSize { get; set; }
        public int? CombineAQL { get; set; }
        public string FactoryRef { get; set; }
        public double InspectionCost { get; set; }
        public double? Discount { get; set; }
        public double? TravelCostAir { get; set; }
        public double? TravelCostLand { get; set; }
        public double? TravelCostHotel{ get; set; }
        public double? OtherCost { get; set; }
        public double? TotalFee { get; set; }
        public string Currency { get; set; }
        public int BillPaidById { get; set; }
        public string BillPaidBy { get; set; }
        public string BillPaidByAddress { get; set; }
        public string BillPaidByContact { get; set; }
        public string APIRemark { get; set; }
        public string CustomerRemark { get; set; }
        public string PaymentTerm { get; set; }
        public string BookingStatus { get; set; }
        public string BillingEntity { get; set; }
        public DateTime? ValidatedOn { get; set; }
        public string ValidatedByName { get; set; }
        public int? ValidatedByUserType { get; set; }
        public string DepartmentName { get; set; }
        public string BrandName { get; set; }
        public string SupplierCode { get; set; }
    }


    public enum QuotationSummaryStatusResult
    {
        Success = 1,
        CannotGetList = 2,
        Fail = 3
    }
    public class QuotationItemRepoResponse
    {
        public IEnumerable<QuotationItemRepo> QuotationItemList { get; set; }
        public int TotlaCount { get; set; }
        public List<QuotationSummaryStatus> QuotationStatusList { get; set; }
        

    }
        public class QuotationItemRepo
    {
        public int QuotationId { get; set; }
        public string QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public string FactoryName { get; set; }
        public string ServiceType { get; set; }
        public string Office { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public double? discount { get; set; }
        public double EstimatedManDay { get; set; }
        public double InspectionFees { get; set; }
        public double? TravelCost { get; set; }
        public double TotalCost { get; set; }
        public int ServiceId { get; set; }
        public string BookingStatusList { get; set; }
        public bool IsAdeoClient { get; set; }
        public string BillingEntity { get; set; }
        public double OtherCost { get; set; }
        public string PaymentTerm { get; set; }
        public double? TravelDistance { get; set; }
        public double? TravelTime { get; set; }
        public string BillMethodName { get; set; }
        public string BillPaidByName { get; set; }
        public int? BillPaidById { get; set; }
        public int ProductCount { get; set; }
        public string CurrencyName { get; set; }
        public int? ValidatedBy { get; set; }
        public string ValidatedUserName { get; set; }
        public string ValidatedOn { get; set; }
        public string customerRemark { get; set; }
    }

    public enum CalculateWorkingHoursResult
    {
        Success = 1,
        Fail = 2,
        ProdCatSub3NotMapped = 3,
        UnitNotPcs = 4
    }

    public class CalculatedWorkingHoursResponse
    {
        public double CalculatedWorkingHours { get; set; }
        public double CalculatedManday { get; set; }
        public CalculateWorkingHoursResult Result { get; set; }
        public CalculateManDaySaveResult SaveResult { get; set; }
    }
    public enum CalculateManDaySaveResult
    {
        Success = 1,
        Fail = 2,
    }
    public class CalculatedWorkingHoursData
    {
        public double CalculatedWorkingHours { get; set; }
        public double CalculatedManday { get; set; }
    }
}
