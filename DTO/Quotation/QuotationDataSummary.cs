using DTO.Inspection;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationDataSummaryResponse
    {
        public IEnumerable<QuotationItem> Data { get; set;  }

        public QuotationDataSummaryResult Result { get; set;  }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<QuotationSummaryStatus> StatusList { get; set; }
    }

    public enum QuotationDataSummaryResult
    {
        Success  = 1, 
        NotFound = 2,
        NoBookingNo = 3
    }


    public class QuotationItem
    {
        public int QuotationId { get; set; }
        public string BookingNoCusBookingNo { get; set; }
        public string QuotationDate { get; set; }
        public string ServiceDateList { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string SupplierName { get; set;  } 
        public string FactoryName { get; set;  }
        public string ServiceType { get; set;  }
        public string Office { get; set;  }
        public int StatusId { get; set;  }
        public string StatusName { get; set;  }
        public double? discount { get; set; }
        public double EstimatedManDay { get; set; }
        public double InspectionFees { get; set; }
        public double? TravelCost { get; set; }
        public double TotalCost { get; set; }
        public QuotationExportInformation ExportAdditionalInfo { get; set; }
        public IEnumerable<QuotationAbility> Abilities { get; set; }
        public string ProductCode { get; set; }
        public string PoNo { get; set; }
        public int ServiceId { get; set; }
        public string ActualServiceDate { get; set; }
        public string BookingStatusList { get; set; }
        public bool IsAdeoClient { get; set; }
        public string BillingEntity { get; set; }
        public double OtherCost { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceRemarks { get; set; }
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
        public string ValidatedOn{ get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string CountyName { get; set; }
        public string ProvinceName { get; set; }
        public string customerRemark { get; set; }
        public List<BookingStatusColorList> BookingIdStatusColorList { get; set; }
        public string BrandName { get; set; }
        public string DepartmentName { get; set; }
        public bool? IsEAQF { get; set; }
    }

    public class QuotationExportInformation
    {
        public int quotationid { get; set; }
        public string currency { get; set; }
        public string billPaidBy { get; set; }
        public string billPaidByAddress { get; set; }
        public string billPaidByContact { get; set; }
        public string apiremark { get; set; }
        public string customerRemark { get; set; }
        public string customerLegalName { get; set; }
        public string supplierLegalName { get; set; }
        public string factoryLegalName { get; set; }
    }

    public class BookingStatusColorList
    {
        public string BookingId { get; set; }
        public string StatusColor { get; set; }
        public string StatusName { get; set; }
    }
}
