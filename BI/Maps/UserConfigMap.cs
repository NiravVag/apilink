using DTO.Common;
using DTO.CommonClass;
using DTO.UserConfig;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class UserConfigMap: ApiCommonData
    {

        public  UserConfigSaveRequest EditUserAccessMap(DaUserCustomer item)
        {

            return new UserConfigSaveRequest
            {
                //edit the main value assign to response
                ProfileId = item.UserType,
                EmailAccess = item.Email,
                UserId = item.UserId,
                Id = item.Id
            };
        }

        public  UserAccessMasterData EditUserAccessMasterDataMap(DaUserCustomer item, IEnumerable<ParentDataSource> deptList, IEnumerable<ParentDataSource> brandList)
        {
            UserAccessMasterData masterDataAccess = new UserAccessMasterData
            {
                CustomerId = item.CustomerId
            };

            //customer id has value we have to take department and brand list
            if (item.CustomerId > 0)
            {
                masterDataAccess.CustomerDepartmentList = deptList.Where(x => x.ParentId == item.CustomerId).Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                });

                masterDataAccess.CustomerBrandList = brandList.Where(x => x.ParentId == item.CustomerId).Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }

            //assign role access id list
            masterDataAccess.RoleIdAccessList = item.DaUserByRoles.Where(x => x.DauserCustomerId == item.Id)
                                                    .Select(x => x.RoleId).Distinct();

            //assign product category access id list
            masterDataAccess.ProductCategoryIdAccessList = item.DaUserByProductCategories.Where(x => x.DauserCustomerId == item.Id
                                                            && x.ProductCategoryId > 0).Select(x => x.ProductCategoryId).Distinct();

            //assign service access id list
            masterDataAccess.ServiceIdAccessList = item.DaUserByServices.Where(x => x.DauserCustomerId == item.Id && x.ServiceId > 0)
                                                        .Select(x => x.ServiceId).Distinct();

            //assign office access id list
            masterDataAccess.OfficeIdAccessList = item.DaUserRoleNotificationByOffices.Where(x => x.DauserCustomerId == item.Id && x.OfficeId > 0)
                                                        .Select(x => x.OfficeId).Distinct();
            
            //assign department access id list
            masterDataAccess.CusDepartmentIdAccessList = item.DaUserByDepartments.Where(x => x.DauserCustomerId == item.Id && x.DepartmentId > 0)
                                                        .Select(x => x.DepartmentId).Distinct();

            //assign brand access id list
            //masterDataAccess.CusBuyerIdAccessList = item.DaUserByBuyers.Where(x => x.DauserCustomerId == item.Id && x.BuyerId > 0)
            //                                            .Select(x => x.BuyerId).Distinct();

            //assign buyer access id list
            masterDataAccess.CusBrandIdAccessList = item.DaUserByBrands.Where(x => x.DauserCustomerId == item.Id && x.BrandId > 0)
                                                        .Select(x => x.BrandId).Distinct();

            return masterDataAccess;
        }

    }
}
