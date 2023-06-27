using DTO.OfficeLocation;
using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationSummaryResponse
    {
        public IEnumerable<CommonDataSource> CustomerList { get; set;  }

        public IEnumerable<Office> OfficeList { get; set; }

        public IEnumerable<QuotStatus> StatusList { get; set;  }

        public IEnumerable<References.Service> ServiceList { get; set; }

        public QuotationSummaryResult Result { get; set; }
    }

    public class QuotStatus
    {
        public int Id { get; set;  }

        public string Label { get; set; }
    }

    public enum QuotationSummaryResult
    {
        Success = 1, 
        CustomerListNotFound = 2,
        CannotFindOfficeList = 3,
        CannotFindStatusList = 4,
        CannotFindServiceList = 5
    }
}
