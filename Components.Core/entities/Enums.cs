using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Components.Core.entities
{
    public enum SourceType
    {
        None = 0,
        Sql = 1
    }


    public class Variable
    {
      //  public int VariableMapId { get; set; }

        public string VariableName { get; set; }

        public string ColumnName { get; set; }

        public string VariableValue { get; set; }

        public int IndexSource { get; set; }

        public string ParentId { get; set; }

        public IEnumerable<Variable> VariableList { get; set; }

        public System.Data.DataTable DataTable { get; set; }

        public IEnumerable<Func<DataRow, string, string>> ParamOuters { get; set; }

        public IEnumerable<Func<string, string>> EmptyOuters { get; set; }

        public string PropertType { get; set; }

        public bool IsHorizontal { get; set; }

        public int MaxRows { get; set; }

        public bool IsGroupfield { get; set; }

        public bool IsFirstFieldGroup { get; set; }

        public bool IsBoldForFirstRow { get; set; }

        public bool UsedForFirstRowGroup { get; set; }

        public int IndexGroup { get; set; }

        public DataType DataType { get; set; }

        public ChartType ChartType { get; set; }

        public bool FixScale { get; set; }

        public string X { get; set;  }

        public string Y { get; set;  }

        public string OutPutName
        {
            get
            {

                switch (this.PropertType)
                {
                    case PropertTypeConst.Single:
                        return string.Format("[{0}]", this.VariableName);
                    case PropertTypeConst.List:
                        return string.Format("[{0}:{1}]", PropertTypeConst.List, this.VariableName);
                    case PropertTypeConst.Chart:
                        return string.Format("[{0}:{1}]", PropertTypeConst.Chart, this.VariableName);
                    case PropertTypeConst.Picture:
                        return string.Format("[{0}]", this.VariableName);
                    case PropertTypeConst.Matrix:
                        return string.Format("[{0}:{1}]", PropertTypeConst.Matrix, this.VariableName);

                }
                return string.Empty;

            }
        }

        public string EndOutPutName
        {
            get
            {

                switch (this.PropertType)
                {
                    case PropertTypeConst.Single:
                        return string.Format("[{0}]", this.VariableName);
                    case PropertTypeConst.List:
                        return string.Format("[/{0}:{1}]", PropertTypeConst.List, this.VariableName);
                    case PropertTypeConst.Chart:
                        return string.Format("[/{0}:{1}]", PropertTypeConst.Chart, this.VariableName);
                    case PropertTypeConst.Matrix:
                        return string.Format("[/{0}:{1}]", PropertTypeConst.Matrix, this.VariableName);

                }
                return string.Empty;

            }
        }

        public IEnumerable<Serie> Series { get; set; }

        public string Categories { get; set;  }
    
    }

    public class PropertTypeConst
    {
        public const string Single = "S";
        public const string List = "LIST";
        public const string Chart = "CHART";
        public const string Picture = "PICT";
        public const string ID = "ID";
        public const string Matrix = "MATRIX";
    }

    public class Functions
    {
        public const string Sum = "SUM";
        public const string Count = "COUNT";
        public const string Avg = "AVG";
    }

    public class Image
    {
        /// <summary>
        /// Get name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ge content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Get uid
        /// </summary>
        public Guid Uid { get; set; }

    }

    public enum DataType
    {
        String = 1,
        Number = 2,
        Picture = 3,
        Date = 4
    }

    public enum ChartType
    {
        Pie3DChart = 1,
        BarChart = 2,
        DoughnutChart =3
    }

}
