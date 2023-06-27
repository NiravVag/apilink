using Components.Core.entities;
using FileGenerationComponent.SourceProvider;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileGenerationComponent.Excel
{
    public class HtmlExcelProvider : HtmlProvider
    {

        protected override FileObject Translate(string html)
        {
            var lst = new List<SheetObject>();

            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(html);

            var htmlSheetList = pageDocument.DocumentNode.SelectNodes("//xls-sheet");

            if (htmlSheetList == null || !htmlSheetList.Any())
                throw new Exception("Cannot find any 'xls-sheet' tag in html ");

            foreach (var item in htmlSheetList)
            {
                var sheet = new SheetObject
                {
                    Name = item.GetAttributeValue("name", ""),
                    TableList = GetTables(item.SelectNodes("table"))
                };
                lst.Add(sheet);

            }

            return new ExcelFileObject
            {
                Name = "",
                SheetList = lst
            };
        }

        
        protected override  DataType GetDataType(HtmlNode node)
        {
            string type = node.GetAttributeValue("xls-type", "");

            switch (type.Trim().ToLower())
            {
                case "number":
                    return DataType.Number;
                case "picture":
                    return DataType.Picture;
                case "date":
                    return DataType.Date;
                default:
                    return DataType.String;
            }
        }

        protected override string GetTableBeginCell(HtmlNode nodeTable)
        {
            return nodeTable.GetAttributeValue("xls-begin", "");
        }

        protected override string GetRefName(HtmlNode node)
        {
            return node.GetAttributeValue("xls-ref", "");
        }

        protected override string GetFormula(HtmlNode node)
        {
            return node.GetAttributeValue("xls-form", "");
        }

        protected override string GetFormat(HtmlNode node)
        {
            return node.GetAttributeValue("xls-format", "");
        }
    }
}
