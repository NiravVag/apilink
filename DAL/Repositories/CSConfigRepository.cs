using Contracts.Repositories;
using DTO.Customer;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CSConfigRepository : Repository, ICSConfigRepository
    {
        public CSConfigRepository(API_DBContext context) : base(context)
        {
        }
        public Task<List<HrStaff>> GetAllCusService()
        {
            int idVal = _context.HrProfiles
                         .Where(x => x.Id == (int)HRProfile.CS).Select(x => x.Id).FirstOrDefault();

            var entity = _context.HrStaffs
                .Include(x => x.HrStaffProfiles)
                .Where(x => x.Active != null && x.Active.Value &&
                x.HrStaffProfiles.Any(y => y.ProfileId == idVal));
            return entity.ToListAsync();
        }
        public int SaveNewCSConfig(CuCsConfiguration entity)
        {
            if (!_context.CuCsConfigurations.Any(x => x.CustomerId == entity.CustomerId &&
            x.UserId == entity.UserId && x.OfficeLocationId == entity.OfficeLocationId &&
            x.ProductCategoryId == entity.ProductCategoryId &&
            x.ServiceId == entity.ServiceId &&
            x.Active))
            {
                _context.CuCsConfigurations.Add(entity);
                _context.SaveChanges();
            }
            return entity.Id;
        }
        public IEnumerable<CuCsConfiguration> GetAllCSConfig()
        {
            return _context.CuCsConfigurations
                   .Include(x => x.Customer)
                   .Include(x => x.OfficeLocation)
                   .Include(x => x.User)
                   .Include(x => x.ProductCategory)
                   .Include(x => x.Service).Where(x => x.Active);
        }
        public Task<CuCsConfiguration> GetCSConfigDetails(int id)
        {
            return _context.CuCsConfigurations.FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task<CuCsConfiguration> GetCSConfigDetails(int? id)
        {
            return _context.CuCsConfigurations
                .Where(x => x.Id == id).SingleOrDefaultAsync();
        }
        public Task<int> EditCSConfig(CuCsConfiguration entity)
        {
            if (!_context.CuCsConfigurations.Any(x => x.CustomerId == entity.CustomerId &&
         x.UserId == entity.UserId && x.OfficeLocationId == entity.OfficeLocationId &&
         x.ProductCategoryId == entity.ProductCategoryId &&
         x.ServiceId == entity.ServiceId &&
         x.Active))
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            return _context.SaveChangesAsync();
        }
        public Task<List<CuCsConfiguration>> GetCSConfigDetailsByLocationId(int staffid, IEnumerable<int> lstlocation)
        {
            return _context.CuCsConfigurations.Where(x => x.UserId == staffid && lstlocation.Contains(x.OfficeLocationId)).ToListAsync();
        }

        public async Task<List<DaUserByBrand>> GetDaUserByBrandByIds(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByBrands.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<CSAllocationCommonDataSource>> GetDaUserByBrands(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByBrands.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId) && x.BrandId.HasValue).Select(x => new CSAllocationCommonDataSource()
            {
                Id = x.BrandId.Value,
                DaUserCustomerId = x.DauserCustomerId,
                Name = x.Brand.Name
            }).AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserByDepartment>> GetDaUserByDepartmentByIds(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByDepartments.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<CSAllocationCommonDataSource>> GetDaUserByDepartments(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByDepartments.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId) && x.DepartmentId.HasValue)
                .Select(x => new CSAllocationCommonDataSource()
                {
                    Id = x.DepartmentId.Value,
                    DaUserCustomerId = x.DauserCustomerId,
                    Name = x.Department.Name
                })
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<CSAllocationCommonDataSource>> GetDaUserByProductCategories(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByProductCategories.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId) && x.ProductCategoryId.HasValue)
                .Select(x => new CSAllocationCommonDataSource()
                {
                    Id = x.ProductCategoryId.Value,
                    DaUserCustomerId = x.DauserCustomerId,
                    Name = x.ProductCategory.Name

                })
                .AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserByProductCategory>> GetDaUserByProductCategoryByIds(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByProductCategories.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserByRole>> GetDaUserByRoleByIds(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByRoles.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<CSAllocationCommonDataSource>> GetDaUserByRoles(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByRoles.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId))
                .Select(x => new CSAllocationCommonDataSource()
                {
                    Id = x.RoleId,
                    DaUserCustomerId = x.DauserCustomerId,
                    Name = x.Role.RoleName
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserByService>> GetDaUserByServiceByIds(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByServices.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<CSAllocationCommonDataSource>> GetDaUserByServices(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByServices.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId) && x.ServiceId.HasValue)
                .Select(x => new CSAllocationCommonDataSource()
                {
                    Id = x.ServiceId.Value,
                    DaUserCustomerId = x.DauserCustomerId,
                    Name = x.Service.Name
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserRoleNotificationByOffice>> GetDaUserRoleNotificationByOfficeByIds(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserRoleNotificationByOffices.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<CSAllocationCommonDataSource>> GetDaUserRoleNotificationByOffices(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserRoleNotificationByOffices.Where(x => daUserCustomerIds.Contains(x.DauserCustomerId) && x.OfficeId.HasValue)
                .Select(x => new CSAllocationCommonDataSource()
                {
                    Id = x.OfficeId.Value,
                    DaUserCustomerId = x.DauserCustomerId,
                    Name = x.Office.LocationName
                }).AsNoTracking().ToListAsync();
        }


        public async Task<List<CSAllocationCommonDataSource>> GetDaUserByFactoryCountry(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByFactoryCountries.Where(x => daUserCustomerIds.Contains(x.DaUserCustomerId))
                .Select(x => new CSAllocationCommonDataSource()
                {
                    Id = x.FactoryCountryId,
                    DaUserCustomerId = x.DaUserCustomerId,
                    Name = x.FactoryCountry.CountryName
                }).AsNoTracking().ToListAsync();
        }

        public IQueryable<DaUserCustomer> GetAllCSAllocations()
        {
            return _context.DaUserCustomers.Where(x => x.Active.HasValue && x.Active.Value && x.User.Active && x.User.Staff.Active.HasValue && x.User.Staff.Active.Value && (x.UserType == (int)HRProfile.CS || x.UserType == (int)HRProfile.ReportChecker));
        }

        public async Task<int> SaveCSAllocation(DaUserCustomer entity)
        {
            await _context.DaUserCustomers.AddAsync(entity);
            return entity.Id;
        }

        public async Task<IEnumerable<DaUserCustomer>> GetDaUserCustomerByIds(IEnumerable<int> ids)
        {
            return await _context.DaUserCustomers.Where(x => ids.Contains(x.Id)).AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserCustomer>> GetDaUserCustomerByUserIds(IEnumerable<int> Userids)
        {
            return await _context.DaUserCustomers.Where(x => Userids.Contains(x.UserId)).AsNoTracking().ToListAsync();
        }

        public async Task<List<DaUserByFactoryCountry>> GetDaUserByFactoryCountries(IEnumerable<int> daUserCustomerIds)
        {
            return await _context.DaUserByFactoryCountries.Where(x => daUserCustomerIds.Contains(x.DaUserCustomerId)).AsNoTracking().ToListAsync();
        }

    }
}
