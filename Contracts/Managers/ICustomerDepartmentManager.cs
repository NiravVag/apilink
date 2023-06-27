using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Contracts.Managers
{
    public interface ICustomerDepartmentManager
    {
        /// <summary>
        /// GetCustomerDepartments
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns><IEnumerable<CustomerItem></returns>
        Task<CustomerDepartmentResponse> GetCustomerDepartments(int customerID);
        Task<SaveCustomerDepartmentResponse> Save(SaveCustomerDepartmentRequest request);
        Task<CustomerDepartmentDeleteResponse> DeleteCustomerDepartment(int id);
        /// <summary>
        /// get department list by filter with paging
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetDepartmentDataSource(CommonCustomerSourceRequest request);

        Task<List<string>> GetDeptNameByDeptIds(IEnumerable<int> deptIdList);
    }
}
