using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerBuyerManager : ICustomerBuyerManager
    {
        private readonly ICustomerRepository _customerRepo = null;

        private readonly ICustomerBuyerRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly CustomerMap _cusmap = null;
        private ITenantProvider _filterService = null;
        public CustomerBuyerManager(ICustomerBuyerRepository customerbuyerrepo, ICustomerRepository customerRepo, IAPIUserContext applicationContextService, ITenantProvider filterService)
        {
            _repo = customerbuyerrepo;
            _customerRepo = customerRepo;
            _ApplicationContext = applicationContextService;
            _cusmap = new CustomerMap();
            _filterService = filterService;
        }

        public async Task<CustomerBuyerResponse> GetCustomerBuyers(int customerID)
        {

            var response = new CustomerBuyerResponse();
            var CustomerBuyers = await _repo.GetCustomerBuyers(customerID);
            var CustomerItemList = await _customerRepo.GetCustomersItems();
            if (CustomerItemList == null)
            {
                response.Result = CustomerBuyerResult.CannotGetCustomer;
                return response;
            }
            if (CustomerBuyers == null)
            {
                response.Result = CustomerBuyerResult.CannotGetBuyer;
                return response;
            }

            response.CustomerBuyerList = CustomerBuyers.Select(x => _cusmap.MapCustomerBuyerEntity(x)).ToArray();
            response.CustomerList = CustomerItemList.Select(x => _cusmap.GetCustomerItem(x, "")).ToArray();
            response.isEdit = true;
            response.Result = CustomerBuyerResult.Success;
            return response;
        }

        public async Task<SaveCustomerBuyerResponse> Save(SaveCustomerBuyerRequest request)
        {
            var response = new SaveCustomerBuyerResponse();
            var buyerList = request.buyerList;

            if (buyerList != null && buyerList.Count != 0)
            {
                // duplicates data
                List<string> duplicates = buyerList.GroupBy(s => s.Name.ToUpper()).Where(g => g.Count() > 1).Select(g => g.Key).Distinct().ToList();
                if (duplicates != null && duplicates.Any())
                {
                    var errorData = new ErrorData()
                    {
                        Name = "CustomerBuyer",
                        ErrorText = string.Join(", ", duplicates)
                    };

                    return new SaveCustomerBuyerResponse
                    {
                        ErrorData = errorData,
                        Result = SaveCustomerBuyerResult.CustomerBuyerExists
                    };
                }

                var _entityId = _filterService.GetCompanyId();
                foreach (var buyer in buyerList)
                {
                    if (buyer.Id == 0)
                    {
                        CuBuyer entity = new CuBuyer();

                        entity.Id = buyer.Id;
                        entity.Name = buyer.Name?.Trim();
                        entity.Code = buyer.Code.Trim();
                        entity.CustomerId = request.customerValue;
                        entity.Active = true;
                        entity.CreatedBy = _ApplicationContext.UserId;
                        entity.CreatedOn = DateTime.Now;
                        entity.EntityId = _entityId;
                        if (entity == null)
                            return new SaveCustomerBuyerResponse { Result = SaveCustomerBuyerResult.CustomerBuyerIsNotFound };

                        AddBuyerApiServices(entity, buyer);

                        await _repo.AddCustomerBuyer(entity);

                        response.Id = entity.Id;

                        if (response.Id == 0)
                            return new SaveCustomerBuyerResponse { Result = SaveCustomerBuyerResult.CustomerBuyerIsNotSaved };

                        response.Result = SaveCustomerBuyerResult.Success;

                    }
                    else
                    {
                        var entity = _repo.GetCustomerBuyerByID(buyer.Id);

                        if (entity == null)
                            return new SaveCustomerBuyerResponse { Result = SaveCustomerBuyerResult.CustomerBuyerIsNotFound };

                        entity.Id = buyer.Id;
                        entity.Name = buyer.Name?.Trim();
                        entity.Code = buyer.Code.Trim();
                        entity.CustomerId = request.customerValue;
                        entity.UpdatedBy = _ApplicationContext.UserId;
                        entity.UpdatedOn = DateTime.Now;

                        UpdateBuyerApiServices(entity, buyer);

                        await _repo.EditCustomerBuyer(entity);
                        response.Id = entity.Id;

                        response.Result = SaveCustomerBuyerResult.Success;
                    }
                }
            }

            return response;
        }

        private void AddBuyerApiServices(CuBuyer entity, CustomerBuyers buyer)
        {
            if (buyer.apiServiceIds != null)
            {
                entity.CuBuyerApiServices = new List<CuBuyerApiService>();
                foreach (var serviceId in buyer.apiServiceIds)
                {
                    entity.CuBuyerApiServices.Add(new CuBuyerApiService
                    {
                        ServiceId = serviceId,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = entity.EntityId
                });
                }
            }
        }

        private void UpdateBuyerApiServices(CuBuyer entity, CustomerBuyers buyer)
        {
            if (entity.CuBuyerApiServices == null)
                entity.CuBuyerApiServices = new HashSet<CuBuyerApiService>();
            // find the item not exist in current list and update active
            foreach (var apiService in entity.CuBuyerApiServices.Where(x => !buyer.apiServiceIds.Contains(x.ServiceId) && x.Active))
            {
                apiService.Active = false;
                apiService.DeletedBy = _ApplicationContext.UserId;
                apiService.DeletedOn = DateTime.Now;
            }

            if (buyer.apiServiceIds != null)
            {
                // find item not exist in entity and add new item
                foreach (var apiService in buyer.apiServiceIds.Where(x => !entity.CuBuyerApiServices.Where(z => z.Active).Select(y => y.ServiceId).Contains(x)))
                {
                    entity.CuBuyerApiServices.Add(new CuBuyerApiService
                    {
                        ServiceId = apiService,
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = entity.EntityId
                    });
                }
            }
        }

        public async Task<CustomerBuyerDeleteResponse> DeleteCustomerBuyer(int id)
        {
            var customerBuyer = _repo.GetCustomerBuyerByID(id);
            customerBuyer.DeletedBy = _ApplicationContext.UserId;
            customerBuyer.DeletedOn = DateTime.Now;

            if (customerBuyer == null)
                return new CustomerBuyerDeleteResponse { Id = id, Result = CustomerBuyerDeleteResult.NotFound };

            await _repo.RemoveCustomerBuyer(id);

            return new CustomerBuyerDeleteResponse { Id = id, Result = CustomerBuyerDeleteResult.Success };

        }

        //get buyer by customer id and filter apply buyer name
        public async Task<DataSourceResponse> GetBuyerDataSource(CommonCustomerSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetBuyerDataSource(request.CustomerId);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                //filter the selected buyer ids
                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                var buyerList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

                if (buyerList == null || !buyerList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = buyerList;
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
        /// Get the buyer data source list(it is generic function but currently using only for tcf system)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetBuyerDataSourceList(BuyerDataSourceRequest request)
        {
            var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            //get the buyer iqueryable data
            var data = _repo.GetBuyerDataSource();

            //apply buyer id
            if (request.BuyerIds!=null && request.BuyerIds.Any())
            {
                data = data.Where(x => request.BuyerIds.Contains(x.Id));
            }

            //filter the data
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ServiceId > 0)
            {
                data = data.Where(x => x.CuBuyerApiServices.Any(y => y.ServiceId == request.ServiceId));
            }

            if (request.CustomerIds != null && request.CustomerIds.Any())
            {
                data = data.Where(x => request.CustomerIds.Contains(x.CustomerId));
            }

            //Customer GLCode filter
            if (request.CustomerGLCodes != null && request.CustomerGLCodes.Any())
            {
                data = data.Where(x => request.CustomerGLCodes.Contains(x.Customer.GlCode));
            }


            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        data = data.Where(x => x.CustomerId == _ApplicationContext.CustomerId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        data = data.Where(x => x.Customer.SuSupplierCustomers.Any(y=>y.SupplierId == _ApplicationContext.SupplierId));
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        data = data.Where(x => x.Customer.SuSupplierCustomers.Any(y => y.SupplierId == _ApplicationContext.FactoryId));
                        break;
                    }

            }

            //apply and execute the customer buyer list
            var customerBuyerList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            //if buyer list is not empty then assign it to datasource list
            if (customerBuyerList != null && customerBuyerList.Any())
            {
                response.DataSourceList = customerBuyerList;
                response.Result = DataSourceResult.Success;
            }
            return response;
        }
        /// <summary>
        /// get buyer name list by buyer ids
        /// </summary>
        /// <param name="buyerIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBuyerNameByBuyerIds(IEnumerable<int> buyerIdList)
        {
            return await _repo.GetBuyerNameByBuyerIds(buyerIdList);
        }
    }
}
