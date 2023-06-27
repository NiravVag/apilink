using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICacheManager _cacheManager = null;

        public AdminController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        [HttpGet("cache/clear")]
        public IActionResult ClearCache(string passwd)
        {
            if (passwd == "@dm1n@str@t0r;")
            {
                _cacheManager.Clear();

                return Ok();
            }

            return Unauthorized();

        }
    }
}