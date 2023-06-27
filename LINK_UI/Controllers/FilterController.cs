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
    public class FilterController : ControllerBase
    {

        private readonly ITenantProvider _manager;

        public FilterController(ITenantProvider manager)
        {
            _manager = manager;
        }

        // GET: api/Filter
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //  _manager.SetGlobalFilter("1");
            return new string[] { "value1", "value2" };
        }

        // GET: api/Filter/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            // _manager.ReSetGlobalFilter();

            return "success";
        }

        // POST: api/Filter
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Filter/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
