using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Quotation;
using DTO.Schedule;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ScheduleRepository : Repository, IScheduleRepository
    {
        public ScheduleRepository(API_DBContext context) : base(context)
        {
        }

        public IQueryable<ScheduleBookingInfo> GetAllInspections()
        {
            return _context.InspTransactions
                        //.Where(x => x.StatusId != (int)BookingStatus.Cancel && x.StatusId != (int)BookingStatus.Verified && x.StatusId != (int)BookingStatus.Rescheduled && x.StatusId != (int)BookingStatus.Hold)
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
                        }).OrderBy(x => x.ServiceDateFrom).ThenBy(x => x.ServiceDateTo);
        }
        //public async Task<IEnumerable<HrStaffProfile>> GetQCStaffList(IEnumerable<int> location)
        //{
        //    return await _context.HrStaffProfiles
        //        .Include(x => x.Profile)
        //        .Include(x => x.Staff)
        //            .ThenInclude(x => x.HrLeaves)
        //        .Include(x => x.Staff)
        //            .ThenInclude(x => x.HrOfficeControls)
        //        .Where(x => x.Staff.Active != null && x.Staff.Active.Value && x.ProfileId == (int)HRProfile.Inspector &&
        //    location.ToList().Contains(x.Staff.LocationId.Value)).
        //        ToListAsync();
        //}
        public async Task<int> AssignedQCList(DateTime serviceDate, int QCType, int staffId)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && x.ServiceDate.Date == serviceDate
                 && x.Qctype == QCType && x.Qcid == staffId)
                .CountAsync();
        }
        public async Task<List<SchScheduleQc>> GetQCDetails(int bookingId, int QCType)
        {
            return await _context.SchScheduleQcs
                .Include(x => x.Qc.Location)
                .Where(x => x.Active && x.BookingId == bookingId && x.Qctype == QCType).ToListAsync();
        }

        public async Task<List<QcAutoExpense>> GetQCAutoTravelExpenseList(int inspectionId)
        {
            return await _context.EcAutQcTravelExpenses
                .Where(x => x.Active.Value && x.InspectionId == inspectionId)
                .Select(x => new QcAutoExpense()
                {
                    InspectionId = x.InspectionId,
                    StartPortId = x.StartPort,
                    StartPortName = x.StartPortNavigation.StartPortName,
                    TripTypeId = x.TripType,
                    TripTypeName = x.TripTypeNavigation.Name,
                    FactoryTownId = x.FactoryTown,
                    FactoryTownName = x.FactoryTownNavigation.TownName,
                    QcId = x.QcId,
                    QcName = x.Qc.PersonName,
                    Comments = x.Comments,
                    ServiceDate = x.ServiceDate,
                    ExpenseStatus = x.EcExpensesClaimDetais.FirstOrDefault(x => x.Active.Value).Expense.StatusId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<EcAutQcTravelExpense>> GetQCAutoTravelExpenses(List<int?> QcIdList, DateTime startDate, DateTime endDate)
        {
            return await _context.EcAutQcTravelExpenses
                .Where(x => x.Active.Value &&
                x.ServiceDate >= startDate && x.ServiceDate <= endDate &&
                QcIdList.Contains(x.QcId)).ToListAsync();
        }

        public async Task<List<EcAutQcFoodExpense>> GetQCAutoFoodExpenses(List<int?> QcIdList, DateTime startDate, DateTime endDate)
        {
            return await _context.EcAutQcFoodExpenses
                .Where(x => x.Active.Value &&
                x.ServiceDate >= startDate && x.ServiceDate <= endDate &&
                QcIdList.Contains(x.QcId)).ToListAsync();
        }

        //get the qc auto travel expanse by inspection id
        public async Task<List<EcAutQcTravelExpense>> GetQCAutoTravelExpensesByInspectionId(int? inspectionId)
        {
            return await _context.EcAutQcTravelExpenses
                .Where(x => x.Active == true && x.InspectionId == inspectionId).ToListAsync();
        }

        public async Task<List<EcExpencesClaim>> GetAutoQcExpenseListByQcList(List<int?> qcList, int? inspectionId)
        {
            return await _context.EcExpencesClaims
                .Include(x => x.EcExpensesClaimDetais)
                .Where(x => x.Active.Value && x.EcExpensesClaimDetais.Any(y => y.InspectionId == inspectionId && x.Active.Value)).ToListAsync();
        }

        //get the qc auto food expanse by inspection id
        public async Task<List<EcAutQcFoodExpense>> GetQCAutoFoodExpensesByInspectionId(int? inspectionId)
        {
            return await _context.EcAutQcFoodExpenses
                .Where(x => x.Active == true &&
                x.InspectionId == inspectionId).ToListAsync();
        }

        public async Task<List<EcAutTravelTariff>> GetTravelTariffData(DateTime startDate, DateTime endDate)
        {
            return await _context.EcAutTravelTariffs.
                  Where(x => x.Active.Value && !((x.StartDate > startDate) || (x.EndDate < endDate))).ToListAsync();
        }

        public async Task<List<EcFoodAllowance>> GetFoodAllowanceData(DateTime startDate, DateTime endDate)
        {
            return await _context.EcFoodAllowances.
                  Where(x => x.Active.Value && !((x.StartDate > startDate) || (x.EndDate < endDate))).ToListAsync();
        }

        public async Task<List<SchScheduleC>> GetCSDetails(int bookingId)
        {
            return await _context.SchScheduleCs
                .Include(x => x.Cs.Location)
                .Where(x => x.Active && x.BookingId == bookingId).ToListAsync();
        }
        public async Task<InspTransaction> GetInspectionByID(int inspectionID)
        {
            return await _context.InspTransactions.Where(x => x.Id == inspectionID)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetInspectionLocation(int inspectionId)
        {
            return await _context.InspTransactions.
                Where(x => x.Id == inspectionId)
                .Select(x => x.InspectionLocation)
                .FirstOrDefaultAsync();
        }

        public async Task<AllocationBookingItem> GetBookingDetails(int bookingId)
        {
            return await _context.InspTransactions
                .Where(x => x.Id == bookingId && ScheduleStatusList.Contains(x.StatusId))
                .Select(x => new AllocationBookingItem
                {
                    BookingNo = x.Id,
                    ServiceDateFrom = x.ServiceDateFrom,
                    ServiceDateTo = x.ServiceDateTo,
                    CustomerName = x.Customer.CustomerName,
                    CustomerId = x.CustomerId,
                    SupplierName = x.Supplier.SupplierName,
                    FactoryName = x.Factory.SupplierName,
                    BookingStatus = x.StatusId,
                    Comment = x.ScheduleComments,
                    BookingComments = x.InternalComments,
                    RegionalSupplierName = x.Supplier.LocalName,
                    RegionalFactoryName = x.Factory.LocalName,
                    PreviousBookingNo = x.PreviousBookingNo
                }).FirstOrDefaultAsync();
        }
        public async Task<List<int>> GetQCBooking(List<int> qcIdList)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && qcIdList.Contains(x.Qcid)).Select(x => x.BookingId).ToListAsync();
        }

        public async Task<List<int>> GetQCBookingbyDate(int qcId, DateTime fromdate, DateTime todate)
        {
            return await _context.SchScheduleQcs.Where(x => x.Booking.ServiceDateFrom != null && x.Booking.ServiceDateTo != null && x.Active && qcId == x.Qcid && !((x.Booking.ServiceDateFrom > todate) || (x.Booking.ServiceDateTo < fromdate))).Select(x => x.BookingId).ToListAsync();
        }
        /// <summary>
        /// Get QC schedule data with the booking information
        /// </summary>
        /// <param name="qcId"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        public async Task<List<QCBookings>> GetQCBookingbyServiceDate(int qcId, DateTime fromdate, DateTime todate)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && x.IsVisibleToQc.Value && qcId == x.Qcid && !((x.Booking.ServiceDateFrom > todate)
                        || (x.Booking.ServiceDateTo < fromdate))).
                        Select(x => new QCBookings()
                        {

                            QCId = x.Qcid,
                            BookingId = x.BookingId,
                            ScheduledDate = x.ServiceDate,
                            FactoryId = x.Booking.Factory.Id,
                            FactoryName = x.Booking.Factory.SupplierName,
                            FactoryRegionalName = x.Booking.Factory.LocalName,
                            ServiceDateFrom = x.Booking.ServiceDateFrom,
                            ServiceDateTo = x.Booking.ServiceDateTo,
                            BookingStatus = x.Booking.StatusId
                        }).ToListAsync();
        }

        public async Task<InspTransaction> GetBookingProduct(int bookingId)
        {
            return await _context.InspTransactions
                .Include(x => x.InspProductTransactions)
                .ThenInclude(x => x.Product)
                .Where(x => x.Id == bookingId).FirstOrDefaultAsync();
        }
        public async Task<int> GetQcManDay(int qcId, DateTime serviceDate)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && x.Qcid == qcId && x.ServiceDate.Date == serviceDate.Date
                && x.Qctype == (int)QCType.QC).Select(x => x.Id)
                .CountAsync();
        }
        public async Task<IEnumerable<SchScheduleQc>> GetQcListManDay(List<int> qcIdlst, List<DateTime> serviceDatelst)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && qcIdlst.Contains(x.Qcid) && serviceDatelst.Contains(x.ServiceDate.Date))
                .ToListAsync();
        }
        //public async Task<double> GetQCManday(int qcId, DateTime serviceDate)
        //{
        //    var manDay = await _context.SchScheduleQcs.Where(x => x.Active && x.Qcid == qcId && x.ServiceDate.Date
        //    == serviceDate).Select(x => x.ActualManDay).FirstOrDefaultAsync();
        //    return manDay;
        //}

        public async Task<int> SaveQC(SchScheduleQc schScheduleQc)
        {
            _context.Entry(schScheduleQc).State = EntityState.Added;
            return await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Get Qc Count
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<int> GetQCCountbyLocation(IEnumerable<int> location)
        {
            return await _context.HrStaffProfiles
                         .Where(x => x.Staff.Active.Value && x.ProfileId == (int)HRProfile.Inspector &&
                          location.ToList().Contains(x.Staff.LocationId.Value)).
                          CountAsync();
        }
        public async Task<double> QcCountOnDate(DateTime serviceDate, List<int> locationList)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && x.ServiceDate.Date == serviceDate.Date
                && x.Qctype == (int)QCType.QC && locationList.Contains(x.Qc.LocationId.GetValueOrDefault())).SumAsync(x => x.ActualManDay);
        }

        /// <summary>
        /// Get actual manday count on particular range of service date 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="locationList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ActualManDayDateCount>> GetActualQcCountOnDate(DateTime fromDate, DateTime toDate, List<int> locationList)
        {
            return await _context.SchScheduleQcs
                        .Where(l => l.Active && l.Qctype == (int)QCType.QC && locationList.Contains(l.Qc.LocationId.GetValueOrDefault())
                         && l.ServiceDate.Date >= fromDate.Date && l.ServiceDate.Date <= toDate.Date)
                        .GroupBy(c => c.ServiceDate.Date)
                        .Select(g => new ActualManDayDateCount()
                        {
                            ServiceDate = g.Key,
                            ActualManDayCount = g.Select(x => x.ActualManDay).Sum()
                        }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get Actual Man day by service Dates
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QcActualManDayRepo>> GetQCActualManDayByServiceDates(DateTime fromDate, DateTime toDate, int bookingId)
        {
            return await _context.SchScheduleQcs.
                              Where(x => x.Active && x.BookingId == bookingId && x.ServiceDate.Date >= fromDate.Date && x.ServiceDate.Date <= toDate.Date)
                             .Select(x => new QcActualManDayRepo()
                             {
                                 QcId = x.Qcid,
                                 ActualManDay = x.ActualManDay,
                                 ServiceDate = x.ServiceDate
                             }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<QuotationManday>> GetQuotationManDay(int bookingId)
        {
            return await _context.QuQuotationInspMandays
                .Where(x => x.BookingId == bookingId &&
                x.Quotation.IdStatus != (int)QuotationStatus.Canceled && x.Active.HasValue && x.Active.Value)
                .Select(x => new QuotationManday
                {
                    BookingId = bookingId,
                    ManDay = x.NoOfManday.Value,
                    Remarks = x.Remarks,
                    ServiceDate = x.ServiceDate.ToString(StandardDateFormat)
                })
                .ToListAsync();
        }
        public IQueryable<HrStaff> GetQCList()
        {
            return _context.HrStaffs
                .Include(x => x.HrLeaves)
                .Include(x => x.SchScheduleQcs)
                .Where(x => x.Active.HasValue && x.Active.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Inspector)
                        && x.ItUserMasters.Any(y => y.Active));

        }

        //get SchScheduleQc active record using booking id
        public async Task<List<SchScheduleQc>> GetQCDetails(int bookingId)
        {
            return await _context.SchScheduleQcs
                            .Where(x => x.Active && x.BookingId == bookingId).ToListAsync();
        }


        public async Task<List<string>> GetQCNameList(int bookingId)
        {
            return await _context.SchScheduleQcs
                            .Where(x => x.Active && x.BookingId == bookingId)
                            .Select(x => x.Qc.PersonName)
                            .ToListAsync();
        }

        //public async Task<List<SchScheduleQc>> GetQCStaffList(DateTime fromdate, DateTime todate, int bookingId)
        //{
        //    var distinctQC = _context.SchScheduleQcs.Where(x => x.BookingId == bookingId && x.Active).Select(x => x.Qcid);

        //    var data = await _context.SchScheduleQcs.
        //        Include(x => x.Qc.Location)
        //        .Where(x => x.ServiceDate >= fromdate && x.ServiceDate <= todate && distinctQC.Contains(x.Qcid) && x.Active).ToListAsync();


        //    return data;
        //}

        //public async Task<List<SchScheduleC>> GetCSStaffList(DateTime fromdate, DateTime todate, int bookingId)
        //{
        //    var distinctCS = _context.SchScheduleCs.Where(x => x.BookingId == bookingId && x.Active).Select(x => x.Csid);

        //    var data = await _context.SchScheduleCs.
        //        Include(x => x.Cs.Location)
        //        .Where(x => x.ServiceDate >= fromdate && x.ServiceDate <= todate && distinctCS.Contains(x.Csid) && x.Active).ToListAsync();


        //    return data;
        //}

        /// <summary>
        /// Get CsList by service dates and booking id
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<StaffScheduleRepo>> GetCSListbyServiceDate(DateTime fromdate, DateTime todate, int bookingId)
        {

            var distinctCS = _context.SchScheduleCs.Where(x => x.BookingId == bookingId && x.Active).Select(x => x.Csid);

            return await _context.SchScheduleCs
                 .Where(x => x.ServiceDate >= fromdate && x.ServiceDate <= todate && x.BookingId == bookingId
                 && distinctCS.Contains(x.Csid) && x.Active).Select(x => new StaffScheduleRepo()
                 {
                     StaffID = x.Csid,
                     StaffName = x.Cs.PersonName,
                     EmailAddress = x.Cs.EmaiLaddress,
                     EmergencyCall = x.Cs.EmergencyCall,
                     Location = x.Cs.Location.LocationName,
                     EmployeeType = x.Cs.EmployeeTypeId,
                     BookingId = x.BookingId,
                     ServiceDate = x.ServiceDate
                 }).AsNoTracking()
                 .ToListAsync();
        }

        /// <summary>
        /// Get Qc List by service Date
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<StaffScheduleQcRepo>> GetQcListbyServiceDate(DateTime fromdate, DateTime todate, int bookingId)
        {

            var distinctQC = _context.SchScheduleQcs.Where(x => x.BookingId == bookingId && x.Active).Select(x => x.Qcid);

            return await _context.SchScheduleQcs
                 .Where(x => x.ServiceDate >= fromdate && x.ServiceDate <= todate
                 && distinctQC.Contains(x.Qcid) && x.Active).Select(x => new StaffScheduleQcRepo()
                 {
                     StaffID = x.Qcid,
                     StaffName = x.Qc.PersonName,
                     EmailAddress = x.Qc.EmaiLaddress,
                     EmergencyCall = x.Qc.EmergencyCall,
                     Location = x.Qc.Location.LocationName,
                     EmployeeType = x.Qc.EmployeeTypeId,
                     BookingId = x.BookingId,
                     ServiceDate = x.ServiceDate,
                     QcType = x.Qctype,
                     isLeader = x.QcLeader.GetValueOrDefault(),
                     IsQcVisibility = x.IsVisibleToQc.GetValueOrDefault(),
                 }).AsNoTracking()
                 .ToListAsync();
        }


        /// <summary>
        /// Qc Leaves
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StaffLeavesDate>> GetQCStaffLeaves(IEnumerable<int> location)
        {
            return await _context.HrLeaves
              .Where(x => x.Staff.Active.Value && x.Staff.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Inspector) &&
              location.ToList().Contains(x.Staff.LocationId.Value)).
              Select(x => new StaffLeavesDate()
              {
                  LeaveStartDate = x.DateBegin,
                  LeaveEndDate = x.DateEnd
              }).AsNoTracking().
              ToListAsync();
        }

        public async Task<List<PoTransactionDetails>> GetBookingPo(List<int> bookingIds)
        {
            var data = _context.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId));
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value && bookingIds.Contains(x.InspectionId))
               .Select(x => new PoTransactionDetails
               {
                   PoTranId = x.Id,
                   ProductId = x.ProductId,
                   PoNo = data.Where(y => y.InspectionId == x.InspectionId).Select(y => y.Po.Pono).FirstOrDefault(),
                   BookingId = x.InspectionId,
                   CombineProductId = x.CombineProductId,
                   CombineAqlQty = x.CombineAqlQuantity,
                   AqlQty = x.AqlQuantity,
                   ReportId = x.FbReportId
               }).AsNoTracking().ToListAsync();
        }

        //Fetch the Quotation Man Day for the bookings
        public async Task<IEnumerable<QuotationManday>> GetQuotationManDayList(List<int> bookingIds)
        {
            return await _context.QuQuotationInsps//.Include(x => x.Quotation)
                .Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) //&& x.Active.HasValue && x.Active.Value)
                .Select(x => new QuotationManday
                {
                    BookingId = x.IdBooking,
                    ManDay = x.NoOfManDay
                })
                .ToListAsync();
        }

        //Fetch the allocated QCs for the booking
        public async Task<List<ScheduleStaffItem>> GetQCBookingDetails(List<int> bookingIds)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && bookingIds.Contains(x.BookingId))
                .Select(x => new ScheduleStaffItem
                {
                    BookingId = x.BookingId,
                    Id = x.Qc.Id,
                    QCType = x.Qctype,
                    ServiceDate = x.ServiceDate,
                    Name = x.Qc.PersonName,
                    ActualManDay = x.ActualManDay,
                    CompanyEmail = x.Qc.CompanyEmail,
                    IsQcVisibility = x.IsVisibleToQc.GetValueOrDefault()
                }).ToListAsync();
        }

        public async Task<List<BookingCsItem>> GetBookingCSItemList(List<int> bookingIds)
        {
            return await _context.InspTranCs
                .Where(x => x.Active.Value && x.InspectionId != null && x.CsId != null && bookingIds.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new BookingCsItem
                {
                    BookingId = x.InspectionId.GetValueOrDefault(),
                    CsId = x.CsId.GetValueOrDefault()
                }).AsNoTracking().ToListAsync();
        }




        //Fetch the allocated QCs Visible false for the booking
        public async Task<List<SchScheduleQc>> GetQCBookingVisibleDetails(List<int> bookingIds)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && x.IsVisibleToQc.HasValue && x.IsVisibleToQc.Value == false
                && bookingIds.Contains(x.BookingId))
                 .ToListAsync();
        }

        public async Task<List<ScheduleStaffItem>> GetQCBookingDetailsByBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && bookingIds.Contains(x.BookingId))
                .Select(x => new ScheduleStaffItem
                {
                    BookingId = x.BookingId,
                    Id = x.Qc.Id,
                    QCType = x.Qctype,
                    ServiceDate = x.ServiceDate,
                    Name = x.Qc.PersonName,
                    ActualManDay = x.ActualManDay,
                    CompanyEmail = x.Qc.CompanyEmail,
                    IsQcVisibility = x.IsVisibleToQc.GetValueOrDefault(),
                    EmployeeTypeName = x.Qc.EmployeeType.EmployeeTypeName,
                    CustomerId = x.Booking.CustomerId,
                    PayrollCurrency = x.Qc.PayrollCurrency.CurrencyName,
                    StartPortName = x.Qc.StartPortNavigation.StartPortName
                }).ToListAsync();
        }


        //Fetch the allocated QCs for the booking
        public async Task<List<ScheduleStaffItem>> GetQCBookingDetails(List<int> bookingIds, DateTime todayDate)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && x.IsVisibleToQc.Value && bookingIds.Contains(x.BookingId) && (x.ServiceDate >= todayDate) && x.Qc.Active.Value)
                .Select(x => new ScheduleStaffItem
                {
                    BookingId = x.BookingId,
                    Id = x.Qc.Id,
                    QCType = x.Qctype,
                    ServiceDate = x.ServiceDate,
                    Name = x.Qc.PersonName,
                    ActualManDay = x.ActualManDay,
                    CompanyEmail = x.Qc.CompanyEmail,
                    IsChinaCountry = x.Qc.NationalityCountryId == (int)CountryEnum.China
                }).ToListAsync();
        }

        //Fetch the allocated CS List for the booking
        public async Task<List<ScheduleStaffItem>> GetCSBookingDetails(List<int> bookingIds)
        {
            return await _context.SchScheduleCs
                .Where(x => x.Active && bookingIds.Contains(x.BookingId) && x.Cs.Active.Value)
                .Select(x => new ScheduleStaffItem
                {
                    BookingId = x.BookingId,
                    ServiceDate = x.ServiceDate,
                    Name = x.Cs.PersonName,
                    CompanyEmail = x.Cs.CompanyEmail,
                    Email = x.Cs.EmaiLaddress,
                    Id = x.Csid,
                }).ToListAsync();
        }


        //Fetch the allocated CS List for the booking
        public async Task<List<ScheduleStaffItem>> GetCSBookingDetails(List<int> bookingIds, DateTime scheduleDate)
        {
            return await _context.SchScheduleCs
                .Where(x => x.Active && bookingIds.Contains(x.BookingId) && x.ServiceDate == scheduleDate && x.Cs.Active.Value)
                .Select(x => new ScheduleStaffItem
                {
                    BookingId = x.BookingId,
                    ServiceDate = x.ServiceDate,
                    Name = x.Cs.PersonName,
                    CompanyEmail = x.Cs.CompanyEmail,
                    Email = x.Cs.EmaiLaddress,
                    Id = x.Csid,
                }).AsNoTracking().ToListAsync();
        }


        public async Task<List<ScheduleStaffItem>> GetCSBookingDetailsByBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.SchScheduleCs
                .Where(x => x.Active && bookingIds.Contains(x.BookingId))
                .Select(x => new ScheduleStaffItem
                {
                    BookingId = x.BookingId,
                    ServiceDate = x.ServiceDate,
                    Name = x.Cs.PersonName
                }).ToListAsync();
        }

        /// <summary>
        /// Get QC allocated bookings
        /// </summary>
        /// <param name="staffIDs"></param>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<int>> GetQCAllocatedBookings(IEnumerable<int> staffIDs, IEnumerable<int> bookingIds)
        {
            return await _context.SchScheduleQcs
                                    .Where(x => x.Active && staffIDs.Contains(x.Qcid) && bookingIds.Contains(x.BookingId))
                                    .Select(x => x.BookingId).ToListAsync();
        }

        public async Task<IEnumerable<QCStaffInfo>> GetQCListByLocationForForecast(IEnumerable<int> location, IEnumerable<DateTime> serviceDates)
        {
            var data = _context.HrStaffs
                .Include(x => x.HrLeaves)
                .Where(x => x.Active.HasValue && x.Active.Value && x.EmployeeTypeId == (int)StaffType.Permanent && x.IsForecastApplicable.HasValue
                    && x.IsForecastApplicable.Value && x.HrStaffProfiles.Any(y => y.ProfileId == (int)HRProfile.Inspector));

            if (location != null && location.Any())
                data = data.Where(x => location.Contains(x.LocationId.Value));


            var result = await data.Select(x => new QCStaffInfo
            {
                Id = x.Id,
                Name = x.PersonName,
                Location = x.Location.LocationName,
                LocationId = x.LocationId.GetValueOrDefault(),
                EmployeeType = x.EmployeeTypeId,
                ZoneId = x.CurrentCounty.ZoneId.GetValueOrDefault(),
                //StaffImage = x.HrStaffPhotos.FirstOrDefault(),
                LeaveQC = x.HrLeaves

            }).ToListAsync();

            return result;
        }

        public IQueryable<HrStaffProfile> GetQcDataSource()
        {
            return _context.HrStaffProfiles.Where(x => x.ProfileId == (int)HRProfile.Inspector && x.Staff.Active == true);

        }

        //Get man day date wise
        public async Task<List<ScheduleQuotationManDay>> GetQuotationManDay(List<int> bookingIdList, DateTime startDate, DateTime endDate)
        {
            return await _context.QuQuotationInspMandays
                .Where(x => bookingIdList.Contains(x.BookingId) &&
                x.Quotation.IdStatus != (int)QuotationStatus.Canceled && x.Active.HasValue && x.Active.Value &&
                x.ServiceDate >= startDate && x.ServiceDate <= endDate)
                .Select(x => new ScheduleQuotationManDay
                {
                    BookingId = x.BookingId,
                    ManDay = x.NoOfManday.Value,
                    ServiceDate = x.ServiceDate
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ScheduleQuotationManDay>> GetQuotationManDaybyBookingQuery(IQueryable<int> bookingIdList, DateTime startDate, DateTime endDate)
        {
            return await _context.QuQuotationInspMandays
                .Where(x => bookingIdList.Contains(x.BookingId) &&
                x.Quotation.IdStatus != (int)QuotationStatus.Canceled && x.Active.HasValue && x.Active.Value && x.NoOfManday.HasValue &&
                x.ServiceDate >= startDate && x.ServiceDate <= endDate)
                .Select(x => new ScheduleQuotationManDay
                {
                    BookingId = x.BookingId,
                    ManDay = x.NoOfManday.Value,
                    ServiceDate = x.ServiceDate
                }).AsNoTracking().ToListAsync();
        }
        //Fetch the county for the zone
        public async Task<List<int>> GetCountyByZone(IEnumerable<int> zoneIds)
        {
            return await _context.RefCounties
                .Where(x => x.Active && zoneIds.Contains(x.ZoneId.Value)).Select(x => x.Id).ToListAsync();

        }
        /// <summary>
        /// Get the quotation mandays by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<QuotationManday>> GetQuotationManDays(IQueryable<int> bookingIds)
        {
            return await _context.QuQuotationInsps//.Include(x => x.Quotation)
                .Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled) //&& x.Active.HasValue && x.Active.Value)
                .Select(x => new QuotationManday
                {
                    BookingId = x.IdBooking,
                    ManDay = x.NoOfManDay
                })
                .ToListAsync();
        }

        public async Task<List<ActualManday>> GetQcManDayData(List<int> qcIdList, List<DateTime> serviceDateList)
        {
            return await _context.SchScheduleQcs
                .Where(x => x.Active && qcIdList.Contains(x.Qcid) && serviceDateList.Contains(x.ServiceDate.Date))
                .GroupBy(x => new { x.Qcid, x.ServiceDate, x.Booking.CustomerId, x.Booking.FactoryId }, y => new { y.Qcid, y.ServiceDate }, (key, _data) =>
                        new ActualManday
                        {
                            CustomerId = key.CustomerId,
                            FactoryId = key.FactoryId,
                            ServiceDate = key.ServiceDate,
                            QcId = key.Qcid,
                            TotalBooking = _data.Select(x => x.ServiceDate).Count()
                        }
                 ).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the FB Entity Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public async Task<int?> GetFBEntityId(int entityId)
        {
            return await _context.ApEntities.Where(x => x.Id == entityId).Select(x => x.FbId).FirstOrDefaultAsync();
        }


        public async Task<List<ActualManday>> GetSchuduleQcs(IEnumerable<int> qcIdList, DateTime fromDate, DateTime toDate)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && qcIdList.Contains(x.Qcid) && x.ServiceDate >= fromDate && x.ServiceDate <= toDate).Select(x => new ActualManday()
            {
                QcId = x.Qcid,
                ServiceDate = x.ServiceDate,
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<ScheduleProductData>> GetProductPODetails(int bookingId)
        {
            int msFileType = (int)ProductRefFileType.MSChartExcel;
            return await _context.InspProductTransactions.Where(x => x.InspectionId == bookingId && x.Active.Value).Select(x => new ScheduleProductData()
            {
                ProductId = x.Product.ProductId,
                CuProductId = x.Product.Id,
                OrderQty = x.TotalBookingQuantity,
                IsMSChart = x.Product.CuProductFileAttachments.Where(y => y.FileTypeId == msFileType && y.Active).Any()
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<SchedulePOData>> GetPODetails(int bookingId)
        {
            return await _context.InspPurchaseOrderTransactions.Where(x => x.InspectionId == bookingId && x.Active.Value).Select(x => new SchedulePOData()
            {
                PONumber = x.Po.Pono,
                CuProductId = x.ProductRef.ProductId
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ScheduleQcCustomerFactory>> GetQCBookingCustomerFactory(IEnumerable<int> qcIds)
        {
            return await _context.SchScheduleQcs.Where(x => qcIds.Contains(x.Qcid) && x.Active == true)
                .Select(y => new ScheduleQcCustomerFactory
                {
                    CustomerId = y.Booking.CustomerId,
                    FactoryId = y.Booking.FactoryId,
                    Id = y.Id,
                    QcId = y.Qcid
                }).ToListAsync();
        }
    }
}
