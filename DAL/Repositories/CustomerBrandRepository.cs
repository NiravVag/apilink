using Contracts.Repositories;
using DTO.CommonClass;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerBrandRepository : Repository, ICustomerBrandRepository
    {
        public CustomerBrandRepository(API_DBContext context) : base(context)
        {

        }

        public Task<int> AddCustomerBrand(CuBrand entity)
        {
            _context.CuBrands.Add(entity);
            return _context.SaveChangesAsync();
        }
        public Task<int> EditCustomerBrand(CuBrand entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> RemoveCustomerBrand()
        {
            return _context.SaveChangesAsync();
        }

        public Task<CuCustomer> GetCustomerBrandDetails(int? id)
        {
            return _context.CuCustomers.Where(x => x.Active.HasValue && x.Active.Value).
                Include(x => x.CuBrands).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<CuBrand>> GetCustomerBrands(int customerId)
        {
            return _context.CuBrands.
                Where(x => x.CustomerId == customerId && x.Active == true)
                .OrderBy(x => x.Name).ToListAsync();
        }

        //get actvie brand list by customer id
        public IQueryable<CommonDataSource> GetBrandDataSource(int customerId)
        {
            return _context.CuBrands.Where(x => x.Active && x.CustomerId == customerId)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name }).OrderBy(x => x.Name);
        }
        /// <summary>
        /// get brand name list by brand ids
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBrandNameByBrandId(IEnumerable<int> brandIdList)
        {
            return await _context.CuBrands.Where(x => x.Active && brandIdList.Contains(x.Id))
                                  .Select(x => x.Name).ToListAsync();
        }
    }
}
