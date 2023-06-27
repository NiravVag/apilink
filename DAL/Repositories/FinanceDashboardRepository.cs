using Contracts.Repositories;
using DAL.Helper;
using DTO.Common;
using DTO.Dashboard;
using DTO.Expense;
using DTO.ExtraFees;
using DTO.FinanceDashboard;
using DTO.Invoice;
using DTO.Manday;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FinanceDashboardRepository : Repository, IFinanceDashboardRepository
    {
        private static IConfiguration _configuration = null;

        public FinanceDashboardRepository(API_DBContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public async Task<FinanceDashboardMandayData> GetBilledMandayData(MandaySPRequest request)
        {
            FinanceDashboardMandayData response = new FinanceDashboardMandayData();
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, FinanceDashboardBilledManday_sp, request);

            response.BilledManday = await ADOHelper.ConvertDataTable<MandayYearChartItem>(dataSet.Tables[0]);
            response.BilledMandayBudget = await ADOHelper.ConvertDataTable<MandayYearChartItem>(dataSet.Tables[1]);


            return response;
        }

        public async Task<FinanceDashboardMandayData> GetMandayRateData(MandaySPRequest request)
        {
            FinanceDashboardMandayData response = new FinanceDashboardMandayData();
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, FinanceDashboardMandayRate_sp, request);
            try
            {
                response.MandayRate = await ADOHelper.ConvertDataTable<MandayRateItem>(dataSet.Tables[0]);
                response.MandayRateBudget = await ADOHelper.ConvertDataTable<MandayRateItem>(dataSet.Tables[1]);
            }
            catch (Exception e)
            {
                var l = e;
            }

            return response;
        }

        /// <summary>
        /// Get the booking detail by customerdashboard filter request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Base booking detail</returns>
        public async Task<List<BookingDetail>> GetFinanceDashboardBookingDetail(FinanceDashboardRequest request)
        {
            var inspTransactions = _context.InspTransactions.Where(x => InspectedStatusList.Contains(x.StatusId));
            if (request != null)
            {
                if (request.CustomerId != null && request.CustomerId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.CustomerId == request.CustomerId);
                }

                if (request.SupplierId != null && request.SupplierId > 0)
                {
                    inspTransactions = inspTransactions.Where(x => x.SupplierId == request.SupplierId);
                }

                if (request.FactoryIdList != null && request.FactoryIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.FactoryId > 0 && request.FactoryIdList.Contains(x.FactoryId.GetValueOrDefault()));
                }

                if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
                {
                    inspTransactions = inspTransactions.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                }

                if (request.BrandIdList != null && request.BrandIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuBrands.Any(y => request.BrandIdList.Contains(y.BrandId)));
                }

                if (request.BuyerIdList != null && request.BuyerIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuBuyers.Any(y => request.BuyerIdList.Contains(y.BuyerId)));
                }

                if (request.DeptIdList != null && request.DeptIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranCuDepartments.Any(y => request.DeptIdList.Contains(y.DepartmentId)));
                }

                if (request.OfficeIdList != null && request.OfficeIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => request.OfficeIdList.Contains(x.OfficeId.GetValueOrDefault()));
                }

                if (request.ServiceTypeList != null && request.ServiceTypeList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.InspTranServiceTypes.Any(y => request.ServiceTypeList.Contains(y.ServiceTypeId)));
                }

                if (request.CountryIdList != null && request.CountryIdList.Any())
                {
                    inspTransactions = inspTransactions.Where(x => x.Factory.SuAddresses.Any(y => request.CountryIdList.Contains(y.CountryId)));
                }
            }

            return await inspTransactions.
                                    Select(x =>
                                    new BookingDetail
                                    {
                                        InspectionId = x.Id,
                                        CustomerId = x.CustomerId,
                                        SupplierId = x.SupplierId,
                                        FactoryId = x.FactoryId,
                                        CreationDate = x.CreatedOn.Value,
                                        ServiceDateFrom = x.ServiceDateFrom,
                                        ServiceDateTo = x.ServiceDateTo,
                                        StatusId = x.StatusId
                                    }).AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// get the total invoice fee and extra fee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<FinanceTurnOverDbItem>> GetFinanceTurnOverData(TurnOverSpRequest request)
        {
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, FinanceDashboardTurnOver_sp, request);
            var response = await ADOHelper.ConvertDataTable<FinanceTurnOverDbItem>(dataSet.Tables[0]);

            return response;
        }

        /// <summary>
        /// get the total invoice fee and extra fee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ChargeBackSpData> GetChargeBackChartData(TurnOverSpRequest request)
        {
            var response = new ChargeBackSpData();
            var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, FinanceDashboardChargeBack_sp, request);
            response.TotalExpenseAmount = await ADOHelper.ConvertDataTable<ExchangeCurrencyItem>(dataSet.Tables[0]);
            response.InvoiceData = await ADOHelper.ConvertDataTable<FinanceTurnOverDbItem>(dataSet.Tables[1]);

            return response;
        }

        /// <summary>
        /// get the total quotation count and rejected quotation count
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QuotationCountData> GetQuotationChartData(TurnOverSpRequest request)
        {
            var response = new QuotationCountData();
            try
            {
                var dataSet = await ADOHelper.GetLinkDataSource(_configuration, DbConnectionName, FinanceDashboardQuotationCount_sp, request);
                response = ADOHelper.ConvertDataTable<QuotationCountData>(dataSet.Tables[0]).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
                var l = e;
            }
            return response;
        }
        /// <summary>
        /// Get the BilledManday by inspectio
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<FinanceDashboardBilledMandayRepo>> GetInspectionBilledManDays(IQueryable<int> inspectionIds)
        {
            return await _context.QuQuotationInsps.Where(x => inspectionIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus != (int)QuotationStatus.Canceled && x.IdBookingNavigation.StatusId != (int)BookingStatus.Cancel)
                .Select(x => new FinanceDashboardBilledMandayRepo()
                {
                    CustomerId = x.IdBookingNavigation.CustomerId,
                    Customer = x.IdBookingNavigation.Customer.CustomerName,
                    BilledManday = x.NoOfManDay
                }).GroupBy(p => new { p.CustomerId, p.Customer }, (key, _data) => new FinanceDashboardBilledMandayRepo()
                {
                    CustomerId = key.CustomerId,
                    Customer = key.Customer,
                    BilledManday = _data.Sum(x => x.BilledManday)
                })
                .AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the InspectionFees by inspectio
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<FinanceDashboardInspectionFeesRepo>> GetInspectionFees(IQueryable<int> inspectionIds)
        {
            return await _context.InvAutTranDetails.Where(x => x.InspectionId != null && x.InvoiceStatus != (int)InvoiceStatus.Cancelled && inspectionIds.Contains(x.InspectionId.GetValueOrDefault()) && x.Inspection.StatusId != (int)BookingStatus.Cancel)
                .Select(x => new FinanceDashboardInspectionFeesRepo()
                {
                    CustomerId = x.Inspection.CustomerId,
                    Customer = x.Inspection.Customer.CustomerName,
                    inspectionFees = x.InspectionFees,
                    discount = x.Discount,
                    CurrencyId = x.InvoiceCurrency,
                    TravelAir = x.TravelAirFees,
                    TravleLand = x.TravelLandFees,
                    HotelFee = x.HotelFees,
                    OtherFee = x.OtherFees
                }).GroupBy(p => new { p.CustomerId, p.Customer, p.CurrencyId }, (key, _data) => new FinanceDashboardInspectionFeesRepo()
                {
                    CustomerId = key.CustomerId,
                    Customer = key.Customer,
                    inspectionFees = _data.Sum(x => x.inspectionFees),
                    discount = _data.Sum(x => x.discount),
                    CurrencyId = key.CurrencyId,
                    TravelAir = _data.Sum(x => x.TravelAir),
                    TravleLand = _data.Sum(x => x.TravleLand),
                    HotelFee = _data.Sum(x => x.HotelFee),
                    OtherFee = _data.Sum(x => x.OtherFee)
                })
                .AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the schedule Manday by inspectio
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<FinanceDashboardScheduleMandayRepo>> GetInspectionScheduleManDays(IQueryable<int> inspectionIds, List<int> typeIds)
        {
            var data = _context.SchScheduleQcs.Where(x => inspectionIds.Contains(x.BookingId) && x.Active);

            if (typeIds != null && typeIds.Any())
            {
                data = data.Where(x => typeIds.Contains(x.Qc.EmployeeTypeId));
            }

            return await data
                .Select(x => new FinanceDashboardScheduleMandayRepo()
                {
                    QcId = x.Qc.Id,
                    ServiceDate = x.ServiceDate,
                    CustomerId = x.Booking.CustomerId,
                    ActualManDay = x.ActualManDay,
                    EmployeeTypeId = x.Qc.EmployeeTypeId
                })
               .GroupBy(p => new { p.QcId, p.ServiceDate, p.CustomerId, p.EmployeeTypeId }, (key, _data) => new FinanceDashboardScheduleMandayRepo()
               {
                   CustomerId = key.CustomerId,
                   ActualManDay = _data.Sum(x => x.ActualManDay),
                   EmployeeTypeId = key.EmployeeTypeId,
                   QcId = key.QcId,
                   ServiceDate = key.ServiceDate
               }).AsNoTracking().ToListAsync();
        }

        public async Task<List<FinanceDashboardInspectionFeesRepo>> GetExtraFeeByInspection(IQueryable<int> bookingIdList)
        {
            return await _context.InvExfTransactions.Where(x => x.Active.HasValue && x.StatusId == (int)ExtraFeeStatus.Invoiced && x.Active == true && bookingIdList.Contains(x.InspectionId.GetValueOrDefault()))
                .Select(x => new FinanceDashboardInspectionFeesRepo
                {
                    BookingId = x.InspectionId.GetValueOrDefault(),
                    ExtraFeeCurrencyId = x.CurrencyId,
                    ExtraFees = x.TotalExtraFee,
                    CustomerId = x.Inspection.CustomerId
                })
                .GroupBy(p => new { p.CustomerId, p.ExtraFeeCurrencyId }, (key, _data) => new FinanceDashboardInspectionFeesRepo()
                {
                    CustomerId = key.CustomerId,
                    ExtraFeeCurrencyId = key.ExtraFeeCurrencyId,
                    ExtraFees = _data.Sum(x => x.ExtraFees)
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ExchangeCurrencyItem>> GetBookingExpense(IQueryable<int> bookingIdList)
        {
            return await (from expeclaim in _context.EcExpencesClaims
                          join expclaimdetails in _context.EcExpensesClaimDetais on expeclaim.Id equals expclaimdetails.ExpenseId into expenseData
                          from expense in expenseData.DefaultIfEmpty()
                          join insp in _context.InspTransactions on expense.InspectionId equals insp.Id
                          where expeclaim.Active.HasValue && expeclaim.Active.Value
                                 && (expeclaim.StatusId == (int)ExpenseClaimStatus.Checked || expeclaim.StatusId == (int)ExpenseClaimStatus.Paid ||
                                 expeclaim.StatusId == (int)ExpenseClaimStatus.Approved)
                                           && expeclaim.ClaimTypeId == (int)ClaimTypeEnum.Inspection &&
                                           bookingIdList.Contains(expense.InspectionId.Value)
                          select new ExchangeCurrencyItem
                          {
                              Fee = expense.AmmountHk.GetValueOrDefault(),
                              CurrencyId = expense.CurrencyId,
                              CustomerId = insp.CustomerId
                          })
                          .GroupBy(p => new { p.CustomerId, p.CurrencyId }, (key, _data) => new ExchangeCurrencyItem()
                          {
                              Fee = _data.Sum(x => x.Fee),
                              CurrencyId = key.CurrencyId,
                              CustomerId = key.CustomerId
                          }).Distinct().ToListAsync();
        }
    }
}
