using Contracts.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Contracts.Repositories;
using System.Threading.Tasks;
using DTO.UserConfig;
using BI.Maps;
using Entities.Enums;
using Entities;
using DTO.Common;
using DTO.CommonClass;

namespace BI
{
    public class UserConfigManager : IUserConfigManager
    {
        private readonly IUserConfigRepository _repo = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly UserConfigMap UserConfigMap = null;
        private readonly ITenantProvider _filterService = null;

        public UserConfigManager(IUserConfigRepository repo,
            IAPIUserContext applicationContext, ICustomerManager customerManager, ITenantProvider filterService)
        {
            _repo = repo;
            _applicationContext = applicationContext;
            _customerManager = customerManager;
            UserConfigMap = new UserConfigMap();
            _filterService = filterService;
        }

        //save the user config
        public async Task<UserConfigSaveResponse> Save(UserConfigSaveRequest request)
        {
            try
            {
                //empty check
                if (request == null)
                    return new UserConfigSaveResponse() { Result = ResponseResult.RequestNotCorrectFormat };

                var response = new UserConfigSaveResponse();

                var dataAccess = await _repo.GetDaUserCustomerMasterData(request.UserId);

                var entity = FrameUserAccessList(request);

                if (dataAccess == null || dataAccess.Count() == 0)
                {
                    //add the user config
                    _repo.SaveList(entity, false);

                }
                else
                {
                    //remove the existing record
                    var deleteEntity = RemoveUserAccess(dataAccess);

                    entity.AddRange(deleteEntity);

                    await _repo.Save();
                }

                //User Config any unsaved data 
                if (entity.Any(x => x.Id <= 0))
                    return new UserConfigSaveResponse
                    {
                        Result = ResponseResult.Faliure
                    };

                response.Result = ResponseResult.Success;
                response.Id = request.UserId;

                return response;
            }
            catch (Exception ex)
            {
                return new UserConfigSaveResponse() { Result = ResponseResult.Error };
                //throw ex;

            }
        }

        //frame the main table
        private DaUserCustomer FrameUserList(UserConfigSaveRequest request, int? customerId, DaUserCustomer entity)
        {
            entity.UserId = request.UserId;
            entity.CustomerId = customerId;
            entity.Email = request.EmailAccess;
            entity.UserType = request.ProfileId;
            entity.CreatedBy = _applicationContext.UserId;
            entity.EntityId = _filterService.GetCompanyId();
            return entity;
        }

        //Frame the user access DA_UserRoleNotificationByOffice list table
        private List<DaUserCustomer> FrameUserAccessList(UserConfigSaveRequest request)
        {
            var entity = new List<DaUserCustomer>();

            var createdUser = _applicationContext.UserId;

            if (request.userAccessList != null)
            {
                foreach (var userAccess in request.userAccessList)
                {
                    var entityMain = new DaUserCustomer();
                    entityMain = FrameUserList(request, userAccess.CustomerId, entityMain);

                    if (userAccess.OfficeIdAccessList != null && userAccess.OfficeIdAccessList.Any())
                    {
                        foreach(var officeId in userAccess.OfficeIdAccessList)
                        {
                            var officeItem = new DaUserRoleNotificationByOffice()
                            {
                                OfficeId = officeId,
                                CreatedBy = _applicationContext.UserId,
                                EntityId = _filterService.GetCompanyId()
                        };

                            entityMain.DaUserRoleNotificationByOffices.Add(officeItem);
                            _repo.AddEntity(officeItem);
                        }
                    }

                    if (userAccess.ServiceIdAccessList != null && userAccess.ServiceIdAccessList.Any())
                    {
                        foreach (var serviceId in userAccess.ServiceIdAccessList)
                        {
                            var itemService = new DaUserByService()
                            {
                                ServiceId = serviceId,
                                CreatedBy = _applicationContext.UserId,
                                EntityId = _filterService.GetCompanyId()
                            };

                            entityMain.DaUserByServices.Add(itemService);
                            _repo.AddEntity(itemService);
                        }
                    }

                    if (userAccess.ProductCategoryIdAccessList != null && userAccess.ProductCategoryIdAccessList.Any())
                    {
                        foreach (var productCategoryId in userAccess.ProductCategoryIdAccessList)
                        {
                            var itemProductCategory = new DaUserByProductCategory()
                            {
                                ProductCategoryId = productCategoryId,
                                CreatedBy = _applicationContext.UserId,
                                EntityId = _filterService.GetCompanyId()
                            };

                            entityMain.DaUserByProductCategories.Add(itemProductCategory);
                            _repo.AddEntity(itemProductCategory);
                        }
                    }

                    if (userAccess.RoleIdAccessList != null && userAccess.RoleIdAccessList.Any())
                    {
                        foreach (var roleId in userAccess.RoleIdAccessList)
                        {
                            var roleItem = new DaUserByRole()
                            {
                                RoleId = roleId,
                                CreatedBy = _applicationContext.UserId,
                                EntityId = _filterService.GetCompanyId()
                            };

                            entityMain.DaUserByRoles.Add(roleItem);
                            _repo.AddEntity(roleItem);
                        }
                    }

                    if (userAccess.CusDepartmentIdAccessList != null && userAccess.CusDepartmentIdAccessList.Any())
                    {
                        foreach (var departmentId in userAccess.CusDepartmentIdAccessList)
                        {
                            var departmentItem = new DaUserByDepartment()
                            {
                                DepartmentId = departmentId,
                                CreatedBy = _applicationContext.UserId,
                                EntityId = _filterService.GetCompanyId()
                            };

                            entityMain.DaUserByDepartments.Add(departmentItem);
                            _repo.AddEntity(departmentItem);
                        }
                    }

                    if (userAccess.CusBrandIdAccessList != null && userAccess.CusBrandIdAccessList.Any())
                    {
                        foreach (var brandId in userAccess.CusBrandIdAccessList)
                        {
                            var brandItem = new DaUserByBrand()
                            {
                                BrandId = brandId,
                                CreatedBy = _applicationContext.UserId,
                                EntityId = _filterService.GetCompanyId()
                            };

                            entityMain.DaUserByBrands.Add(brandItem);
                            _repo.AddEntity(brandItem);
                        }
                    }

                    //if (userAccess.CusBuyerIdAccessList != null && userAccess.CusBuyerIdAccessList.Any())
                    //{
                    //    foreach (var buyerId in userAccess.CusBuyerIdAccessList)
                    //    {
                    //        var buyerItem = new DaUserByBuyer()
                    //        {
                    //            BuyerId = buyerId,
                    //            CreatedBy = _applicationContext.UserId
                    //        };

                    //        entityMain.DaUserByBuyers.Add(buyerItem);
                    //        _repo.AddEntity(buyerItem);
                    //    }
                    //}

                    entity.Add(entityMain);
                    _repo.AddEntity(entityMain);
                }
            }
            return entity;
        }

