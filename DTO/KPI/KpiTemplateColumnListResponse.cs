using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiTemplateColumnListResponse
    {
        public IEnumerable<TemplateColumn> Data { get; set;  }

        public KpiTemplateColumnListResult Result { get; set;  }

    }

    public class TemplateColumn : Field
    {
        public bool SumFooter { get; set;  }

        public bool Group { get; set;  }

        public bool IsKey { get; set;  }     
        
        public string Valuecolumn { get; set;  }
    }

    public enum KpiTemplateColumnListResult
    {
        Success = 1,
        NotFound = 2
    }


}
