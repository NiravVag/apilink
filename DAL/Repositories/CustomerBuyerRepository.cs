using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerBuyerRepository : Repository, ICustomerBuyerRepository
    {
        public CustomerBuyerRepository(API_DBContext context) : base(context)
        {

        }
        //
        public Task<List<CuBuyer>> GetCustomerBuyers(int customerID)
        {
            return _context.CuBuyers.
                Include(x => x.CuBuyerApiServices).
                Where(x => x.CustomerId == customerID && x.Active == true)
                .OrderBy(x => x.Name).ToListAsync();
        }

        //
        public Task<int> AddCustomerBuyer(CuBuyer entity)
        {
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }

        //
        public Task<int> EditCustomerBuyer(CuBuyer entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();

        }

        //
        public CuBuyer GetCustomerBuyerByID(int? buyerID)
        {
            return _context.CuBuyers.
                Include(x => x.CuBuyerApiServices).
                Where(x => x.Id == buyerID)
                .SingleOrDefault();
        }

        public async Task<bool> RemoveCustomerBuyer(int id)
        {
            var entity = await _context.CuBuyers.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }
        //get actvie buyer list by customer id
        public IQueryable<CommonDataSource> GetBuyerDataSource(int customerId)
        {
            return _context.CuBuyers.Where(x => x.Active && x.CustomerId == customerId)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name });
        }

        /// <summary>
        /// Get the Buyer IQueryable Data
        /// </summary>
        /// <returns></returns>
        public IQueryable<CuBuyer> GetBuyerDataSource()
        {
            return _context.CuBuyers.Where(x => x.Active).OrderBy(x=>x.Name);
        }

        /// <summary>
        /// get buyer name list by buyer ids
        /// </summary>
        /// <param name="buyerIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBuyerNameByBuyerIds(IEnumerable<int> buyerIdList)
        {
            return await _context.CuBuyers.Where(x => x.Active && buyerIdList.Contains(x.Id))
                                  .Select(x => x.Name).ToListAsync();
        }
    }
}
