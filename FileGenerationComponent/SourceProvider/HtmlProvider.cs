using Components.Core.entities;
using FileGenerationComponent.Excel;
using FileGenerationComponent.PDF;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileGenerationComponent.SourceProvider
{
    public abstract class HtmlProvider : ISourceProvider
    {
        public Func<string, string> FuncGetMimeType { get; set; }
        protected Action<int, string> OnLog = null; 


        public string RootPath { get; set;  }

        public static HtmlProvider  GetInstance(FileType type, Action<int, string> onLog)
        {

            switch (type)
            {
                case FileType.Excel:
                    return new HtmlExcelProvider();
                case FileType.PDF:
                    return new HtmlPdfProvider();
                default:
                    return new HtmlExcelProvider();
            }
        }


        /// <summary>
        ///  Get file data 
        /// </summary>
        /// <param name="source">Source is html</param>
        /// <returns></returns>
        public FileObject GetFileObject(object source)
        {
            return Translate(source as string);
        }

        protected abstract FileObject Translate(string html);

        protected abstract string GetTableBeginCell(HtmlNode nodeTable);

        protected abstract DataType GetDataType(HtmlNode node);

        protected abstract string GetRefName(HtmlNode node);

        protected abstract string GetFormula(HtmlNode node);

        protected abstract string GetFormat(HtmlNode node);


        protected  virtual IEnumerable<TableObject> GetTables(HtmlNodeCollection tableList)
        {
            var lst = new List<TableObject>();

            if (tableList != null)
            {
                foreach (var table in tableList)
                {
                    var objTable = new TableObject
                    {
                        BeginCell = GetTableBeginCell(table)                       
                    };

                    SetProperties(objTable, table);

                    objTable.RowList = table.SelectNodes("tr")?.Select(x => GetRow(x, objTable));

                    lst.Add(objTable);
                }
            }

            return lst;
        }

        private RowObject GetRow(HtmlNode row, TableObject objParent)
        {
            var tdNodes = row.SelectNodes("td");

            if (tdNodes == null)
                tdNodes = row.SelectNodes("th");


            var objRow = new RowObject();
       

            SetProperties(objRow, row);
            MergeProperties(objParent,objRow);

            IEnumerable<CellObject> cellList = tdNodes == null ? null : tdNodes.Select(x => GetCell(x, objRow));
            objRow.CellList = cellList;
     
            return objRow;
        }
               
        private CellObject GetCell(HtmlNode cell, RowObject objParent)
        {
            var objCell = new CellObject();
            SetCellProperties(objCell, cell);
            MergeProperties(objParent,objCell);
            return objCell;
        }

        private void MergeProperties(StandardObject parent, StandardObject child)
        {
            foreach (var property in typeof(StandardObject).GetProperties())
            {
                if (!IsEmpty(property.GetValue(parent)) && IsEmpty(property.GetValue(child)))
                    property.SetValue(child, property.GetValue(parent));
            }
        }
        
        private bool IsEmpty(object obj)
        {
            if (obj == null)
                return true;

            if (obj.ToString().Trim() == "")
                return true;

            if (int.TryParse(obj.ToString(), out int objInt) && objInt <= 0)
                return true;

            return false;
        }


        private void SetCellProperties(CellObject obj, HtmlNode node)
        {
            //colspan : 
            string colSpan = node.GetAttributeValue("colspan", "");
            obj.ColSpan = string.IsNullOrEmpty(colSpan) ? (int?)null : Convert.ToInt32(colSpan);

            //rowSpan 
            string rowSpan = node.GetAttributeValue("rowspan", "");
            obj.RowSpan = string.IsNullOrEmpty(rowSpan) ? (int?)null : Convert.ToInt32(rowSpan);

            //Type 
            obj.Type = GetDataType(node);

            // get Value
            obj.Value = node.InnerText;

            // get ref name 
            obj.RefName = GetRefName(node);

            // get formulas 
            obj.Formulas = GetFormula(node);

            // get Format 
            obj.Format = GetFormat(node);

            //Width 
            var width = node.GetAttributeValue("width", "");
            obj.Width = string.IsNullOrEmpty(width) ? (double?)null : Convert.ToDouble(width.Replace("px",""));

            //Picture
            if(node.SelectNodes("img") != null && node.SelectNodes("img").Any())
            {
                //src = data:image/{extension};base64,{Content}
                var imgNode = node.SelectNodes("img").First();

                string attr = imgNode.GetAttributeValue("src","");

                if (attr.Contains("data:image/"))
                {
                    string[] tab = attr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (tab.Length == 2)
                    {
                        obj.Content = Convert.FromBase64String(tab[1]);
                        obj.MimeType = FuncGetMimeType(tab[0].Replace("data:image/", ""));
                    }
                }
                else
                {
                    obj.ImagePath = Path.Combine(RootPath, attr.Substring(1, attr.Length -1).Replace("/","\\"));
                    obj.MimeType = FuncGetMimeType( Path.GetExtension(obj.ImagePath));
                }

            }

                       
            // Other prperties
            SetProperties(obj, node);
        }

        protected  void SetProperties(StandardObject obj, HtmlNode node)
        {
            string style = node.GetAttributeValue("style", "");

            if (!string.IsNullOrEmpty(style))
            {
                string[] tab = style.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in tab)
                {
                    if (item.Contains("background-color"))
                        obj.BackGroundColor = GetStylePropery(item);

                    else if (item.Contains("border-left-color"))
                        obj.BorderLeftColor = GetStylePropery(item);
                    else if (item.Contains("border-right-color"))
                        obj.BordeRightrColor = GetStylePropery(item);
                    else if (item.Contains("border-top-color"))
                        obj.BorderTopColor = GetStylePropery(item);
                    else if (item.Contains("border-bottom-color"))
                        obj.BorderBottomColor = GetStylePropery(item);
                    else if (item.Contains("border-color"))
                        obj.BorderColor = GetStylePropery(item);
                    else if (item.Contains("color")) // color must be the last one if color list to not create confusion
                        obj.Color = GetStylePropery(item);

                    else if (item.Contains("border-left-style"))
                        obj.BorderLeftStyle = GetBorderStylePropery(item);
                    else if (item.Contains("border-right-style"))
                        obj.BorderRightStyle = GetBorderStylePropery(item);
                    else if (item.Contains("border-bottom-style"))
                        obj.BorderBottomStyle = GetBorderStylePropery(item);
                    else if (item.Contains("border-top-style"))
                        obj.BorderTopStyle = GetBorderStylePropery(item);
                    else if (item.Contains("border-style"))
                        obj.BorderStyle = GetBorderStylePropery(item);

                    else if (item.Contains("font-weight"))
                        obj.Weight = GetWightStylePropery(item);
                    else if (item.Contains("font-size"))
                        obj.FontSize = GetStylePropery(item);

                    else if (item.Contains("text-align"))
                        obj.Align = GetAlignProperty(item);
                    else if (item.Contains("vertical-align"))
                        obj.VerticalAlign = GetVerticalAlign(item);

                    else if (item.Contains("overflow-wrap") && GetStylePropery(item) == "break-word")
                        obj.WrapBreakWord = true; 

                }

            }

        }

        private string GetStylePropery(string styleName)
        {
            string[] tab = styleName.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (tab.Length >= 2)
                return tab[1];

            return "";
        }


        private BorderStyle GetBorderStylePropery(string styleName)
        {
            string value = GetStylePropery(styleName);

            switch (value.ToLower().Trim())
            {
                case "solid":
                    return BorderStyle.Solid;
                default:
                    return BorderStyle.None;
            }
        }
        private WeightStyle GetWightStylePropery(string styleName)
        {
            string value = GetStylePropery(styleName);

            switch (value.ToLower().Trim())
            {
                case "bold":
                    return WeightStyle.Bold;
                default:
                    return WeightStyle.None;
            }
        }


        private TextAlign GetAlignProperty(string attribute)
        {

            string value = GetStylePropery(attribute);

            switch (value.ToLower().Trim())
            {
                case "left":
                    return TextAlign.Left;
                case "right":
                    return TextAlign.Right;
                case "center":
                    return TextAlign.Center;
                default:
                    return TextAlign.None;
            }

        }
        private VeriticalAlign GetVerticalAlign(string attribute)
        {
            string value = GetStylePropery(attribute);

            switch (value.ToLower().Trim())
            {
                case "top":
                    return VeriticalAlign.Top;
                case "middle":
                    return VeriticalAlign.Middle;
                case "bottom":
                    return VeriticalAlign.Bottom;
                default:
                    return VeriticalAlign.None;
            }

        }




    }
}
