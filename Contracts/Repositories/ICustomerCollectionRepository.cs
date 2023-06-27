using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.CommonClass;
using Entities;

namespace Contracts.Repositories
{
    public interface ICustomerCollectionRepository : IRepository
    {
        /// <summary>
        /// customer collection add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> AddCustomerCollection(CuCollection entity);
        /// <summary>
        /// customer collection edit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> EditCustomerCollection(CuCollection entity);
        /// <summary>
        /// customer collection remove
        /// </summary>
        /// <returns></returns>
        Task<int> RemoveCustomerCollection();
        /// <summary>
        /// get customer collection 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CuCustomer> GetCustomerCollectionDetails(int? id);
        /// <summary>
        /// get customer collection 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<CuCollection> GetCustomerCollectionDetail(int? id);
        /// <summary>
        /// get customer collection list for remove
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<CuCollection>> RemoveCustomerCollectionDetail(List<int> ids);
        /// <summary>
        /// get active collection list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IQueryable<CommonDataSource> GetCollectionDataSource(int customerId);

        Task<List<string>> GetCollectionNameByCollectionIds(IEnumerable<int> collectionIdList);
    }
}
