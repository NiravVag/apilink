using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiColumnListResponse
    {
        public IEnumerable<KpiColumnItem> Data { get; set;  }

        public KpiColumnListResult Result { get; set;  }
    }

    public class KpiColumnItem
    {
        public int Id { get; set;  }

        public  string FieldLabel { get; set;  }

        public string FieldName { get; set;  }

        public int? IdSubModule { get; set;  }

        public int? IdModule { get; set; }

        public FieldType Type { get; set;  }
       
    }

    public enum KpiColumnListResult
    {
        Success = 1,
        NotFound = 2
    }


}
