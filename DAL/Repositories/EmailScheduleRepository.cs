using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EmailScheduleRepository : Repository, IEmailScheduleRepository
    {
        //  private readonly ICombineOrdersManager _combineOrdermanager = null;
        public EmailScheduleRepository(API_DBContext context) : base(context)
        {
            // _combineOrdermanager = combineOrdermanager;
        }


        public async Task<IEnumerable<ScheduleStaffItem>> GetScheduledDetailsForQCsByServiceDate(DateTime startDate, DateTime endDate, List<int> office)
        {

            var resultQuery = _context.SchScheduleQcs
                .Where(x => x.Active && x.Booking.StatusId == (int)BookingStatus.AllocateQC && x.ServiceDate >= startDate && x.IsVisibleToQc.Value
                                                                && x.ServiceDate <= endDate);

            // Applying office filter if available
            if (office.Count() > 0)
            {
                resultQuery = resultQuery.Where(x => office.Contains(x.Booking.OfficeId.Value));
            }

            return await resultQuery.Select(x => new ScheduleStaffItem
            {
                BookingId = x.BookingId,
                ServiceId = (int)Service.InspectionId,
                Id = x.Qc.Id,
                QCType = x.Qctype,
                ServiceDate = x.ServiceDate,
                Name = x.Qc.PersonName,
                ActualManDay = x.ActualManDay,
                CompanyEmail = x.Qc.CompanyEmail,
                IsChinaCountry = x.Qc.NationalityCountryId == (int)CountryEnum.China
            }).AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<ScheduleQCEntityData>> GetScheduledInspectionByInspectionList(List<int> bookingIds)
        {
            var scheduleEntityDataList = await _context.InspTransactions.
                         Where(x => bookingIds.Contains(x.Id))
                        .Select(res => new ScheduleQCEntityData
                        {
                            BookingId = res.Id,
                            MisssionId = res.FbMissionId,
                            CustomerName = res.Customer.CustomerName,
                            CustomerId = res.CustomerId,
                            SupplierName = res.Supplier.SupplierName,
                            FactoryName = res.Factory.SupplierName,
                            FactoryRegionalName = res.Factory.LocalName,
                            FactoryContactEmail = res.Factory.Email,
                            FactoryPhNo = res.Factory.Phone,
                            ServiceDateFrom = res.ServiceDateFrom,
                            ServiceDateTo = res.ServiceDateTo,
                            ScheduleComments = res.ScheduleComments,
                            //OfficeLocation = res.Office,
                            QCBookingcomments = res.QcbookingComments,
                            BusinessLine = res.BusinessLine,
                            BookingOfficeLocation = res.Office.LocationName
                        }).AsNoTracking().ToListAsync();

            return scheduleEntityDataList;

        }

        public async Task<List<CommonDataSource>> GetCSName(List<int> bookingIds)
        {
            return await _context.SchScheduleCs.
                        Where(x => bookingIds.Contains(x.BookingId) && x.Active)
                        .Select(x => new CommonDataSource
                        {
                            Id = x.BookingId,
                            Name = x.Cs.PersonName
                        }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<ScheduleStaffItem>> GetAuditorDetailsByServiceDate(DateTime startDate, DateTime endDate, List<int> office)
        {

            var resultQuery = _context.AudTranAuditors
                .Where(x => x.Active && x.Audit.StatusId == (int)AuditStatus.Scheduled && !((x.Audit.ServiceDateFrom > endDate) || (x.Audit.ServiceDateTo < startDate)));

            // Applying office filter if available
            if (office.Count() > 0)
            {
                resultQuery = resultQuery.Where(x => office.Contains(x.Audit.OfficeId.Value));
            }

            return await resultQuery.Select(x => new ScheduleStaffItem
            {
                BookingId = x.AuditId,
                ServiceId = (int)Service.AuditId,
                Id = x.Staff.Id,
                ServiceDate = x.Audit.ServiceDateFrom,
                Name = x.Staff.PersonName,
                CompanyEmail = x.Staff.CompanyEmail,
                IsChinaCountry = x.Staff.NationalityCountryId == (int)CountryEnum.China
            }).AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<ScheduleQCEntityData>> GetAuditDataByAuditIds(List<int> auditIds)
        {
            var scheduleEntityDataList = await _context.AudTransactions.
                         Where(x => auditIds.Contains(x.Id))
                        .Select(res => new ScheduleQCEntityData
                        {
                            BookingId = res.Id,
                            MisssionId = res.FbmissionId,
                            CustomerName = res.Customer.CustomerName,
                            CustomerId = res.CustomerId,
                            SupplierName = res.Supplier.SupplierName,
                            FactoryName = res.Factory.SupplierName,
                            FactoryRegionalName = res.Factory.LocalName,
                            FactoryContactEmail = res.Factory.Email,
                            FactoryPhNo = res.Factory.Phone,
                            ServiceDateFrom = res.ServiceDateFrom,
                            ServiceDateTo = res.ServiceDateTo,
                            ScheduleComments = res.ApiBookingComments,
                            BookingOfficeLocation = res.Office.LocationName,
                            ReportTitle = res.FbreportTitle
                        }).AsNoTracking().ToListAsync();

            return scheduleEntityDataList;

        }
    }
}
