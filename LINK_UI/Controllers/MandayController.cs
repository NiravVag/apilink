
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Manday;
using DTO.OfficeLocation;
using DTO.References;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class MandayController : ControllerBase
    {
        private readonly IMandayManager _manager = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly IOfficeLocationManager _officeLocationManager = null;

        public MandayController(IMandayManager manager, IReferenceManager referenceManager, IOfficeLocationManager officeLocationManager)
        {
            _manager = manager;
            _referenceManager = referenceManager;
            _officeLocationManager = officeLocationManager;
        }

        [HttpPost("Search")]
        [Right("mandayDashboard")]
        public async Task<MandayDashboardResponse> GetMandayDashboardSearch([FromBody] MandayDashboardRequest request)
        {
            return await _manager.GetMandayDashboardSearch(request);
        }

        [HttpGet("GetService")]
        [Right("mandayDashboard")]
        public async Task<DataSourceResponse> GetService()
        {
            return await _manager.GetServices();
        }

        [HttpGet("GetOfficeLocations")]
        [Right("mandayDashboard")]
        public async Task<DataSourceResponse> GetOfficeLocations()
        {
            return await _manager.GetOfficeLocations();
        }

        [HttpPost("ManDayYearChart")]
        [Right("mandayDashboard")]
        public async Task<MandayYearChartResponse> GetMandayYearChart([FromBody] MandayDashboardRequest request)
        {
            return await _manager.GetMandayYearChart(request);
        }

        [HttpPost("ManDayCustomerChart")]
        [Right("mandayDashboard")]
        public async Task<MandayCustomerChartResponse> GetMandayCustomerChart([FromBody] MandayDashboardRequest request)
        {
            return await _manager.GetMandayCustomerChart(request);
        }

        [HttpPost("ManDayCountryChart")]
        [Right("mandayDashboard")]
        public async Task<MandayCountryChartResponse> GetMandayCountryChart([FromBody] MandayDashboardRequest request)
        {
            return await _manager.GetMandayCountryChart(request);
        }

        [HttpPost("ManDayEmployeeTypeChart")]
        [Right("mandayDashboard")]
        public async Task<MandayEmployeeTypeChartResponse> GetManDayEmployeeTypeChart([FromBody] MandayDashboardRequest request)
        {
            return await _manager.GetManDayEmployeeTypeChart(request);
        }

        [HttpPost("ManDayYearChartExport")]
        [Right("mandayDashboard")]
        public async Task<IActionResult> ExportMandayYearChart([FromBody] MandayDashboardRequest request)
        {
            var response = await _manager.ExportMandayYearChart(request);
            if (response == null && !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManDay/MandayYearChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ManDayCustomerChartExport")]
        [Right("mandayDashboard")]
        public async Task<IActionResult> ExportManDayCustomerChart([FromBody] MandayDashboardRequest request)
        {
            var response = await _manager.ExportMandayCustomerChart(request);
            if (response == null || !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManDay/MandayCustomerChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ManDayCountryChartExport")]
        [Right("mandayDashboard")]
        public async Task<IActionResult> ExportManDayCountryChart([FromBody] MandayDashboardRequest request)
        {
            var response = await _manager.ExportMandayCountryChart(request);
            if (response == null && !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManDay/ManDayCountryChartExport", response, Components.Core.entities.FileType.Excel);
        }

        [HttpPost("ManDayEmployeeTypeChartExport")]
        [Right("mandayDashboard")]
        public async Task<IActionResult> ExportManDayEmployeeTypeChart([FromBody] MandayDashboardRequest request)
        {
            var response = await _manager.ExportManDayEmployeeTypeChart(request);
            if (response == null && !response.Data.Any())
                return NotFound();
            return await this.FileAsync("Excel/ManDay/ManDayEmployeeTypeChartExport", response, Components.Core.entities.FileType.Excel);
        }
    }
}