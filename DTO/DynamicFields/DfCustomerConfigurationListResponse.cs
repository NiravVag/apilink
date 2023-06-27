using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{

    public class DfCustomerConfigurationListResponse
    {
        public IEnumerable<DfCustomerConfiguration> dfCustomerConfigurationList { get; set; }
        public DfCustomerConfigurationListResult Result { get; set; }
        public string message { get; set; }
        public List<string> errors { get; set; }

        public DfCustomerConfigurationListResponse()
        {
            this.errors = new List<string>();
        }
    }

    public class DfCustomerConfigurationRequest
    {
        public int CustomerId { get; set; }
        public int ModuleId { get; set; }
        public List<int> DataSourceTypeIds { get; set; }
    }

    public enum DfCustomerConfigurationListResult
    {
        Success = 1,
        NotFound = 2,
        Failed = 3,
        BadRequest = 4
    }
}
