using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.CommonClass;
using DTO.DefectDashboard;
using DTO.DataAccess;
using DTO.RepoRequest.Enum;
using DTO.MasterConfig;
using DTO.Quotation;
using DTO.Location;
using static BI.TenantProvider;

namespace BI
{
    public class SupplierManager : ApiCommonData, ISupplierManager
    {
        private readonly ISupplierRepository _repo = null;
        private readonly ILocationManager _locManager = null;
        private readonly ICustomerManager _custmanager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IUserAccountRepository _userAccountRepo = null;
        private readonly IUserRightsManager _userManager = null;
        private readonly IUserRepository _userRepository = null;
        private readonly SupplierMap SupplierMap = null;
        private readonly ITenantProvider _filterService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserConfigRepository _userConfigRepo = null;
        private readonly ILocationRepository _locationRepository = null;
        private readonly IInspectionBookingRepository _inspectionBookingRepository;

        public SupplierManager(ISupplierRepository repo, ILocationManager locManager, IUserRepository userRepository, ICustomerManager custmanager, IAPIUserContext applicationContext,
            ITenantProvider filterService, IInspectionBookingRepository inspectionBookingRepository,
            IUserAccountRepository userAccountRepo, IUserRightsManager userManager, IUserConfigRepository userConfigRepo,
            ILocationRepository locationRepository,
            ICustomerRepository customerRepository)
        {
            _repo = repo;
            _locManager = locManager;
            _custmanager = custmanager;
            _ApplicationContext = applicationContext;
            _userAccountRepo = userAccountRepo;
            _userManager = userManager;
            _userRepository = userRepository;
            _userConfigRepo = userConfigRepo;
            SupplierMap = new SupplierMap();
            _filterService = filterService;
            _customerRepository = customerRepository;
            _locationRepository = locationRepository;
            _inspectionBookingRepository = inspectionBookingRepository;
        }

        public async Task<SupplierDeleteResponse> DeleteSupplier(int id)
        {
            var supplier = await _repo.GetSupplierDetails(id);

            if (supplier == null)
                return new SupplierDeleteResponse { Id = id, Result = SupplierDeleteResult.NotFound };

            if (supplier.AudTransactionFactories.Count > 0 || supplier.AudTransactionSuppliers.Count > 0 || supplier.CuPoSuppliers.Any(x => x.Active.HasValue && x.Active.Value) ||
                    supplier.InspTransactionFactories.Count > 0 || supplier.InspTransactionSuppliers.Count > 0 || supplier.CuPoFactories.Any(x => x.Active.HasValue && x.Active.Value))
                return new SupplierDeleteResponse { Id = id, Result = SupplierDeleteResult.CannotDelete };

            if (supplier.SuAddresses.Count > 0)
                _repo.RemoveEntities(supplier.SuAddresses);

            if (supplier.SuSupplierCustomers.Count > 0)
                _repo.RemoveEntities(supplier.SuSupplierCustomers);

            if (supplier.SuSupplierCustomerContacts.Count > 0)
                _repo.RemoveEntities(supplier.SuSupplierCustomerContacts);

            if (supplier.SuSupplierFactorySuppliers.Count > 0)
                _repo.RemoveEntities(supplier.SuSupplierFactorySuppliers);

            if (supplier.SuGrades.Count > 0)
            {
                foreach (var item in supplier.SuGrades)
                {
                    item.Active = false;
                    item.UpdatedBy = _ApplicationContext.UserId;
                    item.UpdatedOn = DateTime.Now;
                }
                _repo.EditEntities(supplier.SuGrades);
            }

            if (supplier.SuApiServices.Count > 0)
            {
                foreach (var item in supplier.SuApiServices)
                {
                    item.Active = false;
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.DeletedOn = DateTime.Now;
                }
                _repo.EditEntities(supplier.SuApiServices);
            }

            if (supplier.SuEntities.Count > 0)
            {
                foreach (var item in supplier.SuEntities)
                {
                    item.Active = false;
                    item.DeletedBy = _ApplicationContext.UserId;
                    item.DeletedOn = DateTime.Now;
                }
                _repo.EditEntities(supplier.SuEntities);
            }
            if (supplier.SuContacts.Count > 0)
            {
                foreach (var item in supplier.SuContacts)
                {
                    if (item.SuContactEntityServiceMaps.Count > 0)
                        _repo.RemoveEntities(item.SuContactEntityServiceMaps);

                    item.Active = false;

                }
                _repo.EditEntities(supplier.SuContacts);
            }

            if (supplier.SuSupplierFactoryParents.Count > 0)
                _repo.RemoveEntities(supplier.SuSupplierFactoryParents);

            supplier.Active = false;
            _repo.Save(supplier);

            return new SupplierDeleteResponse { Id = id, Result = SupplierDeleteResult.Success };

        }

        public async Task<EditSupplierResponse> GetEditSupplier(int? id)
        {
            var response = new EditSupplierResponse();

            //Staff
            if (id != null)
            {
                response.SupplierDetails = await GetSupplierDetails(id.Value);

                if (response.SupplierDetails == null)
                    return new EditSupplierResponse { Result = EditSupplierResult.CannotGetSelectSupplier };

            }



            //TypeList
            var typeList = await _repo.GetTypes();

            if (typeList == null || typeList.Count == 0)
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetTypeList };

            response.TypeList = typeList.Select(SupplierMap.GetSuppType);

            //LevelList
            var levelList = await _repo.GetLevels();

            if (levelList == null || levelList.Count == 0)
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetLevelList };

            response.LevelList = levelList.Select(SupplierMap.Getlevel);

            //OwnerList
            var ownerList = await _repo.GetOwners();

