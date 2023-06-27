using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.BookingRuleContact;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class InspBookingRuleContactController : ControllerBase
    {
        private readonly IInspBookingRuleContactManager _bookingManager = null;
        public InspBookingRuleContactController(IInspBookingRuleContactManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        [Right("edit-booking")]
        [HttpGet("getInspBookingContactDetails/{factoryId}/{customerId}")]
        public async Task<BookingContactResponse> GetInspBookingContactDetails(int factoryId, int customerId)
        {
            return await _bookingManager.GetBookingContactInformation(factoryId, customerId);
        }
        [Right(new string[] { "edit-booking", "cancelreschedule-booking" })]
        [HttpGet("getInspBookingRuleDetails/{customerId}")]
        public async Task<InspBookingRuleResponse> GetInspBookingRuleDetails(int customerId, int? factoryId = null)
        {
            return await _bookingManager.GetInspBookingRules(customerId, factoryId);
        }
    }
}