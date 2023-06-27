using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerDepartmentRepository:Repository,ICustomerDepartmentRepository
    {
        public CustomerDepartmentRepository(API_DBContext context) : base(context)
        {
           
        }
        public Task<List<CuDepartment>> GetCustomerDepartments(int customerId)
        {
            return _context.CuDepartments.
                Where(x=>x.CustomerId==customerId && x.Active==true)
                .OrderBy(x => x.Name).ToListAsync();
        }

        public Task<int> AddCustomerDepartment(CuDepartment entity)
        {
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<int> EditCustomerDepartment(CuDepartment entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();

        }

        public CuDepartment GetCustomerDepartmentByID(int? departmentID)
        {
            return _context.CuDepartments.
                Where(x => x.Id == departmentID)
                .SingleOrDefault();
        }

        public async Task<bool> RemoveCustomerDepartment(int id)
        {
            var entity = await _context.CuDepartments.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;
            entity.Active = false;
            int numReturn = await _context.SaveChangesAsync();

            return numReturn > 0;
        }

        //Get the customer Departments
        public Task<List<BookingCustomerDepartment>> GetCustomerDepartmentsbyBooking(List<int> bookingIds)
        {
            return _context.InspTranCuDepartments.
                Where(x => bookingIds.Contains(x.InspectionId) && x.Active == true)
                .Select(x => new BookingCustomerDepartment
                {
                    Id = x.Department.Id,
                    Name = x.Department.Name,
                    BookingId = x.InspectionId,
                    DepartmentCode = x.Department.Code
                })
                .OrderBy(x => x.Name).ToListAsync();
        }

        /// <summary>
        /// Get the customer departments by booking id query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public Task<List<BookingCustomerDepartment>> GetCustomerDepartmentsbyBooking(IQueryable<int> bookingIds)
        {
            return _context.InspTranCuDepartments.
                Where(x => bookingIds.Contains(x.InspectionId) && x.Active == true)
                .Select(x => new BookingCustomerDepartment
                {
                    Id = x.Department.Id,
                    Name = x.Department.Name,
                    BookingId = x.InspectionId,
                    DepartmentCode = x.Department.Code
                })
                .OrderBy(x => x.Name).ToListAsync();
        }


        //get actvie dept list by customer id
        public IQueryable<CommonDataSource> GetDeptDataSource(int customerId)
        {
            return _context.CuDepartments.Where(x => x.Active && x.CustomerId == customerId)
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.Name });
        }

        /// <summary>
        /// get department name list by department id list
        /// </summary>
        /// <param name="deptIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetDeptNameByDeptIds(IEnumerable<int> deptIdList)
        {
            return await _context.CuDepartments.Where(x => x.Active && deptIdList.Contains(x.Id))
                                  .Select(x => x.Name).ToListAsync();
        }
    }
}
