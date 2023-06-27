using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiTemplateListResponse
    {
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<KpiTemplateItem> Data { get; set;  }

        public KpiTemplateListResult Result { get; set; }
    }

    public class KpiTemplateItemResponse
    {
        public KpiTemplateItem Item { get; set; }

        public KpiTemplateListResult Result { get; set; }
    }

    public class KpiTemplateListRequest
    {

        public int IdSubModule { get; set;  }

        public int IdModule { get; set;  }

        public int Index { get; set; }

        public int PageSize { get; set; }

    }

    public class KpiTemplateItem
    {
        public int Id { get; set;  }

        public string Name { get; set;  }

        public IEnumerable<int> IdSubModuleList { get; set;  }

        public int IdModule { get; set;  }

        public string SubModuleName { get; set;  }

        public bool Shared { get; set; }

        public bool UseXlsFormulas { get; set; }

        public string UserName { get; set;  }

        public bool CanSave { get; set;  }
    }
    

    public abstract class Field
    {
        public int Id { get; set; }

        public int IdColumn { get; set; }

        public string ColumnName { get; set; }

        public FieldType Type { get; set; }

        public string OriginalLabel { get; set;  }

        public int? IdSubModule { get; set;  }
        public int? IdModule { get; set; }

        public string FieldName { get; set;  }

    }

    public enum KpiTemplateListResult
    {
        Success = 1, 
        NotFound = 2 
    }
}
