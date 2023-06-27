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
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class CustomerCollectionManager : ICustomerCollectionManager
    {
        private readonly ICustomerCollectionRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private ITenantProvider _filterService = null;

        public CustomerCollectionManager(ICustomerCollectionRepository repo, IAPIUserContext applicationContextService, ITenantProvider filterService)
        {
            _repo = repo;
            _ApplicationContext = applicationContextService;
            _filterService = filterService;
        }

        //get customer collection list with customer id
        public async Task<CustomerCollectionDetails> GetCustomerCollectionList(CustomerCollectionListSummary request)
        {
            if (request == null)
                return new CustomerCollectionDetails() { Result = CustomerCollectionListResult.RequestNotCorrectFormat };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.PageSize == null || request.PageSize.Value <= 0)
                request.PageSize = 10;

            int skip = (request.Index.Value - 1) * request.PageSize.Value;

            int take = request.PageSize.Value;

            var customerCollectionList =  _repo.GetCustomerCollectionDetail(request.Id);
            var result = new CustomerCollectionDetails();
            var customerCollections = new List<CustomerCollection>();

            result.Id = customerCollectionList.Select(x => (int)x.CustomerId).FirstOrDefault();

            if (customerCollectionList.Any())
            {
                var collectionList = await customerCollectionList.Skip(skip).Take(take).ToListAsync();
                var count = await customerCollectionList.CountAsync();

                foreach (var item in collectionList)
                {
                    var cusCollection = new CustomerCollection() { Id = item.Id, Name = item.Name };
                        customerCollections.Add(cusCollection);
                }
                result.CustomerCollectionList = customerCollections;

                return new CustomerCollectionDetails()
                {
                    Result = CustomerCollectionListResult.Success,
                    TotalCount = count,
                    Index = request.Index.Value,
                    PageSize = request.PageSize.Value,
                    PageCount = (count / request.PageSize.Value) + (count % request.PageSize.Value > 0 ? 1 : 0),
                    CustomerCollectionList = customerCollections,
                    Id = request.Id
                };
            }
            return new CustomerCollectionDetails()
            {
                Result = CustomerCollectionListResult.NotFound,
            };
        }

        //save the customer collection, and remove the deleted one
        public async Task<SaveCustomerResponse> Save(CustomerCollectionDetails request)
        {
            var response = new SaveCustomerResponse();

            // duplicates data
            List<string> duplicates = request.CustomerCollectionList.GroupBy(s => s.Name.ToUpper()).Where(g => g.Count() > 1).Select(g => g.Key).Distinct().ToList();
            if (duplicates != null && duplicates.Any())
            {
                List<ErrorData> errorDataList = new List<ErrorData>();
                var errorData = new ErrorData()
                {
                    Name = "CustomerCollection",
                    ErrorText = string.Join(", ", duplicates)
                };
                errorDataList.Add(errorData);

                return new SaveCustomerResponse
                {
                    ErrorList = errorDataList,
                    Result = SaveCustomerResult.CustomerExists
                };
            }

            var entity = await _repo.GetCustomerCollectionDetails(request.Id);

            if (entity == null)
                return new SaveCustomerResponse { Result = SaveCustomerResult.CustomerIsNotFound };

            //remove the deleted records
            if(request.RemoveIds != null && request.RemoveIds.Any())
            {
                var collectionList = await _repo.RemoveCustomerCollectionDetail(request.RemoveIds);

                foreach (var item in collectionList)
                {
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.DeletedOn = DateTime.Now;
                    item.Active = false;

                    await _repo.EditCustomerCollection(item);
                }
            }

            if (request.CustomerCollectionList != null && request.CustomerCollectionList.Any())
            {
                //add new collection records
                var _entityId = _filterService.GetCompanyId();
                foreach (var item in request.CustomerCollectionList.Where(x => x.Id <= 0))
                {
                    await _repo.AddCustomerCollection(new CuCollection
                    {
                        CustomerId = request.Id,
                        Id = item.Id,
                        Name = item.Name?.Trim(),
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId=_entityId
                    });
                }

                //update the existing record
                foreach (var item in request.CustomerCollectionList.Where(x => x.Id > 0))
                {
                    var collectionRecord = entity.CuCollections.FirstOrDefault(x => x.Id == item.Id);

                    if (collectionRecord != null)
                    {
                        collectionRecord.Id = item.Id;
                        collectionRecord.CustomerId = request.Id;
                        collectionRecord.Name = item.Name;
                        collectionRecord.UpdatedBy = _ApplicationContext.UserId;
                        collectionRecord.UpdatedOn = DateTime.Now;
                    }
                    await _repo.EditCustomerCollection(collectionRecord);
                }
            }
            response.Result = SaveCustomerResult.Success;
            return response;
        }

        //get collection by customer id and filter apply collection name
        public async Task<DataSourceResponse> GetCollectionDataSource(CommonCustomerSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetCollectionDataSource(request.CustomerId);

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                //filter the selected collection ids
                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                var collectionList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

                if (collectionList == null || !collectionList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = collectionList;
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
        /// get collection name list by collection ids
        /// </summary>
        /// <param name="collectionIdList"></param>
        /// <returns></returns>
        public async Task<List<string>> GetCollectionNameByCollectionIds(IEnumerable<int> collectionIdList)
        {
            return await _repo.GetCollectionNameByCollectionIds(collectionIdList);
        }
    }
}
