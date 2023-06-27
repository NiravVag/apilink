using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.Manday;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MandayRepository : Repository, IMandayRepository
    {
        public MandayRepository(API_DBContext context) : base(context)
        {
        }

        //Fetch all the audits
        public IQueryable<AuditResponseManday> GetAllAudits()
        {
            return _context.AudTransactions.
                Select(x => new AuditResponseManday
                {
                    AuditId = x.Id,
                    OfficeId = x.OfficeId,
                    StatusId = x.StatusId,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    FactoryId = x.FactoryId,
                    CustomerId = x.CustomerId
                });
        }

        //Get total audit man days
        public async Task<List<InspectionMandayDashboard>> GetAuditManDays(IEnumerable<int> auditIds)
        {
            return await _context.QuQuotations.Where(x => x.IdStatus != (int)QuotationStatus.Canceled).SelectMany(x => x.QuQuotationAudits).Where(x => x.NoOfManDay.HasValue && auditIds.Contains(x.IdBooking))
                    .Select(x => new InspectionMandayDashboard()
                    {
                        ServiceDateTo = x.IdBookingNavigation.ServiceDateTo,
                        MandayCount = x.NoOfManDay.Value,
                        InspectionId = x.IdBooking
                    }).AsNoTracking().ToListAsync();
        }

        //Get all the Services
        public async Task<List<CommonDataSource>> GetServices()
        {
            return await _context.RefServices.Where(x => x.Active)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().ToListAsync();
        }

        //Get all the Office Locations
        public async Task<List<CommonDataSource>> GetOfficeLocations()
        {
            return await _context.RefLocations.Where(x => x.Active)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.LocationName
                }).AsNoTracking().ToListAsync();
        }
        //Get audit manday by month
        public async Task<List<MandayYearChartItem>> GetMonthlyAuditManDays(IEnumerable<int> auditIds)
        {
            var data = await _context.QuQuotationAudits.Where(x => auditIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled).
                Select(k => new { k.IdBookingNavigation.ServiceDateFrom.Year, k.IdBookingNavigation.ServiceDateFrom.Month, k.NoOfManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthManDay = group.Sum(k => k.NoOfManDay).GetValueOrDefault(),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();

            return data;
        }

        //Get booking manday by month
        public async Task<List<MandayYearChartItem>> GetMonthlyInspManDays(IEnumerable<int> bookingIds)
        {
            var data = await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated).
                Select(k => new { k.IdBookingNavigation.ServiceDateTo.Year, k.IdBookingNavigation.ServiceDateTo.Month, k.NoOfManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthManDay = group.Sum(k => k.NoOfManDay).GetValueOrDefault(),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();

            return data;
        }

        //Get booking manday by month
        public async Task<List<MandayYearChartItem>> GetMonthlyInspActualManDays(IEnumerable<int> bookingIds)
        {
            var data = await _context.SchScheduleQcs.Where(x => bookingIds.Contains(x.BookingId) && x.Active).
                Select(k => new { k.Booking.ServiceDateTo.Year, k.Booking.ServiceDateTo.Month, k.ActualManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthActualManDay = Math.Round(group.Sum(k => k.ActualManDay), 1),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();

            return data;
        }


        //Get booking manday by customer
        public IQueryable<MandayCustomerChart> GetCustomerInspManDays(IQueryable<int> bookingIds)
        {
            return _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                .Select(y => new MandayCustomerChart
                {
                    CustomerName = y.IdBookingNavigation.Customer.CustomerName,
                    ServiceDateTo = y.IdBookingNavigation.ServiceDateTo,
                    MandayCount = y.NoOfManDay.GetValueOrDefault(),
                    InspectionId = y.IdBooking
                }).AsNoTracking();

        }

        // get actual manday by customer 
        public IQueryable<MandayCustomerChart> GetCustomerActualManDays(IQueryable<int> bookingIds)
        {
            return _context.SchScheduleQcs.Where(x => bookingIds.Contains(x.BookingId) && x.Active)
            //&& x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                .Select(y => new MandayCustomerChart
                {
                    CustomerName = y.Booking.Customer.CustomerName, // nathi mltu
                    ServiceDateTo = y.Booking.ServiceDateTo,
                    MandayCount = y.ActualManDay,
                    InspectionId = y.BookingId
                }).AsNoTracking();

        }

        //get report by customer
        public async Task<List<MandayCustomerChartData>> GetCustomerInspReportsData(IEnumerable<int> bookingIds)
        {
            var data = await _context.FbReportDetails.Where(x => bookingIds.Contains(x.InspectionId.Value) && x.Active.Value
                && (x.InspProductTransactions.Any(x => x.Active.Value) || x.InspContainerTransactions.Any(x => x.Active.Value)))
                .Select(y => new { y.Inspection.Customer.CustomerName, y.Id, y.InspectedQty, y.OrderQty, y.PresentedQty, y.Inspection.ServiceDateTo })
                .GroupBy(x => new { x.CustomerName, x.ServiceDateTo }, (key, group) => new MandayCustomerChartData
                {
                    CustomerName = key.CustomerName,
                    ReportCount = group.Select(x => x.Id).Count(),
                    InspectedQty = group.Sum(x => x.InspectedQty),
                    OrderQty = group.Sum(x => x.OrderQty),
                    PresentedQty = group.Sum(x => x.PresentedQty),
                    ServiceDateTo = key.ServiceDateTo
                }).AsNoTracking().ToListAsync();

            return data;
        }

        //Get audit manday by customer
        public IQueryable<MandayCustomerChart> GetCustomerAuditManDays(IEnumerable<int> auditIds)
        {
            return _context.QuQuotationAudits.Where(x => auditIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                .Select(y => new MandayCustomerChart
                {
                    CustomerName = y.IdBookingNavigation.Customer.CustomerName,
                    MandayCount = y.NoOfManDay.GetValueOrDefault(),
                    ServiceDateTo = y.IdBookingNavigation.ServiceDateTo,
                    InspectionId = y.IdBooking
                }).AsNoTracking();

        }

        //Get booking manday by country/ province
        public async Task<List<MandayCountryChart>> GetCountryInspManDays(IEnumerable<int> bookingIds, int countryId)
        {
            if (countryId > 0)
            {
                return await _context.QuQuotationInsps
                    .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                    (quot, sup) => new { QuQuotationInsp = quot, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.QuQuotationInsp.IdBooking) && x.QuQuotationInsp.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated
                    && x.SuAddress.CountryId == countryId)
                    .Select(y => new MandayCountryChart
                    {
                        Name = y.SuAddress.Region.ProvinceName,
                        ServiceDateTo = y.QuQuotationInsp.IdBookingNavigation.ServiceDateTo,
                        MandayCount = y.QuQuotationInsp.NoOfManDay.GetValueOrDefault()
                    }).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.QuQuotationInsps
                    .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                    (quot, sup) => new { QuQuotationInsp = quot, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.QuQuotationInsp.IdBooking) && x.QuQuotationInsp.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                    .Select(y => new MandayCountryChart
                    {
                        Name = y.SuAddress.Country.CountryName,
                        ServiceDateTo = y.QuQuotationInsp.IdBookingNavigation.ServiceDateTo,
                        MandayCount = y.QuQuotationInsp.NoOfManDay.GetValueOrDefault()
                    }).AsNoTracking().ToListAsync();
            }
        }

        //actual manday by country/ province
        public async Task<List<MandayCountryChart>> GetCountryActualInspManDays(IEnumerable<int> bookingIds, int countryId)
        {
            var query = _context.SchScheduleQcs
                    .Join(_context.SuAddresses, schQcs => schQcs.Booking.FactoryId, sup => sup.SupplierId,
                    (schQcs, sup) => new { SchScheduleQc = schQcs, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.SchScheduleQc.BookingId) && x.SchScheduleQc.Active);

            if (countryId > 0)
                query.Where(x => x.SuAddress.CountryId == countryId);

            return await query
                .Select(y => new MandayCountryChart
                {
                    Name = countryId > 0 ? y.SuAddress.Region.ProvinceName : y.SuAddress.Country.CountryName,
                    ServiceDateTo = y.SchScheduleQc.Booking.ServiceDateTo,
                    MandayCount = y.SchScheduleQc.ActualManDay
                }).AsNoTracking().ToListAsync();
        }

        //Get audit manday by country/ province
        public async Task<List<MandayCountryChart>> GetCountryAuditManDays(IEnumerable<int> bookingIds, int countryId)
        {
            if (countryId > 0)
            {
                return await _context.QuQuotationAudits
                    .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                    (quot, sup) => new { QuQuotationaudit = quot, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.QuQuotationaudit.IdBooking) && x.QuQuotationaudit.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled
                    && x.SuAddress.CountryId == countryId)
                    .Select(y => new MandayCountryChart
                    {
                        Name = y.SuAddress.Region.ProvinceName,
                        MandayCount = y.QuQuotationaudit.NoOfManDay.GetValueOrDefault(),
                        ServiceDateTo = y.QuQuotationaudit.IdBookingNavigation.ServiceDateTo,
                    }).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.QuQuotationAudits
                    .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                    (quot, sup) => new { QuQuotationaudit = quot, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.QuQuotationaudit.IdBooking) && x.QuQuotationaudit.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                    .Select(y => new MandayCountryChart
                    {
                        Name = y.SuAddress.Country.CountryName,
                        MandayCount = y.QuQuotationaudit.NoOfManDay.GetValueOrDefault(),
                        ServiceDateTo = y.QuQuotationaudit.IdBookingNavigation.ServiceDateTo,
                    }).AsNoTracking().ToListAsync();
            }
        }

        //Get booking manday by employeetype and month
        public async Task<List<EmployeeTypes>> GetMandayInspByEmployeeType(IEnumerable<int> bookingIds, DateTime serviceDateFrom, DateTime serviceDateTo)
        {
            var data = await _context.SchScheduleQcs.Where(x => bookingIds.Contains(x.BookingId) && x.Active && x.Qctype == (int)QCType.QC
                && !((x.ServiceDate > serviceDateTo) || (x.ServiceDate < serviceDateFrom)))
                .Join(_context.QuQuotationInsps, schedule => schedule.BookingId, quot => quot.IdBooking,
                    (schedule, quot) => new { SchScheduleQcs = schedule, QuQuotationInsps = quot })
                    .Where(x => x.QuQuotationInsps.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                        .Select(x => new EmployeeTypes
                        {
                            EmployeeType = x.SchScheduleQcs.Qc.EmployeeType.EmployeeTypeName,
                            MandayCount = x.QuQuotationInsps.NoOfManDay.GetValueOrDefault(),
                            Month = x.SchScheduleQcs.ServiceDate.Month,
                            ServiceDateTo = x.QuQuotationInsps.IdBookingNavigation.ServiceDateTo,
                        })
                        .AsNoTracking().ToListAsync();

            return data.GroupBy(x => new { x.Month, x.EmployeeType }, (key, group) => new EmployeeTypes
            {
                EmployeeType = key.EmployeeType,
                MandayCount = group.Where(x => x.EmployeeType == key.EmployeeType && x.Month == key.Month).Count(),
                Month = key.Month,
                MonthName = MonthData.GetValueOrDefault(key.Month),
                QuotationManday = group.Where(x => x.Month == key.Month).Sum(x => x.MandayCount)
            }).ToList();
        }

        //Get booking actual manday by employeetype and month
        public async Task<List<EmployeeTypes>> GetActualMandayInspByEmployeeType(IEnumerable<int> bookingIds, DateTime serviceDateFrom, DateTime serviceDateTo)
        {
            var data = await _context.SchScheduleQcs.Where(x => bookingIds.Contains(x.BookingId) && x.Active && x.Qctype == (int)QCType.QC && x.Active
                && !((x.ServiceDate > serviceDateTo) || (x.ServiceDate < serviceDateFrom)))
                        .Select(x => new EmployeeTypes
                        {
                            EmployeeType = x.Qc.EmployeeType.EmployeeTypeName,
                            MandayCount = x.ActualManDay,
                            Month = x.ServiceDate.Month,
                            ServiceDateTo = x.Booking.ServiceDateTo,
                        })
                        .AsNoTracking().ToListAsync();

            return data.GroupBy(x => new { x.Month, x.EmployeeType }, (key, group) => new EmployeeTypes
            {
                EmployeeType = key.EmployeeType,
                MandayCount = group.Where(x => x.EmployeeType == key.EmployeeType && x.Month == key.Month).Count(),
                Month = key.Month,
                MonthName = MonthData.GetValueOrDefault(key.Month),
                QuotationManday = group.Where(x => x.Month == key.Month).Sum(x => x.MandayCount)
            }).ToList();
        }

        //Get audit manday by employeetype and month
        public async Task<List<EmployeeTypes>> GetMandayAuditByEmployeeType(IEnumerable<int> auditIds)
        {
            var data = await _context.AudTranAuditors.Where(x => auditIds.Contains(x.AuditId) && x.Active)
                .Join(_context.QuQuotationAudits, audit => audit.AuditId, quot => quot.IdBooking,
                    (audit, quot) => new { AudTranAuditors = audit, QuQuotationAudits = quot })
                    .Where(x => x.QuQuotationAudits.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                        .Select(x => new EmployeeTypes
                        {
                            EmployeeType = x.AudTranAuditors.Staff.EmployeeType.EmployeeTypeName,
                            MandayCount = x.QuQuotationAudits.NoOfManDay.GetValueOrDefault(),
                            Month = x.AudTranAuditors.Audit.ServiceDateFrom.Month,
                            ServiceDateTo = x.QuQuotationAudits.IdBookingNavigation.ServiceDateTo,
                        }).AsNoTracking().ToListAsync();

            return data.GroupBy(x => new { x.Month, x.EmployeeType }, (key, group) => new EmployeeTypes
            {
                EmployeeType = key.EmployeeType,
                MandayCount = group.Where(x => x.EmployeeType == key.EmployeeType && x.Month == key.Month).Count(),
                Month = key.Month,
                MonthName = MonthData.GetValueOrDefault(key.Month)
            }).ToList();
        }

        //Get booking manday by country and province
        public async Task<List<MandayCountryChartExport>> GetCountryInspManDaysExport(IEnumerable<int> bookingIds, bool isCountrySelected)
        {
            if (isCountrySelected)
            {
                return await _context.QuQuotationInsps
                    .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                    (quot, sup) => new { QuQuotationInsp = quot, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.QuQuotationInsp.IdBooking) && x.QuQuotationInsp.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                    .Select(y => new MandayCountryChartExport
                    {
                        CountryName = y.SuAddress.Country.CountryName,
                        ProvinceName = y.SuAddress.Region.ProvinceName,
                        MandayCount = y.QuQuotationInsp.NoOfManDay.GetValueOrDefault(),
                        ServiceDateTo = y.QuQuotationInsp.IdBookingNavigation.ServiceDateTo
                    }).AsNoTracking().ToListAsync();
            }

            else
            {
                return await _context.QuQuotationInsps
                   .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                   (quot, sup) => new { QuQuotationInsp = quot, SuAddress = sup })
                   .Where(x => bookingIds.Contains(x.QuQuotationInsp.IdBooking) && x.QuQuotationInsp.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated)
                   .Select(y => new MandayCountryChartExport
                   {
                       CountryName = y.SuAddress.Country.CountryName,
                       ProvinceName = "",
                       MandayCount = y.QuQuotationInsp.NoOfManDay.GetValueOrDefault(),
                       ServiceDateTo = y.QuQuotationInsp.IdBookingNavigation.ServiceDateTo
                   }).AsNoTracking().ToListAsync();
            }
        }

        //Get audit manday by country and province
        public async Task<List<MandayCountryChartExport>> GetCountryAuditManDaysExport(IEnumerable<int> bookingIds, bool isCountrySelected)
        {
            if (isCountrySelected)
            {
                return await _context.QuQuotationAudits
                    .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                    (quot, sup) => new { QuQuotationaudit = quot, SuAddress = sup })
                    .Where(x => bookingIds.Contains(x.QuQuotationaudit.IdBooking) && x.QuQuotationaudit.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                    .Select(y => new MandayCountryChartExport
                    {
                        CountryName = y.SuAddress.Country.CountryName,
                        ProvinceName = y.SuAddress.Region.ProvinceName,
                        MandayCount = y.QuQuotationaudit.NoOfManDay.GetValueOrDefault(),
                        ServiceDateTo = y.QuQuotationaudit.IdBookingNavigation.ServiceDateTo
                    }).AsNoTracking().ToListAsync();
            }

            else
            {
                return await _context.QuQuotationAudits
                   .Join(_context.SuAddresses, quot => quot.IdBookingNavigation.FactoryId, sup => sup.SupplierId,
                   (quot, sup) => new { QuQuotationaudit = quot, SuAddress = sup })
                   .Where(x => bookingIds.Contains(x.QuQuotationaudit.IdBooking) && x.QuQuotationaudit.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled)
                   .Select(y => new MandayCountryChartExport
                   {
                       CountryName = y.SuAddress.Country.CountryName,
                       ProvinceName = "",
                       MandayCount = y.QuQuotationaudit.NoOfManDay.GetValueOrDefault(),
                       ServiceDateTo = y.QuQuotationaudit.IdBookingNavigation.ServiceDateTo
                   }).AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// implement the actual manday 
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <param name="isCountrySelected"></param>
        /// <returns></returns>
        public async Task<List<MandayCountryChartExport>> GetCountryInspActualManDaysExport(IEnumerable<int> bookingIds, bool isCountrySelected)
        {
            return await _context.SchScheduleQcs
                             .Join(_context.SuAddresses, schQcs => schQcs.Booking.FactoryId, sup => sup.SupplierId,
                             (schQcs, sup) => new { SchScheduleQc = schQcs, SuAddress = sup })
                             .Where(x => bookingIds.Contains(x.SchScheduleQc.BookingId) && x.SchScheduleQc.Active)
                             .Select(y => new MandayCountryChartExport
                             {
                                 CountryName = y.SuAddress.Country.CountryName,
                                 ProvinceName = isCountrySelected ? y.SuAddress.Region.ProvinceName : "",
                                 MandayCount = y.SchScheduleQc.ActualManDay,
                                 ServiceDateTo = y.SchScheduleQc.Booking.ServiceDateTo
                             }).AsNoTracking().ToListAsync();
        }
    }
}
