using DTO.EmailSend;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EmailSendingDetails
{
    public class EmailSendingDetails
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int? CustomerId { get; set; }
        public int ServiceId { get; set; }        
        public IEnumerable<int?> OfficeIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        public IEnumerable<int> CollectionIds { get; set; }
        public IEnumerable<int> BuyerIds { get; set; }
        public IEnumerable<int> ServiceTypeIds { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<int?> CustomerResultIds { get; set; }
        public IEnumerable<int> SupplierOrFactoryIds { get; set; }
        public IEnumerable<int> FactoryCountryIds { get; set; }
        public IEnumerable<int> CustomerContactIds { get; set; }
        public IEnumerable<int> ApiContactIds { get; set; }
        public IEnumerable<int> ApiDefaultContactIds { get; set; }
    }

    public class CustomerDecisionEmailData
    {
        public int? ReportId { get; set; }
        public string ReportName { get; set; }
        public int BookingId { get; set; }
        public string CustomerBookingNumber { get; set; }
        public string ServiceFrom { get; set; }
        public string ServiceTo { get; set; }
        public string ServiceType { get; set; }
        public string SupplierName { get; set; }
        public string CustomerDecision { get; set; }
        public string CustomerDecisionResult { get; set; }
        public int CustomerDecisionResultId { get; set; }
        public DateTime? CustomerDecisionDate { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int FactoryId { get; set; }
        public int FactoryCountryId { get; set; }
        public int OfficeId { get; set; }
        public int StatusId { get; set; }
        public string FactoryCountryName { get; set; }
        public IEnumerable<int> ServiceTypeIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        public IEnumerable<int> BuyerIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> CustomerContactIds { get; set; }
        public int ProductId { get; set; }
        public int BookingQuantity { get; set; }
        public string DestinationCountry { get; set; }
        public string Etd { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string PoName { get; set; }
        public string ReportResult { get; set; }

        public List<string> PoNumberList { get; set; }
        public int? CombineProductId { get; set; }
        public int? CombineAqlQuantity { get; set; }
    }

    public class CustomerDecisionEmailContainerData
    {
        public int BookingId { get; set; }
        public string PoNumber { get; set; }
        public string ContainerNumber { get; set; }
        public int? ContainerId { get; set; }
        public int TotalBookingQty { get; set; }
        public int? ReportId { get; set; }
        public string ReportLink { get; set; }
        public List<string> PoNumberList { get; set; }
        public string CustomerDecisionResult { get; set; }
        public int CustomerDecisionResultId { get; set; }
    }

    public class CustomerDecisionEmailRequest
    {
        public CustomerDecisionEmailDataItem EmailList { get; set; }
    }

    public class CustomerDecisionEmailDataItem
    {
        public List<ReportDetailsRepo> ReportData { get; set; }
        public List<CustomerDecisionEmailData> ProductData { get; set; }
        public List<CustomerDecisionEmailContainerData> ContainerData { get; set; }
    }
}
