using Contracts.Repositories;
using DTO.Common;
using DTO.Manday;
using DTO.Schedule;
using DTO.UtilizationDashboard;
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
    public class UtilizationDashboardRepository: Repository, IUtilizationDashboardRepository
    {
        public UtilizationDashboardRepository(API_DBContext context) : base(context)
        {
        }

        public IQueryable<BookingAuditItems> GetAllInspections()
        {
            return _context.InspTransactions
                .Select(x => new BookingAuditItems
                {
                    Id = x.Id,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    Office = x.Office.LocationName,
                    OfficeId = x.OfficeId,
                    StatusId = x.StatusId,
                    FactoryId = x.FactoryId
                });
        }

        //Fetch all the audits
        public IQueryable<BookingAuditItems> GetAllAudits()
        {
            return _context.AudTransactions.
                Select(x => new BookingAuditItems
                {
                    Id = x.Id,
                    OfficeId = x.OfficeId,
                    Office = x.Office.LocationName,
                    StatusId = x.StatusId,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    FactoryId = x.FactoryId
                });
        }

        public async Task<IEnumerable<QCStaffInfo>> GetQCListByLocationForForecast(List<int> location, int serviceId)
        {
            IQueryable<HrStaff> data;

            if (serviceId == (int)Service.InspectionId)
            {
                data = _context.HrStaffs
                    //.Include(x => x.HrLeaves)
                    .Where(x => x.Active.HasValue && x.Active.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Inspector)
                            && x.IsForecastApplicable.HasValue && x.IsForecastApplicable.Value);
            }

            else
            {
                data = _context.HrStaffs
                    //.Include(x => x.HrLeaves)
                    .Where(x => x.Active.HasValue && x.Active.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Auditor));
            }

            if (location != null && location.Any())
            {
                //  api guangzhou office should contain sz office qc list
                if(location.Contains((int)OfficeLocationEnum.API_Guangzhou) && !location.Contains((int)OfficeLocationEnum.API_Shenzhen))
                {
                    location.Add((int)OfficeLocationEnum.API_Shenzhen);
                }

                data = data.Where(x => location.Contains(x.LocationId.Value));
            }

            //keep shenzhen office qc in guagzhou office.
            var result = await data.Select(x => new QCStaffInfo
            {
                Id = x.Id,
                Name = x.PersonName,
                Location = x.LocationId.GetValueOrDefault()!=(int)OfficeLocationEnum.API_Shenzhen? x.Location.LocationName: GuangzhouOffice,
                LocationId = x.LocationId.GetValueOrDefault() != (int)OfficeLocationEnum.API_Shenzhen ? x.LocationId.GetValueOrDefault(): (int)OfficeLocationEnum.API_Guangzhou,
                EmployeeType = x.EmployeeTypeId,
                LeaveQC = x.HrLeaves

            }).AsNoTracking().ToListAsync();

            return result;
        }

        //Fetch the Quotation Man Day for the Audit
        public async Task<List<QuotationManday>> GetQuotationManDayAudit(List<int> auditIds)
        {
            return await _context.AudTranAuditors
                .Where(x => auditIds.Contains(x.AuditId) && x.Active)
                .Select(x => new QuotationManday
                {
                    ServiceDateFrom = x.Audit.ServiceDateFrom,
                    ServiceDateTo = x.Audit.ServiceDateTo,
                    StaffId = x.StaffId,
                    LocationName = x.Audit.Office.LocationName,
                    LocationId = x.Audit.OfficeId.GetValueOrDefault(),
                    StaffType=x.Staff.EmployeeTypeId,
                    factoryId=x.Audit.FactoryId
                }).AsNoTracking().ToListAsync();
        }

        //Get Leave List by date range and Group by Location
        public async Task<List<LeaveData>> GetHrLeaves(DateTime startdate, DateTime enddate, List<int> officeIdList, int serviceId)
            {
                IQueryable<LeaveData> res;
                if (serviceId == (int)Service.AuditId)
                {
                    res = _context.HrLeaves
                        .Join(_context.HrStaffProfiles, leave => leave.StaffId, profile => profile.StaffId,
                            (leave, profile) => new { HrLeaves = leave, HrStaffProfiles = profile })
                         .Where(x => x.HrLeaves.DateBegin <= enddate && x.HrLeaves.DateEnd >= startdate && x.HrLeaves.Status == (int)LeaveStatus.Approved
                         && x.HrLeaves.Staff.EmployeeTypeId == (int)StaffType.Permanent && x.HrStaffProfiles.ProfileId == (int)HRProfile.Auditor && x.HrLeaves.Staff.IsForecastApplicable.HasValue && x.HrLeaves.Staff.IsForecastApplicable.Value)
                         .Select(x => new LeaveData
                         {
                             LocationId = x.HrLeaves.Staff.LocationId.GetValueOrDefault(),
                             LocationName = x.HrLeaves.Staff.Location.LocationName,
                             NoOfDays = x.HrLeaves.NumberOfDays
                         });
                }

                else
                {
                    res = _context.HrLeaves
                        .Join(_context.HrStaffProfiles, leave => leave.StaffId, profile => profile.StaffId,
                            (leave, profile) => new { HrLeaves = leave, HrStaffProfiles = profile })
                         .Where(x => x.HrLeaves.DateBegin <= enddate && x.HrLeaves.DateEnd >= startdate && x.HrLeaves.Status == (int)LeaveStatus.Approved
                         && x.HrLeaves.Staff.EmployeeTypeId == (int)StaffType.Permanent && x.HrStaffProfiles.ProfileId == (int)HRProfile.Inspector && x.HrLeaves.Staff.IsForecastApplicable.HasValue && x.HrLeaves.Staff.IsForecastApplicable.Value)
                         .Select(x => new LeaveData
                         {
                             LocationId = x.HrLeaves.Staff.LocationId.GetValueOrDefault(),
                             LocationName = x.HrLeaves.Staff.Location.LocationName,
                             NoOfDays = x.HrLeaves.NumberOfDays
                         });
                }

                if (officeIdList != null && officeIdList.Any())
                {
                    //  api guangzhou office should contain sz office qc list
                    if (officeIdList.Contains((int)OfficeLocationEnum.API_Guangzhou) && !officeIdList.Contains((int)OfficeLocationEnum.API_Shenzhen))
                    {
                        officeIdList.Add((int)OfficeLocationEnum.API_Shenzhen);
                    }
                    res = res.Where(x => officeIdList.Contains(x.LocationId));
                }

                var result = await res.AsNoTracking().ToListAsync();

                var data = result.GroupBy(x => new { x.LocationId }, (key, group) => new LeaveData //groupby to get the total number od leave days for each location
                {
                    LocationId = key.LocationId,
                    LocationName = group.Select(x => x.LocationName).FirstOrDefault(),
                    NoOfDays = group.Sum(x => x.NoOfDays)
                }).ToList();

                var szdays = data.Where(x => x.LocationId == (int)OfficeLocationEnum.API_Shenzhen)?.Sum(x => x.NoOfDays);
                data.ForEach(x => x.NoOfDays = (x.LocationId == (int)OfficeLocationEnum.API_Guangzhou ? x.NoOfDays + szdays.GetValueOrDefault() : x.NoOfDays));
                return data;
        }

        //get the holiday List based on office and country
        public async Task<List<HrHoliday>> GetHolidaysByRange(DateTime startdate, DateTime enddate, List<int> officeIdList, List<int> countryIdList)
        {
            var data = _context.HrHolidays
                .Where(x => x.StartDate != null && (x.EndDate != null) && !(x.StartDate.Value > enddate || x.EndDate.Value < startdate));

            if (officeIdList != null && officeIdList.Any())
                data = data.Where(x => officeIdList.Contains(x.LocationId.GetValueOrDefault()) || x.LocationId == null);

            if(countryIdList != null && countryIdList.Any())
                data = data.Where(x => countryIdList.Contains(x.CountryId));

            return await data.AsNoTracking().OrderBy(x => x.StartDate).ToListAsync();
        }

        //Fetch the Quotation Man Day for the bookings
        public async Task<List<QuotationManday>> GetActualInspManDay(List<int> bookingIds)
        {
            return await _context.SchScheduleQcs//.Include(x => x.Quotation)
                .Where(x => bookingIds.Contains(x.BookingId) && x.Active && x.Qc.EmployeeTypeId == (int)StaffType.Permanent) 
                .Select(x => new QuotationManday
                {
                    BookingId = x.BookingId,
                    ManDay = x.ActualManDay,
                    LocationId = x.Booking.OfficeId.GetValueOrDefault(),
                    LocationName = x.Booking.Office.LocationName,
                    StaffId = x.Qcid
                }).AsNoTracking().ToListAsync();
        }

        //Get booking manday by month
        public async Task<List<QuotationManday>> GetMonthlyInspManDays(List<int> bookingIds, UtilizationDashboardRequest request)
        {
            return await _context.SchScheduleQcs
                .Where(x => bookingIds.Contains(x.BookingId) && x.Active  && x.ServiceDate <= request.ServiceDateTo.ToDateTime() && x.ServiceDate >= request.ServiceDateFrom.ToDateTime()) 
                .Select(x => new QuotationManday
                {
                    ServiceDateFrom = x.ServiceDate,
                    ServiceDateTo = x.ServiceDate,
                    StaffId = x.Qcid,
                    LocationName = x.Booking.Office.LocationName,
                    LocationId = x.Booking.OfficeId.GetValueOrDefault(),
                    StaffType=x.Qc.EmployeeTypeId,
                    factoryId=x.Booking.FactoryId,
                    BookingId=x.BookingId
                }).AsNoTracking().ToListAsync();
        }
    }
}
