using Contracts.Managers;
using DTO.Expense;
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
    public class FoodAllowanceController : ControllerBase
    {
        private readonly IFoodAllowanceManager _manager = null;

        public FoodAllowanceController(IFoodAllowanceManager manager)
        {
            _manager = manager;
        }

        [HttpPost("save_food_allowance")]
        public async Task<SaveResponse> Save(FoodAllowance request)
        {
            return await _manager.Save(request);
        }

        [HttpPost("update_food_allowance")]
        public async Task<SaveResponse> Update(FoodAllowance request)
        {
            return await _manager.Update(request);
        }

        [HttpPost("delete_food_allowance")]
        public async Task<SaveResponse> Delete([FromBody]int id)
        {
            return await _manager.Delete(id);
        }

        [HttpPost("get_food_allowance")]
        public async Task<FoodAllowanceSummaryResponse> GetFoodAllowanceSummary(FoodAllowanceSummaryRequest request)
        {
            return await _manager.GetFoodAllowanceSummary(request);
        }

        [HttpGet("get_food_allowance_by_id/{id}")]
        public async Task<FoodAllowanceEditResponse> EditFoodAllowance(int id)
        {
            return await _manager.EditFoodAllowance(id);
        }

    }
}
