using Contracts.Repositories;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Enums;
using DTO.Common;

namespace DAL.Repositories
{
    public class CancelBookingRepository : Repository, ICancelBookingRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public CancelBookingRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        { _ApplicationContext = applicationContext; }

        #region CancelBooking
        public Task<int> SaveCancelDetail(InspTranCancel entity)
        {
            _context.InspTranCancels.Add(entity);
            return _context.SaveChangesAsync();
        }
        public Task<int> EditCancel(InspTranCancel entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
        public Task<List<InspCancelReason>> GetBookingCancelReasons(int? customerId)
        {
            var data = _context.InspCancelReasons
                  .Where(x => x.Active);
            if (customerId != null && data.Any(x => x.CustomerId == customerId.Value))
                data = data.Where(x => x.CustomerId == customerId.Value);
            else
                data = data.Where(x => x.IsDefault);
            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                data = data.Where(x => x.IsApi);
            else
                data = data.Where(x => !(x.IsApi));
            return data.ToListAsync();
        }
        public Task<InspTranCancel> GetCancelDetailsById(int bookingId)
        {
            return _context.InspTranCancels
                 .Include(x => x.ReasonType)
                 .Where(x => x.InspectionId == bookingId).OrderByDescending(x => x.Id)
                 .FirstOrDefaultAsync();
        }

        public Task<AudTranCancelReschedule> GetAuditCancelDetailsById(int bookingId)
        {
            return _context.AudTranCancelReschedules
                     .Include(x => x.ReasonType)
                     .FirstOrDefaultAsync(x => x.AuditId == bookingId);
        }

        public async Task<QuInspProduct> BookingQuotationExists(int bookingId)
        {
            return await _context.QuInspProducts.Include(x => x.IdQuotationNavigation).Include(x => x.ProductTran)
                   .Where(x => x.ProductTran.InspectionId == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).AsNoTracking().FirstOrDefaultAsync();
        }
        #endregion CancelBooking
        #region RescheduleBooking
        public int SaveReschedule(InspTranReschedule entity)
        {
            _context.InspTranReschedules.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }
        public Task<int> EditReschedule(InspTranReschedule entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
        public Task<List<InspRescheduleReason>> GetBookingRescheduleReasons(int? customerId)
        {
            var data = _context.InspRescheduleReasons
                  .Where(x => x.Active);
            if (customerId != null && data.Any(x => x.CustomerId == customerId.Value))
                data = data.Where(x => x.CustomerId == customerId.Value);
            else
                data = data.Where(x => x.IsDefault);
            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
                data = data.Where(x => x.IsApi);
            else
                data = data.Where(x => !(x.IsApi));
            return data.ToListAsync();
        }
        public Task<InspTranReschedule> GetRescheduleDetailsById(int bookingId)
        {
            return _context.InspTranReschedules
                 .Where(x => x.InspectionId == bookingId)
                 .Include(x => x.Inspection.InspTranStatusLogs)
                 .Include(x => x.ReasonType)
                 .FirstOrDefaultAsync();
        }

        public Task<AudTranCancelReschedule> GetAuditRescheduleDetailsById(int bookingId)
        {
            return _context.AudTranCancelReschedules
                 .Where(x => x.AuditId == bookingId)
                 .Include(x => x.Audit.AudTranStatusLogs)
                 .Include(x => x.ReasonType)
                 .FirstOrDefaultAsync();
        }

        #endregion RescheduleBooking
        #region CancelRescheduleBooking
        public Task<List<RefCurrency>> GetCurrencies()
        {
            return _context.RefCurrencies.Where(x => x.Active).OrderBy(x => x.CurrencyName).ToListAsync();
        }
        public Task<InspTransaction> GetBookingDetailsById(int bookingId)
        {
            return _context.InspTransactions
                 .Include(x => x.InspTranReschedules)
                 .Include(x => x.InspTranCancels)
                 .Include(x => x.InspPurchaseOrderTransactions)
                 .Where(x => x.Id == bookingId).FirstOrDefaultAsync();
        }
        public Task<InspTransaction> GetCancelBookingDetails(int bookingId)
        {
            return _context.InspTransactions
                   .Include(x => x.Customer)
                   .Include(x => x.Supplier)
                   .Include(x => x.Factory)
                        .Include(x => x.InspProductTransactions)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductCategoryNavigation)

                         .Include(x => x.InspProductTransactions)
                         .ThenInclude(x => x.InspPurchaseOrderTransactions)

                   .Include(x => x.InspTranServiceTypes)
                   .ThenInclude(x => x.ServiceType)
                   .Where(x => x.Id == bookingId).FirstOrDefaultAsync();
        }
        #endregion CancelRescheduleBooking

        public Task<int> UpdateBookingStatusServiceDate(InspTransaction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        //Get the Booking Cancel Reasons
        public async Task<InspCancelReason> GetBookingCancelReasonsById(int reasonId)
        {
            return await _context.InspCancelReasons
                  .Where(x => x.Active && x.Id == reasonId).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get quotation data
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<int> GetBookingQuotationDetails(int bookingId)
        {
            return await _context.QuQuotationInsps.
                Where(x => x.IdBooking == bookingId && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                Select(x => x.IdQuotation).FirstOrDefaultAsync();
        }

        /// <summary>
        /// get quotation data by id
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public async Task<QuQuotation> GetQuotationData(int quotationId)
        {
            return await _context.QuQuotations.
                Where(x => x.Id == quotationId && x.IdStatus != (int)QuotationStatus.Canceled).
                FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get the booking count involved in the quotation
        /// </summary>
        /// <param name="quotationId"></param>
        /// <returns></returns>
        public async Task<int> GetQuotationBookingCount(int quotationId)
        {
            return await _context.QuQuotationInsps.Where(x => x.IdQuotation == quotationId).Select(x => x.IdBooking).CountAsync();
        }
    }
}
