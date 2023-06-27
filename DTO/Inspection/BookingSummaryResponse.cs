using DTO.Customer;
using DTO.OfficeLocation;
using DTO.Quotation;
using DTO.Supplier;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingSummaryResponse
    {

        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public IEnumerable<Office> OfficeList { get; set; }

        public IEnumerable<InspectionStatus> StatusList { get; set; }

        public IEnumerable<QuotStatus> QuotationStatusList { get; set; }

        public IEnumerable<AECustomerList> AEList { get; set; }

        public BookingSummaryResponseResult Result { get; set; }

        public enum BookingSummaryResponseResult
        {
            success = 1,
            failed = 2
        }
    }

    public class BookingSummaryStatusResponse
    {

        public List<InspectionStatus> StatusList { get; set; }

        public List<QuotStatus> QuotationStatusList { get; set; }

        public BookingSummaryStatusResponseResult Result {get;set;}
    }

    public enum BookingSummaryStatusResponseResult
    {
        Success = 1,
        StatusListNotFound = 2,
        QuotationStatusListNotFound = 3
    }

    public class  BookingDataInfo
    {
        public int BookingNo { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int BookingStatus { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public string CountryName { get; set; }
        public string ProvinceName { get; set; }
        public string FirstProductName { get; set; }
        public string OfficeName { get; set; }
        public int BookingQty { get; set; }
    }
    public class  BookingDataInfoResponse
    {
        public BookingDataInfo Data { get; set; }
        public BookingDatainfoResult Result { get; set; }
    }
    public enum BookingDatainfoResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4,
        ServiceIdRequired = 5

    }
    public class BookingNoDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int ServiceId { get; set; }
        public int Id { get; set; }
    }
    public class BookingNoDataSourceResponse
    {
        public IEnumerable<BookingNo> DataSourceList { get; set; }
        public BookingDatainfoResult Result { get; set; }
    }
}
