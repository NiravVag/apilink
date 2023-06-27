using Contracts.Managers;
using DTO.Common;
using DTO.FBInternalReport;
using DTO.File;
using DTO.Inspection;
using DTO.MasterConfig;
using Microsoft.AspNetCore.Hosting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LINK_UI.FileModels
{
    public class QCInspectionDetailPDF : ApiCommonData, IQCInspectionDetailPDF
    {
        private readonly IHostingEnvironment _environnement = null;
        private Document Document = null;
        private TextFrame AddressFrame = null;
        private Table ProductTable = null;
        private Section Section = null;

        int pageNumber = 1;
        int pageRowCount = 0;
        int initialPageRowLimit = 22;
        int initialPageRowLimitValue = 22;
        int combineInitialPageRowLimit = 18;
        int combineInitialPageRowLimitValue = 18;
        int initialCombinePageRowLimit = 12;
        private Color TableBorder = Colors.Black;
        private Color TableBlue = Colors.DarkBlue;
        private Color TableGray = Colors.Gray;
        readonly TextInfo myTI = new CultureInfo(EnglishUS, false).TextInfo;
        readonly int maxCharInRow = 16;
        int otherPageRowLimit = 35;
        int otherPageRowLimitNonCombined = 35;
        const int nonCombinedOtherPageLimit = 35;
        const int combinedOtherPageLimit = 35;
        const int commentRowLimit = 5;
        const int commentRowCharLimit = 161;
        const int maxCharLength = 16;

        public QCInspectionDetailPDF(IHostingEnvironment environnement)
        {
            _environnement = environnement;
        }

        /// <summary>
        /// CreateQCInspectionDetailDocument
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public FileResponse CreateQCInspectionDetailDocument(QCInspectionDetailsPDF dataSource)
        {
            var entity = dataSource.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            this.Document = new Document();
            this.Document.Info.Title = QCInspectionDetailsPDF.GetValueOrDefault("Title", "");
            this.Document.Info.Subject = QCInspectionDetailsPDF.GetValueOrDefault("Title", "");
            this.Document.Info.Author = entity;
            DefineStyles();

            CreatePage(dataSource);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);

            renderer.Document = Document;
            renderer.RenderDocument();
            using (var memory = new MemoryStream())
            {
                renderer.PdfDocument.Save(memory);
                renderer.PdfDocument.Close();
                return new FileResponse
                {
                    Name = $"InspectionDetail_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
                    Content = memory.ToArray(),
                    MimeType = "application/pdf",
                    Result = FileResult.Success
                };
            }
        }

        /// <summary>
        /// Add base styles for the pdf
        /// </summary>
        private void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = this.Document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = this.Document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style.ParagraphFormat.LeftIndent = 30;
            style = this.Document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.Document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.Document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        /// <summary>
        /// add page header for the section
        /// </summary>
        /// <param name="section"></param>
        private void AddPageHeader(Section section, string imageLogo)
        {
            //add table header which comes for every page.
            Table tableHeader = section.Headers.Primary.AddTable();
            tableHeader.Style = "Table";
            tableHeader.Borders.Color = TableBorder;
            tableHeader.Rows.LeftIndent = 0;
            tableHeader.Format.SpaceBefore = 10;
            tableHeader.Format.SpaceAfter = 10;

            //Define two columns one for image and other for title
            Column columnImgLogo = tableHeader.AddColumn("4cm");
            columnImgLogo.Format.Alignment = ParagraphAlignment.Left;
            Column columnTitle = tableHeader.AddColumn("22cm");
            columnTitle.Format.Alignment = ParagraphAlignment.Left;


            //Define row for the table
            Row rowInfoTitle = tableHeader.AddRow();
            rowInfoTitle.Height = "1.5cm";
            rowInfoTitle.Format.Alignment = ParagraphAlignment.Center;
            rowInfoTitle.VerticalAlignment = VerticalAlignment.Center;

            //add image in the first column
            Image image = rowInfoTitle.Cells[0].AddImage(Path.Combine(_environnement.ContentRootPath, imageLogo));
            image.Height = "1.5cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;

            image.Top = ShapePosition.Center;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.WrapFormat.Style = WrapStyle.Through;

            //add text in the second column
            var rParagraph = rowInfoTitle.Cells[1].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Title", ""));
            rowInfoTitle.Cells[1].Format.Font.Bold = true;
            rowInfoTitle.Cells[1].Format.Font.Size = 18;
        }

        /// <summary>
        /// add inspection detail data
        /// </summary>
        /// <param name="section"></param>
        /// <param name="dataSource"></param>
        private void AddInspectionDetailData(Section section, QCInspectionDetailsPDF dataSource)
        {
            if (dataSource != null)
            {
                int CommentsCharLimit = 100;

                //Define Text Frame which holds the table.
                this.AddressFrame = Section.AddTextFrame();
                this.AddressFrame.Height = "15.0cm";
                this.AddressFrame.Width = "7.0cm";
                this.AddressFrame.Left = ShapePosition.Left;
                this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                this.AddressFrame.Top = "3.2cm";
                this.AddressFrame.RelativeVertical = RelativeVertical.Page;

                //create the table for inspection detail data
                var inspectionInfo = this.AddressFrame.AddTable();
                inspectionInfo.Style = "Table";
                inspectionInfo.Borders.Color = TableBorder;
                inspectionInfo.Rows.LeftIndent = 0;

                //add colums with specific width
                inspectionInfo.AddColumn("2.5cm"); // 3
                inspectionInfo.AddColumn("2.5cm"); // 5
                inspectionInfo.AddColumn("5cm"); //3
                inspectionInfo.AddColumn("5cm");
                inspectionInfo.AddColumn("2.3cm"); //3
                inspectionInfo.AddColumn("2.9cm");
                inspectionInfo.AddColumn("2.9cm");
                inspectionInfo.AddColumn("2.9cm");

                //add header row
                Row rowInspectionHeader = inspectionInfo.AddRow();
                rowInspectionHeader.Shading.Color = Colors.LightGray;
                rowInspectionHeader.Height = "1cm";
                rowInspectionHeader.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("InspectionID", ""));
                rowInspectionHeader.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[0].Format.Font.Bold = true;
                rowInspectionHeader.Cells[0].Format.Font.Size = 10;
                rowInspectionHeader.Cells[1].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("InspectionDate", ""));
                rowInspectionHeader.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[1].Format.Font.Bold = true;
                rowInspectionHeader.Cells[1].Format.Font.Size = 10;

                rowInspectionHeader.Cells[2].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Customer", ""));
                rowInspectionHeader.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[2].Format.Font.Bold = true;
                rowInspectionHeader.Cells[2].Format.Font.Size = 10;
                rowInspectionHeader.Cells[3].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Supplier", ""));

                rowInspectionHeader.Cells[3].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[3].Format.Font.Bold = true;
                rowInspectionHeader.Cells[3].Format.Font.Size = 10;
                rowInspectionHeader.Cells[4].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("ManDays", ""));
                rowInspectionHeader.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[4].Format.Font.Bold = true;
                rowInspectionHeader.Cells[4].Format.Font.Size = 10;
                rowInspectionHeader.Cells[5].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Collection", ""));
                rowInspectionHeader.Cells[5].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[5].Format.Font.Bold = true;
                rowInspectionHeader.Cells[5].Format.Font.Size = 10;
                rowInspectionHeader.Cells[6].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Brand", ""));
                rowInspectionHeader.Cells[6].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[6].Format.Font.Bold = true;
                rowInspectionHeader.Cells[6].Format.Font.Size = 10;
                rowInspectionHeader.Cells[7].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Department", ""));
                rowInspectionHeader.Cells[7].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionHeader.Cells[7].Format.Font.Bold = true;
                rowInspectionHeader.Cells[7].Format.Font.Size = 10;

                //add first row for the inspection details
                Row rowInspectionData = inspectionInfo.AddRow();
                rowInspectionData.Height = "1.5cm";
                rowInspectionData.Cells[0].AddParagraph(Convert.ToString(dataSource.InspectionID));
                rowInspectionData.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                if (dataSource.ServiceDate != null)
                {
                    rowInspectionData.Cells[1].AddParagraph(dataSource.ServiceDate);
                    rowInspectionData.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                }
                if (dataSource.Customer != null)
                {
                    var customerName = dataSource.Customer.Length > 11 ?
                                                    SplitStringWithSpace(dataSource.Customer, 11) : dataSource.Customer;
                    var customer = rowInspectionData.Cells[2].AddParagraph(customerName);
                    customer.Format.Font = new Font("Arial Unicode MS", Unit.FromPoint(9));
                    rowInspectionData.Cells[2].VerticalAlignment = VerticalAlignment.Center;

                }
                if (dataSource.Supplier != null)
                {
                    var supplierName = dataSource.Supplier.Length > 11 ?
                                                    SplitStringWithSpace(dataSource.Supplier, 11) : dataSource.Supplier;
                    var supplierData = rowInspectionData.Cells[3].AddParagraph(supplierName);
                    supplierData.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                    rowInspectionData.Cells[3].VerticalAlignment = VerticalAlignment.Center;
                }
                if (dataSource.NoofManDays > 0)
                {
                    rowInspectionData.Cells[4].AddParagraph(Convert.ToString(dataSource.NoofManDays));
                    rowInspectionData.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                }
                if (dataSource.CollectionName != null)
                {
                    var collectionName = dataSource.CollectionName.Length > 15 ?
                                                    SplitStringWithSpace(dataSource.CollectionName, 15) : dataSource.CollectionName;
                    rowInspectionData.Cells[5].AddParagraph(collectionName);
                    rowInspectionData.Cells[5].VerticalAlignment = VerticalAlignment.Center;
                }
                if (dataSource.BrandNames != null)
                {
                    var brandName = dataSource.BrandNames.Length > 15 ?
                                                       SplitStringWithSpace(dataSource.BrandNames, 15) : dataSource.BrandNames;
                    rowInspectionData.Cells[6].AddParagraph(brandName);
                    rowInspectionData.Cells[6].VerticalAlignment = VerticalAlignment.Center;
                }
                if (dataSource.DepartmentNames != null)
                {
                    var deptName = dataSource.DepartmentNames.Length > 15 ?
                                                          SplitStringWithSpace(dataSource.DepartmentNames, 15) : dataSource.DepartmentNames;
                    rowInspectionData.Cells[7].AddParagraph(deptName);
                    rowInspectionData.Cells[7].VerticalAlignment = VerticalAlignment.Center;
                }

                //add row for factory data
                Row rowInspectionFactory = inspectionInfo.AddRow();

                rowInspectionFactory.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Factory", ""));

                rowInspectionFactory.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionFactory.Cells[0].Format.Font.Bold = true;
                rowInspectionFactory.Cells[0].Format.Font.Size = 10;
                rowInspectionFactory.Cells[0].Shading.Color = Colors.LightGray;

                var factoryName = "";
                if (dataSource.Factory != null)
                {
                    factoryName = dataSource.Factory;
                }
                var factoryParagraph = rowInspectionFactory.Cells[1].AddParagraph(factoryName);
                factoryParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                rowInspectionFactory.Cells[1].MergeRight = 2;
                factoryParagraph.AddLineBreak();
                if (dataSource.FactoryAddress != null)
                {
                    var factoryAddress = dataSource.FactoryAddress;
                    if (dataSource.FactoryRegionalAddress != null && !dataSource.FactoryRegionalAddress.Equals(factoryAddress))
                    {
                        factoryAddress = factoryAddress + "(" + dataSource.FactoryRegionalAddress + ")";
                    }
                    factoryParagraph.AddText(factoryAddress);
                    factoryParagraph.AddLineBreak();
                }
                if (dataSource.FactoryPhoneNo != null)
                {
                    factoryParagraph.AddText(dataSource.FactoryPhoneNo);
                    factoryParagraph.AddLineBreak();
                }
                if (dataSource.FactoryContact != null)
                {
                    factoryParagraph.AddText(dataSource.FactoryContact);
                    factoryParagraph.AddLineBreak();
                }
                //factoryParagraph.AddText(dataSource.FactoryContactPhoneNo);

                rowInspectionFactory.Cells[4].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("TotalReports", ""));
                rowInspectionFactory.Cells[4].Format.Font.Bold = true;
                rowInspectionFactory.Cells[4].Format.Font.Size = 10;
                rowInspectionFactory.Cells[4].Shading.Color = Colors.LightGray; ;
                // rowInspectionFactory.Cells[4].MergeRight = 2;
                rowInspectionFactory.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionFactory.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                var result = dataSource.ProductDetails.Select(x => x.ProductName).Distinct().Count() + "/" + dataSource.TotalNumberofReports;

                rowInspectionFactory.Cells[5].AddParagraph(result);
                rowInspectionFactory.Cells[5].Format.Font.Size = 10;
                rowInspectionFactory.Cells[5].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionFactory.Cells[5].Format.Alignment = ParagraphAlignment.Center;

                rowInspectionFactory.Cells[6].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("ServiceType", ""));
                rowInspectionFactory.Cells[6].Format.Font.Bold = true;
                rowInspectionFactory.Cells[6].Format.Font.Size = 10;
                rowInspectionFactory.Cells[6].Shading.Color = Colors.LightGray; ;
                // rowInspectionFactory.Cells[4].MergeRight = 2;
                rowInspectionFactory.Cells[6].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionFactory.Cells[6].Format.Alignment = ParagraphAlignment.Center;

                rowInspectionFactory.Cells[7].AddParagraph(dataSource.ServiceType);
                rowInspectionFactory.Cells[7].Format.Font.Size = 10;
                rowInspectionFactory.Cells[7].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionFactory.Cells[7].Format.Alignment = ParagraphAlignment.Center;

                //add row for comments data
                Row rowInspectionComments = inspectionInfo.AddRow();
                rowInspectionComments.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Comments", ""));
                rowInspectionComments.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionComments.Cells[0].Format.Font.Bold = true;
                rowInspectionComments.Cells[0].Format.Font.Size = 10;
                rowInspectionComments.Cells[0].Shading.Color = Colors.LightGray;


                if (dataSource.Comments != null)
                {
                    var comments = SplitStringWithSpace(dataSource.Comments?.RemoveExtraSpace(), CommentsCharLimit);
                    var CommentsParagraph = rowInspectionComments.Cells[1].AddParagraph(comments);
                    rowInspectionComments.Cells[1].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                    rowInspectionComments.Cells[1].MergeRight = 6;
                }

                //add row for csnames data
                Row rowInspectionCsNames = inspectionInfo.AddRow();
                rowInspectionCsNames.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("CS", ""));
                rowInspectionCsNames.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowInspectionCsNames.Cells[0].Format.Font.Bold = true;
                rowInspectionCsNames.Cells[0].Format.Font.Size = 10;
                rowInspectionCsNames.Cells[0].Shading.Color = Colors.LightGray;


                if (dataSource.CsNames != null)
                {
                    var csNames = dataSource.CsNames;
                    var csNamesParagraph = rowInspectionCsNames.Cells[1].AddParagraph(csNames);
                    rowInspectionCsNames.Cells[1].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial Unicode MS", Unit.FromPoint(9));
                    rowInspectionCsNames.Cells[1].MergeRight = 6;
                }
            }

        }

        /// <summary>
        /// add non combined product table headers and main header if first page width exceeds
        /// </summary>
        /// 
        private void AddNonCombinedProductHeaders()
        {
            this.Section.AddPageBreak();
            this.AddressFrame = Section.AddTextFrame();
            this.AddressFrame.Height = "3.0cm";
            this.AddressFrame.Width = "7.0cm";
            this.AddressFrame.Left = ShapePosition.Left;
            this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.AddressFrame.Top = "3.5cm";
            //this.AddressFrame. = "4.0cm";
            this.AddressFrame.RelativeVertical = RelativeVertical.Page;
            pageRowCount = 0;
            pageNumber = 2;
            Paragraph paragraphPageBreakHeader = this.AddressFrame.AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("NonCombineProductDetails", ""));
            paragraphPageBreakHeader.Format.SpaceBefore = "1cm";
            paragraphPageBreakHeader.Format.SpaceAfter = "0.5cm";
            paragraphPageBreakHeader.Format.Font.Bold = true;
            //NonCombinedProductHeaderTable(false, dataSource);
        }

        /// <summary>
        /// add inspection product details
        /// </summary>
        /// <param name="section"></param>
        /// <param name="dataSource"></param>
        private void AddInspectionProductDetails(Section section, QCInspectionDetailsPDF dataSource)
        {
            string comments = string.Empty;
            if (dataSource.ProductDetails != null && dataSource.ProductDetails.Any())
            {
                //check if any products are non combined
                var noncombinedProducts = dataSource.ProductDetails.Where(x => x.CombineProductId == null).OrderBy(x => x.ProductName).ToList();
                if (dataSource.Comments != null)
                {
                    comments = dataSource.Comments.RemoveExtraSpace();
                }
                if (comments != string.Empty)
                {
                    //check how many rows required for the comment section
                    var commentRowCount = Math.Ceiling((double)comments.Length / commentRowCharLimit);

                    //if it requires more than 5 rows, then reduce the page row limit by the extra rows required
                    if (commentRowCount > commentRowLimit)
                    {
                        initialPageRowLimit = initialPageRowLimit - ((int)commentRowCount - commentRowLimit);
                        combineInitialPageRowLimit = combineInitialPageRowLimit - ((int)commentRowCount - commentRowLimit);
                        initialPageRowLimitValue = initialPageRowLimitValue - ((int)commentRowCount - commentRowLimit);
                        combineInitialPageRowLimitValue = combineInitialPageRowLimitValue - ((int)commentRowCount - commentRowLimit);
                    }
                    else if (commentRowCount < commentRowLimit)
                    {
                        initialPageRowLimit = initialPageRowLimit + (int)(commentRowLimit - commentRowCount);
                        combineInitialPageRowLimit = combineInitialPageRowLimit + (int)(commentRowLimit - commentRowCount);
                        initialPageRowLimitValue = initialPageRowLimitValue + (int)(commentRowLimit - commentRowCount);
                        combineInitialPageRowLimitValue = combineInitialPageRowLimitValue + (int)(commentRowLimit - commentRowCount);
                    }
                }
                else
                {
                    initialPageRowLimit = initialPageRowLimit + (commentRowLimit);
                    combineInitialPageRowLimit = combineInitialPageRowLimit + (commentRowLimit);
                    initialPageRowLimitValue = initialPageRowLimitValue + (commentRowLimit);
                    combineInitialPageRowLimitValue = combineInitialPageRowLimitValue + (commentRowLimit);
                }

                //process non combined products if it is null
                if (noncombinedProducts != null && noncombinedProducts.Any())
                {
                    this.NonCombinedProductTableData(noncombinedProducts, dataSource);
                }

                //check if products are combined
                var combinedProducts = dataSource.ProductDetails.Where(x => x.CombineProductId != null)
                                                      .OrderBy(x => x.CombineProductId).
                                                       ThenByDescending(x => x.CombinedAQLQuantity).ThenBy(x => x.ProductName).ToList();

                if (combinedProducts != null && combinedProducts.Any())
                {
                    //take combineproductids to identify the no of combined groups
                    var combineProductIds = combinedProducts.Select(x => x.CombineProductId).Distinct();
                    int combineGroup = 1;
                    //create table for each combined group
                    foreach (var combineProductId in combineProductIds)
                    {
                        var selectedCombineProducts = combinedProducts.Where(x => x.CombineProductId == combineProductId).ToList();
                        if (selectedCombineProducts != null && selectedCombineProducts.Any())
                        {
                            var firstPageWidthExceeds = false;
                            var sampleSize = selectedCombineProducts.Where(x => x.CombinedAQLQuantity > 0).Select(x => x.CombinedAQLQuantity.Value).FirstOrDefault();
                            var pickingQty = selectedCombineProducts.Sum(x => x.Picking.GetValueOrDefault());
                            //if there is no noncombined products for the inspection then we need to check the comments data length for the combined product table

                            CombinedProductTableData(selectedCombineProducts, combineGroup, sampleSize, firstPageWidthExceeds, pickingQty, dataSource.BussinessLine);
                            combineGroup = combineGroup + 1;
                        }

                    }
                }

            }
        }

        /// <summary>
        /// create page footer data
        /// </summary>
        /// <param name="section"></param>
        private void AddPageFoooter(Section section)
        {
            Paragraph paragraphFooter = section.Footers.Primary.AddParagraph("v1");
            paragraphFooter.Format.Alignment = ParagraphAlignment.Left;


            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddTab();
            paragraph.AddPageField();
            paragraph.AddText(" of ");
            paragraph.AddNumPagesField();

            //paragraph.AddText($"{DateTime.Now.ToString("dd/MM/yyyy")}");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            Section.Footers.EvenPage.Add(paragraph.Clone());
        }

        /// <summary>
        /// Create the page for the section
        /// </summary>
        /// <param name="dataSource"></param>
        private void CreatePage(QCInspectionDetailsPDF dataSource)
        {
            if (dataSource != null)
            {
                Section = this.Document.AddSection();
                Section.PageSetup = new PageSetup() { RightMargin = "1cm", LeftMargin = "1cm", Orientation = Orientation.Landscape, PageFormat = PageFormat.Letter };

                var imageLogo = dataSource.EntityMasterConfigs.Where(x => x.Type == (int)EntityConfigMaster.ImageLogo).Select(x => x.Value).FirstOrDefault();

                AddPageHeader(Section, imageLogo);

                AddInspectionDetailData(Section, dataSource);

                AddInspectionProductDetails(Section, dataSource);

                AddPageFoooter(Section);
            }

        }

        /// <summary>
        /// add NonCombine Products headers
        /// </summary>
        private void NonCombinedProductHeaderTable(bool headerAdded, QCInspectionDetailsPDF dataSource)
        {
            if (!headerAdded)
            {
                var table = this.AddressFrame.AddTable();
                table.Format.SpaceBefore = "0.5cm";
                table.Format.SpaceAfter = "0.5cm";
                table.AddColumn("10cm");
                table.AddColumn("10cm");
                table.AddColumn("6cm");

                var rowHeader = table.AddRow();

                rowHeader.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("NonCombineProductDetails", ""));
                rowHeader.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowHeader.Cells[0].Format.Font.Bold = true;
                rowHeader.Cells[0].Format.Font.Size = 10;

                rowHeader.Cells[1].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("TotalSamplingSize", "") + dataSource.TotalSamplingSizeNonCombined);
                rowHeader.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                rowHeader.Cells[1].Format.Font.Bold = true;
                rowHeader.Cells[1].Format.Font.Size = 10;

                rowHeader.Cells[2].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("TotalPickingQty", "") + dataSource.TotalPickingQtyNoncombined);
                rowHeader.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                rowHeader.Cells[2].Format.Font.Bold = true;
                rowHeader.Cells[2].Format.Font.Size = 10;
            }
            //add the table for non combined product
            this.ProductTable = this.AddressFrame.AddTable();
            this.ProductTable.Style = "Table";
            this.ProductTable.Borders.Color = TableBorder;
            this.ProductTable.Rows.LeftIndent = 0;
            //add the columns with the specific width
            this.ProductTable.AddColumn("0.75cm"); //1-No
            this.ProductTable.AddColumn("3.4cm"); //2-Prod Name
            this.ProductTable.AddColumn("3.4cm"); //3-Description
            this.ProductTable.AddColumn("3.4cm"); //4-Sub/Sub2
            this.ProductTable.AddColumn("3.4cm"); //5-PO No
            this.ProductTable.AddColumn("1cm");  //6-DST
            this.ProductTable.AddColumn("1.2cm"); //7-Qty
            this.ProductTable.AddColumn("0.75cm"); //1.5  8-GIL
            this.ProductTable.AddColumn("1cm"); //2.5    9-SS
            this.ProductTable.AddColumn("1cm"); //1.5    10-Pick or color.            
            this.ProductTable.AddColumn("3.4cm"); //1.6 12-Remarks
            this.ProductTable.AddColumn("1.3cm"); //1.7 13-
            this.ProductTable.AddColumn("1cm"); //1.7   14-
            this.ProductTable.AddColumn("1cm"); //1.7                                                 
            //add the row for the header
            Row row = this.ProductTable.AddRow();
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            row.HeightRule = RowHeightRule.Exactly;
            row.Height = 20;

            row.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SerialNumber", ""));
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[1].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("ProductName", ""));
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[2].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("ProductDescription", ""));
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[3].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Category", ""));
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[4].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("PoNumber", ""));
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[5].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("DestinationCountry", ""));
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[6].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Qty", ""));
            row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[6].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[7].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("AQL", ""));
            row.Cells[7].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[7].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[8].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SampleSize", ""));
            row.Cells[8].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[8].Format.Alignment = ParagraphAlignment.Center;

            if (dataSource.BussinessLine == (int)BusinessLine.SoftLine)
            {
                row.Cells[9].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Color", ""));
                row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
            }
            else if (dataSource.BussinessLine == (int)BusinessLine.HardLine)
            {
                row.Cells[9].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Picking", ""));
                row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
            }


            row.Cells[10].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Remarks", ""));
            row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[10].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[11].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Pcsctn", ""));
            row.Cells[11].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[11].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[12].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("TotalCtn", ""));
            row.Cells[12].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[12].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[13].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SelectCtn", ""));
            row.Cells[13].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[13].Format.Alignment = ParagraphAlignment.Center;
        }

        /// <summary>
        /// add  non combined product header data
        /// </summary>
        /// <param name="productDetails"></param>
        private void NonCombinedProductTableData(List<QCInspectionProductDetails> productDetails, QCInspectionDetailsPDF dataSource)
        {
            pageRowCount = 3; //including the header
            int serialNumber = 0;
            bool headerAdded = false;

            var lastProduct = string.Empty;

            var nonCombinedProducts = productDetails.Where(x => !(x.CombineProductId > 0)).ToList();
            var nonCombinedRowCount = 0;
            var totalTextLength = 0;

            foreach (var product in nonCombinedProducts)
            {
                var prodCat_Sub_Sub2 = product.ProdSubCategory + " / " + product.ProdSub2Category;
                product.ProductRemarks = string.IsNullOrEmpty(product.ProductRemarks) ? product.ProductRemarks : product.ProductRemarks?.RemoveExtraSpace();
                product.PickingRemarks = string.IsNullOrEmpty(product.PickingRemarks) ? product.PickingRemarks : product.PickingRemarks?.RemoveExtraSpace();

                var productRemarksLength = string.IsNullOrEmpty(product.ProductRemarks) ? 0 : product.ProductRemarks.Length + maxCharInRow;
                var pickingRemarksLength = string.IsNullOrEmpty(product.PickingRemarks) ? 0 : product.PickingRemarks.Length + maxCharInRow;

                var remarks = productRemarksLength + pickingRemarksLength + (product.IsEcopack.GetValueOrDefault() ? maxCharInRow : 0);
                product.ProductDescription = string.IsNullOrEmpty(product.ProductDescription) ? product.ProductDescription : product.ProductDescription?.RemoveExtraSpace();
                product.PoNumber = string.IsNullOrEmpty(product.PoNumber) ? product.PoNumber : product.PoNumber?.RemoveExtraSpace();
                product.ProductName = string.IsNullOrEmpty(product.ProductName) ? product.ProductName : product.ProductName?.RemoveExtraSpace();

                // Added serial number as first column
                if (lastProduct == string.Empty || lastProduct != product.ProductName)
                    serialNumber = serialNumber + 1;

                //if last prodcut name and product name is same that time we are remove productname
                if ((lastProduct != string.Empty) && lastProduct == product.ProductName)
                {
                    product.ProductDescription = "";
                    product.ProductName = "";
                    productRemarksLength = 0;
                    remarks = productRemarksLength + pickingRemarksLength + (product.IsEcopack.GetValueOrDefault() ? maxCharInRow : 0);
                }
                else
                {
                    //if product name is different then set lastProductname
                    lastProduct = product.ProductName;
                }

                //get the max length by comparing desc, category and remarks
                totalTextLength = CalculatePageSize(prodCat_Sub_Sub2.Length, remarks, product.ProductDescription.Length, product.PoNumber.Length, product.ProductName.Length);

                //divide the maxlength by max char in a row to find the number of extra rows needed for the product
                var average = Math.Ceiling((double)totalTextLength / maxCharInRow);

                if (!headerAdded && pageNumber == 1 && pageRowCount + average <= initialPageRowLimitValue)
                {
                    NonCombinedProductHeaderTable(headerAdded, dataSource);
                    headerAdded = true;
                }

                //break the page if row exceeds more than rowlimit
                //check if pagerowcount is greater than the total rows allowed and initialPageRowLimit < average --  if there is space for the new row (average)
                else if ((pageNumber == 1 && (pageRowCount + average > initialPageRowLimitValue || (initialPageRowLimit - average) < average)) || (pageNumber > 1 && (nonCombinedRowCount + average > nonCombinedOtherPageLimit || otherPageRowLimitNonCombined < average)))
                {
                    this.Section.AddPageBreak();

                    this.AddressFrame = Section.AddTextFrame();
                    this.AddressFrame.Height = "3.0cm";
                    this.AddressFrame.Width = "7.0cm";
                    this.AddressFrame.Left = ShapePosition.Left;
                    this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    this.AddressFrame.RelativeVertical = RelativeVertical.Page;
                    pageRowCount = 0;
                    pageNumber++;

                    if (!headerAdded && pageNumber > 1)
                        this.AddressFrame.Top = "3cm";
                    else
                    {
                        this.AddressFrame.Top = "3.5cm";
                    }
                    nonCombinedRowCount = 0;
                    otherPageRowLimitNonCombined = nonCombinedOtherPageLimit;
                    NonCombinedProductHeaderTable(headerAdded, dataSource);
                }

                //remove the current product from the total number of rows allowed in the first page
                if (pageNumber == 1 && totalTextLength > maxCharInRow)
                {
                    initialPageRowLimit = initialPageRowLimit - (int)average;
                }

                //remove the current product from the total number of rows allowed in the other pages
                else if (pageNumber > 1 && totalTextLength > maxCharInRow)
                {
                    otherPageRowLimitNonCombined = otherPageRowLimitNonCombined - (int)average;
                }

                pageRowCount = pageRowCount + (int)average;
                nonCombinedRowCount = nonCombinedRowCount + (int)average;

                var row = this.ProductTable.AddRow();
                row.Height = 20;

           

                row.Cells[0].AddParagraph($"{serialNumber}");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                if (product.ProdCategory != null)
                {
                    prodCat_Sub_Sub2 = myTI.ToTitleCase(myTI.ToLower(prodCat_Sub_Sub2));
                    var category = prodCat_Sub_Sub2.Length > maxCharLength ? SplitStringWithSpace(prodCat_Sub_Sub2, maxCharLength) : prodCat_Sub_Sub2;
                    row.Cells[3].AddParagraph($"{category}");
                    row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                    row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
                }

                if (product.PoNumber != null)
                {
                    product.PoNumber = myTI.ToTitleCase(myTI.ToLower(product.PoNumber));
                    var poNumber = product.PoNumber.Length > maxCharLength ? SplitStringWithSpace(product.PoNumber, maxCharLength) : product.PoNumber;
                    row.Cells[4].AddParagraph($"{poNumber}");
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                }

                if (product.DestinationCountry != null)
                {
                    var destinationCountry = product.DestinationCountry.Length > 11 ? SplitStringWithSpace(product.DestinationCountry, 11) : product.DestinationCountry;
                    row.Cells[5].AddParagraph($"{destinationCountry}");
                    row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
                }

                var bookingqty = product.BookingQty.ToString().Length > 4 ? SplitStringWithSpace(product.BookingQty.ToString(), 4) : product.BookingQty.ToString();
                row.Cells[6].AddParagraph($"{bookingqty}");
                row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[6].VerticalAlignment = VerticalAlignment.Center;

                if (dataSource.BussinessLine == (int)BusinessLine.HardLine)
                {
                    row.Cells[9].AddParagraph($"{product.Picking}");
                    row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                }
                else if (dataSource.BussinessLine == (int)BusinessLine.SoftLine)
                {
                    row.Cells[9].AddParagraph($"{product.Color}");
                    row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                }


                //var productCount = productDetails.Where(x => x.ProductName == product.ProductName).Count();
                var firstItem = productDetails.Where(x => x.ProductName == product.ProductName).FirstOrDefault();

                //if it is same product with differe quantity, that time we are showing only one time 
                if (firstItem != null && firstItem.PoNumber == product.PoNumber && !string.IsNullOrEmpty(product.ProductName)
                    && product.ProductName == firstItem.ProductName)
                {
                    if (product.ProductName != null)
                    {
                        var productname = product.ProductName?.RemoveExtraSpace();
                        productname = myTI.ToTitleCase(myTI.ToLower(productname));
                        productname = productname.Length > 17 ? SplitStringWithSpace(productname, 17) : productname;
                        row.Cells[1].AddParagraph($"{productname}");
                        row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (product.ProductDescription != null)
                    {
                        var productDesc = product.ProductDescription?.RemoveExtraSpace();
                        productDesc = myTI.ToTitleCase(myTI.ToLower(productDesc));
                        productDesc = productDesc.Length > maxCharLength ? SplitStringWithSpace(productDesc, maxCharLength) : productDesc;
                        row.Cells[2].AddParagraph($"{productDesc}");
                        row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[2].VerticalAlignment = VerticalAlignment.Center;

                    }

                    if (product.AQL != null)
                    {
                        row.Cells[7].AddParagraph($"{product.AQL}");
                        row.Cells[7].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[7].VerticalAlignment = VerticalAlignment.Center;

                    }

                    row.Cells[8].AddParagraph($"{product.AQLQuantity}");
                    row.Cells[8].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[8].VerticalAlignment = VerticalAlignment.Center;


                    if (!string.IsNullOrWhiteSpace(product.PickingRemarks))
                    {
                        var pickingRemarks = row.Cells[10].AddParagraph();
                        pickingRemarks.AddFormattedText("Picking : ", TextFormat.Bold);
                        product.PickingRemarks = myTI.ToTitleCase(myTI.ToLower(product.PickingRemarks));
                        var trimmedRemarks = product.PickingRemarks.Length > maxCharLength ? SplitStringWithSpace(product.PickingRemarks, maxCharLength) : product.PickingRemarks;
                        row.Cells[10].AddParagraph($"{trimmedRemarks}");
                        row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (!string.IsNullOrWhiteSpace(product.ProductRemarks))
                    {
                        var productRemarks = row.Cells[10].AddParagraph();
                        productRemarks.AddFormattedText("Product : ", TextFormat.Bold);
                        product.ProductRemarks = myTI.ToTitleCase(myTI.ToLower(product.ProductRemarks));
                        var trimmedRemarks = product.ProductRemarks.Length > maxCharLength ? SplitStringWithSpace(product.ProductRemarks, maxCharLength) : product.ProductRemarks;
                        row.Cells[10].AddParagraph($"{trimmedRemarks}");
                        row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (product.IsEcopack.GetValueOrDefault())
                    {
                        var productRemarks = row.Cells[10].AddParagraph();
                        productRemarks.AddFormattedText(Ecopack, TextFormat.Bold);
                        productRemarks.AddFormattedText("Yes");
                    }

                }
                else // picking qty showing per po.
                {
                    if (!string.IsNullOrWhiteSpace(product.PickingRemarks))
                    {
                        var pickingRemarks = row.Cells[10].AddParagraph();
                        pickingRemarks.AddFormattedText("Picking : ", TextFormat.Bold);
                        product.PickingRemarks = myTI.ToTitleCase(myTI.ToLower(product.PickingRemarks));
                        var trimmedRemarks = product.PickingRemarks.Length > maxCharLength ? SplitStringWithSpace(product.PickingRemarks, maxCharLength) : product.PickingRemarks;
                        row.Cells[10].AddParagraph($"{trimmedRemarks}");
                        row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (product.IsEcopack.GetValueOrDefault())
                    {
                        var productRemarks = row.Cells[10].AddParagraph();
                        productRemarks.AddFormattedText(Ecopack, TextFormat.Bold);
                        productRemarks.AddFormattedText("Yes");
                    }
                }

                row.Cells[11].AddParagraph();
                row.Cells[11].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[11].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[12].AddParagraph();
                row.Cells[12].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[12].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[13].AddParagraph();
                row.Cells[13].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[13].VerticalAlignment = VerticalAlignment.Center;                
            }
        }

        /// <summary>
        /// add combined product header table
        /// </summary>
        /// <param name="combineGroup"></param>
        /// <param name="sampleSize"></param>
        /// <param name="isGroupHeaderAdded"></param>
        private void CombinedProductHeaderTable(int combineGroup, int sampleSize, bool isGroupHeaderAdded, int pickingQty, int? businessLine)
        {
            if (!isGroupHeaderAdded)
            {
                var table = this.AddressFrame.AddTable();

                table.Format.SpaceBefore = "0.5cm";
                table.Format.SpaceAfter = "0.5cm";
                table.AddColumn("10cm");
                table.AddColumn("10cm");
                table.AddColumn("6cm");

                var rowCombined = table.AddRow();

                rowCombined.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("CombinedGroup", "") + combineGroup + ":");
                rowCombined.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowCombined.Cells[0].Format.Font.Bold = true;
                rowCombined.Cells[0].Format.Font.Size = 10;

                rowCombined.Cells[1].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SamplingSize", "") + sampleSize);
                rowCombined.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                rowCombined.Cells[1].Format.Font.Bold = true;
                rowCombined.Cells[1].Format.Font.Size = 10;

                rowCombined.Cells[2].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("PickingQty", "") + pickingQty);
                rowCombined.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                rowCombined.Cells[2].Format.Font.Bold = true;
                rowCombined.Cells[2].Format.Font.Size = 10;
            }

            this.ProductTable = this.AddressFrame.AddTable();
            this.ProductTable.Style = "Table";
            this.ProductTable.Borders.Color = TableBorder;
            this.ProductTable.Rows.LeftIndent = 0;

            this.ProductTable.AddColumn("0.75cm"); //1-No
            this.ProductTable.AddColumn("3.4cm"); //2-Prod Name
            this.ProductTable.AddColumn("3.4cm"); //3-Description
            this.ProductTable.AddColumn("3.4cm"); //4-Sub/Sub2
            this.ProductTable.AddColumn("3.4cm"); //5-PO No
            this.ProductTable.AddColumn("1cm"); //6-DST
            this.ProductTable.AddColumn("1.2cm"); //7-Qty
            this.ProductTable.AddColumn("0.75cm"); //1.5 8-GIL
            this.ProductTable.AddColumn("1cm"); //2.5 9-SS
            this.ProductTable.AddColumn("1cm"); //1.5 10-Color or picking            
            this.ProductTable.AddColumn("3.4cm"); //1.6 11-Remarks
            this.ProductTable.AddColumn("1.3cm"); //1.7  12
            this.ProductTable.AddColumn("1cm"); //1.7  13-Total Ctn
            this.ProductTable.AddColumn("1cm"); //1.7  14-

            Row row = this.ProductTable.AddRow();
            row.Height = 20;
            //row.HeadingFormat = true;
            //row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightGray;
            //row.HeightRule = RowHeightRule.Exactly;
            //row.Height = 20;


            var serialNumber = row.Cells[0].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SerialNumber", ""));
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[1].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("ProductName", ""));
            row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[2].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("ProductDescription", ""));
            row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[3].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Category", ""));
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;

            var productId = row.Cells[4].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("PoNumber", ""));
            row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[5].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("DestinationCountry", ""));
            row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[6].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Qty", ""));
            row.Cells[6].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[6].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[7].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("AQL", ""));
            row.Cells[7].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[7].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[8].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SampleSize", ""));
            row.Cells[8].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[8].Format.Alignment = ParagraphAlignment.Center;

            if (businessLine == (int)BusinessLine.SoftLine)
            {
                row.Cells[9].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Color", ""));
                row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
            }
            else if (businessLine == (int)BusinessLine.HardLine)
            {
                row.Cells[9].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Picking", ""));
                row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
            }

            row.Cells[10].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Remarks", ""));
            row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[10].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[11].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("Pcsctn", ""));
            row.Cells[11].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[11].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[12].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("TotalCtn", ""));
            row.Cells[12].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[12].Format.Alignment = ParagraphAlignment.Center;

            row.Cells[13].AddParagraph(QCInspectionDetailsPDF.GetValueOrDefault("SelectCtn", ""));
            row.Cells[13].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[13].Format.Alignment = ParagraphAlignment.Center;

        }

        /// <summary>
        /// add combined product table data
        /// </summary>
        /// <param name="productDetails"></param>
        /// <param name="combineGroup"></param>
        /// <param name="sampleSize"></param>
        /// <param name="firstPageWidthExceeds"></param>
        private void CombinedProductTableData(List<QCInspectionProductDetails> productDetails, int combineGroup, int sampleSize, bool firstPageWidthExceeds, int pickingQty, int? busineeLine)
        {
            //isGroupHeaderAdded-combinegroup and sample size should not be repeated for each group
            bool isGroupHeaderAdded = false;

            //5 denotes the number of rows we need to insert 1 row of combined data with header (header, column header, row data). so if there is already non combined data, then we should have enought space for at least 1 row of combined data
            if (pageRowCount > 0)
            {
                pageRowCount = pageRowCount + 3;
            }

            //if comments data exceeds and non combined products are not there then combined table starts from page2
            if (pageNumber == 1 && firstPageWidthExceeds)
            {
                this.Section.AddPageBreak();
                this.AddressFrame = Section.AddTextFrame();
                this.AddressFrame.Height = "3.0cm";
                this.AddressFrame.Width = "7.0cm";
                this.AddressFrame.Left = ShapePosition.Left;
                this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                this.AddressFrame.Top = "3.5cm";
                this.AddressFrame.RelativeVertical = RelativeVertical.Page;
                CombinedProductHeaderTable(combineGroup, sampleSize, isGroupHeaderAdded, pickingQty, busineeLine);
                pageRowCount = 2;
                pageNumber++;
                otherPageRowLimit = combinedOtherPageLimit;
            }

            int serialNumber = 0;
            var lastProductName = string.Empty;
            int totalTextLength = 0;
            bool isMainHeaderAdded = false;

            foreach (var product in productDetails)
            {
                //pageRowCount++;

                var prodCat_Sub_Sub2 = product.ProdSubCategory + " / " + product.ProdSub2Category;
                product.ProductRemarks = string.IsNullOrEmpty(product.ProductRemarks) ? product.ProductRemarks : product.ProductRemarks?.RemoveExtraSpace();
                product.PickingRemarks = string.IsNullOrEmpty(product.PickingRemarks) ? product.PickingRemarks : product.PickingRemarks?.RemoveExtraSpace();

                var productRemarksLength = string.IsNullOrEmpty(product.ProductRemarks) ? 0 : product.ProductRemarks.Length + maxCharInRow;
                var pickingRemarksLength = string.IsNullOrEmpty(product.PickingRemarks) ? 0 : product.PickingRemarks.Length + maxCharInRow;

                var remarks = productRemarksLength + pickingRemarksLength + (product.IsEcopack.GetValueOrDefault() ? maxCharInRow : 0);
                product.ProductDescription = string.IsNullOrEmpty(product.ProductDescription) ? product.ProductDescription : product.ProductDescription?.RemoveExtraSpace();
                product.PoNumber = string.IsNullOrEmpty(product.PoNumber) ? product.PoNumber : product.PoNumber?.RemoveExtraSpace();
                product.ProductName = string.IsNullOrEmpty(product.ProductName) ? product.ProductName : product.ProductName?.RemoveExtraSpace();


                // Added serial number as first column
                if (lastProductName == string.Empty || lastProductName != product.ProductName)
                    serialNumber = serialNumber + 1;

                if ((lastProductName != string.Empty) && lastProductName == product.ProductName)
                {
                    product.ProductDescription = "";
                    product.ProductName = "";
                    productRemarksLength = 0;
                    remarks = productRemarksLength + pickingRemarksLength + (product.IsEcopack.GetValueOrDefault() ? maxCharInRow : 0);
                }
                else
                {
                    //if product name is different then set lastProductname
                    lastProductName = product.ProductName;
                }

                //get the max length by comparing desc, category and remarks
                totalTextLength = CalculatePageSize(prodCat_Sub_Sub2.Length, remarks, product.ProductDescription.Length, product.PoNumber.Length, product.ProductName.Length);

                //divide the maxlength by max char in a row to find the number of extra rows needed for the product
                var average = Math.Ceiling((double)totalTextLength / maxCharInRow);

                if (!isMainHeaderAdded && ((pageNumber == 1 && pageRowCount + average <= combineInitialPageRowLimitValue) || (pageNumber > 1 && pageRowCount + average <= combinedOtherPageLimit)))
                {
                    CombinedProductHeaderTable(combineGroup, sampleSize, isGroupHeaderAdded, pickingQty, busineeLine);
                    isGroupHeaderAdded = true;
                    isMainHeaderAdded = true;
                }

                //insert the header only if there is space - pageRowCount is less than the allowed count in a page
                //pageRowCount + average -- check if we have enough space for the header and first row, (initialPageRowLimit - average) < average -- check if remaining rows in the page minus current row count is less than current row then add the header
                else if ((pageNumber == 1 && (pageRowCount + average >= combineInitialPageRowLimitValue || (combineInitialPageRowLimit - average) < average)) || (pageNumber > 1 && (pageRowCount + average > combinedOtherPageLimit || otherPageRowLimit < average)))
                {
                    isGroupHeaderAdded = isMainHeaderAdded;
                    this.Section.AddPageBreak();
                    this.AddressFrame = Section.AddTextFrame();
                    this.AddressFrame.Height = "3.0cm";
                    this.AddressFrame.Width = "7.0cm";
                    this.AddressFrame.Left = ShapePosition.Left;
                    this.AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
                    if (isGroupHeaderAdded) this.AddressFrame.Top = "3.5cm";
                    else
                    {
                        this.AddressFrame.Top = "3cm";
                    }
                    this.AddressFrame.RelativeVertical = RelativeVertical.Page;
                    pageRowCount = 2;
                    pageNumber++;
                    otherPageRowLimit = combinedOtherPageLimit;
                    CombinedProductHeaderTable(combineGroup, sampleSize, isGroupHeaderAdded, pickingQty, busineeLine);
                    isMainHeaderAdded = true;
                }
                //remove the current product rows from the total number of rows allowed in the first page
                if (pageNumber == 1 && totalTextLength > maxCharInRow)
                {
                    combineInitialPageRowLimit = combineInitialPageRowLimit - (int)average; //average - 1 because we have already considered 1 row for the product
                }

                //remove the current product from the total number of rows allowed in the remaining pages
                else if (pageNumber > 1 && totalTextLength > maxCharInRow)
                {
                    otherPageRowLimit = otherPageRowLimit - (int)average; //average - 1 because we have already considered 1 row for the product
                }

                pageRowCount = pageRowCount + (int)average;

                var row = this.ProductTable.AddRow();
                row.Height = 20;


              
                row.Cells[0].AddParagraph($"{serialNumber}");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                if (product.ProdCategory != null)
                {
                    prodCat_Sub_Sub2 = myTI.ToTitleCase(myTI.ToLower(prodCat_Sub_Sub2));
                    var category = prodCat_Sub_Sub2.Length > maxCharLength ? SplitStringWithSpace(prodCat_Sub_Sub2, maxCharLength) : prodCat_Sub_Sub2;
                    row.Cells[3].AddParagraph($"{category}");
                    row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
                    row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
                }

                if (product.PoNumber != null)
                {
                    product.PoNumber = myTI.ToTitleCase(myTI.ToLower(product.PoNumber));
                    var poNumber = product.PoNumber.Length > maxCharLength ? SplitStringWithSpace(product.PoNumber, maxCharLength) : product.PoNumber;
                    row.Cells[4].AddParagraph($"{poNumber}");
                    row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[4].VerticalAlignment = VerticalAlignment.Center;
                }

                if (product.DestinationCountry != null)
                {
                    var destinationCountry = product.DestinationCountry.Length > 11 ? SplitStringWithSpace(product.DestinationCountry, 11) : product.DestinationCountry;
                    row.Cells[5].AddParagraph($"{destinationCountry}");
                    row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[5].VerticalAlignment = VerticalAlignment.Center;
                }

                var bookingqty = product.BookingQty.ToString().Length > 4 ? SplitStringWithSpace(product.BookingQty.ToString(), 4) : product.BookingQty.ToString();
                row.Cells[6].AddParagraph($"{bookingqty}");
                row.Cells[6].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[6].VerticalAlignment = VerticalAlignment.Center;

                if (busineeLine == (int)BusinessLine.HardLine)
                {
                    row.Cells[9].AddParagraph($"{product.Picking}");
                    row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                }
                else if (busineeLine == (int)BusinessLine.SoftLine)
                {
                    row.Cells[9].AddParagraph($"{product.Color}");
                    row.Cells[9].Format.Alignment = ParagraphAlignment.Center;
                    row.Cells[9].VerticalAlignment = VerticalAlignment.Center;
                }

                // var productCount = productDetails.Where(x => x.ProductName == product.ProductName).Count();

                var firstItem = productDetails.Where(x => x.ProductName == product.ProductName).FirstOrDefault();

                if (firstItem != null && firstItem.PoNumber == product.PoNumber
                    && product.ProductName == firstItem.ProductName)
                {
                    if (product.ProductName != null)
                    {
                        var productname = product.ProductName?.RemoveExtraSpace();
                        productname = myTI.ToTitleCase(myTI.ToLower(productname));
                        productname = productname.Length > 17 ? SplitStringWithSpace(productname, 17) : productname;
                        row.Cells[1].AddParagraph($"{productname}");
                        row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (product.ProductDescription != null)
                    {
                        var productDesc = product.ProductDescription?.RemoveExtraSpace();
                        productDesc = myTI.ToTitleCase(myTI.ToLower(productDesc));
                        productDesc = productDesc.Length > maxCharLength ? SplitStringWithSpace(productDesc, maxCharLength) : productDesc;
                        row.Cells[2].AddParagraph($"{productDesc}");
                        row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[2].VerticalAlignment = VerticalAlignment.Center;
                    }

                    if (product.AQL != null)
                    {
                        row.Cells[7].AddParagraph();
                        row.Cells[7].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[7].VerticalAlignment = VerticalAlignment.Center;
                    }

                    if (product.CombinedAQLQuantity != null)
                    {
                        row.Cells[8].AddParagraph($"{product.CombinedAQLQuantity}");
                        row.Cells[8].Format.Alignment = ParagraphAlignment.Center;
                        row.Cells[8].VerticalAlignment = VerticalAlignment.Center;
                    }


                    if (!string.IsNullOrWhiteSpace(product.PickingRemarks))
                    {

                        var pickingRemarks = row.Cells[10].AddParagraph();
                        pickingRemarks.AddFormattedText("Picking : ", TextFormat.Bold);
                        product.PickingRemarks = myTI.ToTitleCase(myTI.ToLower(product.PickingRemarks));
                        var trimmedRemarks = product.PickingRemarks.Length > maxCharLength ? SplitStringWithSpace(product.PickingRemarks, maxCharLength) : product.PickingRemarks;
                        row.Cells[10].AddParagraph($"{trimmedRemarks}");
                        row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (!string.IsNullOrWhiteSpace(product.ProductRemarks))
                    {
                        var productRemarks = row.Cells[10].AddParagraph();
                        productRemarks.AddFormattedText("Product : ", TextFormat.Bold);
                        product.ProductRemarks = myTI.ToTitleCase(myTI.ToLower(product.ProductRemarks));
                        var trimmedRemarks = product.ProductRemarks.Length > maxCharLength ? SplitStringWithSpace(product.ProductRemarks, maxCharLength) : product.ProductRemarks;
                        row.Cells[10].AddParagraph($"{trimmedRemarks}");
                        row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (product.IsEcopack.GetValueOrDefault())
                    {
                        var productRemarks = row.Cells[10].AddParagraph();
                        productRemarks.AddFormattedText(Ecopack, TextFormat.Bold);
                        productRemarks.AddFormattedText("Yes");
                    }
                }
                else//picking info per po.
                {
                    if (!string.IsNullOrWhiteSpace(product.PickingRemarks))
                    {
                        var pickingRemarks = row.Cells[10].AddParagraph();
                        pickingRemarks.AddFormattedText("Picking : ", TextFormat.Bold);
                        product.PickingRemarks = myTI.ToTitleCase(myTI.ToLower(product.PickingRemarks));
                        var trimmedRemarks = product.PickingRemarks.Length > maxCharLength ? SplitStringWithSpace(product.PickingRemarks, maxCharLength) : product.PickingRemarks;
                        row.Cells[10].AddParagraph($"{trimmedRemarks}");
                        row.Cells[10].Format.Alignment = ParagraphAlignment.Left;
                        row.Cells[10].VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (product.IsEcopack.GetValueOrDefault())
                    {
                        var productRemarks = row.Cells[10].AddParagraph();
                        productRemarks.AddFormattedText(Ecopack, TextFormat.Bold);
                        productRemarks.AddFormattedText("Yes");
                    }

                }

                row.Cells[11].AddParagraph();
                row.Cells[11].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[11].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[12].AddParagraph();
                row.Cells[12].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[12].VerticalAlignment = VerticalAlignment.Center;

                row.Cells[13].AddParagraph();
                row.Cells[13].Format.Alignment = ParagraphAlignment.Center;
                row.Cells[13].VerticalAlignment = VerticalAlignment.Center;

               
            }
        }

        public int CalculatePageSize(int productCategory, int remarks, int productDescription, int poNumber, int productName)
        {
            var totalTextLength = 0;



            List<int> list = new List<int> { productCategory, remarks, productDescription, poNumber, productName };
            totalTextLength = list.Max();

            return totalTextLength;
        }

    }
}

