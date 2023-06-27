using DTO.KPI;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.FileModels
{
    public class KpiModel
    {
        public IEnumerable<HtmlRow> Rows { get; set;  }
        
        public IEnumerable<TemplateColumn> ColumnList { get; set ; }

        public bool UseFormulas { get; set;  }

    }
}
