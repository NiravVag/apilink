using DTO.CommonClass;
using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerCollectionManager
    {

        /// <summary>
        /// save the collection
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveCustomerResponse> Save(CustomerCollectionDetails request);

        /// <summary>
        /// get collection list edit the collection data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CustomerCollectionDetails> GetCustomerCollectionList(CustomerCollectionListSummary request);
        /// <summary>
        /// get collection list by customer id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCollectionDataSource(CommonCustomerSourceRequest request);

        Task<List<string>> GetCollectionNameByCollectionIds(IEnumerable<int> collectionIdList);
    }
}