            if (ownerList == null || ownerList.Count == 0)
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetOwnerList };

            response.OwnerList = ownerList.Select(SupplierMap.GetOwner);


            //Customer List
            response.CustomerList = await _custmanager.GetCustomerItems();

            //Country List 

            response.CountryList = _locManager.GetCountries();

            if (response.CountryList == null || !response.CountryList.Any())
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetCountryList };

            //Supplier List 
            //var suppList = _repo.GetSuppliers().Where(x => x.TypeId != (int)Supplier_Type.Factory).ToList();
            //if (suppList != null)
            //    response.SupplierList = suppList.Select(x => SupplierMap.MapSupplierItem(x, (int)Entities.Enums.Supplier_Level.Child));

            //AddressTypeList
            var addresTypeList = await _repo.GetAddressTypes();

            if (addresTypeList == null || addresTypeList.Count == 0)
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetAddressTypes };

            response.AddressTypeList = addresTypeList.Select(SupplierMap.GetAddressType);

            //CreditTermList
            var creditTermList = await GetCreditTerms();

            if (creditTermList == null)
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetCreditTerm };

            response.CreditTermList = creditTermList;

            //StatusList
            var statusList = await GetStatus();

            if (statusList == null)
                return new EditSupplierResponse { Result = EditSupplierResult.CannotGetStatus };

            response.StatusList = statusList;


            response.Result = EditSupplierResult.Success;

            return response;

        }


        public async Task<EditSupplierResponse> GetSupplierOrFactoryDetails(int? id)
        {
            var response = new EditSupplierResponse();

            if (id != null)
            {
                response.SupplierDetails = await GetSupplierOrFactoryDetails(id.Value);

                if (response.SupplierDetails == null)
                    return new EditSupplierResponse { Result = EditSupplierResult.CannotGetSelectSupplier };

            }

            response.Result = EditSupplierResult.Success;

            return response;

        }


        public async Task<SupplierDetails> GetSupplierDetails(int id)
        {
            var supplier = await _repo.GetSupplierDetails(id);

            if (supplier == null)
                return null;

            //get supplier graders details
            var grades = await _repo.GetSupplierGradeDetailsBySupplierId(id);

            return SupplierMap.GetSupplierDetails(supplier, grades);

        }

        private async Task<SupplierDetails> GetSupplierOrFactoryDetails(int id)
        {
            var supplier = await _repo.GetSupplierORFactoryDetails(id);

            if (supplier == null)
                return null;

            return SupplierMap.GetSupplierDetails(supplier);

        }

        /// <summary>
        /// get supplier Search Data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SupplierSearchItemResponse> GetSupplierSearchData(SupplierSearchRequestNew request)
        {
            var response = new SupplierSearchItemResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            var data = GetSupplierFilterData(request);

            response.TotalCount = await data.Select(x => x.Id).Distinct().CountAsync();

            if (response.TotalCount == 0)
            {
                response.Result = SupplierSearchResult.NotFound;
                return response;
            }

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) +
                (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);


            var pagedItems = await data.SelectMany(x => x.SuAddresses.Where(y => y.AddressTypeId == (int)SuAddressTypeEnum.Headoffice))
                .Skip(skip).Take(request.pageSize.Value).Select(sup =>
            new SupplierSearchItemRepo()
            {
                Id = sup.SupplierId,
                Name = sup.Supplier.SupplierName,
                TypeId = sup.Supplier.TypeId,
                TypeName = sup.Supplier.Type.Type,
                LocalName = sup.Supplier.LocalName,
                CountryName = sup.Country.CountryName,
                CityName = sup.City.CityName,
                RegionName = sup.Region.ProvinceName,
                RegionalLanguageName = sup.Supplier.LocalName,
                TownName = sup.Town.TownName,
                Address = sup.Address,
                CountyName = sup.County.CountyName
            }).ToListAsync();

            var pagedSupplierList = pagedItems.Select(x => x.Id);
            //var addressList = await _repo.GetSupplierAddressDataList(pagedSupplierList);

            var supplierInvolvedList = await _repo.GetSupplierInvolvedItemsCount(pagedSupplierList);

            response.Data = pagedItems.Select(x => SupplierMap.MapSupplierSearchItem(x, null, supplierInvolvedList));

            response.Result = SupplierSearchResult.Success;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IQueryable<SuSupplier> GetSupplierFilterData(SupplierSearchRequestNew request)
        {
            var data = _repo.GetSuppliersSearchData();

            if (request.SuppValues != null)
            {
                if (request.SuppValues > 0)
                {
                    data = data.Where(x => x.Id == request.SuppValues);
                }
            }

            if (request.CountryValues != null && request.CountryValues.Any())
            {
                var country = data.Select(x => x.SuAddresses.Select(y => y.CountryId));
                data = data.Where(x => x.SuAddresses.Any(y => request.CountryValues.Contains(y.CountryId)));
            }

            if (request.TypeValues != null && request.TypeValues.Any())
            {
                var typeIds = request.TypeValues.ToList();
                data = data.Where(x => x.TypeId != null && typeIds.Contains(x.TypeId.Value));
            }

            if (request.CustomerId > 0)
            {
                data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId.Value));
            }

            if (request.provinceId > 0)
            {
                data = data.Where(x => x.SuAddresses.Any(y => y.RegionId == request.provinceId.Value));
            }

            if (request.CityValues != null && request.CityValues.Any())
            {
                data = data.Where(x => x.SuAddresses.Any(y => request.CityValues.Contains(y.CityId)));
            }

            //filter by email in supplier or supplier contacts
            if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrWhiteSpace(request.Email))
            {
                data = data.Where(x => x.Email == request.Email || x.SuContacts.Any(x => x.Mail == request.Email && x.Active == true));
            }

            //filter by phone number in supplier or supplier contacts
            if (!string.IsNullOrEmpty(request.PhoneNumber) && !string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                data = data.Where(x => x.Phone == request.PhoneNumber || x.SuContacts.Any(x => x.Phone == request.PhoneNumber && x.Active == true));
            }

            if (request.IsEAQF.GetValueOrDefault())
            {
                data = data.Where(x => x.IsEaqf == request.IsEAQF.GetValueOrDefault());
            }

            return data;
        }

        public async Task<SupplierSearchItemResponse> GetSupplierSearchChildData(int id, int supplierType)
        {
            var response = new SupplierSearchItemResponse { };

            var data = _repo.GetSuppliersSearchChildData(id, supplierType);

            response.TotalCount = await data.Select(x => x.Id).CountAsync();

            if (response.TotalCount == 0)
            {
                response.Result = SupplierSearchResult.NotFound;
                return response;
            }
            var pagedItems = await data.ToListAsync();
            var supplierList = pagedItems.Select(x => x.Id);
            var addressList = await _repo.GetSupplierAddressDataList(supplierList);
            var supplierInvolvedList = await _repo.GetSupplierInvolvedItemsCount(supplierList);
            response.Data = pagedItems.Select(x => SupplierMap.MapSupplierSearchItem(x, addressList, supplierInvolvedList));
            response.Result = SupplierSearchResult.Success;
            return response;
        }

        public async Task<SupplierSummaryResponse> GetSupplierSummary()
        {
            var response = new SupplierSummaryResponse();

            // Countries
            //  response.CountryList = _locManager.GetCountries();

            //if (response.CountryList == null)
            //    return new SupplierSummaryResponse { Result = SupplierSummaryResult.CannotGetCountryList };

            //Types
            var data = await _repo.GetTypes();

            if (data == null || data.Count == 0)
                return new SupplierSummaryResponse { Result = SupplierSummaryResult.CannotGetTypeList };

            response.TypeList = data.Select(SupplierMap.GetSuppType);

            response.IsEdit = (_ApplicationContext.CustomerId == 0 && _ApplicationContext.SupplierId == 0);

            response.Result = SupplierSummaryResult.Success;

            return response;
        }

        public async Task<SaveSupplierResponse> Save(SupplierDetails request)
        {
            if (request.Id == 0)
                return await AddSupplier(request);

            return await UpdateSupplier(request);
        }

        private async Task<ErrorData> IsSupplierCodeExistsAsync(SupplierDetails request)
        {
            List<string> errorDataList = new List<string>();
            var customerIds = request.CustomerList.Select(x => x.Id).ToList();
            var supplierCustomerData = await _repo.GetSupplierCustomerData(customerIds);

            supplierCustomerData = supplierCustomerData?.Where(x => !String.IsNullOrEmpty(x.Code) && !String.IsNullOrWhiteSpace(x.Code)).ToList();

            if (request.Id > 0)
                supplierCustomerData = supplierCustomerData?.Where(x => x.SupplierId != request.Id).ToList();

            foreach (var item in request.CustomerList)
            {
                var duplicate = supplierCustomerData?.FirstOrDefault(x => x.CustomerId == item.Id && x.Code.Trim().ToUpper() == item.Code.Trim().ToUpper());
                if (duplicate != null)
                {
                    errorDataList.Add($"{duplicate.Code} code exists for {duplicate.SupplierName}");
                }
            }
            if (errorDataList.Any())
            {
                var errorData = new ErrorData()
                {
                    Name = "SupplierCode",
                    ErrorText = string.Join(", ", errorDataList)
                };
                return errorData;
            }
            else
                return null;
        }

        private async Task<SaveSupplierResponse> AddSupplier(SupplierDetails request)
        {
            int supplierId = 0;
            //check if supplier exists 

            bool exists = await _repo.IsSupplierExists(request);
            if (exists)
                return new SaveSupplierResponse { Result = SaveSupplierResult.SupplierExists };

            if (request.CustomerList != null)
            {
                var supplierCodeExists = await IsSupplierCodeExistsAsync(request);
                if (supplierCodeExists != null)
                    return new SaveSupplierResponse { ErrorData = supplierCodeExists, Result = SaveSupplierResult.SupplierCodeExists };
            }

            var entity = CreateEntity(request, request.IsNewSupplier, null);

            int id = await _repo.AddSupplier(entity);

            if (request.TypeId == (int)Supplier_Type.Factory)
                supplierId = entity.SuSupplierFactoryParents.Where(x => x.ParentId == id).Select(x => x.SupplierId).FirstOrDefault();
            else
                supplierId = id;

            if (id <= 0)
                return new SaveSupplierResponse { Result = SaveSupplierResult.SupplierIsNotSaved };

            await SendEmailToBookingTeamIfFactoryCountyOrTownNotExists(entity, request, supplierId);

            int? parentId = null;
            //if we checked the same as factory or supplier at that time we need to pass the related id
            if (request.IsNewSupplier)
            {
                //when we create supplier and check the facotry same as supplier that time we need to pick the return factory id
                if (request.TypeId == (int)Supplier_Type.Supplier_Agent)
                {
                    parentId = entity.SuSupplierFactorySuppliers.FirstOrDefault()?.ParentId;
                }
                //when we create factory and check the supplier same as factory that time we need to return the supplier id
                else if (request.TypeId == (int)Supplier_Type.Factory)
                {
                    parentId = entity.SuSupplierFactoryParents.FirstOrDefault()?.SupplierId;
                }

            }

            return new SaveSupplierResponse { Id = id, Result = SaveSupplierResult.Success, ParentId = parentId };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<SaveSupplierResponse> SendEmailToBookingTeamIfFactoryCountyOrTownNotExists(SuSupplier entity, SupplierDetails request, int supplierId)
        {

            if (request.IsFromBookingPage && entity.TypeId == (int)Supplier_Type.Factory)
            {
                var cityid = request.AddressList.Select(x => x.CityId).FirstOrDefault();

                var officeid = await _locManager.GetOfficeIdByCityId(cityid);

                //get if china address
                var factoryCountryData = entity.SuAddresses.Where(x => x.CountryId == (int)CountryEnum.China).ToList();

                bool isCounty = factoryCountryData.Any(x => x.CountyId.GetValueOrDefault() == 0);

                bool isTown = factoryCountryData.Any(x => x.TownId.GetValueOrDefault() == 0);

                //county or town values not exists in factory details
                if (isCounty || isTown)
                {
                    var userAccessFilter = new UserAccess
                    {
                        ServiceId = (int)Service.InspectionId,
                        RoleId = (int)RoleEnum.InspectionRequest,
                        OfficeId = officeid.GetValueOrDefault(),
                        //CustomerId = entity.CustomerId,
                        //ProductCategoryIds = entity.InspProductTransactions.Select(x => x.Product.ProductCategory).Distinct(),
                        //DepartmentIds = entity.InspTranCuDepartments?.Select(x => (int?)x.Id).Distinct(),
                        //BrandIds = entity.InspTranCuBrands?.Select(x => (int?)x.Id).Distinct(),
                    };

                    //get user mail list
                    var toUserList = await _userManager.GetUserListByRoleOfficeServiceCustomer(userAccessFilter);

                    if (toUserList != null && toUserList.Any())
                    {
                        var res = new SaveSupplierResponse();

                        //factory name
                        res.FactoryName = entity.SupplierName;

                        res.UserName = !string.IsNullOrWhiteSpace(_ApplicationContext.UserName) ? _ApplicationContext.UserName : string.Empty;

                        res.EmailId = !string.IsNullOrWhiteSpace(_ApplicationContext.EmailId) ? " (" + _ApplicationContext.EmailId + ")" : string.Empty;

                        res.ToEmailList = toUserList.Where(x => IsValidEmail(x.EmailAddress)).Select(x => x.EmailAddress).ToList();

                        if (isCounty && isTown)
                        {
                            res.MissingFields = Factory_Address_County_Town;
                        }
                        else if (isCounty)
                        {
                            res.MissingFields = Factory_Address_County;
                        }
                        else if (isTown)
                        {
                            res.MissingFields = Factory_Address_Town;
                        }

                        res.FactoryResult = SaveSupplierResult.FactoryCountyTownNotFound;
                        res.Result = SaveSupplierResult.Success;

                        res.Id = supplierId;
                        return res;
                    }
                }
            }

            return new SaveSupplierResponse { Id = supplierId, Result = SaveSupplierResult.Success };
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private SuSupplier CreateEntity(SupplierDetails request, bool isNew, int? typeId)
        {
            //SuSupplier
            var entity = new SuSupplier
            {
                SupplierName = request.Name?.Trim()?.ToUpper(),
                Active = true,
                Comments = request.Comment?.Trim(),
                LegalName = request.LegalName?.Trim().ToUpper(),
                Fax = request.Fax?.Trim(),
                Email = request.Email?.Trim(),
                ContactPerson = request.ContactPersonName?.Trim(),
                CreatedDate = DateTime.Now,
                GlCode = request.GlCode,
                LevelId = request.LevelId == 0 ? null : request.LevelId,
                LocalName = request.LocLanguageName?.Trim(),
                Mobile = request.Mobile?.Trim(),
                Phone = request.Phone?.Trim(),
                OwnerShipId = request.OwnerId,
                TypeId = typeId == null ? request.TypeId : typeId.Value,
                Website = request.WebSite?.Trim(),
                TotalStaff = request.TotalStaff?.Trim(),
                DailyProduction = request.DailyProduction?.Trim(),
                CreditTermId = request.CreditTerm,
                StatusId = request.Status,
                Vatno = request.VatNo?.Trim(),
                CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                CompanyId = request.CompanyId,
                IsEaqf = request.IsEAQF
            };


            //when supplier entityids is null then add the filter service entityid 
            if (request.SupplierEntityIds == null || !request.SupplierEntityIds.Any())
            {
                request.SupplierEntityIds.Add(_filterService.GetCompanyId());
            }
            //supplier entity
            if (request.SupplierEntityIds != null && request.SupplierEntityIds.Any())
            {
                foreach (var supplierEntityId in request.SupplierEntityIds)
                {
                    var suEntity = new SuEntity()
                    {
                        Active = true,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = supplierEntityId
                    };

                    entity.SuEntities.Add(suEntity);
                    _repo.AddEntity(suEntity);
                }
            }


            // AddressList
            if (request.AddressList != null)
            {
                foreach (var item in request.AddressList.Where(x => x.CountryId > 0))
                {
                    var address = new SuAddress
                    {
                        CountryId = item.CountryId,
                        CityId = item.CityId,
                        RegionId = item.RegionId,
                        ZipCode = item.ZipCode,
                        Address = item.Way?.Trim(),
                        AddressTypeId = item.AddressTypeId == 0 ? (int)Supplier_Address_Type.HeadOffice : (int?)item.AddressTypeId,
                        Longitude = item.Longitude,
                        Latitude = item.Latitude,
                        CountyId = item.CountyId,
                        TownId = item.TownId,
                        LocalLanguage = item.LocalLanguage?.Trim()
                    };

                    entity.SuAddresses.Add(address);

                    _repo.AddEntity(address);
                }
            }

            //CustomerList
            if (request.CustomerList != null)
            {
                foreach (var item in request.CustomerList)
                {
                    var cust = new SuSupplierCustomer
                    {
                        CustomerId = item.Id,
                        CreditTerm = item.CreditTerm,
                        Code = item.Code?.Trim(),
                        IsStatisticsVisibility = item.IsStatisticsVisibility
                    };

                    entity.SuSupplierCustomers.Add(cust);
                    _repo.AddEntity(cust);
                }
            }

            //CustomerContactList
            if (request.SupplierContactList != null)
            {
                foreach (var item in request.SupplierContactList.Where(x => !string.IsNullOrEmpty(x.ContactName)))
                {
                    var contact = new SuContact
                    {
                        Active = true,
                        Comment = item.Comment?.Trim(),
                        ContactName = item.ContactName?.Trim(),
                        Fax = item.Fax?.Trim(),
                        JobTitle = item.JobTitle?.Trim(),
                        Mail = item.ContactEmail?.Trim(),
                        Mobile = item.Mobile?.Trim(),
                        Phone = item.Phone?.Trim(),
                        PrimaryEntity = item.PrimaryEntity,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    //configure api services for supplier contacts
                    if (item.ContactAPIServiceIds != null)
                    {
                        contact.SuContactApiServices = new List<SuContactApiService>();
                        foreach (var serviceId in item.ContactAPIServiceIds)
                        {
                            var suContactAPIServices = new SuContactApiService
                            {
                                ServiceId = serviceId,
                                Active = true,
                                CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                                CreatedOn = DateTime.Now
                            };

                            contact.SuContactApiServices.Add(suContactAPIServices);
                            _repo.AddEntity(suContactAPIServices);
                        }
                    }

                    //configure api entity for supplier contacts
                    if (item.ApiEntityIds != null)
                    {
                        contact.SuContactEntityMaps = new List<SuContactEntityMap>();
                        foreach (var entityId in item.ApiEntityIds)
                        {
                            var suContactEntity = new SuContactEntityMap
                            {
                                EntityId = entityId
                            };

                            contact.SuContactEntityMaps.Add(suContactEntity);
                            _repo.AddEntity(suContactEntity);
                        }
                    }

                    //configure api entity and service for supplier contacts
                    if (item.EntityServiceIds != null)
                    {
                        contact.SuContactEntityServiceMaps = new List<SuContactEntityServiceMap>();
                        foreach (var data in item.EntityServiceIds)
                        {
                            var suContactServiceEntity = new SuContactEntityServiceMap
                            {
                                EntityId = data.EntityId,
                                ServiceId = data.ServiceId
                            };

                            contact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
                            _repo.AddEntity(suContactServiceEntity);
                        }
                    }
                    else if (item.ApiEntityIds != null && item.ContactAPIServiceIds != null)
                    {
                        foreach (var apiEntityId in item.ApiEntityIds)
                        {
                            foreach (var serviceId in item.ContactAPIServiceIds)
                            {

                                var suContactServiceEntity = new SuContactEntityServiceMap
                                {
                                    EntityId = apiEntityId,
                                    ServiceId = serviceId
                                };

                                contact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
                                _repo.AddEntity(suContactServiceEntity);
                            }
                        }
                    }

                    entity.SuContacts.Add(contact);
                    _repo.AddEntity(contact);

                    if (item.CustomerList != null)
                    {
                        foreach (var cust in item.CustomerList)
                        {
                            var custContact = new SuSupplierCustomerContact
                            {
                                CustomerId = cust.Id,
                                Contact = contact
                            };

                            entity.SuSupplierCustomerContacts.Add(custContact);
                            _repo.AddEntity(custContact);
                        }
                    }


                }
            }

            // GradeList
            if (request.GradeList != null && !request.GradeList.Any())
            {
                var entityId = _filterService.GetCompanyId();
                foreach (var grade in request.GradeList)
                {
                    var suGrade = new SuGrade
                    {
                        CustomerId = grade.CustomerId,
                        LevelId = grade.LevelId,
                        PeriodFrom = grade.PeriodFrom.ToDateTime(),
                        PeriodTo = grade.PeriodTo.ToDateTime(),
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        EntityId = entityId
                    };

                    entity.SuGrades.Add(suGrade);

                    _repo.AddEntity(suGrade);
                }
            }

            //configure api services for the suppliers
            if (request.ApiServiceIds != null)
            {
                foreach (var id in request.ApiServiceIds)
                {
                    var suApiService = new SuApiService()
                    {
                        ServiceId = id,
                        Active = true,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };
                    entity.SuApiServices.Add(suApiService);
                    _repo.AddEntity(suApiService);
                }
            }

            entity.SuSupplierFactoryParents = new HashSet<SuSupplierFactory>();

            //if user select same supplier or same factory
            if (isNew)
            {
                //create new entity of supplier or factory based on the checkbox 
                //1. if user type is supplier and select new factory then pass the factory type 
                //2. if user type is factory and select new supplier then pass the supplier type
                var newSupplierOrFactoryEntity = CreateEntity(request, false, request.TypeId == (int)Supplier_Type.Supplier_Agent ? (int)Supplier_Type.Factory : (int)Supplier_Type.Supplier_Agent);

                //create new entity of supplier or factory based on the checkbox 
                //1. if user type is supplier and select new factory then parent factory (isNewEntity) and Supplier (entity)
                //2. if user type is factory and select new supplier then parent parent (entity) and Supplier (isNewEntity)
                var parents = new SuSupplierFactory
                {
                    Parent = request.TypeId == (int)Supplier_Type.Supplier_Agent ? newSupplierOrFactoryEntity : entity,
                    Supplier = request.TypeId == (int)Supplier_Type.Supplier_Agent ? entity : newSupplierOrFactoryEntity
                };
                _repo.AddEntity(parents);
            }
            else
            {
                if (request.SupplierParentList != null)
                {
                    foreach (var item in request.SupplierParentList)
                    {
                        var suppParent = new SuSupplierFactory
                        {
                            SupplierId = item.Id
                        };
                        entity.SuSupplierFactoryParents.Add(suppParent);

                        _repo.AddEntity(suppParent);
                    }
                }
            }

            _repo.AddEntity(entity);
            return entity;
        }

        private async Task<SaveSupplierResponse> UpdateSupplier(SupplierDetails request)
        {
            var entity = await _repo.GetSupplierDetails(request.Id);
            var loggdInUserId = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
            if (entity == null)
                return new SaveSupplierResponse { Result = SaveSupplierResult.SupplierIsNotFound };

            if (request.CustomerList != null)
            {
                var supplierCodeExists = await IsSupplierCodeExistsAsync(request);
                if (supplierCodeExists != null)
                    return new SaveSupplierResponse { ErrorData = supplierCodeExists, Result = SaveSupplierResult.SupplierCodeExists };
            }

            entity.SupplierName = request.Name?.Trim()?.ToUpper();
            entity.Active = true;
            entity.Comments = request.Comment?.Trim();
            entity.LegalName = request.LegalName?.Trim()?.ToUpper();
            entity.Fax = request.Fax?.Trim();
            entity.Email = request.Email?.Trim();
            entity.ContactPerson = request.ContactPersonName?.Trim();
            entity.GlCode = request.GlCode?.Trim();
            entity.LevelId = request.LevelId == 0 ? null : request.LevelId;
            entity.LocalName = request.LocLanguageName?.Trim();
            entity.Mobile = request.Mobile?.Trim();
            entity.Phone = request.Phone?.Trim();
            entity.OwnerShipId = request.OwnerId;
            entity.TypeId = request.TypeId;
            entity.Website = request.WebSite?.Trim();
            entity.TotalStaff = request.TotalStaff?.Trim();
            entity.DailyProduction = request.DailyProduction?.Trim();
            entity.CreditTermId = request.CreditTerm;
            entity.StatusId = request.Status;
            entity.Vatno = request.VatNo?.Trim();
            entity.ModifiedBy = loggdInUserId;
            entity.ModifiedOn = DateTime.Now;
            entity.CompanyId = request.CompanyId;
            entity.IsEaqf = request.IsEAQF;

            // AddressList
            var addressIds = request.AddressList.Where(x => x.CountryId > 0).Select(x => x.Id).ToArray();
            var lstAddressToremove = new List<SuAddress>();
            var SuAddresses = entity.SuAddresses.Where(x => !addressIds.Contains(x.Id));
            foreach (var item in SuAddresses)
            {
                lstAddressToremove.Add(item);
                entity.SuAddresses.Remove(item);
            }

            _repo.RemoveEntities(lstAddressToremove);

            if (request.AddressList != null)
            {
                foreach (var item in request.AddressList.Where(x => x.Id <= 0 && x.CountryId > 0))
                    entity.SuAddresses.Add(new SuAddress
                    {
                        CountryId = item.CountryId,
                        CityId = item.CityId,
                        RegionId = item.RegionId,
                        ZipCode = item.ZipCode?.Trim(),
                        Address = item.Way?.Trim(),
                        AddressTypeId = item.AddressTypeId == 0 ? (int)Supplier_Address_Type.HeadOffice : (int?)item.AddressTypeId,
                        Longitude = item.Longitude,
                        Latitude = item.Latitude,
                        LocalLanguage = item.LocalLanguage?.Trim(),
                        CountyId = item.CountyId,
                        TownId = item.TownId
                    });

                var lstAddressToEdit = new List<SuAddress>();
                foreach (var item in request.AddressList.Where(x => x.Id > 0 && x.CountryId > 0))
                {
                    var address = entity.SuAddresses.FirstOrDefault(x => x.Id == item.Id);

                    if (address != null)
                    {
                        lstAddressToEdit.Add(address);

                        address.CountryId = item.CountryId;
                        address.CityId = item.CityId;
                        address.RegionId = item.RegionId;
                        address.ZipCode = item.ZipCode;
                        address.Address = item.Way?.Trim();
                        address.AddressTypeId = item.AddressTypeId == 0 ? (int)Supplier_Address_Type.HeadOffice : (int?)item.AddressTypeId;
                        address.Longitude = item.Longitude;
                        address.Latitude = item.Latitude;
                        address.LocalLanguage = item.LocalLanguage?.Trim();
                        address.CountyId = item.CountyId;
                        address.TownId = item.TownId;
                    }

                }

                if (lstAddressToEdit.Count > 0)
                    _repo.EditEntities(lstAddressToEdit);
            }

            //CustomerList

            var SuSupplierCustomers = entity.SuSupplierCustomers.ToList();
            foreach (var item in SuSupplierCustomers)
                entity.SuSupplierCustomers.Remove(item);

            if (SuSupplierCustomers.Count > 0)
                _repo.RemoveEntities(SuSupplierCustomers);

            if (request.CustomerList != null)
            {
                foreach (var item in request.CustomerList)
                    entity.SuSupplierCustomers.Add(new SuSupplierCustomer
                    {
                        CustomerId = item.Id,
                        Code = item.Code?.Trim(),
                        CreditTerm = item.CreditTerm,
                        IsStatisticsVisibility = item.IsStatisticsVisibility

                    });
            }

            //CustomerContactList            
            var lstContactToRemove = new List<SuContact>();

            var SuSupplierCustomerContacts = entity.SuSupplierCustomerContacts.ToList();

            foreach (var item in SuSupplierCustomerContacts)
                entity.SuSupplierCustomerContacts.Remove(item);

            _repo.RemoveEntities(SuSupplierCustomerContacts);

            //
            if (request.GradeList == null)
                request.GradeList = new List<SupplierGrade>();

            var suGrades = await _repo.GetSupplierGradesBySupplierId(request.Id);
            var dbGradeIds = request.GradeList.Where(x => x.Id > 0).Select(y => y.Id).ToList();
            var deleteSuGrades = suGrades.Where(x => !dbGradeIds.Contains(x.Id)).ToList();
            if (deleteSuGrades.Any())
            {
                deleteSuGrades.ForEach(x =>
                {
                    x.DeletedOn = DateTime.Now;
                    x.DeletedBy = loggdInUserId;
                    x.Active = false;
                });
                _repo.EditEntities(deleteSuGrades);
            }

            var updatedGrades = request.GradeList.Where(x => x.Id > 0).ToList();
            if (updatedGrades.Any())
            {
                foreach (var grade in updatedGrades)
                {
                    var dbGrade = suGrades.FirstOrDefault(x => x.Id == grade.Id);
                    if (dbGrade != null)
                    {
                        dbGrade.LevelId = grade.LevelId;
                        dbGrade.PeriodFrom = grade.PeriodFrom.ToDateTime();
                        dbGrade.PeriodTo = grade.PeriodTo.ToDateTime();
                        dbGrade.CustomerId = grade.CustomerId;
                        dbGrade.UpdatedBy = loggdInUserId;
                        dbGrade.UpdatedOn = DateTime.Now;
                    }
                }
            }

            var newGrades = request.GradeList.Where(x => x.Id == 0).ToList();
            if (newGrades.Any())
            {
                var entityId = _filterService.GetCompanyId();
                foreach (var grade in newGrades)
                {
                    var suGrade = new SuGrade
                    {
                        CustomerId = grade.CustomerId,
                        LevelId = grade.LevelId,
                        PeriodFrom = grade.PeriodFrom.ToDateTime(),
                        PeriodTo = grade.PeriodTo.ToDateTime(),
                        CreatedBy = loggdInUserId,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        EntityId = entityId
                    };

                    entity.SuGrades.Add(suGrade);

                    _repo.AddEntity(suGrade);
                }
            }




            //deleted cu entity 
            var deleteSupplierEntityList = entity.SuEntities.Where(x => x.Active == true && !request.SupplierEntityIds.Contains(x.EntityId)).ToList();

            //get the db customer entity id
            var dbSupplierEntityIdList = entity.SuEntities.Where(x => x.Active == true).Select(y => y.EntityId);
            //fetch new entityid
            var newSupplierEntityList = request.SupplierEntityIds.Where(x => !dbSupplierEntityIdList.Contains(x)).ToList();

            //get active customer contacts
            var supplierContacts = entity.SuContacts.Where(x => x.Active == true);

            var supplierContactEntityMaps = supplierContacts.SelectMany(x => x.SuContactEntityMaps);
            var anySupplierContactDeletedEntity = supplierContactEntityMaps.Any(x => request.SupplierContactList.Any(y => x.ContactId == y.ContactId && !y.ApiEntityIds.Contains(x.EntityId)));
            var anySupplierContactAddedEntity = request.SupplierContactList.Any(x => x.ApiEntityIds.Any(y => !supplierContactEntityMaps.Where(a => a.ContactId == x.ContactId).Select(y => y.EntityId).Contains(y)));
            SupplierContactEntityData supplierContactEntity = null;
            //based on deleted customer entity , new customer entity and customer contacts fetch the customer contact entity data
            if ((deleteSupplierEntityList.Any() || newSupplierEntityList.Any() || anySupplierContactDeletedEntity || anySupplierContactAddedEntity) && supplierContacts.Any())
            {
                supplierContactEntity = await GetSupplierContactEntityData(entity.Id, supplierContacts, entity.TypeId);
            }




            if (request.SupplierContactList != null)
            {
                var SupplierContactIds = request.SupplierContactList.Where(x => x.ContactId > 0 && !string.IsNullOrEmpty(x.ContactName)).Select(x => x.ContactId).ToArray();
                var SuContacts = entity.SuContacts.Where(x => !SupplierContactIds.Contains(x.Id)).ToList();

                foreach (var item in SuContacts)
                {
                    item.Active = false;
                    lstContactToRemove.Add(item);
                    _repo.RemoveEntities(item.SuContactEntityServiceMaps);
                }

                if (lstContactToRemove.Count > 0)
                {
                    _repo.EditEntities(lstContactToRemove);

                    //Disable the Supplier contact in IT_UserMaster
                    var contactIds = lstContactToRemove.Select(x => x.Id).ToList();
                    var userId = await _userAccountRepo.GetUserListByContactId(contactIds);
                    var deletedby = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                    foreach (var item in userId)
                    {
                        await _userAccountRepo.RemoveUserAccount(item, (int)deletedby);
                    }
                }

                foreach (var item in request.SupplierContactList.Where(x => x.ContactId <= 0 && !string.IsNullOrEmpty(x.ContactName)))
                {
                    var contact = new SuContact
                    {
                        Active = true,
                        Comment = item.Comment?.Trim(),
                        ContactName = item.ContactName?.Trim(),
                        Fax = item.Fax?.Trim(),
                        JobTitle = item.JobTitle?.Trim(),
                        Mail = item.ContactEmail?.Trim(),
                        Mobile = item.Mobile?.Trim(),
                        Phone = item.Phone?.Trim(),
                        PrimaryEntity = item.PrimaryEntity,
                        CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    };

                    //add api servies for the supplier contacts
                    if (item.ContactAPIServiceIds != null)
                    {
                        contact.SuContactApiServices = new List<SuContactApiService>();
                        foreach (var serviceId in item.ContactAPIServiceIds)
                        {
                            var suContactApiService = new SuContactApiService
                            {
                                ServiceId = serviceId,
                                Active = true,
                                CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                                CreatedOn = DateTime.Now
                            };
                            contact.SuContactApiServices.Add(suContactApiService);
                            _repo.AddEntity(suContactApiService);
                        }
                    }

                    //add api entity for the supplier contacts
                    if (item.ApiEntityIds != null)
                    {
                        contact.SuContactEntityMaps = new List<SuContactEntityMap>();
                        foreach (var entityId in item.ApiEntityIds)
                        {
                            var suContactEntity = new SuContactEntityMap
                            {
                                EntityId = entityId
                            };
                            contact.SuContactEntityMaps.Add(suContactEntity);
                            _repo.AddEntity(suContactEntity);
                        }
                    }

                    //configure api entity and service for supplier contacts
                    if (item.EntityServiceIds != null && item.EntityServiceIds.Any())
                    {
                        contact.SuContactEntityServiceMaps = new List<SuContactEntityServiceMap>();
                        foreach (var data in item.EntityServiceIds)
                        {
                            var suContactServiceEntity = new SuContactEntityServiceMap
                            {
                                EntityId = data.EntityId,
                                ServiceId = data.ServiceId
                            };

                            contact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
                            _repo.AddEntity(suContactServiceEntity);
                        }
                    }
                    else if (item.ApiEntityIds != null && item.ContactAPIServiceIds != null)
                    {
                        foreach (var apiEntityId in item.ApiEntityIds)
                        {
                            foreach (var serviceId in item.ContactAPIServiceIds)
                            {

                                var suContactServiceEntity = new SuContactEntityServiceMap
                                {
                                    EntityId = apiEntityId,
                                    ServiceId = serviceId
                                };

                                contact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
                                _repo.AddEntity(suContactServiceEntity);
                            }
                        }
                    }

                    entity.SuContacts.Add(contact);

                    foreach (var cust in item.CustomerList)
                        entity.SuSupplierCustomerContacts.Add(new SuSupplierCustomerContact
                        {
                            CustomerId = cust.Id,
                            Contact = contact
                        });
                }


                foreach (var item in request.SupplierContactList.Where(x => x.ContactId > 0 && !string.IsNullOrEmpty(x.ContactName)))
                {
                    var contact = entity.SuContacts.FirstOrDefault(x => x.Id == item.ContactId);
                    if (contact != null)
                    {
                        int dbPrimaryEntity = contact.PrimaryEntity.GetValueOrDefault();
                        contact.Comment = item.Comment?.Trim();
                        contact.ContactName = item.ContactName?.Trim();
                        contact.Fax = item.Fax?.Trim();
                        contact.JobTitle = item.JobTitle?.Trim();
                        contact.Mail = item.ContactEmail?.Trim();
                        contact.Mobile = item.Mobile?.Trim();
                        contact.Phone = item.Phone?.Trim();
                        contact.PrimaryEntity = item.PrimaryEntity;


                        foreach (var cust in item.CustomerList)
                        {
                            entity.SuSupplierCustomerContacts.Add(new SuSupplierCustomerContact
                            {
                                CustomerId = cust.Id,
                                Contact = contact
                            });
                        }

                        // remove existing item
                        _repo.RemoveEntities(contact.SuContactEntityServiceMaps);

                        //update the api services for the supplier contacts
                        if (contact.SuContactApiServices == null)
                            contact.SuContactApiServices = new List<SuContactApiService>();
                        // find the item not exist in current list and update active
                        foreach (var service in contact.SuContactApiServices.Where(x => !item.ContactAPIServiceIds.Contains(x.ServiceId) && x.Active))
                        {
                            service.Active = false;
                            service.DeletedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId;
                            service.DeletedOn = DateTime.Now;
                        }
                        if (item.ContactAPIServiceIds != null)
                        {
                            // find item not exist in entity and add new item
                            foreach (var serviceId in item.ContactAPIServiceIds.Where(x => !contact.SuContactApiServices.Where(z => z.Active).Select(y => y.ServiceId).Contains(x)))
                            {
                                var suContactApiService = new SuContactApiService()
                                {
                                    ServiceId = serviceId,
                                    Active = true,
                                    CreatedBy = request.UserId > 0 ? request.UserId : _ApplicationContext.UserId,
                                    CreatedOn = DateTime.Now
                                };
                                contact.SuContactApiServices.Add(suContactApiService);
                                _repo.AddEntity(suContactApiService);
                            }
                        }


                        //update the api entity for the supplier contacts
                        if (contact.SuContactEntityMaps == null)
                            contact.SuContactEntityMaps = new List<SuContactEntityMap>();


                        if (item.ApiEntityIds != null)
                        {

                            //new code 
                            UpdateSupplierContactEntityMap(contact, item.ApiEntityIds, dbPrimaryEntity, supplierContactEntity);
                        }

                        if (contact.SuContactEntityServiceMaps == null)
                            contact.SuContactEntityServiceMaps = new List<SuContactEntityServiceMap>();
                        //configure api entity and service for supplier contacts
                        if (item.EntityServiceIds != null)
                        {
                            contact.SuContactEntityServiceMaps = new List<SuContactEntityServiceMap>();
                            foreach (var data in item.EntityServiceIds)
                            {
                                var suContactServiceEntity = new SuContactEntityServiceMap
                                {
                                    EntityId = data.EntityId,
                                    ServiceId = data.ServiceId
                                };

                                contact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
                                _repo.AddEntity(suContactServiceEntity);
                            }
                        }
                        else if (item.ApiEntityIds != null && item.ContactAPIServiceIds != null)
                        {
                            foreach (var apiEntityId in item.ApiEntityIds)
                            {
                                foreach (var serviceId in item.ContactAPIServiceIds)
                                {

                                    var suContactServiceEntity = new SuContactEntityServiceMap
                                    {
                                        EntityId = apiEntityId,
                                        ServiceId = serviceId
                                    };

                                    contact.SuContactEntityServiceMaps.Add(suContactServiceEntity);
                                    _repo.AddEntity(suContactServiceEntity);
                                }
                            }
                        }


                    }
                }
            }

            var SuSupplierFactoryParents = entity.SuSupplierFactoryParents.ToList();
            foreach (var item in SuSupplierFactoryParents)
                entity.SuSupplierFactoryParents.Remove(item);

            _repo.RemoveEntities(SuSupplierFactoryParents);

            if (request.SupplierParentList != null)
            {
                foreach (var item in request.SupplierParentList)
                    entity.SuSupplierFactoryParents.Add(new SuSupplierFactory
                    {
                        SupplierId = item.Id
                    });
            }

            UpdateSupplierServices(request, entity, _ApplicationContext.UserId);



            //if user remove any entity 
            if (deleteSupplierEntityList.Any())
            {
                var deleteSupplierEntityResult = DeleteSupplierEntityList(deleteSupplierEntityList);
                if (deleteSupplierEntityResult != null)
                    return deleteSupplierEntityResult;
            }

            if (newSupplierEntityList.Any())
            {
                AddSupplierEntityList(entity.Id, newSupplierEntityList, supplierContactEntity, request.MapAllSupplierContacts);
            }

            int id = await _repo.EditSupplier(entity);

            if (id > 0)
            {
                return new SaveSupplierResponse { Id = entity.Id, Result = SaveSupplierResult.Success };

            }

            return new SaveSupplierResponse { Id = entity.Id, Result = SaveSupplierResult.SupplierIsNotFound };

        }


        //update supplier contact entity map-> if any entity add in contact and delete entity form contact
        private void UpdateSupplierContactEntityMap(SuContact contact, IEnumerable<int> apiEntityIds, int dbPrimaryEntity, SupplierContactEntityData supplierContactEntity)
        {
            //fetch the deleted supplier contact entity map 
            var deleteSupplierContactEntityList = contact.SuContactEntityMaps.Where(x => !apiEntityIds.Contains(x.EntityId));

            //fetch the db supplier contact entityid
            var dbSupplierContactEntityIds = contact.SuContactEntityMaps.Select(x => x.EntityId);

            //from request get only new api entity ids
            var newSupplierContactEntityIds = apiEntityIds.Where(x => !dbSupplierContactEntityIds.Contains(x));


            IEnumerable<ItUserRole> supplierContactUserRoles = null;
            if ((deleteSupplierContactEntityList.Any() || newSupplierContactEntityIds.Any()) && supplierContactEntity != null && supplierContactEntity.SupplierContactUsers != null && supplierContactEntity.SupplierContactUserRoles != null)
            {
                var supplierContactUsers = supplierContactEntity.SupplierContactUsers.Where(x => x.ContactId == contact.Id);
                var supplierContactUserIds = supplierContactUsers.Select(x => x.UserId);
                supplierContactUserRoles = supplierContactEntity.SupplierContactUserRoles.Where(x => supplierContactUserIds.Contains(x.UserId));
            }
            // delete supplier contact entity
            if (deleteSupplierContactEntityList != null && deleteSupplierContactEntityList.Any())
            {
                if (supplierContactUserRoles != null && supplierContactUserRoles.Any())
                {
                    var deleteSupplierContactEntityIds = deleteSupplierContactEntityList.Select(y => y.EntityId);
                    var deleteSupplierContactUserRoles = supplierContactUserRoles.Where(x => deleteSupplierContactEntityIds.Contains(x.EntityId));
                    _repo.RemoveEntities(deleteSupplierContactUserRoles);
                }
                _repo.RemoveEntities(deleteSupplierContactEntityList);
            }

            //add new supplier contact entity
            if (newSupplierContactEntityIds.Any())
            {
                //new supplier contact entity id loop
                newSupplierContactEntityIds.ToList().ForEach(entity =>
                {
                    //add new supplier contact entity map
                    var supplierContactEntityMap = new SuContactEntityMap()
                    {
                        EntityId = entity
                    };

                    contact.SuContactEntityMaps.Add(supplierContactEntityMap);
                    _repo.AddEntity(supplierContactEntityMap);

                    //if any supplier contact user roles available
                    if (supplierContactUserRoles != null && supplierContactUserRoles.Any())
                    {
                        // if for existing data role already map with new entity then we want to skip this role
                        var newlyEntityContactUserRoles = supplierContactUserRoles.Where(x => x.EntityId == entity);
                        if (newlyEntityContactUserRoles.Any())
                        {
                            var newlyEntityRoles = newlyEntityContactUserRoles.Select(x => x.RoleId);
                            supplierContactUserRoles = supplierContactUserRoles.Where(x => !newlyEntityRoles.Contains(x.RoleId));
                        }
                        //get the supplier contact primary roles based on db primary entity
                        var supplierContactPrimaryRoles = supplierContactUserRoles.Where(x => x.EntityId == dbPrimaryEntity).ToList();
                        if (supplierContactUserRoles.Any())
                        {
                            // loop of primary entity role
                            supplierContactPrimaryRoles.ForEach(userRole =>
                            {
                                //add new user role
                                var itUserRole = new ItUserRole()
                                {
                                    UserId = userRole.UserId,
                                    RoleId = userRole.RoleId,
                                    EntityId = entity
                                };

                                _repo.AddEntity(itUserRole);
                            });
                        }
                    }
                });
            }
        }

        //delete supplier list
        private SaveSupplierResponse DeleteSupplierEntityList(List<SuEntity> deletedSupplierEntityList)
        {

            if (deletedSupplierEntityList.Any())
            {
                //delete supplier entity
                deletedSupplierEntityList.ForEach(supplierEntity =>
                {
                    supplierEntity.Active = false;
                    supplierEntity.DeletedOn = DateTime.Now;
                    supplierEntity.DeletedBy = _ApplicationContext.UserId;
                });
            }


            _repo.EditEntities(deletedSupplierEntityList);

            return null;
        }

        //add supplier entity list
        private void AddSupplierEntityList(int supplierId, List<int> newSupplierEntityIds, SupplierContactEntityData supplierContactEntity, bool isMappedToAllContacts)
        {

            //new supplier entity loop
            newSupplierEntityIds.ForEach(entityId =>
            {
                var suEntity = new SuEntity()
                {
                    Active = true,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now,
                    EntityId = entityId,
                    SupplierId = supplierId

                };

                _repo.AddEntity(suEntity);

                //is mapped to all contacts
                if (isMappedToAllContacts)
                {

                    if (supplierContactEntity != null && supplierContactEntity.SupplierContacts != null && supplierContactEntity.SupplierContacts.Any())
                    {
                        //supplier contacts loop
                        supplierContactEntity.SupplierContacts.ForEach(supplierContact =>
                        {
                            var suContactEntityMap = new SuContactEntityMap()
                            {
                                EntityId = entityId,
                                ContactId = supplierContact.Id
                            };


                            //add  SU_Supplier_Contact_Entity_Map
                            supplierContact.SuContactEntityMaps.Add(suContactEntityMap);
                            _repo.AddEntity(suContactEntityMap);

                            //add SU_Supplier_Contact_Entity_Service_Map
                            var supplierContactPrimaryEntityServiceList = supplierContact.SuContactEntityServiceMaps.Where(x => x.EntityId == supplierContact.PrimaryEntity).ToList();
                            supplierContactPrimaryEntityServiceList.ForEach(contactPrimaryEntityService =>
                            {
                                var suContactPrimaryEntityService = new SuContactEntityServiceMap()
                                {
                                    ContactId = supplierContact.Id,
                                    ServiceId = contactPrimaryEntityService.ServiceId,
                                    EntityId = entityId,
                                };
                                supplierContact.SuContactEntityServiceMaps.Add(suContactPrimaryEntityService);
                                _repo.AddEntity(suContactPrimaryEntityService);
                            });


                            if (supplierContactEntity.SupplierContactUserRoles != null && supplierContactEntity.SupplierContactUserRoles.Any())
                            {
                                var supplierContactUsers = supplierContactEntity.SupplierContactUsers.Where(x => x.ContactId == supplierContact.Id);
                                if (supplierContactUsers.Any())
                                {
                                    var supplierContactUserIds = supplierContactUsers.Select(y => y.UserId);
                                    //add Primary roles 
                                    var supplierContactUserRoles = supplierContactEntity.SupplierContactUserRoles.Where(x => supplierContactUserIds.Contains(x.UserId) && x.EntityId == supplierContact.PrimaryEntity).ToList();
                                    if (supplierContactUserRoles.Any())
                                    {
                                        supplierContactUserRoles.ForEach(userRole =>
                                        {
                                            var itUserRole = new ItUserRole()
                                            {
                                                EntityId = entityId,
                                                RoleId = userRole.RoleId,
                                                UserId = userRole.UserId
                                            };
                                            _repo.AddEntity(itUserRole);
                                        });
                                    }

                                }

                            }
                        });


                    }

                }
            });
        }


        //get supplier contact details
        private async Task<SupplierContactEntityData> GetSupplierContactEntityData(int supplierId, IEnumerable<SuContact> supplierContacts, int? supplierType)
        {
            if (!supplierContacts.Any())
                return null;

            var supplierContactEntity = new SupplierContactEntityData();
            supplierContactEntity.SupplierContacts = supplierContacts.ToList();
            var userQueryable = _userAccountRepo.GetUserDetails();
            List<SupplierContactUser> supplierContactUsers = null;
            if (supplierType == (int)Supplier_Type.Factory)
                supplierContactUsers = await userQueryable.Where(x => x.FactoryId == supplierId && x.Active == true).Select(y => new SupplierContactUser() { ContactId = y.FactoryContactId, UserId = y.Id }).AsNoTracking().ToListAsync();
            else
                supplierContactUsers = await userQueryable.Where(x => x.SupplierId == supplierId && x.Active == true).Select(y => new SupplierContactUser() { ContactId = y.SupplierContactId, UserId = y.Id }).AsNoTracking().ToListAsync();


            if (supplierContactUsers.Any())
            {
                supplierContactEntity.SupplierContactUsers = supplierContactUsers;
                supplierContactEntity.SupplierContactUserRoles = await _userRepository.GetUserRolesByUserIdsIgnoreQueryFilter(supplierContactUsers.Select(y => y.UserId));
            }

            return supplierContactEntity;
        }

        private void UpdateSupplierServices(SupplierDetails request, SuSupplier entity, int userId)
        {
            var serviceIds = request.ApiServiceIds.Select(x => x).ToArray();
            var lstServiceToremove = new List<SuApiService>();
            var services = entity.SuApiServices.Where(x => !serviceIds.Contains(x.ServiceId) && x.Active.HasValue && x.Active.Value);
            var existingServices = entity.SuApiServices.Where(x => serviceIds.Contains(x.ServiceId) && x.Active.HasValue && x.Active.Value);

            // Remove if data does not exist in the db.

            foreach (var item in services)
            {
                lstServiceToremove.Add(item);
                item.DeletedOn = DateTime.Now;
                item.DeletedBy = userId;
                item.Active = false;
            }

            _repo.EditEntities(lstServiceToremove);

            // Update if data already exist in the db

            if (request.ApiServiceIds != null)
            {
                // Add if data is new it means id = 0;
                foreach (var id in serviceIds)
                {
                    if (!existingServices.Any() || !existingServices.Any(x => x.ServiceId == id))
                    {
                        entity.SuApiServices.Add(new SuApiService()
                        {
                            ServiceId = id,
                            Active = true,
                            CreatedBy = _ApplicationContext.UserId,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }
        }

        public async Task<SupplierListResponse> GetSuppliersByCustomerId(int customerId)
        {
            var suplist = await _repo.GetSupplierByCustomerId(customerId);
            if (suplist == null || suplist.Count == 0)
                new SupplierListResponse() { Result = SupplierListResult.NodataFound };
            return new SupplierListResponse() { Result = SupplierListResult.Success, Data = suplist.Select(SupplierMap.MapSupplierItem) };
        }

        public async Task<SupplierListResponse> GetFactoryByCustomerId(int customerId)
        {
            var factlist = await _repo.GetFactoryByCustomerId(customerId);
            if (factlist == null || factlist.Count == 0)
                new SupplierListResponse() { Result = SupplierListResult.NodataFound };
            return new SupplierListResponse() { Result = SupplierListResult.Success, Data = factlist.Select(SupplierMap.MapSupplierItem) };
        }

        public async Task<IEnumerable<suppliercontact>> GetSupplierContactsById(int supid, int cusid)
        {
            var data = await _repo.GetSuppliercontactById(supid, cusid);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(x => SupplierMap.GetSupplierContact(x));
        }

        public async Task<IEnumerable<suppliercontact>> GetFactoryContactsById(int supid, int cusid)
        {
            var data = await _repo.GetSuppliercontactById(supid, cusid);
            if (data == null || data.Count == 0)
                return null;
            return data.Select(SupplierMap.GetSupplierContact);
        }

        public async Task<SupplierListResponse> GetFactoryByCustomerSupplierId(int? customerId, int? supplierId)
        {
            var factlist = await _repo.GetFactoryByCustomerIdSupplierId(customerId, supplierId);
            if (factlist == null || factlist.Count == 0)
                new SupplierListResponse() { Result = SupplierListResult.NodataFound };
            return new SupplierListResponse() { Result = SupplierListResult.Success, Data = factlist.Select(SupplierMap.MapSupplierItem) };
        }

        public async Task<SupplierListResponse> GetFactoryBySupplierId(int supplierId)
        {
            var factlist = await _repo.GetFactoryBySupplierId(supplierId);
            if (factlist == null || factlist.Count == 0)
                new SupplierListResponse() { Result = SupplierListResult.NodataFound };
            return new SupplierListResponse() { Result = SupplierListResult.Success, Data = factlist.Select(SupplierMap.MapSupplierItem) };
        }

        public async Task<SupplierListResponse> GetSuppliersByUserType(int? CustomerId, bool isBookingRequest = false)
        {
            var response = new SupplierListResponse();
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        response = await GetSuppliersByCustomerId(CustomerId == null ? 0 : CustomerId.Value);
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        if (!isBookingRequest)
                            CustomerId = _ApplicationContext.CustomerId;
                        else
                        {
                            if (!CustomerId.HasValue)
                                CustomerId = _ApplicationContext.CustomerId;
                        }
                        if (!CustomerId.HasValue && CustomerId.Value > 0)
                            return new SupplierListResponse() { Result = SupplierListResult.NodataFound };

                        response = await GetSuppliersByCustomerId(CustomerId.Value);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        var supdetails = await GetSupplierDetails(_ApplicationContext.SupplierId);
                        if (supdetails != null)
                        {
                            response.Data = new List<SupplierItem>().Append(new SupplierItem { Id = supdetails.Id, Name = supdetails.Name });
                            response.Result = SupplierListResult.Success;
                        }
                        else if (supdetails == null)
                            response.Result = SupplierListResult.NodataFound;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        response = await GetSupplierByFactId(_ApplicationContext.FactoryId);
                        break;
                    }

            }
            return response;
        }

        public async Task<SupplierListResponse> GetSupplierByFactId(int factoryId)
        {
            var data = await _repo.GetSupplierByfactId(factoryId);
            if (data == null || data.Count() == 0)
                return new SupplierListResponse() { Result = SupplierListResult.NodataFound };
            return new SupplierListResponse() { Result = SupplierListResult.Success, Data = data.Select(SupplierMap.MapSupplierItem) };
        }

        public async Task<SupplierListResponse> GetFactorysByUserType(int? customerId, int? SupplierId)
        {
            var response = new SupplierListResponse();
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        response = await GetFactoryByCustomerSupplierId(customerId, SupplierId != null ? SupplierId.Value : 0);
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        response = await GetFactoryByCustomerSupplierId(customerId, SupplierId != null ? SupplierId.Value : 0);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        response = await GetFactoryByCustomerSupplierId(customerId, _ApplicationContext.SupplierId);
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        var factdetails = await GetSupplierDetails(_ApplicationContext.FactoryId);
                        response.Data = new List<SupplierItem>().Append(new SupplierItem
                        {
                            Id = factdetails.Id,
                            RegionalLanguageName = factdetails.LocLanguageName,
                            Name = factdetails.Name,
                        });
                        response.Result = SupplierListResult.Success;
                        break;
                    }

            }
            return response;
        }

        public async Task<IEnumerable<CreditTerm>> GetCreditTerms()
        {
            var creditTermList = await _repo.GetCreditTerms();

            if (creditTermList == null || creditTermList.Count == 0)
                return null;

            return creditTermList.Select(SupplierMap.GetCreditTerm);
        }

        public async Task<IEnumerable<Status>> GetStatus()
        {
            var statusList = await _repo.GetStatus();

            if (statusList == null || statusList.Count == 0)
                return null;

            return statusList.Select(SupplierMap.GetStatus);
        }

        public async Task<SupplierListResponse> GetSupplierByName(string supName, int type)
        {
            if (supName.Length >= 4)
            {
                var data = await _repo.GetAllSuppliersByName(supName).ToListAsync();
                if (type != 0)
                    data = data.Where(x => x.TypeId == type).ToList();

                return new SupplierListResponse() { Result = SupplierListResult.Success, Data = data.Select(SupplierMap.MapSupplierItem) };
            }

            else
            {
                return new SupplierListResponse();
            }
        }
        public async Task<SupplierAddress> GetSupplierHeadOfficeAddress(int supplierId)
        {
            if (supplierId > 0)
            {
                var data = await _repo.GetSupplierHeadOfficeAddress(supplierId);

                return new SupplierAddress()
                {
                    Result = SupplierListResult.Success,
                    Address = data.Address,
                    RegionalAddress = data.RegionalAddress,
                    SupplierName = data.SupplierName,
                    RegionalSupplierName = data.RegionalSupplierName,
                    Latitude = data.Latitude,
                    Longitude = data.Longitude,
                    countryId = data.countryId
                };
            }
            else
            {
                return new SupplierAddress()
                {
                    Result = SupplierListResult.NodataFound,
                };
            }
        }

        public async Task<string> GetSupplierCode(int Supid, int cusid)
        {
            return await _repo.GetSupplierCode(Supid, cusid);
        }

        public async Task<List<SupplierAddress>> GetSupplierOfficeAddressBylstId(List<int> lstsupplierId)
        {
            return await _repo.GetSupplierOfficeAddressBylstId(lstsupplierId);
        }
        /// <summary>
        /// Get the supplier id by factoryid
        /// </summary>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        public async Task<int> GetFactoryIdBySupplierId(int supplierId)
        {
            return await _repo.GetFactoryIdBySupplierId(supplierId);
        }
        public async Task<List<int>> GetFactoryByCountryId(List<int> countrylist)
        {
            return await _repo.GetFactoryByCountryId(countrylist);
        }

        //vertical scroll for supplier
        public async Task<DataSourceResponse> GetSupplierDataSource(CommonDataSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();
                IQueryable<SuSupplier> data = null;

                if (request == null)
                    return response;

                if (string.IsNullOrEmpty(request.SearchText))
                {
                    data = _repo.GetAllSuppliersByName("");
                }
                else
                {
                    data = request.SupSearchTypeId != (int)SearchType.SupplierCode ? _repo.GetAllSuppliersByName(request.SearchText) : _repo.GetAllSuppliersByName("");
                }

                if (request.SupplierType > 0)
                {
                    data = data.Where(x => x.TypeId == request.SupplierType);
                }

                if (request.CustomerId != null && request.CustomerId > 0)
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId));

                //fetch factory data if supplier id available
                if (request.SupplierId != null && request.SupplierId > 0)
                {
                    //data = data.Where(x => x.TypeId == (int)Supplier_Type.Factory && x.SuSupplierFactoryParents.Any(z => z.SupplierId == request.SupplierId));
                    data = data.Where(z => z.SuSupplierFactoryParents.Any(y => y.SupplierId == request.SupplierId));
                }

                //apply customer glCodes filter
                if (request.CustomerGLCodes != null && request.CustomerGLCodes.Any())
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => request.CustomerGLCodes.Contains(y.Customer.GlCode)));

                //apply service id
                if (request.ServiceId != null && request.ServiceId > 0)
                    data = data.Where(x => x.SuApiServices.Any(y => y.ServiceId == request.ServiceId && y.Active == true));


                //get supplier data by factory id
                if (request.FactoryId != null && request.FactoryId > 0)
                {
                    data = data.Where(z => z.SuSupplierFactorySuppliers.Any(y => y.ParentId == request.FactoryId));
                }

                if (request.Id != null && request.Id > 0)
                {
                    data = data.Where(x => x.Id == request.Id);
                }

                //filter the selected factory ids
                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                switch (_ApplicationContext.UserType)
                {
                    case UserTypeEnum.Customer:
                        {
                            var contactId = await _userRepository.GetCustomerContactIdByUserId(_ApplicationContext.UserId);
                            var sisterCompanyIds = await _customerRepository.GetSisterCompanieIdsByCustomerContactId(contactId);
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == _ApplicationContext.CustomerId || sisterCompanyIds.Contains(y.CustomerId)));
                            break;
                        }
                    case UserTypeEnum.Supplier:
                        {
                            if (request.SupplierType == (int)Supplier_Type.Supplier_Agent)
                                data = data.Where(x => x.Id == _ApplicationContext.SupplierId);
                            else if (request.SupplierType == (int)Supplier_Type.Factory && request.SupplierId != null)
                                data = data.Where(x => x.SuSupplierFactoryParents.Any(y => request.SupplierId == y.SupplierId));
                            break;
                        }
                }

                IEnumerable<CommonDataSource> supList = null;
                if (request.Skip != 0)
                {
                    if (request.SupSearchTypeId == (int)SearchType.SupplierCode)
                    {
                        if (!string.IsNullOrEmpty(request.SearchText))
                        {
                            data = data.Where(x => x.SuSupplierCustomers != null && EF.Functions.Like(x.SuSupplierCustomers.Where(y => y.CustomerId == request.CustomerId).Select(y => y.Code).FirstOrDefault(),
                                $"%{request.SearchText.Trim()}%"));
                        }

                        supList = await data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId &&
                        y.Code != null && y.Code != "")).Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.SuSupplierCustomers.Where(y => y.CustomerId == request.CustomerId).Select(y => y.Code).FirstOrDefault()
                        }).OrderBy(o => o.Name).Skip(request.Skip)
                               .Take(request.Take).ToListAsync();

                    }
                    else
                    {
                        supList = await data.Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.SupplierName
                        }).OrderBy(o => o.Name).Skip(request.Skip)
                           .Take(request.Take).ToListAsync();
                    }
                }
                else
                {
                    if (request.SupSearchTypeId == (int)SearchType.SupplierCode)
                    {
                        if (!string.IsNullOrEmpty(request.SearchText))
                        {
                            data = data.Where(x => x.SuSupplierCustomers != null && EF.Functions.Like(x.SuSupplierCustomers.Where(y => y.CustomerId == request.CustomerId).Select(y => y.Code).FirstOrDefault(),
                                $"%{request.SearchText.Trim()}%"));
                        }

                        supList = await data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId &&
                        y.Code != null && y.Code != "")).Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.SuSupplierCustomers.Where(y => y.CustomerId == request.CustomerId).Select(y => y.Code).FirstOrDefault()
                        }).OrderBy(o => o.Name).Take(request.Take).ToListAsync();

                    }
                    else
                    {
                        supList = await data.Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.SupplierName
                        }).OrderBy(o => o.Name).Take(request.Take).ToListAsync();
                    }
                }



                if (supList == null || !supList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = supList;
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
        /// get supplier or factory list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetFactoryOrSupplierList(CommonSupplierSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _repo.GetAllSuppliersAndCountryList();

                if (request.Id > 0)
                {
                    data = data.Where(x => x.Id == request.Id);
                }

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    if (request.IsRegionalNameChecked)
                    {
                        data = data.Where(x => x.LocalName != null && EF.Functions.Like(x.LocalName, "%" + request.SearchText.Trim() + "%"));
                    }
                    else
                    {
                        data = data.Where(x => x.SupplierName != null && EF.Functions.Like(x.SupplierName, "%" + request.SearchText.Trim() + "%"));
                    }
                }

                if (request.SupplierTypes.Any())
                    data = data.Where(x => request.SupplierTypes.Contains(x.TypeId));

                if (request.CountryIds.Any())
                    data = data.Where(x => x.SuAddresses.Any(y => request.CountryIds.Contains(y.CountryId)));

                if (request.CustomerId > 0)
                {
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId));
                }

                if (request.ProvinceId > 0)
                {
                    data = data.Where(x => x.SuAddresses.Any(y => y.RegionId == request.ProvinceId));
                }

                if (request.CityIds != null && request.CityIds.Any())
                {
                    data = data.Where(x => x.SuAddresses.Any(y => request.CityIds.Contains(y.CityId)));
                }

                IEnumerable<CommonDataSource> supplierOrFactoryList = null;
                if (request.Skip != 0)
                {
                    if (!request.IsRegionalNameChecked)
                    {
                        supplierOrFactoryList = await data.Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.SupplierName
                        }).OrderBy(o => o.Name).Skip(request.Skip)
                            .Take(request.Take).ToListAsync();
                    }
                    else
                    {
                        supplierOrFactoryList = await data.Where(x => !string.IsNullOrEmpty(x.LocalName)).Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.LocalName
                        }).OrderBy(o => o.Name).Skip(request.Skip)
                            .Take(request.Take).ToListAsync();
                    }
                }
                else
                {
                    if (!request.IsRegionalNameChecked)
                    {
                        supplierOrFactoryList = await data.Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.SupplierName
                        }).OrderBy(o => o.Name).Take(request.Take).ToListAsync();
                    }
                    else
                    {
                        supplierOrFactoryList = await data.Where(x => !string.IsNullOrEmpty(x.LocalName)).Select(x => new CommonDataSource
                        {
                            Id = x.Id,
                            Name = x.LocalName
                        }).OrderBy(o => o.Name).Take(request.Take).ToListAsync();
                    }
                }

                if (supplierOrFactoryList == null || !supplierOrFactoryList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = supplierOrFactoryList;
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
        /// Get the supplier contacts by supplier id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetSupplierContactsbySupplier(int supplierId)
        {
            var response = new DataSourceResponse();
            var data = await _repo.GetSupplierContactListbySupplier(supplierId);
            if (data == null || data.Count == 0)
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = data;
                response.Result = DataSourceResult.Success;
            }

            return response;
        }

        public async Task<List<SupplierGeoLocation>> GetSupplierGeoLocation(IEnumerable<int> supplierIds)
        {
            return await _repo.GetSupplierGeoLocations(supplierIds);
        }

        /// <summary>
        /// get supplier or factory address details
        /// </summary>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SupplierGeoLocation>> GetSupplierOrFactoryLocations(IEnumerable<int?> supplierIds)
        {
            return await _repo.GetSupplierOrFactoryLocations(supplierIds);
        }


        public async Task<DataSourceResponse> GetSupplierDataSourceList(SupplierDataSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

                var data = _repo.GetSupplierDataSource();

                //apply supplier id
                if (request.SupplierIds != null && request.SupplierIds.Any() && request.SupplierType == (int)Supplier_Type.Supplier_Agent)
                    data = data.Where(x => request.SupplierIds.Contains(x.Id));
                else if (request.SupplierIds != null && request.SupplierIds.Any() && request.SupplierType == (int)Supplier_Type.Factory)
                    data = data.Where(x => x.SuSupplierFactoryParents.Any(y => request.SupplierIds.Contains(y.SupplierId)));

                //apply supplier type filter
                if (request.SupplierType > 0)
                    data = data.Where(x => x.TypeId == request.SupplierType);

                //apply customer filter
                if (request.CustomerIds != null && request.CustomerIds.Any())
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => request.CustomerIds.Contains(y.CustomerId)));

                //apply customer glCodes filter
                if (request.CustomerGLCodes != null && request.CustomerGLCodes.Any())
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => request.CustomerGLCodes.Contains(y.Customer.GlCode)));

                //apply service id
                if (request.ServiceId != null && request.ServiceId > 0)
                    data = data.Where(x => x.SuApiServices.Any(y => y.ServiceId == request.ServiceId));

                switch (_ApplicationContext.UserType)
                {
                    case UserTypeEnum.Customer:
                        {
                            var contactId = await _userRepository.GetCustomerContactIdByUserId(_ApplicationContext.UserId);
                            var sisterCompanyIds = await _customerRepository.GetSisterCompanieIdsByCustomerContactId(contactId);
                            data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == _ApplicationContext.CustomerId || sisterCompanyIds.Contains(y.CustomerId)));
                            break;
                        }
                    case UserTypeEnum.Supplier:
                        {
                            if (request.SupplierType == (int)Supplier_Type.Supplier_Agent)
                                data = data.Where(x => x.Id == _ApplicationContext.SupplierId);
                            else if (request.SupplierType == (int)Supplier_Type.Factory && request.SupplierIds != null)
                                data = data.Where(x => x.SuSupplierFactoryParents.Any(y => request.SupplierIds.Contains(y.SupplierId)));
                            break;
                        }
                    case UserTypeEnum.Factory:
                        {
                            data = data.Where(x => x.Id == _ApplicationContext.FactoryId);
                            break;
                        }
                }

                IEnumerable<CommonDataSource> supList = null;
                if (request.SupSearchTypeId == (int)SearchType.SupplierCode)
                {
                    if (request.CustomerGLCodes != null && request.CustomerGLCodes.Any())
                    {
                        if (!string.IsNullOrEmpty(request.SearchText))
                        {
                            data = data.Where(x => x.SuSupplierCustomers != null && EF.Functions.Like(x.SuSupplierCustomers.Where(a => !string.IsNullOrEmpty(a.Code) && request.CustomerGLCodes.Contains(a.Customer.GlCode)).Select(y => y.Code).FirstOrDefault(), $"%{request.SearchText.Trim()}%"));
                        }
                        supList = await data.Where(x => x.SuSupplierCustomers.Any(y => request.CustomerGLCodes.Contains(y.Customer.GlCode) && !string.IsNullOrEmpty(y.Code))).Skip(request.Skip).Take(request.Take).
                            Select(x => new CommonDataSource
                            {
                                Id = x.Id,
                                Name = x.SuSupplierCustomers.Where(a => !string.IsNullOrEmpty(a.Code) && request.CustomerGLCodes.Contains(a.Customer.GlCode)).Select(y => y.Code).FirstOrDefault()
                            }).AsNoTracking().ToListAsync();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(request.SearchText))
                        {
                            data = data.Where(x => x.SuSupplierCustomers != null && EF.Functions.Like(x.SuSupplierCustomers.Where(a => !string.IsNullOrEmpty(a.Code)).Select(y => y.Code).FirstOrDefault(), $"%{request.SearchText.Trim()}%"));
                        }
                        supList = await data.Where(x => x.SuSupplierCustomers.Any(y => !string.IsNullOrEmpty(y.Code))).Skip(request.Skip).Take(request.Take).
                            Select(x => new CommonDataSource
                            {
                                Id = x.Id,
                                Name = x.SuSupplierCustomers.Where(a => !string.IsNullOrEmpty(a.Code)).Select(y => y.Code).FirstOrDefault()
                            }).AsNoTracking().ToListAsync();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.SearchText))
                    {
                        data = data.Where(x => x.SupplierName != null && EF.Functions.Like(x.SupplierName, $"%{request.SearchText.Trim()}%"));
                    }
                    supList = await data.OrderBy(o => o.SupplierName).Skip(request.Skip).Take(request.Take).
                                    Select(x => new CommonDataSource()
                                    {
                                        Id = x.Id,
                                        Name = x.SupplierName
                                    }).AsNoTracking().ToListAsync();
                }

                if (supList != null && supList.Any())
                {
                    response.DataSourceList = supList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        //get the supplier contact by booking Id
        public async Task<DataSourceResponse> GetSupplierContactByBooking(int bookingId, int supType, int serviceType)
        {
            var data = new List<CommonDataSource>();
            if (serviceType == (int)Service.InspectionId)
            {
                data = await _repo.GetSupplierContactByBooking(bookingId, supType);
            }
            else if (serviceType == (int)Service.AuditId)
            {
                data = await _repo.GetSupplierContactByBookingForAudit(bookingId, supType);
            }

            if (data == null && !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        /// <summary>
        /// get factory details by supplier
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetFactoryDataSourceBySupplier(CommonDataSourceRequest request)
        {
            var response = new DataSourceResponse();

            if (request == null)
                return response;

            var data = string.IsNullOrEmpty(request.SearchText) ?
               _repo.GetAllSuppliersByName("") : _repo.GetAllSuppliersByName(request.SearchText);

            if (request.SupplierType > 0)
            {
                data = data.Where(x => x.TypeId == request.SupplierType);
            }

            //customer data filter by supplier id
            //factory fetch
            if (request.SupplierType == (int)Supplier_Type.Factory && request.SupplierId > 0)
            {
                data = data.Where(x => x.SuSupplierFactoryParents.Any(z => z.SupplierId == request.SupplierId));
            }
            //supplier fetch
            else if (request.SupplierType == (int)Supplier_Type.Supplier_Agent && request.FactoryId > 0)
            {
                data = data.Where(x => x.SuSupplierFactorySuppliers.Any(z => z.ParentId == request.FactoryId));
            }

            //get factory data by customer filter
            if (request.CustomerId > 0)
            {
                data = data.Where(x => x.SuSupplierCustomers.Any(z => z.CustomerId == request.CustomerId));
            }

            if (request.Id != null && request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }

            var supList = await data.Skip(request.Skip).Take(request.Take).Select(x => new CommonDataSource
            {
                Id = x.Id,
                Name = x.SupplierName
            }).OrderBy(o => o.Name).AsNoTracking().ToListAsync();


            if (supList == null || !supList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = supList;
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        //get the supplier datasource by Country
        public async Task<List<CommonDataSource>> GetSupplierByCountryDataSourceNew(CommonSupplierSourceRequest request)
        {
            var data = _repo.GetSuppliersSearchData().Where(x => x.TypeId != (int)Supplier_Type.Factory);

            if (request != null && request.CountryIds != null && request.CountryIds.Any())
            {
                data = data.Where(x => x.SuAddresses.Any(y => request.CountryIds.Contains(y.CountryId)));
            }
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.SupplierName != null && EF.Functions.Like(x.SupplierName, $"%{request.SearchText.Trim()}%"));
            }

            var dataSourceList = await data.Skip(request.Skip).Take(request.Take).
                            Select(x => new CommonDataSource() { Id = x.Id, Name = x.SupplierName }).
                            OrderBy(x => x.Name).AsNoTracking().ToListAsync();

            return dataSourceList;
        }

        //get the Supplier data by Id
        public async Task<DataSourceResponse> GetSupplierById(List<int> supplierIdList)
        {
            var data = await _repo.GetSupplierById(supplierIdList);

            if (!data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        //fetch the supplier list without any other filter dependency
        public async Task<DataSourceResponse> GetSupplierList(CommonDataSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

                var data = _repo.GetSupplierDataSourceList(request.SupplierType);

                //apply search text
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    data = data.Where(x => x.SupplierName != null && EF.Functions.Like(x.SupplierName, $"%{request.SearchText.Trim()}%"));
                }

                if (request.SupplierId > 0)
                {
                    data = data.Where(x => x.Id == request.SupplierId);
                }

                if (request.CustomerId > 0)
                {
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId.Value));
                }

                //execute the data
                var supList = await data.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.SupplierName
                }).Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

                if (supList != null && supList.Any())
                {
                    response.DataSourceList = supList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        //fetch the factory list without any other filter dependency
        public async Task<DataSourceResponse> GetFactoryrList(CommonDataSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

                var data = _repo.GetSupplierDataSourceList(request.SupplierType);

                //apply search text
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    data = data.Where(x => x.SupplierName != null && EF.Functions.Like(x.SupplierName, $"%{request.SearchText.Trim()}%"));
                }

                if (request.IdList != null && request.IdList.Any())
                {
                    data = data.Where(x => request.IdList.Contains(x.Id));
                }

                if (request.CustomerId > 0)
                {
                    data = data.Where(x => x.SuSupplierCustomers.Any(y => y.CustomerId == request.CustomerId.Value));
                }

                if (request.SupplierId != null && request.SupplierId > 0)
                {
                    data = data.Where(z => z.SuSupplierFactoryParents.Any(y => y.SupplierId == request.SupplierId));
                }

                //execute the data
                var supList = await data.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.SupplierName
                }).Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();


                if (supList != null && supList.Any())
                {
                    response.DataSourceList = supList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// get supplier list
        /// </summary>
        /// <returns></returns>
        public IQueryable<SuSupplier> GetSupplierList()
        {
            return _repo.GetAllSuppliersAndCountryList();
        }

        /// <summary>
        /// get factory country list by factory ids
        /// </summary>
        /// <param name="factoryIds"></param>
        /// <returns></returns>
        public async Task<List<CountryListModel>> GetFactoryCountryById(IEnumerable<int> factoryIds)
        {
            return await _repo.GetFactoryCountryById(factoryIds);
        }

        /// <summary>
        /// Get the edit booking suppliers by user type
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetEditBookingSuppliers(int? customerId, int bookingId)
        {
            var response = new List<CommonDataSource>();
            switch (_ApplicationContext.UserType)
            {

                case UserTypeEnum.InternalUser:
                    {
                        response = await _repo.GetEditBookingSuppliersByCustId(customerId, bookingId);
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        response = await _repo.GetEditBookingSuppliersByCustId(_ApplicationContext.CustomerId, bookingId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        response = await _repo.GetEditBookingSuppliersBySupId(_ApplicationContext.SupplierId);
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        response = await _repo.GetEditBookingSuppliersByfactId(_ApplicationContext.FactoryId, bookingId);
                        break;
                    }

            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SupplierExportDataResponse> GetSupplierSummaryExportDetails(SupplierSearchRequestNew request)
        {
            var supplierQueryData = GetSupplierFilterData(request);

            //iqueryable supplier id list
            var supplierIds = supplierQueryData.Select(x => x.Id);

            //get the address details
            var addressDetails = await _repo.GetSupplierAddressDataByIds(supplierIds);

            //customer tab
            var customerDetails = await _repo.GetSupplierCustomer(supplierIds);

            //contact
            var contactDetails = await _repo.GetSupplierContactDetailsBySupplierIdQuery(supplierIds);

            var factoryNameDetails = await _repo.GetFactoryDetailsBySupplierIdQuery(supplierIds);

            var suApiServiceDetails = await _repo.GetSuAPIServiceBySupplierIdQuery(supplierIds);

            var response = new SupplierExportDataResponse();

            var supplierDetails = await supplierQueryData
                .Select(x => new SupplierItemData()
                {
                    ContactPerson = x.ContactPerson,
                    Email = x.Email,
                    Fax = x.Fax,
                    GLCode = x.GlCode,
                    Name = x.SupplierName,
                    Phone = x.Phone,
                    RegionalName = x.LocalName,
                    Website = x.Website,
                    Type = x.Type.Type,
                    Status = x.Status.Name,
                    //Service = x.SuApiServices.Select(z => z.Service.Name).ToList(),
                    SupplierId = x.Id,
                    TypeId = x.TypeId
                }).AsNoTracking()
                .ToListAsync();

            if (!supplierDetails.Any())
            {
                return new SupplierExportDataResponse() { Result = SupplierSearchResult.NotFound };
            }

            response.SupplierExportList = addressDetails.Select(x => SupplierMap.MapSupplierSummaryExport(x, addressDetails,
                customerDetails, contactDetails, factoryNameDetails, supplierDetails, suApiServiceDetails)).ToList();

            if (!response.SupplierExportList.Any())
                response.Result = SupplierSearchResult.NotFound;
            else
                response.Result = SupplierSearchResult.Success;

            return response;
        }

        /// <summary>
        /// Get Supplier or factory address by id
        /// </summary>
        /// <param name="supplierFactoryId"></param>
        /// <returns></returns>
        public async Task<AddressDataSourceResponse> GetSupplierFactorAddressById(int supplierFactoryId)
        {
            var result = await _repo.GetSupplierOfficeAddressBylstId(new List<int>() { supplierFactoryId });
            return new AddressDataSourceResponse()
            {
                DataSourceList = result.Select(x => SupplierMap.MapSupplierAddress(x)),
                Result = DataSourceResult.Success
            };
        }

        /// <summary>
        /// Get the base supplier contact details by id
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public async Task<SupplierContactDataResponse> GetBaseSupplierContactData(int supplierId)
        {
            var response = new SupplierContactDataResponse() { Result = SupplierContactDataResult.NotFound };
            var supplierContacts = await _repo.GetBaseSupplierContactDataById(supplierId);
            if (supplierContacts.Any())
            {
                response.Result = SupplierContactDataResult.Success;
                response.ContactList = supplierContacts;
            }

            return response;
        }

        public async Task<SupplierDataResponse> GetExistSupplierDetails(SupplierDetails request)
        {
            var supplierDetails = await _repo.SupplierDetailsExists(request);
            var supplierResponse = new SupplierDataResponse();
            var masterinfo = await _userConfigRepo.GetMasterConfiguration();
            var entityName = masterinfo.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            if (supplierDetails.Count > 0)
            {
                var suContactEmail = request.SupplierContactList.Select(x => x.ContactEmail).Distinct().ToList();
                var suContactPhone = request.SupplierContactList.Where(x => x.Phone != null && x.Phone != "").Select(x => x.Phone).Distinct().ToList();
                foreach (var supplierDetail in supplierDetails)
                {
                    var supplierEntityIds = supplierDetail.SuEntities.Where(x => x.Active == true).Select(y => y.EntityId);
                    var newSupplierEntityIds = request.SupplierEntityIds.Where(x => !supplierEntityIds.Contains(x)).ToList();
                    supplierDetail.SupplierEntityIds = newSupplierEntityIds;
                    if (newSupplierEntityIds.Count > 0)
                    {
                        //var entityName = supplierDetail.SuEntities.Where(x => newSupplierEntityIds.Contains(x.EntityId)).Select(x => x.Entity.Name).FirstOrDefault();
                        supplierDetail.Remarks = "This " + supplierDetail.Type + " is not available in " + entityName + ". please add the entity to access it";
                        supplierDetail.IsView = false;
                    }
                    else
                    {
                        supplierDetail.IsView = true;
                    }
                    supplierDetail.IsSupplierEmailMatched = supplierDetail.Email == request.Email;
                    supplierDetail.IsContactEmailMatched = suContactEmail.Contains(supplierDetail.ContactEmail);
                    supplierDetail.IsSupplierPhoneMatched = !string.IsNullOrEmpty(request.Phone) && supplierDetail.Phone == request.Phone;
                    supplierDetail.IsContactPhoneMatched = suContactPhone.Contains(supplierDetail.ContactPhone);
                }
                supplierResponse.Data = supplierDetails;
                supplierResponse.Result = SupplierDataResult.Success;
            }
            else
            {
                supplierResponse.Result = SupplierDataResult.NotFound;
            }
            return supplierResponse;
        }

        public async Task<SaveSupplierResponse> UpdateSupplierEntity(SupplierData request)
        {
            var entity = await _repo.GetSupplierDetailById(request.Id);

            if (entity == null)
                return new SaveSupplierResponse { Result = SaveSupplierResult.SupplierIsNotFound };

            if (request.SupplierEntityIds != null && request.SupplierEntityIds.Any())
            {
                var SuContacts = entity.SuContacts.ToList();

                foreach (var supplierEntityId in request.SupplierEntityIds)
                {
                    var suEntity = new SuEntity()
                    {
                        Active = true,
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = supplierEntityId
                    };

                    entity.SuEntities.Add(suEntity);
                    _repo.AddEntity(suEntity);

                    foreach (var suContact in SuContacts)
                    {
                        suContact.SuContactEntityMaps = new List<SuContactEntityMap>();

                        var suContactEntity = new SuContactEntityMap
                        {
                            EntityId = supplierEntityId
                        };
                        suContact.SuContactEntityMaps.Add(suContactEntity);
                        _repo.AddEntity(suContactEntity);

                        entity.SuContacts.Add(suContact);
                    }
                }
            }

            //add the customer in supplier 
            if (request.CustomerList != null && request.CustomerList.Any())
            {
                var dbSuCustomerIds = entity.SuSupplierCustomers.Select(y => y.CustomerId).ToList();
                var newSuCustomer = request.CustomerList.Where(x => !dbSuCustomerIds.Contains(x.Id)).ToList();
                if (newSuCustomer != null && newSuCustomer.Any())
                {
                    foreach (var item in newSuCustomer)
                    {

                        var cust = new SuSupplierCustomer
                        {
                            CustomerId = item.Id,
                            CreditTerm = item.CreditTerm,
                            Code = item.Code?.Trim(),
                        };

                        entity.SuSupplierCustomers.Add(cust);
                        _repo.AddEntity(cust);
                    }
                }
            }
            int id = await _repo.EditSupplier(entity);

            if (id > 0)
            {
                return new SaveSupplierResponse { Id = entity.Id, Result = SaveSupplierResult.Success };

            }

            return new SaveSupplierResponse { Id = entity.Id, Result = SaveSupplierResult.SupplierIsNotFound };

        }
        /// <summary>
        /// get supplier code list by customer and supplier ids
        /// </summary>
        /// <param name="customerIds"></param>
        /// <param name="supplierIds"></param>
        /// <returns></returns>
        public async Task<List<SupplierCode>> GetSupplierCode(List<int> customerIds, List<int> supplierIds)
        {
            return await _repo.GetSupplierCode(customerIds, supplierIds);
        }

        /// <summary>
        /// This method is for EAQF save supplier
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>        
        public async Task<SaveSupplierResponse> SaveEaqfSupplier(EaqfSupplierDetails request, int type, int customerId, int? userId, int? supplierId = null)
        {
            SupplierDetails supplierDetails = new SupplierDetails();
            var suppliercontactlst = new List<suppliercontact>();
            var supplierAddressList = new List<Address>();
            var entityId = _filterService.GetCompanyId();
            supplierDetails.Id = request.Id;
            supplierDetails.Name = request.Name;
            supplierDetails.Email = request.Email;
            supplierDetails.TypeId = type;
            supplierDetails.Status = (int)SupplierStatus.Confirmed;
            supplierDetails.ContactPersonName = request.eaqfSupplierContacts.Select(x => x.ContactName).FirstOrDefault();
            supplierDetails.CompanyId = (int)Company.aqf;
            supplierDetails.ApiServiceIds = new[] { (int?)APIServiceEnum.Inspection }.ToList();
            supplierDetails.SupplierEntityIds = new[] { entityId }.ToList();
            supplierDetails.IsEAQF = true;
            supplierDetails.UserId = userId;


            var contacts = await _repo.GetSupplierContactById(request.Id);

            foreach (var contact in request.eaqfSupplierContacts)
            {
                if (contacts.FirstOrDefault(w => w.ContactName.ToLower() == contact.ContactName.Trim().ToLower()) == null)
                {
                    var supplierContact = new suppliercontact()
                    {
                        ContactName = contact.ContactName,
                        ContactEmail = request.Email,
                        Phone = contact.ContactPhone,
                        ContactAPIServiceIds = new[] { (int)APIServiceEnum.Inspection }.ToList(),
                        ApiEntityIds = new[] { (int)Company.aqf }.ToList(),
                        PrimaryEntity = (int)Company.aqf,
                        CustomerList = new List<DTO.Customer.CustomerItem>() { new DTO.Customer.CustomerItem() { Id = customerId } }
                    };
                    suppliercontactlst.Add(supplierContact);
                }
            }

            var existingContacts = contacts.Where(w => !suppliercontactlst.Any(a => a.ContactId == w.ContactId));
            suppliercontactlst.AddRange(existingContacts.Select(s => new suppliercontact()
            {
                ContactId = s.ContactId.Value,
                ContactName = s.ContactName,
                ContactEmail = s.ContactEmail,
                Phone = s.ContactPhone,
                ContactAPIServiceIds = new[] { (int)APIServiceEnum.Inspection }.ToList(),
                ApiEntityIds = new[] { (int)Company.aqf }.ToList(),
                PrimaryEntity = (int)Company.aqf,
                CustomerList = new List<DTO.Customer.CustomerItem>() { new DTO.Customer.CustomerItem() { Id = customerId } }
            }));

            supplierDetails.SupplierContactList = suppliercontactlst;
            var country = await _locationRepository.GetCountriesByAlpha2Code(request.Country);

            var city = await _locationRepository.GetCityByName(request.City);
            var provinceId = 0;
            var cityId = 0;
            if (city != null && city.CountryId == country.Id)
            {
                provinceId = city.ProvinceId;
                cityId = city.Id;
            }
            else
            {
                var provinces = _locationRepository.GetProvincesByCountryId(country.Id);
                provinceId = provinces.FirstOrDefault().Id;
                var cities = await _locationRepository.GetCityByProvinceIds(new List<int>() { provinceId });
                cityId = cities.FirstOrDefault().Id;
            }

            var supplierAddress = new Address()
            {
                Id = await _repo.GetAddressIdBySuppllierId(request.Id),
                CountryId = country.Id,
                CityId = cityId,
                AddressTypeId = (int)Supplier_Address_Type.HeadOffice,
                Way = string.IsNullOrEmpty(request.Address.Trim()) ? DefaultAddress : request.Address,
                RegionId = provinceId,
                LocalLanguage = string.IsNullOrEmpty(request.Address.Trim()) ? DefaultAddress : request.Address

            };

            supplierAddressList.Add(supplierAddress);
            supplierDetails.AddressList = supplierAddressList;

            supplierDetails.CustomerList = new List<SupplierMappedCustomer>() { new SupplierMappedCustomer { Id = customerId, Code = "" } };

            if (type == (int)Supplier_Type.Factory)
                supplierDetails.SupplierParentList = new List<SupplierItem>() { new SupplierItem() { Id = supplierId.GetValueOrDefault() } };
            var response = await Save(supplierDetails);

            return response;
        }

        public async Task<DataSourceResponse> GetSupplierLevelByCustomerId(int customerId)
        {
            var data = await _repo.GetSupplierLevelByCustomerId(customerId);
            if (data != null && data.Any())
            {
                var result = data.Select(x => new CommonDataSource()
                {
                    Id = x.Id,
                    Name = !string.IsNullOrEmpty(x.CustomName) ? x.CustomName : x.Level.ToString()
                }).ToList();
                return new DataSourceResponse() { DataSourceList = result, Result = DataSourceResult.Success };
            }
            else
            {
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
            }

        }

        public async Task<SupplierGradeResponse> GetGradeBySupplierIdAndCustomerIdAndBookingIds(SupplierGradeRequest request)
        {
            var response = new SupplierGradeResponse();
            if (request.CustomerId == 0)
            {
                response.Result = SupplierGradeResult.CustomerRequired;
                return response;
            }
            if (request.SupplierId == 0)
            {
                response.Result = SupplierGradeResult.SupplierRequired;
                return response;
            }
            if (request.BookingIds == null || !request.BookingIds.Any())
            {
                response.Result = SupplierGradeResult.BookingIdsRequired;
                return response;
            }

            var bookingServiceDateTos = await _inspectionBookingRepository.GetAllInspectionsQuery().Where(x => request.BookingIds.Contains(x.Id)).AsNoTracking().Select(y => y.ServiceDateTo).ToListAsync();

            var suGrades = await _repo.GetGradeByCustomerSupplier(request.CustomerId, request.SupplierId);

            //after service date to filter the take latest grade
            var grade = suGrades.Where(x => bookingServiceDateTos.Any(y => x.PeriodFrom <= y && y <= x.PeriodTo)).OrderByDescending(x => x.PeriodTo)
                .Select(x => !string.IsNullOrEmpty(x.CustomName) ? x.CustomName : x.Level).Distinct().FirstOrDefault();

            return new SupplierGradeResponse()
            {
                Grade = grade,
                Result = !string.IsNullOrWhiteSpace(grade) ? SupplierGradeResult.Success : SupplierGradeResult.NotFound
            };
        }
    }
}