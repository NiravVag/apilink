using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Web;
using Contracts.Managers;
using DTO.CombineOrders;
using DTO.Inspection;
using DTO.InspectionPicking;
using DTO.Kpi;
using DTO.Lab;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    /// <summary>
    /// This API will handle Inspection Picking  Details
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspectionPickingController : ControllerBase
    {
        private readonly IInspectionPickingManager _manager = null;
        private readonly IKpiCustomManager _kpiManager = null;
        private readonly ILabManager _labManager = null;

        public InspectionPickingController(IInspectionPickingManager manager, ILabManager labManager, IKpiCustomManager kpiManager)
        {
            _manager = manager;
            _labManager = labManager;
            _kpiManager = kpiManager;
        }

        [HttpPost("SaveInspectionPicking/{bookingid}")]
        public async Task<SaveInspectionPickingResponse> SaveInspectionPicking(List<InspectionPickingData> inspectionPickingList, int bookingid)
        {
            return await _manager.SavePickingDetails(inspectionPickingList, bookingid);
        }

        [HttpGet("{bookingid}")]
        public async Task<InspectionPickingSummaryResponse> GetInspectionPickingList(int bookingid)
        {
            return await _manager.GetPickingDetails(bookingid);
        }

        [HttpGet("GetLabList/{customerId}")]
        public LabDataList GetLabList(int customerId)
        {
            return _labManager.GetLabDetailsByCustomerId(customerId);
        }

        [HttpGet("GetLabAddressListByLab/{labId}")]
        public async Task<LabAddressDataList> GetLabAddressListByLab(int labId)
        {
            return await _labManager.GetLabAddressByLabId(labId);
        }

        [HttpGet("GetLabContactList/{customerId}/{labId}")]
        public async Task<LabContactsDataList> GetLabContactList(int customerId, int labId)
        {
            return await _labManager.GetLabContactByLabIdAndCustomerId(customerId, labId);
        }

        [HttpGet("GetCustomerContacts/{customerid}")]
        //[Right("customer-summary")]
        public async Task<CustomerContactsResponse> GetCustomerContacts(int customerid)
        {
            return await _manager.GetCustomerContacts(customerid);
        }

        [HttpPost("ExportInspectionPicking")]
        public async Task<IActionResult> ExportTemplateSummary([FromBody] KpiRequest request)
        {
            var inspectionPickingList = await _kpiManager.GetInspectionPickingData(request);

            if (inspectionPickingList == null && inspectionPickingList.Any())
                return NotFound();

            return await this.FileAsync("InspectionPickingExport", inspectionPickingList, Components.Core.entities.FileType.Excel);
        }
    }
}