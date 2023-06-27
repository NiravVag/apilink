using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerDepartmentManager : ICustomerDepartmentManager 
    {
        private readonly ICustomerRepository _customerRepo = null;
        private readonly ICustomerDepartmentRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly CustomerMap _customermap = null;
        private readonly ITenantProvider _filterService = null;
        public CustomerDepartmentManager(ICustomerDepartmentRepository repo,ICustomerRepository customerRepo, IAPIUserContext applicationContextService, ITenantProvider filterService)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _ApplicationContext = applicationContextService;
            _customermap = new CustomerMap();
            _filterService = filterService;
        }

        public async Task<CustomerDepartmentResponse> GetCustomerDepartments(int customerID)
        {

            var response = new CustomerDepartmentResponse();
            var CustomerDepartments = await _repo.GetCustomerDepartments(customerID);
            var CustomerItemList = await _customerRepo.GetCustomersItems();
            if (CustomerItemList == null)
            {
                response.Result = CustomerDepartmentResult.CannotGetCustomer;
                return response;
            }
            if (CustomerDepartments == null)
            {
                response.Result = CustomerDepartmentResult.CannotGetDepartment;
                return response;
            }

            response.CustomerDepartmentList = CustomerDepartments.Select(x => _customermap.MapCustomerDepartmentEntity(x)).ToArray();
            response.CustomerList = CustomerItemList.Select(x => _customermap.GetCustomerItem(x,"")).ToArray();
            response.isEdit = true;
            response.Result = CustomerDepartmentResult.Success;
            return response;
        }

        public async Task<SaveCustomerDepartmentResponse> Save(SaveCustomerDepartmentRequest request)
        {
            var response = new SaveCustomerDepartmentResponse();
            var departmentList = request.departmentList;
            if(departmentList!=null && departmentList.Count != 0)
            {
                // duplicates data
                List<string> duplicates = departmentList.GroupBy(s => s.Name.ToUpper()).Where(g => g.Count() > 1).Select(g => g.Key).Distinct().ToList();
                if (duplicates != null && duplicates.Any())
                {
                    var errorData = new ErrorData()
                    {
                        Name = "CustomerDepartment",
                        ErrorText = string.Join(", ", duplicates)
                    };

                    return new SaveCustomerDepartmentResponse
                    {
                        ErrorData = errorData,
                        Result = SaveCustomerDepartmentResult.CustomerDepartmentExists
                    };
                }

                foreach(var department in departmentList)
                {
                    if (department.Id == 0)
                    {
                        CuDepartment entity = new CuDepartment();
                        entity.Id = department.Id;
                        entity.Name = department.Name?.Trim();
                        entity.Code = department.Code?.Trim();
                        entity.CustomerId = request.customerValue;
                        entity.Active = true;
                        entity.CreatedBy = _ApplicationContext.UserId;
                        entity.CreatedOn = DateTime.Now;
                        entity.EntityId = _filterService.GetCompanyId();       

                        response.Id = await _repo.AddCustomerDepartment(entity);

                        if (response.Id == 0)
                            return new SaveCustomerDepartmentResponse { Result = SaveCustomerDepartmentResult.CustomerDepartmentIsNotSaved };

                        response.Result = SaveCustomerDepartmentResult.Success;

                        
                    }
                    else
                    {
                        var entity = _repo.GetCustomerDepartmentByID(department.Id);

                        if (entity == null)
                            return new SaveCustomerDepartmentResponse { Result = SaveCustomerDepartmentResult.CustomerDepartmentIsNotFound };

                        entity.Id = department.Id;
                        entity.Name = department.Name?.Trim();
                        entity.Code = department.Code?.Trim();
                        entity.CustomerId = request.customerValue;
                        entity.UpdatedBy = _ApplicationContext.UserId;
                        entity.UpdatedOn = DateTime.Now;
                        entity.EntityId= _filterService.GetCompanyId();
                        await _repo.EditCustomerDepartment(entity);
                        response.Id = entity.Id;

                        response.Result = SaveCustomerDepartmentResult.Success;
                    }
                }
               
            }

           

            return response;
        }

        public async Task<CustomerDepartmentDeleteResponse> DeleteCustomerDepartment(int id)
        {
            var customerDepartment = _repo.GetCustomerDepartmentByID(id);
            customerDepartment.DeletedBy = _ApplicationContext.UserId;
            customerDepartment.DeletedOn = DateTime.Now;

            if (customerDepartment == null)
                return new CustomerDepartmentDeleteResponse { Id = id, Result = CustomerDepartmentDeleteResult.NotFound };

            await _repo.RemoveCustomerDepartment(id);

            return new CustomerDepartmentDeleteResponse { Id = id, Result = CustomerDepartmentDeleteResult.Success };

        }

        //get dept list by customer id and filter by dept name substring
        public async Task<DataSourceResponse> GetDepartmentDataSource(CommonCustomerSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetDeptDataSource(request.CustomerId);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                //filter the selected department ids
                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                var deptList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

                if (deptList == null || !deptList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = deptList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// get department name list by department id list
        /// </summary>
        /// <param name="deptIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetDeptNameByDeptIds(IEnumerable<int> deptIdList)
        {
            return await _repo.GetDeptNameByDeptIds(deptIdList);
        }
    }
}
