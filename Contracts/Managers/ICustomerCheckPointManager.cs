using DTO.Customer;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ICustomerCheckPointManager
    {
        /// <summary>
        /// Delete Customer Check Point
        /// </summary>
        /// <param name="deleteRequest"></param>
        /// <returns>delete response</returns>
        Task<CustomerCheckPointDeleteResponse> Delete(int deleteRequest);
        /// <summary>
        /// get all check point types from master table
        /// </summary>
        /// <returns>List of check points</returns>
        Task<CheckPointResponse> GetCheckPointType();
        /// <summary>
        /// load the customer check point records in table based on customer and service id
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="serviceId"></param>
        /// <returns>returns customer check point data  based on customer and service id </returns>
        Task<CustomerCheckPointGetResponse> GetCustomerCheckPointSummary(int? cusId, int? serviceId);
        /// <summary>
        /// save customer check points
        /// </summary>
        /// <param name="request"></param>
        /// <returns>insert response</returns>
        Task<CustomerCheckPointSaveResponse> Save(CustomerCheckPointSaveRequest request);
        /// <summary>
        /// get checkpoint list based on customer list , service id, checkpoint list
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <param name="serviceId"></param>
        /// <param name="checkPointIdList"></param>
        /// <returns> checkpoint list stasify the condition</returns>
        Task<List<CuCheckPoint>> GetCheckPointList(IEnumerable<int> customerIdList, int serviceId, IEnumerable<int> checkPointIdList);

        /// <summary>
        /// Get the customer checkpoint by customer Id
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <returns></returns>
        Task<List<CuCheckPoint>> GetCheckPointListByCustomer(List<int> customerIdList, int serviceId);

        /// <summary>
        /// get the checkpoint brand data
        /// </summary>
        /// <param name="checkPointIdList"></param>
        /// <returns></returns>
        Task<List<CommonCheckPointDataSource>> GetCheckPointBrandList(List<int> checkPointIdList);

        /// <summary>
        ///  get the checkpoint dept data
        /// </summary>
        /// <param name="checkPointIdList"></param>
        /// <returns></returns>
        Task<List<CommonCheckPointDataSource>> GetCheckPointDeptList(List<int> checkPointIdList);

        /// <summary>
        ///  get the checkpoint service Type data
        /// </summary>
        /// <param name="checkPointIdList"></param>
        /// <returns></returns>
        Task<List<CommonCheckPointServiceTypeDataSource>> GetCheckPointServiceTypeList(List<int> checkPointIdList);

        Task<CustomerCheckPoint> GetCustomerCheckpoint(int customerId, int serviceId, int checkpointTypeId);

        /// <summary>
        /// Get the customer check point list
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        Task<CommonCheckPointDataSourceResponse> GetCustomerCheckPointList(int customerId, int serviceId);
    }
}
