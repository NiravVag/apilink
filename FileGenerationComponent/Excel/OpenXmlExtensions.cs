using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FileGenerationComponent.Excel
{
    public static class OpenXmlExtensions
    {
        public static MergeCells GetMergeCells(this Worksheet worksheet)
        {
            if (worksheet.Elements<MergeCells>() == null || !worksheet.Elements<MergeCells>().Any())
            {
                var sheetData = worksheet.Elements<SheetData>().FirstOrDefault();

                if (sheetData == null)
                    throw new Exception("cannot find any sheet data");

                worksheet.InsertAfter(new MergeCells(), sheetData);
            }

            return worksheet.Elements<MergeCells>().First();
        }


        public static Columns GetColumns(this Worksheet worksheet)
        {
            if (worksheet.Elements<Columns>() == null || !worksheet.Elements<Columns>().Any())
            {
                var sheetData = worksheet.Elements<SheetData>().FirstOrDefault();

                if (sheetData == null)
                    throw new Exception("cannot find any sheet data");

                worksheet.InsertBefore(new Columns(), sheetData);
            }

            return worksheet.Elements<Columns>().First();
        }

        public static (uint, string) GetColumnProperties(this Cell cell)
        {
            if (cell.Parent != null)
                return (((Row)cell.Parent).RowIndex, cell.CellReference.Value.Replace(((Row)cell.Parent).RowIndex.ToString(), ""));

            string x = cell.CellReference.Value[0].ToString();
            string y = cell.CellReference.Value.Substring(1, cell.CellReference.Value.Count() - 1);
            uint z = 0;

            while (y.Length > 0 && !uint.TryParse(y, out z))
            {
                x = x + y[0].ToString();
                y = y.Substring(1, y.Length - 1);
            }

            if (z <= 0)
                throw new Exception($"Cannot get Column Properties { cell.CellReference.Value}  ");

            return (z, x);
        }


        public static string Increment(this Cell cell)
        {
            string columnName = cell.GetColumnProperties().Item2;

            char[] str = columnName.ToArray();
            char x = str[columnName.Length - 1];
            char y = (char)(Convert.ToUInt16(x) + 1);
            str[columnName.Length - 1] = y;

            return new string(str);
        }


        public static void AddCell(this Row row, Cell cell)
        {
            if (row.Elements<Cell>().Any())
            {
                uint index = GetIndexOf(GetColumnProperties(cell).Item2);
                bool added = false;

                foreach (var currCell in row.Elements<Cell>())
                {
                    uint indexCurrent = GetIndexOf(GetColumnProperties(currCell).Item2);

                    if (index < indexCurrent)
                    {
                        row.InsertBefore(cell, currCell);
                        added = true;
                        break;
                    }
                }

                if (!added)
                    row.Append(cell);
            }
            else
                row.Append(cell);
        }

        public static uint GetIndexColumn(this Cell cell)
        {
            string cellReference = cell.CellReference;

            return GetIndexColumn(cellReference);
        }

        public static uint GetIndexColumn(this string cellReference)
        {
            //remove digits
            string columnReference = Regex.Replace(cellReference.ToUpper(), @"[\d]", string.Empty);

            int columnNumber = -1;
            int mulitplier = 1;

            foreach (char c in columnReference.ToCharArray().Reverse())
            {
                columnNumber += mulitplier * ((int)c - 64);

                mulitplier = mulitplier * 26;
            }

            return Convert.ToUInt32(columnNumber);
        }



        public static uint GetIndexOf(this string column, bool firstIs0 = false)
        {
            uint i = firstIs0 ? (uint)0 : (uint)1;
            string currentColumn = "A";

            while (currentColumn != column)
            {
                currentColumn = IncrementColumn(currentColumn);
                i++;
            }

            return i;
        }

        public static string IncrementColumn(this string columnName)
        {

            if (columnName[columnName.Length - 1] == 'Z')
            {
                if (columnName.Length == 1)
                    return "AA";
                else
                    return $"{IncrementColumn(columnName.Substring(0, columnName.Length - 1))}A";
            }
            else
            {
                char[] str = columnName.ToArray();
                char x = str[columnName.Length - 1];


                char y = (char)(Convert.ToUInt16(x) + 1);
                str[columnName.Length - 1] = y;

                return new string(str);
            }
        }
    }
}
