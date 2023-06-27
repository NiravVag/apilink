using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.AuditReport;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class AuditCusReportController : ControllerBase
    {
        private IAuditCusReportManager _manager = null;

        public AuditCusReportController(IAuditCusReportManager manager)
        {
            _manager = manager;
        }


        [Right("edit-audit")]
        [HttpPost("auditcussearch")]
        public async Task<AuditCusReportBookingDetailsResponse> SaveAudit([FromBody] AuditCusReportBookingDetailsRequest request)
        {
            if (request == null)
                return new AuditCusReportBookingDetailsResponse { Result = AuditCusReportBookingDetailsResult.RequestError };

            return await _manager.SearchAuditCusReport(request);

        }
    }
}