using Contracts.Repositories;
using DTO.CommonClass;
using DTO.TravelTariff;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace DAL.Repositories
{
    public class TravelTariffRepository : Repository, ITravelTariffRepository
    {
        public TravelTariffRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<bool> CheckTravelTarifExist(TravelTariffSaveRequest request)
        {
            return await _context.EcAutTravelTariffs.
                  Where(x => x.Id != request.Id && x.Active.Value
                  && x.StartPort == request.StartPort && x.TownId == request.TownId
                  && !((x.StartDate > request.StartDate.ToDateTime()) || (x.EndDate < request.EndDate.ToDateTime()))).AnyAsync();
        }

        public async Task<List<EcAutQcTravelExpense>> GetAutoQcFoodExpenseList(int townId, DateTime startDate, DateTime endDate)
        {
            return await _context.EcAutQcTravelExpenses.
                   Where(x => x.Active.Value
                   && !x.IsTravelAllowanceConfigured.Value &&
                   !((x.ServiceDate > endDate) || (x.ServiceDate < startDate))
                   && x.FactoryTown == townId).ToListAsync();
        }

        public async Task<bool> CheckTravelStartPortEndPortExist(int startPort, int factoryTown)
        {
            return await _context.EcAutTravelTariffs.
                  Where(x => x.Active.Value
                  && x.StartPort == startPort && x.TownId == factoryTown).AnyAsync();
        }

        public async Task<int> AddTravelTariff(EcAutTravelTariff entity)
        {
            _context.EcAutTravelTariffs.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity.Id;
            else
                return 0;
        }

        public async Task<EcAutTravelTariff> GetTravelTariff(int id)
        {
            return await _context.EcAutTravelTariffs.
                   Where(x => x.Active.Value && x.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<EcAutTravelTariff> GetTravelTariffList()
        {
            return _context.EcAutTravelTariffs.Where(x => x.Active.Value);
        }

        public async Task<List<CommonDataSource>> GetStartPotList()
        {
            return await _context.EcAutRefStartPorts.
                          Where(x => x.Active.Value)
                         .Select(x => new CommonDataSource { Id = x.Id, Name = x.StartPortName })
                         .ToListAsync();
        }

        public IQueryable<EcAutTravelTariff> GetTravelTariffData(int id)
        {
            return _context.EcAutTravelTariffs.Where(x => x.Active.Value && x.Id == id);
        }
    }
}
