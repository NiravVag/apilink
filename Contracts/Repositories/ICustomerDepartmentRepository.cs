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
    public interface ICustomerDepartmentRepository
    {
        Task<List<CuDepartment>> GetCustomerDepartments(int CustomerId);
        Task<int> AddCustomerDepartment(CuDepartment entity);
        Task<int> EditCustomerDepartment(CuDepartment entity);
        CuDepartment GetCustomerDepartmentByID(int? departmentID);
        Task<bool> RemoveCustomerDepartment(int id);
        Task<List<BookingCustomerDepartment>> GetCustomerDepartmentsbyBooking(List<int> bookingIds);
        Task<List<BookingCustomerDepartment>> GetCustomerDepartmentsbyBooking(IQueryable<int> bookingIds);
        /// <summary>
        /// get active dept list by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IQueryable<CommonDataSource> GetDeptDataSource(int customerId);

        Task<List<string>> GetDeptNameByDeptIds(IEnumerable<int> deptIdList);
    }
}
