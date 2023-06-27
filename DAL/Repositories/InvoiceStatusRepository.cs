using Contracts.Repositories;
using DTO.Invoice;
using Entities;
using Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DTO.ExtraFees;

namespace DAL.Repositories
{
    public class InvoiceStatusRepository : Repository, IInvoiceStatusRepository
    {
        public InvoiceStatusRepository(API_DBContext context) : base(context)
        {
        }
        /// <summary>
        /// Fetch booking details 
        /// </summary>
        /// <returns>IQueryable InspTransaction</returns>
        public IQueryable<InspTransaction> GetBookingDetailsQuery()
        {
            return _context.InspTransactions.Where(x => x.StatusId != (int)BookingStatus.Cancel);
        }
        /// <summary>
        /// Fetch Audit details
        /// </summary>
        /// <returns>IQueryable InspTransaction</returns>
        public IQueryable<AudTransaction> GetAuditDetailsQuery()
        {
            return _context.AudTransactions.Where(x => x.StatusId != (int)AuditStatus.Cancel);
        }

        /// <summary>
        /// get invoice details by inspection booking number
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceItem>> GetInspBookingInvoiceList(List<int?> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.InspectionId) && x.InvoiceStatus != (int)InvoiceStatus.Cancelled)
                .Select(x => new InvoiceItem()
                {
                    InvoiceNo = x.InvoiceNo,
                    Id = x.Id,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Service.Name,
                    InvoiceTo = x.InvoiceToNavigation.Label,
                    InvoiceTypeId = x.InvoiceType,
                    InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                    StatusId = x.InvoiceStatus,
                    StatusName = x.InvoiceStatusNavigation.Name,
                    InvoiceDate = x.InvoiceDate,
                    IsInspection = x.IsInspection,
                    PaymentStatusId = x.InvoicePaymentStatus,
                    PaymentStatusName = x.InvoicePaymentStatusNavigation.Name,
                    PaymentDate = x.InvoicePaymentDate,
                    BookingId = x.InspectionId,
                    InvoiceAmount = x.TotalInvoiceFees,
                    CurrencyCode = x.InvoiceCurrencyNavigation.CurrencyCodeA
                }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Extra fee invoice list fetch by booking numbers
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceItem>> GetExtraFeeInvoiceList(List<int?> bookingIds)
        {
            return await _context.InvExfTransactions.Where(x => bookingIds.Contains(x.InspectionId) && x.StatusId != (int)ExtraFeeStatus.Cancelled)
                .Select(x => new InvoiceItem()
                {
                    InvoiceNo = x.ExtraFeeInvoiceNo,
                    Id = x.Id,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Service.Name,
                    InvoiceTo = x.BilledToNavigation.Label,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    InvoiceDate = x.ExtraFeeInvoiceDate,
                    PaymentStatusId = x.PaymentStatus,
                    PaymentStatusName = x.PaymentStatusNavigation.Name,
                    PaymentDate = x.PaymentDate,
                    BookingId = x.InspectionId,
                    InvoiceAmount = x.TotalExtraFee,
                    CurrencyCode = x.InvoiceCurrency.CurrencyCodeA
                }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// Extra fee invoice list fetch by booking numbers
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceItem>> GetExtraFeeInvoiceListByAuditIds(IEnumerable<int> auditIds)
        {
            return await _context.InvExfTransactions.Where(x => auditIds.Contains(x.AuditId.GetValueOrDefault()) && x.StatusId != (int)ExtraFeeStatus.Cancelled)
                .Select(x => new InvoiceItem()
                {
                    InvoiceNo = x.ExtraFeeInvoiceNo,
                    Id = x.Id,
                    ServiceId = x.ServiceId,
                    ServiceName = x.Service.Name,
                    InvoiceTo = x.BilledToNavigation.Label,
                    StatusId = x.StatusId,
                    StatusName = x.Status.Name,
                    InvoiceDate = x.ExtraFeeInvoiceDate,
                    PaymentStatusId = x.PaymentStatus,
                    PaymentStatusName = x.PaymentStatusNavigation.Name,
                    PaymentDate = x.PaymentDate,
                    BookingId = x.InspectionId,
                    InvoiceAmount = x.TotalExtraFee,
                    CurrencyCode = x.InvoiceCurrency.CurrencyCodeA
                }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<InvoiceItem>> GetAuditBookingInvoiceList(List<int?> bookingIds)
        {
            return await _context.InvAutTranDetails.Where(x => bookingIds.Contains(x.AuditId) && x.InvoiceStatus != (int)InvoiceStatus.Cancelled).Select(x => new InvoiceItem()
            {
                InvoiceNo = x.InvoiceNo,
                Id = x.Id,
                BookingId = x.AuditId,
                ServiceId = x.ServiceId,
                ServiceName = x.Service.Name,
                InvoiceTo = x.InvoiceToNavigation.Label,
                InvoiceTypeId = x.InvoiceType,
                InvoiceTypeName = x.InvoiceTypeNavigation.Name,
                StatusId = x.InvoiceStatus,
                StatusName = x.InvoiceStatusNavigation.Name,
                InvoiceDate = x.InvoiceDate,
                IsInspection = x.IsInspection,
                PaymentStatusId = x.InvoicePaymentStatus,
                PaymentStatusName = x.InvoicePaymentStatusNavigation.Name,
                PaymentDate = x.InvoicePaymentDate,
                InvoiceAmount = x.TotalInvoiceFees,
                CurrencyCode = x.InvoiceCurrencyNavigation.CurrencyCodeA
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Invoice communication data get by invoice number
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public async Task<List<InvoiceCommunicationTableRepo>> GetInvoiceCommunicationData(string invoiceNo)
        {
            return await _context.InvAutTranCommunications.Where(x => x.Active.Value && x.InvoiceNumber == invoiceNo).Select(x => new InvoiceCommunicationTableRepo
            {
                Comment = x.Comment,
                CreatedBy = x.CreatedByNavigation.FullName,
                CreatedOn = x.CreatedOn
            }).AsNoTracking().OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        /// <summary>
        /// invoice comminucation data
        /// </summary>
        /// <param name="invoiceNoList"></param>
        /// <returns></returns>
        public async Task<List<InvoiceCommunicationTableRepo>> GetInvoiceCommunicationByInvoiceNoList(IEnumerable<string> invoiceNoList)
        {
            return await _context.InvAutTranCommunications.Where(x => x.Active.Value && invoiceNoList.Contains(x.InvoiceNumber))
                .Select(x => new InvoiceCommunicationTableRepo
                {
                    Comment = x.Comment,
                    CreatedBy = x.CreatedByNavigation.FullName,
                    CreatedOn = x.CreatedOn,
                    InvoiceNumber = x.InvoiceNumber
                }).AsNoTracking().ToListAsync();
        }
    }
}