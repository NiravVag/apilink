using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts.Managers;
using DTO.Email;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailsManager _emailsManager = null;
        public EmailController(IEmailsManager emailmanager)
        {
            _emailsManager = emailmanager;
        }
        //get default email recipient
        [HttpGet("GetDefaultRecipientByEmailType")]
        public async Task<DeafultEmailRecipientResponse> GetDefaultRecipientByEmailType()
        {
            return await _emailsManager.GetInternalDefaultRecipientByEmailType(EmailType.InspBookingRequest);
        }
        //get inspection related email recipient
        [HttpPost("GetInspEmailRecipient")]
        public async Task<InspEmailRecipientResponse> GetInspEmailRecipient([FromBody] InspEmailRecipientRequest request)
        {
            return await _emailsManager.GetInspEmailRecipient(request);
        }
        //get Audit related email recipient
        [HttpPost("GetAudEmailRecipient")]
        public async Task<AudEmailRecipientResponse> GetAudEmailRecipient([FromBody] AudEmailRecipientRequest request)
        {
            return await _emailsManager.GetAudEmailRecipient(request);
        }
    }
}