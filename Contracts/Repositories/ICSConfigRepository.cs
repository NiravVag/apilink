using DTO.Customer;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ICSConfigRepository : IRepository
    {

        int SaveNewCSConfig(CuCsConfiguration entity);

        Task<List<HrStaff>> GetAllCusService();

        IEnumerable<CuCsConfiguration> GetAllCSConfig();

        Task<CuCsConfiguration> GetCSConfigDetails(int id);


        Task<CuCsConfiguration> GetCSConfigDetails(int? id);

        Task<int> EditCSConfig(CuCsConfiguration entity);

        Task<List<CuCsConfiguration>> GetCSConfigDetailsByLocationId(int staffid, IEnumerable<int> lstlocation);

        Task<List<CSAllocationCommonDataSource>> GetDaUserByBrands(IEnumerable<int> daUserCustomerIds);
        Task<List<CSAllocationCommonDataSource>> GetDaUserByDepartments(IEnumerable<int> daUserCustomerIds);
        Task<List<CSAllocationCommonDataSource>> GetDaUserByProductCategories(IEnumerable<int> daUserCustomerIds);
        Task<List<CSAllocationCommonDataSource>> GetDaUserByRoles(IEnumerable<int> daUserCustomerIds);
        Task<List<CSAllocationCommonDataSource>> GetDaUserByServices(IEnumerable<int> daUserCustomerIds);
        Task<List<CSAllocationCommonDataSource>> GetDaUserRoleNotificationByOffices(IEnumerable<int> daUserCustomerIds);


        Task<List<DaUserByBrand>> GetDaUserByBrandByIds(IEnumerable<int> ids);
        Task<List<DaUserByRole>> GetDaUserByRoleByIds(IEnumerable<int> ids);
        Task<List<DaUserByDepartment>> GetDaUserByDepartmentByIds(IEnumerable<int> ids);
        Task<List<DaUserByProductCategory>> GetDaUserByProductCategoryByIds(IEnumerable<int> ids);
        Task<List<DaUserByService>> GetDaUserByServiceByIds(IEnumerable<int> ids);
        Task<List<DaUserRoleNotificationByOffice>> GetDaUserRoleNotificationByOfficeByIds(IEnumerable<int> ids);

        IQueryable<DaUserCustomer> GetAllCSAllocations();
        Task<int> SaveCSAllocation(DaUserCustomer entity);
        Task<IEnumerable<DaUserCustomer>> GetDaUserCustomerByIds(IEnumerable<int> ids);
        Task<List<DaUserCustomer>> GetDaUserCustomerByUserIds(IEnumerable<int> userids);

        Task<List<DaUserByFactoryCountry>> GetDaUserByFactoryCountries(IEnumerable<int> daUserCustomerIds);

        Task<List<CSAllocationCommonDataSource>> GetDaUserByFactoryCountry(IEnumerable<int> daUserCustomerIds);
    }
}
