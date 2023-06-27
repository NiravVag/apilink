using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.EmailConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class EmailConfigurationController : ControllerBase
    {
        private readonly IEmailConfigurationManager _emailConfigManager = null;
        public EmailConfigurationController(IEmailConfigurationManager emailConfigManager)
        {
            _emailConfigManager = emailConfigManager;
        }

        [HttpGet("get-special-rule-data")]
        public async Task<DataSourceResponse> GetSpecialRuleList()
        {
            return await _emailConfigManager.GetSpecialRuleList();
        }

        [HttpGet("get-report-email-data")]
        public async Task<DataSourceResponse> GetReportInEmailList()
        {
            return await _emailConfigManager.GetReportInEmailList();
        }

        [HttpGet("get-email-size-data")]
        public async Task<DataSourceResponse> GetEmailSizeList()
        {
            return await _emailConfigManager.GetEmailSizeList();
        }

        [HttpGet("get-report-send-type/{emailTypeId}")]
        public async Task<DataSourceResponse> GetReportSendTypeListByEmailType(int emailTypeId)
        {
            return await _emailConfigManager.GetReportSendTypeListByEmailType(emailTypeId);
        }

        [HttpPost("get-email-subject-data")]
        public async Task<DataSourceResponse> GetTemplateMasterList(EmailSubRequest request)
        {
            return await _emailConfigManager.GetTemplateMasterList(request);
        }

        [HttpGet("get-staff-name-data")]
        public async Task<DataSourceResponse> GetStaffNameList()
        {
            return await _emailConfigManager.GetStaffNameList();
        }

        [HttpGet("get-email-send-data")]
        public async Task<DataSourceResponse> GetEmailSendTypeList()
        {
            return await _emailConfigManager.GetEmailSendTypeList();
        }

        [HttpPost()]
        public async Task<EmailConfigSaveReponse> Save(EmailConfiguration model)
        {
            return await _emailConfigManager.Save(model);
        }

        [HttpGet("{id}")]
        public async Task<EmailEditResponse> EditEmailSend(int id)
        {
            return await _emailConfigManager.EditDetails(id);
        }

        [HttpPost("search")]
        public async Task<EmailConfigurationSummaryResponse> Search(EmailConfigurationSummary request)
        {
            return await _emailConfigManager.Search(request);
        }

        [HttpDelete("{Id}")]
        public async Task<EmailConfigurationDeleteResponse> Delete(int Id)
        {
            return await _emailConfigManager.Delete(Id);
        }

        [HttpPost("get-email-file-name-data")]
        public async Task<DataSourceResponse> GetFileNameList(EmailSubRequest request)
        {
            return await _emailConfigManager.GetFileNameList(request);
        }

        [HttpGet("get-recipient-type-data/{emailTypeId}")]
        public async Task<DataSourceResponse> GetRecipientTypeList(int emailTypeId)
        {
            return await _emailConfigManager.GetRecipientTypeList(emailTypeId);
        }

        //[HttpGet("get-customer-contact-name-data")]
        //public async Task<DataSourceResponse> GetCustomerContactNameList()
        //{
        //    return await _emailConfigManager.GetCustomerContactNameList();
        //}

        //[HttpGet("get-customer-decision-data")]
        //public async Task<DataSourceResponse> GetCustomerDecisionList()
        //{
        //    return await _emailConfigManager.GetCustomerDecisionList();
        //}

        [HttpGet("get-recipient-data")]
        public async Task<DataSourceResponse> GetEsRefRecipientList()
        {
            return await _emailConfigManager.GetEsRefRecipientList();
        }
    }
}