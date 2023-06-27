using DTO.Schedule;
using DTO.ScheduleJob;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IScheduleJobRepository : IRepository
    {
        IQueryable<InspProductTransaction> GetCulturaScheduleJobData(int customerid);
        IQueryable<InspProductTransaction> GetScheduleJobData();
        Task<List<ScheduleTravelTariffEmail>> GetInActivatTravelTariffList();
        Task<List<EcAutQcTravelExpense>> GetAutoQcTravelExpenseData(List<int> travelExpenseIds);
        Task<List<StartPortCity>> GetCityIdByStartPortList(List<int?> startPortIds);
        Task<List<FactoryTownCity>> GetCityIdByTownIdList(List<int?> townIds);
        IQueryable<EcAutQcTravelExpense> GetQcTravelExpenseData();
        IQueryable<EcAutQcFoodExpense> GetQcFoodExpenseData();
        Task<List<EcAutQcFoodExpense>> GetAutoQcFoodExpenseData(List<int> foodExpenseIds);
        Task<List<ScheduleClaimReminderEmail>> GetClaimReminderList(IQueryable<ClmTransaction> claimQuery);
        Task<List<JobConfiguration>> GetScheduleJobConfigurationList();
        Task<List<ProductDetail>> GetProductTransactionList(List<int> bookingIds);
        Task<List<CustomerDetail>> GetCustomerBookingDetails(List<int> customerIds);
        Task<List<JobConfiguration>> GetScheduleJobConfigurations(List<int> typeId);
        Task<JobConfiguration> GetScheduleJobConfiguration(int configureId);
    }
}
