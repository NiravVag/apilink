using Contracts.Repositories;
using DTO.Invoice;
using DTO.InvoicePreview;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ManualInvoiceRepository : Repository, IManualInvoiceRepository
    {
        public ManualInvoiceRepository(API_DBContext context) : base(context)
        {

        }

        public async Task<InvManTransaction> GetManualInvoice(int id)
        {
            return await _context.InvManTransactions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<InvManTransaction> GetManualInvoicebyInvoiceNo(string invoiceNo)
        {
            return await _context.InvManTransactions.FirstOrDefaultAsync(x => x.InvoiceNo == invoiceNo);
        }

        public async Task<InvManTransaction> GetManualInvoiceByBookingId(int bookingId)
        {
            return await _context.InvManTransactions.FirstOrDefaultAsync(x => x.BookingNo == bookingId);
        }

        public async Task<InvManTransaction> GetAuditManualInvoiceByBookingId(int bookingId)
        {
            return await _context.InvManTransactions.FirstOrDefaultAsync(x => x.AuditId == bookingId);
        }

        public async Task<InvManTransaction> GetManualInvoiceByBookingIdAndInvoice(int bookingId, string invoiceno)
        {
            return await _context.InvManTransactions
                .Include(x => x.InvManTranDetails)
                .FirstOrDefaultAsync(x => x.BookingNo == bookingId && x.InvoiceNo == invoiceno);
        }

        public async Task<IEnumerable<InvManTranDetail>> GetManualInvoiceItemsByManualInvoiceIds(IEnumerable<int> ids)
        {
            return await _context.InvManTranDetails.Where(x => ids.Contains(x.InvManualId) && x.Active.HasValue && x.Active.Value).AsNoTracking().ToListAsync();
        }

        public IQueryable<ManualInvoiceItemRepo> GetManualInvoices()
        {
            return _context.InvManTransactions.Where(x => x.Status != (int)InvoiceStatus.Cancelled).Select(x => new ManualInvoiceItemRepo()
            {
                Id = x.Id,
                ToDate = x.ToDate,
                InvoiceDate = x.InvoiceDate,
                InvoiceTo = x.InvoiceToId,
                InvoiceToName = x.InvoiceTo.Label,
                FromDate = x.FromDate,
                CustomerId = x.CustomerId,
                CustomerName = x.Customer.CustomerName,
                Attn = x.Attn,
                InvoiceNo = x.InvoiceNo,
                BilledName = x.BilledName,
                CreatedBy = x.CreatedByNavigation.FullName,
                CreatedOn = x.CreatedOn,
                Service = x.Service.Name,
                ServiceType = x.ServiceTypeNavigation.Name,
                Tax = x.Tax,
                TaxAmount = x.TaxAmount,
                TotalFee = x.TotalAmount,
                UpdatedBy = x.UpdatedByNavigation.FullName,
                UpdatedOn = x.UpdatedOn,
                StatusId = x.Status,
                StatusName = x.StatusNavigation.Name,
                IsEAQF = x.BookingNoNavigation.IsEaqf
            });
        }
        public async Task<bool> IsInvoiceNumberExists(string invoiceNumber)
        {
            var lowerInvoiceNumber = invoiceNumber.ToLower();
            return await _context.InvManTransactions.Where(x => x.InvoiceNo.ToLower() == lowerInvoiceNumber).AnyAsync()
                || await _context.InvAutTranDetails.Where(x => x.InvoiceNo.ToLower() == lowerInvoiceNumber).AnyAsync()
                || await _context.InvExfTransactions.Where(x => x.ExtraFeeInvoiceNo.ToLower() == lowerInvoiceNumber).AnyAsync();
        }

        public IQueryable<ManualInvoiceExportRepoItem> GetManualInvoiceExports()
        {
            return _context.InvManTranDetails.Where(x => x.InvManual.Status != (int)InvoiceStatus.Cancelled).Select(x => new ManualInvoiceExportRepoItem()
            {
                ToDate = x.InvManual.ToDate,
                InvoiceDate = x.InvManual.InvoiceDate,
                InvoiceTo = x.InvManual.InvoiceToId,
                InvoiceToName = x.InvManual.InvoiceTo.Label,
                FromDate = x.InvManual.FromDate,
                CustomerId = x.InvManual.CustomerId,
                CustomerName = x.InvManual.Customer.CustomerName,
                Attn = x.InvManual.Attn,
                InvoiceNo = x.InvManual.InvoiceNo,
                BilledName = x.InvManual.BilledName,
                CreatedBy = x.InvManual.CreatedByNavigation.FullName,
                CreatedOn = x.InvManual.CreatedOn,
                Service = x.InvManual.Service.Name,
                ServiceType = x.InvManual.ServiceTypeNavigation.Name,
                Tax = x.InvManual.Tax,
                SubTotal = x.Subtotal,
                UpdatedBy = x.UpdatedByNavigation.FullName,
                UpdatedOn = x.UpdatedOn,
                StatusId = x.InvManual.Status,
                ServiceFee = x.ServiceFee,
                ExpChargeBack = x.ExpChargeBack,
                OtherCost = x.OtherCost,
                StatusName = x.InvManual.StatusNavigation.Name
            });
        }

        public Task<List<InvoiceDetailsRepo>> GetManualInvoiceDetails(string invoiceNo)
        {
            return _context.InvManTranDetails.Where(x => x.InvManual.InvoiceNo == invoiceNo && x.Active == true && x.InvManual.Status != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceDetailsRepo()
                {
                    Id = x.InvManual.Id,
                    CustomerName = x.InvManual.Customer.CustomerName,
                    InvoiceDate = x.InvManual.InvoiceDate,
                    InvoiceNumber = x.InvManual.InvoiceNo,
                    PaymentDuration = x.InvManual.PaymentDuration.GetValueOrDefault().ToString(),
                    PaymentTerm = x.InvManual.PaymentTerms,
                    Currency = x.InvManual.CurrencyNavigation.CurrencyCodeA,
                    ServiceId = x.InvManual.ServiceId,
                    BilledAddress = x.InvManual.BilledAddress,
                    BilledName = x.InvManual.BilledName,

                    ManualChargeBack = x.ExpChargeBack,
                    ManualOtherCost = x.OtherCost,
                    ManualServiceFee = x.ServiceFee,
                    ManualDescription = x.Description,
                    ManualRemarks = x.Remarks,

                    OfficeAddress = x.InvManual.Office.Address,
                    OfficeFax = x.InvManual.Office.Fax,
                    OfficeMail = x.InvManual.Office.Mail,
                    OfficeName = x.InvManual.Office.Name,
                    OfficePhone = x.InvManual.Office.Phone,
                    OfficeWebsite = x.InvManual.Office.Website,
                    AccountId = x.InvManual.Bank.Id,
                    AccountNumber = x.InvManual.Bank.AccountNumber,
                    BankAddress = x.InvManual.Bank.BankAddress,
                    BankName = x.InvManual.Bank.BankName,
                    BankSwiftCode = x.InvManual.Bank.SwiftCode,
                    AccountName = x.InvManual.Bank.AccountName,
                    InvoiceTo = x.InvManual.InvoiceToId,
                    TotalInvoiceFees = x.Subtotal,
                    TaxValue = x.InvManual.Tax,
                    ServiceType = x.InvManual.ServiceTypeNavigation.Name,
                    Service = x.InvManual.Service.Name,
                    ServiceFromDate = x.InvManual.FromDate,
                    ServiceToDate = x.InvManual.ToDate,
                    CreatedOn = x.CreatedOn,
                    Country = x.InvManual.Country.CountryName,
                    Attention = x.InvManual.Attn
                }).ToListAsync();
        }

        public async Task<IEnumerable<InvManTranTax>> GetManualInvoiceTransTaxes(int invoiceManualId)
        {
            return await _context.InvManTranTaxes.Where(x => x.ManInvoiceId == invoiceManualId).AsNoTracking().ToListAsync();
        }

        public async Task<List<InvoiceTaxData>> GetManualInvoiceTaxDetails(List<int> manualInvoiceIds)
        {
            return await _context.InvManTranTaxes.Where(x => manualInvoiceIds.Contains(x.ManInvoiceId.GetValueOrDefault()))
                .Select(x => new InvoiceTaxData
                {
                    InvoiceId = x.ManInvoiceId,
                    TaxId = x.TaxId
                }).ToListAsync();
        }

        public async Task<int> GetManualInvoiceCountByBookingId(int bookingId)
        {
            return await _context.InvManTransactions.Where(x => x.BookingNo == bookingId).CountAsync();
        }

        public async Task<int> GetAuditManualInvoiceCountByBookingId(int bookingId)
        {
            return await _context.InvManTransactions.Where(x => x.AuditId == bookingId).CountAsync();
        }

        public async Task<IEnumerable<EAQFManualInvoiceData>> GetEaqfManualInvoiceDataById(int manualInvoiceId)
        {
            return await _context.InvManTransactions.Where(x => x.Id == manualInvoiceId).Select(y => new EAQFManualInvoiceData()
            {
                Attn = y.Attn,
                PhoneNumber = y.Customer.Phone,
                BilledAddress = y.BilledAddress,
                BilledName = y.BilledName,
                Currency = y.CurrencyNavigation.CurrencyName,
                Email = y.Email,
                InvoiceNo = y.InvoiceNo,
                InvoiceDateTime = y.InvoiceDate,
                PaymentMode = y.PaymentModeNavigation.Name,
                PaymentRef = y.PaymentRef,
                CurrencyCode = y.CurrencyNavigation.CurrencyCodeA
            }).AsNoTracking().ToListAsync();
        }
    }
}