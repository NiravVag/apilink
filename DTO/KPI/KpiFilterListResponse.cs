using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiFilterListResponse
    {
        public IEnumerable<KpiFilterItem> Data { get; set;  }

        public KpiFilterListResult Result { get; set;  }
    }

    public class KpiFilterItem
    {
        public int Id { get; set; }

        public string FieldLabel { get; set; }

        public int? IdSubModule { get; set; }

        public int? IdModule { get; set; }

        public bool IsMultiple { get; set;  }

        public string FieldName { get; set;  }

        //public string DataSourceName { get; set;  }

        //public DataSourceType DataSourceType { get; set;  }

        //public string DataSourceFieldValue { get; set;  }

        //public string DataSourceFieldName { get; set;  }

        //public string DataSourceFieldCondition { get; set;  }

        //public string DataSourceFieldConditionValue { get; set;  }

        public bool Required { get; set;  }

        //public SignEquality SignEquality { get; set;  }
    }


    public enum KpiFilterListResult
    {
        Success = 1, 
        NoFound = 2
    }

}
