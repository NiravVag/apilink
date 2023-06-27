using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Components.Core.entities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using FileGenerationComponent.SourceProvider;
using FileGenerationComponent;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace FileGenerationComponent.PPT
{
    internal class PPTProvider : FileProvider
    {

        private PPTObject _fileObject = null;

        public PPTProvider(IConfiguration configuration) : base(configuration)
        {

        }

        protected override FileProperties GenerateDocument(FileObject source)
        {
            _fileObject = source as PPTObject;

            using (var stream = new MemoryStream())
            {
                _onLog?.Invoke(32, "Readig the power point file");

                // Generate file from model
                using (var templateFile = System.IO.File.Open(_fileObject.ModelPath, FileMode.Open, FileAccess.Read))
                {
                    _onLog?.Invoke(36, "Copy the power point file in memory");
                    templateFile.CopyTo(stream); //copy template

                    using (var presentationDocument = CreateFilePackage(stream))
                        IntializeDocument(presentationDocument, _fileObject);

                    stream.Seek(0, SeekOrigin.Begin); //scroll to stream start point

                }

                string fileName = ReplaceText(_fileObject.FileName, _fileObject.VariableList,
                 _fileObject.DataSource.Tables[0].Rows.Count > 0 ? _fileObject.DataSource.Tables[0].Rows[0] : null);

                return new FileProperties
                {
                    Content = stream.ToArray(),
                    FileType = FileType.PPT,
                    FileModelName = fileName,
                    MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"
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


        protected string GetParagraph(string paragraph, PPTObject fileObject)
        {
            // Only root variables with property is single
            var variabeList = fileObject.VariableList.Where(x => x.PropertType == PropertTypeConst.Single);

            if (!variabeList.Any())
                return paragraph;

            // Datarow for paragraph is the first row for first Index
            if (fileObject.DataSource.Tables[0].Rows.Count == 0)
                throw new Exception("Cannot find data for report ");

            var datarow = fileObject.DataSource.Tables[0].Rows[0];

            foreach (var variable in variabeList)
                if (variable.ParamOuters != null)
                    foreach (var func in variable.ParamOuters)
                        paragraph = func(datarow, paragraph);

            return paragraph;
        }

        // Search parameter
        protected IEnumerable<string> GetParameters(string text)
        {
            int i = text.Length - 1;
            string newText = text;

            var lststr = new List<string>();

            while (i >= 0)
            {
                if (text[i] == '[')
                {
                    int indexFinish = text.IndexOf("]", i + 1);
                    lststr.Add(text.Substring(i + 1, indexFinish - i - 1));
                    newText = newText.Replace("[" + text.Substring(i + 1, indexFinish - i - 1) + "]", "");
                }

                i--;
            }

            return lststr;
        }

        protected bool ContainsElement(string text, Variable variable)
        {
            return text.IndexOf(variable.OutPutName) > -1
                    || text.IndexOf(string.Format("[{0}:{1}", Functions.Sum, variable.VariableName)) > -1
                    || text.IndexOf(string.Format("[{0}:{1}", Functions.Count, variable.VariableName)) > -1
                    || text.IndexOf(string.Format("[{0}:{1}", Functions.Avg, variable.VariableName)) > -1;
        }

        private OpenXmlPackage CreateFilePackage(MemoryStream stream)
        {
            return PresentationDocument.Open(stream, true);
        }



        protected void IntializeDocument(OpenXmlPackage package, PPTObject fileObject)
        {
            _onLog?.Invoke(38, " Get Presentation file ");

            // Get the presentation part from the presentation document.
            var presentationPart = ((PresentationDocument)package).PresentationPart;

            // Get the presentation from the presentation part.
            var presentation = presentationPart.Presentation;

            //get available slide list

            // Create Tables 
            var varTableList = fileObject.VariableList.Where(x => x.PropertType == PropertTypeConst.List || x.PropertType == PropertTypeConst.Matrix);

            var varChartList = fileObject.VariableList.Where(x => x.PropertType == PropertTypeConst.Chart);

            int position = 0;
            IDictionary<SlideId, SlidePart> SlidesToDelete = new Dictionary<SlideId, SlidePart>();

            int percent = 50;
            int partpercent = percent / presentation.SlideIdList.Count();


            foreach (SlideId slideID in presentation.SlideIdList)
            {


                position++;
                _onLog?.Invoke(percent, $"Building slide {position}");
                var slide = (SlidePart)presentationPart.GetPartById(slideID.RelationshipId);

                bool toRemoveSlide = false;

                // Create Partaghraphs

                foreach (var x in slide.Slide.Descendants<Paragraph>().ToList())
                {
                    string outer = x.OuterXml;
                    if (outer.Contains("[OPTION:"))
                    {
                        toRemoveSlide = ToRemoveOption(ref outer, fileObject.VariableList, fileObject.DataSource.Tables[0]);

                        if (toRemoveSlide)
                            break;


                        // x.Remove();
                        // continue; 
                    }

                    var parent = x.Parent; //get parent element - to be used when removing placeholder
                    parent.InsertBefore(new Paragraph(GetParagraph(outer, fileObject)), x);
                    x.Remove();
                }

                if (toRemoveSlide)
                {
                    SlidesToDelete.Add(slideID, slide);
                    continue;
                }

                if (varTableList.Any())
                {
                    foreach (var variable in varTableList)
                    {
                        // search row in document

                        foreach (var x in slide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Table>())
                        //.ToList().ForEach(x =>
                        {
                            // get first row
                            if (x.Descendants<TableRow>().Any(y => y.InnerText.Contains(variable.OutPutName)))
                            {

                                var rowList = x.Descendants<TableRow>().Where(y => y.InnerText.Contains(variable.OutPutName) || y.InnerText.Contains("[ITEM]")).ToList();

                                if (variable.PropertType == PropertTypeConst.Matrix)
                                {
                                    CreateMatrixTable(rowList.First(), x, variable, slideID, presentationPart, slide, out bool toDelete);

                                    if (toDelete)
                                        SlidesToDelete.Add(slideID, slide);
                                }
                                else if (variable.IsHorizontal)
                                    CreateHorizontalRows(rowList.First(), x, variable);
                                else
                                {
                                    if (variable.MaxRows > 0 && variable.DataTable.Rows.Count > variable.MaxRows)
                                    {
                                        int index = 0;
                                        // SlidePart previousSlide = slide; 

                                        int iteration = (variable.DataTable.Rows.Count / variable.MaxRows);

                                        if (variable.DataTable.Rows.Count % variable.MaxRows > 0)
                                            iteration += 1;

                                        for (int i = 0; i < iteration; i++)
                                        {
                                            var newslide = CloneSlide(slide);
                                            AppendSlide(presentationPart, newslide, slideID);

                                            position++;

                                            if (newslide != null)
                                            {
                                                newslide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Table>().ToList().ForEach(y =>
                                                {
                                                    BuildTable(y, variable, index, (i == (iteration - 1)));
                                                });

                                            }
                                            //previousSlide = newslide;
                                            index += variable.MaxRows;
                                        }

                                        SlidesToDelete.Add(slideID, slide);

                                    }
                                    else
                                        BuildTable(x, variable, 0, true);
                                }

                            }


                            if (variable.DataTable.Rows.Count > 0)
                            {
                                //check if we have another rows if yes we have to check params 
                                foreach (var drow in x.Descendants<TableRow>().ToList())
                                    foreach (var cell in drow.Descendants<TableCell>())
                                        UpdateCell(cell, variable.VariableList, variable.DataTable.Rows[0], variable.OutPutName, variable.IsGroupfield, 0, variable.DataTable.Rows.Count);
                            }
                            else
                            {
                                foreach (var drow in x.Descendants<TableRow>().ToList())
                                    foreach (var cell in drow.Descendants<TableCell>())
                                        EmptyCell(cell, variable.VariableList, variable.OutPutName);
                            }
                        }
                        //);

                    }

                    //// hauyya sidi  charts 
                    //if (slide.ChartParts != null && slide.ChartParts.Any())
                    //    foreach (var chartPart in slide.ChartParts)
                    //        if (chartPart.EmbeddedPackagePart != null)
                    //        {
                    //            if (chartPart.EmbeddedPackagePart.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    //                continue;

                    //            var providerSource = SourceFactory.GetSourceProvider(SourceEnum.Json, FileType.Excel);


                    //            var excelObject = new ExcelJsonFileObject
                    //            {
                    //                DataSource = fileObject.DataSource,
                    //                Stream = chartPart.EmbeddedPackagePart.GetStream(),
                    //                ModelRequest = fileObject.ModelRequest,
                    //                VariableList = fileObject.VariableList
                    //            };

                    //            // Get Generator provider
                    //            var provider = FileProvider.GetProvider(FileType.Excel);

                    //            var embedFile = provider.BuildFile(excelObject);

                    //            if (embedFile != null)
                    //                chartPart.EmbeddedPackagePart.FeedData(new MemoryStream(embedFile.Content));
                    //        }
                }
                percent = percent + partpercent;
            }

            _onLog?.Invoke(82, $"removing unused slides");

            // delete slides 
            foreach (var item in SlidesToDelete)
                deleteSlide(presentationPart, item.Value, item.Key);

            _onLog?.Invoke(86, $"creating charts ");
            foreach (var slidepart in presentationPart.SlideParts)
            {
                //create charts 
                foreach (var charPart in slidepart.ChartParts)
                {
                    var variable = varChartList.FirstOrDefault(x => charPart.ChartDrawingPart != null
                    && charPart.ChartDrawingPart.UserShapes.Descendants<RelativeAnchorSize>()
                        .Any(y => y.InnerText.Contains(x.OutPutName)));

                    if (variable == null)
                        continue;

                    CreateChartPart(charPart, variable);
                }

            }

            // hayya sidi haya haya sidi  charts
            _onLog?.Invoke(90, $"saving  data");
            presentation.Save(); //save document changes we've made
        }

        private bool ToRemoveOption(ref string outerXml, IEnumerable<Variable> variableList, System.Data.DataTable dttable)
        {
            int index1 = outerXml.IndexOf("[OPTION:");

            if (index1 < 0)
                return false;

            int posEnd = outerXml.IndexOf("]", index1);

            int length = outerXml.Length - (index1 - 1) - (outerXml.Length - posEnd);
            string key = outerXml.Substring(index1, length).Replace("[OPTION:", "", StringComparison.OrdinalIgnoreCase).Replace("]", "").Trim();

            if (string.IsNullOrEmpty(key))
                return false;

            var variable = variableList.FirstOrDefault(x => x.VariableName == key);

            if (variable == null)
                return true;

            var value = dttable.Rows[0][variable.ColumnName].ToString().ToLower().Trim();

            outerXml = outerXml.Replace($"[OPTION:{key}]", "");

            return value != "1" && value != "true";
        }

        private void BuildMatrixTable(Table table, Variable variable, int index, bool isLastTable)
        {

            //columns 
            var varColumn = variable.VariableList.FirstOrDefault(x => x.VariableName == variable.X);
            var columns = variable.DataTable.AsEnumerable().GroupBy(x => x[varColumn.ColumnName].ToString());

            if (columns == null || !columns.Any())
            {
                foreach (var row in table.Descendants<TableRow>().ToList())
                    row.Remove();

                foreach (var column in table.TableGrid.Descendants<GridColumn>().ToList())
                    column.Remove();

                return;
            }

            var rowHeader = table.Descendants<TableRow>().FirstOrDefault(y => y.InnerText.Contains(variable.OutPutName) || y.InnerText.Contains("[ITEM]"));

            TableRow rowBody = null;


            foreach (var chValriable in variable.VariableList)
            {
                rowBody = table.Descendants<TableRow>().FirstOrDefault(x => x.InnerText.Contains($"[{chValriable.VariableName}]"));

                if (rowBody != null)
                    break;
            }

            // Delete all rows after details rows if we are not in last table
            if (!isLastTable)
            {
                var othersRows = rowBody.ElementsAfter();

                foreach (var element in othersRows)
                    element.Remove();

            }




            //Rows with title 
            var varRow = variable.VariableList.FirstOrDefault(x => x.VariableName == variable.Y);
            var rows = variable.DataTable.AsEnumerable().Select(x => x[varRow.ColumnName].ToString()).Distinct();

            // add rows
            var rowList = new List<TableRow>();

            // header row
            int indexRef = -1;
            TableCell firstCell = null;

            foreach (var item in rowHeader.Descendants<TableCell>())
            {
                indexRef++;

                if (item.InnerText.Contains(variable.OutPutName))
                {
                    firstCell = item;
                    break;
                }
            }

            TableCell lastCell = firstCell;
            TableCell firstCellClone = (TableCell)firstCell.CloneNode(true);



            foreach (var column in columns)
            {

                var cellHead = (TableCell)firstCellClone.CloneNode(true);
                var para = cellHead.ElementAt(0).Descendants<Paragraph>().FirstOrDefault(x => x.InnerText.Contains(variable.OutPutName));


                if (para != null)
                {
                    string text = para.OuterXml.Replace(variable.OutPutName, EscapeXml(column.Key));

                    var paragraph = new Paragraph(text);

                    cellHead.ElementAt(0).InsertBefore(paragraph, para);

                    para.Remove();

                    rowHeader.InsertAfter(cellHead, lastCell);
                    lastCell = cellHead;
                }


            }


            firstCell.Remove();
            int i = 0;

            // other columns 
            var firstRow = rowBody;
            TableRow lastRow = firstRow;
            string strVarBody = "";
            DataColumn varBodyColumn = null;
            Variable varBody = null;
            bool isSumX = false;

            foreach (var row in rows)
            {

                if (variable.MaxRows == 0 || (i >= index && i <= index + variable.MaxRows - 1))
                {

                    var tabRow = (TableRow)firstRow.CloneNode(true);

                    // first cell with current data
                    var firstBodyCell = tabRow.Descendants<TableCell>().ElementAt(indexRef - 1);
                    var firstBodyCellClone = (TableCell)firstBodyCell.CloneNode(true);

                    var emptyBodyCell = tabRow.Descendants<TableCell>().ElementAt(indexRef);

                    if (emptyBodyCell != null && emptyBodyCell.InnerText.Trim() == "")
                        emptyBodyCell.Remove();

                    strVarBody = firstBodyCell.InnerText.Trim().Replace("[", "").Replace("]", "");
                    varBody = variable.VariableList.FirstOrDefault(x => x.VariableName == strVarBody);


                    foreach (DataColumn column in variable.DataTable.Columns)
                    {
                        if (column.ColumnName == varBody.ColumnName)
                        {
                            varBodyColumn = column;
                            break;
                        }
                    }

                    var para = firstBodyCell.Descendants<Paragraph>().FirstOrDefault(x => x.InnerText.Contains($"[{strVarBody}]"));

                    if (para != null)
                    {
                        string text = para.OuterXml.Replace($"[{strVarBody}]", EscapeXml(row));

                        var paragraph = new Paragraph(text);

                        firstBodyCell.ElementAt(0).InsertBefore(paragraph, para);

                        para.Remove();
                    }

                    TableCell lastBodyCell = firstBodyCell;

                    foreach (var column in columns)
                    {

                        var cellBody = (TableCell)firstBodyCellClone.CloneNode(true);

                        // get item row
                        var datarow = column.FirstOrDefault(x => x[varRow.ColumnName].ToString() == row);

                        var paran = cellBody.Descendants<Paragraph>().FirstOrDefault(x => x.InnerText.Contains($"[{strVarBody}]"));

                        string txt = "";

                        if (paran != null)
                        {
                            if (datarow == null || datarow[varBody.ColumnName] == DBNull.Value)
                                txt = paran.OuterXml.Replace($"[{strVarBody}]", "0");
                            else
                                txt = paran.OuterXml.Replace($"[{strVarBody}]", EscapeXml(datarow[varBody.ColumnName].ToString()));

                            var paragraphBody = new Paragraph(txt);

                            cellBody.ElementAt(0).InsertBefore(paragraphBody, paran);
                            paran.Remove();
                        }

                        tabRow.InsertAfter(cellBody, lastBodyCell);
                        lastBodyCell = cellBody;
                    }

                    // check if we have sum in this row 
                    var cellSumx = tabRow.Descendants<TableCell>().FirstOrDefault(x => x.InnerText.Contains("[SUM:X]"));

                    if (cellSumx != null)
                    {
                        isSumX = true;
                        var param = cellSumx.Descendants<Paragraph>().FirstOrDefault();

                        if (param != null)
                        {
                            string text = param.OuterXml;
                            text = text.Replace("[SUM:X]", CallDynamicMethod("GetSumWhere", varBodyColumn.DataType, new object[] { variable.DataTable, varBody.ColumnName, varRow.ColumnName, row }).GetString(true));

                            var paragraphSum = new Paragraph(text);

                            cellSumx.ElementAt(0).InsertBefore(paragraphSum, param);
                            param.Remove();
                        }

                    }

                    table.InsertAfter(tabRow, lastRow);
                    lastRow = tabRow;
                }
            }

            firstRow.Remove();

            var rowFooter = table.Descendants<TableRow>().FirstOrDefault(x => x.InnerText.Contains("[SUM:Y]"));

            if (rowFooter != null)
            {

                TableCell refFooterCell = rowFooter.Descendants<TableCell>().FirstOrDefault(x => x.InnerText.Contains("[SUM:Y]"));
                TableCell firstFooterCell = rowFooter.Descendants<TableCell>().ElementAt(indexRef);


                var lastFooterCell = firstFooterCell;

                foreach (var column in columns)
                {
                    var cellFooter = (TableCell)firstFooterCell.CloneNode(true);

                    var paraO = cellFooter.Descendants<Paragraph>().FirstOrDefault();
                    var paraF = refFooterCell.Descendants<Paragraph>().FirstOrDefault(x => x.InnerText.Contains("[SUM:Y]"));

                    string txt = paraF.OuterXml.Replace("[SUM:Y]", CallDynamicMethod("GetSumRows", varBodyColumn.DataType, new object[] { column, varBody.ColumnName }).GetString(true));
                    var paragraphFooter = new Paragraph(txt);

                    cellFooter.ElementAt(0).InsertBefore(paragraphFooter, paraO);
                    paraO.Remove();

                    rowFooter.InsertAfter(cellFooter, lastFooterCell);

                    lastFooterCell = cellFooter;

                }

                //total

                if (isSumX)
                {
                    var paray = refFooterCell.Descendants<Paragraph>().FirstOrDefault(x => x.InnerText.Contains("[SUM:Y]"));
                    string txt = paray.OuterXml.Replace("[SUM:Y]", CallDynamicMethod("GetSum", varBodyColumn.DataType, new object[] { variable.DataTable, varBody.ColumnName }).GetString(true));
                    var paragraphFooter = new Paragraph(txt);


                    refFooterCell.ElementAt(0).InsertBefore(paragraphFooter, paray);
                    paray.Remove();
                }

                //clean 

                foreach (var tabcell in rowFooter.Descendants<TableCell>().Where(x => x.InnerText.Trim() == "" || x.InnerText.Contains("[SUM:Y]")).ToList())
                    tabcell.Remove();

            }


            int columnsCount = table.TableGrid.Descendants<GridColumn>().Count();
            int cellCount = rowHeader.Descendants<TableCell>().Count();

            if (cellCount > columnsCount)
                for (int j = columnsCount; j < cellCount; j++)
                {
                    var gcolumn = table.TableGrid.Descendants<GridColumn>().ElementAt(indexRef).CloneNode(true);
                    table.TableGrid.AppendChild(gcolumn);
                }
        }

        private static string EscapeXml(string s)
        {
            string toxml = s;
            if (!string.IsNullOrEmpty(toxml))
            {
                // replace literal values with entities
                toxml = toxml.Replace("&", "&amp;");
                toxml = toxml.Replace("'", "&apos;");
                toxml = toxml.Replace("\"", "&quot;");
                toxml = toxml.Replace(">", "&gt;");
                toxml = toxml.Replace("<", "&lt;");
            }
            return toxml;
        }

        private static object CallDynamicMethod(string name, Type typeArg, object[] parameters)
        {
            // Just for simplicity, assume it's public etc
            MethodInfo method = typeof(PPTProvider).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            MethodInfo generic = method.MakeGenericMethod(typeArg);
            return generic.Invoke(null, parameters);
        }

        private static string GetSumWhere<T>(System.Data.DataTable dataTable, string columnName, string columnFilterName, string columnValueName) where T : struct
        {
            T total = default(T);

            foreach (DataRow row in dataTable.AsEnumerable().Where(x => x[columnFilterName].ToString() == columnValueName))
            {
                if (row[columnName] != DBNull.Value)
                    total = Add(total, (T)row[columnName]);
            }

            return total.ToString();
        }

        private static string GetSum<T>(System.Data.DataTable dataTable, string columnName) where T : struct
        {
            T total = default(T);

            foreach (DataRow row in dataTable.Rows)
            {
                if (row[columnName] != DBNull.Value)
                    total = Add(total, (T)row[columnName]);
            }

            return total.ToString();
        }

        private static string GetSumRows<T>(IEnumerable<DataRow> dataRows, string columnName) where T : struct
        {
            T total = default(T);

            foreach (DataRow row in dataRows)
            {
                if (row[columnName] != DBNull.Value)
                    total = Add(total, (T)row[columnName]);
            }

            return total.ToString();
        }


        protected static T Add<T>(T number1, T number2)
        {
            dynamic a = number1;
            dynamic b = number2;
            return a + b;
        }



        private void BuildTable(Table table, Variable variable, int index, bool isLastTable)
        {
            var rowList = table.Descendants<TableRow>().Where(y => y.InnerText.Contains(variable.OutPutName) || y.InnerText.Contains("[ITEM]")).ToList();
            var lstRows = CreateRows(rowList, table, variable, index);

            foreach (var tbRow in lstRows)
                table.InsertBefore(tbRow, rowList.Last());

            // Delete all rows after details rows if we are not in last table
            if (!isLastTable)
            {
                var lastRow = rowList.Last();
                var othersRows = lastRow.ElementsAfter();

                foreach (var element in othersRows)
                    element.Remove();

            }

            foreach (var row in rowList)
                row.Remove();

            if (variable.DataTable.Rows.Count > 0)
            {
                //check if we have another rows if yes we have to check params 
                foreach (var drow in table.Descendants<TableRow>().ToList())
                    foreach (var cell in drow.Descendants<TableCell>())
                        UpdateCell(cell, variable.VariableList, variable.DataTable.Rows[0], variable.OutPutName, variable.IsGroupfield, 0, variable.DataTable.Rows.Count);
            }
            else
            {
                foreach (var drow in table.Descendants<TableRow>().ToList())
                    foreach (var cell in drow.Descendants<TableCell>())
                        EmptyCell(cell, variable.VariableList, variable.OutPutName);
            }
        }

        private void BuildTableHorizontal(Table table, Variable variable, int index, bool isLastTable)
        {

            var rowList = table.Descendants<TableRow>().Where(y => y.InnerText.Contains(variable.OutPutName) || y.InnerText.Contains("[ITEM]")).ToList();
            var lstRows = CreateRows(rowList, table, variable, index);

            foreach (var tbRow in lstRows)
                table.InsertBefore(tbRow, rowList.Last());

            // Delete all rows after details rows if we are not in last table
            if (!isLastTable)
            {
                var lastRow = rowList.Last();
                var othersRows = lastRow.ElementsAfter();

                foreach (var element in othersRows)
                    element.Remove();

            }

            foreach (var row in rowList)
                row.Remove();

            if (variable.DataTable.Rows.Count > 0)
            {
                //check if we have another rows if yes we have to check params 
                foreach (var drow in table.Descendants<TableRow>().ToList())
                    foreach (var cell in drow.Descendants<TableCell>())
                        UpdateCell(cell, variable.VariableList, variable.DataTable.Rows[0], variable.OutPutName, variable.IsGroupfield, 0, variable.DataTable.Rows.Count);
            }
            else
            {
                foreach (var drow in table.Descendants<TableRow>().ToList())
                    foreach (var cell in drow.Descendants<TableCell>())
                        EmptyCell(cell, variable.VariableList, variable.OutPutName);
            }
        }

        private void CreateMatrixTable(TableRow row, Table table, Variable variable, SlideId slideID, PresentationPart presentationPart, SlidePart slide, out bool toDeleteSilde)
        {
            if (variable.MaxRows > 0 && variable.DataTable.Rows.Count > variable.MaxRows)
            {
                int index = 0;
                // SlidePart previousSlide = slide; 

                int iteration = (variable.DataTable.Rows.Count / variable.MaxRows);

                if (variable.DataTable.Rows.Count % variable.MaxRows > 0)
                    iteration += 1;

                for (int i = 0; i < iteration; i++)
                {
                    var newslide = CloneSlide(slide);
                    AppendSlide(presentationPart, newslide, slideID);

                    //position++;

                    if (newslide != null)
                    {
                        newslide.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Table>().ToList().ForEach(y =>
                        {
                            BuildMatrixTable(y, variable, index, (i == (iteration - 1)));
                        });

                    }
                    //previousSlide = newslide;
                    index += variable.MaxRows;
                }
                toDeleteSilde = true;
                // SlidesToDelete.Add(slideID, slide);

            }
            else
            {
                toDeleteSilde = false;

                BuildMatrixTable(table, variable, 0, true);
            }

        }


        private void CreateHorizontalRows(TableRow row, DocumentFormat.OpenXml.Drawing.Table table, Variable variable)
        {
            if (variable.DataTable != null)
            {
                if (variable.IsHorizontal)
                {

                    // var row = rowList.FirstOrDefault();
                    int rowNum = 0;
                    var rowCopy = (TableRow)row.CloneNode(true);
                    var rowCopy2 = (TableRow)row.CloneNode(true);
                    bool createColumns = true;

                    rowCopy2.Descendants<TableCell>().ToList().ForEach(x => { x.Remove(); });

                    row.Remove();

                    foreach (DataRow dtrow in variable.DataTable.Rows)
                    {
                        rowNum++;
                        if (rowNum > variable.MaxRows)
                        {
                            rowNum = 1;
                            createColumns = false;
                            table.AppendChild(rowCopy2);

                            rowCopy2 = (TableRow)rowCopy.CloneNode(true);
                            rowCopy2.Descendants<TableCell>().ToList().ForEach(x => { x.Remove(); });

                            foreach (var cell in rowCopy.Descendants<TableCell>())
                            {
                                var copycell = (TableCell)cell.CloneNode(true);
                                UpdateCell(copycell, variable.VariableList, dtrow, variable.OutPutName, variable.IsGroupfield, 0, variable.DataTable.Rows.Count);
                                rowCopy2.AppendChild(copycell);
                            }

                        }
                        else
                        {

                            foreach (var cell in rowCopy.Descendants<TableCell>())
                            {
                                var copycell = (TableCell)cell.CloneNode(true);
                                UpdateCell(copycell, variable.VariableList, dtrow, variable.OutPutName, variable.IsGroupfield, 0, variable.DataTable.Rows.Count);


                                if (rowNum > 1 && createColumns)
                                {
                                    var column = table.TableGrid.Descendants<GridColumn>().FirstOrDefault().CloneNode(true);
                                    table.TableGrid.AppendChild(column);
                                }

                                rowCopy2.AppendChild(copycell);
                            }

                        }

                    }

                    int columnNumbers = table.TableGrid.Descendants<GridColumn>().Count();
                    int cellNumber = rowCopy2.Descendants<TableCell>().Count();

                    if ((columnNumbers - cellNumber) > 0)
                    {
                        var refCell = rowCopy.Descendants<TableCell>().FirstOrDefault().CloneNode(true);
                        refCell.ElementAt(0).Descendants<Paragraph>().ToList().ForEach(x => { x.InnerXml = ""; });

                        for (int i = 1; i <= (columnNumbers - cellNumber); i++)
                        {
                            //  Create cell with colspans
                            var copycell = refCell.CloneNode(true);

                            rowCopy2.AppendChild(copycell);
                        }

                        MergeCells(rowCopy2, cellNumber, (columnNumbers - cellNumber), true);
                    }


                    table.AppendChild(rowCopy2);
                }

                //check if we have another rows if yes we have to check params 
                if (variable.DataTable.Rows.Count > 0)
                {
                    foreach (var drow in table.Descendants<TableRow>().ToList())
                        foreach (var cell in drow.Descendants<TableCell>())
                            UpdateCell(cell, variable.VariableList, variable.DataTable.Rows[0], variable.OutPutName, variable.IsGroupfield, 0, variable.DataTable.Rows.Count);
                }
                else
                {
                    foreach (var drow in table.Descendants<TableRow>().ToList())
                        foreach (var cell in drow.Descendants<TableCell>())
                            EmptyCell(cell, variable.VariableList, variable.OutPutName);
                }



            }
        }

        private IEnumerable<TableRow> CreateRows(IEnumerable<TableRow> rowList, DocumentFormat.OpenXml.Drawing.Table table, Variable variable, int index)
        {
            var lstRows = new List<TableRow>();

            if (variable.DataTable != null)
            {
                if (!variable.IsHorizontal)
                {
                    IDictionary<string, string> LastOnes = new Dictionary<string, string>() { { "", "" } };
                    int i = 0;
                    foreach (DataRow dtrow in variable.DataTable.Rows)
                    {
                        if (variable.MaxRows == 0 || (i >= index && i <= index + variable.MaxRows - 1))
                        {
                            if (variable.IsGroupfield)
                            {
                                var groupList = variable.VariableList.Where(x => x.IsGroupfield).ToList();

                                foreach (var row in rowList)
                                {
                                    bool found = false;
                                    Variable itemVar = null;

                                    foreach (var item in groupList)
                                        if (row.InnerText.Contains(item.OutPutName))
                                        {
                                            found = true;
                                            itemVar = item;
                                        }

                                    if (found)
                                    {
                                        if (!LastOnes.Keys.Contains(itemVar.VariableName) || dtrow[itemVar.ColumnName].ToString() != LastOnes[itemVar.VariableName].ToString())
                                        {
                                            var rowCopy = (TableRow)row.CloneNode(true);
                                            foreach (var cell in rowCopy.Descendants<TableCell>().ToList())
                                                UpdateCell(cell, variable.VariableList, dtrow, variable.OutPutName, variable.IsGroupfield, index, (variable.MaxRows == 0 ? variable.DataTable.Rows.Count : variable.MaxRows));

                                            lstRows.Add(rowCopy);
                                        }

                                        if (!LastOnes.Keys.Contains(itemVar.VariableName))
                                            LastOnes.Add(itemVar.VariableName, dtrow[itemVar.ColumnName].ToString());
                                        else
                                            LastOnes[itemVar.VariableName] = dtrow[itemVar.ColumnName].ToString();

                                    }
                                    else
                                    {
                                        var rowCopy = (TableRow)row.CloneNode(true);
                                        foreach (var cell in rowCopy.Descendants<TableCell>().ToList())
                                            UpdateCell(cell, variable.VariableList, dtrow, variable.OutPutName, variable.IsGroupfield, index, (variable.MaxRows == 0 ? variable.DataTable.Rows.Count : variable.MaxRows));

                                        lstRows.Add(rowCopy);
                                    }


                                }

                            }
                            else
                            {
                                foreach (var row in rowList)
                                {
                                    var rowCopy = (TableRow)row.CloneNode(true);
                                    foreach (var cell in rowCopy.Descendants<TableCell>().ToList())
                                        UpdateCell(cell, variable.VariableList, dtrow, variable.OutPutName, variable.IsGroupfield, index, (variable.MaxRows == 0 ? variable.DataTable.Rows.Count : variable.MaxRows));

                                    //table.InsertBefore(rowCopy, row);
                                    lstRows.Add(rowCopy);
                                }
                            }
                        }
                        i++;
                    }

                    //Remove row
                    // foreach(var row in rowList)
                    // row.Remove();
                }

                //check if we have another rows if yes we have to check params 
                //foreach(var drow in table.Descendants<TableRow>().ToList())
                //    foreach (var cell in drow.Descendants<TableCell>())
                //        UpdateCell(cell, variable.VariableList, variable.DataTable.Rows[0], variable.OutPutName);

            }

            return lstRows;
        }



        private void CreateChartPart(ChartPart chartPart, Variable variable)
        {

            if (chartPart.ChartDrawingPart != null)
            {
                //if (variable.Series == null || !variable.Series.Any())
                //    CreateOldChartPart(chartPart, variable);
                //else
                CreateNewChartPart(chartPart, variable);
            }
        }


        private void CreateOldChartPart(ChartPart chartPart, Variable variable)
        {
            // We need first row for creating chart 
            DataRow dtRow = variable.DataTable.Rows[0];

            if (chartPart.ChartDrawingPart != null && chartPart.ChartDrawingPart.UserShapes.Descendants<RelativeAnchorSize>().Any(x => x.InnerText.Contains(variable.OutPutName)))
            {
                var userShapes = chartPart.ChartDrawingPart.UserShapes.Descendants<RelativeAnchorSize>().FirstOrDefault(x => x.InnerText.Contains(variable.OutPutName));

                foreach (var chart in chartPart.ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Chart>().ToList())
                {
                    var pieChart = chart.PlotArea.Descendants<Pie3DChart>().FirstOrDefault();
                    var pieChartSeries = pieChart.Descendants<PieChartSeries>().FirstOrDefault();


                    var cat = pieChartSeries.Descendants<CategoryAxisData>().FirstOrDefault();
                    var strRef = cat.Descendants<StringReference>().FirstOrDefault();
                    var strCache = strRef.Descendants<StringCache>().FirstOrDefault();

                    var points = strCache.Descendants<StringPoint>().ToDictionary(x => x.GetAttribute("idx", "").Value, y => y.InnerText);

                    var values = pieChartSeries.Descendants<Values>().FirstOrDefault();
                    var numref = values.Descendants<NumberReference>().FirstOrDefault();
                    var numCache = numref.Descendants<NumberingCache>().FirstOrDefault();

                    numCache.Descendants<NumericPoint>().ToList().ForEach(x =>
                    {
                        string val = points[x.GetAttribute("idx", "").Value];

                        if (!string.IsNullOrEmpty(val))
                        {
                            var selVariable = variable.VariableList.FirstOrDefault(y => y.VariableName == val);

                            if (selVariable != null)
                            {
                                string dtValue = dtRow[selVariable.ColumnName].ToString();

                                var numValue = x.Descendants<NumericValue>().FirstOrDefault();

                                x.InsertBefore(new NumericValue(dtValue), numValue);
                                numValue.Remove();
                            }
                        }

                    });
                }
                userShapes.Remove();
            }
        }

        private void CreateNewChartPart(ChartPart chartPart, Variable variable)
        {
            // We need first row for creating chart 

            foreach (var chart in chartPart.ChartSpace.Descendants<DocumentFormat.OpenXml.Drawing.Charts.Chart>().ToList())
            {

                switch (variable.ChartType)
                {
                    case ChartType.BarChart:
                        CreateNewChartPartBarChart(chart, variable, chartPart);
                        break;
                    case ChartType.Pie3DChart:
                        CreateNewChartPartBarChart(chart, variable, chartPart);
                        break;
                    case ChartType.DoughnutChart:
                        CreateNewChartPartDoughnutChart(chart, variable, chartPart);
                        break;
                }

            }


        }




        private bool CreateNewChartPartBarChart(DocumentFormat.OpenXml.Drawing.Charts.Chart chart, Variable variable, ChartPart chartPart)
        {

            double maxValAxe = 0;

            if (variable.DataTable.Rows.Count <= 0)
                return false;

            var dtRow = variable.DataTable.Rows[0];
            var currentChart = chart.PlotArea.Descendants<BarChart>().FirstOrDefault();
            var currentValAxis = chart.PlotArea.Descendants<ValueAxis>().FirstOrDefault();

            var categoriesList = new List<StringPoint>();

            // Categories 


            if (!string.IsNullOrEmpty(variable.Categories))
            {
                var currentVariableCategory = variable.VariableList.FirstOrDefault(x => x.VariableName == variable.Categories);

                if (currentVariableCategory != null)
                {
                    uint index = 0;
                    foreach (DataRow row in variable.DataTable.Rows)
                    {
                        categoriesList.Add(new StringPoint
                        {
                            Index = index,
                            NumericValue = new NumericValue(row[currentVariableCategory.ColumnName].ToString())
                        });

                        index++;

                    }

                }
            }

            // Series
            bool settingSeries = variable.Series != null && variable.Series.Any();


            if (settingSeries && variable.Series.Count() != currentChart.Descendants<BarChartSeries>().Count())
                throw new Exception($"The series Number are not same for charts");


            //respect orders
            Serie[] varSeries = null;

            if (settingSeries)
                varSeries = variable.Series.ToArray();

            int i = 0;

            foreach (var chartSerie in currentChart.Descendants<BarChartSeries>())
            {


                var currentPoint = chartSerie.SeriesText.StringReference.StringCache.Descendants<StringPoint>().FirstOrDefault();

                if (currentPoint == null)
                    throw new Exception("StringPoint Point doens not find for serie");

                var categories = chartSerie.Descendants<CategoryAxisData>().FirstOrDefault(); //(x => x.GetAttribute("idx", "").Value, y => y.InnerText);

                if (categories == null)
                    throw new Exception("Cannot find categories");

                var values = chartSerie.Descendants<Values>().FirstOrDefault();

                if (values == null)
                    throw new Exception("Cannot find values");

                var idxPoint = currentPoint.GetAttribute("idx", "").Value;

                //var valuePoint = currentPoint.InnerText;
                Variable selVariable = null;
                Variable selVariableValues = null;

                Serie varSerie = null;
                if (settingSeries)
                {
                    varSerie = varSeries[i];
                    selVariable = variable.VariableList.FirstOrDefault(y => y.VariableName == varSerie.Name);
                    selVariableValues = variable.VariableList.FirstOrDefault(y => y.VariableName == varSerie.Values);
                }
                else
                    selVariableValues = variable.VariableList.FirstOrDefault(y => y.VariableName == currentPoint.InnerText);


                if (settingSeries)
                {
                    if (selVariable != null)
                    {
                        // Need to be changed 
                        string dtValue = dtRow[selVariable.ColumnName].ToString();
                        currentPoint.NumericValue = new NumericValue(dtValue);

                    }
                    else
                        currentPoint.NumericValue = new NumericValue(varSerie.Name);


                    if (!string.IsNullOrEmpty(varSerie.Color))
                    {
                        if (chartSerie.ChartShapeProperties == null)
                            chartSerie.ChartShapeProperties = new ChartShapeProperties();

                        var solidFill = chartSerie.ChartShapeProperties.GetFirstChild<SolidFill>();

                        if (solidFill == null)
                        {
                            solidFill = new SolidFill();
                            chartSerie.ChartShapeProperties.AppendChild(solidFill);
                        }

                        var hexa = solidFill.GetFirstChild<RgbColorModelHex>();

                        if (hexa == null)
                        {
                            hexa = new RgbColorModelHex();
                            solidFill.AppendChild(hexa);

                        }

                        hexa.Val = new HexBinaryValue(varSerie.Color);


                        var shemeColor = solidFill.GetFirstChild<SchemeColor>();

                        if (shemeColor != null)
                            solidFill.RemoveChild(shemeColor);


                    }
                }


                categories.StringReference.StringCache.RemoveAllChildren<StringPoint>();
                values.NumberReference.NumberingCache.RemoveAllChildren<NumericPoint>();

                foreach (var cat in categoriesList)
                {
                    var row = variable.DataTable.Rows[(int)cat.Index.Value];
                    var cloneCat = cat.Clone() as StringPoint;
                    categories.StringReference.StringCache.AppendChild(cloneCat);
                    var rowvalue = row[selVariableValues.ColumnName] != DBNull.Value ? row[selVariableValues.ColumnName] : 0;
                    double number = Convert.ToDouble(rowvalue);

                    if (maxValAxe < number)
                        maxValAxe = number;

                    values.NumberReference.NumberingCache.AppendChild(new NumericPoint
                    {
                        Index = cat.Index,
                        NumericValue = new NumericValue(number.ToString().Replace(",", "."))
                    });

                }
                categories.StringReference.StringCache.PointCount.Val = (uint)categoriesList.Count;
                values.NumberReference.NumberingCache.PointCount.Val = (uint)categoriesList.Count;
                i++;
            }

            maxValAxe = (maxValAxe / 5 + (maxValAxe % 5 > 0 ? 1 : 0)) * 5;

            if (!variable.FixScale)
            {
                currentValAxis.Scaling = new Scaling
                {
                    Orientation = new Orientation
                    {
                        Val = new EnumValue<OrientationValues>(OrientationValues.MinMax)
                    },
                    MinAxisValue = new MinAxisValue
                    {
                        Val = 0
                    },
                    MaxAxisValue = new MaxAxisValue
                    {
                        Val = maxValAxe
                    }
                };
            }


            var userShapes = chartPart.ChartDrawingPart.UserShapes.Descendants<RelativeAnchorSize>().FirstOrDefault(x => x.InnerText.Contains(variable.OutPutName));
            userShapes.Remove();

            return true;

        }

        private bool CreateNewChartPartDoughnutChart(DocumentFormat.OpenXml.Drawing.Charts.Chart chart, Variable variable, ChartPart chartPart)
        {

            if (variable.DataTable.Rows.Count <= 0)
                return false;

            var dtRow = variable.DataTable.Rows[0];
            var currentChart = chart.PlotArea.Descendants<DoughnutChart>().FirstOrDefault();


            // Only one serie for that type of charts
            var serie = currentChart.Descendants<PieChartSeries>().FirstOrDefault();

            if (serie == null)
                throw new Exception("No series were found");


            var categories = serie.Descendants<CategoryAxisData>().FirstOrDefault();

            if (categories == null)
                throw new Exception("Cannot find categories");

            var values = serie.Descendants<Values>().FirstOrDefault();

            if (values == null)
                throw new Exception("Cannot find values");

            foreach (var category in categories.StringReference.StringCache.Descendants<StringPoint>())
            {
                var item = category.NumericValue.InnerText;

                if (string.IsNullOrEmpty(item))
                    continue;

                var selVariable = variable.VariableList.FirstOrDefault(y => y.VariableName == item);

                if (selVariable == null)
                    continue;

                double number = Convert.ToDouble(dtRow[selVariable.ColumnName]);

                var currentValue = values.NumberReference.NumberingCache.Descendants<NumericPoint>().FirstOrDefault(x => x.Index == category.Index);

                if (currentValue == null)
                    continue;

                currentValue.NumericValue = new NumericValue(number.ToString());
            }

            var userShapes = chartPart.ChartDrawingPart.UserShapes.Descendants<RelativeAnchorSize>().FirstOrDefault(x => x.InnerText.Contains(variable.OutPutName));
            userShapes.Remove();

            return true;
        }

        private void MergeCells(TableRow row, int index, int length, bool horizontal)
        {
            if (horizontal)
            {
                for (int i = index; i < length + index; i++)
                {
                    var cell = row.Descendants<TableCell>().ToList().ElementAt(i);

                    if (length > 1)
                    {
                        if (i == index)
                            cell.SetAttribute(new OpenXmlAttribute("", "gridSpan", "", length.ToString()));
                        else
                            cell.SetAttribute(new OpenXmlAttribute("", "hMerge", "", "1"));
                    }

                }
            }
        }

        private void UpdateCell(TableCell cell, IEnumerable<Variable> variableList, DataRow row, string outerText, bool isGroupField, int index, int count)
        {

            string columnName = string.Empty;

            foreach (var variable in variableList.Where(x => x.PropertType == PropertTypeConst.Single))
            {
                if (ContainsElement(cell.InnerXml, variable))
                {
                    foreach (var paragraph in cell.ElementAt(0).Descendants<Paragraph>())
                    {
                        string text = paragraph.OuterXml.Replace(outerText, "").Replace("[ITEM]", "");

                        if (variable.ParamOuters != null)
                            foreach (var func in variable.ParamOuters)
                                text = func(row, text);

                        var newparagPraph = new Paragraph(text);
                        cell.ElementAt(0).InsertBefore(newparagPraph, paragraph);
                        paragraph.Remove();
                    }

                    if (!isGroupField && variable.IsGroupfield)
                    {
                        // get all variables  where indexgroups is more of this variable
                        List<string> columnGroups = new List<string>();

                        if (variable.IndexGroup == 0)
                            columnGroups = new List<string>() { variable.ColumnName };
                        else
                            columnGroups = variableList.Where(x => x.IsGroupfield && x.IndexGroup <= variable.IndexGroup)
                           .Select(x => x.ColumnName).ToList();

                        if (variable.IsGroupfield && row.GetCountByGroup(columnGroups, index, count) > 1)
                        {
                            if (row.IsFirstElementInGroup(columnGroups, index))
                                cell.SetAttribute(new OpenXmlAttribute("", "rowSpan", "", row.GetCountByGroup(columnGroups, index, count).ToString()));
                            else
                                cell.SetAttribute(new OpenXmlAttribute("", "vMerge", "", "1"));

                        }
                    }

                    if (variable.IsBoldForFirstRow && !variable.IsGroupfield)
                    {
                        // search variable groupfield in row
                        var variableGrp = variableList.FirstOrDefault(x => x.IsGroupfield && x.UsedForFirstRowGroup);
                        List<string> columnGroups = new List<string>();

                        if (variableGrp.IndexGroup == 0)
                            columnGroups = new List<string>() { variableGrp.ColumnName };
                        else
                            columnGroups = variableList.Where(x => x.IsGroupfield && x.IndexGroup <= variableGrp.IndexGroup)
                           .Select(x => x.ColumnName).ToList();

                        if (variableGrp != null)
                        {

                            if (row.IsFirstElementInGroup(columnGroups, index))
                                MakeCellBold(cell);
                        }
                    }

                }

            }

        }

        private void EmptyCell(TableCell cell, IEnumerable<Variable> variableList, string outerText)
        {

            string columnName = string.Empty;

            foreach (var variable in variableList.Where(x => x.PropertType == PropertTypeConst.Single))
            {
                if (ContainsElement(cell.InnerXml, variable))
                {
                    foreach (var paragraph in cell.ElementAt(0).Descendants<Paragraph>())
                    {
                        string text = paragraph.OuterXml.Replace(outerText, "").Replace("[ITEM]", "");

                        if (variable.EmptyOuters != null)
                            foreach (var func in variable.EmptyOuters)
                                if (func != null)
                                    text = func(text);

                        var newparagPraph = new Paragraph(text);
                        cell.ElementAt(0).InsertBefore(newparagPraph, paragraph);
                        paragraph.Remove();
                    }

                }

            }

        }

        private void MakeCellBold(TableCell cell)
        {
            foreach (var property in cell.Descendants<RunProperties>())
                property.Bold = true;

        }

        // Insert the specified slide into the presentation at the specified position.
        //private  Slide InsertNewSlide(PresentationDocument presentationDocument, int position, string slideTitle)
        //{

        //    if (presentationDocument == null)
        //    {
        //        throw new ArgumentNullException("presentationDocument");
        //    }

        //    if (slideTitle == null)
        //    {
        //        throw new ArgumentNullException("slideTitle");
        //    }

        //    PresentationPart presentationPart = presentationDocument.PresentationPart;

        //    // Verify that the presentation is not empty.
        //    if (presentationPart == null)
        //    {
        //        throw new InvalidOperationException("The presentation document is empty.");
        //    }

        //    // Declare and instantiate a new slide.
        //    Slide slide = new Slide(new CommonSlideData(new ShapeTree()));
        //    uint drawingObjectId = 1;

        //    // Construct the slide content.            
        //    // Specify the non-visual properties of the new slide.
        //    DocumentFormat.OpenXml.Presentation.NonVisualGroupShapeProperties nonVisualProperties = slide.CommonSlideData.ShapeTree.AppendChild(new DocumentFormat.OpenXml.Presentation.NonVisualGroupShapeProperties());
        //    nonVisualProperties.NonVisualDrawingProperties = new DocumentFormat.OpenXml.Presentation.NonVisualDrawingProperties() { Id = 1, Name = "" };
        //    nonVisualProperties.NonVisualGroupShapeDrawingProperties = new DocumentFormat.OpenXml.Presentation.NonVisualGroupShapeDrawingProperties();
        //    nonVisualProperties.ApplicationNonVisualDrawingProperties = new ApplicationNonVisualDrawingProperties();

        //    // Specify the group shape properties of the new slide.
        //    slide.CommonSlideData.ShapeTree.AppendChild(new DocumentFormat.OpenXml.Presentation.GroupShapeProperties());

        //    // Declare and instantiate the title shape of the new slide.
        //    DocumentFormat.OpenXml.Presentation.Shape titleShape = slide.CommonSlideData.ShapeTree.AppendChild(new DocumentFormat.OpenXml.Presentation.Shape());

        //    drawingObjectId++;

        //    // Specify the required shape properties for the title shape. 
        //    titleShape.NonVisualShapeProperties = new DocumentFormat.OpenXml.Presentation.NonVisualShapeProperties
        //        (new DocumentFormat.OpenXml.Presentation.NonVisualDrawingProperties() { Id = drawingObjectId, Name = "Title" },
        //        new DocumentFormat.OpenXml.Presentation.NonVisualShapeDrawingProperties(new ShapeLocks() { NoGrouping = true }),
        //        new ApplicationNonVisualDrawingProperties(new PlaceholderShape() { Type = PlaceholderValues.Title }));
        //    titleShape.ShapeProperties = new DocumentFormat.OpenXml.Presentation.ShapeProperties();

        //    // Specify the text of the title shape.
        //    titleShape.TextBody = new DocumentFormat.OpenXml.Presentation.TextBody(new BodyProperties(),
        //            new ListStyle(),
        //            new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text() { /*Text = slideTitle*/ })));

        //    // Declare and instantiate the body shape of the new slide.
        //    DocumentFormat.OpenXml.Presentation.Shape bodyShape = slide.CommonSlideData.ShapeTree.AppendChild(new DocumentFormat.OpenXml.Presentation.Shape());
        //    drawingObjectId++;

        //    // Specify the required shape properties for the body shape.
        //    bodyShape.NonVisualShapeProperties = new DocumentFormat.OpenXml.Presentation.NonVisualShapeProperties(new DocumentFormat.OpenXml.Presentation.NonVisualDrawingProperties() { Id = drawingObjectId, Name = "Content Placeholder" },
        //            new DocumentFormat.OpenXml.Presentation.NonVisualShapeDrawingProperties(new ShapeLocks() { NoGrouping = true }),
        //            new ApplicationNonVisualDrawingProperties(new PlaceholderShape() { Index = 1 }));
        //    bodyShape.ShapeProperties = new DocumentFormat.OpenXml.Presentation.ShapeProperties();

        //    // Specify the text of the body shape.
        //    bodyShape.TextBody = new DocumentFormat.OpenXml.Presentation.TextBody(new BodyProperties(),
        //            new ListStyle(),
        //            new Paragraph());

        //    // Create the slide part for the new slide.
        //    SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();

        //    // Save the new slide part.
        //    slide.Save(slidePart);

        //    // Modify the slide ID list in the presentation part.
        //    // The slide ID list should not be null.
        //    SlideIdList slideIdList = presentationPart.Presentation.SlideIdList;

        //    // Find the highest slide ID in the current list.
        //    uint maxSlideId = 1;
        //    SlideId prevSlideId = null;

        //    foreach (SlideId slideId in slideIdList.ChildElements)
        //    {
        //        if (slideId.Id > maxSlideId)
        //        {
        //            maxSlideId = slideId.Id;
        //        }

        //        position--;
        //        if (position == 0)
        //        {
        //            prevSlideId = slideId;
        //        }

        //    }

        //    maxSlideId++;

        //    // Get the ID of the previous slide.
        //    SlidePart lastSlidePart;

        //    if (prevSlideId != null)
        //    {
        //        lastSlidePart = (SlidePart)presentationPart.GetPartById(prevSlideId.RelationshipId);
        //    }
        //    else
        //    {
        //        lastSlidePart = (SlidePart)presentationPart.GetPartById(((SlideId)(slideIdList.ChildElements[0])).RelationshipId);
        //    }




        //    // Get SlideMasterPart and SlideLayoutPart from the existing Presentation Part
        //    string layoutName = "Comparison";
        //    SlideMasterPart slideMasterPart = presentationPart.SlideMasterParts.First();
        //    SlideLayoutPart slideLayoutPart = slideMasterPart.SlideLayoutParts.SingleOrDefault
        //        (sl => sl.SlideLayout.CommonSlideData.Name.Value.Equals(layoutName, StringComparison.OrdinalIgnoreCase));
        //    if (slideLayoutPart == null)
        //    {
        //        throw new Exception("The slide layout " + layoutName + " is not found");
        //    }

        //    slidePart.AddPart<SlideLayoutPart>(slideLayoutPart);

        //    // Insert the new slide into the slide list after the previous slide.
        //    SlideId newSlideId = slideIdList.InsertAfter(new SlideId(), prevSlideId);
        //    newSlideId.Id = maxSlideId;
        //    newSlideId.RelationshipId = presentationPart.GetIdOfPart(slidePart);

        //    // Save the modified presentation.
        //    //presentationPart.Presentation.Save();

        //    return slide;
        //}

        private static SlidePart CloneSlide(SlidePart templatePart)
        {
            // find the presentationPart: makes the API more fluent
            var presentationPart = templatePart.GetParentParts()
                .OfType<PresentationPart>()
                .Single();

            // clone slide contents
            Slide currentSlide = (Slide)templatePart.Slide.CloneNode(true);
            var slidePartClone = presentationPart.AddNewPart<SlidePart>();
            currentSlide.Save(slidePartClone);

            // copy layout part
            slidePartClone.AddPart(templatePart.SlideLayoutPart);

            return slidePartClone;
        }

        private static void AppendSlide(PresentationPart presentationPart, SlidePart newSlidePart, SlideId slideId)
        {
            SlideIdList slideIdList = presentationPart.Presentation.SlideIdList;

            //// find the highest id
            uint maxSlideId = slideIdList.ChildElements
                .Cast<SlideId>()
                .Max(x => x.Id.Value);

            // Insert the new slide into the slide list after the previous slide.
            var id = maxSlideId + 1;

            SlideId newSlideId = new SlideId();
            slideIdList.InsertBefore(newSlideId, slideId);
            newSlideId.Id = id;
            newSlideId.RelationshipId = presentationPart.GetIdOfPart(newSlidePart);
        }


        private static void deleteSlide(PresentationPart presentationPart, SlidePart slidePart, SlideId slideId)
        {
            SlideIdList slideIdList = presentationPart.Presentation.SlideIdList;
            slideIdList.RemoveChild(slideId);

            presentationPart.DeletePart(slidePart);

        }
    }
}
