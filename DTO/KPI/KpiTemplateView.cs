using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiTemplateViewResponse
    {
        public KpiTemplateViewResult Result { get; set;  }

       public KpiTemplateView Item { get; set;  }

    }

    public class KpiTemplateView
    {
        public int IdTemplate { get; set; }

        public string TemplateName { get; set;  }

        public string Module { get; set;  }

        public IEnumerable<TemplateFilter> FilterList { get; set; }

        public IEnumerable<TemplateColumn> ColumnList { get; set; }
    }

    public class KpiTemplateViewExport
    {
        public IEnumerable<TemplateColumn> ColumnList { get; set; }

        public IEnumerable<IDictionary<string, object>> DataSource { get; set; }
    }

    public enum KpiTemplateViewResult
    {
        Success = 1, 
        NotFound = 2 ,
    }

    public class KpiTemplateViewRequest
    {
        public int IdTemplate { get; set;  }

        public IEnumerable<FilterRequest> FilterValues { get; set;  }

        public bool ForExport { get; set;  }
    }

    public class FilterRequest
    {
        public int Id { get; set; }

        public object Value { get; set;  }
    }
}
