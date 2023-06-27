using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.EntPages;
using DTO.Inspection;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.Kpi;
using DTO.Quotation;
using DTO.Report;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IInspectionBookingRepository : IRepository
    {
        /// <summary>
        /// Get all inspection as Iqueryable
        /// </summary>
        /// <returns></returns>
        IQueryable<InspectionBookingItems> GetAllInspections();
        /// <summary>
        /// Get inspection by booking id
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        Task<InspTransaction> GetInspectionByID(int inspectionID);
        /// <summary>
        /// Add inspection booking entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddInspectionBooking(InspTransaction entity);
        /// <summary>
        /// Edit inspection booking by entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> EditInspectionBooking(InspTransaction entity);
        /// <summary>
        /// Get Inspections
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<InspTransaction>> GetInspections();
        /// <summary>
        /// Edit Po Transaction entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> EditBookingPoTransaction(InspPoTransaction entity);
        /// <summary>
        /// Get the Booking PO transaction details.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<InspProductTransaction>> GetBookingPOTransactionDetails(int bookingId);
        /// <summary>
        /// Get Booking details and combine orders
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<InspTransaction> GetBookingCombineOrders(int bookingId);
        /// <summary>
        /// Get only booking transaction by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<InspTransaction> GetBookingTransaction(int bookingId);
        /// <summary>
        /// Get all booking po transaction 
        /// </summary>
        /// <returns></returns>
        IQueryable<InspTransaction> GetAllBookingPoTransactions();
        /// <summary>
        /// Get Booking status
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<InspStatus>> GetBookingStatus();
        /// <summary>
        /// Get booking status by status id
        /// </summary>
        /// <param name="statusId">status id</param>
        /// <returns>bokoing status</returns>
        InspStatus GetBookingStatusById(int statusId);
        /// <summary>
        /// Get Product File attachment
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        CuProductFileAttachment GetProductFile(string uniqueId);
        /// <summary>
        /// Get inspection File by Inspection Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InspTranFileAttachment> GetFile(int id);
        /// <summary>
        /// Get Product File
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CuProductFileAttachment> GetProductFile(int id, int userId);
        /// <summary>
        /// Get Inspection picking by booking Id
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        Task<InspTransaction> GetInspectionPickingByBookingID(int inspectionID);

        /// <summary>
        /// Get Booking response after booking inspection
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>list</returns>
        Task<InspTransaction> GetBookingResponse(int bookingId);

        /// <summary>
        /// Get username
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ItUserMaster</returns>
        Task<ItUserMaster> GetUserName(int userId);

        /// <summary>
        /// Get Reason Name
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns>InspCancelReason</returns>
        Task<InspCancelReason> GetReasonName(int reasonId);

        /// <summary>
        /// Get Reschedule Reason Name
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns>InspRescheduleReason</returns>
        Task<InspRescheduleReason> GetRescheduleReasonName(int reasonId);

        /// <summary>
        /// Get Service Type Name
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns>RefServiceType</returns>
        Task<RefServiceType> GetServiceTypeName(int serviceTypeId);

        /// <summary>
        /// Get brand name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CuBrand</returns>
        Task<CuBrand> GetBrand(int id);

        /// <summary>
        /// Get department name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CuDepartment</returns>
        Task<CuDepartment> GetDepartment(int id);

        /// <summary>
        /// Get buyer name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CuBuyer</returns>
        Task<CuBuyer> GetBuyer(int id);

        Task<List<InspTransaction>> GetInspectionList(IEnumerable<int> LstinspectionID);

        /// <summary>
        /// GetCustomersByUserId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<CuCheckPoint> GetCustomersByCustomerId(int customerId);

        /// <summary>
        /// Get Excluded Inspection By Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        IQueryable<InspTransaction> GetExcludedInspectionByStatus(int statusId);

        /// <summary>
        /// Get Customer Contacts By BrandId
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        List<CuContactBrand> GetCustomerContactByBrandId(int? brandId);

        /// <summary>
        /// Get Customer Contacts by department Id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        List<CuContactDepartment> GetCustomerContactByDeptId(int? departmentId);

        /// <summary>
        /// Get all booking purchase orders
        /// </summary>
        /// <returns></returns>
        IQueryable<InspTransaction> GetAllBookingPurchaseOrders();

        /// <summary>
        /// Get users based on Role
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItUserRole> GetUsersBasedOnRole(int roleId, int? locationId);

        /// <summary>
        /// Get Tasks based on booking Id and type
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MidTask>> GetTask(int bookingId, IEnumerable<int> typeIdList, bool isdone);

        /// <summary>
        /// Get Last status of the booking
        /// </summary>
        /// <returns></returns>
        Task<int> GetLastStatus(int bookingId);
        /// <summary>
        /// Get Inpection Booking Report Details by inspection id.
        /// </summary>
        /// <param name="inspectionID"></param>
        /// <returns></returns>
        Task<InspTransaction> GetInspectionReportDetails(int inspectionID);
        /// <summary>
        /// Get all the inspection Reports once allocated QC
        /// </summary>
        /// <returns></returns>
        IQueryable<InspTransaction> GetAllInspectionsReports();
        /// Get booking from and to date
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>from and to date from booking table</returns>
        Task<BookingDate> getInspBookingDateDetails(int bookingId);
        /// <summary>
        /// get cs name using it master table user id from dausercustomer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>get CS name, customer id, loation list</returns>
        Task<IEnumerable<CustomerCSLocation>> GetCSLocationList(IEnumerable<int> customerId);
        /// <summary>
        /// Get list of booking from and to date
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>from and to date from booking table</returns>
        Task<List<BookingDate>> getListInspBookingDateDetails(IEnumerable<int> lstbookingId);
        ///<summary>
        ///GetBookingLastServiceDate
        ///</summary>
        /// <param name="bookingId"></param>
        /// <returns>from and to date from Insp status log table</returns>
        Task<BookingDate> GetLastServiceDate(int bookingId);
        ///<summary>
        ///Get Booking Details and customer contact
        ///</summary>
        /// <param name="bookingId"></param>
        /// <returns>booking details and customer contact</returns>
        Task<InspTransaction> GetInspectionCustomerContactByID(int inspectionID);
        /// <summary>
        /// get booking data by bookng id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingData> GetBookingData(int bookingId);

        Task<List<int>> GetBookingServiceTypes(int bookingId);
        Task<List<int>> GetBookingBrands(int bookingId);
        Task<List<int>> GetBookingDepartments(int bookingId);


        /// <summary>
        /// Get Booking Products by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<IEnumerable<ReportProductsData>> GetReportsProductListByBooking(int bookingId, int reportId);
        /// <summary>
        /// Get Product List by booking 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingProductsData>> GetProductListByBooking(IEnumerable<int> bookingId);

        Task<List<BookingPoTransaction>> GetBookingPoList(List<int> bookingIds);

        /// <summary>
        /// Get FB Reports by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<FBReport> GetFbReports(int reportId);
        /// <summary>
        /// Get Fb Reports Detail by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<FBReport_Detail> GetFbReportsDetail(int reportId);

        Task<List<string>> GetFBReportAdditionalPhotos(int reportId);
        /// <summary>
        /// Get Inspection Summary by report Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<IEnumerable<InspectionReportSummary>> GetInspectionSummary(int reportId);
        /// <summary>
        /// Get Inspection Defects by Report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<IEnumerable<InspectionReportDefects>> GetInspectionDefects(int reportId);
        /// <summary>
        /// Get Inspection defects by report and products
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="inspPOId"></param>
        /// <returns></returns>
        Task<IEnumerable<InspectionReportDefects>> GetInspectionDefects(int reportId, IEnumerable<int> inspPOId);

        ///<summary>
        ///Get all the POs and other details for the booking
        ///</summary>
        /// <param name="bookingId"></param>
        /// <returns>booking details and customer contact</returns>
        Task<IEnumerable<PoDetails>> GetBookingPOTransactionDetails(List<int> bookingId);

        ///<summary>
        ///Get all the Quotation Details for the respective PO
        ///</summary>
        /// <param name="POId"></param>
        /// <returns>booking details and customer contact</returns>
        Task<IEnumerable<PoDetails>> GetBookingQuotationDetails(List<int> poIds);

        ///<summary>
        ///Get all the Quotation Details for the respective PO
        ///</summary>
        /// <param name="POId"></param>
        Task<List<PoDetails>> GetBookingQuotationDetailsbybookingId(List<int> lstbookingid);

        /// <summary>
        /// Get all the service Type details
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingId);

        Task<IEnumerable<ServiceTypeList>> GetServiceTypeList(IQueryable<int> bookingId);

        /// <summary>
        /// Get the factory country for the bookings
        /// </summary>
        /// <returns></returns>
        Task<List<FactoryCountry>> GetFactorycountryId(IEnumerable<int> bookingId);

        /// <summary>
        /// Get customer decision by report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<ReportCustomerDecision> GetReportCustomerDecision(int reportId);
        /// <summary>
        /// Get customer check point type count
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="serviceId"></param>
        /// <param name="checkPointType"></param>
        /// <returns></returns>
        Task<int> GetCusCPByCusServiceId(int? cusId, int? serviceId, int checkPointType);
        /// <summary>
        /// get insppotransaction list by inspPoTransIds
        /// </summary>
        /// <param name="inspPoTransIds"></param>
        /// <returns></returns>
        Task<IEnumerable<InspProductTransaction>> GetInspPoDetails(IEnumerable<int> inspPoTransIds);
        /// <summary>
        /// get Product List for client quotation
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<IEnumerable<ClientQuotationBookingItem>> GetProductListByBookingForClientQuotation(int bookingId);
        /// <summary>
        /// fetch customer contact for the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<string>> GetBookingCustomerContact(int bookingId);

        /// <summary>
        /// get product category details
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingProductCategory>> GetProductCategoryDetails(IEnumerable<int> bookingIds);
        /// <summary>
        /// get booking details based on booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingDetail>> GetBookingData(IEnumerable<int> bookingIds);

        Task<IEnumerable<int>> GetPoTransactionIdsByProductRefId(int productRefId);

        Task<IEnumerable<BookingPoData>> GetProductPOListByBooking(IEnumerable<int> bookingId, int productRefId);

        Task<IEnumerable<BookingProductsExportData>> GetProductPoListByBooking(IEnumerable<int> bookingId);

        Task<IEnumerable<BookingContainersRepo>> GetContainerListByBooking(IEnumerable<int> bookingId);

        Task<IEnumerable<BookingProductsData>> GetContainerProductListByBooking(IEnumerable<int> bookingId, int containerRefId);
        /// <summary>
        /// get booking po list by product and container
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingPoData>> GetContainerPoListByBooking(IEnumerable<int> bookingId, int containerRefId, int productRefId);

        Task<IEnumerable<ReportProductsData>> GetContainerListByBooking(IEnumerable<int> bookingId, int conatinerId);

        Task<IEnumerable<int>> GetPoTransactionIdsByContainerRefId(int containerRefId);

        Task<List<BookingContainer>> GetBookingContainer(IEnumerable<int> bookingIds);
        /// <summary>
        /// Get Booking Data with product and purchase order transactions by bookingid

        /// <summary>
        /// get booking information by pass the id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingInfo> GetBookingInfo(int bookingId);
        /// <summary>
        /// Get the product transaction details from the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<InspProductTransaction>> GetBookingProductTransactionDetails(int bookingId);

        Task<bool> CheckBookingIsProcessed(int bookingId);

        /// <summary>
        /// Get the booking buyer list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetBookingBuyerList(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get the booking brand list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetBookingBrandList(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get the booking department list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetBookingDepartmentList(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get the booking price category list
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetBookingPriceCategoryList(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<List<CustomerPriceBookingProductRepo>> GetBookingProductDetails(int bookingId);

        /// <summary>
        /// get price category based on customer id and product sub category 2 id 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<PriceCategoryDetails>> GetPriceCategory(PriceCategoryRequest request);

        /// <summary>
        /// Get FB Inspection Sub Summary by report Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<List<FBReportInspSubSummary>> GetMobileFBInspSummaryResult(List<int> fbReportIdList);

        /// <summary>
        /// Get Product List for mobile app by booking Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingProductsData>> GetMobileProductListByBooking(int bookingId);
        Task<IEnumerable<InspectionReportDefects>> GetMobileInspectionDefectsByReport(List<int> reportIdList);
        Task<IEnumerable<InspectionReportSummary>> GetMobileInspectionSummaryByReport(List<int> reportIdList);
        Task<List<ReportCustomerDecision>> GetMobileReportCustomerDecisionByReport(List<int> reportIdList);


        /// <summary>
        /// get brand id and booking id by bookingids, brandids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="brandIds"></param>
        /// <returns></returns>
        Task<List<int>> GetBookingIdsByBrandsAndBookings(IEnumerable<int> brandIds, IEnumerable<int> bookingIds);
        /// <summary>
        /// get dept id and booking id by bookingids, deptids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        Task<List<int>> GetBookingIdsByDeptsAndBookings(IEnumerable<int> departmentIds, IEnumerable<int> bookingIds);
        /// <summary>
        /// Get the brand booking ids by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingBrandAccess>> GetBrandBookingIdsByBookingIds(IEnumerable<int> bookingIds);
        /// <summary>
        /// Get the dept booking ids by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingDeptAccess>> GetDeptBookingIdsByBookingIds(IEnumerable<int> bookingIds);


        /// <summary>
        /// Get the booking service types by booking list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingServiceType>> GetBookingServiceTypes(IEnumerable<int> bookingIds);
        /// <summary>
        ///  Get the booking buyer list by buyer ids and booking ids
        /// </summary>
        /// <param name="buyerIds"></param>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<int>> GetBookingIdsByBuyersAndBookings(IEnumerable<int> buyerIds, IEnumerable<int> bookingIds);
        /// <summary>
        /// Get the buyer details with booking ids by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingBuyerAccess>> GetBuyerBookingIdsByBookingIds(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get the defect Image data by defect Ids
        /// </summary>
        /// <param name="defectIdList"></param>
        /// <returns></returns>
        Task<List<ReportDefectsImage>> GetMobileInspectionDefectsImageByReport(List<int> defectIdList);

        /// <summary>
        /// Get picking data
        /// </summary>
        /// <param name="poTranId"></param>
        /// <returns></returns>
        Task<bool> GetPickingData(int poTranId, int prodId);

        Task<InspectionBookingApplicantItems> GetInspectionDetails(int CreatedById);

        Task<List<InternalReportProducts>> GetProductListByBookingByPO(List<int> bookingId);

        Task<IEnumerable<ServiceTypeList>> GetAuditServiceType(IEnumerable<int> bookingIds);

        Task<List<BookingServiceType>> GetAuditServiceTypes(IEnumerable<int> auditIds);

        Task<int?> GetPreviousBookingNumber(int bookingNo);

        Task<List<BookingReportData>> GetReportDataByBooking(List<int> bookingNoList);
        Task<List<BookingReportData>> GetReportDataByQueryableBooking(IQueryable<int> bookingNoList);

        Task<List<BookingReportData>> GetContainerReportDataByBooking(List<int> bookingNoList);

        Task<bool> IsBookingInvoiced(int bookingId);

        Task<bool> IsAnyOneBookingInvoiced(IEnumerable<int> bookingIds);

        Task<double?> GetQuotationManDayByBooking(int bookingId);

        Task<List<int>> BookingQuotationExists(List<int> bookingIds);

        Task<List<int>> GetPickingExists(List<int> poTransactionIds);

        Task<List<int>> GetProductReport(List<int> poTransactionIds);

        Task<IEnumerable<ScheduleProductsData>> GetScheduleProductListByBooking(IEnumerable<int> bookingId);

        Task<IEnumerable<ScheduleContainersRepo>> GetScheduleContainerListByBooking(IEnumerable<int> bookingId);

        Task<List<BookingReportData>> GetInspectionReportData(List<int> bookingNoList);

        IQueryable<InspTransaction> GetAllInspectionsQuery();

        Task<List<InspectionProductsExportData>> GetBookingProductPoList(IQueryable<int> bookingIds);

        Task<List<InspectionProductsExportData>> GetContainerBookingProductList(IQueryable<int> bookingIds);

        Task<List<BookingBuyerAccess>> GetBuyerBookingIdsByBookingQuery(IQueryable<int> bookingIds);

        Task<List<BookingBrandAccess>> GetBrandBookingIdsByBookingQuery(IQueryable<int> bookingIds);

        Task<List<BookingDeptAccess>> GetDeptBookingIdsByBookingQuery(IQueryable<int> bookingIds);

        Task<List<InspectionQuotationExportData>> GetBookingQuotationDetailsbybookingId(IQueryable<int> lstbookingid);

        Task<List<InspectionPOExportData>> GetBookingPoListByBookingQuery(IQueryable<int> lstbookingid);

        Task<List<FactoryCountry>> GetFactorycountryByBookingQuery(IQueryable<int> bookingIds);

        Task<List<ServiceTypeList>> GetServiceTypeByBookingQuery(IQueryable<int> bookingId);

        Task<List<PoDetails>> GetBookingQuotationDetailsbybookingIdQuery(IQueryable<int> lstbookingid);

        Task<IEnumerable<ScheduleProductsData>> GetScheduleProductListByBookingQuery(IQueryable<int> bookingId);

        Task<IEnumerable<ScheduleContainersRepo>> GetScheduleContainerListByBookingQuery(IQueryable<int> bookingId);

        Task<List<CustomerContact>> GetEditBookingCustomerContacts(int bookingId);

        Task<List<suppliercontact>> GetEditBookingSupplierContacts(int? bookingId);

        Task<List<suppliercontact>> GetEditBookingFactoryContacts(int? bookingId);

        Task<List<CommonDataSource>> GetEditBookingOffice(int bookingId);

        Task<List<CommonDataSource>> GetEditBookingUnit(int bookingId);
        IQueryable<int> GetReportIdDataByQueryableBooking(IQueryable<int> bookingNoList);
        Task<List<CommonDataSource>> GetHoldReasonTypes();

        Task<List<CommonDataSource>> GetEditBookingInspectionLocations(int bookingId);

        Task<List<CommonDataSource>> GetEditBookingShipmentTypes(int bookingId);

        Task<List<CommonDataSource>> GetEditBookingCuProductCategory(int customerId, int bookingId);

        Task<List<CommonDataSource>> GetEditBookingBusinessLines(int bookingId);

        Task<IEnumerable<ServiceTypeList>> GetQueryableAuditServiceType(IQueryable<int> bookingIds);

        Task<InspectionBookingDetail> GetInspectionBookingDetails(int bookingId);

        Task<List<InspectionProductDetail>> GetProductTransactionList(int bookingId);

        Task<List<InspectionPODetail>> GetPurchaseOrderTransactionList(int bookingId);

        Task<InspectionHoldReasons> GetInspectionHoldReasons(int bookingId);

        Task<List<InspectionCancelReason>> GetInspectionCancelReasons(IQueryable<int> bookingIds);

        Task<List<int>> GetBookingMappedCustomerContacts(int bookingId);

        Task<List<int>> GetBookingMappedSupplierContacts(int bookingId);

        Task<List<int?>> GetBookingMappedFactoryContacts(int bookingId);

        Task<List<int>> GetBookingMappedBuyers(int bookingId);

        Task<List<int>> GetBookingMappedBrands(int bookingId);

        Task<List<int>> GetBookingMappedDepartments(int bookingId);

        Task<List<int>> GetBookingMappedMerchandisers(int bookingId);

        Task<List<int?>> GetBookingMappedShipmentTypes(int bookingId);

        Task<List<int>> GetBookingMappedContainers(int bookingId);

        Task<List<InspectionDFTransactions>> GetBookingMappedDFTransactions(int bookingId);

        Task<List<BookingFileAttachment>> GetBookingMappedFiles(int bookingId);

        Task<List<ProductFileAttachmentRepsonse>> GetProductFileAttachments(int bookingId);

        Task<List<InspectionProductSubCategory>> GetProductSubCategoryList(List<int?> productCategoryIds);

        Task<List<InspectionProductSubCategory2>> GetProductSubCategory2List(List<int?> productSubCategoryIds);
        Task<BookingDataRepo> GetBookingDetails(int bookingId);
        Task<IEnumerable<BookingProductinfo>> GetProductItemByBooking(int bookingId);
        Task<List<BookingProductPoRepo>> GetBookingProductsPoItemsByProductRefIds(int bookingId);
        IQueryable<int> GetInspectionNo();

        Task<List<BookingCustomerContactAccess>> GetBookingCustomerContacts(IEnumerable<int> bookingIds);

        Task<List<InspectionProductAndReport>> GetBookingProductAndReportResult(IEnumerable<int> bookingIds);

        Task<List<InspectionPoNumberList>> GetPoNoListByBookingIds(IEnumerable<int> bookingIds);

        Task<List<InspectionSupplierFactoryContacts>> GetSupplierContactsByBookingIds(List<int> bookingIds);

        Task<List<InspectionSupplierFactoryContacts>> GetFactoryContactsByBookingIds(List<int> bookingIds);

        Task<List<CommonDataSource>> GetInspectionSummaryPhoto(int reportId);

        Task<List<BookingMerchandiserContactList>> GetMerchandiserContactsByBookingIds(List<int> bookingIds);

        Task<List<InspectionPOColorTransaction>> GetPOColorTransactions(int bookingId);

        Task<List<CommonDataSource>> GetInspectionTransCSList(int bookingId);

        Task<InspTransaction> GetInspectionBaseTransaction(int bookingId);
        Task<bool> GetEntityFeatureIsExist(int factoryCountryId);
        Task<List<InspectionProductSubCategory3>> GetProductSubCategory3List(List<int> productSubCategory2Ids);
        Task<IEnumerable<BookingQuantityData>> GetBookingQuantityDetails(IEnumerable<int> bookingId);
        Task<IEnumerable<BookingReportQuantityData>> GetBookingReportQuantityDetails(IEnumerable<int> bookingId);
        Task<IEnumerable<BookingQuantityData>> GetContainerReports(IEnumerable<int> bookingId);

        Task<IEnumerable<InspTranStatusLog>> GetBookingStatusLogsByQuery(IQueryable<int> inspectionIdList);

        Task<List<InspectionHoldReasons>> GetInspectionHoldReasons(IQueryable<int> bookingIds);

        Task<List<BookingFileAttachment>> GetBookingMappedFilesByBookingIds(IEnumerable<int> bookingIds);

        Task<InspTransactionDraft> GetInspectionDraftById(int id);

        Task<List<DraftInspectionRepo>> GetInspectionDraftByUserId(int userId);

        Task<List<EntPageFieldAccess>> GetEntPageFieldAccess(EntPageRequest request);

        Task<List<InspectionPriceProductCategory>> GetBookingPriceProductCategory(IEnumerable<int> bookingIds);
        Task<List<InspectionPriceProductSubCategory>> GetBookingPriceProductSubCategory(IEnumerable<int> bookingIds);
        Task<List<InvoiceBookingDetail>> GetOrdersOnSameFactoryAndSameDate(List<int> bookingIds, int customerId, int factoryId, DateTime serviceDate);

        Task<List<InspTransaction>> GetInspectionByProductId(int productId);
        Task<List<InspectionHoldReasons>> GetInspectionHoldReasons(List<int> bookingIds);

        Task<List<PoProductData>> GetPoProductDataByPoAndCustomer(BookingPoSearchData bookingPoSearchData);

        Task<List<InspTranFileAttachment>> GetFileAttachmentsByBookingIdsAndZipStatus(int? inspectionId);

        Task<InspTranFileAttachmentZip> GetInspectionFileAttachmentZipData(int inspectionId);

        Task<BookingFileZipAttachment> GetBookingFileAttachment(int inspectionId);

        Task<bool> CheckPoDetailAvailableByPoProductDetail(BookingPoProductSearchData bookingPoProductSearchData);

        Task<bool> GetServiceTypeIgnoreAcceptanceLevel(int customerId, int serviceTypeId);

        Task<BookingCustomerServiceTypeData> GetBookingCustomerServiceTypes(int bookingId);

        Task<List<InvoicePreviewReportResult>> GetReportResultByInspectionId(List<int> inspectionIds);

        Task<IEnumerable<ProductTranData>> GetProductDetails(IEnumerable<int> bookingIds);


        Task<List<InspectionPOColorTransaction>> GetPOColorTransactionsByBookingIds(IEnumerable<int> bookingIds);
        Task<List<BookingProductPoRepo>> GetPoDataByBookingIds(IEnumerable<int> bookingIds);
        Task<bool> GetPODataBypoId(string poid);
        Task<List<CustomerReportInspectionRepo>> GetCustomerReportInspectionDetails(IQueryable<int> bookingIds);

        Task<string> GetBoookingStatus(string poid);
        Task<RefUnit> GetUnitByName(string unitname);
        Task<bool> IsAnyProductReportInProgress(int productId);
        Task<List<CSNameRepo>> GetInspectionTransCSList(IQueryable<int> bookingIdList);
        Task<IEnumerable<InspectionCsData>> GetInspectionTransCsDetails(List<int> bookingIds);
        Task<bool> CheckInspectionStatusByBookingId(int bookingId, int statusId);

        Task<InspBookingEmailConfiguration> GetCCEmailConfigurationEmailsByCustomer(int customerId, int? factoryCountryId, int bookingStatusId);

        /// <summary>
        ///  Get the booking Id by customer id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<bool> CheckInspectionBookingByCustomerId(int bookingId, int customerId);

        Task<List<ScheduleBookingDetailsRepoItem>> GetBookingsByServiceDates(DateTime yesterdayDate);
        Task<IEnumerable<EaqfProductDetails>> GetEaqfBookingPOTransactionDetails(List<int> bookingIds);
        Task<IEnumerable<EaqfReportDetails>> GetEaqfBookingReportDetails(List<int> bookingIds);

        Task<string> GetCustomerBookingDefaulComments(int customerId);
        Task<List<ReportVersionData>> GetReportVersionDetails(List<int> ReportIdList);
        Task<List<InspectionPickingDetails>> GetInspectionPicking(int bookingId);
        Task<List<InspectionPickingContactDetails>> GetInspectionPickingContacts(List<int> pickingIds);
        Task<InspTransaction> GetInspectionWithFileAttachment(int inspectionId);

        Task<IEnumerable<EaqfInvoiceDetails>> GetEaqfBookingInvoiceDetails(List<int> bookingIds);
        Task<IEnumerable<EaqfInvoiceDetails>> GetEaqfBookingInvoiceFileDetails(List<string> invoiceNoList);
        Task<InspTransaction> GetBookingDataUptoInspected(int inspectionId);
    }
}
