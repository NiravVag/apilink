using DTO.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ScheduleJob
{
    public class ScheduleSkipMSchartEmailResponse
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<InspectionDetail> InspectionList { get; set; }
    }
    public class InspectionDetail
    {
        public int InspectionId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? FactoryId { get; set; }
        public string FactoryName { get; set; }
        public int? OfficeId { get; set; }
        public int? ServiceTypeId { get; set; }
        public string CustomerEmail { get; set; }
        public string Office { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string EntityName { get; set; }
        public string InspectionURL { get; set; }
        public string QcName { get; set; }
        public List<ProductDetail> ProductList { get; set; }
        public List<BookingCsItem> CsList { get; set; }
    }
    public class ProductDetail
    {
        public int InspectionId { get; set; }
        public int ProductId { get; set; }
        public string ProductRef { get; set; }
        public string ProductName { get; set; }
        public string ProductURL { get; set; }
    }
    public class CustomerDetail
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
