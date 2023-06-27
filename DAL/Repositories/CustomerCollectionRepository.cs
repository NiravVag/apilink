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
    public class CustomerCollectionRepository: Repository, ICustomerCollectionRepository
    {
        public CustomerCollectionRepository(API_DBContext context) : base(context)
        {

        }
        //add context
        public Task<int> AddCustomerCollection(CuCollection entity)
        {
            _context.CuCollections.Add(entity);
            return _context.SaveChangesAsync();
        }

        //edit context
        public Task<int> EditCustomerCollection(CuCollection entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        //save the context
        public Task<int> RemoveCustomerCollection()
        {
            return _context.SaveChangesAsync();
        }

        //edit the collection record
        public Task<CuCustomer> GetCustomerCollectionDetails(int? id)
        {
            return _context.CuCustomers.Where(x => x.Active.Value).
                Include(x => x.CuCollections).FirstOrDefaultAsync(x => x.Id == id);
        }

        //get customer details by id
        public IQueryable<CuCollection> GetCustomerCollectionDetail(int? id)
        {
            return _context.CuCollections.Where(x => x.Active.Value && x.CustomerId == id).
                Select(x => new CuCollection
                {
                    Name = x.Name,
                    Id = x.Id,
                    CustomerId = x.CustomerId
                }).OrderByDescending(x=>x.Id);
        }

        //get the customer collection details by collection ids
        public async Task<IEnumerable<CuCollection>> RemoveCustomerCollectionDetail(List<int> ids)
        {
            return await _context.CuCollections.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        //get actvie collection list by customer id
        public IQueryable<CommonDataSource> GetCollectionDataSource(int customerId)
        {
            return _context.CuCollections.Where(x => x.Active.Value && x.CustomerId == customerId)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name });
        }

        /// <summary>
        /// get collection name list by collection ids
        /// </summary>
        /// <param name="collectionIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetCollectionNameByCollectionIds(IEnumerable<int> collectionIdList)
        {
            return await _context.CuCollections.Where(x => x.Active.Value && collectionIdList.Contains(x.Id))
                                  .Select(x => x.Name).ToListAsync();
        }
    }
}
