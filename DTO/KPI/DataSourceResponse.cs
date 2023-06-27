using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.KPI
{
    public class KpiDataSourceResponse
    {
        public IEnumerable<object> DataSource { get; set; }

        public string DataSourceFieldName { get; set; }

        public string DataSourceFieldValue { get; set; }

        public DataSourceResult Result { get; set;  }
    }

    public class ViewDataResponse
    {
        public IEnumerable<HtmlRow> Rows { get; set;  }

        public ViewDataResult Result { get; set; }

        public IEnumerable<TemplateColumn> ColumnList { get; set;  }

        public bool UseXls { get; set;  }


    }


    public class ViewData
    {
        public string TemplateName { get; set;  }

        public  bool UseXls { get; set;  }

        public IEnumerable<TemplateColumn> ColumnList { get; set; }

        public IEnumerable<object> Data { get; set;  }

        public IEnumerable<object> BasicData { get; set; }

    }

    public class HtmlRow
    {
        public List<HtmlCell> Cells { get; set; }

        public bool IsSum { get; set; }
    }

    public class HtmlCell
    {
        public int RowSpan { get; set; }

        public int ColSpan { get; set; }

        public string Value { get; set; }


        public FieldType Type { get; set; }

    }

    public enum DataSourceResult
    {
        Success =1, 
        NotFound= 2,
        FilterNotFound = 3
    }

    public enum ViewDataResult
    {
        Success = 1,
        NotFound = 2,
        TemplateNotFound = 3
    }


}
