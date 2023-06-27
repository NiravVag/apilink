using DTO.CommonClass;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerBuyerRepository
    {
        Task<List<CuBuyer>> GetCustomerBuyers(int customerID);
        CuBuyer GetCustomerBuyerByID(int? buyerId);
        Task<int> AddCustomerBuyer(CuBuyer entity);
        Task<int> EditCustomerBuyer(CuBuyer entity);
        Task<bool> RemoveCustomerBuyer(int id);
        /// <summary>
        /// get active buyer list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IQueryable<CommonDataSource> GetBuyerDataSource(int customerId);
        IQueryable<CuBuyer> GetBuyerDataSource();
        Task<List<string>> GetBuyerNameByBuyerIds(IEnumerable<int> buyerIdList);
    }
}
