using Contracts.Repositories;
using DTO.CancelBooking;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.DataAccess;
using DTO.Eaqf;
using DTO.EntPages;
using DTO.File;
using DTO.Inspection;
using DTO.MobileApp;
using DTO.OfficeLocation;
using DTO.PurchaseOrder;
using DTO.Quotation;
using DTO.Report;
using DTO.Schedule;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInspectionBookingManager
    {
        Task<GetInspectionResponse> GetInspection(int inspectionID);
        Task<PickingAndCombineOrderResponse> GetPickingAndCombineOrders(int id);
        Task<BookingCustomerDetails> GetBookingDetailsByCustomerId(int customerId);
        //Task<BookingPoProductResponse> GetPurchaseOrderProductsByPoNumber(int? id, int supplierId);
        OfficeSummaryResponse GetBookingOffice();
        Task<SaveInspectionBookingResponse> SaveInspectionBooking(SaveInsepectionRequest entity);
        Task<SaveDraftInsepectionResponse> SaveDraftInspectionBooking(DraftInspectionRequest request);
        Task<DraftInspectionResponse> GetInspectionDraftByUserId();
        Task<DeleteDraftInspectionResponse> RemoveInspectionDraft(int draftInspectionId);
        Task<SaveInspectionBookingResponse> CancelInspectionBooking(SplitBooking entity);
        Task<SaveInspectionBookingResponse> NewInspectionBooking(SplitBooking entity);
        Task<BookingSummarySearchResponse> GetAllInspectionsData(InspectionSummarySearchRequest request, bool IsExport = false);
        Task<EditInspectionFactDetails> GetFactoryDetailsByCustomerIdFactoryId(int? cusid, int factid, int? bookingId);
        Task<EditInspectionBookingSupDetails> GetSupplierDetailsByCustomerIdSupplierId(int? cusid, int supid, int? bookingId);
        Task<UnitDetailsResponse> GetUnits();
        Task<BookingSummaryResponse> GetBookingSummary();
        Task UploadProductFiles(Dictionary<string, byte[]> fileList, int bookingId);
        BookingPOListResponse GetPOListByCustomerAndProducts(int? customerid, int productcategoryid, int supplierId);
        List<CustomerProduct> GetProductsByCustomerPOAndCategory(int customerId, int poid);
        Task<FileResponse> GetFile(int id);
        Task<BookingMailRequest> GetBookingMailDetail(int bookingId, bool? isEmailRequired, bool? isEdit, int? userId = 0);
        Task<BookingStatusUpdateResponse> UpdateBookingStatus(int bookingId, int statusId);
        ReInspectionTypeResponse GetReInspectionTypes();
        Task<BookingCustomerContactDetails> GetCustomerContacts(BookingCustomerContactRequest request);
        Task<Boolean> IsHolidayExists(HolidayRequest request);
        IQueryable<InspTransaction> GetExcludedInspections(int statusId);
        Task<SetInspNotifyResponse> BookingTaskNotification(int id, bool isCombineOrderDataChanged, int statusId, SaveInsepectionRequest request);
        Task<ReportSummaryResponse> GetAllInspectionReportProducts(InspectionSummarySearchRequest request);
        Task<BookingProductAndReportDataResponse> GetBookingAndProductsAndReports(int bookingId, int reportId, int containerId);
        Task<BookingProductsResponse> GetBookingProductsAndStatus(int bookingId);
        Task<InspectionReportSummaryRepsonse> GetInspectionSummary(int reportId);
        Task<InspectionDefectsRepsonse> GetInspectionDefects(int reportId);
        Task<InspectionDefectsRepsonse> GetInspectionDefectsByReportandInspection(int reportId, int inspPoId);
        ///<summary>
        ///get service type for booking id list
        ///</summary>
        /// <param name="bookingIdList"></param>
        /// <returns>service type details</returns>
        Task<IEnumerable<ServiceTypeList>> GetServiceTypeList(IEnumerable<int> bookingIdList);

        ///<summary>
        /// get product category details
        ///</summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingProductCategory>> GetProductCategoryDetails(IEnumerable<int> bookingIds);
        /// <summary>
        /// get product category list with bookingid
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingProductCategoryData>> GetProductCategoryList(IEnumerable<int> bookingIds);
        ///<summary>
        /// update task Column
        ///</summary>
        /// <param name="bookingId"></param>
        /// <param name="typeIdList"></param>
        /// <param name="newTaskDoneValue"></param>
        /// <param name="oldTaskDoneValue"></param>
        /// <returns></returns>
        Task<IEnumerable<MidTask>> UpdateTask(int bookingId, IEnumerable<int> typeIdList, bool oldTaskDoneValue, bool newTaskDoneValue);
        ///<summary>
        /// get booking details by booking id
        ///</summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<InspTransaction> GetBookingData(int bookingId);
        /// <summary>
        /// get booking details based on booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<IEnumerable<BookingDetail>> GetBookingData(IEnumerable<int> bookingIds);

        /// <summary>
        /// get booking products po list by booking id and product ref id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        Task<BookingProductPOResponse> GetBookingProductPoList(int bookingId, int productRefId);

        /// <summary>
        /// get booking container list by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingContainerResponse> GetBookingContainersAndStatus(int bookingId);
        /// <summary>
        /// get only booking containers
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingContainerResponse> GetBookingContainers(int bookingId);

        /// <summary>
        /// get booking container products by booking id AND container ref id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <returns></returns>
        Task<BookingContainerProductResponse> GetBookingContainerProductList(int bookingId, int containerRefId);
        /// <summary>
        /// get booking po list by booking product and container
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="containerRefId"></param>
        /// <param name="productRefId"></param>
        /// <returns></returns>
        Task<BookingProductPOResponse> GetBookingProductPoList(int bookingId, int containerRefId, int productRefId);

        /// <summary>
        /// get inspection defects by container and reports
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="containerRefId"></param>
        /// <returns></returns>
        Task<InspectionDefectsRepsonse> GetInspectionDefectsByReportandContainer(int reportId, int containerRefId);

        /// <summary>
        /// get fb template List
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetFbTemplateList();

        /// <summary>
        /// Get only the booking products by booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingProductDataResponse> GetBookingProducts(int bookingId);

        /// <summary>
        /// get booking information by pass the id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingInformation> GetBookingInformation(int bookingId);

        Task<bool> CheckBookingIsProcessed(int bookingId);
        /// <summary>
        /// get price category based on customer id and product sub category 2 id 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PriceCategoryResponse> GetPriceCategoryByCustomerIdPCSub2Id(PriceCategoryRequest request);
        /// <summary>
        /// get booking department list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetBookingDepartmentList(IEnumerable<int> bookingIds);
        /// <summary>
        /// get booking brand list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetBookingBrandList(IEnumerable<int> bookingIds);

        /// <summary>
        /// get booking details for Mobile App
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<InspSummaryMobileDetaiResponse> GetInspDetailMobileSummary(int reportId);
        /// <summary>
        /// Get the booking Service types
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<BookingServiceType>> GetBookingServiceTypes(IEnumerable<int> bookingIds);

        /// <summary>
        /// Get insp summary for mobile app
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<InspSummaryMobileResponse> GetMobileInspSummary(InspSummaryMobileRequest request);

        /// <summary>
        /// get product and status details fro mobile app
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        Task<BookingProductMobileResponse> GetMobileBookingProductsAndStatusTimeline(int bookingId);

        /// <summary>
        /// Get the virtual scroll country for mobile app
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FilterDataSourceResponse> GetMobileFactoryCountry(CommonCountrySourceRequest request);

        /// <summary>
        /// Get the virtual scroll factory/ supplier for mobile app
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FilterDataSourceResponse> GetMobileSupplierFactory(CommonDataSourceRequest request);

        /// <summary>
        /// Get the virtual scroll customer for mobile app 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FilterDataSourceResponse> GetMobileCustomer(CommonDataSourceRequest request);

        /// <summary>
        /// Get the virtual scroll department for mobile app 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FilterDataSourceResponse> GetMobileDepartment(CommonCustomerSourceRequest request);

        /// <summary>
        /// Get the virtual scroll collection for mobile app 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FilterDataSourceResponse> GetMobileCollection(CommonCustomerSourceRequest request);

        /// <summary>
        /// Get the virtual scroll buyer for mobile app 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<FilterDataSourceResponse> GetMobileBuyer(CommonCustomerSourceRequest request);

        /// <summary>
        /// Get status and service type list
        /// </summary>
        /// <returns></returns>
        Task<CommonFilterListResponse> GetMobileCommonFilter();

        /// <summary>
        /// get product picking and quotation information for validation during product deletion, from the booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="poTranId"></param>
        /// <returns></returns>
        Task<ProductValidationResponse> BookingProductValidationInfo(int bookingId, int poTranId, int prodId);

        Task<ApplicantStaffResponse> GetApplicantInfoById();

        Task<BookingDate> getInspBookingDateDetails(int bookingId);

        Task<List<BookingProductsExportData>> ExportBookingProductSummary(int bookingId);

        Task<DataSourceResponse> GetBookingStatusList();

        IQueryable<InspectionBookingItems> GetAllInspections();

        Task<SetInspNotifyResponse> GetConfirmEmailUsers(int bookingId, SaveInsepectionRequest request);
        /// <summary>
        /// Get CS Names by customer,Office,brand,department
        /// </summary>
        /// <returns></returns>
        Task<CSConfigResponse> getCSNames(UserAccess userAccess);

        Task<CSConfigListResponse> getCSList(UserAccess userAccess);

        Task<List<BookingReportData>> GetReportDataByBooking(List<int> bookingNoList);
        Task<List<BookingReportData>> GetReportDataByQueryableBooking(IQueryable<int> bookingNoList);

        Task<List<ProductValidateData>> BookingProductValidation(List<ProductValidateData> productValidateData);

        Task<InspectionExportData> ExportReportDataSummary(InspectionSummarySearchRequest request);

        Task<BookingSummaryStatusResponse> GetBookingSummaryStatus();

        Task<DataSourceResponse> GetAEUserList();

        Task<EditBookingCustomerDetails> GetEditBookingDetailByCustomerId(int customerId, int bookingId);

        Task<DataSourceResponse> GetEditBookingOffice(int bookingId);

        Task<DataSourceResponse> GetEditBookingUnit(int bookingId);

        IQueryable<int> GetReportIdDataByQueryableBooking(IQueryable<int> bookingNoList);

        Task<DataSourceResponse> GetHoldReasonTypes();

        Task<DataSourceResponse> GetEditBookingInspectionLocations(int bookingId);

        Task<DataSourceResponse> GetEditBookingShipmentTypes(int bookingId);

        Task<DataSourceResponse> GetEditBookingCuProductCategory(int customerId, int bookingId);

        Task<DataSourceResponse> GetEditBookingCustomerSeason(int? customerId, int bookingId);


        Task<DataSourceResponse> GetEditBookingBusinessLines(int bookingId);
        Task<EditInspectionBookingResponse> EditInspectionData(int bookingId);

        Task<EditInspectionBookingResponse> AddInspectionData();
        Task<BookingDataRepo> GetBookingDetails(int bookingId);
        Task<List<FactoryCountry>> GetFactorycountryId(IEnumerable<int> bookingId);
        Task<IEnumerable<BookingProductinfo>> GetProductItemByBooking(int bookingId);
        Task<List<BookingProductPoRepo>> GetBookingProductsPoItemsByProductRefIds(int bookingId);
        IQueryable<int> GetInspectionNo();
        Task<BookingDataInfoResponse> GetBookingInfoDetails(int bookingId);
        Task<BookingNoDataSourceResponse> GetBookingNoDataSource(BookingNoDataSourceRequest request);

        Task<bool> GetInspectionPickingExists(InspectionPickingExistRequest request);

        Task<InspectionProductBaseDetailResponse> GetInspectionProductBaseDetails(int bookingId);

        Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration();

        Task<EntPageFieldAccessResponse> GetEntPageFieldAccess(EntPageRequest request);

        Task<SaveMasterContactResponse> SaveMasterContact(SaveMasterContactRequest request);

        Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingIds);

        Task<BookingFileZipResponse> GetBookingFileAttachment(int bookingId);

        Task<POProductDetailResponse> GetPoProductDetails(PoProductDetailRequest request);

        Task<IEnumerable<ProductTranData>> GetProductDetails(IEnumerable<int> bookingIds);

        Task<List<BookingProductPoRepo>> GetPoDataByBookingIds(IEnumerable<int> bookingIds);

        Task<FileResponse> GetPurchaseOrderSampleFile();

        Task<List<string>> GetCCEmailConfigurationEmailsByCustomer(int customerId, int factoryCountryId, int bookingStatusId);
        Task<IEnumerable<BookingDetail>> GetBookingDetails(IEnumerable<int> bookingIds);
        Task<List<FactoryCountry>> GetBookingFactorycountryDetails(IEnumerable<int> bookingIds);
        Task<object> SaveEaqfInspectionBooking(SaveEaqfInsepectionRequest request);
        Task<object> GetEaqfInspectionBooking(GetEaqfInspectionBookingRequest request);
        Task<object> GetEaqfInspectionReportBooking(string bookingIds);
        Task<BookingPOProductListResponse> GetPoProductListBooking(BookingPOProductDataSourceRequest request);
        Task<object> EaqfBookingEventUpdate(int bookingId, EaqfEvent request);
    }
}
