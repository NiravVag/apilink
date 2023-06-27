using DTO.CommonClass;
using DTO.Customer;
using DTO.DynamicFields;
using DTO.HumanResource;
using DTO.Inspection;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.Kpi;
using DTO.Quotation;
using DTO.Report;
using DTO.Schedule;
using DTO.Supplier;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO.KPI
{
    public class KPIMapParameters
    {
        public KpiBookingInvoiceResponse BookingInvoice { get; set; }

        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<BookingCustomerDepartment> CustomerDept { get; set; }
        public List<SupplierCode> SupplierCode { get; set; }
        public List<CommonDataSource> QcNames { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public KPIQuotDetails QuotationDetails { get; set; }
        public List<Reinspection> ReInspectionList { get; set; }
        public List<FbReportRemarks> ReportRemarks { get; set; }
        public List<KPIMerchandiser> MerchandiserList { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<BookingShipment> FbBookingQuantity { get; set; }
        public List<BookingContacts> ContactData { get; set; }
        public List<BookingContacts> CustomerContactData { get; set; }
        public List<InspectionBookingDFData> BookingDFDataList { get; set; }
        public List<BookingCustomerBuyer> CustomerBuyerList { get; set; }
        public List<FBReportDefects> FBReportDefectsList { get; set; }
        public List<FBReportInspSubSummary> FBReportInspSubSummaryList { get; set; }
        public List<KPIMerchandiser> CustomerBrandList { get; set; }
        public List<FBOtherInformation> FbReportComments { get; set; }
        public List<BookingContainerItem> ContainerItems { get; set; }
        public List<KpiReportBatteryItem> ReportBatteryData { get; set; }
        public List<KpiReportPackingItem> ReportPackingData { get; set; }
        public List<RescheduleData> RescheduleList { get; set; }
        public List<FbPackingDimention> FBDimentionList { get; set; }
        public List<FbPackingWeight> FBWeightList { get; set; }
        public List<CommonIdDate> CustomerDecisionData { get; set; }
        public List<KpiExtraFeeData> ExtraFeeData { get; set; }
        public List<InspectionPickingReport> InspectionPickingList { get; set; }
        public List<ParentDataSource> CustomerProductCategoryList { get; set; }
        public List<InspectionPOColorTransaction> POColorList { get; set; }
        public List<ParentDataSource> OfficeCountryList { get; set; }
        public List<BilledContactsName> BilledContactList { get; set; }
        public List<CustomerDecisionData> CustomerDecision { get; set; }
        public string EntityName { get; set; }
        public IQueryable<InspTranStatusLog> BookingStatusLogs { get; set; }
        public List<ScheduleStaffItem> ScheduleQcList { get; set; }
        public List<ExpencesClaimsItem> ExpenseData { get; set; }
        public List<SupplierGradeRepo> SupplierGrades { get; set; }
        public List<SupplierGradeRepo> FactoryGrades { get; set; }        
    }

    public class KPIExpenseMapParameters
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<KpiBookingDepartment> CustomerDept { get; set; }
        public List<CommonDataSource> QcNames { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public KPIExpenseQuotDetails QuotationDetails { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<KpiBookingContact> CustomerContactData { get; set; }
        public List<KpiBookingBuyer> CustomerBuyerList { get; set; }
        public List<KPIMerchandiser> CustomerBrandList { get; set; }
        public List<BookingContainerItem> ContainerItems { get; set; }
        public List<CommonIdDate> CustomerDecisionData { get; set; }
        public List<KpiExtraFeeData> ExtraFeeData { get; set; }
        public List<InvoiceBookingData> InvoiceBookingData { get; set; }
        public List<KPIMerchandiser> MerchandiserList { get; set; }
    }

    public class KPIReportResultMapParameters
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<KpiBookingDepartment> CustomerDept { get; set; }
        public List<CommonDataSource> QcNames { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public KPIExpenseQuotDetails QuotationDetails { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<KpiBookingContact> CustomerContactData { get; set; }
        public List<KpiBookingBuyer> CustomerBuyerList { get; set; }
        public List<KPIMerchandiser> CustomerBrandList { get; set; }
        public List<BookingContainerItem> ContainerItems { get; set; }
        public List<CommonIdDate> CustomerDecisionData { get; set; }
        public List<InvoiceBookingData> InvoiceBookingData { get; set; }
        public List<KPIMerchandiser> MerchandiserList { get; set; }
        public List<FBReportInspSubSummary> FBReportInspSubSummaryList { get; set; }
    }

    public class ECIRemarkParameters
    {
        public KpiBookingProductsData ProductItem { get; set; }
        public KpiInspectionBookingItems BookingDetails { get; set; }
        public ClientQuotationItem QuotationDetails { get; set; }
        public int? TotalReports { get; set; }
        public int RemarkSerialNo { get; set; }
        public string RemarkResult { get; set; }
        public string Remarks { get; set; }
        public int InspSummaryId { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public string RemarkSubCategory2 { get; set; }
        public string RemarkSubCategory { get; set; }
        public string CustomerRemarkCode { get; set; }
    }

    public class DefectParameters
    {
        public KpiBookingProductsData ProductItem { get; set; }
        public KpiInspectionBookingItems BookingDetails { get; set; }
        public FBReportDefects DefectData { get; set; }
        public int DefectSerialNo { get; set; }
    }

    public class ResultParameters
    {
        public List<KpiBookingProductsData> ProductItem { get; set; }
        public List<BookingContainerItem> ContainerItems { get; set; }
        public KpiInspectionBookingItems BookingDetails { get; set; }
        public List<FbReportInspSummaryResult> ReportResultData { get; set; }
        public ClientQuotationItem QuotationDetails { get; set; }
        public KPIManday QuotationMandayDetails { get; set; }
        public int PrevQuotationId { get; set; }
        public double OtherCost { get; set; }
        public double Discount { get; set; }
        public List<KpiExtraFeeData> ExtraFeeDetails { get; set; }
    }

    public class KpiExpenseResultParameters
    {
        public List<KpiBookingProductsData> ProductItem { get; set; }
        public List<BookingContainerItem> ContainerItems { get; set; }
        public KpiInspectionBookingItems BookingDetails { get; set; }
        public List<FbReportInspSummaryResult> ReportResultData { get; set; }
        public ExpenseClientQuotationItem QuotationDetails { get; set; }
        public KPIManday QuotationMandayDetails { get; set; }
        public int PrevQuotationId { get; set; }
        public double OtherCost { get; set; }
        public double Discount { get; set; }
        public List<KpiExtraFeeData> ExtraFeeDetails { get; set; }


    }

    public class KpiCarrefourInvoiceParameters
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public List<InvoiceBookingData> InvoiceBookingData { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public List<CarrefourQuoationDetails> QuotationDetails { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<RescheduleData> RescheduleList { get; set; }
        public List<FBOtherInformation> FbReportComments { get; set; }
        public List<FBReportDefects> FBReportDefectsList { get; set; }
        public List<FbReportRemarks> ReportRemarks { get; set; }
        public List<BookingShipment> FbBookingQuantity { get; set; }
        public List<FbPackingWeight> FBWeightList { get; set; }
        public List<FbPackingDimention> FBDimentionList { get; set; }
        public List<CommonDataSource> QcNames { get; set; }
        public List<SupplierCode> SupplierCode { get; set; }
        public List<FBReportInspSubSummary> FBReportInspSubSummaryList { get; set; }
        public List<KpiExtraFeeData> ExtraFeeData { get; set; }
    }

    public class KpiGeneralInvoiceParameters
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public List<KpiBookingContact> CustomerContactData { get; set; }
        public List<KpiBookingDepartment> CustomerDept { get; set; }
        public List<KPIMerchandiser> CustomerBrandList { get; set; }
        public List<KPIMerchandiser> CustomerMerchandiserList { get; set; }
        public List<BookingCustomerBuyer> CustomerBuyerList { get; set; }
        public List<InvoiceBookingData> InvoiceBookingData { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public KPIExpenseQuotDetails QuotationDetails { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<RescheduleData> RescheduleList { get; set; }
        public List<FBOtherInformation> FbReportComments { get; set; }
        public List<FBReportDefects> FBReportDefectsList { get; set; }
        public List<FbReportRemarks> ReportRemarks { get; set; }
        public List<BookingShipment> FbBookingQuantity { get; set; }
        public List<FbPackingWeight> FBWeightList { get; set; }
        public List<FbPackingDimention> FBDimentionList { get; set; }
        public List<CommonDataSource> QcNames { get; set; }
        public List<SupplierCode> SupplierCode { get; set; }
        public List<FBReportInspSubSummary> FBReportInspSubSummaryList { get; set; }
        public List<KpiExtraFeeData> ExtraFeeData { get; set; }
    }

    public class KpiCarreFourECOPackParameters
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public List<KpiBookingDepartment> CustomerDept { get; set; }
        public List<KpiReportBatteryItem> ReportBatteryData { get; set; }
        public List<KpiReportPackingItem> ReportPackingData { get; set; }

        public List<KPIMerchandiser> CustomerBrandList { get; set; }
        public List<BookingCustomerBuyer> CustomerBuyerList { get; set; }
        public List<InvoiceBookingData> InvoiceBookingData { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public KPIExpenseQuotDetails QuotationDetails { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<RescheduleData> RescheduleList { get; set; }
        public List<FBOtherInformation> FbReportComments { get; set; }
        public List<FBReportDefects> FBReportDefectsList { get; set; }
        public List<FbReportRemarks> ReportRemarks { get; set; }
        public List<BookingShipment> FbBookingQuantity { get; set; }
        public List<FbPackingWeight> FBWeightList { get; set; }
        public List<FbPackingDimention> FBDimentionList { get; set; }
        public List<CommonDataSource> QcNames { get; set; }
        public List<SupplierCode> SupplierCode { get; set; }
        public List<FBReportInspSubSummary> FBReportInspSubSummaryList { get; set; }
        public List<KpiExtraFeeData> ExtraFeeData { get; set; }
    }

    public class ECITemplateParameters
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public IEnumerable<ServiceTypeList> ServiceTypeList { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<KpiBookingProductsData> ProductList { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<FBReportDefects> FBReportDefectsList { get; set; }
        public List<KPIMerchandiser> CustomerMerchandiserList { get; set; }
        public List<InspectionBookingDFData> BookingDFDataList { get; set; }
        public List<FBReportInspSubSummary> FBReportInspSubSummaryList { get; set; }
        public List<FbReportRemarks> ReportRemarks { get; set; }
        public List<BookingShipment> FbBookingQuantity { get; set; }
        public List<KpiBookingDepartment> CustomerDept { get; set; }
        public List<KpiBookingBuyer> CustomerBuyerList { get; set; }
    }
}
