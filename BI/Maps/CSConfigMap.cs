using DTO.Common;
using DTO.Customer;
using DTO.References;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class CSConfigMap : ApiCommonData
    {

        public SaveOneCSConfig SaveMapCSConfigEntity(int? customerId, int userId, int locationId, int? serviceId, int? productCategoryId)
        {
            return new SaveOneCSConfig
            {
                CustomerId = customerId,
                UserId = userId,
                OfficeLocationId = locationId,
                ServiceId = serviceId,
                ProductCategoryId = productCategoryId
            };
        }
        public CuCsConfiguration MapCSConfigEntity(SaveOneCSConfig request)
        {
            if (request == null)
                return null;
            return new CuCsConfiguration
            {
                CustomerId = request.CustomerId,
                UserId = request.UserId,
                OfficeLocationId = request.OfficeLocationId,
                ServiceId = request.ServiceId,
                ProductCategoryId = request.ProductCategoryId,
                Active = true
            };
        }
        public CSConfigItem GetCustomerServiceConfigSearchResult(CuCsConfiguration entity)
        {
            if (entity == null)
                return null;
            return new CSConfigItem()
            {
                CusServiceConfigId = entity.Id,
                CustomerName = entity?.Customer?.CustomerName,
                CustomerService = entity?.User?.PersonName,
                ProductCatgory = entity?.ProductCategory?.Name,
                Service = entity?.Service?.Name,
                Office = entity?.OfficeLocation?.LocationName,
            };
        }
        public CSAllocation GetCSAllocationSearchResult(CSAllocationData cSAllocation, IEnumerable<CSAllocationCommonDataSource> brands, IEnumerable<CSAllocationCommonDataSource> services,
            IEnumerable<CSAllocationCommonDataSource> departments, IEnumerable<CSAllocationCommonDataSource> offices,
            IEnumerable<CSAllocationCommonDataSource> productCategories, List<CSAllocationCommonDataSource> factoryCountries)
        {
            if (cSAllocation == null)
                return null;
            return new CSAllocation()
            {
                CustomerName = cSAllocation.CustomerId.HasValue ? cSAllocation.CustomerName : "",
                StaffName = cSAllocation.StaffName,
                Departments = string.Join(",", departments.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Services = string.Join(",", services.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                ProductCategory = string.Join(",", productCategories.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Brands = string.Join(",", brands.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Offices = string.Join(",", offices.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Id = cSAllocation.Id,
                BackupCS = cSAllocation.BackupCS,
                BackupReportChecker = cSAllocation.BackupReportChecker,
                FactoryCountries = string.Join(",", factoryCountries.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(y => y.Name).ToList())
            };
        }

        public SaveCSAllocation GetCSAllocationMap(DaUserCustomer entity, HrStaff staff, IEnumerable<int> brandIds, IEnumerable<int> serviceIds, IEnumerable<int> departmentIds, IEnumerable<int> officeIds,
            IEnumerable<int> productCategoryIds, IEnumerable<int> notificationIds, IEnumerable<int> factoryCountryIds)
        {
            if (entity == null)
                return null;
            return new SaveCSAllocation()
            {
                CustomerId = entity.CustomerId.Value,
                BrandIds = brandIds,
                DepartmentIds = departmentIds,
                OfficeIds = officeIds,
                ServiceIds = serviceIds,
                ProductCategoryIds = productCategoryIds,
                FactoryCountryIds = factoryCountryIds,
                Staffs = new List<SaveSelectdStaff>()
                {
                    new SaveSelectdStaff()
                    {
                        Id=entity.UserId,
                        Name=staff.PersonName,
                        PrimaryCS=entity.PrimaryCs,
                        PrimaryReportChecker=entity.PrimaryReportChecker,
                        Notification=notificationIds,
                        Profile=entity.UserType
                    }
                }
            };
        }
        public SaveOneCSConfig GetCSConfigSummaryDetails(CuCsConfiguration entity)
        {
            if (entity == null)
                return null;
            var item = new SaveOneCSConfig
            {
                Id = entity.Id,
                OfficeLocationId = entity.OfficeLocationId,
                ProductCategoryId = entity.ProductCategoryId,
                CustomerId = entity.CustomerId,
                ServiceId = entity.ServiceId,
                UserId = entity.UserId
            };
            return item;
        }
        public void UpdateCSConfigEnity(CuCsConfiguration entity, SaveCSConfig request)
        {
            entity.Id = request.Id;
            entity.UserId = request.UserId;
            entity.OfficeLocationId = request.OfficeLocationId.First();
            entity.CustomerId = request.CustomerId.Count() > 0 ? request.CustomerId.First() : null;
            entity.ServiceId = request.ServiceId.Count() > 0 ? request.ServiceId.First() : null;
            entity.ProductCategoryId = request.ProductCategoryId.Count() > 0 ? request.ProductCategoryId.First() : null;
        }

        public DaUserCustomer MapDaUserCustomerEntity(int userId, int entityId, SaveSelectdStaff staff, SaveCSAllocation request, int createdBy)
        {
            return new DaUserCustomer
            {
                CustomerId = request.CustomerId,
                EntityId = entityId,
                Email = true,
                Active = true,
                UserId = userId,
                PrimaryReportChecker = staff.PrimaryReportChecker,
                PrimaryCs = staff.PrimaryCS,
                UserType = staff.Profile,
                DaUserByBrands = request.BrandIds.Select(x => MapDaUserByBrandEntity(x, entityId, createdBy)).ToList(),
                DaUserByRoles = staff.Notification.Select(x => MapDaUserByRoleEntity(x, entityId, createdBy)).ToList(),
                DaUserByProductCategories = request.ProductCategoryIds.Select(x => MapDaUserByProdCategoryEntity(x, entityId, createdBy)).ToList(),
                DaUserByServices = request.ServiceIds.Select(x => MapDaUserByServicesEntity(x, entityId, createdBy)).ToList(),
                DaUserByDepartments = request.DepartmentIds.Select(x => MapDaUserByDepartmentEntity(x, entityId, createdBy)).ToList(),
                DaUserRoleNotificationByOffices = request.OfficeIds.Select(x => MapUserRoleNotificationByOffices(x, entityId, createdBy)).ToList(),
                DaUserByFactoryCountries = request.FactoryCountryIds.Select(x => MapDaUserByFactoryCountry(x, entityId, createdBy)).ToList(),
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
            };

        }

        public DaUserCustomer MapDaUserCustomerEntity(DaUserCustomer entity, SaveDaUserCustomerItem input)
        {
            entity.CustomerId = input.CustomerId;
            entity.Email = true;
            entity.Active = true;
            entity.PrimaryReportChecker = input.PrimaryReportChecker;
            entity.PrimaryCs = input.PrimaryCS;
            entity.UserType = input.Profile;
            entity.DaUserByBrands = input.BrandIds.Select(x => MapDaUserByBrandEntity(x, input.EntityId, input.CreatedBy)).ToList();
            entity.DaUserByRoles = input.NotificationIds.Select(x => MapDaUserByRoleEntity(x, input.EntityId, input.CreatedBy)).ToList();
            entity.DaUserByProductCategories = input.ProductCategoryIds.Select(x => MapDaUserByProdCategoryEntity(x, input.EntityId, input.CreatedBy)).ToList();
            entity.DaUserByServices = input.ServiceIds.Select(x => MapDaUserByServicesEntity(x, input.EntityId, input.CreatedBy)).ToList();
            entity.DaUserByDepartments = input.DepartmentIds.Select(x => MapDaUserByDepartmentEntity(x, input.EntityId, input.CreatedBy)).ToList();
            entity.DaUserRoleNotificationByOffices = input.OfficeIds.Select(x => MapUserRoleNotificationByOffices(x, input.EntityId, input.CreatedBy)).ToList();
            entity.DaUserByFactoryCountries = input.FactoryCountryIds.Select(x => MapDaUserByFactoryCountry(x, input.EntityId, input.CreatedBy)).ToList();
            return entity;

        }

        //public CSAllocation GetCSAllocationSearchResult(DaUserCustomer entity, CuCustomer cuCustomer, List<CSAllocationCommonDataSource> brands,
        //    List<CSAllocationCommonDataSource> daUserByDepartments,
        //    List<CSAllocationCommonDataSource> productCategories,
        //    List<CSAllocationCommonDataSource> services,
        //    List<CSAllocationCommonDataSource> offices)
        //{
        //    return new CSAllocation()
        //    {
        //        Id = entity.Id,
        //        CustomerName = entity.CustomerId.HasValue ? cuCustomer.CustomerName : "",
        //        Brands = string.Join(", ", brands.Where(x => x.DaUserCustomerId == entity.Id).Select(x => x.Name).ToList()),
        //        Departments = string.Join(", ", daUserByDepartments.Where(x => x.DaUserCustomerId == entity.Id).Select(x => x.Name).ToList()),
        //        ProductCategory = string.Join(", ", productCategories.Where(x => x.DaUserCustomerId == entity.Id).Select(x => x.Name).ToList()),
        //        Offices = string.Join(", ", offices.Where(x => x.DaUserCustomerId == entity.Id).Select(x => x.Name).ToList()),
        //        Services = string.Join(", ", services.Where(x => x.DaUserCustomerId == entity.Id).Select(x => x.Name).ToList()),
        //        BackupCS = entity.PrimaryCS,
        //        BackupReportChecker = entity.PrimaryReportChecker,
        //    };
        //}

        public ExportCSAllocationData ExportCSAllocationSearchResult(CSAllocationData cSAllocation, List<CSAllocationCommonDataSource> brands,
            List<CSAllocationCommonDataSource> daUserByDepartments,
            List<CSAllocationCommonDataSource> productCategories,
            List<CSAllocationCommonDataSource> services,
            List<CSAllocationCommonDataSource> offices,
            List<CSAllocationCommonDataSource> factoryCountries)
        {
            return new ExportCSAllocationData()
            {
                CustomerName = cSAllocation.CustomerId.HasValue ? cSAllocation.CustomerName : "",
                Brands = string.Join(", ", brands.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Departments = string.Join(", ", daUserByDepartments.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                ProductCategory = string.Join(", ", productCategories.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Offices = string.Join(", ", offices.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                Services = string.Join(", ", services.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                FactoryCountry = string.Join(", ", factoryCountries.Where(x => x.DaUserCustomerId == cSAllocation.Id).Select(x => x.Name).ToList()),
                BackupCS = cSAllocation.BackupCS,
                BackupReportChecker = cSAllocation.BackupReportChecker,
            };
        }
        //public DaUserByRole MapUserByRoleEntity(int roleId)
        //{
        //    return new DaUserRoleNotificationByOffice()
        //    {
        //        OfficeId = officeId

        //    };
        //}

        public DaUserByFactoryCountry MapDaUserByFactoryCountry(int factoryCountryId, int entityId, int createdBy)
        {
            return new DaUserByFactoryCountry()
            {
                FactoryCountryId = factoryCountryId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }

        public DaUserRoleNotificationByOffice MapUserRoleNotificationByOffices(int officeId, int entityId, int createdBy)
        {
            return new DaUserRoleNotificationByOffice()
            {
                OfficeId = officeId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now

            };
        }

        public DaUserByBrand MapDaUserByBrandEntity(int branId, int entityId, int createdBy)
        {
            return new DaUserByBrand()
            {
                BrandId = branId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }
        public DaUserByRole MapDaUserByRoleEntity(int roleId, int entityId, int createdBy)
        {
            return new DaUserByRole()
            {
                RoleId = roleId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }

        public DaUserByDepartment MapDaUserByDepartmentEntity(int departmentId, int entityId, int createdBy)
        {
            return new DaUserByDepartment()
            {
                DepartmentId = departmentId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }
        public DaUserByProductCategory MapDaUserByProdCategoryEntity(int prodCategoryId, int entityId, int createdBy)
        {
            return new DaUserByProductCategory()
            {
                ProductCategoryId = prodCategoryId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }

        public DaUserByService MapDaUserByServicesEntity(int serviceId, int entityId, int createdBy)
        {
            return new DaUserByService()
            {
                ServiceId = serviceId,
                EntityId = entityId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now
            };
        }
    }
}