using DTO.CommonClass;
using DTO.MobileApp;
using DTO.Quotation;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IQuotationRepository : IRepository
    {
        /// <summary>
        /// BillmethodList
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<QuBillMethod>> GetBillMethodList();

        /// <summary>
        /// PaidByList
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<QuPaidBy>> GetPaidByList();


        /// <summary>
        /// Get inspection list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        Task<IEnumerable<InspTransaction>> GetInspectionList(FilterOrderRequest request);

        /// <summary>
        /// Get Audit List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<AudTransaction>> GetAuditList(FilterOrderRequest request);

        /// <summary>
        /// Get quotation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuQuotation> GetQuotation(int id);

        /// <summary>
        /// GetInspectionListByBooking
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<IEnumerable<InspTransaction>> GetInspectionListByBooking(IEnumerable<int> idList);

        /// <summary>
        /// GetAuditListByBooking
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<IEnumerable<AudTransaction>> GetAuditListByBooking(IEnumerable<int> idList);


        /// <summary>
        /// Set status
        /// </summary>
        /// <param name="objStatusRequest"></param>
        /// <returns>success value</returns>
        Task<bool> SetStatus(SetStatusRequest objStatusRequest);

        Task<IEnumerable<QuotationInsp>> GetQuotationInspList(List<int> quIds);
        Task<IEnumerable<QuotationInvoiceItem>> GetQuotationAuditandInvoice(List<int> quIds);
        Task<IEnumerable<QuotationInvoiceItem>> GetQuotationAuditDetails(IQueryable<int> quIds);
        Task<IEnumerable<QuotationAuditReportItem>> GetQuotationAuditReportDetails(IQueryable<int> BookIds);
        Task<IEnumerable<QuInspProduct>> GetQuotationInspProdList(List<int> quIds);
        IQueryable<QuInspProduct> GetQuotationInspProductList(QuotationSummaryRepoRequest request);
        IQueryable<QuQuotationAudit> GetQuotationAuditList(QuotationSummaryRepoRequest request);

        Task<IEnumerable<QuotationInspAuditExportRepo>> GetQuotationInspProductExport(IQueryable<int> quIds);
        Task<IEnumerable<QuotationInspAuditExportRepo>> GetQuotationAuditExport(IQueryable<int> quIds);


        /// <summary>
        /// GetQuotationItemByBookingAndAudit
        /// </summary>
        /// <param name="QuIds"></param>
        /// <returns></returns>
        IQueryable<QuotationItemRepo> GetQuotationItemByBookingAndAudit(IQueryable<int> QuIds);

        /// <summary>
        /// Get status List
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<QuStatus>> GetStatusList();
        /// <summary>
        /// Get minor, major, critical values from insp po table top 1 using quotation id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>minor, major, critical values from insp po table top 1</returns>
        Task<QuQuotation> GetBookingPoDetails(int id);
        /// <summary>
        /// Get inspection Quotation manday details
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns>Get Quotation manday</returns>
        Task<IEnumerable<QuQuotationInspManday>> GetQuotationInspManday(int quotationId);

        /// <summary>
        /// Get Audit Quotation manday details
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns>Get Quotation manday</returns>
        Task<IEnumerable<QuQuotationAudManday>> GetQuotationAudManday(int quotationId);

        /// <summary>
        /// Get quotation additional information
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns>Get Quotation details</returns>
        Task<List<QuotationExportInformation>> GetQuotationAdditionalInfo(List<int> quotids);

        /// <summary>
        /// get quotation exist for insp booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>bool value true if exists</returns>
        Task<bool> QuotationInspExists(int bookingId);

        /// <summary>
        /// get quotation id by audit id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>quotation id</returns>

        Task<int> GetQuotationIdByAuditid(int AuditbookingId);

        /// <summary>
        /// get quotation inspection manday with service date
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>get list of exist service date records for the quotation</returns>
        Task<IEnumerable<QuQuotationInspManday>> GetQuotationInspManDay(int bookingId);
        /// <summary>
        /// select customer email contact enable in quotation
        /// </summary>
        /// <param name="quotationId"></param>
        /// <param name="statusIdList"></param>
        /// <returns>select customer email contact enable in quotation</returns>
        Task<IEnumerable<User>> CustomerEmailIdQuotation(int quotationId, int[] statusIdList);

        /// <summary>
        /// select factory email contact enable in quotation
        /// </summary>
        /// <param name="quotationId"></param>
        /// <param name="statusIdList"></param>
        /// <returns>select factory email contact enable in quotation</returns>
        Task<IEnumerable<User>> FactoryEmailIdQuotation(int quotationId, int[] statusIdList);

        /// <summary>
        /// select supplier email contact enable in quotation
        /// </summary>
        /// <param name="quotationId"></param>
        /// <param name="statusIdList"></param>
        /// <returns>select supplier email contact enable in quotation</returns>
        Task<IEnumerable<User>> SupplierEmailIdQuotation(int quotationId, int[] statusIdList);

        /// <summary>
        /// select internal user email contact enable in quotation
        /// </summary>
        /// <param name="quotationId"></param>
        /// <param name="statusIdList"></param>
        /// <returns>select internal user email contact enable in quotation</returns>
        Task<IEnumerable<User>> InternalUserEmailIdQuotation(int quotationId, int[] statusIdList);

        /// <summary>
        /// audit quotation exists which is not canceled return the audit ids
        /// </summary>
        /// <param name="auditIds"></param>
        /// <param name="quotationId"></param>
        /// <returns> audit quotation exists return the audit ids</returns>
        Task<IEnumerable<int>> IsAuditQuotationExists(IEnumerable<int> auditIds, int quotationId);

        /// <summary>
        /// booking quotation exists which is not canceled return the booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="quotationId"></param>
        /// <returns> booking quotation exists return the booking ids</returns>
        Task<IEnumerable<int>> IsBookingQuotationExists(IEnumerable<int> bookingIds, int quotationId);

        /// <summary>
        /// fetch adeo client quotation details
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns> booking quotation exists return the quotation details</returns>
        Task<ClientQuotationItem> GetClientQuotation(int quotationId);

        /// <summary>
        /// Get quotation details for export to excel
        /// </summary>
        /// <returns></returns>
        IQueryable<QuotationExportRepo> GetAllQuotations();

        /// <summary>
        /// Get Quotation Booking Products by bookingids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<QuotationBookingProductRepo>> GetQuotationBookingProductsByBookingIds(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get Bookings details involved in the quotations
        /// </summary>
        /// <param name="quotationIds"></param>
        /// <returns></returns>
        Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByQuotationIds(IEnumerable<int> quotationIds);

        /// <summary>
        /// Get Quotation Bookings by booking service start date and service end date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByServiceDate(DateTime startDate, DateTime endDate, int? bookingNo);

        /// <summary>
        /// Get Quotation Booking Details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<QuotationBookingRepo>> GetQuotationBookingDetailsByBookingIds(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get quotation customercontacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuotationBookingContactRepo>> GetQuotationCustomerContactsById(List<int> lstid);

        /// <summary>
        /// Get quotation customercontacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Tuple<int, string>>> GetQuotationCustomerContactsByIds(IQueryable<int> lstid);
        /// <summary>
        /// Get quotation suppliercontacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuotationBookingContactRepo>> GetQuotationSupplierContactsById(List<int> lstid);

        /// <summary>
        /// Get quotation suppliercontacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Tuple<int, string>>> GetQuotationSupplierContactsByIds(IQueryable<int> lstid);
        /// <summary>
        /// Get quotation factorycontacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuotationBookingContactRepo>> GetQuotationFactoryContactsById(List<int> lstid);
        /// <summary>
        /// Get quotation factorycontacts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Tuple<int, string>>> GetQuotationFactoryContactsByIds(IQueryable<int> lstid);

        /// <summary>
        /// Billing Entities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CommonDataSource>> GetBillingEntities();

        /// <summary>
        /// Quotation Insp
        /// </summary>
        /// <returns></returns>
        Task<List<QuotationInvoiceItem>> GetquotationInsp(List<int> quotationId);
        /// <summary>
        /// Quotation Invoice
        /// </summary>
        /// <returns></returns>
        Task<List<QuotationInvoiceItem>> GetquotationInvoiceList(IQueryable<int> quotationIds);

        /// <summary>
        /// Quotation Audit
        /// </summary>
        /// <returns></returns>
        Task<List<QuotationInvoiceItem>> GetquotationAudit(List<int> quotationId);

        /// <summary>
        /// Quotation Insp
        /// </summary>
        /// <returns></returns>
        Task<QuQuotationInsp> GetquotationInsp(int quotationId);

        /// <summary>
        /// Quotation Audit
        /// </summary>
        /// <returns></returns>
        Task<QuQuotationAudit> GetquotationAudit(int quotationId);

        /// <summary>
        /// Get Quotation Bookings by booking no
        /// </summary>
        /// <param name="bookingNo"></param>
        /// <returns></returns>
        Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByBookingNo(int bookingNo);

        /// <summary>
        /// Get Booking Product po data by productrefids
        /// </summary>
        /// <param name="productRefIds"></param>
        /// <returns></returns>
        Task<List<BookingProductPoRepo>> GetBookingProductsPoListByProductRefIds(IQueryable<int> bookingIds);

        /// <summary>
        /// get quotation details for status save
        /// </summary>
        /// <param name="quotaiotn Id"></param>
        /// <returns></returns>
        Task<List<QuTranStatusLog>> GetQuotationDataForStatusLogs(int quotationId);

        /// <summary>
        /// get quotation man day by dates
        /// </summary>
        /// <param name="quotaiotn Id"></param>
        /// <returns></returns>
        IQueryable<Manday> GetQuotationMandayByDate(IEnumerable<DateTime> serviceDates);

        /// <summary>
        /// get quotation travel man day by booking Ids
        /// </summary>
        /// <param name="booking Id list"></param>
        /// <returns></returns>
        Task<QuotManday> GetQuotationTravelManDay(int bookingIds);

        /// <summary>
        /// Get booking status list by quotation id
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetquotationBookingStatus(int quotationId);
        /// <summary>
        /// Get the holiday information
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        Task<HrHoliday> GetHolidayInfo(DateTime fromDate, DateTime toDate, int locationId);

        /// <summary>
        /// Get the holiday type list
        /// </summary>
        /// <param name="priceId"></param>
        /// <returns></returns>
        Task<CuPrDetail> GetHolidayTypes(int priceId);
        /// <summary>
        /// Get Quotation Bookings by cus booking no
        /// </summary>
        /// <param name="cusBookingNo"></param>
        /// <returns></returns>
        Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByCusBookingNo(string cusBookingNo);
        /// <summary>
        /// Get Quotation Bookings by booking service start date and service end date and cusbookingno
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cusBookingNo"></param>
        /// <returns></returns>
        Task<List<QuotationBookingMapRepo>> GetQuotationBookingsByServiceDateCusBookingNo(DateTime startDate, DateTime endDate, string cusBookingNo);

        /// <summary>
        /// Get the quotations for Mobile App
        /// </summary>
        /// <param name="priceId"></param>
        /// <returns></returns>
        IQueryable<MobilePendingQuotation> GetMobileQuotationDetails();

        Task<List<InvoiceInfo>> InvoiceInfoBybookingdId(IEnumerable<int> bookingIds);
        Task<List<InvoiceInfo>> InvoiceDetailsBybookingdId(IQueryable<int> bookingIds);

        /// <summary>
        /// fetch the quotation details by booking Id 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<QuQuotation> GetBookingQuotationDetails(int bookingId);

        Task<List<QuotationData>> GetQuotationOtherCost(IEnumerable<int> bookingIdList);

        Task<QuotationCheckpointData> GetCheckpointByCustomerId(int customerId);

        Task<List<BookingQuotationData>> GetBookingDetails(List<int> bookingIdList);

        Task<QuQuotation> GetQuotationData(int quotationId);

        Task<List<InvoiceInfo>> InvoiceInfoByAuditId(IEnumerable<int> bookingIds);

        Task<int> GetBookingsByQuotation(int bookingId);
        Task<List<InvoiceInfo>> InvoiceDetailsByAuditId(IQueryable<int> bookingIds);

        Task<List<int>> GetCheckpointBrandByCheckpointId(int checkpointId);

        Task<List<int>> GetCheckpointDepartmentByCheckpointId(int checkpointId);

        Task<List<int>> GetCheckpointServiceTypeByCheckpointId(int checkpointId);

        Task<string> GetquotationPdfPath(int quotationId);

        Task<QuQuotationPdfVersion> GetquotationPdfFile(int quotationId);

        Task<List<QuQuotationPdfVersion>> GetquotationPdfHistoryList(int quotationId);
        Task<double> GetCustomerRequirementIndex(int bookingId);

        Task<QuQuotation> GetOnlyQuotation(int id);

        Task<bool?> GetPriceCardTravel(int ruleId);
        Task<List<QuotationBookingContactRepo>> GetQuotationInternalContactsById(List<int> lstid);

        Task<QuQuotationInsp> GetQuotationInsp(int bookingId);
    }
}
