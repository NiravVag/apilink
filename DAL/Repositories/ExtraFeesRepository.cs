using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.ExtraFees;
using DTO.Invoice;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ExtraFeesRepository : Repository, IExtraFeesRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public ExtraFeesRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        /// <summary>
        /// get insp boooking list
        /// </summary>
        /// <returns></returns>
        public IQueryable<BookingRepo> GetBookingNoList()
        {
            return _context.InspTransactions.Select(x => new BookingRepo
            {
                BookingId = x.Id,
                SupplierId = x.SupplierId,
                CustomerId = x.CustomerId,
                FactoryId = x.FactoryId,
                ServiceDate = x.ServiceDateTo,
                CustomerName = x.Customer.CustomerName,
                SupplierName = x.Supplier.SupplierName,
                FactoryName = x.Factory.SupplierName
            });
        }

        /// <summary>
        ///  get extra fees details, tax, extra type details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EditExtraFeesRepo> GetExtraFees(int id)
        {
            return await _context.InvExfTransactions.Select(x => new EditExtraFeesRepo
            {
                AuditId = x.AuditId,
                BankId = x.BankId,
                BilledToId = x.BilledTo,
                BilledName = x.BilledName,
                BilledAddress = x.BilledAddress,
                PaymentTerms = x.PaymentTerms,
                PaymentDuration = x.PaymentDuration,
                BillingEntityId = x.BillingEntityId,
                BookingNumberId = x.InspectionId,
                CurrencyId = x.CurrencyId,
                CustomerId = x.CustomerId,
                ExtraFeeInvoiceNo = x.ExtraFeeInvoiceNo,
                FactoryId = x.FactoryId,
                Id = x.Id,
                InvoiceNumberId = x.InvoiceId,
                PaymentDate = x.PaymentDate,
                PaymentStatusId = x.PaymentStatus,
                ServiceId = x.ServiceId,
                Remarks = x.Remarks,
                TaxAmt = x.TaxAmount,
                TaxValue = x.Tax,
                SupplierId = x.SupplierId,
                SubTotal = x.ExtraFeeSubTotal,
                TotalFees = x.TotalExtraFee,
                StatusName = x.Status.Name,
                StatusId = x.StatusId,
                TaxId = x.InvExtTranTaxes.Select(z => z.TaxId).FirstOrDefault(),
                OfficeId = x.OfficeId,
                ExtraFeeInvoiceDate = x.ExtraFeeInvoiceDate,
                InvoiceCurrencyId = x.InvoiceCurrencyId,
                ExchangeRate = x.ExchangeRate
            }).FirstOrDefaultAsync(x => id == x.Id);
        }
        public async Task<List<ExtraFeeContactData>> GetExtraContactsById(int id)
        {
            return await _context.InvExfContactDetails.Where(x => x.ExtraFeeId.HasValue && x.ExtraFeeId.Value == id).Select(y => new ExtraFeeContactData
            {
                CustomerContactId = y.CustomerContactId,
                CustomerContactName = y.CustomerContact.ContactName,
                FactContactId = y.FactoryContactId,
                FactContactName = y.FactoryContact.ContactName,
                SupContactId = y.SupplierContactId,
                SupContactName = y.SupplierContact.ContactName
            }).AsNoTracking().ToListAsync();
        }
        public async Task<List<EditExtraFeeType>> GetExtraFeesTypeById(int id)
        {
            return await _context.InvExfTranDetails.Where(y => y.Active.Value && y.ExftransactionId.HasValue && y.ExftransactionId.Value == id)
                .Select(y => new EditExtraFeeType
                {
                    Fees = y.ExtraFees,
                    Remarks = y.Remarks,
                    TypeId = y.ExtraFeeType
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// check extra fee exists by billedto and booking number
        /// </summary>
        /// <param name="billedTo"></param>
        /// <param name="bookingNo"></param>
        /// <returns></returns>
        public async Task<bool> ExistsExtraFee(int billedTo, int bookingNo, int extraFeeId, int serviceId)
        {
            return await _context.InvExfTransactions.Where(x => x.StatusId != (int)ExtraFeeStatus.Cancelled && x.Active.Value && x.BilledTo == billedTo &&
            x.InspectionId == bookingNo && x.ServiceId == serviceId && x.Id != extraFeeId).AnyAsync();
        }

        /// <summary>
        /// get invoice No list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetInvoiceNoList(int inspectionId, int? billedToId, int serviceId)
        {
            return await _context.InvAutTranDetails.Where(x => x.InspectionId == inspectionId && x.InvoiceStatus != (int)InvoiceStatus.Cancelled
                && x.InvoiceTo == billedToId && x.ServiceId == serviceId).Select(x => new CommonDataSource { Id = x.Id, Name = x.InvoiceNo }).ToListAsync();
        }

        /// <summary>
        /// get invoice No list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommonDataSource>> GetInvoiceNoListByAudit(int auditId, int? billedToId, int serviceId)
        {
            return await _context.InvAutTranDetails.Where(x => x.AuditId == auditId && x.InvoiceStatus != (int)InvoiceStatus.Cancelled
                && x.InvoiceTo == billedToId && x.ServiceId == serviceId).Select(x => new CommonDataSource { Id = x.Id, Name = x.InvoiceNo }).ToListAsync();
        }

        /// <summary>
        /// get extra fee type records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InvExfTransaction> GetExtraFeeData(int id)
        {
            return await _context.InvExfTransactions.
                Include(x => x.InvExfTranDetails)
                .Include(x => x.InvExfContactDetails)
                .Include(x => x.InvExtTranTaxes).FirstOrDefaultAsync(x => x.Id == id);
        }
        /// <summary>
        /// exists the invoice number check
        /// </summary>
        /// <param name="InvoiceNumber"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsInvoiceNumber(string InvoiceNumber)
        {
            return await _context.InvExfTransactions.Where(x => x.Active.Value && x.ExtraFeeInvoiceNo == InvoiceNumber).AnyAsync();
        }

        /// <summary>
        /// Get the Extra fee data from the transaction table
        /// </summary>
        /// <returns></returns>
        public IQueryable<ExtraFeeSummaryItem> GetExFeeData()
        {
            return _context.InvExfTransactions.Where(x => x.Active.HasValue && x.Active == true)
                .Select(x => new ExtraFeeSummaryItem
                {
                    ExfTranId = x.Id,
                    BookingId = x.InspectionId,
                    CustomerBookingNo = x.Inspection.CustomerBookingNo,
                    CustomerName = x.Inspection.Customer.CustomerName,
                    BilledToId = x.BilledTo ?? 0,
                    BilledTo = x.BilledToNavigation.Label,
                    Currency = x.Currency.CurrencyCodeA,
                    Service = x.Service.Name,
                    ServiceId = x.ServiceId ?? 0,
                    ApplyDate = x.CreatedOn,
                    ServiceDateFrom = x.Inspection.ServiceDateFrom,
                    ServiceDateTo = x.Inspection.ServiceDateTo,
                    ExtraFeeInvoiceNo = x.ExtraFeeInvoiceNo,
                    InvoiceNo = x.Invoice.InvoiceNo,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    Remarks = x.Remarks,
                    TotalAmt = x.TotalExtraFee,
                    CustomerId = x.Inspection.CustomerId,
                    SupplierId = x.Inspection.SupplierId,
                    InvoiceCurrency = x.InvoiceCurrency.CurrencyCodeA,
                    ExchangeRate = x.ExchangeRate
                });
        }

        /// <summary>
        /// Get the Extra fee data from the transaction details table
        /// </summary>
        /// <returns></returns>
        public IQueryable<ExtraFeeSummaryItem> GetExFeeDetailsData()
        {
            return _context.InvExfTranDetails.Where(x => x.Active.HasValue && x.Active == true)
                .Select(x => new ExtraFeeSummaryItem
                {
                    BookingId = x.Exftransaction.InspectionId,
                    CustomerBookingNo = x.Exftransaction.Inspection.CustomerBookingNo,
                    CustomerName = x.Exftransaction.Inspection.Customer.CustomerName,
                    BilledTo = x.Exftransaction.BilledToNavigation.Label,
                    BilledToId = x.Exftransaction.BilledTo ?? 0,
                    Currency = x.Exftransaction.Currency.CurrencyCodeA,
                    Service = x.Exftransaction.Service.Name,
                    ServiceId = x.Exftransaction.ServiceId ?? 0,
                    ApplyDate = x.Exftransaction.CreatedOn,
                    ServiceDateFrom = x.Exftransaction.Inspection.ServiceDateFrom,
                    ServiceDateTo = x.Exftransaction.Inspection.ServiceDateTo,
                    ExtraFeeInvoiceNo = x.Exftransaction.ExtraFeeInvoiceNo,
                    InvoiceNo = x.Exftransaction.Invoice.InvoiceNo,
                    StatusName = x.Exftransaction.Status.Name,
                    StatusId = x.Exftransaction.StatusId,
                    SupplierName = x.Exftransaction.Inspection.Supplier.SupplierName,
                    Remarks = x.Exftransaction.Remarks,
                    ExFeeType = x.ExtraFeeTypeNavigation.Name,
                    ExtraTypefee = x.ExtraFees,
                    ExtraTypeRemarks = x.Remarks,
                    TotalAmt = x.Exftransaction.TotalExtraFee,
                    CustomerId = x.Exftransaction.Inspection.CustomerId,
                    SupplierId = x.Exftransaction.Inspection.SupplierId,
                    InvoiceDate = x.Exftransaction.ExtraFeeInvoiceDate,
                    PaymentDate = x.Exftransaction.PaymentDate,
                    PaymentStatus = x.Exftransaction.PaymentStatusNavigation.Name,
                    InvoiceCurrency = x.Exftransaction.InvoiceCurrency.CurrencyCodeA,
                    ExchangeRate = x.Exftransaction.ExchangeRate,
                    ExfTranId = x.ExftransactionId
                }).OrderBy(x => x.BookingId);
        }

        /// <summary>
        /// Get the Extra fee data from the transaction table
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExtraFeeSummaryItem>> GetExFeeTranDetails(List<int> exfTranIdList)
        {
            return await _context.InvExfTranDetails.Where(x => x.Active.HasValue && x.Active.Value && x.ExftransactionId.HasValue && exfTranIdList.Contains(x.ExftransactionId.Value))
                .Select(x => new ExtraFeeSummaryItem
                {
                    ExfTranId = x.ExftransactionId,
                    ExFeeType = x.ExtraFeeTypeNavigation.Name,
                    TotalAmt = x.ExtraFees
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Fetch the extra fee status List
        /// </summary>
        /// <returns>List DataSource</returns>
        public async Task<List<CommonDataSource>> GetExtraFeeStatus()
        {
            return await _context.InvExfStatuses.Where(x => x.Active.HasValue && x.Active.Value).AsNoTracking().
                Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        /// <summary>
        /// get audit booking list
        /// </summary>
        /// <returns></returns>
        public IQueryable<BookingRepo> GetAuditBookingList()
        {
            return _context.AudTransactions.Select(x => new BookingRepo
            {
                BookingId = x.Id,
                SupplierId = x.SupplierId,
                CustomerId = x.CustomerId,
                FactoryId = x.FactoryId,
                CustomerName = x.Customer.CustomerName,
                FactoryName = x.Factory.SupplierName,
                SupplierName = x.Supplier.SupplierName
            });
        }

        /// <summary>
        /// Get the Extra fee data from the transaction table
        /// </summary>
        /// <returns></returns>
        public IQueryable<ExtraFeeSummaryItem> GetAuditExFeeData()
        {
            return _context.InvExfTransactions.Where(x => x.Active.Value)
                .Select(x => new ExtraFeeSummaryItem
                {
                    ExfTranId = x.Id,
                    BookingId = x.AuditId,
                    CustomerName = x.Audit.Customer.CustomerName,
                    BilledToId = x.BilledTo,
                    BilledTo = x.BilledToNavigation.Label,
                    Currency = x.Currency.CurrencyCodeA,
                    Service = x.Service.Name,
                    ServiceId = x.ServiceId,
                    ApplyDate = x.CreatedOn,
                    ServiceDateFrom = x.Audit.ServiceDateFrom,
                    ServiceDateTo = x.Audit.ServiceDateTo,
                    ExtraFeeInvoiceNo = x.ExtraFeeInvoiceNo,
                    InvoiceNo = x.Invoice.InvoiceNo,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    SupplierName = x.Audit.Supplier.SupplierName,
                    Remarks = x.Remarks,
                    TotalAmt = x.TotalExtraFee,
                    CustomerId = x.Audit.CustomerId,
                    SupplierId = x.Audit.SupplierId,
                    InvoiceCurrency = x.InvoiceCurrency.CurrencyCodeA,
                    ExchangeRate = x.ExchangeRate
                });
        }

        /// <summary>
        /// Get the Extra fee data from the transaction details with audit table 
        /// </summary>
        /// <returns></returns>
        public IQueryable<ExtraFeeSummaryItem> GetExtraFeeDetailsAuditData()
        {
            return _context.InvExfTranDetails.Where(x => x.Active.Value)
                .Select(x => new ExtraFeeSummaryItem
                {
                    BookingId = x.Exftransaction.AuditId,
                    CustomerName = x.Exftransaction.Audit.Customer.CustomerName,
                    BilledTo = x.Exftransaction.BilledToNavigation.Label,
                    BilledToId = x.Exftransaction.BilledTo ?? 0,
                    Currency = x.Exftransaction.Currency.CurrencyCodeA,
                    Service = x.Exftransaction.Service.Name,
                    ServiceId = x.Exftransaction.ServiceId ?? 0,
                    ApplyDate = x.Exftransaction.CreatedOn,
                    ServiceDateFrom = x.Exftransaction.Audit.ServiceDateFrom,
                    ServiceDateTo = x.Exftransaction.Audit.ServiceDateTo,
                    ExtraFeeInvoiceNo = x.Exftransaction.ExtraFeeInvoiceNo,
                    InvoiceNo = x.Exftransaction.Invoice.InvoiceNo,
                    StatusName = x.Exftransaction.Status.Name,
                    StatusId = x.Exftransaction.StatusId,
                    SupplierName = x.Exftransaction.Audit.Supplier.SupplierName,
                    Remarks = x.Exftransaction.Remarks,
                    ExFeeType = x.ExtraFeeTypeNavigation.Name,
                    ExtraTypefee = x.ExtraFees,
                    ExtraTypeRemarks = x.Remarks,
                    TotalAmt = x.Exftransaction.TotalExtraFee,
                    CustomerId = x.Exftransaction.Audit.CustomerId,
                    SupplierId = x.Exftransaction.Audit.SupplierId,
                    InvoiceDate = x.Exftransaction.ExtraFeeInvoiceDate,
                    PaymentDate = x.Exftransaction.PaymentDate,
                    PaymentStatus = x.Exftransaction.PaymentStatusNavigation.Name,
                    ExfTranId = x.ExftransactionId,
                    ExchangeRate = x.Exftransaction.ExchangeRate,
                    InvoiceCurrency = x.Exftransaction.InvoiceCurrency.CurrencyName
                }).OrderBy(x => x.BookingId);
        }
    }
}
