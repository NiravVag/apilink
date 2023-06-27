using DTO.CommonClass;
using DTO.TravelTariff;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ITravelTariffRepository : IRepository
    {
        Task<bool> CheckTravelTarifExist(TravelTariffSaveRequest request);
        Task<EcAutTravelTariff> GetTravelTariff(int id);
        IQueryable<EcAutTravelTariff> GetTravelTariffList();
        Task<List<CommonDataSource>> GetStartPotList();
        Task<bool> CheckTravelStartPortEndPortExist(int startPort, int factoryTown);

        Task<int> AddTravelTariff(EcAutTravelTariff entity);

        Task<List<EcAutQcTravelExpense>> GetAutoQcFoodExpenseList(int townId, DateTime startDate, DateTime endDate);
        IQueryable<EcAutTravelTariff> GetTravelTariffData(int id);
    }
}
