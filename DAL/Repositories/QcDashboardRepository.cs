using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.QcDashboard;
using DTO.Quotation;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace DAL.Repositories
{
    public class QcDashboardRepository : Repository, IQcDashboardRepository
    {
        public QcDashboardRepository(API_DBContext context) : base(context)
        {
        }
        public async Task<IEnumerable<QcDashboardCountData>> GetScheduelQcDetails(int qcId,DateTime fromDate,DateTime toDate)
        {
            return await _context.SchScheduleQcs
                        .Where(x => x.Qcid == qcId && x.Active && QcStatusList.Contains(x.Booking.StatusId) && (x.ServiceDate >= fromDate && x.ServiceDate <= toDate))
                        .Select(x => new QcDashboardCountData
                        {
                            BookingId = x.BookingId,
                            CustomerId=x.Booking.CustomerId,
                            FactoryId=x.Booking.FactoryId
                        }).AsNoTracking().ToListAsync();
        }
         
        /// <summary>
        /// Get the Qc Inspections
        /// </summary>
        /// <returns></returns>
        public IQueryable<ScheduleBookingInfo> GetQcInspections()
        {
            return _context.InspTransactions
                        .Where(x => QcStatusList.Contains(x.StatusId)  )
                        .Select(x => new ScheduleBookingInfo
                        {
                            BookingId = x.Id,
                            CustomerId = x.CustomerId,
                            SupplierId = x.SupplierId,
                            FactoryId = x.FactoryId,
                            CustomerName = x.Customer.CustomerName,
                            SupplierName = x.Supplier.SupplierName,
                            FactoryName = x.Factory.SupplierName,
                            FactoryRegionalName = x.Factory.LocalName,
                            ServiceDateFrom = x.ServiceDateFrom,
                            ServiceDateTo = x.ServiceDateTo,
                            StatusName = x.Status.Status,
                            StatusPriority = x.Status.Priority,
                            Office = x.Office.LocationName,
                            OfficeId = x.OfficeId.GetValueOrDefault(),
                            Customer = x.Customer,
                            StatusId = x.StatusId,
                            FirstServiceDateFrom = x.FirstServiceDateFrom,
                            FirstServiceDateTo = x.FirstServiceDateTo,
                            CustomerBookingNo = x.CustomerBookingNo
                        }).AsNoTracking().OrderBy(x => x.ServiceDateFrom).ThenBy(x => x.ServiceDateTo);
        }
         
        /// <summary>
        /// Get the Qc Report
        /// </summary>
        /// <returns></returns>
        public IQueryable<QcReports>  GetQcReport(List<int> bookingIds)
        {
            return  _context.InspProductTransactions.
                Where(x => bookingIds.Contains(x.InspectionId)&&x.Active.HasValue &&x.Active.Value&& x.FbReportId>0)
                .Select(x => new QcReports
            {
                bookingId= x.Inspection.Id,
                ReportId =x.FbReportId,
                ServiceDateTo=x.Inspection.ServiceDateTo,
                ServiceDateFrom = x.Inspection.ServiceDateFrom 
                }).Distinct().AsNoTracking();
        }
        public async Task<IEnumerable<QcReports>> GetQcReportsDetails(int qcId,List<int> bookingIds)
        {
            var fbQcId = await _context.ItUserMasters.FirstOrDefaultAsync(x => x.StaffId == qcId);
             

            return await _context.FbReportQcdetails.
                Where(x=>x.QcId== fbQcId.FbUserId.GetValueOrDefault()&&x.Active.HasValue &&x.Active.Value &&x.FbReportDetail.InspProductTransactions.Any(y=> bookingIds.Contains(y.InspectionId)) )
                .Select(x => new QcReports
                {
                    
                    ReportId = x.FbReportDetail.Id,
                    ServiceDateTo = x.FbReportDetail.ServiceToDate,
                    ServiceDateFrom = x.FbReportDetail.ServiceFromDate
                }).Distinct().AsNoTracking().ToListAsync();
             
        }

        /// <summary>
        /// Get the Qc Rejection Report
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAllRejectionReport(DateTime fromDate, DateTime toDate)
        {
            return await _context.FbReportQcdetails.
               Where(x =>x.Active.HasValue && x.Active.Value && x.FbReportDetail.ResultId== (int)FBReportResult.Fail && (x.FbReportDetail.ServiceToDate >= fromDate && x.FbReportDetail.ServiceToDate <= toDate))
               .Select(x => new QcReports
               {

                   ReportId = x.FbReportDetail.Id,
                   ServiceDateTo = x.FbReportDetail.ServiceToDate,
                   ServiceDateFrom = x.FbReportDetail.ServiceFromDate
               }).Distinct().AsNoTracking().CountAsync();
        }
        public async Task<int> GetQcRejectionReport(int qcId, DateTime fromDate, DateTime toDate)
        {
            var fbQcId = await _context.ItUserMasters.FirstOrDefaultAsync(x => x.StaffId == qcId);
            return await _context.FbReportQcdetails.
               Where(x => x.QcId == fbQcId.FbUserId.GetValueOrDefault() &&x.Active.HasValue && x.Active.Value && x.FbReportDetail.ResultId ==(int) FBReportResult.Fail && (x.FbReportDetail.ServiceToDate >= fromDate && x.FbReportDetail.ServiceToDate <= toDate))
               .Select(x => new QcReports
               {

                   ReportId = x.FbReportDetail.Id,
                   ServiceDateTo = x.FbReportDetail.ServiceToDate,
                   ServiceDateFrom = x.FbReportDetail.ServiceFromDate
               }).Distinct().AsNoTracking().CountAsync();
        }
    }
}
