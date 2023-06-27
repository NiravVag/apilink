using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Components.Core.entities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FileGenerationComponent.PPT;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace FileGenerationComponent.Excel
{
    internal class ExcelProvider : FileProvider
    {
        private readonly SpreadsheetDocument _spreadsheetDocument = null;
        private WorkbookPart _WorkbookPart = null;
        private Sheets _sheets = null;
        private SharedStringTablePart _sharedStringTablePart = null;
        private WorkbookStylesPart _workbookStylesPart = null;

        private IDictionary<DataType, CellValues> _dictTypes = new Dictionary<DataType, CellValues>() {
            { DataType.Number, CellValues.Number },
            { DataType.String, CellValues.String },
            { DataType.Picture, CellValues.String }
        };

        private IDictionary<string, ImagePartType> _dictImageTypes = new Dictionary<string, ImagePartType>() {
            { "image/bmp", ImagePartType.Bmp },
            {"application/x-msmetafile", ImagePartType.Emf },
            {"image/gif", ImagePartType.Gif },
            {"image/x-icon", ImagePartType.Icon },
            {"image/jpeg", ImagePartType.Jpeg },
            {"image/x-pcx", ImagePartType.Pcx },
            {"image/png", ImagePartType.Png },
            {"image/tiff", ImagePartType.Tiff }
        };

        public ExcelProvider(IConfiguration configuration) : base(configuration)
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(Stream, SpreadsheetDocumentType.Workbook);
        }



        protected override FileProperties GenerateDocument(FileObject source)
        {
            ExcelFileObject excelFileObject = source as ExcelFileObject;

            if (excelFileObject != null)
                return GenerateDocument(excelFileObject);

            ExcelJsonFileObject excelJsonObject = source as ExcelJsonFileObject;

            if (excelJsonObject != null)
                return GenerateDocument(excelJsonObject, DateTime.Now.ToString("ddMMyyyyHHmmss"));

            throw new Exception($"Cannot traduce FileObject to ExcelFileObject or ExcelJsonFileObject \n ");
        }


        protected FileProperties GenerateDocument(ExcelJsonFileObject source, string fileName)
        {
            using (var stream = new MemoryStream())
            {
                source.Stream.CopyTo(stream);

                using (var presentationDocument = CreateFilePackage(stream))
                    IntializeDocument(presentationDocument, source);

                stream.Seek(0, SeekOrigin.Begin); //scroll to stream start point
                fileName = ReplaceText(fileName, source.VariableList, source.DataSource.Tables[0].Rows.Count > 0 ? source.DataSource.Tables[0].Rows[0] : null);
                stream.Close();
                source.Stream.Close();

                return new FileProperties
                {
                    Content = stream.ToArray(),
                    FileType = FileType.PPT,
                    FileModelName = fileName,
                    MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };
            }

        }

        protected string ReplaceText(string paragraph, IEnumerable<Variable> variableList, DataRow row)
        {
            foreach (var variable in variableList.Where(x => x.PropertType == PropertTypeConst.Single))
                if (variable.ParamOuters != null)
                    foreach (var func in variable.ParamOuters)
                        paragraph = func(row, paragraph);



            return paragraph;
        }


        protected DocumentFormat.OpenXml.Packaging.OpenXmlPackage CreateFilePackage(MemoryStream stream)
        {
            return SpreadsheetDocument.Open(stream, true);
        }

        protected void IntializeDocument(OpenXmlPackage package, ExcelJsonFileObject fileObject)
        {
            Sheets sheets = ((SpreadsheetDocument)package).WorkbookPart.Workbook.GetFirstChild<Sheets>();
            var varTableList = fileObject.VariableList.Where(x => x.PropertType == PropertTypeConst.List);

            var listSheets = sheets.Descendants<Sheet>().ToList();

            foreach (var sheet in listSheets)
            {
                // Get WorkSheet template
                GetWorksheetPart(sheet, ((SpreadsheetDocument)package),
                    out WorkbookPart workbookPart,
                    out WorksheetPart worksheetPart,
                    out SharedStringTablePart stringTable,
                    out Worksheet worksheet);


                // Add new Worksheet
                AddNewWorkSheet(workbookPart, worksheet, worksheetPart, out SheetData newSheetData, out Worksheet newWorksheet, out DrawingsPart drawingsPart);


                ProcessRows(fileObject.VariableList, worksheet, stringTable, newWorksheet, newSheetData, fileObject.DataSource);

                AddColumnsProperties(worksheet, newWorksheet, newSheetData);


                DeleteTemplateSheet(sheet, worksheetPart, workbookPart);
            }


           ((SpreadsheetDocument)package).WorkbookPart.Workbook.WorkbookPart.Workbook.Save();

            // Close the document handle.
            ((SpreadsheetDocument)package).Close();
        }

        private void AddColumnsProperties(Worksheet worksheet, Worksheet newWorksheet, SheetData newSheetData)
        {

            if (worksheet.Elements<Columns>() != null && worksheet.Elements<Columns>().Any())
            {
                var lstcolmn = new List<Column>();

                foreach (var col in worksheet.Elements<Columns>().First().Elements<Column>())
                    lstcolmn.Add(new Column { Width = col.Width, Max = col.Max, Min = col.Min, CustomWidth = col.CustomWidth });

                var columns = new Columns(lstcolmn.ToArray());

                newWorksheet.InsertBefore(columns, newSheetData);
            }

        }

        private void DeleteTemplateSheet(Sheet sheet, WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {

            sheet.Remove();

            // Delete the worksheet part.
            //workbookPart.DeletePart(worksheetPart);

        }

        private void GetWorksheetPart(Sheet sheet, SpreadsheetDocument document, out WorkbookPart workbookPart, out WorksheetPart worksheetPart, out SharedStringTablePart stringTable, out Worksheet worksheet)
        {
            string relationshipId = sheet.Id.Value;

            workbookPart = document.WorkbookPart;
            stringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            worksheetPart = (WorksheetPart)workbookPart.GetPartById(relationshipId);
            worksheet = worksheetPart.Worksheet;
        }

        private void AddNewWorkSheet(WorkbookPart workbookPart, Worksheet worksheet, WorksheetPart worksheetPart, out SheetData newSheetData, out Worksheet newWorksheet, out DrawingsPart drawingsPart)
        {
            drawingsPart = null;

            var newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newSheetData = new SheetData();
            newWorksheet = new Worksheet();
            CloneElement<SheetViews>(worksheet, newWorksheet);
            CloneElement<SheetFormatProperties>(worksheet, newWorksheet);
            CloneElement(worksheet, newWorksheet, newSheetData);

            newWorksheetPart.Worksheet = newWorksheet;

            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            uint sheetId = 1;
            string sheetName = "Sheet" + sheetId;

            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                sheetName = sheets.Elements<Sheet>().First().Name;
            }


            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };

            sheets.Append(sheet);



            // clone other elements 
            CloneElement<PageMargins>(worksheet, newWorksheet);

            if (worksheetPart.SpreadsheetPrinterSettingsParts != null)
            {
                foreach (var item in worksheetPart.SpreadsheetPrinterSettingsParts)
                {

                    var printerSettingPart = newWorksheetPart.AddNewPart<SpreadsheetPrinterSettingsPart>();
                    printerSettingPart.FeedData(item.GetStream());

                    if (worksheet.ChildElements.OfType<PageSetup>().Any())
                    {
                        var pageSetup = worksheet.ChildElements.OfType<PageSetup>().First();

                        var newPageSetup = new PageSetup
                        {
                            Scale = pageSetup.Scale,
                            Orientation = pageSetup.Orientation,
                            Id = newWorksheetPart.GetIdOfPart(printerSettingPart)
                        };
                        newWorksheet.Append(newPageSetup);
                    }

                }


            }

            if (worksheetPart.DrawingsPart != null)
            {
                var dict = new Dictionary<string, string>();

                drawingsPart = newWorksheetPart.AddNewPart<DrawingsPart>();

                foreach (var imagePart in worksheetPart.DrawingsPart.ImageParts)
                {
                    var imgPart = drawingsPart.AddImagePart(imagePart.ContentType);
                    imgPart.FeedData(imagePart.GetStream());

                    dict.Add(worksheetPart.DrawingsPart.GetIdOfPart(imagePart), drawingsPart.GetIdOfPart(imgPart));
                }

                if (drawingsPart.WorksheetDrawing == null)
                    drawingsPart.WorksheetDrawing = new WorksheetDrawing();

                foreach (var element in worksheetPart.DrawingsPart.WorksheetDrawing.Elements<TwoCellAnchor>())
                {
                    var picture = element.Elements<DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture>().FirstOrDefault();

                    if (picture != null)
                    {
                        picture.BlipFill.Blip.Embed = dict[picture.BlipFill.Blip.Embed];
                        drawingsPart.WorksheetDrawing.Append(element.CloneNode(true));
                    }
                }


                if (!newWorksheet.ChildElements.OfType<Drawing>().Any())
                    newWorksheet.Append(new Drawing { Id = newWorksheetPart.GetIdOfPart(drawingsPart) });

                drawingsPart.WorksheetDrawing.Save();

            }

            newWorksheet.Save();
        }

        private void CloneElement<T>(Worksheet worksheet, Worksheet newWorksheet, T element = null) where T : OpenXmlElement
        {
            if (element != null)
                newWorksheet.Append(element);
            else if (worksheet.ChildElements.OfType<T>().Any())
            {
                var newElement = (T)(worksheet.ChildElements.OfType<T>().First().Clone());
                newWorksheet.Append(newElement);
            }
        }
        private void ProcessRows(IEnumerable<Variable> variableList, Worksheet worksheet, SharedStringTablePart stringTable, Worksheet newWorksheet, SheetData newSheetData, DataSet datasource)
        {
            uint rowNum = worksheet.Elements<SheetData>().First().Elements<Row>().First().RowIndex;
            uint originalRowNum = worksheet.Elements<SheetData>().First().Elements<Row>().First().RowIndex;
            uint diffRowNum = 0;

            var simpleVariables = variableList.Where(x => x.ParentId == null && x.PropertType != PropertTypeConst.List && x.PropertType != PropertTypeConst.Chart);

            IDictionary<string, string> dictMap = new Dictionary<string, string>();

            for (int rowIndex = 0; rowIndex < worksheet.Elements<SheetData>().First().Elements<Row>().Count(); rowIndex++)
            {
                var row = worksheet.Elements<SheetData>().First().Elements<Row>().ElementAt(rowIndex);

                diffRowNum = row.RowIndex - originalRowNum;

                originalRowNum = row.RowIndex;

                rowNum = rowNum + diffRowNum;

                foreach (var cell in row.Elements<Cell>())
                {
                    string cellValue = GetCellValueText(cell, stringTable);

                    if (cellValue.Contains("[LIST:"))
                    {
                        CloneList(cell, ref rowNum, ref rowIndex, variableList, "", 0, stringTable, worksheet, newWorksheet, newSheetData);
                        rowNum--;
                        break;
                    }

                    else
                    {
                        CloneItem(cell, ref rowNum, originalRowNum, simpleVariables, stringTable, datasource, newSheetData);
                        dictMap.Add(cell.CellReference.Value, (GetColumnName(cell) + rowNum));
                    }

                }
            }
            AddMergedCells(dictMap, worksheet, newWorksheet, newSheetData);
        }



        private void CloneList(Cell cell, ref uint indexNewRow, ref int rowTemplateIndex, IEnumerable<Variable> varList, string columnIdentName, int identifiant, SharedStringTablePart stringTable, Worksheet worksheet, Worksheet newWorksheet, SheetData newSheetData)
        {
            foreach (var variable in varList.Where(x => x.PropertType == PropertTypeConst.List))
            {
                if (ContainsCell(variable, cell, stringTable) && !variable.IsHorizontal)
                {
                    // found cell LIST
                    var lstRows = new List<Row>();
                    uint indexBegin;
                    uint indexEnd;
                    Cell EndList;

                    if (Contains(variable, worksheet, stringTable, out EndList, true))
                    {
                        // Get all rows between begin cell and end cell
                        indexBegin = ((Row)cell.Parent).RowIndex.Value;
                        indexEnd = ((Row)EndList.Parent).RowIndex.Value;


                        for (var i = indexBegin; i <= indexEnd; i++)
                        {
                            var newRow = GetRow(i, worksheet);
                            lstRows.Add(newRow);
                        }

                        rowTemplateIndex = rowTemplateIndex + Convert.ToInt32(indexEnd - indexBegin);
                        int index = 0;

                        IEnumerable<DataRow> rows = new HashSet<DataRow>();

                        if (!string.IsNullOrEmpty(columnIdentName) && identifiant > 0)
                        {
                            if (variable.DataTable.Columns.Contains(columnIdentName))
                                rows = variable.DataTable.AsEnumerable().Where(x => (int)x[columnIdentName] == identifiant);
                            else
                                rows = variable.DataTable.AsEnumerable();

                        }
                        else
                            rows = variable.DataTable.AsEnumerable();


                        foreach (DataRow row in rows)
                        {
                            int newidentifiant = 0;
                            string columnName = "";

                            if (variable.VariableList != null)
                            {
                                // Get value to filter 
                                var varIdentifiant = varList.FirstOrDefault(x => x.PropertType == PropertTypeConst.ID);
                                if (varIdentifiant != null)
                                {
                                    columnName = varIdentifiant.ColumnName;
                                    newidentifiant = (int)row[varIdentifiant.ColumnName];
                                }
                            }

                            index++;
                            CloneRow(lstRows, row, variable, ref indexNewRow, columnName, newidentifiant, stringTable, worksheet, newWorksheet, newSheetData);
                            //indexNewRow = Convert.ToUInt32(indexNewRow + lstRows.Count);
                        }

                        newWorksheet.Save();

                    }

                }
                else if (ContainsCell(variable, cell, stringTable) && variable.IsHorizontal)
                {
                    CloneListHorizontal(variable, cell, ref rowTemplateIndex, ref indexNewRow, worksheet, stringTable, newSheetData);
                }

            }
        }

        private void CloneListHorizontal(Variable variableList, Cell templateCell, ref int rowTemplateIndex, ref uint newIndex, Worksheet worksheet, SharedStringTablePart stringTable, SheetData newSheetData)
        {
            Cell EndList;
            uint indexBegin;
            uint indexEnd;

            // Excel sheet template must have End of tag and this Tag must be  in same column
            if (Contains(variableList, worksheet, stringTable, out EndList, true) && GetColumnName(templateCell) == GetColumnName(EndList))
            {
                indexBegin = ((Row)templateCell.Parent).RowIndex.Value;
                indexEnd = ((Row)EndList.Parent).RowIndex.Value;
                var lstCells = new List<Cell>();

                rowTemplateIndex = rowTemplateIndex + Convert.ToInt32(indexEnd - indexBegin);

                for (var i = indexBegin; i <= indexEnd; i++)
                {
                    var cell = GetCell(i, GetColumnName(templateCell), worksheet);
                    var newRow = GetRow(i, worksheet);
                    string columnName = GetColumnName(templateCell);

                    foreach (DataRow dtRow in variableList.DataTable.Rows)
                    {
                        var newCell = (Cell)cell.Clone();
                        newCell.CellReference = columnName + i;

                        string newColumnName;
                        string text = GetCellValueText(cell, stringTable);

                        CloneCell(variableList, newCell, text, dtRow, newRow, newIndex, stringTable, newSheetData, out newColumnName);

                        columnName = incrementColumn(columnName);

                    }

                    newIndex = newIndex + 1;
                }


            }
        }

        private string incrementColumn(string columnName)
        {
            char[] str = columnName.ToArray();
            char x = str[columnName.Length - 1];
            char y = (char)(Convert.ToUInt16(x) + 1);
            str[columnName.Length - 1] = y;

            return new string(str);
        }

        private Cell GetCell(uint rowIndex, string columnName, Worksheet worksheet)
        {
            var row = GetRow(rowIndex, worksheet);

            if (row != null)
                return row.Elements<Cell>().Where(x => GetColumnName(x) == columnName).FirstOrDefault();

            return null;
        }

        private void CloneRow(IEnumerable<Row> rowList, DataRow row, Variable variableList, ref uint indexClone, string columnName, int identifiant,
            SharedStringTablePart stringTable, Worksheet worksheet, Worksheet newWorksheet, SheetData newSheetData)
        {
            var lstCloned = rowList.ToList();

            IDictionary<string, string> dictMap = new Dictionary<string, string>();

            for (int index = 0; index < rowList.Count(); index++)
            {
                var Excelrow = rowList.ElementAt(index);

                foreach (var cell in Excelrow.Elements<Cell>())
                {
                    string text = "";
                    text = GetCellValueText(cell, stringTable);

                    if (!text.Contains(variableList.OutPutName) && text.Contains("[LIST:"))
                    {
                        CloneList(cell, ref indexClone, ref index, variableList.VariableList, columnName, identifiant, stringTable, worksheet, newWorksheet, newSheetData);
                        indexClone--;
                        break;
                    }
                    else if (text.Contains("[IMG:"))
                    {
                        // STATIC picutres 
                        // text = text.Replace("[IMG:", "").Replace("]", "");
                        //ReplaceStaticImages(indexClone, cell, Excelrow.RowIndex,text);
                    }
                    else
                    {
                        //int nb = 0;
                        //DataType type = DataType.String;
                        //foreach (var variable in variableList.VariableList.Where(x => x.PropertType == FileFactory.PropertType.Single))
                        //{
                        //    if (ContainsElement(GetCellValueText(cell), variable))
                        //    {
                        //        nb++;
                        //        type = variable.DataType;

                        //        if (variable.ParamOuters != null)
                        //            foreach (var func in variable.ParamOuters)
                        //                text = func(row, text);
                        //    }
                        //}

                        //text = text.Replace(variableList.OutPutName, "").Replace(variableList.EndOutPutName, "");

                        ////Update cells with pictures
                        //foreach (var variable in variableList.VariableList.Where(x => x.PropertType == FileFactory.PropertType.Picture))
                        //    ReplacePicture(text, variable, row, indexClone, cell, Excelrow.RowIndex, variableList.VariableList); 


                        //string newColumnName = cell.CellReference.Value.Replace(Excelrow.RowIndex.ToString(), "");

                        //if (nb > 1)
                        //    type = DataType.String;

                        //InsertCellInWorksheet(newColumnName, indexClone, text, cell, Excelrow, type);

                        string newColumnName;
                        CloneCell(variableList, cell, text, row, Excelrow, indexClone, stringTable, newSheetData, out newColumnName);
                        dictMap.Add(cell.CellReference.Value, (newColumnName + indexClone));
                    }

                }

                indexClone++;
            }

            AddMergedCells(dictMap, worksheet, newWorksheet, newSheetData);

        }


        private Row GetRow(uint rowIndex, Worksheet worksheet)
        {
            return worksheet.GetFirstChild<SheetData>().
              Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }

        private void CloneCell(Variable variableList, Cell cell, string text, DataRow row, Row excelRow, uint indexClone, SharedStringTablePart stringTable, SheetData newSheetData, out string newColumnName)
        {
            int nb = 0;
            DataType type = DataType.String;
            foreach (var variable in variableList.VariableList.Where(x => x.PropertType == PropertTypeConst.Single))
            {
                if (ContainsElement(GetCellValueText(cell, stringTable), variable))
                {
                    nb++;
                    type = variable.DataType;

                    if (variable.ParamOuters != null)
                        foreach (var func in variable.ParamOuters)
                            text = func(row, text);
                }
            }

            text = text.Replace(variableList.OutPutName, "").Replace(variableList.EndOutPutName, "");

            //Update cells with pictures
            //  foreach (var variable in variableList.VariableList.Where(x => x.PropertType == FileFactory.PropertType.Picture))
            //    ReplacePicture(text, variable, row, indexClone, cell, excelRow.RowIndex, variableList.VariableList);


            newColumnName = cell.CellReference.Value.Replace(excelRow.RowIndex.ToString(), "");

            if (nb > 1)
                type = DataType.String;

            InsertCellInWorksheet(newColumnName, indexClone, text, cell, excelRow, type, newSheetData);

        }


        private bool Contains(Variable variable, Worksheet worksheet, SharedStringTablePart stringTable, out Cell cell, bool isEnd = false)
        {

            foreach (var row in worksheet.Elements<SheetData>().First().Elements<Row>())
            {
                foreach (var currentcell in row.Elements<Cell>())
                {
                    if (GetCellValueText(currentcell, stringTable).Contains(isEnd ? variable.EndOutPutName : variable.OutPutName))
                    {
                        cell = currentcell;
                        return true;
                    }
                }
            }

            cell = null;
            return false;

        }

        private void CloneItem(Cell cell, ref uint indexNewRow, uint indexTemplateRow, IEnumerable<Variable> varList, SharedStringTablePart stringTable, DataSet dataSource, SheetData newSheetData)
        {
            string text = GetCellValueText(cell, stringTable);

            int nb = 0;
            DataType type = DataType.String;

            if (text.Contains("[IMG:"))
            {
                // STATIC picutres 
                text = text.Replace("[IMG:", "").Replace("]", "");
                // ReplaceStaticImages(ref indexNewRow, cell, indexTemplateRow, text);
                text = "";
            }

            foreach (var variable in varList)
            {

                if (ContainsElement(GetCellValueText(cell, stringTable), variable))
                {
                    nb++;
                    type = variable.DataType;

                    // var dataRows = _fileFactory.GetDataRowsUsedForBase()
                    if (dataSource != null && dataSource.Tables.Count > 0 && dataSource.Tables[0].Rows.Count > 0)
                    {

                        if (variable.ParamOuters != null)
                            foreach (var func in variable.ParamOuters)
                                text = func(dataSource.Tables[variable.IndexSource].Rows[0], text);
                    }
                    else
                        text = text.Replace(variable.OutPutName, "");
                }
            }

            if (nb > 1)
                type = DataType.String;

            InsertCellInWorksheet(GetColumnName(cell), indexNewRow, text, cell, ((Row)cell.Parent), type, newSheetData);

        }

        protected bool ContainsElement(string text, Variable variable)
        {
            return text.IndexOf(variable.OutPutName) > -1
                    || text.IndexOf(string.Format("[{0}:{1}", Functions.Sum, variable.VariableName)) > -1
                    || text.IndexOf(string.Format("[{0}:{1}", Functions.Count, variable.VariableName)) > -1
                    || text.IndexOf(string.Format("[{0}:{1}", Functions.Avg, variable.VariableName)) > -1;
        }

        private void InsertCellInWorksheet(string columnName, uint rowIndex, string text, Cell newcell, Row myrow, DataType type, SheetData newSheetData)
        {
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;

            if (newSheetData.Elements<Row>().Any(r => r.RowIndex == rowIndex))
                row = newSheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            else
            {
                row = new Row { RowIndex = rowIndex, StyleIndex = myrow.StyleIndex };

                newSheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Any(c => c.CellReference.Value == columnName + rowIndex))
            {
                var mycell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == columnName + rowIndex);
                mycell.CellValue = new CellValue(text);
                mycell.DataType = new EnumValue<CellValues>(_dictTypes[type]);

            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                var mycell = (Cell)(newcell.CloneNode(true));
                mycell.CellReference = cellReference;

                //// Set the cell value to be a numeric value of 100.
                mycell.CellValue = new CellValue(text);
                mycell.DataType = new EnumValue<CellValues>(_dictTypes[type]);
                row.Append(mycell);

            }
        }




        private void AddMergedCells(IDictionary<string, string> dictMap, Worksheet worksheet, Worksheet newWorksheet, SheetData newSheetData)
        {
            if (worksheet.Elements<MergeCells>().Any())
            {

                MergeCells newMergeCells;

                if (newWorksheet.Elements<MergeCells>() != null && newWorksheet.Elements<MergeCells>().Any())
                    newMergeCells = newWorksheet.Elements<MergeCells>().First();
                else
                {
                    newMergeCells = new MergeCells();
                    newWorksheet.InsertAfter(newMergeCells, newSheetData);
                }

                var mergeCells = worksheet.Elements<MergeCells>().First();


                foreach (var mergeCell in mergeCells.Elements<MergeCell>())
                {
                    string[] tab = mergeCell.Reference.Value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tab.Length == 2)
                    {
                        if (dictMap.Any(x => x.Key == tab[0]) && dictMap.Any(x => x.Key == tab[1]))
                        {
                            var newMergeCell = new MergeCell { Reference = string.Format("{0}:{1}", dictMap[tab[0]], dictMap[tab[1]]) };
                            newMergeCells.Append(newMergeCell);
                        }
                    }
                }
            }
        }

        private bool ContainsCell(Variable variable, Cell cell, SharedStringTablePart stringTable, bool isEnd = false)
        {
            return GetCellValueText(cell, stringTable).Contains(isEnd ? variable.EndOutPutName : variable.OutPutName);
        }


        protected FileProperties GenerateDocument(ExcelFileObject source)
        {
            // Add a WorkbookPart to the document.
            _WorkbookPart = _spreadsheetDocument.AddWorkbookPart();
            _WorkbookPart.Workbook = new Workbook();

            // Add Sheets to the Workbook.
            _sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

            // Shared string table
            _sharedStringTablePart = _WorkbookPart.AddNewPart<SharedStringTablePart>();
            _sharedStringTablePart.SharedStringTable = new SharedStringTable();
            _sharedStringTablePart.SharedStringTable.Save();

            // Stylesheet
            _workbookStylesPart = _WorkbookPart.AddNewPart<WorkbookStylesPart>();
            _workbookStylesPart.Stylesheet = new Stylesheet();



            // Fonts 
            var fonts = new Fonts { Count = 1 };
            fonts.Append(new Font
            {
                FontSize = new FontSize { Val = 11 },
                Color = new Color { Theme = 1 },
                FontName = new FontName { Val = "Calibri" },
                FontFamilyNumbering = new FontFamilyNumbering { Val = 2 },
                FontScheme = new FontScheme { Val = FontSchemeValues.Minor }
            });

            _workbookStylesPart.Stylesheet.Fonts = fonts;

            //Fills
            var fills = new Fills { Count = 2 };
            fills.Append(new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = PatternValues.None
                }
            });

            fills.Append(new Fill
            {
                PatternFill = new PatternFill
                {
                    PatternType = PatternValues.Gray125
                }
            });

            _workbookStylesPart.Stylesheet.Fills = fills;

            //Borders
            var borders = new Borders { Count = 1 };
            borders.Append(new Border
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder()
            });

            _workbookStylesPart.Stylesheet.Borders = borders;

            //CellStyleFormats
            var stylesFormat = new CellStyleFormats { Count = 1 };

            stylesFormat.Append(new CellFormat
            {
                NumberFormatId = 0,
                FontId = 0,
                FillId = 0,
                BorderId = 0
            });

            _workbookStylesPart.Stylesheet.CellStyleFormats = stylesFormat;


            //Cell formats 
            var cellFormats = new CellFormats { Count = 1 };

            cellFormats.Append(new CellFormat
            {
                NumberFormatId = 0,
                FontId = 0,
                FillId = 0,
                BorderId = 0,
                FormatId = 0
            });

            _workbookStylesPart.Stylesheet.CellFormats = cellFormats;


            if (source.SheetList == null || !source.SheetList.Any())
                throw new Exception("Cannot found any sheet in object ");

            foreach (var sheetObj in source.SheetList)
                _sheets.Append(GetSheet(sheetObj));

            _WorkbookPart.Workbook.Save();

            // Close the document.
            _spreadsheetDocument.Close();

            Stream.Seek(0, SeekOrigin.Begin); //scroll to stream start point

            //Create object Properties 
            return new FileProperties
            {
                FileType = FileType.Excel,
                Content = Stream.ToArray(),
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileModelName = $"file_{DateTime.Now.ToString("ddMMyyyyHHmmss")}",
                Name = ""
            };

        }

        private Sheet GetSheet(SheetObject sheetobject)
        {

            var lstMergedCells = new List<RefMergedCell>();
            var dictFormulas = new Dictionary<Cell, string>();
            var dictRefNames = new Dictionary<string, string>();
            var dictPictureList = new Dictionary<Cell, CellObject>();

            // sheet Data 
            var sheetData = new SheetData();

            // Add a WorksheetPart to the WorkbookPart.
            var worksheetPart = _WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            // Append a new worksheet and associate it with the workbook.
            var sheet = new Sheet()
            {
                Id = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = GetNewSheetId(),
                Name = sheetobject.Name
            };

            foreach (var table in sheetobject.TableList)
                AppendTable(sheetData, table, worksheetPart.Worksheet, ref lstMergedCells, ref dictRefNames, ref dictFormulas, ref dictPictureList);

            // merged cells 
            foreach (var item in lstMergedCells)
            {
                var cellSpan = sheetData.Elements<Row>().SelectMany(x => x.Elements<Cell>()).FirstOrDefault(x => x.CellReference == item.CellReference);

                if (cellSpan == null)
                {
                    var rowSpan = sheetData.Elements<Row>().FirstOrDefault(x => x.RowIndex == item.Index);

                    if (rowSpan == null)
                    {
                        rowSpan = new Row
                        {
                            RowIndex = item.Index
                        };

                        sheetData.InsertAt(rowSpan, Convert.ToInt32(item.Index - 1));
                    }

                    cellSpan = new Cell
                    {
                        CellReference = item.CellReference
                    };

                    //Index for insert:
                    rowSpan.AddCell(cellSpan);
                }

                SetStyleCell(cellSpan, item.CellObject);
            }

            // Apply formulas
            ApplyFormulas(dictFormulas, dictRefNames);

            // Add pictures
            foreach (var item in dictPictureList)
                SetPicture(worksheetPart, item.Value.ImagePath, item.Value.Content, item.Key, item.Value.MimeType);

            return sheet;
        }

        private void AppendTable(SheetData sheetData, TableObject table, Worksheet worksheet, ref List<RefMergedCell> mergedCells, ref Dictionary<string, string> dictRefNames, ref Dictionary<Cell, string> dictFormulas, ref Dictionary<Cell, CellObject> dictPictureList)
        {
            string firstColumnIndex = "";
            UInt32Value firstRowIndex;

            if (string.IsNullOrWhiteSpace(table.BeginCell))
            {
                firstColumnIndex = "A";
                firstRowIndex = 1;
            }
            else
            {
                string[] tab = table.BeginCell.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (tab.Length < 2)
                    throw new Exception($"cannot parse Begin Cell {table.BeginCell}");

                firstColumnIndex = tab[0];
                firstRowIndex = Convert.ToUInt32(tab[1]);
            }

            foreach (var rowObj in table.RowList)
            {
                AppendRow(sheetData, rowObj, firstRowIndex, firstColumnIndex, worksheet, ref mergedCells, ref dictRefNames, ref dictFormulas, ref dictPictureList);
                firstRowIndex++;
            }

        }

        private void ApplyFormulas(Dictionary<Cell, string> dictFormulas, Dictionary<string, string> dictRefNames)
        {
            if (dictFormulas.Any())
            {
                foreach (var item in dictFormulas)
                {
                    var keys = GetKeys(item.Value);

                    if (keys != null && keys.Any())
                    {
                        string form = item.Value;

                        foreach (var key in keys)
                        {
                            if (dictRefNames.TryGetValue(key, out string refCell))
                                form = form.Replace(key, refCell);
                            else
                                throw new Exception($"Cannot find key {key} for formula { item.Value} ");
                        }

                        item.Key.CellFormula = new CellFormula(form);
                        //item.Key.CellValue = new CellValue("0");
                    }

                }
            }

        }

        private IEnumerable<string> GetKeys(string text)
        {
            int index = text.IndexOf("[");
            int indexEnd = text.IndexOf("]");
            var lst = new List<string>();

            while (index >= 0 && indexEnd >= 0)
            {
                string key = text.Substring(index, indexEnd - index + 1);
                lst.Add(key);

                text = text.Replace(key, "");
                index = text.IndexOf("[");
                indexEnd = text.IndexOf("]");
            }

            return lst;
        }


        private void AppendRow(SheetData sheetData, RowObject rowObj, UInt32Value index, string indexColumn, Worksheet worksheet, ref List<RefMergedCell> mergedCells, ref Dictionary<string, string> dictRefNames, ref Dictionary<Cell, string> dictFormulas, ref Dictionary<Cell, CellObject> dictPictureList)
        {
            var row = sheetData.Elements<Row>().FirstOrDefault(x => x.RowIndex == index);

            if (row == null)
            {
                row = new Row { RowIndex = index };
                sheetData.Append(row);
            }


            foreach (var objCell in rowObj.CellList)
            {
                var mergedCellsCopy = mergedCells;

                while (mergedCells.Any(x => x.CellReference == $"{indexColumn}{index}"))
                {
                    var cellSpan = row.Elements<Cell>().FirstOrDefault(x => x.CellReference == $"{indexColumn}{index}");

                    if (cellSpan == null)
                    {
                        cellSpan = new Cell
                        {
                            CellReference = $"{indexColumn}{index}"
                        };

                        row.AddCell(cellSpan);
                    }

                    var objStyle = mergedCells.First(x => x.CellReference == $"{indexColumn}{index}");
                    SetStyleCell(cellSpan, objStyle.CellObject);

                    mergedCellsCopy.Remove(objStyle);
                    indexColumn = indexColumn.IncrementColumn();
                }

                mergedCells = mergedCellsCopy;

                if (objCell.RowSpan != null && objCell.RowSpan.Value > 1)
                {
                    var mergeCells = worksheet.GetMergeCells();

                    for (uint i = index.Value + 1; i <= index + objCell.RowSpan - 1; i++)
                        mergedCells.Add(new RefMergedCell
                        {
                            CellReference = $"{indexColumn}{i}",
                            Index = i,
                            CellObject = objCell,
                            ColumnName = indexColumn
                        });

                    var mergeCell = new MergeCell { Reference = $"{indexColumn}{index}:{indexColumn}{index + objCell.RowSpan - 1}" };
                    mergeCells.Append(mergeCell);
                }

                AppendCell(objCell, row, index, ref indexColumn, worksheet, sheetData, ref dictRefNames, ref dictFormulas, ref dictPictureList);
                indexColumn = indexColumn.IncrementColumn();
            }

        }

        private string GetCellValue(CellObject objCell)
        {
            if (string.IsNullOrEmpty(objCell.Value))
                return "";

            if (objCell.Type == DataType.Date)
            {
                string format = string.IsNullOrEmpty(objCell.Format) ? "dd/MM/yyyy" : objCell.Format;

                if (DateTime.TryParseExact(objCell.Value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    return dt.ToOADate().ToString();

            }


            return objCell.Value;
        }

        private void AppendCell(CellObject objCell, Row row, UInt32Value indexRow, ref string indexColumn, Worksheet worksheet, SheetData sheetData, ref Dictionary<string, string> dictRefNames, ref Dictionary<Cell, string> dictFormulas, ref Dictionary<Cell, CellObject> dictPictureList)
        {
            string cellReference = $"{indexColumn}{indexRow}";
            string firstCellreference = cellReference;
            string firstCellColumn = indexColumn;

            var cell = row.Elements<Cell>().FirstOrDefault(x => x.CellReference == cellReference);

            if (cell == null)
            {
                cell = new Cell
                {

                    CellValue = new CellValue { Text = GetCellValue(objCell) },
                    CellReference = cellReference
                };

                if (_dictTypes.TryGetValue(objCell.Type, out CellValues value))
                    cell.DataType = value;

                row.AddCell(cell);
            }

            if (!string.IsNullOrWhiteSpace(objCell.RefName))
                dictRefNames.Add($"[{objCell.RefName.Trim()}]", cellReference);

            if (!string.IsNullOrWhiteSpace(objCell.Formulas))
                dictFormulas.Add(cell, objCell.Formulas.Trim());

            SetStyleCell(cell, objCell);

            if (objCell.Width != null && objCell.Width.Value > 0)
            {
                var columns = worksheet.GetColumns();

                uint index = GetIndexOf(firstCellColumn);

                var column = columns.Elements<Column>().FirstOrDefault(x => x.Min <= index && x.Max >= index);

                if (column == null)
                {
                    column = new Column { Min = index, Max = index, Width = ((objCell.Width.Value - 12) / 7d + 1), CustomWidth = true };
                    columns.Append(column);
                }
                else
                {
                    double value = ((objCell.Width.Value - 12) / 7d + 1);

                    if (value > column.Width)
                        column.Width = value;
                }
            }
            if (objCell.ColSpan != null && objCell.ColSpan.Value > 1)
            {

                for (int i = 0; i < objCell.ColSpan.Value - 1; i++)
                {
                    indexColumn = indexColumn.IncrementColumn();
                    cellReference = $"{indexColumn}{indexRow}";

                    var cellSpan = row.Elements<Cell>().FirstOrDefault(x => x.CellReference == cellReference);

                    if (cellSpan == null)
                    {
                        cellSpan = new Cell
                        {
                            CellReference = cellReference
                        };

                        row.AddCell(cellSpan);
                    }

                    SetStyleCell(cellSpan, objCell);

                }

                var mergeCells = worksheet.GetMergeCells();
                mergeCells.Append(new MergeCell { Reference = $"{firstCellreference}:{cellReference}" });

            }

            if ((objCell.Content != null && objCell.Content.Any()) || !string.IsNullOrWhiteSpace(objCell.ImagePath))
                dictPictureList.Add(cell, objCell);
        }



        private void SetStyleCell(Cell cell, CellObject objCell)
        {

            if (objCell.HasStyle<CellObject>())
            {

                // Add cellFormat
                var cellFormat = new CellFormat();
                _workbookStylesPart.Stylesheet.CellFormats.Append(cellFormat);

                if (!string.IsNullOrWhiteSpace(objCell.Color) || objCell.Weight != WeightStyle.None || !string.IsNullOrEmpty(objCell.FontSize))
                {
                    var font = new Font
                    {
                        FontSize = new FontSize { Val = 11 },
                        FontName = new FontName { Val = "Calibri" },
                        FontFamilyNumbering = new FontFamilyNumbering { Val = 2 },
                        FontScheme = new FontScheme { Val = FontSchemeValues.Minor },
                        Color = new Color { Theme = 1 }
                    };

                    if (!string.IsNullOrWhiteSpace(objCell.Color))
                        font.Color = new Color { Rgb = new HexBinaryValue() { Value = objCell.Color.Replace("#", "") } };

                    if (objCell.Weight == WeightStyle.Bold)
                        font.Bold = new Bold();

                    if (!string.IsNullOrEmpty(objCell.FontSize) && double.TryParse(objCell.FontSize.Replace("px", ""), out double value))
                        font.FontSize = new FontSize { Val = value };

                    _workbookStylesPart.Stylesheet.Fonts.Append(font);

                    cellFormat.FontId = (uint)_workbookStylesPart.Stylesheet.Fonts.Count++;

                }


                if (!string.IsNullOrWhiteSpace(objCell.BackGroundColor))
                {
                    var fill = new Fill();
                    PatternFill patternFill = new PatternFill() { PatternType = PatternValues.Solid };

                    ForegroundColor foregroundColor1 = new ForegroundColor() { Rgb = new HexBinaryValue() { Value = objCell.BackGroundColor.Replace("#", "") } };
                    patternFill.Append(foregroundColor1);

                    fill.Append(patternFill);
                    _workbookStylesPart.Stylesheet.Fills.Append(fill);

                    cellFormat.FillId = (uint)_workbookStylesPart.Stylesheet.Fills.Count++;
                }

                if (objCell.Type == DataType.Date)
                    cellFormat.NumberFormatId = 14;

                uint indexNumberFormat = 164;
                if (objCell.Type == DataType.Number && !string.IsNullOrWhiteSpace(objCell.Format))
                {
                    var numberingFormat = new NumberingFormat
                    {
                        FormatCode = StringValue.FromString(objCell.Format.Trim()),
                        NumberFormatId = indexNumberFormat
                    };

                    if (_workbookStylesPart.Stylesheet.NumberingFormats == null)
                        _workbookStylesPart.Stylesheet.NumberingFormats = new NumberingFormats { Count = 0 };

                    _workbookStylesPart.Stylesheet.NumberingFormats.Append(numberingFormat);

                    _workbookStylesPart.Stylesheet.NumberingFormats.Count++;

                    cellFormat.NumberFormatId = indexNumberFormat;

                    indexNumberFormat++;
                }

                if (objCell.WrapBreakWord)
                {
                    cellFormat.ApplyAlignment = new BooleanValue(true);
                    cellFormat.Alignment = new Alignment { WrapText = new BooleanValue(true) };
                }

                // Generate border 
                var border = new Border();

                AddDirectionBorder<LeftBorder>(border, objCell.BorderLeftColor, objCell.BorderColor, objCell.BorderLeftStyle, objCell.BorderStyle);
                AddDirectionBorder<RightBorder>(border, objCell.BordeRightrColor, objCell.BorderColor, objCell.BorderRightStyle, objCell.BorderStyle);
                AddDirectionBorder<TopBorder>(border, objCell.BorderTopColor, objCell.BorderColor, objCell.BorderTopStyle, objCell.BorderStyle);
                AddDirectionBorder<BottomBorder>(border, objCell.BorderBottomColor, objCell.BorderColor, objCell.BorderBottomStyle, objCell.BorderStyle);

                DiagonalBorder diagonalBorder2 = new DiagonalBorder();
                border.Append(diagonalBorder2);

                _workbookStylesPart.Stylesheet.Borders.Append(border);

                // Add cell format
                cellFormat.BorderId = (uint)_workbookStylesPart.Stylesheet.Borders.Count++;

                Alignment alignement = null;

                if (objCell.Align != TextAlign.None)
                {
                    alignement = new Alignment();

                    switch (objCell.Align)
                    {
                        case TextAlign.Left:
                            alignement.Horizontal = HorizontalAlignmentValues.Left;
                            break;
                        case TextAlign.Center:
                            alignement.Horizontal = HorizontalAlignmentValues.Center;
                            break;
                        case TextAlign.Right:
                            alignement.Horizontal = HorizontalAlignmentValues.Right;
                            break;
                    }

                    cellFormat.Alignment = alignement;
                }

                if (objCell.VerticalAlign != VeriticalAlign.None)
                {
                    if (alignement == null)
                    {
                        alignement = new Alignment();
                        cellFormat.Alignment = alignement;
                    }

                    switch (objCell.VerticalAlign)
                    {
                        case VeriticalAlign.Top:
                            alignement.Vertical = VerticalAlignmentValues.Top;
                            break;
                        case VeriticalAlign.Middle:
                            alignement.Vertical = VerticalAlignmentValues.Center;
                            break;
                        case VeriticalAlign.Bottom:
                            alignement.Vertical = VerticalAlignmentValues.Bottom;
                            break;
                    }

                }


                cell.StyleIndex = (uint)_workbookStylesPart.Stylesheet.CellFormats.Count++;
            }
        }


        private uint InsertFill(Fill fill)
        {
            Fills fills = _WorkbookPart.WorkbookStylesPart.Stylesheet.Elements<Fills>().First();
            fills.Append(fill);
            return (uint)fills.Count++;
        }

        private BorderStyleValues GetBorderStyle(BorderStyle style)
        {

            switch (style)
            {
                case BorderStyle.Solid:
                    return BorderStyleValues.Thin;
                default:
                    return BorderStyleValues.None;
            }
        }

        private void AddDirectionBorder<T>(Border border, string directionColor, string genColor, BorderStyle directionStyle, BorderStyle style)
            where T : BorderPropertiesType, new()
        {
            if (!string.IsNullOrEmpty(directionColor)
            || directionStyle != BorderStyle.None
            || !string.IsNullOrEmpty(genColor)
            || style != BorderStyle.None)
            {

                BorderStyle borderDirStyle = directionStyle == BorderStyle.None ? style : directionStyle;
                T dirBorder = new T() { Style = GetBorderStyle(borderDirStyle) };

                if (!string.IsNullOrEmpty(directionColor) || !string.IsNullOrEmpty(genColor))
                {
                    string borderDirColor = string.IsNullOrEmpty(directionColor) ? genColor : directionColor;
                    Color color = new Color() { Indexed = (UInt32Value)64U, Rgb = new HexBinaryValue(borderDirColor) };
                    dirBorder.Append(color);
                }

                border.Append(dirBorder);
            }
        }

        private void SetPicture(WorksheetPart worksheetPart, string filePath, byte[] content, Cell cell, string mimeType)
        {
            if (worksheetPart.DrawingsPart == null)
                worksheetPart.AddNewPart<DrawingsPart>();

            ImagePart imgp = worksheetPart.DrawingsPart.AddImagePart(_dictImageTypes[mimeType]);

            bool isOK = false;

            using (var stream = GetStreamFrom(filePath, content, out isOK))
                if (isOK)
                    imgp.FeedData(stream);

            if (!isOK)
                return;

            var nvdp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties
            {
                Id = 1025,
                Name = "Picture 1",
                Description = "polymathlogo"
            };

            var picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks
            {
                NoChangeAspect = true,
                NoChangeArrowheads = true
            };
            var nvpdp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties
            {
                PictureLocks = picLocks
            };
            var nvpp = new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties
            {
                NonVisualDrawingProperties = nvdp,
                NonVisualPictureDrawingProperties = nvpdp
            };

            var stretch = new DocumentFormat.OpenXml.Drawing.Stretch
            {
                FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle()
            };

            var blipFill = new DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill();
            var blip = new DocumentFormat.OpenXml.Drawing.Blip
            {
                Embed = worksheetPart.DrawingsPart.GetIdOfPart(imgp),
                CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print
            };
            blipFill.Blip = blip;
            blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
            blipFill.Append(stretch);

            var t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
            var offset = new DocumentFormat.OpenXml.Drawing.Offset
            {
                X = 0,
                Y = 0
            };
            t2d.Offset = offset;
            //Bitmap bm = new Bitmap(sImagePath);

            var extents = new DocumentFormat.OpenXml.Drawing.Extents
            {
                Cx = 942976,//(long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
                Cy = 942976//(long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
            };
            //bm.Dispose();
            t2d.Extents = extents;
            var sp = new ShapeProperties
            {
                BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto,
                Transform2D = t2d
            };
            var prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry
            {
                Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle,
                AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList()
            };
            sp.Append(prstGeom);
            sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

            var picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture
            {
                NonVisualPictureProperties = nvpp,
                BlipFill = blipFill,
                ShapeProperties = sp
            };

            var clientData1 = new ClientData();
            var twoCellAnchor1 = new TwoCellAnchor() { EditAs = EditAsValues.OneCell };
            var fromMarker1 = new DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker();

            var columnId1 = new ColumnId
            {
                Text = cell.GetIndexColumn().ToString()
            };
            ColumnOffset columnOffset1 = new ColumnOffset
            {
                Text = "1000"
            };
            RowId rowId1 = new RowId
            {
                Text = (cell.GetColumnProperties().Item1 - 1).ToString()
            };
            RowOffset rowOffset1 = new RowOffset
            {
                Text = "1000"
            };

            fromMarker1.Append(columnId1);
            fromMarker1.Append(columnOffset1);
            fromMarker1.Append(rowId1);
            fromMarker1.Append(rowOffset1);


            // we need to get to Maker : if  our column n merged with others then we need to find last one , find the map and use it as tomarker
            string cellReference = cell.CellReference.Value;
            string endRowIndex = cell.GetColumnProperties().Item1.ToString();
            string endColumnIndex = (cell.GetIndexColumn() + 1).ToString();

            var mergeCells = worksheetPart.Worksheet.GetMergeCells();
            var mergedCell = worksheetPart.Worksheet.Elements<MergeCells>().First().Elements<MergeCell>().FirstOrDefault(x => x.Reference.Value.Contains(cellReference + ":"));

            if (mergedCell != null)
            {
                // get the last cell
                string lastCellReference = mergedCell.Reference.Value.Replace(cellReference + ":", "");

                if (!string.IsNullOrEmpty(lastCellReference))
                {
                    string value = Regex.Match(lastCellReference, @"\d+").Value;

                    if (int.TryParse(value, out int lastRowIndexTemplate))
                    {
                        endColumnIndex = (lastCellReference.Replace(lastRowIndexTemplate.ToString(), "").GetIndexColumn() + 1).ToString();
                        endRowIndex = cell.GetColumnProperties().Item1.ToString();
                    }
                }
            }

            var toMarker1 = new DocumentFormat.OpenXml.Drawing.Spreadsheet.ToMarker();
            ColumnId columnId2 = new ColumnId
            {
                Text = endColumnIndex
            };
            ColumnOffset columnOffset2 = new ColumnOffset
            {
                Text = "100"
            };
            RowId rowId2 = new RowId
            {
                Text = endRowIndex
            };
            RowOffset rowOffset2 = new RowOffset
            {
                Text = "100"
            };

            toMarker1.Append(columnId2);
            toMarker1.Append(columnOffset2);
            toMarker1.Append(rowId2);
            toMarker1.Append(rowOffset2);

            twoCellAnchor1.Append(fromMarker1);
            twoCellAnchor1.Append(toMarker1);
            twoCellAnchor1.Append(picture);
            twoCellAnchor1.Append(clientData1);

            if (worksheetPart.DrawingsPart.WorksheetDrawing == null)
                worksheetPart.DrawingsPart.WorksheetDrawing = new WorksheetDrawing();


            worksheetPart.DrawingsPart.WorksheetDrawing.Append(twoCellAnchor1);

            if (!worksheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
                worksheetPart.Worksheet.Append(new Drawing { Id = worksheetPart.GetIdOfPart(worksheetPart.DrawingsPart) });


            worksheetPart.DrawingsPart.WorksheetDrawing.Save(worksheetPart.DrawingsPart);
            worksheetPart.Worksheet.Save();
        }

        private Stream GetStreamFrom(string path, byte[] content, out bool isOk)
        {
            isOk = true;
            if (content != null && content.Length > 0)
                return new MemoryStream(content);
            else
            {
                if (File.Exists(path))
                    return new FileStream(path, FileMode.Open);
            }

            isOk = false;
            return null;

        }


        private UInt32Value GetNewSheetId()
        {
            if (_sheets == null)
                return 1;

            if (!_sheets.Any())
                return 1;

            return _sheets.Descendants<Sheet>().Max(x => x.SheetId) + 1;
        }


        private string GetColumnIndex(int index, bool firstIs0 = false)
        {
            int i = firstIs0 ? 0 : 1;
            string column = "A";

            while (i < index)
            {
                column = column.IncrementColumn();
                i++;
            }

            return column;

        }

        private uint GetIndexOf(string column, bool firstIs0 = false)
        {
            uint i = firstIs0 ? (uint)0 : (uint)1;
            string currentColumn = "A";

            while (currentColumn != column)
            {
                currentColumn = currentColumn.IncrementColumn();
                i++;
            }

            return i;
        }



        private string GetColumnName(Cell cell)
        {
            return cell.CellReference.Value.Replace(((Row)cell.Parent).RowIndex.ToString(), "");
        }


        private string GetCellValueText(Cell cell, SharedStringTablePart stringTable)
        {
            string value = string.Empty;
            int nBr = 0;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && stringTable != null && !string.IsNullOrWhiteSpace(cell.InnerText))
                if (int.TryParse(cell.InnerText, out nBr))
                    value = stringTable.SharedStringTable.ElementAt(nBr).InnerText;
                else
                    value = cell.InnerText;

            return value;
        }


        internal class RefMergedCell
        {
            public string CellReference { get; set; }

            public CellObject CellObject { get; set; }

            public uint Index { get; set; }

            public string ColumnName { get; set; }
        }


    }
}
