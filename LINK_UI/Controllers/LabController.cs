using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.Lab;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class LabController : ControllerBase
    {
		private ILabManager _labManager = null;
		public LabController(ILabManager manager)
		{
			_labManager = manager;
		}

		[HttpGet("address/type")]
		[Right("edit-lab")]
		public LabAddressTypeResponse GetAllAddressType()
		{
			return _labManager.GetLabAddressTypeSummary();
		}

		[HttpGet("type")]
		[Right("edit-lab")]
		public LabTypeResponse GetAllLabType()
		{
			return _labManager.GetLabTypeSummary();
		}

		[HttpGet()]
		[Right("lab-summary")]
		public LabDetailsResponse GetAllLabDetail()
		{
			return _labManager.GetLabDetailsSummary();
		}

		[HttpDelete("{id}")]
		[Right("edit-lab")]
		public async Task<LabDeleteResponse> DeleteLab(int id)
		{
			return await _labManager.DeleteLab(id);
		}

		[HttpPost("save")]
		[Right("edit-lab")]
		public async Task<SaveLabResponse> SaveNewLabDetail([FromBody] LabDetails request)
		{
			return await _labManager.Save(request);
		}

        [HttpPut("save")]
        [Right("edit-lab")]
        public async Task<SaveLabResponse> UpdateLabDetail([FromBody] LabDetails request)
        {
            return await _labManager.Save(request);
        }

        [HttpGet("{id}")]
		[Right("edit-lab")]
		public async Task<EditLabResponse> GetLabDetails(int id)
		{
			return await _labManager.GetEditLabById(id);
		}

        [HttpPost()]
        [Right("lab-summary")]
        public LabSearchResponse Search([FromBody] LabSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return _labManager.GetLabSearchData(request);
        }

		[HttpPost("lab-address-by-lab-list")]
		public async Task<LabAddressListResponse> GetLabAddressList(LabAddressRequest request)
		{
			return await _labManager.GetLabAddressByLabIdList(request);
		}

		[HttpPost("lab-contacts-by-lab-list")]
		public async Task<LabContactsListResponse> GetLabContactList(LabContactRequest request)
		{
			return await _labManager.GetLabContactByLabListAndCustomerId(request);
		}

		[HttpPost("save-lab-address-list")]
		[Right("edit-lab")]
		public async Task<SaveLabAddressResponse> SaveLabAddressList(SaveLabAddressRequestData request)
		{
			return await _labManager.SaveLabAddressList(request);
		}
	}
}