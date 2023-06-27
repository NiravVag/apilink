using DTO.Common;
using DTO.EmailSend;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps
{
    public  class EmailSubjectConfigMap: ApiCommonData
    {
        //map the template master details
        public  EmailSubConfigSummaryItem TemplateMasterMap(EmailSubConfigRepo data, List<int> emailSubjectIdList)
        {
            return new EmailSubConfigSummaryItem()
            {
                CustomerName = data.CustomerName,
                SubConfigId = data.SubConfigId,
                TemplateDisplayName = data.TemplateDisplayName,
                TemplateName = data.TemplateName,
                IsDelete = !(emailSubjectIdList.Contains(data.SubConfigId)),
                EmailType=data.EmailType,
                ModuleName=data.ModuleName
            };
        }
    }
}
