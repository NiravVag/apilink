using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Contracts.Managers
{
    public interface ICustomerBuyerManager
    {
        
        /// <summary>
        /// SaveCustomerBuyers
        /// </summary>
        /// <param name="buyer"></param>
        /// <returns></returns>
        Task<SaveCustomerBuyerResponse> Save(SaveCustomerBuyerRequest request);
        Task<CustomerBuyerResponse> GetCustomerBuyers(int customerID);
        Task<CustomerBuyerDeleteResponse> DeleteCustomerBuyer(int customerID);
        /// <summary>
        /// get buyer by customer id and filter apply buyer name
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetBuyerDataSource(CommonCustomerSourceRequest request);

        Task<DataSourceResponse> GetBuyerDataSourceList(BuyerDataSourceRequest request);

        Task<List<string>> GetBuyerNameByBuyerIds(IEnumerable<int> buyerIdList);
    }
}
