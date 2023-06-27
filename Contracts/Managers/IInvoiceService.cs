using DTO.CommonClass;
using DTO.Customer;
using DTO.Invoice;
using DTO.Quotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IInvoiceManager
    {
        Task<InvoiceGenerateResponse> InspectionInvoiceGenerate(InvoiceGenerateRequest requestDto);
        Task<InvoiceGenerateResponse> AuditInvoiceGenerate(InvoiceGenerateRequest requestDto);
        Task<InvoiceBaseDetailResponse> GetInvoiceBaseDetails(string invoiceNo, int serviceId);
        Task<InvoiceBilledAddressResponse> GetInvoiceBilledAddress(int billToId, int searchId);
        Task<InvoiceContactsResponse> GetInvoiceContacts(int billToId, int searchId);
        Task<DataSourceResponse> GetInvoicePaymentStatus();
        Task<InvoiceTransactionDetailsResponse> GetInvoiceTransactionDetails(string invoiceNo, int serviceId);
        Task<InvoiceBookingMoreInfoResponse> GetInvoiceBookingMoreInfo(int bookingId);
        Task<UpdateInvoiceDetailsResponse> UpdateInvoiceTransaction(UpdateInvoiceDetailRequest request);
        Task<DeleteInvoiceDetailResponse> DeleteInvoiceDetail(int invoiceId);
        Task<DataSourceResponse> GetInvoiceOffice();
        Task<InvoiceSummaryResponse> GetInvoiceSearchSummary(InvoiceSummaryRequest requestDto);
        Task<InvoiceReportTemplateResponse> GetInvoiceReportTemplates(InvoiceReportTemplateRequest invoiceReportTemplateRequest);
        Task<InvoiceMoExistsResult> CheckInvoiceNumberExist(string invoiceNumber);
        Task<InvoiceBookingProductsResponse> GetInvoiceBookingProducts(int bookingId);
        Task<InvoiceCancelResponse> CancelInvoice(string invoiceId);
        Task<InvoiceBookingSummaryResponse> GetInvoiceBookingSearchSummary(string invoiceNo, int serviceId);
        Task<List<ExportInvoiceBookingData>> ExportInvoiceSearchSummary(InvoiceSummaryRequest requestDto);
        Task<DataSourceResponse> GetInvoiceStatusList();

        Task<InvoiceKpiTemplateResponse> GetInvoiceKpiTemplate(InvoiceKpiTemplateRequest request);
        Task<InvoiceNewBookingResponse> GetNewInvoiceBookingData(NewBookingInvoiceSearch request);

        Task<IEnumerable<InvoiceBankTax>> GetTaxDetails(int bankId, DateTime maxInspectionDate, DateTime minInspectionDate);
        Task<IEnumerable<InvoiceBookingDetail>> GetInspectioDatabyInvoiceRequest(InvoiceGenerateRequest requestDto, List<int> invoicedBookings);

        Task SetRuleDataList(IEnumerable<CustomerPriceCardRepo> ruleConfigList);

        Task<List<CustomerPriceCardRepo>> GetRuleConfigListbyBookingFilter(InvoiceBookingDetail orderTransactionDto, IEnumerable<CustomerPriceCardRepo> ruleConfigs);
        Task<IEnumerable<CustomerPriceCardRepo>> GetPriceCardRuleList(InvoiceGenerateRequest requestDto);
        Task<List<InvoiceDetail>> GetInvoiceListbyPriceCalculations(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings, bool isFromQuotation = false);
        Task<InvoicePdfCreatedResponse> CheckInvoicePdfCreated(InvoicePdfCreatedRequest request);
        Task<List<InvoiceDetail>> CalculateInspectionFeesByCarrefourSampling(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings, bool isFromQuotation = false);
        Task<List<InvoiceDetail>> CalculateInspectionFeesFromCarrefourByManDay(List<InvoiceBookingDetail> orderTransactions, List<InvoiceDetail> invoiceList, CustomerPriceCardRepo ruleConfig, InvoiceGenerateRequest requestDto, List<QuotationBooking> quotationBookings, bool isFromQuotation = false);
        Task<CustomerPriceCardRepo> GetRuleConfigData(InvoiceBookingDetail orderTransactionDto, IEnumerable<CustomerPriceCardRepo> ruleConfigs);

    }
}
