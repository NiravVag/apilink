using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiTemplateFilterListResponse
    {
        public IEnumerable<TemplateFilter> Data { get; set;  }

        public KpiTemplateFilterListResult Result { get; set;  }
    }

    public class TemplateFilter : Field
    {
        public bool IsMultiple { get; set; }

        public bool Required { get; set; }

        public bool SelectMultiple { get; set;  }

        public bool FilterLazy { get; set;  }

        public string Valuecolumn { get; set;  }

    }

    public enum KpiTemplateFilterListResult
    {
        Success  = 1, 
        NotFound = 2
    }
}
