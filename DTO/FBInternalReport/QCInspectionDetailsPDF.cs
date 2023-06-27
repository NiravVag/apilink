using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.FBInternalReport
{
    public class QCInspectionDetailsPDF
    {
        public int InspectionID { get; set; }
        public string ServiceDate { get; set; }
        public string Customer { get; set; }
        public string CustomerBookingNo { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string FactoryContact { get; set; }
        public string FactoryContactPhoneNo { get; set; }
        public string FactoryPhoneNo { get; set; }
        public string QCName { get; set; }
        public string FactoryAddress { get; set; }
        public string FactoryRegionalAddress { get; set; }
        public double NoofManDays { get; set; }
        public string Comments { get; set; }
        public int TotalNumberofReports { get; set; }
        public int TotalCombineProducts { get; set; }
        public List<QCInspectionProductDetails> ProductDetails { get; set; }
        public string BrandNames { get; set; }
        public string CollectionName { get; set; }
        public string DepartmentNames { get; set; }
        public int TotalSamplingSizeNonCombined { get; set; }
        public int TotalPickingQtyNoncombined { get; set; }
        public string ServiceType { get; set; }
        public IEnumerable<EntMasterConfig> EntityMasterConfigs { get; set; }
        public string CsNames { get; set; }
        public int? BussinessLine { get; set; }
    }

    public class QCInspectionDetailsRepo
    {
        public int InspectionID { get; set; }
        public string Customer { get; set; }
        public string CustomerBookingNo { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public IEnumerable<InspectionSupplierFactoryContacts> FactoryContacts { get; set; }
        public IEnumerable<QuQuotationInsp> QuQuotationInspMandays { get; set; }
        public IEnumerable<InspTranServiceType> InspectionServiceTypes { get; set; }
        public string FactoryContactPhoneNo { get; set; }
        public string FactoryPhoneNo { get; set; }
        public string ScheduleComments { get; set; }
        public string QCBookingComments { get; set; }
        public string FactoryAddress { get; set; }
        public string FactoryRegionalAddress { get; set; }
        public int QuotationId { get; set; }
        public IEnumerable<string> BrandNameList { get; set; }
        public IEnumerable<string> DeptNameList { get; set; }
        public string CollectionName { get; set; }
        public int TotalNonCombineAQLQuantity { get; set; }
        public string ServiceTypeName { get; set; }
        public int? BussinessLine { get; set; }
    }

    public class QCInspectionProductDetails
    {
        public string PoNumber { set; get; }
        public string ProductName { set; get; }
        public string ProductDescription { set; get; }
        public string DestinationCountry { set; get; }
        public string BarCode { set; get; }
        public string FactoryReference { set; get; }
        public string AQL { set; get; }
        public int BookingQty { set; get; }
        public int? AQLQuantity { set; get; }
        public int? CombinedAQLQuantity { set; get; }
        public int? CombineProductId { set; get; }
        public string CombineProduct { set; get; }
        public int CombineCount { set; get; }
        public bool IsParentProduct { set; get; }
        public int? Picking { set; get; }
        public string ProductRemarks { set; get; }
        public string PickingRemarks { set; get; }
        public string ProdCategory { get; set; }
        public string ProdSubCategory { get; set; }
        public string ProdSub2Category { get; set; }
        public bool? IsEcopack { get; set; }
        public string Color { get; set; }        
    }

    public enum ProductTypeEnum
    {
        Combined = 1,
        NonCombined = 2
    }
}
