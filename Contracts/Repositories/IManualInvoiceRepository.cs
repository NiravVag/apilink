using DTO.Invoice;
using DTO.InvoicePreview;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IManualInvoiceRepository : IRepository
    {
        IQueryable<ManualInvoiceItemRepo> GetManualInvoices();
        Task<IEnumerable<InvManTranDetail>> GetManualInvoiceItemsByManualInvoiceIds(IEnumerable<int> ids);

        Task<IEnumerable<InvManTranTax>> GetManualInvoiceTransTaxes(int invoiceManualId);
        Task<InvManTransaction> GetManualInvoice(int id);
        Task<bool> IsInvoiceNumberExists(string invoiceNumber);
        IQueryable<ManualInvoiceExportRepoItem> GetManualInvoiceExports();

        Task<List<InvoiceDetailsRepo>> GetManualInvoiceDetails(string invoiceNo);
        Task<List<InvoiceTaxData>> GetManualInvoiceTaxDetails(List<int> manualInvoiceIds);
        Task<int> GetManualInvoiceCountByBookingId(int bookingId);
        Task<int> GetAuditManualInvoiceCountByBookingId(int bookingId);
        Task<IEnumerable<EAQFManualInvoiceData>> GetEaqfManualInvoiceDataById(int manualInvoiceId);
        Task<InvManTransaction> GetManualInvoiceByBookingId(int bookingId);

        Task<InvManTransaction> GetAuditManualInvoiceByBookingId(int bookingId);
        Task<InvManTransaction> GetManualInvoiceByBookingIdAndInvoice(int bookingId, string invoiceno);

        Task<InvManTransaction> GetManualInvoicebyInvoiceNo(string invoiceNo);

    }
}
