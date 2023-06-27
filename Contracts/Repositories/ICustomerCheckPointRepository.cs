using DTO.Customer;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICustomerCheckPointRepository : IRepository
    {
        /// <summary>
        /// get all check point types from master table
        /// </summary>
        /// <returns>List of check points</returns>
        Task<List<CuCheckPointType>> GetCheckPointType();
        /// <summary>
        /// load the customer check point records in table
        /// </summary>
        /// <returns>all customer check point data</returns>
        Task<IEnumerable<CustomerCheckPoint>> GetCustomerCheckPoint();
        /// <summary>
        /// Save customer check points
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>insert response</returns>
        Task<int> SaveCustomerCP(CuCheckPoint entity);
        /// <summary>
        /// get customer check point using id(customer check point)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>get customer check points data</returns>
        Task<CuCheckPoint> GetCustomerCPbyId(int id);
        /// <summary>
        /// Update customer check points
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>update response</returns>
        Task<int> UpdateCustomerCP(CuCheckPoint entity);
        /// <summary>
        /// record exists in customer check points table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>exists response</returns>
        Task<bool> IsRecordExists(CuCheckPoint entity);
        /// <summary>
        /// load the customer check point records in table using customer and service id
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="serviceId"></param>
        /// <returns>customer check point data returns based on customer and service id </returns>
        Task<IEnumerable<CustomerCheckPoint>> GetCusCPByCusServiceId(int? cusId, int? serviceId);
        /// <summary>
        /// load the customer check point records in table using customer id
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns>customer check point data returns based on customer id </returns>
        Task<IEnumerable<CustomerCheckPoint>> GetCusCPByCustomerId(int? cusId);
        /// <summary>
        /// get checkpoint list based on customer list , service id, checkpoint list
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <param name="serviceId"></param>
        /// <param name="checkPointIdList"></param>
        /// <returns> checkpoint list stasify the condition</returns>
        Task<List<CuCheckPoint>> GetCheckPointList(IEnumerable<int> customerIdList, int serviceId, IEnumerable<int> checkPointIdList);

        /// <summary>
        /// Fetch the customer checkpoint list
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <returns></returns>
        Task<List<CuCheckPoint>> GetCustomerCheckPointByCustomer(List<int> customerIdList, int serviceId);

        /// <summary>
        /// get checkpoint brands
        /// </summary>
        /// <param name="checkpointIdList"></param>
        /// <returns></returns>
        Task<List<CommonCheckPointDataSource>> GetCustomerCheckPointBrand(List<int> checkpointIdList);

        /// <summary>
        /// get checkpoint departments
        /// </summary>
        /// <param name="checkpointIdList"></param>
        /// <returns></returns>
        Task<List<CommonCheckPointDataSource>> GetCustomerCheckPointDept(List<int> checkpointIdList);

        /// <summary>
        /// get checkpoint service types
        /// </summary>
        /// <param name="checkpointIdList"></param>
        /// <returns></returns>
        Task<List<CommonCheckPointServiceTypeDataSource>> GetCustomerCheckPointServiceType(List<int> checkpointIdList);
        /// <summary>
        /// based in customer id and checkpoint type id check customer checkpoint configured or not
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="checkPointTypeId"></param>
        /// <returns></returns>
        Task<bool> IsCustomerCheckpointConfigured(int customerId, int checkPointTypeId);
        Task<List<CommonCheckPointDataSource>> GetCustomerCheckPointCountry(List<int> checkpointIdList);

        Task<List<int>> GetCustomerCheckPointList(int customerId, int serviceId);
        Task<CustomerCheckPoint> GetCustomerCheckpoint(int customerId, int serviceId, int checkpointTypeId);
    }
}
