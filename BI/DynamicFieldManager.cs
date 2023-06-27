using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.DynamicFields;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class DynamicFieldManager : IDynamicFieldManager
    {
        #region Declartion
        private IDynamicFieldRepository _dynamicFieldRepository = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly DynamicFieldMap _dynamicfieldmap = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IConfiguration _configuration = null;
        #endregion

        #region Constructor
        public DynamicFieldManager(IDynamicFieldRepository dynamicFieldRepository, IAPIUserContext applicationContext, IConfiguration configuration, ITenantProvider filterService = null)
        {
            _dynamicFieldRepository = dynamicFieldRepository;
            _applicationContext = applicationContext;
            _dynamicfieldmap = new DynamicFieldMap();
            _configuration = configuration;
            _filterService = filterService;
        }
        #endregion

        /// <summary>
        /// Get the modules
        /// </summary>
        /// <returns></returns>
        public ModuleResponse GetModules()
        {
            var response = new ModuleResponse();
            var data = _dynamicFieldRepository.GetModules();
            if (data == null || !data.Any())
            {
                response.Result = ModuleResult.CannotGetModules;
            }
            response.ModuleList = data.Select(_dynamicfieldmap.GetModules).ToArray();
            response.Result = ModuleResult.Success;
            return response;
        }

        /// <summary>
        /// Get the control type response
        /// </summary>
        /// <returns></returns>
        public ControlTypeResponse GetControlTypes()
        {
            var response = new ControlTypeResponse();
            var data = _dynamicFieldRepository.GetControlTypes();
            if (data == null || !data.Any())
            {
                response.Result = ControlTypeResult.CannotGetControlTypes;
            }
            response.ControlTypeList = data.Select(_dynamicfieldmap.GetControlTypes).ToArray();
            response.Result = ControlTypeResult.Success;
            return response;
        }

        /// <summary>
        /// Get the ddl source type response
        /// </summary>
        /// <returns></returns>
        public DFDDLSourceTypeResponse GetDDLSourceTypes(int customerId)
        {
            var response = new DFDDLSourceTypeResponse();
            var data = _dynamicFieldRepository.GetDDLSourceTypes(customerId);
            if (data == null || !data.Any())
            {
                response.Result = DDLSourceTypeResult.CannotGetDDLSourceTypes;
            }
            response.DDLSourceTypeList = data.Select(_dynamicfieldmap.GetDDLSourceTypes).ToArray();
            response.Result = DDLSourceTypeResult.Success;
            return response;
        }

        /// <summary>
        /// Get the ddl source response
        /// </summary>
        /// <returns></returns>
        public DFDDLSourceResponse GetDDLSource(int typeId)
        {
            var response = new DFDDLSourceResponse();
            var data = _dynamicFieldRepository.GetDDLSource(typeId);
            if (data == null || !data.Any())
            {
                response.Result = DDLSourceResult.CannotGetDDLSource;
            }
            response.DDLSourceList = data.Select(_dynamicfieldmap.GetDDLSource).ToArray();
            response.Result = DDLSourceResult.Success;
            return response;
        }

        /// <summary>
        /// Save Dynamic fields for customer configuration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SaveDfCustomerConfigurationResponse> Save(DfCustomerConfiguration request)
        {
            if (request.Id == 0)
                return await AddDfCustomerConfiguration(request);

            return await UpdateDfCustomerConfiguration(request);
        }


        private async Task<SaveDfCustomerConfigurationResponse> AddDfCustomerConfiguration(DfCustomerConfiguration request)
        {
            var response = new SaveDfCustomerConfigurationResponse();
            if (request.ControlAttributeList.Any(x => x.AttributeId == (int)DfControlAttributeEnum.IsCascading))
            {
                if (request.ControlAttributeList.Where(x => x.AttributeId ==
                                     (int)DfControlAttributeEnum.IsCascading).FirstOrDefault()?.Value == "false")
                {
                    request.ControlAttributeList = request.ControlAttributeList.
                                                    Where(x => x.AttributeId != (int)DfControlAttributeEnum.ParentDropDown);
                }
                if (request.ControlAttributeList.Where(x => x.AttributeId ==
                                     (int)DfControlAttributeEnum.IsCascading).FirstOrDefault()?.Value == "true")
                {
                    if (request.ControlAttributeList.Where(x => x.AttributeId ==
                                      (int)DfControlAttributeEnum.ParentDropDown).FirstOrDefault()?.Value == null)
                    {
                        response.Result = DfCustomerConfgigurationResult.CascadingParentDropDownNotFound;
                        return response;
                    }
                }
            }
            //DfCuConfiguration entity = DynamicFieldMap.MapDFCustomerConfigurationEntity(request, _applicationContext.UserId);
            DfCuConfiguration entity = DynamicFieldMap.MapDFCustomerConfigurationEntity(request, _applicationContext.UserId, _filterService.GetCompanyId());
            if (request.ControlAttributeList != null)
            {
                foreach (var item in request.ControlAttributeList)
                {
                    var dynamicControlAttribute = _dynamicfieldmap.MapDFControlAttributes(item);
                    entity.DfControlAttributes.Add(dynamicControlAttribute);
                    _dynamicFieldRepository.AddEntity(dynamicControlAttribute);
                }
            }
            int id = await _dynamicFieldRepository.AddDfCustomerConfiguration(entity);
            response.Result = DfCustomerConfgigurationResult.Success;

            return response;
        }

        private async Task<SaveDfCustomerConfigurationResponse> UpdateDfCustomerConfiguration(DfCustomerConfiguration request)
        {
            var response = new SaveDfCustomerConfigurationResponse();
            var dfCustomerConfigurationEntity = _dynamicFieldRepository.GetDfCustomerConfiguration(request.Id);

            //DfCuConfiguration entity = DynamicFieldMap.UpdateDFCustomerConfigurationEntity(request, dfCustomerConfigurationEntity, _applicationContext.UserId);
            DfCuConfiguration entity = DynamicFieldMap.UpdateDFCustomerConfigurationEntity(request, dfCustomerConfigurationEntity, _applicationContext.UserId, _filterService.GetCompanyId());
            //Dynamic Field Control Attributes 
            if (request.ControlAttributeList != null)
            {
                var controlAttributeIds = request.ControlAttributeList.Where(x => x.Id > 0).Select(x => x.Id).ToArray();
                var lstcontrolAttributeToremove = new List<DfControlAttribute>();
                var DfControlAttributes = entity.DfControlAttributes.Where(x => !controlAttributeIds.Contains(x.Id)).ToList();
                foreach (var item in DfControlAttributes)
                {
                    item.Active = false;
                    //lstcontrolAttributeToremove.Add(item);
                    //entity.DfControlAttributes.Remove(item);
                }

                //_dynamicFieldRepository.RemoveEntities(lstcontrolAttributeToremove);

                foreach (var item in request.ControlAttributeList.Where(x => x.Id <= 0))
                    entity.DfControlAttributes.Add(new DfControlAttribute
                    {
                        Id = item.Id,
                        //Key = item.Key,
                        Value = item.Value,
                        Active = true,
                    });

                var lstControlAttributesToEdit = new List<DfControlAttribute>();
                foreach (var item in request.ControlAttributeList.Where(x => x.Id > 0))
                {
                    var dfControlAttribute = entity.DfControlAttributes.FirstOrDefault(x => x.Id == item.Id);

                    if (dfControlAttribute != null)
                    {
                        dfControlAttribute.Id = item.Id;
                        dfControlAttribute.ControlAttributeId = item.ControlAttributeId;
                        dfControlAttribute.Value = item.Value;
                        dfControlAttribute.Active = true;
                        lstControlAttributesToEdit.Add(dfControlAttribute);

                    }
                }

                if (lstControlAttributesToEdit.Count > 0)
                    _dynamicFieldRepository.EditEntities(lstControlAttributesToEdit);
            }
            int id = await _dynamicFieldRepository.EditDfCuConfiguration(entity);

            response.Id = entity.Id;

            response.Result = DfCustomerConfgigurationResult.Success;

            return response;
        }

        public async Task<EditDFCustomerConfigResponse> GetEditDfCustomerConfiguration(int id)
        {
            var response = new EditDFCustomerConfigResponse();
            var dfBaseData = await _dynamicFieldRepository.GetDfCustomerConfigBaseData(id);
            var dfAttributeData = await _dynamicFieldRepository.GetDfCustomerConfigAttributes(id);
            var isBooking = await _dynamicFieldRepository.CheckDFCustomerConfigInBooking(id);
            if (dfBaseData != null && dfAttributeData != null && dfAttributeData.Any())
            {
                response.DFCustomerConfiguration = _dynamicfieldmap.MapEditDfCustomerConfigData(dfBaseData, dfAttributeData, isBooking);
                response.Result = EditDFCustomerConfigResult.Success;
            }
            else
            {
                response.Result = EditDFCustomerConfigResult.NotFound;
            }

            return response;
        }

        public async Task<DfCustomerSearchResponse> SearchDfCustomerConfiguration(DfCustomerSearchRequest request)
        {
            var response = new DfCustomerSearchResponse { Index = request.index.Value, PageSize = request.pageSize.Value };

            var dfCustomerConfigSearchData = await _dynamicFieldRepository.GetDfCustomerConfigData();

            var inspectionConfigData = await _dynamicFieldRepository.GetInspectionDFCustomerConfig();


            if (request.moduleId != null)
            {
                dfCustomerConfigSearchData = dfCustomerConfigSearchData.Where(x => x.ModuleId == request.moduleId);
            }
            if (request.customerDataList != null && request.customerDataList.Any())
            {
                dfCustomerConfigSearchData = dfCustomerConfigSearchData.Where(x => request.customerDataList.Contains(x.CustomerId));
            }
            if (request.controlTypeDataList != null && request.controlTypeDataList.Any())
            {
                dfCustomerConfigSearchData = dfCustomerConfigSearchData.Where(x => request.controlTypeDataList.Contains(x.ControlTypeId));
            }

            if (inspectionConfigData != null && inspectionConfigData.Any())
            {
                inspectionConfigData = inspectionConfigData.Where(x => request.customerDataList.Contains(x.CustomerId)).ToList();
                CheckDFConfigInBooking(dfCustomerConfigSearchData, inspectionConfigData);
            }

            response.TotalCount = dfCustomerConfigSearchData.Count();

            if (response.TotalCount == 0)
            {
                response.Result = DfCustomerSearchResult.NotFound;
                return response;
            }

            int skip = (request.index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.Data = dfCustomerConfigSearchData.Skip(skip).Take(request.pageSize.Value).Select(x => x).ToArray();
            response.Result = DfCustomerSearchResult.Success;

            return response;

        }

        public void CheckDFConfigInBooking(IEnumerable<DfCustomerConfigSearchData> dfCustomerConfigSearchData,
                                    List<InspectionDFCustomerConfig> inspectionConfigData)
        {

            foreach (var data in dfCustomerConfigSearchData)
            {
                data.IsBooking = false;
                var bookingConfigData = inspectionConfigData.Where(x => x.ControlConfigId == data.id);
                if (bookingConfigData != null && bookingConfigData.Any())
                {
                    data.IsBooking = true;
                }
            }
        }

        /// <summary>
        /// GetDfCustomerConfiguration
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public DfCustomerConfigurationListResponse GetDfCustomerConfiguration(int customerId, int moduleId)
        {
            var response = new DfCustomerConfigurationListResponse();
            //Get the dynamic field customer configuration list
            var data = _dynamicFieldRepository.GetDfCustomerConfiguration(customerId, moduleId);
            //Map the dynamicfield customer configuration list
            response.dfCustomerConfigurationList = data.Select(x => _dynamicfieldmap.MapDfCustomerConfigurationRequest(x));
            response.Result = DfCustomerConfigurationListResult.Success;
            return response;
        }


        /// <summary>
        /// Get the ddl source type response
        /// </summary>
        /// <returns></returns>
        public DfControlTypeAttributesResponse GetDfControlTypeAttributes(int controlTypeId)
        {
            var response = new DfControlTypeAttributesResponse();
            var data = _dynamicFieldRepository.GetDFControlTypeAttributes(controlTypeId);
            if (data == null || !data.Any())
            {
                response.Result = DFControlTypeAttributeResult.CannotGetDFControlTypeAttribute;
            }
            response.DfControlTypeAttributes = data.Select(_dynamicfieldmap.GetDfControlsAttribute).ToArray();
            response.Result = DFControlTypeAttributeResult.Success;
            return response;
        }
        /// <summary>
        /// Get the dropdowns names configured for the customer 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<DFParentDropDownResponse> GetParentDropDownTypes(int customerId)
        {
            var response = new DFParentDropDownResponse();
            var parentDropdDownIds = await _dynamicFieldRepository.GetParentDropDownIds(customerId);
            if (parentDropdDownIds != null && parentDropdDownIds.Any())
            {
                var parentDropDownTypes = await _dynamicFieldRepository.GetParentDropDownTypes(parentDropdDownIds);
                if (parentDropDownTypes != null && parentDropDownTypes.Any())
                {
                    response.dfParentDDLList = parentDropDownTypes;
                    response.Result = DFParentDropDownResult.Success;
                }
                else
                    response.Result = DFParentDropDownResult.NotFound;
            }


            return response;
        }

        /// <summary>
        /// Remove DF Customer configuration data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DFCustomerConfigurationDeleteResponse> RemoveDFCustomerConfiguration(int id)
        {
            var dfCustomerConfiguration = _dynamicFieldRepository.GetDfCustomerConfiguration(id);

            if (dfCustomerConfiguration == null)
                return new DFCustomerConfigurationDeleteResponse { Id = id, Result = DFCustomerConfigurationDeleteResult.NotFound };

            await _dynamicFieldRepository.RemoveDFCustomerConfiguration(id);

            return new DFCustomerConfigurationDeleteResponse { Id = id, Result = DFCustomerConfigurationDeleteResult.Success };

        }

        /// <summary>
        /// Check Dynamic fields used in the booking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckDFCustomerConfigInBooking(int id)
        {
            return await _dynamicFieldRepository.CheckDFCustomerConfigInBooking(id);
        }
        /// <summary>
        /// Get Booking Dynamic fields by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<InspectionBookingDFDataResponse> GetBookingDFDataByBookingIds(IEnumerable<int> bookingIds)
        {
            var response = new InspectionBookingDFDataResponse();
            var dataSourceValues = new List<DFDataSourceRepo>();
            var bookingDFDataList = await _dynamicFieldRepository.GetBookingDFDataByBookingIds(bookingIds);
            if (bookingDFDataList != null && bookingDFDataList.Any())
            {
                var bookingDropDowndata = bookingDFDataList.Where(x => x.ControlType == (int)DfControlTypeEnum.DropDown);
                if (bookingDropDowndata != null && bookingDropDowndata.Any())
                {
                    var dataSourceTypeIds = bookingDropDowndata.Where(x => x.DFSourceType.HasValue).Select(x => x.DFSourceType.Value).Distinct();
                    dataSourceValues = await _dynamicFieldRepository.GetDropDownSourceByDataSourceTypeId(dataSourceTypeIds);
                }

                response.bookingDFDataList = _dynamicfieldmap.MapBookingDFData(bookingDFDataList, dataSourceValues);
                response.Result = InspectionBookingDFDataResult.Success;
            }
            else
            {
                response.Result = InspectionBookingDFDataResult.NotFound;
            }
            return response;
        }
        /// <summary>
        /// Get booking dynamic fields by booking query
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<InspectionBookingDFDataResponse> GetBookingDFDataByBookings(IQueryable<int> bookingIds)
        {
            var response = new InspectionBookingDFDataResponse();
            var dataSourceValues = new List<DFDataSourceRepo>();
            var bookingDFDataList = await _dynamicFieldRepository.GetBookingDFDataByBookings(bookingIds);
            if (bookingDFDataList != null && bookingDFDataList.Any())
            {
                var bookingDropDowndata = bookingDFDataList.Where(x => x.ControlType == (int)DfControlTypeEnum.DropDown);
                if (bookingDropDowndata != null && bookingDropDowndata.Any())
                {
                    var dataSourceTypeIds = bookingDropDowndata.Where(x => x.DFSourceType.HasValue).Select(x => x.DFSourceType.Value).Distinct().ToList();
                    dataSourceValues = await _dynamicFieldRepository.GetDropDownSourceByDataSourceTypeId(dataSourceTypeIds);
                }

                response.bookingDFDataList = _dynamicfieldmap.MapBookingDFData(bookingDFDataList, dataSourceValues);
                response.Result = InspectionBookingDFDataResult.Success;
            }
            else
            {
                response.Result = InspectionBookingDFDataResult.NotFound;
            }
            return response;
        }

        /// <summary>
        /// dynamic field gap customer configuration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public DfCustomerConfigurationListResponse DFGapCuConfiguration(DfCustomerConfigurationRequest request)
        {
            var response = new DfCustomerConfigurationListResponse();

            if (request == null)
                return new DfCustomerConfigurationListResponse 
                {
                    errors = new List<string>() { "Request is not valid" },
                    message = "Bad Request",
                    Result = DfCustomerConfigurationListResult.BadRequest
                };

            if (request.CustomerId <= 0)
                return new DfCustomerConfigurationListResponse
                {
                    errors = new List<string>() { "CustomerId is not valid" },
                    message = "Bad Request",
                    Result = DfCustomerConfigurationListResult.BadRequest
                };

            if (request.ModuleId <= 0)
                return new DfCustomerConfigurationListResponse
                {
                    errors = new List<string>() { "ModuleId is not valid" },
                    message = "Bad Request",
                    Result = DfCustomerConfigurationListResult.BadRequest
                };

            int gapCustomerId = 0;
            var gapCustomerIds = _configuration["CustomerGAP"].Split(',').Where(str => int.TryParse(str, out gapCustomerId)).Select(str => gapCustomerId).ToList();

            if (gapCustomerIds.Contains(request.CustomerId))
            {
                //Get the dynamic field customer configuration list
                var data = _dynamicFieldRepository.GetDfCustomerConfiguration(request.CustomerId, request.ModuleId);

                if (request.DataSourceTypeIds == null)
                    request.DataSourceTypeIds = new List<int>();

                data = data.Where(x => request.DataSourceTypeIds.Contains(x.DataSourceType.GetValueOrDefault()));
                
                //Map the dynamicfield customer configuration list
                response.dfCustomerConfigurationList = data.Select(x => _dynamicfieldmap.MapDfCustomerConfigurationRequest(x));
            }

            if (response.dfCustomerConfigurationList == null || !response.dfCustomerConfigurationList.Any())
                return new DfCustomerConfigurationListResponse { Result = DfCustomerConfigurationListResult.NotFound };

            response.Result = DfCustomerConfigurationListResult.Success;
            return response;
        }
        /// <summary>
        /// Get the booking audit product category for the inspection
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="dataSourceTypeId"></param>
        /// <returns></returns>
        public async Task<string> GetBookingAuditProductCategory(int bookingId, int? dataSourceTypeId)
        {
            return await _dynamicFieldRepository.GetBookingAuditProductCategory(bookingId, dataSourceTypeId);
        }

    }
}