        // Remove the data access if exist in the db for same user
        private IEnumerable<DaUserCustomer> RemoveUserAccess(IEnumerable<DaUserCustomer> dataAccess)
        {
           

            foreach (var access in dataAccess)
            {
                _repo.RemoveEntities(access.DaUserRoleNotificationByOffices);

                _repo.RemoveEntities(access.DaUserByProductCategories);

                _repo.RemoveEntities(access.DaUserByRoles);

                _repo.RemoveEntities(access.DaUserByServices);

                _repo.RemoveEntities(access.DaUserByDepartments);

                //_repo.RemoveEntities(access.DaUserByBuyers);

                _repo.RemoveEntities(access.DaUserByBrands);
            }

             _repo.RemoveEntities(dataAccess);

            return dataAccess;
        }

        //edit the values by userid
        public async Task<UserConfigEditResponse> Edit(int userId)
        {
            try
            {
                var response = new UserConfigEditResponse();

                //get saved user data values
                var dataAccess = await _repo.GetDaUserCustomerMasterData(userId);

                if (dataAccess != null && dataAccess.Count() > 0)
                {
                    response.data = UserConfigMap.EditUserAccessMap(dataAccess?.FirstOrDefault());

                    /* get list of customer ids. note: here we don't need nullable customer id 
                     * so we are converting as non nullable. because we are passing the list of customer id to db
                     */
                    var customerIdList = dataAccess.Where(x => x.CustomerId > 0).Select(x => (int)x.CustomerId).ToList();

                    //get dept details
                    var deptListResponse = await _customerManager.GetCustomerDepartments(customerIdList);
                    
                    //get brand details
                    var brandListResponse = await _customerManager.GetCustomerBrands(customerIdList);

                    //if dept success assign the dept list
                    var deptList = deptListResponse.Result == DataSourceResult.Success ? deptListResponse.DataSourceList : Enumerable.Empty<ParentDataSource>();
                    
                    //if brand success assign the brand list
                    var brandList = brandListResponse.Result == DataSourceResult.Success ? brandListResponse.DataSourceList: Enumerable.Empty<ParentDataSource>();

                    response.data.userAccessList = new List<UserAccessMasterData>();
                    foreach (var item in dataAccess)
                    {
                        response.data.userAccessList.Add(UserConfigMap.EditUserAccessMasterDataMap(item, deptList, brandList));

                        response.Result = ResponseResult.Success;
                    }
                }
                else
                    response.Result = ResponseResult.NotFound;
                return response;
            }
            catch (Exception ex)
            {
                return new UserConfigEditResponse() { Result = ResponseResult.Error };
                //throw ex;
            }
        }
    }
}
