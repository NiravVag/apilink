using Components.Core.entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace FileGenerationComponent
{

    public class StandardObject
    {
        public BorderStyle BorderStyle { get; set; }
        public BorderStyle BorderLeftStyle { get; set; }
        public BorderStyle BorderRightStyle { get; set; }
        public BorderStyle BorderTopStyle { get; set; }
        public BorderStyle BorderBottomStyle { get; set; }
        
        public string BorderColor { get; set; }
        public string BorderLeftColor { get; set; }
        public string BordeRightrColor { get; set; }
        public string BorderBottomColor { get; set; }
        public string BorderTopColor { get; set; }

        public string Color { get; set; }
        public string BackGroundColor { get; set;  }

        public WeightStyle Weight { get; set;  }

        public string FontSize { get; set;  }

        public string FontFamily { get; set;  }

        public TextAlign Align { get; set; }

        public VeriticalAlign VerticalAlign { get; set;  }

        public bool WrapBreakWord { get; set;  }

        public bool HasStyle<T>() where T : StandardObject
        {
 
            foreach(var property in typeof(T).GetProperties())
            {
                if (property.Name == "Value")
                    continue; 

                if (property.GetValue(this) == null)
                    continue;

                if (property.PropertyType == typeof(string))
                {

                    string value = property.GetValue(this).ToString();

                    if (!string.IsNullOrWhiteSpace(value))
                        return true; 
                }
                else
                {
                    var value = property.GetValue(this);

                    if (value == null)
                        continue;

                    if(property.PropertyType.IsEnum)
                    {
                        var enumValue = Convert.ChangeType(value, property.PropertyType);

                        if (enumValue != null)
                            return true;
                    }

                    int? valueInt = value as int?;

                    if (valueInt == null)
                        continue;

                    if (valueInt.Value > 0)
                        return true; 
                }

            }

            return false;
        } 
    }

    public class FileObject
    {


    }



    public class ExcelFileObject : FileObject
    {
        public string Name { get; set;  }

        public IEnumerable<SheetObject> SheetList { get; set;  } 
    }


    public class ExcelJsonFileObject : FileObject
    {
        public object ModelRequest { get; set; }

        public Func<int, string, string> GetFilePath { get; set; }

     //   public IEnumerable<Image> ImageList { get; set; }

        public DataSet DataSource { get; set; }

        public IEnumerable<Variable> VariableList { get; set; }

        //public string FileName { get; set; }

        //public string ModelPath { get; set; }

        public Stream Stream { get; set;  }

    }

    public class PdfFileObject : FileObject
    {
        public string Title { get; set;  }

        public IEnumerable<Container> ContainerList { get; set;  }

        public class Container : StandardObject
        {
            public ContainerType Type { get; set; }

            public string Text { get; set;  }
             
            public IEnumerable<Container> ContainerList { get; set;  }
        }

        public enum ContainerType
        {
            Header = 1, 
            Footer = 2, 
            Page = 3,
            Container = 4,
            Table = 5
        }

    }

    public class SheetObject
    {
        public string Name { get; set;  }

        public IEnumerable<TableObject> TableList { get; set;  }
    }

    public class TableObject : StandardObject
    {

        public string BeginCell { get; set;  }

        public IEnumerable<CellObject>  Header { get; set;  } 

        public IEnumerable<RowObject> RowList { get; set;  }
    }

    public class RowObject : StandardObject
    {
        public IEnumerable<CellObject> CellList { get; set;  }
    }



    public class CellObject : StandardObject
    {
        public int? ColSpan { get; set;  }

        public int? RowSpan { get; set;  }

        public  double? Width { get; set;  }

        public string Value { get; set;  }

        public DataType Type { get; set;  }

        public string RefName { get; set;  }

        public string Formulas { get; set;  }

        public string Format { get; set;  }

        public string ImagePath { get; set;  }
        
        public byte[] Content { get; set;  }

        public string MimeType { get; set;  }
    }

    public enum BorderStyle
    {
        None = 0, 
        Solid = 1
    }

    public enum TextAlign {
        None = 0,
        Right  = 1, 
        Left =  2,
        Center = 3
    }

    public enum VeriticalAlign
    {
        None = 0,
        Top = 1,
        Middle = 2,
        Bottom = 3
    }


    public enum WeightStyle
    {
        None = 0 , 
        Bold = 1,
        BoldItalic = 2,
        Italic = 4,
        Underline = 5
    };



}
