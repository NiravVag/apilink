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
    public class CustomerBrandManager : ICustomerBrandManager
    {
        private readonly ICustomerBrandRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ITenantProvider _filterService = null;

        public CustomerBrandManager(ICustomerBrandRepository repo, IAPIUserContext applicationContextService, ITenantProvider filterService)
        {
            _repo = repo;
            _ApplicationContext = applicationContextService;
            _filterService = filterService;
        }

        public async Task<CustomerBrandDetails> GetCustomerBrands(int customerId)
        {
            var result = new CustomerBrandDetails();
            var customer = await _repo.GetCustomerBrandDetails(customerId);
            if (customer == null)
            {

                return result;
            }

            var customerBrands = new List<CustomerBrand>();
            result.Id = customer.Id;
            foreach (var item in customer.CuBrands)
            {
                if (item.Active)
                {
                    var brand = new CustomerBrand() { Id = item.Id, Name = item.Name, BrandCode = item.Code };
                    customerBrands.Add(brand);
                }
            }
            result.CustomerBrands = customerBrands;
            return result;
        }

        public async Task<SaveCustomerResponse> Save(CustomerBrandDetails request)
        {
            var response = new SaveCustomerResponse();

            // duplicates data
            List<string> duplicates = request.CustomerBrands.GroupBy(s => s.Name.ToUpper()).Where(g => g.Count() > 1).Select(g => g.Key).Distinct().ToList();
            if (duplicates != null && duplicates.Any())
            {
                List<ErrorData> errorDataList = new List<ErrorData>();
                var errorData = new ErrorData()
                {
                    Name = "CustomerBrand",
                    ErrorText = string.Join(", ", duplicates)
                };
                errorDataList.Add(errorData);

                return new SaveCustomerResponse
                {
                    ErrorList = errorDataList,
                    Result = SaveCustomerResult.CustomerExists
                };
            }

            var entity = await _repo.GetCustomerBrandDetails(request.Id);

            if (entity == null)
                return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerIsNotFound };

            if (entity.CuBrands.Any() && !request.CustomerBrands.Any())
                return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerOneBrandRequired };

            // Customer brandlist
            var brandIds = request.CustomerBrands.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
            var CuBrand = entity.CuBrands.Where(x => !brandIds.Contains(x.Id));

            foreach (var item in CuBrand)
            {
                item.DeletedBy = _ApplicationContext.UserId;
                item.DeletedOn = DateTime.Now;
                item.Active = false;

                await _repo.EditCustomerBrand(item);
            }

            if (request.CustomerBrands != null)
            {
                foreach (var item in request.CustomerBrands.Where(x => x.Id <= 0))
                {
                    await _repo.AddCustomerBrand(new CuBrand
                    {
                        CustomerId = request.Id,
                        Id = item.Id,
                        Name = item.Name?.Trim(),
                        Code = item.BrandCode?.Trim(),
                        Active = true,
                        EntityId = _filterService.GetCompanyId(),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

                var lstBrandToEdit = new List<CuBrand>();
                foreach (var item in request.CustomerBrands.Where(x => x.Id > 0))
                {
                    var brand = entity.CuBrands.FirstOrDefault(x => x.Id == item.Id);

                    if (brand != null)
                    {
                        brand.Id = item.Id;
                        brand.CustomerId = request.Id;
                        brand.Code = item.BrandCode;
                        brand.Name = item.Name;
                        brand.UpdatedBy = _ApplicationContext.UserId;
                        brand.EntityId = _filterService.GetCompanyId();
                        brand.UpdatedOn = DateTime.Now;
                        lstBrandToEdit.Add(brand);
                    }

                    await _repo.EditCustomerBrand(brand);
                }

            }

            response.Result = SaveCustomerResult.Success;

            return response;
        }

        //get Customer's brand by searchtext and customer id
        public async Task<DataSourceResponse> GetBrandDataSource(CommonCustomerSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetBrandDataSource(request.CustomerId);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                //filter the selected brand Ids
                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                var brandList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

                if (brandList == null || !brandList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = brandList;
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
        /// Get brand name list by brand id list
        /// </summary>
        /// <param name="brandIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBrandNameByBrandId(IEnumerable<int> brandIdList)
        {
            return await _repo.GetBrandNameByBrandId(brandIdList);
        }
    }
}
