using DTO.CommonClass;
using DTO.Invoice;
using DTO.Kpi;
using DTO.Quotation;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IKpiCustomRepository
    {
        IQueryable<RefKpiTeamplate> GetTemplateList();
        IQueryable<KpiInspectionBookingItems> GetAllInspections();
        Task<List<KpiPoDetails>> GetBookingPOTransactionDetails(List<int> bookingId);
        Task<List<KpiBookingProductsData>> GetProductListByBooking(IEnumerable<int> bookingId);
        Task<List<KpiBookingProductsData>> GetContainerListByBooking(IEnumerable<int> bookingId);
        Task<KPIQuotDetails> GetClientQuotationByBooking(List<int> bookingIds);
        Task<List<BookingShipment>> GetInspectionQuantities(List<int> bookingIds);

        Task<List<BookingShipment>> GetInspectionQuantities(IQueryable<int> bookingIds);

        IQueryable<CuCustomer> GetCustomersItems();
        Task<List<QuTranStatusLog>> GetQuotationStatusLogById(List<int> bookingIds);
        Task<List<KPIMerchandiser>> GetMerchandiserByBooking(List<int> bookingIds);
        Task<List<QuotationManday>> GetQuotationManDay(List<int> bookingIds);
        Task<List<CommonDataSource>> GetFbQcNames(List<int> reportIdList);

        Task<List<CommonDataSource>> GetFbQcNames(IQueryable<int> bookingIds);

        Task<List<BookingContainerItem>> GetContainerItemsByReportId(List<int> BookingIdList);

        /// <summary>
        /// get inspection, quotation, report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>export the data</returns>
        Task<List<BookingCustomerBuyer>> GetCustomerBuyerbyBooking(List<int> bookingIds);
        Task<List<CommonIdDate>> GetDecisionDateByReport(IQueryable<int> bookingIdList);
        Task<List<CommonIdDate>> GetDecisionDateByReport(List<int> bookingIdList);
        Task<List<CommonIdDate>> GetICByReport(IQueryable<int> bookingIdList);
        Task<List<CommonIdDate>> GetICByReport(List<int> bookingIdList);
        Task<List<HrHolidayData>> GetHolidaysByDateRange(DateTime startdate, DateTime enddate);
        Task<List<BookingCustomerBuyer>> GetCustomerBuyerbyBookingQuery(IQueryable<int> bookingIds);
        //Task<List<CommonIdDate>> GetDecisionDateByReport(List<int> reportIds);
        // Task<List<CommonIdDate>> GetICByReport(List<int> reportIds);
        Task<List<HrHoliday>> GetHolidaysByRange(DateTime startdate, DateTime enddate);
        Task<List<Reinspection>> GetReinspectionBooking(List<int> bookingIds);
        Task<List<FbReportRemarks>> GetFbProblematicRemarks(List<int> reportIds);

        Task<List<FbReportRemarks>> GetFbProblematicRemarks(IQueryable<int> bookingIds);

        Task<List<KpiBookingProductsData>> GetFailedProductListByBooking(IEnumerable<int> bookingId);
        /// <summary>
        /// get fb other information list
        /// </summary>
        /// <param name="fbReportIds"></param>
        /// <returns></returns>
        Task<List<FBOtherInformation>> GetFBOtherInformationList(List<int> fbReportIds);

        Task<List<FBOtherInformation>> GetFBOtherInformationList(IQueryable<int> bookingIds);

        /// <summary>
        /// get fb sample type list
        /// </summary>
        /// <param name="fbReportIds"></param>
        /// <returns></returns>
        Task<List<FBSampleType>> GetFBSampleTypeList(List<int> fbReportIds);
        /// <summary>
        /// get fb defects list
        /// </summary>
        /// <param name="poIdList"></param>
        /// <returns></returns>
        Task<List<FBReportDefects>> GetFBDefects(IEnumerable<int> poIdList);

        Task<List<FBReportDefects>> GetFBDefects(IQueryable<int> bookingIds);

        /// <summary>
        /// get fb insp summary result list
        /// </summary>
        /// <param name="fbReportIdList"></param>
        /// <returns></returns>
        Task<List<FBReportInspSubSummary>> GetFBInspSummaryResult(IEnumerable<int> fbReportIdList);
        Task<List<BookingContacts>> GetContactNames(List<int> bookingIds);
        Task<List<BookingContacts>> GetCustomerContactNames(List<int> bookingIds);
        Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReport(IEnumerable<int> fbReportIdList);

        Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReport(IQueryable<int> bookingIds);

        Task<List<KPIMerchandiser>> GetCustomerBrandbyBooking(List<int> bookingIds);
        Task<List<KPIMerchandiser>> GetCustomerBrandbyBookingQuery(IQueryable<int> bookingIds);
        Task<List<FBOtherInformation>> GetFBReportComments(List<int> fbReportIds);
        Task<List<KpiBookingProductsData>> GetProductListForEcoPack(List<int> bookingId);
        Task<List<KpiBookingProductsData>> GetProductListForEcoPack(IQueryable<int> bookingIds);
        Task<List<KpiReportBatteryItem>> GetReportBatteryInfo(List<int> reportIdList);

        Task<List<KpiReportBatteryItem>> GetReportBatteryInfo(IQueryable<int> bookingIds);

        Task<List<KpiReportPackingItem>> GetReportPackingInfo(List<int> reportIdList);

        Task<List<KpiReportPackingItem>> GetReportPackingInfo(IQueryable<int> bookingIds);
        /// <summary>
        /// get fb report product weight details
        /// </summary>
        /// <param name="fbReportIds"></param>
        /// <returns></returns>
        Task<List<FbPackingWeight>> GetFBReportWeightDetails(IEnumerable<int> fbReportIds);

        Task<List<FbPackingWeight>> GetFBReportWeightDetails(IQueryable<int> bookingIds);

        /// <summary>
        /// get fb report dimention details
        /// </summary>
        /// <param name="fbReportIds"></param>
        /// <returns></returns>
        Task<List<FbPackingDimention>> GetFBReportDimentionDetails(IEnumerable<int> fbReportIds);

        Task<List<FbPackingDimention>> GetFBReportDimentionDetails(IQueryable<int> bookingIds);

        /// <summary>
        /// get reschedule booking details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<RescheduleData>> GetRescheduleBookingDetails(IEnumerable<int> bookingIds);

        Task<List<RescheduleData>> GetRescheduleBookingDetails(IQueryable<int> bookingIds);

        Task<List<KpiInvoiceData>> GetInvoiceNoByBooking(List<int> bookingIds);

        Task<List<KpiExtraFeeData>> GetExtraFeeByBooking(List<int> bookingIds);

        Task<List<KpiExtraFeeData>> GetExtraFeeByBooking(IQueryable<int> bookingIds);

        IQueryable<InvExfTransaction> GetExtraFeeByBookingQuery(IQueryable<int> bookingIds);

        Task<List<KpiExtraFeeData>> GetExtraFeeByInvoiceNo(string invoiceNo);

        Task<List<MdmDefectData>> GetMDMDefectData(KpiDbRequest request);

        IQueryable<InspectionPicking> GetInspectionPickingData();

        Task<List<LabDetails>> GetLabDetails(List<int> labIds);

        IQueryable<InspTransaction> GetAllInspectionQuery();

        Task<Tuple<List<KpiDefectDataRepo>, List<KpiBookingDepartment>, List<KpiBookingBuyer>, List<KpiBookingContact>>> GetDefectSummaryData(KpiBookingSPRequest request);

        Task<List<KpiDefectDataRepo>> GetDefectSummaryDataEfCore(IQueryable<int> bookingIdList);

        Task<List<KPIDefectPurchaseOrderRepo>> GetKpiDefectPurchaseOrderData(IQueryable<int> bookingIdList);

        Task<List<KpiBookingBuyer>> GetKPIBookingBuyerDataEfCore(IQueryable<int> bookingIdList);

        Task<List<KpiBookingDepartment>> GetKPIBookingDepartmentDataEfCore(IQueryable<int> bookingIdList);

        Task<List<KpiBookingContact>> GetKPIBookingCustomerContactsDataEfCore(IQueryable<int> bookingIdList);

        Task<List<KpiReportRemarksTemplateRepo>> GetReportRemarksDataEfCore(IQueryable<int> bookingIdList);

        Task<List<KpiPoDetails>> GetBookingPoDetails(IQueryable<int> bookingIds);

        Task<List<KpiBookingProductsData>> GetProductListByBooking(IQueryable<int> bookingId);

        Task<List<KpiBookingProductsData>> GetContainerListByBooking(IQueryable<int> bookingId);

        Task<List<BookingContainerItem>> GetContainerItemsByReportId(IQueryable<int> BookingIdList);

        Task<List<KpiInvoiceData>> GetInvoiceNoByBooking(IQueryable<int> bookingIds);

        Task<List<KpiInspectionBookingItems>> GetBookingItemsbyBookingIdAsQuery(IQueryable<int> bookingIds);

        Task<List<KpiInspectionBookingItems>> GetBookingEcoPackbyBookingQuery(IQueryable<int> bookingIds);

        Task<KPIExpenseQuotDetails> GetClientQuotationByBooking(IQueryable<int> bookingIds);

        Task<List<CarrefourQuoationDetails>> GetQuotationByBookings(IQueryable<int> bookingIds);

        Task<List<KPIMerchandiser>> GetMerchandiserByBooking(IQueryable<int> bookingIds);

        Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFollowUpProductDataEfCore(IQueryable<int> bookingIdList);

        Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFollowUpContainerDataEfCore(IQueryable<int> bookingIdList);

        Task<List<FbReportRemarks>> GetFbProblematicRemarksEfCore(IQueryable<int> bookingIdList);

        Task<List<Reinspection>> GetReinspectionBookingByBookingQuey(IQueryable<int> bookingIds);

        Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFailedProductDataByEfCore(IQueryable<int> bookingIdList);

        Task<List<KpiAdeoFollowUpDataRepo>> GetAdeoFailedContainerDataByEfCore(IQueryable<int> bookingIdList);

        Task<List<QuotationManday>> GetQuotationManDay(IQueryable<int> bookingIds);

        Task<List<KpiInvoiceData>> GetInvoiceDetailsByInvoiceNo(string invoiceNo);

        Task<List<KpiReportCommentsTemplateRepo>> GetReportCommentsDataEfCore(IQueryable<int> bookingIdList);

        Task<List<CustomerDecisionData>> GetCustomerDecisionData(List<int> bookingIds);

        Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReportIds(IQueryable<int> reportIds);
        Task<List<FBReportDefects>> GetFBDefectsByReportIds(IEnumerable<int> reportIds);
        Task<List<BookingShipment>> GetInspectionQuantitiesByReportIds(IQueryable<int> reportIds);
        Task<List<KpiDefectInspectionRepo>> GetKpiDefectInspectionData(IQueryable<int> bookingIds);
        Task<List<ExpencesClaimsItem>> GetBookingExpense(List<int> bookingIds);
        Task<List<InspectionProductCountDto>> GetProductsCountByInspectionIds(IEnumerable<int> bookingIds);
        Task<List<InspectionQcKpiInvoiceDetails>> GetQcKpiBookingInvoiceDetails(IQueryable<int> bookingIds);
        Task<List<InspectionQcKpiExpenseDetails>> GetQcKpiBookingExpenseDetails(IQueryable<int> bookingIds);
        Task<List<CommonIdDate>> GetDecisionDateByReportId(List<int> reportIdlist);
        Task<List<XeroInvoiceData>> GetXeroxInvoiceFirstSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetXeroxInvoiceSecondSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetXeroxInvoiceThirdSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetXeroxInvoiceFourthSetItems(IQueryable<int> bookingIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetXeroInvoiceFifthSetItems(IQueryable<int> bookingIds);
        Task<List<GapCustomerKpiReportData>> GetGapKpiReportData(IQueryable<int> bookingIds);
        Task<List<XeroInvoiceData>> GetXeroExpenseItems(IQueryable<int> bookingIds);
        Task<List<InvoiceCommunication>> GetInvoiceCommunicationByInvoiceNo(List<string> invoiceNoList);

        IQueryable<AudTransaction> GetAllAuditQuery();
        Task<List<KpiAuditBookingItems>> GetAuditItemsbyAuditIdAsQuery(IQueryable<int> auditIds);
        IQueryable<InvAutTranDetail> GetInvoiceDetailsQueryable(IQueryable<int> bookingIds);
        Task<List<XeroInvoiceData>> GetAuditXeroInvoiceFirstSetItems(IQueryable<int> auditIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetAuditXeroxInvoiceSecondSetItems(IQueryable<int> auditIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetAuditXeroxInvoiceThirdSetItems(IQueryable<int> auditIds, List<int> invoiceTypeList);
        Task<List<XeroInvoiceData>> GetAuditXeroInvoiceFifthSetItems(IQueryable<int> auditIds);
        Task<List<XeroInvoiceData>> GetAuditXeroExpenseItems(IQueryable<int> auditIds);
        Task<List<XeroInvoiceData>> GetNonInspectionXeroExpenseItems(KpiRequest request);
    }
}
