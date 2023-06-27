using DTO.CancelBooking;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Eaqf;
using DTO.File;
using DTO.Inspection;
using DTO.Invoice;
using DTO.MobileApp;
using DTO.Quotation;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IQuotationManager
    {
        /// <summary>
        /// GetQuotation
        /// </summary>
        /// <returns></returns>
        Task<QuotationResponse> GetQuotation(int? id);


        /// <summary>
        /// Get Quotation details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuotationDetails> GetQuotationDetails(int id);

        /// <summary>
        /// GetCustomerList
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<QuotationDataSourceResponse> GetCustomerList(int countryId, int serviceId);

        /// <summary>
        /// Get supplier List
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<QuotationDataSourceResponse> GetSupplierList(int customerId);

        /// <summary>
        /// Get CustomerContact List
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<QuotationContactListResponse> GetCustomerContactList(int customerId);

        /// <summary>
        /// Get Factory List
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        Task<QuotationDataSourceResponse> GetFactoryList(int supplierId);

        /// <summary>
        /// Get Supplier ContactList
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<QuotationContactListResponse> GetSupplierContactList(int supplierId, int customerId);

        /// <summary>
        /// Get factry contact list
        /// </summary>
        /// <param name="factoryId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<QuotationContactListResponse> GetFactoryContactList(int factoryId, int customerId);

        /// <summary>
        /// Get InternalContact List
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<QuotationContactListResponse> GetInternalContactList(int locationId, int customerId);


        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<QuotationOrderListResponse> GetOrders(FilterOrderRequest request);

        /// <summary>
        /// Save Quotation 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveQuotationResponse> SaveQuotation(SaveQuotationRequest request);

        /// <summary>
        /// Get factory address
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        Task<string> GetFactoryAddress(int factoryId);

        /// <summary>
        /// Set Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idStatus"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<SetStatusQuotationResponse> SetStatus(SetStatusBusinessRequest request);

        /// <summary>
        /// Update email state
        /// </summary>
        /// <param name="id"></param>
        /// <param name="emailState"></param>
        /// <param name="isTask"></param>
        void UpdateEmailState(Guid id, int emailState, bool isTask);

        /// <summary>
        /// Get Quotation Summary
        /// </summary>
        /// <returns></returns>
        Task<QuotationSummaryResponse> GetQuotationSummary();


        /// <summary>
        /// Get quotation list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<QuotationDataSummaryResponse> GetQuotationList(QuotationSummaryGenRequest request, bool Isexport = false);


        /// <summary>
        /// GetQuotationVersion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FileResponse> GetQuotationVersion(Guid id);

        /// <summary>
        /// QuotationManday
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<QuotationMandayResponse> QuotationManday(QuotationMandayRequest request);

        /// <summary>
        /// update and insert quotation service date and api remark quotation column
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateQuotationServiceDate(BookingDateInfo request, InspTransaction entity);

        /// <summary>
        /// get quotation id by audit id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>quotation id</returns>

        Task<int> GetQuotationIdByAuditid(int AuditbookingId);
        /// <summary>
        /// product sample qty from booking and check with quoation sample qty
        /// </summary>
        /// <param name="quotProducts"></param>
        /// <returns></returns>
        Task<bool> CheckQuotationSampleQtyAndBookingSampleQtyAreEqual(IEnumerable<QuotProduct> quotProducts);

        //Fetch Data for the ADeO quotation file
        Task<ClientQuotationResponse> GetClientQuotation(int quotationID);

        /// <summary>
        /// GetQuotationSummaryInspExport Details
        /// </summary>
        /// <returns></returns>
        Task<QuotationExportDataResponse> GetQuotationSummaryInspExportDetails(QuotationSummaryGenRequest request);
        /// <summary>
        /// GetQuotationSummaryAuditExport Details
        /// </summary>
        /// <returns></returns>
        Task<QuotationExportDataResponse> GetQuotationSummaryAuditExportDetails(QuotationSummaryGenRequest request);

        ///// <summary>
        ///// GetQuotationSummaryExport Details
        ///// </summary>
        ///// <returns></returns>
        //Task<QuotationExportDataResponse> GetQuotationSummaryExportDetails(QuotationSummaryGenRequest request);


        /// <summary>
        /// Save Invoice
        /// </summary>
        /// <returns></returns>
        Task<SaveQuotationResponse> SaveInvoice(InvoiceRequest request);

        /// <summary>
        /// Get Invoice
        /// </summary>
        /// <returns></returns>
        Task<QuotationEditInvoiceItem> GetInvoice(int quotationId, int serviceId);

        /// <summary>
        /// get customer price card details based on  booking details and quotation billing method, billing paid by, currency
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="billMethod"></param>
        /// <param name="billPaidBy"></param>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        Task<PriceCard> GetCustomerCardDetailsByBookingDetails(IEnumerable<BookingDetail> bookingFilters, int billMethod, int billPaidBy, int? currencyId);

        /// <summary>
        /// unit price data fetch with booking ids 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CustomerPriceCardUnitPriceResponse> GetCustomerPriceCardUnitPriceData(UnitPriceCardRequest request);

        /// <summary>
        /// GetCustomerPriceCardData for quotation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuotationPriceCard> GetCustomerPriceCardData(CustomerPriceCardRequest request);

        /// <summary>
        /// Get the sampling unit price
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="customerPriceCard"></param>
        /// <returns></returns>
        Task<double> GetSamplingUnitPrice(int bookingId, int priceId);

        /// <summary>
        /// Get the sampling unit price by booking id
        /// </summary>
        /// <param name="samplingUnitPriceRequest"></param>
        /// <returns></returns>
        Task<SamplingUnitPriceResponse> GetSamplingUnitPriceByBooking(List<SamplingUnitPriceRequest> samplingUnitPriceRequest);

        /// <summary>
        /// Get booking status list by quotationId
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetBookingStatusList(int quotationId);
        /// <summary>
        /// update quotation service date 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateQuotationServiceDateReschdule(BookingDateInfo request, InspTransaction entity,int userId);

        /// <summary>
        /// Get quotation details for mobile app
        /// </summary>
        /// <param name="InspectionSummarySearchRequest"></param>
        /// <returns></returns>
        Task<List<MobileInspQuotationData>> GetMobileQuotationData(InspectionSummarySearchRequest request);

        /// <summary>
        /// Get bill paid by list
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetBillPaidByList();

        /// <summary>
        /// Get quotation status color
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        Task<QuotationSummaryStatusResponse> GetQuotationStatusColor();

        /// <summary>
        /// get pending quotations for mobile app
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<InspQuotationMobileResponse> GetMobilePendingQuotation(InspSummaryMobileRequest request);

        /// <summary>
        ///  mobile app status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        FilterDataSourceResponse GetMobileQuotationStatus();

        /// <summary>
        /// check if the skipQuotationSentToClient checkpoint exists
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<bool> GetSkipQuotationSentToClientCheckpoint(QuotCheckpointRequest request);

        Task<string> GetquotationPdfPath(int quotationId);

        string SavePdfReferenceToCloudAndUpdatewithQuotation(FileResponse document, int QuotationId);
        Task<CalculatedWorkingHoursResponse> GetCalculatedWorkingManday(int bookingId);

        Task<QuotationTravelMatrixResponse> GetTravelMatrixData(TravelMatrixRequest request);

        Task<PriceCardTravelResponse> GetPriceCardTravel(int ruleId);
        Task<CalculatedWorkingHoursResponse> SaveWorkingManday(int bookingId);
        Task<object> SaveEaqfQuotation(SaveQuotationEaqfRequest request);

        Task<object> SaveEAQFQuotationAndInvoice(SaveQuotationEaqfRequest request);
        Task<FactoryBookingInfoResponse> FactoryBookingInfo(FactoryBookingInfoRequest request);
        Task<object> GetEaqfInspectionInvoiceDetails(string bookingIds);
        Task<object> GetAuditEaqfInspectionInvoiceDetails(string bookingIds);
    }
}
