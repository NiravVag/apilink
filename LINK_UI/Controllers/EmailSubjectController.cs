using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.EmailSend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class EmailSubjectController : ControllerBase
    {
        private readonly IEmailSubjectManager _manager = null;

        private readonly IReferenceManager _refManager = null;

        public EmailSubjectController(IEmailSubjectManager manager, IReferenceManager refManager)
        {
            _manager = manager;
            _refManager = refManager;
        }

        [HttpGet("get-field-column-values")]
        public async Task<PreDefinedColSourceResponse> GetFieldColumnList()
        {
            return await _manager.GetFieldColumnList();
        }

        [HttpPost()]
        public async Task<SubConfigSaveResponse> Save(EmailSubjectConfig request)
        {
            return await _manager.Save(request);
        }

        [HttpGet("{Id}")]
        public async Task<SubConfigEditResponse> Edit(int Id)
        {
            return await _manager.Edit(Id);
        }

        [HttpPost("search")]
        public async Task<EmailSubConfigSummaryResponse> Search(EmailSubConfigSummary request)
        {
            return await _manager.Search(request);
        }

        [HttpDelete("{Id}")]
        public async Task<DeleteResponse> Delete(int Id)
        {
            return await _manager.Delete(Id);
        }
        [HttpGet("get-email-type-data")]
        public async Task<DataSourceResponse> GetEmailTypeList()
        {
            return await _manager.GetEmailTypeList();
        }
        [HttpGet("get-module-data")]
        public async Task<DataSourceResponse> GetModuleList()
        {
            return await _manager.GetModuleList();
        }
        [HttpGet("getDateFormats")]
        public async Task<DataSourceResponse> GetDateFormats()
        {
            return await _refManager.GetDateFormats();
        }
    }
}