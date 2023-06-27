using Contracts.Repositories;
using DTO.Common;
using DTO.Expense;
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
    public class ExpenseRepository : Repository, IExpenseRepository
    {
        private readonly IAPIUserContext _ApplicationContext = null;

        public ExpenseRepository(API_DBContext context, IAPIUserContext applicationContext) : base(context)
        {
            _ApplicationContext = applicationContext;
        }

        public async Task<(EcExpencesClaim, IEnumerable<ExpenseClaimReceipt>)> GetExpenseClaim(int id)
        {
            //var data2 = await _context.EcExpensesClaimDetais
            //.Where(x => x.ExpenseId == id)
            //  .Include(x => x.StartCity)
            // .Include(x => x.ArrivalCity)
            // //.Include(x => x.EcReceiptFiles)
            // .Include(x => x.ExpenseType)
            // .Include(x => x.Currency)
            //.ToListAsync();

            //var expenseIdList = data2.Select(x => x.Id);


            var datafiles = await _context.EcReceiptFiles
                    .Include(x => x.Expense)
                        .ThenInclude(x => x.Expense)
                .Where(x => x.Expense.ExpenseId == id)
                .Select(x => new ExpenseClaimReceipt
                {
                    Id = x.Id,
                    ExpenseId = x.ExpenseId,
                    FileName = x.FullFileName,
                    GuidId = x.GuidId,
                    FileUrl = ""
                }).ToListAsync();

            datafiles.AddRange(await _context.EcReceiptFileAttachments
                .Where(x => x.Expense.ExpenseId == id && x.Active)
                .Select(x => new ExpenseClaimReceipt
                {
                    Id = x.Id,
                    ExpenseId = x.ExpenseId,
                    FileName = x.FileName,
                    FileUrl = x.FileUrl,
                    Uniqueld = x.UniqueId
                }).ToListAsync());

            //foreach (var item in data2)
            //    item.EcReceiptFiles = datafiles.Where(x => x.ExpenseId == item.Id).ToList();


            var data = await _context.EcExpencesClaims
                        .Include(x => x.Country)
                        .Include(x => x.Staff)
                            .ThenInclude(x => x.PayrollCurrency)
                        .Include(x => x.Staff)
                            .ThenInclude(x => x.ItUserMasters)
                        .Include(x => x.Location)
                        .Include(x => x.Status)
                        .Include(x => x.EcExpensesClaimDetais)
                        .ThenInclude(x => x.StartCity)
                        .Include(x => x.EcExpensesClaimDetais)
                            .ThenInclude(x => x.ArrivalCity)
                        .Include(x => x.EcExpensesClaimDetais)
                            .ThenInclude(x => x.EcReceiptFiles)
                        .Include(x => x.EcExpensesClaimDetais)
                            .ThenInclude(x => x.ExpenseType)
                        .Include(x => x.EcExpensesClaimDetais)
                            .ThenInclude(x => x.Currency)

                            .Include(x => x.Staff)
                            .ThenInclude(x => x.EmployeeType)

                        .Include(x => x.EcExpensesClaimDetais)
                            .ThenInclude(x => x.EcReceiptFileAttachments)
                        .FirstOrDefaultAsync(x => x.Id == id);



            //data1.EcExpensesClaimDetais = data2;

            return (data, datafiles);

            //return await data1.Join(data2, x => x.Id, y => y.ExpenseId, (x, y) => x).FirstOrDefaultAsync(x => x.Id == id);


            //return await _context.EcExpencesClaims
            //            .Include(x => x.Country)
            //            .Include(x => x.Staff)
            //              .ThenInclude(x => x.PayrollCurrency)
            //            .Include(x => x.Location)
            //            .Include(x => x.EcExpensesClaimDetais)
            //              .ThenInclude(x => x.StartCity)
            //            .Include(x => x.EcExpensesClaimDetais)
            //              .ThenInclude(x => x.ArrivalCity)
            //            .Include(x => x.EcExpensesClaimDetais)
            //              .ThenInclude(x => x.EcReceiptFiles)
            //            .Include(x => x.EcExpensesClaimDetais)
            //              .ThenInclude(x => x.ExpenseType)
            //            .Include(x => x.EcExpensesClaimDetais)
            //              .ThenInclude(x => x.Currency)
            //            .Include(x => x.Status)
            //            .Include(x => x.Staff)
            //                .ThenInclude(x => x.ItUserMasters)
            //            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<EcExpensesType>> GetExpenseTypeList()
        {
            return await _context.EcExpensesTypes.Where(x => x.Active.HasValue && x.Active.Value).ToListAsync();
        }

        /// <summary>
        /// Get the expense types Iqueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<EcExpensesType> GetExpenseTypes()
        {
            return _context.EcExpensesTypes.Where(x => x.Active.HasValue && x.Active.Value);
        }

        public async Task<EcReceiptFile> GetFile(int id)
        {
            return await _context.EcReceiptFiles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<EcReceiptFile>> GetReceptFiles(IEnumerable<Guid> GuidList)
        {
            return await _context.EcReceiptFiles.Where(x => GuidList.Contains(x.GuidId)).ToListAsync();
        }

        public async Task<IEnumerable<EcExpClaimStatus>> GetStatusList()
        {
            return await _context.EcExpClaimStatuses
                // .Include(x => x.EcStatusRoles)
                //.Where(x => x.EcStatusRoles.Any(y => roleList.Contains(y.IdRole)))
                .ToListAsync();
        }

        public async Task<bool> SetStatus(int id, int statusId)
        {
            var item = await _context.EcExpencesClaims.FirstOrDefaultAsync(x => x.Id == id);
            var oldstatus = item.StatusId;
            //if the status is cancelled , then can't update the status.
            if (oldstatus == (int)ExpenseClaimStatus.Cancelled)
                return false;
            item.StatusId = statusId;

            switch ((ExpenseClaimStatus)statusId)
            {
                case ExpenseClaimStatus.Approved:
                case ExpenseClaimStatus.Rejected:
                    item.ApprovedId = _ApplicationContext.UserId;
                    item.ApprovedDate = DateTime.Now;
                    break;
                case ExpenseClaimStatus.Checked:
                    item.CheckedId = _ApplicationContext.UserId;
                    item.CheckedDate = DateTime.Now;
                    break;
                case ExpenseClaimStatus.Paid:
                    item.PaidId = _ApplicationContext.UserId;
                    item.PaidDate = DateTime.Now;
                    break;
            }

            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Reject(int id, string comment)
        {
            var item = await _context.EcExpencesClaims.FirstOrDefaultAsync(x => x.Id == id);

            item.StatusId = 3;
            item.ApprovedId = _ApplicationContext.UserId;
            item.ApprovedDate = DateTime.Now;
            item.Comment = comment;

            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return true;
        }
        /// <summary>
        /// GetExpenseClaimTypeList
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EcExpenseClaimtype>> GetExpenseClaimTypeList()
        {
            return await _context.EcExpenseClaimtypes.ToListAsync();
        }
        /// <summary>
        /// GetQCAssignedBookings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingDetail>> GetQCAssignedBookings(int userId, DateTime filterdate)
        {
            //where condition - cancel status booking with schedule user(no need active check)
            //and other status ExpenseBookingStatusList(mentioned in list) with schedule assigned user(active check)
            // and date filter and expense details table expense not canceled and not be auto expense and user has to there

            return await _context.InspTransactions.Where(x => x.ServiceDateFrom > filterdate &&
                ((ExpenseBookingStatusList.Contains(x.StatusId) && x.SchScheduleQcs.Any(y => y.Qcid == userId && y.Active))
                || (x.StatusId == (int)BookingStatus.Cancel && x.SchScheduleQcs.Any(y => y.Qcid == userId && y.Active)))
                && !x.EcExpensesClaimDetais.Any(x => x.Expense.StaffId == userId && !x.IsAutoExpense.Value &&
                x.Expense.StatusId != (int)ExpenseClaimStatus.Cancelled)).Distinct().
                         Select(x => new ExpenseBookingDetail
                         {
                             BookingId = x.Id,
                             Customer = x.Customer.CustomerName,
                             Supplier = x.Supplier.SupplierName,
                             Factory = x.Factory.SupplierName,
                             ServiceType = x.InspTranServiceTypes.Select(y => y.ServiceType.Name).FirstOrDefault(),
                             ServiceDateFrom = x.ServiceDateFrom.ToString(StandardDateFormat),
                             ServiceDateTo = x.ServiceDateTo.ToString(StandardDateFormat),
                             Selected = false
                         }).OrderBy(x => x.BookingId).ToListAsync();

        }

        public async Task<List<ExpenseBookingDetail>> GetEditExpenseQCAssignedBookings(int userId, DateTime filterdate)
        {

            return await _context.InspTransactions.Where(x => ((ExpenseBookingStatusList.Contains(x.StatusId)
            && x.SchScheduleQcs.Any(y => y.Qcid == userId && y.Active))
            || (x.StatusId == (int)BookingStatus.Cancel && x.SchScheduleQcs.Any(y => y.Qcid == userId && y.Active)))
                            && x.ServiceDateFrom > filterdate).Distinct().
                         Select(x => new ExpenseBookingDetail
                         {
                             BookingId = x.Id,
                             Customer = x.Customer.CustomerName,
                             Supplier = x.Supplier.SupplierName,
                             Factory = x.Factory.SupplierName,
                             ServiceType = x.InspTranServiceTypes.Select(y => y.ServiceType.Name).FirstOrDefault(),
                             ServiceDateFrom = x.ServiceDateFrom.ToString(StandardDateFormat),
                             ServiceDateTo = x.ServiceDateTo.ToString(StandardDateFormat),
                             Selected = false
                         }).OrderBy(x => x.BookingId).ToListAsync();

        }

        /// <summary>
        /// GetAuditorAssignedBookings
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingDetail>> GetAuditorAssignedBookings(int userId, DateTime filterdate)
        {
            //where condition - cancel status booking with audit user(no need active check)
            //and not equal to scheduled with schedule assigned user(active check) and date filter

            return await _context.AudTransactions.Where(x => x.ServiceDateFrom >= filterdate &&
            ((x.StatusId == (int)AuditStatus.Cancel && x.AudTranAuditors.Any(y => y.StaffId == userId)) ||
            (x.StatusId != (int)AuditStatus.Scheduled && x.AudTranAuditors.Any(y => y.StaffId == userId && y.Active))))
                      .Distinct().
                      Select(x => new ExpenseBookingDetail
                      {
                          BookingId = x.Id,
                          Customer = x.Customer.CustomerName,
                          Supplier = x.Supplier.SupplierName,
                          Factory = x.Factory.SupplierName,
                          ServiceType = x.AudTranServiceTypes.Select(y => y.ServiceType.Name).FirstOrDefault(),
                          ServiceDateFrom = x.ServiceDateFrom.ToString(StandardDateFormat),
                          ServiceDateTo = x.ServiceDateTo.ToString(StandardDateFormat),
                          Selected = false
                      }).OrderBy(x => x.BookingId).ToListAsync();
        }

        public async Task<List<ExpenseBookingDetail>> GetAuditbookingByExpenseId(int expenseId)
        {
            return await (from expeclaim in _context.EcExpencesClaims
                          join expclaimdetails in _context.EcExpensesClaimDetais on expeclaim.Id equals expclaimdetails.ExpenseId into expenseData
                          from expense in expenseData.DefaultIfEmpty()
                          join audittra in _context.AudTransactions on expense.AuditId equals audittra.Id
                          where expeclaim.Id == expenseId
                          select new ExpenseBookingDetail
                          {
                              BookingId = audittra.Id,
                              ServiceDateFrom = audittra.ServiceDateFrom.ToString(StandardDateFormat),
                              ServiceDateTo = audittra.ServiceDateTo.ToString(StandardDateFormat)
                          }).Distinct().ToListAsync();
        }
        public async Task<List<ExpenseBookingDetail>> GetInspbookingByExpenseId(int expenseId)
        {
            return await (from expeclaim in _context.EcExpencesClaims
                          join expclaimdetails in _context.EcExpensesClaimDetais on expeclaim.Id equals expclaimdetails.ExpenseId into expenseData
                          from expense in expenseData.DefaultIfEmpty()
                          join insptra in _context.InspTransactions on expense.InspectionId equals insptra.Id
                          where expeclaim.Id == expenseId
                          select new ExpenseBookingDetail
                          {
                              BookingId = insptra.Id,
                              ServiceDateFrom = insptra.ServiceDateFrom.ToString(StandardDateFormat),
                              ServiceDateTo = insptra.ServiceDateTo.ToString(StandardDateFormat),
                          }).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get the audit booking details by expense id
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingDetail>> GetAuditbookingsByExpenseId(int expenseId)
        {
            return await _context.EcExpensesClaimDetais.Where(x => x.Active.HasValue && x.Active.Value && x.ExpenseId == expenseId).
                Select(x => new ExpenseBookingDetail()
                {
                    BookingId = x.Inspection.Id,
                    ServiceDateFrom = x.Inspection.ServiceDateFrom.ToString(StandardDateFormat),
                    ServiceDateTo = x.Inspection.ServiceDateTo.ToString(StandardDateFormat),
                }).Distinct().AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the inspection bookings by expense id
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingDetail>> GetInspbookingsByExpenseId(int expenseId)
        {
            return await _context.EcExpensesClaimDetais.Where(x => x.Active.HasValue && x.Active.Value && x.ExpenseId == expenseId).
                    Select(x => new ExpenseBookingDetail()
                    {
                        BookingId = x.Inspection.Id,
                        ServiceDateFrom = x.Inspection.ServiceDateFrom.ToString(StandardDateFormat),
                        ServiceDateTo = x.Inspection.ServiceDateTo.ToString(StandardDateFormat),

                    }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// GetExpenseInspectionBookings
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="expenseId"></param>
        /// <param name="BookingIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetExpenseInspectionBookings(int staffId, int? expenseId, List<int> BookingIds)
        {

            return await (from expeclaim in _context.EcExpencesClaims
                          join expclaimdetails in _context.EcExpensesClaimDetais on expeclaim.Id equals expclaimdetails.ExpenseId into expenseData
                          from expense in expenseData.DefaultIfEmpty()
                              //join expinsp in _context.EcExpenseClaimsInspections on expense.Id equals expinsp.ExpenseClaimDetailId
                          where expeclaim.StaffId == staffId && expeclaim.Active.HasValue && expeclaim.Active.Value
                                 && expeclaim.StatusId != (int)ExpenseClaimStatus.Cancelled
                                           && expeclaim.ClaimTypeId == (int)ClaimTypeEnum.Inspection && expeclaim.Id != expenseId &&
                                           BookingIds.Contains(expense.InspectionId.Value)
                          select expense.InspectionId.Value).Distinct().ToListAsync();

        }
        /// <summary>
        /// GetExpenseAuditBookings
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="expenseId"></param>
        /// <param name="BookingIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetExpenseAuditBookings(int staffId, int? expenseId, List<int> BookingIds)
        {

            return await (from expeclaim in _context.EcExpencesClaims
                          join expclaimdetails in _context.EcExpensesClaimDetais on expeclaim.Id equals expclaimdetails.ExpenseId into expenseData
                          from expense in expenseData.DefaultIfEmpty()
                          where expeclaim.StaffId == staffId && expeclaim.Active.HasValue && expeclaim.Active.Value
                                 && expeclaim.StatusId != (int)ExpenseClaimStatus.Cancelled
                                           && expeclaim.ClaimTypeId == (int)ClaimTypeEnum.Audit && expeclaim.Id != expenseId &&
                                           BookingIds.Contains(expense.AuditId.Value)
                          select expense.AuditId.Value).Distinct().ToListAsync();



            //return await expenseClaims.SelectMany(x => x.EcExpensesClaimDetais).Where(x => x.Active.HasValue && x.Active.Value)
            //                    .SelectMany(y => y.EcExpenseClaimsAudits).Where(x => x.Active)
            //                    .Select(z => z.BookingId).ToListAsync();

        }
        /// <summary>
        /// GetExpenseClaimById
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        public async Task<EcExpencesClaim> GetExpenseClaimById(int? expenseId)
        {
            return await _context.EcExpencesClaims.Where(x => x.Id == expenseId && x.Active.HasValue && x.Active.Value).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetHRProfileByStaffId
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HrStaffProfile>> GetHRProfileByStaffId(int staffId)
        {
            return await _context.HrStaffProfiles.Where(y => y.StaffId == staffId).ToListAsync();
        }

        /// <summary>
        /// Get claim details
        /// </summary>
        /// <param name="claim id"></param>
        /// <returns></returns>
        public async Task<List<ExpenseClaimVoucherItem>> ExpenseClaimDetails(List<int> claimDetailIdList)
        {
            return await _context.EcExpensesClaimDetais.Where(x => claimDetailIdList.Contains(x.Expense.Id) && x.Active.HasValue && x.Active == true)
                .Select(x => new ExpenseClaimVoucherItem
                {
                    Id = x.Expense.Id,
                    ClaimDetailId = x.Id,
                    Date = x.Expense.ClaimDate,
                    ClaimTypeId = x.Expense.ClaimTypeId,
                    Amount = x.Amount,
                    StaffId = x.Expense.StaffId,
                    StaffName = x.Expense.Staff.PersonName,
                    RegionalName = x.Expense.Staff.NameChinese,
                    LocationId = x.Expense.LocationId,
                    LocationName = x.Expense.Location.LocationName,
                    DeptId = x.Expense.Staff.DepartmentId,
                    ClaimNo = x.Expense.ClaimNo,
                    BankAccountNo = x.Expense.Staff.BankAccountNo,
                    PayrollCurrency = x.Expense.Staff.PayrollCurrency.CurrencyName,
                    Status = x.Expense.Status.Description,
                    ClaimType = x.Expense.ClaimType.Name,
                    PaidId = x.Expense.PaidId,
                    ExpenseTypeId = x.ExpenseTypeId,
                    PayrollCompanyName = x.Expense.Staff.PayrollCompanyNavigation.CompanyName,
                    ExpenseDate = x.ExpenseDate,
                    InspectionId = x.InspectionId,
                    AuditId = x.AuditId,
                    CountryName = x.Expense.Country.CountryName,
                    CurrencyName = x.Currency.CurrencyName,
                    ExpenseStatus = x.Expense.Status.Description,
                    TripTypeName = x.TripTypeNavigation.Name,
                    StartPortName = x.Expense.Staff.StartPortNavigation.StartPortName,
                    FromCity = x.StartCity.CityName,
                    ToCity = x.ArrivalCity.CityName,
                    IsAutoExpense = x.IsAutoExpense
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get booking data by claim id
        /// </summary>
        /// <param name="claim id"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingData>> BookingDataByExpenseId(List<int> claimDetailIdList)
        {
            return await _context.EcExpensesClaimDetais.Where(x => claimDetailIdList.Contains(x.Id))
                .Select(x => new ExpenseBookingData
                {
                    ClaimId = x.ExpenseId,
                    ClaimDetailId = x.Id,
                    BookingId = x.InspectionId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get audit data by claim id
        /// </summary>
        /// <param name="claim id"></param>
        /// <returns></returns>
        public async Task<List<ExpenseAuditData>> AuditDataByExpenseId(List<int> claimDetailIdList)
        {
            return await _context.EcExpensesClaimDetais.Where(x => claimDetailIdList.Contains(x.Id))
                .Select(x => new ExpenseAuditData
                {
                    ClaimId = x.ExpenseId,
                    ClaimDetailId = x.Id,
                    AuditId = x.AuditId
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<EcExpencesClaim>> GetExpenseClaimByQcAndBookingList(List<int> qcList, List<int> bookingList)
        {
            var pendingStatus = (int)ExpenseClaimStatus.Pending;
            return await _context.EcExpencesClaims.
                                  Include(x => x.EcExpensesClaimDetais).
                                  Where(x => x.StatusId == pendingStatus
                                  && qcList.Contains(x.StaffId) &&
                                  x.EcExpensesClaimDetais.Any(x => x.IsAutoExpense.Value && bookingList.Contains(x.InspectionId.Value))
                                  && x.Active.HasValue && x.Active.Value)
                                  .ToListAsync();
        }


        /// <summary>
        /// /// Get the booking ids for the staff and list of booking ids involved in any booking other than given expense claim id
        /// </summary>
        /// <param name="expenseClaimId"></param>
        /// <param name="staffId"></param>
        /// <param name="bookingList"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetExpenseBookingIdsForQC(int expenseClaimId, int staffId, List<int> bookingList)
        {
            var cancelledStatus = (int)ExpenseClaimStatus.Cancelled;
            return await _context.EcExpensesClaimDetais.Where(x => x.Expense.StaffId == staffId
                        && x.Expense.Id != expenseClaimId && !x.IsAutoExpense.Value
                        && x.Expense.StatusId != cancelledStatus && bookingList.Contains(x.InspectionId.Value))
                        .Select(x => x.InspectionId).ToListAsync();

        }

        /// <summary>
        /// Get the booking ids duplicated in edit scenario
        /// </summary>
        /// <param name="expenseClaimId"></param>
        /// <param name="staffId"></param>
        /// <param name="bookingList"></param>
        /// <param name="isAutoQCExpense"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetEditExpenseBookingIdsForQC(int expenseClaimId, int staffId, List<int> bookingList, bool isAutoQCExpense)
        {

            var cancelledStatus = (int)ExpenseClaimStatus.Cancelled;

            return await _context.EcExpensesClaimDetais.Where(x => x.Expense.StaffId == staffId
                         && x.Expense.Id != expenseClaimId && x.IsAutoExpense.Value == isAutoQCExpense
                         && x.Expense.StatusId != cancelledStatus && bookingList.Contains(x.InspectionId.Value))
                        .Select(x => x.InspectionId).ToListAsync();

        }


        public async Task<bool> CheckPendingFoodExpenseExist(int QcId, List<int> BookingList)
        {
            return await _context.EcAutQcFoodExpenses.
                            AnyAsync(x => x.QcId == QcId && BookingList.Contains(x.InspectionId.GetValueOrDefault())
                            && x.Active.Value && x.IsFoodAllowanceConfigured.Value && !x.IsExpenseCreated.Value);
        }

        public async Task<bool> CheckPendingTravelExpenseExist(int QcId, List<int> BookingList)
        {
            return await _context.EcAutQcTravelExpenses.
                            AnyAsync(x => x.QcId == QcId && BookingList.Contains(x.InspectionId.GetValueOrDefault())
                            && x.Active.Value && x.IsTravelAllowanceConfigured.Value && !x.IsExpenseCreated.Value);
        }

        /// <summary>
        /// Get the booking details for outsource qc
        /// </summary>
        /// <param name="outSourceCompanyId"></param>
        /// <param name="minDate"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingDetail>> GetOutSourceQCBookingDetails(int outSourceCompanyId, DateTime minDate)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && x.Qc.HroutSourceCompanyId == outSourceCompanyId
                            && x.Booking.ServiceDateFrom > minDate
                            && (x.Booking.StatusId == (int)BookingStatus.AllocateQC || InspectedStatusList.Contains(x.Booking.StatusId))
                            && !x.Qc.EcExpencesClaims.Any(y => y.StatusId != (int)ExpenseClaimStatus.Cancelled &&
                                y.EcExpensesClaimDetais.Any(z => z.InspectionId == x.BookingId))
                            ).Distinct().
                            Select(x => new ExpenseBookingDetail()
                            {
                                BookingId = x.Booking.Id,
                                Customer = x.Booking.Customer.CustomerName,
                                Supplier = x.Booking.Supplier.SupplierName,
                                Factory = x.Booking.Factory.SupplierName,
                                ServiceType = x.Booking.InspTranServiceTypes.Select(y => y.ServiceType.Name).FirstOrDefault(),
                                ServiceDateFrom = x.Booking.ServiceDateFrom.ToString(StandardDateFormat),
                                ServiceDateTo = x.Booking.ServiceDateTo.ToString(StandardDateFormat),
                                Selected = false,
                                QCId = x.Qcid,
                                QCName = x.Qc.PersonName
                            }).OrderBy(x => x.BookingId).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the outsource qc booking details for edit scenario
        /// </summary>
        /// <param name="qcId"></param>
        /// <param name="outSourceCompanyId"></param>
        /// <param name="minDate"></param>
        /// <returns></returns>
        public async Task<List<ExpenseBookingDetail>> GetEditOutSourceQCBookingDetails(int qcId, int outSourceCompanyId, DateTime minDate)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && x.Qc.HroutSourceCompanyId == outSourceCompanyId
                            && x.Booking.ServiceDateFrom > minDate && x.Qcid == qcId
                            && (x.Booking.StatusId == (int)BookingStatus.AllocateQC || InspectedStatusList.Contains(x.Booking.StatusId)))
                            .Distinct().
                            Select(x => new ExpenseBookingDetail()
                            {
                                BookingId = x.Booking.Id,
                                Customer = x.Booking.Customer.CustomerName,
                                Supplier = x.Booking.Supplier.SupplierName,
                                Factory = x.Booking.Factory.SupplierName,
                                ServiceType = x.Booking.InspTranServiceTypes.Select(y => y.ServiceType.Name).FirstOrDefault(),
                                ServiceDateFrom = x.Booking.ServiceDateFrom.ToString(StandardDateFormat),
                                ServiceDateTo = x.Booking.ServiceDateTo.ToString(StandardDateFormat),
                                Selected = false,
                                QCId = x.Qcid,
                                QCName = x.Qc.PersonName
                            }).OrderBy(x => x.BookingId).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the expense claim list for the email
        /// </summary>
        /// <param name="expenseClaimIds"></param>
        /// <returns></returns>
        public async Task<List<ExpenseClaim>> GetExpenseClaimListForEmail(List<int> expenseClaimIds)
        {
            return await _context.EcExpencesClaims.Where(x => x.Active.Value && expenseClaimIds.Contains(x.Id)).
                    Select(x => new ExpenseClaim()
                    {
                        Id = x.Id,
                        ClaimNo = x.ClaimNo,
                        ExpenseAmout = x.EcExpensesClaimDetais.Where(x => x.ExpenseTypeId != (int)Entities.Enums.ExpenseType.FoodAllowance).Sum(x => x.Amount),
                        Name = x.Staff.PersonName,
                        TotalAmount = x.EcExpensesClaimDetais.Sum(x => x.Amount),
                        CurrencyName = x.Staff.PayrollCurrency.CurrencyName,
                        Status = x.Status.Description,
                        ExpensePuropose = x.ExpensePurpose
                        //ExpenseAmout=x.Ex
                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the qc by expense id
        /// </summary>
        /// <param name="expenseId"></param>
        /// <returns></returns>
        public async Task<int> GetQCByExpenseId(int expenseId)
        {
            return await _context.EcExpencesClaims.Where(x => x.Active.Value && x.Id == expenseId).
                Select(x => x.StaffId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check expense claim number already exist
        /// </summary>
        /// <param name="expenseClaimNo"></param>
        /// <returns></returns>
        public async Task<bool> CheckExpenseClaimNoExist(string expenseClaimNo)
        {
            return await _context.EcExpencesClaims.Where(x => x.Active.Value && x.ClaimNo == expenseClaimNo).AnyAsync();
        }

        /// <summary>
        /// get food allowance 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<decimal> GetExpenseFoodAllowance(ExpenseFoodClaimRequest request)
        {
            var _dateTime = request.ExpenseDate.ToDateTime();

            return await _context.EcFoodAllowances.Where(x => x.Active.Value && _dateTime >= x.StartDate &&
            _dateTime <= x.EndDate && x.CountryId == request.CountryId && x.CurrencyId == request.CurrencyId).
            Select(x => x.FoodAllowance).FirstOrDefaultAsync();
        }
        /// <summary>
        /// get travel data
        /// </summary>
        /// <param name="qcTravelExpenseIdList"></param>
        /// <returns></returns>
        public async Task<List<ExpenseQCPort>> GetExpenseTravelPortList(List<int?> qcTravelExpenseIdList)
        {
            return await _context.EcAutQcTravelExpenses.Where(x => qcTravelExpenseIdList.Contains(x.Id)).Where(x => x.Active.Value).Select(x => new ExpenseQCPort()
            {
                EndPort = x.FactoryTownNavigation.TownName,
                StartPort = x.StartPortNavigation.StartPortName,
                TravelQCExpenseId = x.Id
            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="QcId"></param>
        /// <param name="BookingIdList"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetPendingFoodExpenseBookingIds(int QcId, List<int?> BookingIdList)
        {
            return await _context.EcAutQcFoodExpenses.
                            Where(x => x.QcId == QcId && BookingIdList.Contains(x.InspectionId)
                            && x.Active.Value && !x.IsExpenseCreated.Value)
                            .Select(x => x.InspectionId).Distinct().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="QcId"></param>
        /// <param name="BookingIdList"></param>
        /// <returns></returns>
        public async Task<List<int?>> GetPendingTravelExpenseBookingIds(int QcId, List<int?> BookingIdList)
        {
            return await _context.EcAutQcTravelExpenses.
                            Where(x => x.QcId == QcId && BookingIdList.Contains(x.InspectionId)
                            && x.Active.Value && !x.IsExpenseCreated.Value)
                            .Select(x => x.InspectionId).Distinct().ToListAsync();
        }

        /// <summary>
        /// get expense details by expense id list
        /// </summary>
        /// <param name="expenseIdList"></param>
        /// <returns></returns>
        public async Task<List<ExpenseDetailsRepo>> GetExpenseDetailsListByExpenseId(List<int> expenseIdList)
        {
            return await _context.EcExpensesClaimDetais.Where(x => expenseIdList.Contains(x.ExpenseId) && x.Active.Value).
                Select(x => new ExpenseDetailsRepo()
                {
                    BookingId = x.InspectionId,
                    IsAutoExpense = x.IsAutoExpense,
                    Amount = x.Amount,
                    ExpenseId = x.ExpenseId,
                    ExpenseTypeId = x.ExpenseTypeId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get expense iqueryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<EcExpencesClaim> GetIqueryableExpense()
        {
            return _context.EcExpencesClaims;
        }
    }
}
