using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.OfficeLocation;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class OfficeLocationController : ControllerBase
    {
        private IOfficeLocationManager _officeLocationManager = null;
        public OfficeLocationController(IOfficeLocationManager officeLocationManager)
        {
            _officeLocationManager = officeLocationManager;
        }

        [HttpGet()]
        [Right("office-summary")]
        public OfficeSummaryResponse GetOfficeSummary()
        {
            return _officeLocationManager.GetOfficeSummary();
        }

        [HttpPost("[action]")]
        [Right("office-summary")]
        public OfficeSearchResponse GetSearchOffice([FromBody] OfficeSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return _officeLocationManager.GetOfficeSearchSummary(request);
        }

        [Right("edit-office")]
        [HttpGet("office/edit/{id}")]
        public async Task<EditOfficeResponse> GetEditOffice(int id)
        {
            return await _officeLocationManager.GetEditOfficeDetails(id);
        }

        [Right("edit-office")]
        [HttpGet("office/edit")]
        public async Task<EditOfficeResponse> GetEditOffice()
        {
            return await _officeLocationManager.GetEditOffice(null);
        }

        [Right("edit-office")]
        [HttpPost("office/save")]
        public async Task<SaveOfficeResponse> SaveOfficeAsync([FromBody]Office request)
        {
            return await _officeLocationManager.SaveOffice(request);
        }

        [Right("edit-office")]
        [HttpGet("getofficeforinternal")]
        public OfficeResponse GetOfficeForInternalUser()
        {
            return  _officeLocationManager.GetofficeforInternalUser();
        }

        [Right("edit-office")]
        [HttpGet("getLocationList")]
        public async Task<DataSourceResponse> GetOfficeLocationList()
        {
            return await _officeLocationManager.GetLocationList();
        }

        [HttpPost("get-office-by-office-access")]
        public async Task<DataSourceResponse> GetOfficeByOfficeAccess(CommonDataSourceRequest request)
        {
            return await _officeLocationManager.GetOfficesByOfficeAccess(request);
        }

        [HttpGet("getofficelocationdetails")]
        public async Task<DataSourceResponse> GetOfficeLocationDetails()
        {
            return await _officeLocationManager.GetOfficeLocationList();
        }
    }
}
