using DTO.CommonClass;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerBrandManager
    {
        Task<SaveCustomerResponse> Save(CustomerBrandDetails request);
        Task<CustomerBrandDetails> GetCustomerBrands(int customerId);
        /// <summary>
        /// get brand list by filter with paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetBrandDataSource(CommonCustomerSourceRequest request);

        Task<List<string>> GetBrandNameByBrandId(IEnumerable<int> brandIdList);
    }
}
