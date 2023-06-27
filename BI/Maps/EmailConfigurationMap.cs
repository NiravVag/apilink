using DTO.Common;
using DTO.CommonClass;
using DTO.EmailConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public  class EmailConfigurationMap: ApiCommonData
    {
        //map the email config details
        public  EmailConfigSummaryItem EmailConfigMap(EmailConfigParameterMap emailConfigDetails, EmailConfigSearchRepo data)
        {
            return new EmailConfigSummaryItem()
            {
                CustomerName = data.CustomerName,
                ServiceTypeName = string.Join(", ", emailConfigDetails.EmailConfigServiceTypeRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.ServiceTypeName).ToList()),
                DepartmentName = string.Join(", ", emailConfigDetails.EmailConfigDeptRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.DepartmentName).ToList()),
                EmailConfigId = data.EmailConfigId,
                FactoryCountryName = string.Join(", ", emailConfigDetails.EmailConfigFactoryCountryRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.FactoryCountryName).ToList()),
                OfficeName = string.Join(", ", emailConfigDetails.EmailConfigOfficeRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.OfficeName).ToList()),
                ProductcategoryName = string.Join(", ", emailConfigDetails.EmailConfigProductCategoryRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.ProductCategoryName).ToList()),
                ResultName = string.Join(", ", emailConfigDetails.EmailConfigResultRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.ResultName).ToList()),
                BrandName = string.Join(", ", emailConfigDetails.EmailConfigBrandRepo.Where(x => x.EmailConfigId == data.EmailConfigId).Select(x => x.BrandName).ToList()),
                ReportInEmail=data.ReportInEmail,
                ReportSendType=data.ReportSendType,
                Service=data.ServiceName,
                EmailTypeName = data.EmailTypeName,
                CreatedBy = data.CreatedBy,
                CreatedOn = data.CreatedOn.ToString(StandardDateFormat),
                UpdatedBy = data.UpdatedBy,
                UpdatedOn = data.UpdatedOn?.ToString(StandardDateFormat)
            };
        }

        //get email subject data
        public  CommonDataSource EmailSubjectDataMap(EmailSubjectRepo data)
        {
            return new CommonDataSource()
            {
                Id = data.Id,
                Name = data.Name
            };
        }
    }
}
