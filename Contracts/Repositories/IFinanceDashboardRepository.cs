using DTO.Dashboard;
using DTO.FinanceDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IFinanceDashboardRepository
    {
        Task<FinanceDashboardMandayData> GetBilledMandayData(MandaySPRequest request);
        Task<FinanceDashboardMandayData> GetMandayRateData(MandaySPRequest request);
        Task<List<BookingDetail>> GetFinanceDashboardBookingDetail(FinanceDashboardRequest request);
        Task<List<FinanceTurnOverDbItem>> GetFinanceTurnOverData(TurnOverSpRequest request);
        Task<ChargeBackSpData> GetChargeBackChartData(TurnOverSpRequest request);
        Task<QuotationCountData> GetQuotationChartData(TurnOverSpRequest request);
        Task<List<FinanceDashboardBilledMandayRepo>> GetInspectionBilledManDays(IQueryable<int> inspectionIds);
        Task<List<FinanceDashboardInspectionFeesRepo>> GetInspectionFees(IQueryable<int> inspectionIds);
        Task<List<FinanceDashboardScheduleMandayRepo>> GetInspectionScheduleManDays(IQueryable<int> inspectionIds, List<int> empTypeIds);
        Task<List<FinanceDashboardInspectionFeesRepo>> GetExtraFeeByInspection(IQueryable<int> bookingIdList);
        Task<List<ExchangeCurrencyItem>> GetBookingExpense(IQueryable<int> bookingIdList);
    }
}
