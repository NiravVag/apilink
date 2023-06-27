using Contracts.Managers;
using DTO.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINK_UI.Controllers.EXTERNAL
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReportController : ExternalBaseController
    {
        private readonly IReportManager _manager = null;
        public CustomerReportController(IReportManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Get the Customer Report details based on the valid input
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="productRef"></param>
        /// <param name="po"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "CflUserPolicy")]
        public async Task<IActionResult> CustomerReportDetails([FromQuery]CustomerReportDetailsRequest request)
        {
            var response = await _manager.GetCustomerReportDetails(request);
            return BuildCommonResponse(response.statusCode, response);
        }
    }
}
