using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
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
    public interface IInvoiceStatusRepository : IRepository
    {
        IQueryable<InspTransaction> GetBookingDetailsQuery();
        IQueryable<AudTransaction> GetAuditDetailsQuery();
        Task<List<InvoiceItem>> GetInspBookingInvoiceList(List<int?> bookingIds);
        Task<List<InvoiceItem>> GetAuditBookingInvoiceList(List<int?> bookingIds);
        Task<List<InvoiceCommunicationTableRepo>> GetInvoiceCommunicationData(string invoiceNo);
        Task<List<InvoiceCommunicationTableRepo>> GetInvoiceCommunicationByInvoiceNoList(IEnumerable<string> invoiceNoList);
        Task<List<InvoiceItem>> GetExtraFeeInvoiceList(List<int?> bookingIds);

        Task<List<InvoiceItem>> GetExtraFeeInvoiceListByAuditIds(IEnumerable<int> auditIds);
    }
}
