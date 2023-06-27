using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Lab;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class LabManager : ILabManager
    {
        #region Declaration
        private readonly ILabRepository _labRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICacheManager _cache = null;
        private readonly ILogger _logger = null;
        private readonly ICustomerRepository _customerRepo = null;
        private readonly LabMap LabMap = null;
        private ITenantProvider _filterService = null;
        #endregion Declaration

        #region Constructor
        public LabManager(ILabRepository labRepo, IAPIUserContext context, ICacheManager cache, ILogger<LabManager> logger, ICustomerRepository customerRepo, ITenantProvider filterService)
        {
            _labRepo = labRepo;
            _ApplicationContext = context;
            _cache = cache;
            _logger = logger;
            _customerRepo = customerRepo;
            LabMap = new LabMap();
            _filterService = filterService;
        }
        #endregion Constructor

        #region Address Type
        public LabAddressTypeResponse GetLabAddressTypeSummary()
        {
            var response = new LabAddressTypeResponse();
            try
            {
                response.AddressTypeList = _labRepo.GetAllLabAddressType().Select(LabMap.GetLabAddressType).ToArray();

                if (response.AddressTypeList == null)
                    return new LabAddressTypeResponse { Result = LabAddressTypeResult.CannotGetLabAddressTypeList };
                response.Result = LabAddressTypeResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get all lab address type");
            }
            return response;
        }
        #endregion Address Type

        #region Details
        public LabDetailsResponse GetLabDetailsSummary()
        {
            var response = new LabDetailsResponse();
            try
            {
                response.LabList = _labRepo.GetAllLab().Select(LabMap.GetLabSelect).ToArray();

                if (response.LabList == null)
                    return new LabDetailsResponse { Result = LabDetailsResult.CannotGetLabDetailsList };
                response.Result = LabDetailsResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get all lab details");
            }
            return response;
        }

        public async Task<LabDeleteResponse> DeleteLab(int id)
        {
            var response = new LabDeleteResponse { Id = id };
            try
            {
                var lab = await _labRepo.GetLabDetailsById(id);

                if (lab == null)
                    return new LabDeleteResponse { Id = id, Result = LabDeleteResult.NotFound };

                lab.Active = false;
                _labRepo.Save(lab);
                response.Result = LabDeleteResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get all lab details");
            }
            return response;
        }

        public async Task<SaveLabResponse> Save(LabDetails request)
        {
            if (request.Id == 0)
                return await AddLab(request);

            return await UpdateLab(request);
        }



        private async Task<SaveLabResponse> AddLab(LabDetails request)
        {
            try
            {
                bool exists = _labRepo.GetAllLabDetails().Any(x => x.LabName.Trim().ToUpper() == request.LabName.Trim().ToUpper());

                if (exists)
                    return new SaveLabResponse { Result = SaveLabResult.LabExists };

                var entity = LabMap.MapLabDetailEntity(request, _filterService.GetCompanyId(), _ApplicationContext.UserId);

                var id = await _labRepo.AddLabDetails(entity);
                if (id <= 0)
                    return new SaveLabResponse { Result = SaveLabResult.LabIsNotSaved };
                return new SaveLabResponse { Result = SaveLabResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add lab details");
                return new SaveLabResponse { Result = SaveLabResult.LabIsNotSaved };
            }
        }

        private async Task<SaveLabResponse> UpdateLab(LabDetails request)
        {
            try
            {
                bool exists = _labRepo.GetAllLabDetails().Where(x => x.Id != request.Id).Any(x => x.LabName.Trim().ToUpper() == request.LabName.Trim().ToUpper());

                if (exists)
                    return new SaveLabResponse { Result = SaveLabResult.LabExists };

                var entity = await _labRepo.GetLabDetailsById(request.Id);
                if (entity == null)
                    return new SaveLabResponse { Result = SaveLabResult.LabIsNotFound };

                LabMap.UpdateLabEnity(entity, request, _labRepo);

                var s = await _labRepo.EditLabDetails(entity);
                return new SaveLabResponse { Result = SaveLabResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update lab details");
                return new SaveLabResponse { Result = SaveLabResult.LabIsNotSaved };
            }
        }

        public async Task<EditLabResponse> GetEditLabById(int? id)
        {
            var response = new EditLabResponse();
            try
            {
                // Lab
                if (id != null)
                {
                    response.LabDetails = await GetLabDetails(id.Value);

                    if (response.LabDetails == null)
                        return new EditLabResponse { Result = EditLabResult.CannotGetSelectLab };
                    response.Result = EditLabResult.Success;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get edit lab details by id");
                return new EditLabResponse { Result = EditLabResult.CannotGetSelectLab };
            }
            return response;
        }



        private async Task<LabDetails> GetLabDetails(int id)
        {
            try
            {
                var lab = await _labRepo.GetLabDetailsById(id);
                if (lab == null)
                    return null;

                return LabMap.GetLabDetails(lab);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get lab details by id");
            }
            return null;
        }

        public LabSearchResponse GetLabSearchData(LabSearchRequest request)
        {
            var response = new LabSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };
            try
            {
                var data = _labRepo.GetAllLabDetails();

                if (request.LabValues != null && request.LabValues.Any())
                {
                    var labIds = request.LabValues;
                    data = data.Where(x => labIds.Contains(x.Id));
                }

                if (request.CountryValues != null && request.CountryValues.Any())
                {
                    var countryIds = request.CountryValues;
                    data = data.Where(x => x.InspLabAddresses.Any(y => countryIds.Contains(y.CountryId)));
                }

                if (request.TypeValues != null && request.TypeValues.Any())
                {
                    var typeIds = request.TypeValues;
                    data = data.Where(x => x.TypeId != null && typeIds.Contains(x.TypeId.Value));
                }

                response.TotalCount = data.Count();

                if (response.TotalCount == 0)
                {
                    response.Result = LabSearchResult.NotFound;
                    return response;
                }

                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.Data = data.Skip(skip).Take(request.pageSize.Value).Select(x => LabMap.MapLabItem(x)).ToArray();
                response.Result = LabSearchResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get lab data search");
            }
            return response;
        }

        public LabDataList GetLabDetailsByCustomerId(int? customerId)
        {
            var response = new LabDataList();
            response.LabList = new List<LabMaster>();
            try
            {

                var data = _labRepo.GetAllLab();
                data = data.Where(x => x.InspLabCustomers.Any(y => y.CustomerId == customerId)).ToList();

                if (data != null && data.Count() > 0)
                    response.LabList = data.ToList().Select(x => LabMap.GetLabMasterData(x)).ToList();

                var customer = _customerRepo.GetCustomerByID(customerId);

                LabMaster labMaster = new LabMaster();
                labMaster.Id = -1;
                labMaster.Name = customer?.CustomerName;
                labMaster.Type = LabTypeEnum.Customer;

                response.LabList.Insert(0, labMaster);

                response.Result = LabDataResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get lab data");
            }

            return response;
        }

        public async Task<LabAddressDataList> GetLabAddressByLabId(int? labId)
        {
            var response = new LabAddressDataList();
            try
            {

                var data = _labRepo.GetLabAddressByLabId(labId);

                if (data == null || data.Count() == 0)
                    return new LabAddressDataList { Result = LabDataAddressResult.CannotGetTypeList };

                response.LabAddressList = data.ToList().Select(x => LabMap.GetLabMasterAddressData(x));

                response.Result = LabDataAddressResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get lab address data");
            }

            return response;
        }

        public async Task<LabContactsDataList> GetLabContactByLabIdAndCustomerId(int? labId, int? customerId)
        {
            var response = new LabContactsDataList();
            try
            {

                var data = _labRepo.GetLabContactByLabIdAndCustomerId(labId, customerId);

                if (data == null || data.Count() == 0)
                    return new LabContactsDataList { Result = LabDataContactsResult.CannotGetTypeList };

                response.LabContactList = data.ToList().Select(x => LabMap.GetLabMasterContactsData(x));

                response.Result = LabDataContactsResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get lab contacts data");
            }

            return response;
        }

        #endregion Details

        #region Type
        public LabTypeResponse GetLabTypeSummary()
        {
            var response = new LabTypeResponse();
            try
            {
                response.TypeList = _labRepo.GetAllLabType().Select(LabMap.GetLabType).ToArray();

                if (response.TypeList == null)
                    return new LabTypeResponse { Result = LabTypeResult.CannotGetLabTypeList };
                response.Result = LabTypeResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get all lab type");
            }
            return response;
        }
        #endregion Type

        /// <summary>
        /// Get the lab address list by lab id list
        /// </summary>
        /// <param name="labIdList"></param>
        /// <returns></returns>
        public async Task<LabAddressListResponse> GetLabAddressByLabIdList(LabAddressRequest request)
        {
            var response = new LabAddressListResponse() { Result = LabAddressResult.CannotGetTypeList };

            if (request.labIdList != null)
            {
                //get the lab query data by lablist
                var data = _labRepo.GetLabAddressByLabIdList(request.labIdList);

                //execute the lab data list
                var labDataList = await data.
                                    Select(x => new LabAddressData()
                                    {
                                        Id = x.Id,
                                        Address = x.Address,
                                        CountryId = x.CountryId,
                                        RegionalLanguage = x.RegionalLanguage,
                                        LabId = x.LabId
                                    }).AsNoTracking().ToListAsync();

                if (labDataList != null)
                {
                    response.AddressList = labDataList.Select(x => LabMap.GetLabAddressData(x)).ToList();
                    response.Result = LabAddressResult.Success;
                }
            }

            return response;
        }

        /// <summary>
        /// Get the lab contacts by lab id list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<LabContactsListResponse> GetLabContactByLabListAndCustomerId(LabContactRequest request)
        {
            var response = new LabContactsListResponse() { Result = LabDataContactsListResult.CannotGetTypeList };

            if (request != null)
            {
                //get the lab contact data by lab list and customer id
                var data = _labRepo.GetLabContactByLabIdListAndCustomerId(request);

                var contactList = await data.Select(x => new LabBaseContact()
                {
                    Id = x.Id,
                    Name = x.ContactName,
                    LabId = x.LabId
                }).AsNoTracking().ToListAsync();

                if (contactList.Any())
                {
                    response.LabContactList = contactList;
                    response.Result = LabDataContactsListResult.Success;
                }
            }


            return response;
        }

        /// <summary>
        /// Save the lab address list for the lab 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveLabAddressResponse> SaveLabAddressList(SaveLabAddressRequestData request)
        {
            var response = new SaveLabAddressResponse() { Result = SaveLabAddressResult.LabAddressIsNotSaved };
            if (request != null && request.labId > 0)
            {
                //get lab data by id
                var labEntity = await _labRepo.GetLabDetailById(request.labId);
                //if lab entity is not null then address list for the lab
                if (labEntity != null)
                {
                    foreach (var addressData in request.labAddressList)
                    {
                        //map the lab address data
                        var address = LabMap.MapLabAddressData(addressData);
                        labEntity.InspLabAddresses.Add(address);
                    }
                    //update lab with the address details
                    await _labRepo.EditLabDetails(labEntity);
                    response.Result = SaveLabAddressResult.Success;
                }
                else
                    response.Result = SaveLabAddressResult.LabAddressIsNotFound;
            }

            return response;
        }


    }
}
