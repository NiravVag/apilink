using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Core.entities
{
    public class ConfigurationFile
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Extension { get; set; }

        public string StreamType { get; set; }

        public string ModelPath { get; set; }

        public string SourceText { get; set; }

        public string FileNameExt { get; set; }

        public string ConnectionStringName { get; set; }

        public IEnumerable<ConfVariable> Variables { get; set; }
    }

    public class ConfVariable
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int? IndexSource { get; set; }

        public string ColumnName { get; set; }

        public bool IsHorizontalCollection { get; set; }

        public bool GroupField { get; set; }

        public int? MaxRowsNumber { get; set; }

        public bool IsFirstRowBoldInGroup { get; set; }

        public bool UsedForFirstRowGroup { get; set; }

        public int? IndexGroup { get; set; }

        public DataType?  DataType { get; set;  }

        public IEnumerable<Serie> Series { get; set;  }

        public string Categories { get; set;  }

        public ChartType ChartType { get; set; }

        public IEnumerable<ConfVariable> Variables { get; set;  }

        public bool? FixScale { get; set;  }

        public string X { get; set;  }

        public string Y { get; set;  }
    }

    public class Serie
    {
        public string Name { get; set;  }

        public string Values { get; set;  }

        public string Color { get; set;  }
    }


}
