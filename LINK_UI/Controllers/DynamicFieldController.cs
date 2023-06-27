using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.DynamicFields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DynamicFieldController : ControllerBase
    {
        private IDynamicFieldManager _dynamicFieldManager = null;

        public DynamicFieldController(IDynamicFieldManager dynamicManager)
        {
            _dynamicFieldManager = dynamicManager;
        }

        //[Right("province-summary")]
        [HttpGet("getmodules")]
        public ModuleResponse GetModules()
        {
            return _dynamicFieldManager.GetModules();
        }

        //[Right("province-summary")]
        [HttpGet("getcontroltypes")]
        public ControlTypeResponse GetControlTypes()
        {
            return _dynamicFieldManager.GetControlTypes();
        }

        //[Right("province-summary")]
        [HttpGet("getddlsourcetypelist/{customerId}")]
        public DFDDLSourceTypeResponse GetDDLSourceTypeList(int customerId)
        {
            return _dynamicFieldManager.GetDDLSourceTypes(customerId);
        }

        [HttpGet("getddlsourcelist/{typeId}")]
        public DFDDLSourceResponse GetDDLSourceList(int typeId)
        {
            return _dynamicFieldManager.GetDDLSource(typeId);
        }

        [HttpGet("getdfcustomerconfiguration/{id}")]
        public async Task<EditDFCustomerConfigResponse> GetDfCustomerConfiguration(int id)
        {
            return await _dynamicFieldManager.GetEditDfCustomerConfiguration(id);
        }
        /// <summary>
        /// Get the dynamic field configured for the customer and module
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [HttpGet("getdfconfiguration/{customerId}/{moduleId}")]
        public DfCustomerConfigurationListResponse GetDfCustomerConfiguration(int customerId,int moduleId)
        {

            return _dynamicFieldManager.GetDfCustomerConfiguration(customerId, moduleId);
        }

        [HttpPost("save")]
        public async Task<SaveDfCustomerConfigurationResponse> Save([FromBody] DfCustomerConfiguration request)
        {
            return await _dynamicFieldManager.Save(request);
        }

        [HttpPost("search")]
        public async Task<DfCustomerSearchResponse> Get(DfCustomerSearchRequest request)
        {
            var response = await _dynamicFieldManager.SearchDfCustomerConfiguration(request);
            return response;
        }

        [HttpGet("getcontroltypeattributes/{controlTypeId}")]
        public DfControlTypeAttributesResponse GetControlTypeAttributes(int controlTypeId)
        {
            return _dynamicFieldManager.GetDfControlTypeAttributes(controlTypeId);
        }

        [HttpGet("getparentdropdowntypes/{customerId}")]
        public async Task<DFParentDropDownResponse> GetParentDropDownTypes(int customerId)
        {
            return await _dynamicFieldManager.GetParentDropDownTypes(customerId);
        }

        [HttpPost("searchdfCustomerConfigSummary")]
        public async Task<DfCustomerSearchResponse> GetDfCustomerConfigSummary(DfCustomerSearchRequest request)
        {
            var response = await _dynamicFieldManager.SearchDfCustomerConfiguration(request);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<DFCustomerConfigurationDeleteResponse> DeleteDFCustomerConfiguration(int id)
        {
            return await _dynamicFieldManager.RemoveDFCustomerConfiguration(id);
        }

        [HttpGet("checkdfcustomerconfiginbooking/{id}")]
        public async Task<bool> CheckDFCustomerConfigInBooking(int id)
        {
            return await _dynamicFieldManager.CheckDFCustomerConfigInBooking(id);
        }


        [HttpPost("dfGapCuConfiguration")]
        public DfCustomerConfigurationListResponse DFGapCuConfiguration(DfCustomerConfigurationRequest request)
        {
            return _dynamicFieldManager.DFGapCuConfiguration(request);
        }
    }
}