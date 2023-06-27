using Contracts.Managers;
using DTO.StartingPort;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUserPolicy")]
    [ApiController]
    public class StartingPortController : ControllerBase
    {
        private readonly IStartingPortManager _manager = null;

        public StartingPortController(IStartingPortManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        public async Task<StartingPortSaveResponse> SaveStartingPort(StartingPortRequest request)
        {
            return await _manager.SaveStartingPort(request);
        }

        [HttpPut("{id}")]
        public async Task<StartingPortSaveResponse> UpdateStartingPort(StartingPortRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new StartingPortSaveResponse() { Result = StartingPortResult.RequestNotCorrectFormat };
            }
            return await _manager.UpdateStartingPort(request);
        }

        [HttpDelete("{id}")]
        public async Task<StartingPortSaveResponse> DeleteStartingPort(int id)
        {
            if (!ModelState.IsValid)
            {
                return new StartingPortSaveResponse() { Result = StartingPortResult.RequestNotCorrectFormat };
            }
            return await _manager.DeleteStartingPort(id);
        }

        [HttpPost("get_starting_port")]
        public async Task<StartingPortResponse> GetStartingPortSummary(StartingPortSearchRequest request)
        {
            return await _manager.GetStartingPortSummary(request);
        }

        [HttpGet("{id}")]
        public async Task<StartingPortEditResponse> GetStartingPortDetails(int id)
        {
            return await _manager.GetStartingPortDetails(id);
        }
    }
}
