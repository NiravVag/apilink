using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiTemplateRequest
    {
        public int Id { get; set;  }

        public  string Name { get; set;  }

        public IEnumerable<ModuleItem> SubmoduleList { get; set;  }

        public ModuleItem Module { get; set;  }

        public IEnumerable<TemplateFilter> TemplateFilterList { get; set;  }

        public IEnumerable<TemplateColumn> TemplateColumnList { get; set;  }

        public bool Shared { get; set; }

        public bool UseXlsFormulas { get; set;  }

        
    }

    public class KpiSavetemplateResponse
    {
        public int Id { get; set;  }

        public KpiSavetemplateResult Result { get; set;  }
    }

    public enum KpiSavetemplateResult
    {
        Success = 1,
        cannotSave = 2, 
        UnAuthorized = 3,
        ModuleIsrequired = 4,
        SubModuleIsrequired = 5,
        TemplateNameIsRequired = 6,
        TemplateColumnsRequired = 7
    }
}
